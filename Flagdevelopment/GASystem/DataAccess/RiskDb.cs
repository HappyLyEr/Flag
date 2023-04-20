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
    public class RiskDb
    {
        public RiskDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        public static RiskDS GetAllRiskToStart()
        {
            return GetAllRiskToStart(null);
        }

        //Gao Added 20230403 get all risks for Verification process
        public static RiskDS GetAllRiskVerifyToStart()
        {
            return GetAllRiskVerifyToStart(null);
        }

        public static RiskDS GetAllRiskVerifyToStart(GADataTransaction transaction)
        {
            RiskDS ds = new RiskDS();
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            SqlDataAdapter da = new SqlDataAdapter(WSRiskVerifySQL, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;

            da.Fill(ds, GADataClass.GARisk.ToString());
            return ds;
        }
        //Gao 20230403 get SQL sentence from GALists
        public static string WSRiskVerifySQL
        {
            get
            {
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("WSRiskVerifySQL");
            }
        }

        //get all RAs for review process
        public static RiskDS GetAllRiskToStart(GADataTransaction transaction)
        {
            // get all DocumentControl records where status not = NextRevisionStarted and (next or last revision date is not null)
            // Tor 20170815 String appendSql = " inner join GALists GALists on GALists.ListsRowId=DocumentRevisionStatusListsRowId where (d.NextRevisionDate is not null or d.LastRevisionStartedDate is not null) and GALists.GAListValue != '";
            RiskDS ds = new RiskDS();
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            // Tor 20170815 get SQL sentence from GALists SqlDataAdapter da = new SqlDataAdapter(_selectSql + appendSql+nextRevisionStarted+"'", myConnection);
            SqlDataAdapter da = new SqlDataAdapter(WSRiskSQL, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;

            da.Fill(ds, GADataClass.GARisk.ToString());
            return ds;
        }
        // Tor 20170815 Get SQL sentence from GALists
        public static string WSRiskSQL
        {
            get
            {
                //return "ga"; 
                // Tor 20160308 return System.Configuration.ConfigurationSettings.AppSettings.Get("CoordinatorWorkitemGivenName");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("WSRiskSQL");
            }
        }

        
      
    }
}
