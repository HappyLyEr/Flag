using System;
using System.Data;
using GASystem.DataModel;
using GASystem.AppUtils.DateRangeGenerator;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for ReportView.
	/// </summary>
	public class ReportView
	{
		public ReportView()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	
		public static DataSet GetRecordSetAllDetailsByDataRecord(DataModel.GADataRecord DataRecord) 
		{
		
			string sql = GASystem.DataAccess.SQLGenerateUtils.GenerateSelectFromFieldDefinition(DataRecord);
			
			DataSet ds = fillDataSet(sql, DataRecord.DataClass);
			
		//	ds.Tables[0].PrimaryKey = new DataColumn[] {ds.Tables[0].Columns[0]};
			
			return ds;
		}
		private static  DataModel.View.ReportView fillDataSet(string sql, DataModel.GADataClass DataClass) 
		{
			return fillDataSet(sql, DataClass.ToString());
		}

		private static  DataModel.View.ReportView fillDataSet(string sql, string TableName) 
		{
			System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection(GASystem.DataAccess.DataUtils.getConnectionString());
			System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(sql, myConnection);
			
			
			//DataSet ds = new DataSet();
			DataModel.View.ReportView ds = new GASystem.DataModel.View.ReportView();

			
			da.Fill(ds, TableName);
			myConnection.Close();
			return ds;
		}

		private static  DataModel.View.ReportView fillDataSet(string sql, string TableName, System.DateTime DateFrom, System.DateTime DateTo)  
		{
			System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(sql);
			System.DateTime fromDate = DateFrom.Date + new TimeSpan(0, 0,0,0,0);//include all of the day in the less than test, set check time to the start of the day
			System.DateTime toDate = DateTo.Date + new TimeSpan(0, 23,59,59,0);

			command.Parameters.AddWithValue("@dateFrom",fromDate) ;
			command.Parameters.AddWithValue("@dateTo",toDate) ;
			
			System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection(GASystem.DataAccess.DataUtils.getConnectionString());
			System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter();
			command.Connection = myConnection;
			da.SelectCommand = command;
			
			DataModel.View.ReportView ds = new GASystem.DataModel.View.ReportView();

			myConnection.Open();
			
			da.Fill(ds, TableName);
			myConnection.Close();
			return ds;
			
		}

		/// <summary>
		/// TODO: Add this as a method to business class and dataaccess. Use that method instead of this.
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="Owner"></param>
		/// <param name="Filter"></param>
		/// <returns></returns>
		public static DataSet GetRecordSetAllDetailsForDataClassByOwner(DataModel.GADataClass DataClass, GADataRecord Owner, string Filter) 
		{
			DataSet ds = new DataSet();
			string sql;
			switch (DataClass) 
			{
				case GADataClass.GAReportInstance :
					ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassByOwner(DataClass, Owner);
					break;
				case GADataClass.GAReports :
					ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassByOwner(DataClass, Owner);
					break;
//				case GADataClass.GASafetyStatistics:
//					ds = GASystem.BusinessLayer.SafetyStatistics.GetSafetyStatisticsByOwner(Owner, Filter);
//					break;
				default:
					sql = GASystem.DataAccess.SQLGenerateUtils.GenerateSelectFromFieldDefinition(DataClass, Owner.RowId, Owner.DataClass);
					if (Filter != string.Empty)
						sql += " and " + Filter;
			
					ds = fillDataSet(sql, DataClass);
					//ds.Tables[0].PrimaryKey = new DataColumn[] {ds.Tables[0].Columns[0]};
					break;
			}
			
			return ds;
		}

		public static DataSet GetRecordSetAllDetailsForViewByOwner(string ViewName, GADataRecord Owner, string Filter) 
		{
			DataSet ds = new DataSet();
			string sql = "select * from " + ViewName;
			sql += " where ownerclass = '" + Owner.DataClass.ToString() + "' and ownerclassrowid = " + Owner.RowId.ToString();
			
			if (Filter != string.Empty)
				sql += " and " + Filter;
			
			ds = fillDataSet(sql, ViewName);
			
			return ds;
		}

		public static DataSet GetRecordSetAllDetails(string ViewName, string Filter) 
		{
			DataSet ds = new DataSet();
			//string sql = "select * from " + ViewName;
			//use sqlview to generate sql;
			Utils.SQLViewSelect vs = new GASystem.DataAccess.Utils.SQLViewSelect();
			string sql = vs.GenerateSQL(ViewName);


			
			if (Filter != string.Empty)
				//sql += " and " + Filter;
				sql += " and " + Filter;
			
			ds = fillDataSet(sql, ViewName);
			
			return ds;
		}

		public static DataSet GetRecordSetAllDetailsWithinOwnerByReportInstance(string ViewName, GADataRecord Owner, string Filter, int ReportInstanceId) 
		{
			
			string DateFilter = string.Empty;
				
			ReportInstanceDS instanceDs = GASystem.BusinessLayer.ReportInstance.GetReportInstanceByReportInstanceRowId(ReportInstanceId);
			if (instanceDs.GAReportInstance.Rows.Count == 0) 
			{
				//instance does not exists, return empty set.
				return new GASystem.DataModel.View.ReportView();
				
			}
			
			ListsDS lds = GASystem.BusinessLayer.Lists.GetListsByListsRowId(instanceDs.GAReportInstance[0].ReportDateFormatListsRowId);
			if (lds.GALists.Rows.Count == 0)
				throw new Exception("Invalid report date format type");  //TODO replace with ga exception
			



			AbstractDateRange dateRange;
			DateRangeEnum dateRangeEnum = DateRangeFactory.ParseDateRangeEnum(lds.GALists[0].GAListValue);
			if (instanceDs.GAReportInstance[0].IsDateFromNull() || instanceDs.GAReportInstance[0].IsDateToNull()) 
			{
				dateRange = DateRangeFactory.Make(dateRangeEnum);
			} 
			else 
			{
				dateRange = DateRangeFactory.Make(dateRangeEnum, instanceDs.GAReportInstance[0].DateFrom, instanceDs.GAReportInstance[0].DateTo);
			}
			



			DateFilter = 	BusinessLayer.Utils.RecordsetFactory.GenerateDateFilter(ViewName);

			
			DataSet ds = new DataSet();
			//string sql = "select * from " + ViewName;
			//use sqlview to generate sql;
			Utils.SQLViewSelect vs = new GASystem.DataAccess.Utils.SQLViewSelect();
			string sql = vs.GenerateSQL(ViewName);

			sql += " AND Path LIKE '%/" + Owner.DataClass + "-" + Owner.RowId + "/%'";
//                       Path LIKE '%/" + Owner.DataClass + "-" + Owner.RowId + "/%'";
			
			if (Filter != string.Empty)
				//sql += " and " + Filter;
				sql += " and " + Filter;

			if (DateFilter != string.Empty) 
			{
				sql += " and " + DateFilter;
			}
			
			ds = fillDataSet(sql, ViewName, dateRange.GetDateFrom(), dateRange.GetDateTo());
			
			return ds;


		}

		public static DataSet GetRecordSetAllDetailsWithinOwner(string ViewName, GADataRecord Owner, string Filter) 
		{
			DataSet ds = new DataSet();
			//string sql = "select * from " + ViewName;
			//use sqlview to generate sql;
			Utils.SQLViewSelect vs = new GASystem.DataAccess.Utils.SQLViewSelect();
			string sql = vs.GenerateSQL(ViewName);

			sql += " and path like '%/" + Owner.DataClass + "-" + Owner.RowId + "/%'";
			
			if (Filter != string.Empty)
				//sql += " and " + Filter;
				sql += " and " + Filter;
			
			ds = fillDataSet(sql, ViewName);
			
			return ds;
		}
	}
}
