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
    public class ProjectRiskDb
    {
        public ProjectRiskDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        public static ProjectRiskDS GetAllProjectRiskToStart()
        {
            return GetAllProjectRiskToStart(null);
        }

        public static ProjectRiskDS GetAllProjectRiskToStart(GADataTransaction transaction)
        {
            
            ProjectRiskDS ds = new ProjectRiskDS();
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            SqlDataAdapter da = new SqlDataAdapter(WSProjectRiskSQL, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;

            da.Fill(ds, GADataClass.GAProjectRisk.ToString());
            return ds;
        }
        // Get SQL sentence from GALists
        public static string WSProjectRiskSQL
        {
            get
            {
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("WSProjectRiskSQL");
            }
        }      
    }
}
