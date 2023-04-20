using System;
using GASystem.BusinessLayer.Utils;
using GASystem.AppUtils.DateRangeGenerator;
using GASystem.DataAccess;
using GASystem.DataModel;
using System.Data;
using System.Data.SqlClient;
using GASystem.AppUtils;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for ReportBuilder.
	/// </summary>
	public class ReportBuilder
	{
		private string m_filter = string.Empty;
		
		public ReportBuilder()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//Filter, set filter where statement for filtering return data.
		//TODO. reimplement this as a class where we are buildign the filter by adding columns, operators, and values
		public string Filter 
		{
			set 
			{
				m_filter = value;
			}
			get 
			{
				return m_filter;
			}
		}

		private void FillMemberDataSet(DataSet ds, GADataRecord Owner, GADataClass TargetMemberClass, SqlConnection conn) 
		{
			ds.Merge(BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(TargetMemberClass, Owner, Filter));

			
		}

		private void FindMembers(DataSet ds, GADataRecord Owner, GADataClass TargetMemberClass, SqlConnection conn) 
		{
			//fill dataset with children of type TargetMemberClass;
			FillMemberDataSet(ds, Owner, TargetMemberClass, conn); 
			
			return;
		}

		public DataSet FindAllMembers(GADataRecord Owner, GADataClass TargetMemberClass) 
		{

			//DataSet ds = GASystem.DataAccess.DatasetFactory.NewDataSet(TargetMemberClass);
			DataSet ds = new DataSet();
			//System.Collections.ArrayList memberRowIds = new System.Collections.ArrayList();
			//memberRowIds.Add(4);
			//temp = (int[])memberRowIds.ToArray(typeof(System.Int32));
			//return temp;
			return  RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(TargetMemberClass, Owner, m_filter);
		//	FindAllMembers(ds, Owner, TargetMemberClass);
	//		ds.DefaultViewManager.DataViewSettings[TargetMemberClass.ToString()].Sort = AppUtils.FieldDefintion.GetSortExpression(TargetMemberClass);
			//return ds;
		}

		
		/// <summary>
		/// Get all recorde by date. Used by the get data for report instance methods
		/// 
		/// </summary>
		/// <param name="Owner"></param>
		/// <param name="TargetMemberClass"></param>
		/// <param name="DateFrom"></param>
		/// <param name="DateTo"></param>
		/// <returns></returns>
		public DataSet FindAllMembers(GADataRecord Owner, GADataClass TargetMemberClass, System.DateTime DateFrom, System.DateTime DateTo) 
		{
			//return  RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(TargetMemberClass, Owner, m_filter, DateFrom, DateTo);
			GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(TargetMemberClass);

			return bc.GetAllRecordsWithinOwnerAndLinkedRecords(Owner, DateFrom, DateTo);// BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(myDataClass, myOwner, filter);//rb.FindAllMembers(myOwner, myDataClass);
				
		}

        /// <summary>
        /// Get all recorde by date. Used by the get data for report instance methods
        /// 
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="TargetMemberClass"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="filter">genric sql where part</param>
        /// <returns></returns>
        public DataSet FindAllMembers(GADataRecord Owner, GADataClass TargetMemberClass, System.DateTime DateFrom, System.DateTime DateTo, string filter)
        {
            //return  RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(TargetMemberClass, Owner, m_filter, DateFrom, DateTo);
            GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(TargetMemberClass);

            return bc.GetAllRecordsWithinOwnerAndLinkedRecords(Owner, DateFrom, DateTo, filter);// BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(myDataClass, myOwner, filter);//rb.FindAllMembers(myOwner, myDataClass);

        }



		public DataSet FindAllMembersByDate(GADataRecord Owner, GADataClass TargetMemberClass, int ReportInstanceId) 
		{

			//TODO more error handling
			AbstractDateRange dateRange = BusinessLayer.ReportInstance.GetDateRange(ReportInstanceId);


//for vertical filter,  add logic here
            string filter = string.Empty;
            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(TargetMemberClass);


            if (cd.hasVerticalField())
            {

                ReportInstanceDS dsi = BusinessLayer.ReportInstance.GetReportInstanceByReportInstanceRowId(ReportInstanceId);
                if (dsi.Tables[0].Rows[0]["IntFree1"] != DBNull.Value)
                {
                    int verticalid = (int)dsi.Tables[0].Rows[0]["IntFree1"];
                    filter = cd.DataClassName + "." + cd.VerticalFieldName + " = " + verticalid.ToString();
                }

            }


           
            

			return  FindAllMembers(Owner, TargetMemberClass, dateRange.GetDateFrom(), dateRange.GetDateTo(), filter );
		}
	

		public void FindAllMembers(DataSet ResultSet, GADataRecord Owner, GADataClass TargetMemberClass) 
		{
			ResultSet = RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(TargetMemberClass, Owner, string.Empty);
//			SqlConnection conn = new SqlConnection(DataUtils.getConnectionString());
//			conn.Open();
//			FindMembers(ResultSet, Owner, TargetMemberClass, conn);
//			conn.Close();			
		}

		public void GenerateDateFilter(int ReportInstanceId, GADataClass DataClass) 
		{
			ReportInstanceDS instanceDs = ReportInstance.GetReportInstanceByReportInstanceRowId(ReportInstanceId);
			GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataClass);
			
			//if classdescription has a datafield definition use this
			if (cd != null && cd.DateField != string.Empty) 
			{
				string filter =  " ( @dateFrom <= _formDateField_  and _toDateField_ <= @DateTo     )";
				filter = filter.Replace("_formDateField_", cd.DateField);
				filter = filter.Replace("_toDateField_", cd.DateField);
				if (m_filter != string.Empty) 
					m_filter += " AND ";
				m_filter += filter;
			}
			else 
			{
				//else check if a dateto and datefrom field are defined
				//TODO fix this, how do we define that the event happen within in the periode. Eg. employment. is employment in periode if the 
				//employment started or ended in the periode? 
				if (cd != null && cd.DateToField != string.Empty && cd.DateFromField != string.Empty) 
				{
					string filter =  " (_formDateField_ <= @dateFrom and (_toDateField_ >= @DateTo or _toDateField_ is null ))";
			
					filter = filter.Replace("_formDateField_", cd.DateFromField);
					filter = filter.Replace("_toDateField_", cd.DateToField);
					if (m_filter != string.Empty) 
						m_filter += " AND ";
					m_filter += filter;
				}
			}


			

			

		}


		public void GenerateFilter(int ReportInstanceId) 
		{
			//find tablename

			ReportInstanceDS instanceDs = ReportInstance.GetReportInstanceByReportInstanceRowId(ReportInstanceId);
			ReportsDS reportsDs = GASystem.BusinessLayer.Reports.GetReportsByReportsRowId(instanceDs.GAReportInstance[0].ReportId);
			string tableName = reportsDs.GAReports[0].tablename;
			
			
			ReportInstanceFilterDS ds = ReportInstanceFilter.GetReportInstanceFilterByOwner(new GADataRecord(ReportInstanceId, GADataClass.GAReportInstance));
			string filter = "";
			if (ds.GAReportInstanceFilter.Rows.Count != 0) 
			{
				string rowSeperatorString = "";
				string columnValue = "";
				foreach (ReportInstanceFilterDS.GAReportInstanceFilterRow row in ds.GAReportInstanceFilter.Rows) 
				{
					string dataFormat = "";
					if (AppUtils.FieldDefintion.GetFieldDescription(row.ColumnName, tableName).Dataformat != null)
						dataFormat = AppUtils.FieldDefintion.GetFieldDescription(row.ColumnName, tableName).Dataformat;

					if (dataFormat.ToUpper() == "NUMBER" || dataFormat.ToUpper() == "INTEGER") 
						columnValue = row.ColumnValue;
					else
						columnValue = "'" + row.ColumnValue + "'";
					
					filter = filter + rowSeperatorString + row.ColumnName + " " + row.Operator + " " + columnValue;
					rowSeperatorString = " and ";

				}
			}
			m_filter = filter;
		}

	}
}
