using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for RecordHistoryLogDb.
	/// </summary>
	public class RecordHistoryLogDb // Tor 20150108 new class
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GARecordHistoryLog";
        // Tor 20150409 changed sort from: private static string _selectSqlSorted = @"SELECT * FROM GARecordHistoryLog order by reportToPersonnelRowId,reportToRoleListsRowId,reportOnClass,reportOnClassRowId,classMember,classMemberRowId,classOwner,classOwnerRowId,DateTimeFieldIdChanged";
        private static string _selectSqlSorted =            @"SELECT * FROM GARecordHistoryLog order by reportToPersonnelRowId,reportToRoleListsRowId,reportOnClass,reportOnClassRowId,DateTimeFieldIdChanged,classMember,classMemberRowId,classOwner,classOwnerRowId";
        private static string _selectPersonnelSqlSorted =   @"SELECT * FROM GARecordHistoryLog where reportToPersonnelRowId is not null order by reportToPersonnelRowId,reportOnClass,reportOnClassRowId,DateTimeFieldIdChanged,classMember,classMemberRowId,classOwner,classOwnerRowId";
        private static string _selectRolesSqlSorted =       @"SELECT * FROM GARecordHistoryLog where reportToRoleListsRowId is not null order by reportToRoleListsRowId,reportOnClass,reportOnClassRowId,DateTimeFieldIdChanged";
		
		/// // Tor 20130908 replaced this with next statement private static string _selectSqlAll = @"SELECT RecordHistoryLogRowId, RecordHistoryLogId,Name, Address,Comment,BankAccount,Telephone, MimeType FROM GARecordHistoryLog";
        //private static string _selectSqlAll = @"SELECT RecordHistoryLogRowId, RecordHistoryLogId, Name, Address,Comment,BankAccount,Telephone, EMailAddress, WebAddress, FileRowId FROM GARecordHistoryLog";
		
		public RecordHistoryLogDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //public static RecordHistoryLogDS GetAllRecordHistoryLogs()
        //{

        //    RecordHistoryLogDS RecordHistoryLogData = new RecordHistoryLogDS();	
        //    SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

        //    SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
        //    da.Fill(RecordHistoryLogData, "GARecordHistoryLog");
		
        //    return RecordHistoryLogData;
        //}

        public static RecordHistoryLogDS GetAllRecordHistoryLogsSorted()
        {

            RecordHistoryLogDS RecordHistoryLogData = new RecordHistoryLogDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

            SqlDataAdapter da = new SqlDataAdapter(_selectSqlSorted, myConnection);
            da.Fill(RecordHistoryLogData, "GARecordHistoryLog");

            return RecordHistoryLogData;
        }

        public static RecordHistoryLogDS GetAllRecordHistoryLogsToPersonSorted()
        {

            RecordHistoryLogDS RecordHistoryLogData = new RecordHistoryLogDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

            SqlDataAdapter da = new SqlDataAdapter(_selectPersonnelSqlSorted, myConnection);
            da.Fill(RecordHistoryLogData, "GARecordHistoryLog");

            return RecordHistoryLogData;
        }

        public static RecordHistoryLogDS GetAllRecordHistoryLogsToRoleSorted()
        {

            RecordHistoryLogDS RecordHistoryLogData = new RecordHistoryLogDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

            SqlDataAdapter da = new SqlDataAdapter(_selectRolesSqlSorted, myConnection);
            da.Fill(RecordHistoryLogData, "GARecordHistoryLog");

            return RecordHistoryLogData;
        }

        //public static RecordHistoryLogDS GetRecordHistoryLogByRecordHistoryLogRowId(int RecordHistoryLogRowId)
        //{
        //    DataCache.ValidateCache(DataCache.DataCacheType.RecordHistoryLogByRecordHistoryLogRowId);

        //    RecordHistoryLogDS cachedObject = (RecordHistoryLogDS)DataCache.GetCachedObject(DataCache.DataCacheType.RecordHistoryLogByRecordHistoryLogRowId, RecordHistoryLogRowId.ToString());
        //    if (cachedObject != null)
        //        return cachedObject;
            
        //    String appendSql = " WHERE RecordHistoryLogRowId="+RecordHistoryLogRowId;
        //    RecordHistoryLogDS RecordHistoryLogData = new RecordHistoryLogDS();
        //    SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
        //    SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
        //    da.Fill(RecordHistoryLogData, "GARecordHistoryLog");
        //    DataCache.AddCachedObject(DataCache.DataCacheType.RecordHistoryLogByRecordHistoryLogRowId, RecordHistoryLogRowId.ToString(), RecordHistoryLogData);
        //    return RecordHistoryLogData;
        //}

        //public static RecordHistoryLogDS GetRecordHistoryLogNoContentByRecordHistoryLogRowId(int RecordHistoryLogRowId)
        //{
        //    DataCache.ValidateCache(DataCache.DataCacheType.RecordHistoryLogNoContentByRecordHistoryLogRowId);
            
        //    RecordHistoryLogDS cachedObject = (RecordHistoryLogDS)DataCache.GetCachedObject(DataCache.DataCacheType.RecordHistoryLogNoContentByRecordHistoryLogRowId, RecordHistoryLogRowId.ToString());
        //    if (cachedObject != null)
        //        return cachedObject;
            
        //    String appendSql = " WHERE RecordHistoryLogRowId="+RecordHistoryLogRowId;
        //    RecordHistoryLogDS RecordHistoryLogData = new RecordHistoryLogDS();
        //    SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
        //    SqlDataAdapter da = new SqlDataAdapter(_selectSqlAll+appendSql, myConnection);
        //    da.Fill(RecordHistoryLogData, "GARecordHistoryLog");
        //    DataCache.AddCachedObject(DataCache.DataCacheType.RecordHistoryLogNoContentByRecordHistoryLogRowId, RecordHistoryLogRowId.ToString(), RecordHistoryLogData);
        //    return RecordHistoryLogData;
        //}

        //public static RecordHistoryLogDS GetRecordHistoryLogsByOwner(int OwnerRowId, GADataClass OwnerDataClass )
        //{
        //    RecordHistoryLogDS RecordHistoryLogData = new RecordHistoryLogDS();	
        //    string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GARecordHistoryLog, OwnerRowId, OwnerDataClass);
			
        //    SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
        //    SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
        //    da.Fill(RecordHistoryLogData, GADataClass.GARecordHistoryLog.ToString());
        //    return RecordHistoryLogData;
        //}

		
		public static RecordHistoryLogDS UpdateRecordHistoryLog(RecordHistoryLogDS RecordHistoryLogSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
//			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			
			da.Update(RecordHistoryLogSet, GADataClass.GARecordHistoryLog.ToString());
			return RecordHistoryLogSet;
		}

	}
}
