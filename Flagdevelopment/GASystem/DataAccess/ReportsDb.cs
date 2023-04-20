using System;
using GASystem.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for ReportInstaneDb.
	/// </summary>
	public class ReportsDb
	{
		private static string _selectSql = @"SELECT * FROM GAReports ";
		
		
		public ReportsDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ReportsDS GetAllReports() 
		{
			return (ReportsDS)fillDataSet(_selectSql);
		}

		public static ReportsDS GetReportsByRowId(int RowId) 
		{
			string sql = _selectSql + " where ReportsRowId = " + RowId;
			return (ReportsDS)fillDataSet(sql);
		}

		public static ReportsDS GetReportsByInstanceId(int InstanceId) 
		{
			string sql = _selectSql + " where ReportsRowId in (select reportid from GAReportInstance where ReportInstanceRowId = " + InstanceId + ")  ";
			return (ReportsDS)fillDataSet(sql);
		}


		public static ReportsDS GetReportInstancesByRowId(int RowId) 
		{
			string sql = _selectSql + " where ReportInstanceRowId = " + RowId;
			return (ReportsDS)fillDataSet(sql);
		}


		/// <summary>
		/// Get all report definitions for single class reports based on classname.
		/// Single class report type is defined by switchfree1 = 1, and dataclass is set in tablename
		/// </summary>
		/// <param name="DataClass"></param>
		/// <returns></returns>
		public static ReportsDS GetSingleClassReportsByClassName(string DataClass)  
		{
			string sql = _selectSql + " where switchfree1 = 1 and  tablename = '" + DataClass + "'";
			return (ReportsDS)fillDataSet(sql);
		}

		//030305 This method might never be used. According to the current superclass links, reports will 
		//be a top level dataclass. added in case we will need it in the future.
		public static ReportsDS GetReportsByOwner(GADataRecord Owner) 
		{
			//string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAReports, Owner.RowId, Owner.DataClass);
			//return (ReportsDS) fillDataSet(selectSqlOwner);
			//since it may not have a owner, return all reports
			return GetAllReports();
		}

		private static DataSet fillDataSet(string sql) 
		{
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			ReportsDS ds = new ReportsDS();
			da.Fill(ds, GADataClass.GAReports.ToString());
			myConnection.Close();
			return ds;
		}
			
		public static ReportsDS UpdateReports(ReportsDS ReportsSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			

			da.Update(ReportsSet, GADataClass.GAReports.ToString());
			return ReportsSet;
		}
	}
}
