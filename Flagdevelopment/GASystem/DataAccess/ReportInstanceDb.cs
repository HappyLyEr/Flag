using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for ReportInstanceDb.
	/// </summary>
	public class ReportInstanceDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAReportInstance";
		
		public ReportInstanceDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ReportInstanceDS GetAllReportInstances()
		{

			ReportInstanceDS ReportInstanceData = new ReportInstanceDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(ReportInstanceData, "GAReportInstance");
		
			return ReportInstanceData;
		}

		public static ReportInstanceDS GetReportInstanceByReportInstanceRowId(int ReportInstanceRowId)
		{
			String appendSql = " WHERE ReportInstanceRowId="+ReportInstanceRowId;
			ReportInstanceDS ReportInstanceData = new ReportInstanceDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(ReportInstanceData, "GAReportInstance");
			return ReportInstanceData;
		}

		public static ReportInstanceDS GetReportInstancesByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			ReportInstanceDS ReportInstanceData = new ReportInstanceDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAReportInstance, OwnerRowId, OwnerDataClass);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(ReportInstanceData, GADataClass.GAReportInstance.ToString());
			return ReportInstanceData;
		}

		public static ReportInstanceDS UpdateReportInstance(ReportInstanceDS ReportInstanceSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);			

			da.Update(ReportInstanceSet, GADataClass.GAReportInstance.ToString());
			return ReportInstanceSet;
		}
	}
}
