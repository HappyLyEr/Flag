using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for ReportDb.
	/// </summary>
	public class ReportDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAReport";
		
		public ReportDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ReportDS GetAllReports()
		{

			ReportDS ReportData = new ReportDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(ReportData, "GAReport");
		
			return ReportData;
		}

		public static ReportDS GetReportByReportRowId(int ReportRowId)
		{
			String appendSql = " WHERE ReportRowId="+ReportRowId;
			ReportDS ReportData = new ReportDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(ReportData, "GAReport");
			return ReportData;
		}

		public static ReportDS GetReportsByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			ReportDS ReportData = new ReportDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAReport, OwnerRowId, OwnerDataClass);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(ReportData, GADataClass.GAReport.ToString());
			return ReportData;
		}

		public static ReportDS UpdateReport(ReportDS ReportSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			
			da.Update(ReportSet, GADataClass.GAReport.ToString());
			return ReportSet;
		}
	}
}
