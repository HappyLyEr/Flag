using System;
using GASystem.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for ReportInstanceFilterDb.
	/// </summary>
	public class ReportInstanceFilterDb
	{
		private static string _selectSql = @"SELECT * FROM GAReportInstanceFilter ";
		
		
		public ReportInstanceFilterDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ReportInstanceFilterDS GetReportInstanceFilterByOwner(GADataRecord Owner) 
		{
			//string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAReportInstanceFilter, Owner.RowId, Owner.DataClass);
			//TODO  throw proper GA error if owner is not ReportIncident

			if (Owner.DataClass != GADataClass.GAReportInstance)
				throw new Exception("Can not get report filters, current context is not ReportInstanceFilter");
			
			string sql = _selectSql + " where ReportInstanceRowId = " + Owner.RowId;
			
			return (ReportInstanceFilterDS) fillDataSet(sql);
		}

		public static ReportInstanceFilterDS GetReportInstanceFilterByRowId(int RowId) 
		{
			string sql = _selectSql + " where ReportInstanceFilterRowId = " + RowId;
			return (ReportInstanceFilterDS) fillDataSet(sql);
		}

		private static DataSet fillDataSet(string sql) 
		{
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			ReportInstanceFilterDS ds = new ReportInstanceFilterDS();
			da.Fill(ds, GADataClass.GAReportInstanceFilter.ToString());
			myConnection.Close();
			return ds;
		}

		public static ReportInstanceFilterDS UpdateReportInstanceFilter(ReportInstanceFilterDS ReportInstanceFilterSet)
		{
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);

			da.Update(ReportInstanceFilterSet, GADataClass.GAReportInstanceFilter.ToString());
			return ReportInstanceFilterSet;
		}

	}
}
