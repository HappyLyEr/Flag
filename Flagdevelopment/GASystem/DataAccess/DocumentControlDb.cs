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
    public class DocumentControlDb
    {
        // Tor 20170815 private static string _selectSql = "select d.* from GADocumentControl d ";
        // Tor 20170815 private static string nextRevisionStarted = "NextRevisionStarted";
                                                    //Approved
                                                    //NextRevisionStarted
                                                    //FrstRevisionNotStarted

        //private static string documentControlWorkflowStatus = "DS";

        public DocumentControlDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        public static DocumentControlDS GetAllDocumentControlsToStart()
        {
            return GetAllDocumentControlsToStart(null);
        }

        public static DocumentControlDS GetAllDocumentControlsToStart(GADataTransaction transaction)
        {
            // get all DocumentControl records where status not = NextRevisionStarted and (next or last revision date is not null)
            // get all DocumentControl records which is the result of sql statement defined in GALists where category='sys' and listvalue='WSDocCntrlSQL'
            // Tor 20170815 String appendSql = " inner join GALists GALists on GALists.ListsRowId=DocumentRevisionStatusListsRowId where (d.NextRevisionDate is not null or d.LastRevisionStartedDate is not null) and GALists.GAListValue != '";
            DocumentControlDS ds = new DocumentControlDS();
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            // Tor 20170815 get SQL sentence from GALists SqlDataAdapter da = new SqlDataAdapter(_selectSql + appendSql+nextRevisionStarted+"'", myConnection);
            SqlDataAdapter da = new SqlDataAdapter(WSDocCntrlSQL, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;

            da.Fill(ds, GADataClass.GADocumentControl.ToString());
            return ds;
        }
        // Tor 20170815 Get SQL sentence from GALists
        public static string WSDocCntrlSQL
        {
            get
            {
                //return "ga"; 
                // Tor 20160308 return System.Configuration.ConfigurationSettings.AppSettings.Get("CoordinatorWorkitemGivenName");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("WSDocCntrlSQL");
            }
        }

        
      
    }
}
