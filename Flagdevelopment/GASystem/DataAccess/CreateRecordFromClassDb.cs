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
    public class CreateRecordFromClassDb
    {
        private static string _selectSql = "select d.* from GACreateRecordFromClass d ";
        //private static string nextRevisionStarted = "NextRevisionStarted";
        //                                            //Approved
        //                                            //NextRevisionStarted
        //                                            //FrstRevisionNotStarted
        //private static string documentControlWorkflowStatus = "DS";

        public CreateRecordFromClassDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        public static CreateRecordFromClassDS GetCreateRecordFromClass(int fromClass, int toClass)
        {
            return GetCreateRecordFromClass(fromClass,toClass,null);
        }

        public static CreateRecordFromClassDS GetCreateRecordFromClass(int fromClass, int toClass, GADataTransaction transaction)
        {
            String appendSql = " where d.CopyFromClassRowId="+fromClass+" and d.CopyToClassRowId="+toClass ;
            CreateRecordFromClassDS ds = new CreateRecordFromClassDS();
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            SqlDataAdapter da = new SqlDataAdapter(_selectSql + appendSql, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;
            da.Fill(ds, GADataClass.GACreateRecordFromClass.ToString());
            return ds;
        }
    }
}
