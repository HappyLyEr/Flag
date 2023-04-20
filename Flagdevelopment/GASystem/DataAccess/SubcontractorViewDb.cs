using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using openwfe.workitem;
//using WorkflowStarter = GASystem.BusinessLayer.WorkflowStarter;
using GASystem.DataAccess.Utils;
using System.Collections.Generic;
using System.Text;

namespace GASystem.DataAccess
{
    public class SubcontractorViewDb
    {
        // Tor 20170815 Get sql sentence from GALists SYS private static string _selectSql = "select d.* from GASubcontractorView d ";
        //private static string nextRevisionStarted = "NextRevisionStarted";
        //Approved
        //NextRevisionStarted
        //FrstRevisionNotStarted

        //private static string SubcontractorViewWorkflowStatus = "DS";
        public SubcontractorViewDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public static SubcontractorViewDS GetAllSubcontractorViewsToStart(string myDataClass)
        {
            return GetAllSubcontractorViewsToStart(myDataClass, null);
        }

        public static SubcontractorViewDS GetAllSubcontractorViewsToStart(string myDataClass,GADataTransaction transaction)
        {
            // get all SubcontractorView records where 
            // workflowstatus not = NextAuditStarted (IntFree2 DCDS)
            // and VendorType not = Materials (IntFree1 VndT)
            // and Status = Active (StatusListsRowId SCSt)
            // and Risk Consequence is valid
            // and Risk Probability is valid
            // and nextAuditDate - 1 month > current data (DateNextAudit)
            // Tor 20170815 Get SQL sentence from GALists SYS String appendSql = @" 
//inner join GALists IntFree2 on IntFree2.ListsRowId=d.IntFree2 and IntFree2.GAListValue != 'AuditInProgress' and IntFree2.GAListCategory = 'ScWS' 
///* inner join GALists CategoryLevelListsRowId on CategoryLevelListsRowId.ListsRowId=d.CategoryLevelListsRowId and CategoryLevelListsRowId.GAListValue in ('1Ma','2Mo') and CategoryLevelListsRowId.GAListCategory = 'VC' */
//inner join GALists StatusListsRowId on StatusListsRowId.ListsRowId=d.StatusListsRowId and StatusListsRowId.GAListValue = 'Active' and StatusListsRowId.GAListCategory = 'SCSt'
//inner join GALists rskC on TypeOfIncidentListsRowId=rskC.ListsRowId and rskC.GAListCategory='rskC'
//inner join GALists rskP on EyeWitness=rskP.ListsRowId and rskP.GAListCategory='rskP'
//where d.DateLastAudit is not null or d.DateNextAudit is not null
//";

            SubcontractorViewDS ds = new SubcontractorViewDS();
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            // Tor 20170815 Get SQL sentence from GALists SYS SqlDataAdapter da = new SqlDataAdapter(_selectSql + appendSql , myConnection);
            SqlDataAdapter da = new SqlDataAdapter(WSSubCntrctrSQL, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;
            //            da.SelectCommand.Parameters.AddWithValue("@nextRevisionStarted", nextRevisionStarted);
            // NextRevisionDate > today
            // AutoWorkflowStartPriorToNextRevisionListsRowId 
            // LastRevisionWorkflowClosedDate<today
            // LastRevisionStartedDate<today
            // DocumentRevisionStatusListsRowId = completed or not started

            da.Fill(ds, GADataClass.GASubcontractorView.ToString());
            return ds;
        }

        public static int ComputeClassificationMatrixValue(int RowId)
        {
            String sql = @" 
select rskC.IntFree1*rskP.IntFree1 
from GASubcontractorView 
inner join GALists rskC on TypeOfIncidentListsRowId=rskC.ListsRowId and rskC.GAListCategory='rskC' 
inner join GALists rskP on EyeWitness=rskP.ListsRowId and rskP.GAListCategory='rskP' 
where SubcontractorViewRowId=" + RowId;

            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            int ClassificationMatrixValue = -1;
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(sql, myConnection);
                ClassificationMatrixValue = (int)myCommand.ExecuteScalar();
            }
            catch
            { }
            finally
            {
                myConnection.Close();
            }
            return ClassificationMatrixValue;
        }

        public static int ComputeCategoryLevelListsRowId(int RowId)
        {
            String sql = @" 
select VC.ListsRowId from GASubcontractorView 
inner join GALists rskC on TypeOfIncidentListsRowId=rskC.ListsRowId and rskC.GAListCategory='rskC'
inner join GALists rskP on EyeWitness=rskP.ListsRowId and rskP.GAListCategory='rskP'
inner join GALists VC on VC.GAListCategory='VC' and (rskC.IntFree1*rskP.IntFree1) 
between VC.IntFree1 and VC.IntFree2 
where SubcontractorViewRowId=" + RowId;
            int CategoryLevelListsRowId=-1;
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(sql, myConnection);
                CategoryLevelListsRowId = (int)myCommand.ExecuteScalar();
            }
            catch
            { }
            finally
            {
                myConnection.Close();
            }
            return CategoryLevelListsRowId;
        }

        // Tor 20170815 Get SQL sentence from GALists SYS
        public static string WSSubCntrctrSQL
        {
            get
            {
                //return "ga"; 
                // Tor 20160308 return System.Configuration.ConfigurationSettings.AppSettings.Get("CoordinatorWorkitemGivenName");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("WSSubCntrctrSQL");
            }
        }

    }
}
