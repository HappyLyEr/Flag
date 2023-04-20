using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using openwfe.workitem;
using WorkflowStarter = GASystem.BusinessLayer.WorkflowStarter;
using GASystem.DataAccess.Utils;
using System.Collections.Generic;
using System.Text;

namespace GASystem.DataAccess
{
    public class WorkflowStarterDb
    {
        private static string _selectSql = "select w.* from GAWorkflowStarter w ";
        private static string completed = "Completed";
        //private static string running = "Running";
        //private static string workflowStarterStatus = "WSS";

        public WorkflowStarterDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        public static WorkflowStarterDS GetAllWorkflowStarters()
        {
            WorkflowStarterDS wds = new WorkflowStarterDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

            SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
            da.Fill(wds, GADataClass.GAWorkflowStarter.ToString());

            return wds;
        }
        public static WorkflowStarterDS GetAllCurrentWorkflowStarters()
        {
            string sql = _selectSql + @" inner join GALists GALists on GALists.ListsRowId=WorkflowStarterStatusListsRowId and GALists.GAListValue= '" + completed + "' where WorkflowNextStartDateTime <= @nextStartDate or WorkflowNextStartDateTime is null"; 
            WorkflowStarterDS wds = new WorkflowStarterDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            da.SelectCommand.Parameters.AddWithValue("@nextStartDate", System.DateTime.UtcNow);

            da.Fill(wds, GADataClass.GAWorkflowStarter.ToString());
            return wds;
        }

        public static WorkflowStarterDS GetWorkflowStarterByWorkflowStarterRowId(int WorkflowStarterRowId)
        {
            return GetWorkflowStarterByWorkflowStarterRowId(WorkflowStarterRowId, null);
            //string sql = _selectSql + " WHERE WorkflowStarterRowId = @WorkflowStarterRowId";
            //WorkflowStarterDS WorkflowStarterData = new WorkflowStarterDS();
            //SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            //SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            //da.SelectCommand.Parameters.AddWithValue("@WorkflowStarterRowId", WorkflowStarterRowId);
            //da.Fill(WorkflowStarterData, "GAWorkflowStarter");
            //return WorkflowStarterData;
        }

        public static WorkflowStarterDS GetWorkflowStarterByWorkflowStarterRowId(int WorkflowStarterRowId, GADataTransaction transaction)
        {
            String appendSql = " WHERE WorkflowStarterRowId = @WorkflowStarterRowId ";
            WorkflowStarterDS WorkflowStarterData = new WorkflowStarterDS();
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;
            da.SelectCommand.Parameters.AddWithValue("@WorkflowStarterRowId", WorkflowStarterRowId);
            da.Fill(WorkflowStarterData, "GAWorkflowStarter");
            return WorkflowStarterData;
        }

        public static WorkflowStarterDS UpdateWorkflowStarter(WorkflowStarterDS WorkflowStarterSet, GADataTransaction Transaction)
        {
            SqlConnection myConnection = DataUtils.GetConnection(Transaction);
            SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
            if (Transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)Transaction.Transaction;
            da.SelectCommand.Connection = myConnection;
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
            da.Update(WorkflowStarterSet, GADataClass.GAWorkflowStarter.ToString());
            return WorkflowStarterSet;
        }
        public static WorkflowStarterDS UpdateWorkflowStarter(WorkflowStarterDS WorkflowStarterSet)
        {
            return UpdateWorkflowStarter(WorkflowStarterSet, null);
        }
    }
}
