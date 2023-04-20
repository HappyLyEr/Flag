using System;
using System.Data;
using GASystem.DataModel;
using GASystem.BusinessLayer;

namespace GASystem.BusinessLayer.Utils
{
	/// <summary>
	/// Summary description for RecordsetFactory.
	/// </summary>
	public class RecordsetFactory
	{
		public RecordsetFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
//		public static DataSet GetReportRecordSetForDataClassByOwner(GADataClass DataClass, GADataRecord Owner) 
//		{
//			//if the current owner can't have records of type DataClass, change owner to owner of owner 
//			//this method will then return sibling of DataClass for the passed Owner
//			if (!GASystem.BusinessLayer.DataClassRelations.IsDataClassValidMember(Owner.DataClass, DataClass))
//				Owner = GASystem.BusinessLayer.DataClassRelations.GetOwner(Owner);
//			
//			if (DataClass == GADataClass.GATextItem) 
//			{
//				return GASystem.DataAccess.TextItemDb.GetTextItemsFullDetailsByOwner(Owner);
//			} 
//			else if (DataClass == GADataClass.GAAction) 
//			{
//				return GetRecordSetAllDetailsForDataClassByOwner(DataClass, Owner);
//			}
//			else if (DataClass == GADataClass.GAMeeting) 
//			{
//				return (GASystem.DataModel.View.ReportView)GetRecordSetAllDetailsForDataClassByOwner(DataClass, Owner);
//			}
//
//			else 
//			{
//				return GetRecordSetForDataClassByOwner(DataClass, Owner);
//			}
//		}

		/// <summary>
		/// get all records for a dataclass by owner
		/// 
		/// /TODO only used by reportview. Rewrite reportview so that is does not call 
		/// 
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="Owner"></param>
		/// <returns></returns>
		public static DataSet GetRecordSetForDataClassByOwner(GADataClass DataClass, GADataRecord Owner) 
		{
			BusinessClass bc = RecordsetFactory.Make(DataClass);
			return bc.GetByOwner(Owner, null);

//			DataSet ds;
////
//			switch (DataClass) 
//			{
//				case GADataClass.GAAction:
//					ds = (DataSet)Action.GetActionsByOwner(Owner);
//					break;
//				case GADataClass.GAAudit:
//					ds = (DataSet)A
//						udit.GetAuditsByOwner(Owner);
//					break;
//				case GADataClass.GACertificate:
//					ds = Certificate.GetCertificatesByOwner(Owner);
//					break;
//				case GADataClass.GACompany:
//					ds = (DataSet)Company.GetCompanysByOwner(Owner);
//					break;
//				case GADataClass.GACost:
//					ds = (DataSet)Cost.GetCostsByOwner(Owner);
//					break;
//				case GADataClass.GACourse:
//					ds = (DataSet)Course.GetCoursesByOwner(Owner);
//					break;
//				case GADataClass.GACoursePersonList:
//					ds = (DataSet)CoursePersonList.GetCoursePersonListsByOwner(Owner);
//					break;
//				case GADataClass.GADailyEmployeeCount:
//					ds = (DataSet)DailyEmployeeCount.GetDailyEmployeeCountsByOwner(Owner);
//					break;					
//				case GADataClass.GADamagedEquipment:
//					ds = (DataSet)DamagedEquipment.GetDamagedEquipmentsByOwner(Owner);
//					break;
//				case GADataClass.GADepartment:
//					ds = (DataSet)Department.GetDepartmentsByOwner(Owner);
//					break;
//				case GADataClass.GAEmployment:
//					ds = (DataSet)Employment.GetEmploymentsByOwner(Owner);
//					break;
//				case GADataClass.GAEquipmentDamageReport:
////					ds = EquipmentDamageReport.GetEquipmentDamageReportsByOwner(Owner);
////					break;
//				case GADataClass.GAFile:
//					ds = File.GetFilesByOwner(Owner);
//					break;
//				case GADataClass.GAHazardIdentification:
//					ds = (DataSet)HazardIdentification.GetHazardIdentificationsByOwner(Owner);
//					break;
//				case GADataClass.GAHelp:
//					ds = Help.GetHelpsByOwner(Owner);
//					break;
//				case GADataClass.GAIncidentReport:
//					ds = (DataSet)IncidentReport.GetIncidentReportsByOwner(Owner);
//					break;
//				case GADataClass.GAInjuredParty:
//					ds = (DataSet)InjuredParty.GetInjuredPartysByOwner(Owner);
//					break;
//				case GADataClass.GAListCategory:
//					ds = (DataSet)ListCategory.GetListCategorysByOwner(Owner);
//					break;
//				case GADataClass.GALists:
//					ds = (DataSet)Lists.GetListssByOwner(Owner);
//					break;
//				case GADataClass.GALocation:
//					ds = (DataSet)Location.GetLocationsByOwner(Owner);
//					break;
//				case GADataClass.GAMeansOfContact:
//					ds = MeansOfContact.GetMeansOfContactsByOwner(Owner);
//					break;
//				case GADataClass.GAMeeting:
//					ds = Meeting.GetMeetingsByOwner(Owner);
//					break;
//				case GADataClass.GAMeetingPersonList:
//					ds = MeetingPersonList.GetMeetingPersonListsByOwner(Owner);
//					break;
//				case GADataClass.GAMeetingText:
//					ds = MeetingText.GetMeetingTextsByOwner(Owner);
//					break;
//				case GADataClass.GANextOfKin:
//					ds = NextOfKin.GetNextOfKinsByOwner(Owner);
//					break;
//				case GADataClass.GAPersonnel:
//					ds = Personnel.GetPersonnelsByOwner(Owner);
//					break;
//					
//				case GADataClass.GAProcedure:
//					ds = Procedure.GetProceduresByOwner(Owner);
//					break;
//				case GADataClass.GAProject:
//					ds = (DataSet)Project.GetProjectsByOwner(Owner);
//					break;
//				case GADataClass.GAReport:
//					ds = Report.GetReportsByOwner(Owner);
//					break;
//				case GADataClass.GAReporter:
//					ds = Reporter.GetReportersByOwner(Owner);
//					break;
//				case GADataClass.GAReportInstance:
//					//ds = ReportInstance.get
//					ds = ReportInstance.GetReportInstancesByOwner(Owner);
//					break;
//				case GADataClass.GAReports:
//					//ds = ReportInstance.get
//					ds = Reports.GetReportssByOwner(Owner);
//					break;
//				case GADataClass.GAReportInstanceFilter:
//					ds = ReportInstanceFilter.GetReportInstanceFilterByOwner(Owner);
//					break;
//				
//				case GADataClass.GAResource:
//					ds = Resource.GetResourcesByOwner(Owner);
//					break;
//				case GADataClass.GAResourceSpecification:
//					ds = ResourceSpecification.GetResourceSpecificationsByOwner(Owner);
//					break;
//				case GADataClass.GASafetyObservation:
//					ds = SafetyObservation.GetSafetyObservationsByOwner(Owner);
//					break;
//					
//				case GADataClass.GATask:
//					ds = Task.GetTasksByOwner(Owner);
//					break;
//				case GADataClass.GATaskTemplate:
//					ds = (DataSet)TaskTemplate.GetTaskTemplatesByOwner(Owner);
//					break;
//				case GADataClass.GATeam:
//					ds = Team.GetTeamsByOwner(Owner);
//					break;
//				case GADataClass.GATextItem:
//					ds = (DataSet)TextItem.GetTextItemsByOwner(Owner);
//					break;
//				case GADataClass.GATimeAndAttendance:
//					ds = (DataSet)TimeAndAttendance.GetTimeAndAttendancesByOwner(Owner);
//					break;
//
//				case GADataClass.GAWorkitem:
//					ds = Workitem.GetAllWorkitems();
//					break;
//
//
//				
//				default:
//					//TODO replace with GA error handling
//					throw new Exception("no recordset found for current dataclass");
//					//ds = new DataSet();
//					break;
//			}
//			return ds;
		}

		public static DataSet GetRecordSetByDataRecord(GADataRecord dataRecord) 
		{
			BusinessClass bc = Make(dataRecord.DataClass);
			return bc.GetByRowId(dataRecord.RowId);
		
		}

		public static DataSet GetRecordSetAllDetailsForDataClassByOwner(GADataClass DataClass, GADataRecord Owner) 
		{
			
			return GetRecordSetAllDetailsForDataClassByOwner(DataClass, Owner, string.Empty);

		}


		

        ///// <summary>
        ///// Returns a dataset with all records for a gadatatable within the specified owner
        ///// </summary>
        ///// <param name="DataClass"></param>
        ///// <param name="Owner"></param>
        ///// <param name="Filter"></param>
        ///// <returns></returns>
        //public static DataSet GetRecordSetForDataClassWithinOwner(GADataClass DataClass, GADataRecord Owner, string Filter) 
        //{
        //    DataSet ds = new DataSet();
        //    string sql;
        //    AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataClass);

		
        //    if (!cd.IsTop)
        //        sql = GASystem.DataAccess.SQLGenerateUtils.GenerateSelectForGATableAllWithinOwner(DataClass, Owner.RowId, Owner.DataClass);
        //    else
        //        //TODO this should only be called if dataclass is company or personell. Should we have a test against class defs?
        //        //Should all this methods be moved to the dataclasses?
        //        sql = "select * from " + DataClass.ToString() + " where 1=1 ";
		
        //    if (Filter != string.Empty)
        //        sql += " and " + Filter;
			
        //    //Append security (filter our any rows that the current user does not have access to)
        //    GASystem.DataAccess.Security.GASecurityDb_new security = new GASystem.DataAccess.Security.GASecurityDb_new(DataClass, null);
        //    sql = security.AppendSecurityFilterQueryForRead(sql);

        //    ds = fillDataSet(sql, DataClass);
			
        //   // ds.Tables[0].PrimaryKey = new DataColumn[] {ds.Tables[0].Columns[0]};   //do we need this one now that we are using businessclass and dataaccess class?
			
        //    return ds;
        //}


        /// <summary>
        ///  Returns a dataset with all records with details for a dataclass for the specified owner
        /// </summary>
        /// <param name="DataClass"></param>
        /// <param name="Owner"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static DataSet GetRecordSetAllDetailsForDataClassByOwner(GADataClass dataClass, GADataRecord owner, string filter)
        {
            if (owner == null)
                owner = new GADataRecord(1, GADataClass.GAFlag);

            return DataAccess.DataAccessReportViewFactory.Make(dataClass).GetByOwner(owner, filter);                        
        }



		/// <summary>
		/// Get all records with details for a dataclass within the owner
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="Owner"></param>
		/// <param name="Filter"></param>
		/// <returns></returns>
		public static DataSet GetRecordSetAllDetailsForDataClassWithinOwner(GADataClass dataClass, GADataRecord owner, string filter) 
		{
            if (owner == null)
                owner = new GADataRecord(1, GADataClass.GAFlag);

            return DataAccess.DataAccessReportViewFactory.Make(dataClass).GetRecordsWithinOwner(owner, filter, System.DateTime.MinValue, System.DateTime.MaxValue);  

		}

//        /// <summary>
//        /// Get all records of a specified type within a owner record
//        /// </summary>
//        /// <param name="DataClass">Record types to retrive</param>
//        /// <param name="Owner">Owner record to get data for</param>
//        /// <param name="Filter">SQL filter for filtering returned data</param>
//        /// <returns>Dataset contain records of specified types</returns>
//        public static DataSet GetRecordSetAllDetailsForDataClassWithinOwner(GADataClass DataClass, GADataRecord Owner, string Filter, System.Collections.Hashtable FilterParams) 
//        {
//            DataSet ds = new DataSet();
//            string sql;
//            AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataClass);

//            if (cd.IsView) 
//            {
//                string sqlview = "select * from {0} where path like '%{1}-{2}/%'";  // "path like" should not be used, as it requires to much database cpu 
//                sql = string.Format(sqlview, new object[] {DataClass.ToString(), Owner.DataClass.ToString(), Owner.RowId.ToString()  });


//            } 
//            else 
//            {

//                if (Owner != null)
//                    sql = GASystem.DataAccess.SQLGenerateUtils.GenerateSelectAllWithinFromFieldDefinition(DataClass, Owner.RowId, Owner.DataClass);
//                else
//                    //TODO this should only be called if dataclass is company or personell. Should we have a test against class defs?
//                    //Should all this methods be moved to the dataclasses?
//                    sql = GASystem.DataAccess.SQLGenerateUtils.GenerateSelectFromFieldDefinition(DataClass);
//            }

//            if (Filter != string.Empty)
//                sql += " and " + Filter;

//            string dateFilter = GenerateDateFilter(DataClass);
//            if (dateFilter != string.Empty)
//                sql += "and" + dateFilter;
			
//            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(sql);
////			System.DateTime fromDate = DateFrom.Date + new TimeSpan(0, 0,0,0,0);//include all of the day in the less than test, set check time to the start of the day
////			System.DateTime toDate = DateTo.Date + new TimeSpan(0, 23,59,59,0);

//            foreach (System.Collections.DictionaryEntry ParamKey in FilterParams ) 
//            {
//                 command.Parameters.Add(ParamKey.Key.ToString(), ParamKey.Value) ;
//            }

////			command.Parameters.Add("@dateFrom",fromDate) ;
////			command.Parameters.Add("@dateTo",toDate) ;


//            ds = fillDataSet(command, DataClass);
//            if (!cd.IsView)
//                ds.Tables[0].PrimaryKey = new DataColumn[] {ds.Tables[0].Columns[0]};
		
			
//            return ds;
//        }

		/// <summary>
		/// Get the recordset for a single gadatatable within the owner based on a daterange
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="Owner"></param>
		/// <param name="Filter"></param>
		/// <param name="DateFrom"></param>
		/// <param name="DateTo"></param>
		/// <returns></returns>
		public static DataSet GetRecordSetForDataClassWithinOwner(GADataClass DataClass, GADataRecord Owner, string Filter, System.DateTime DateFrom, System.DateTime DateTo) 
		{
			GASystem.DataAccess.DataAccess da = new GASystem.DataAccess.DataAccess(DataClass, null);
			return da.GetRecordsWithinOwner(Owner, Filter, DateFrom, DateTo);
		}




        public static DataSet GetRecordSetListViewAllDetailsForDataClassWithinOwner(GADataClass dataClass, GADataRecord owner, string filter, System.DateTime dateFrom, System.DateTime dateTo)
        {
            if (owner != null)
                return DataAccess.DataAccessListViewFactory.Make(dataClass).GetRecordsWithinOwner(owner, filter, dateFrom, dateTo);
            else
                return DataAccess.DataAccessListViewFactory.Make(dataClass).GetRecordsWithinOwner(new GADataRecord(1, GADataClass.GAFlag), filter, dateFrom, dateTo);
        }

        public static DataSet GetRecordSetListViewAllDetailsByDataRecord(GADataRecord dataRecord)
        {
            return DataAccess.DataAccessListViewFactory.Make(dataRecord.DataClass).GetByDataRecord(dataRecord);
        }


		public static DataSet GetRecordSetAllDetailsForDataClassWithinOwner(GADataClass dataClass, GADataRecord owner, string filter, System.DateTime dateFrom, System.DateTime dateTo) 
		{
            if (owner == null)
                owner = new GADataRecord(1, GADataClass.GAFlag);

            return DataAccess.DataAccessReportViewFactory.Make(dataClass).GetRecordsWithinOwner(owner, filter, dateFrom, dateTo);
		}

        /// <summary>
        /// Generate datefilter for specifued dataclass.
        /// Used by reportview. TODO move to reportutils or rewrite reportview
        /// </summary>
        /// <param name="DataClassName"></param>
        /// <returns></returns>
		public static string GenerateDateFilter(string DataClassName) 
		{
			GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataClassName);
			
			//if classdescription has a datafield definition use this
			string filter = string.Empty;
			if (cd != null && cd.DateField != string.Empty) 
			{
				filter =  " ( @dateFrom <= _formDateField_  and _toDateField_ <= @DateTo     )";
                filter = filter.Replace("_formDateField_", cd.DataClassName + "." + cd.DateField);
                filter = filter.Replace("_toDateField_", cd.DataClassName + "." + cd.DateField);
			}
			return filter;
		}

        // Tor 20131223 Create date from-to filter for class
        public static string GenerateDateFromToFilter(string DataClassName)
        {
            GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataClassName);

            //if classdescription has a datafield definition use this
            string filter = string.Empty;
            if (cd != null && cd.DateFromField != string.Empty && cd.DateToField != string.Empty)
            {
                filter = " ( ( _formDateFromField_ <= @dateFrom or _formDateFromField_ is null ) and ( _formDateToField_ >= @DateTo or _formDateToField_ is null ) )";
                filter = filter.Replace("_formDateFromField_", cd.DataClassName + "." + cd.DateFromField);
                filter = filter.Replace("_formDateToField_", cd.DataClassName + "." + cd.DateToField);
            }
            return filter;
        }
        /// <summary>
        /// Generate datespan sql filer. 
        /// user by sibliglookupfilter. TODO move or rewrite siblinglookupfilter
        /// </summary>
        /// <param name="DateFromField"></param>
        /// <param name="DateToField"></param>
        /// <returns></returns>
		public static string generateDateSpanFilter(string DateFromField, string DateToField) 
		{
			//todate in timespan
			string filterto = " ( @dateFrom <= _formDateField_  and _formDateField_ <= @DateTo     ) ";

			//from date in timespan
			string filterfrom = " ( @dateFrom <= _toDateField_  and _toDateField_ <= @DateTo     ) ";

			//timespan between start and end
			string filterin =  " ( @dateFrom >= _formDateField_  and _toDateField_ >= @DateTo     ) ";
			
			//from is null
			string filterfromnull = " ( _formDateField_ is null and _toDateField_ >= @dateFrom     ) ";
			//to is null
			string filtertonull = " ( _toDateField_ is null  and _formDateField_ <= @DateTo     ) ";
			//both is null
			string filternull = " ( _toDateField_ is null  and _formDateField_ is null     ) ";

			//combined 
			string filter = " (" + filterto + " or " + filterfrom + " or "+ filterin  + " or "+ filterfromnull + " or " + filtertonull + " or " + filternull + ") ";

			filter = filter.Replace("_formDateField_", DateFromField);
			filter = filter.Replace("_toDateField_", DateToField);
			
			return filter;
		}




        /// <summary>
        /// Return all records of this class type within Onwer. Returns record in the owner tree and records in trees
        /// linked via child records. Returns records for a single table.
        /// In addition is information for all dropdowns used in table included in the dataset
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static DataSet GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataClass dataClass, GADataRecord owner,  DateTime startDate, DateTime endDate, string filter)
        {
            BusinessClass bc = Make(dataClass);
            
            //get main data
            DataSet ds = bc.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecords(owner, startDate, endDate, filter);

            //add lookup data
            AppUtils.FieldDescription[] fds = AppUtils.FieldDefintion.GetFieldDescriptions(dataClass);
            foreach (AppUtils.FieldDescription fd in fds)
            {
                if (fd.ControlType.ToUpper() == "DROPDOWNLIST" || fd.ControlType.ToUpper() == "POSTBACKDROPDOWNLIST")
                { //C:\gadev2\gamain\FlagGUILibrary\GAControls\ViewDetailsList\
                    System.Data.DataSet dsl = BusinessLayer.Lists.GetListsRowIdByCategory(fd.ListCategory);
                    dsl.Tables[0].TableName = "galists_" + fd.ListCategory;
                    ds.Merge(dsl.Tables[0]);
                }
            }

            return ds;
        }



        /// <summary>
        /// Return all records of this class type within Onwer. Returns record in the owner tree and records in trees
        /// linked via child records. Returns records for a single table.
        /// In addition is information for all dropdowns used in table included in the dataset
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static DataSet GetAllRecordsForGAListViewByOwnerIncludingDropDownDetails(GADataClass dataClass, GADataRecord owner)
        {
            return GetAllRecordsForGAListViewByOwnerIncludingDropDownDetails(dataClass, owner, string.Empty);
        }

        /// <summary>
        /// Return all records of this class type within Onwer. Returns record in the owner tree and records in trees
        /// linked via child records. Returns records for a single table.
        /// In addition is information for all dropdowns used in table included in the dataset
        /// Includes options for setting filter 
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static DataSet GetAllRecordsForGAListViewByOwnerIncludingDropDownDetails(GADataClass dataClass, GADataRecord owner, string filter)
        {
            // BusinessClass bc = Make(dataClass);
            if (owner == null)
                owner = new GADataRecord(1, GADataClass.GAFlag);
            // Tor 201611 Security 20161220 
            // Tor 20170323 recover GASystem.AppUtils.SessionManagement.SetCurrentSubContext(owner);

            //get main data
            DataSet ds = DataAccess.DataAccessListViewFactory.Make(dataClass).GetByOwner(owner, filter);                                                 //bc.GetByOwner(owner, null);

            //add lookup data
            // Tor 20140320 add owner reference to exclude fields if specified in GAFieldDefinitions
            // replaced AppUtils.FieldDescription[] fds = AppUtils.FieldDefintion.GetFieldDescriptions(dataClass);
            AppUtils.FieldDescription[] fds = AppUtils.FieldDefintion.GetFieldDescriptions(dataClass.ToString(),owner.DataClass.ToString());
            foreach (AppUtils.FieldDescription fd in fds)
            {
                if (fd.ControlType.ToUpper() == "DROPDOWNLIST" || fd.ControlType.ToUpper() == "POSTBACKDROPDOWNLIST")
                {
                    System.Data.DataSet dsl = BusinessLayer.Lists.GetListsRowIdByCategory(fd.ListCategory);
                    dsl.Tables[0].TableName = "galists_" + fd.ListCategory;
                    ds.Merge(dsl.Tables[0]);
                }
            }

            return ds;
        }


        /// <summary>
        /// Return all records of this class type within Onwer. Returns record in the owner tree and records in trees
        /// linked via child records. Returns records for a single table.
        /// In addition is information for all dropdowns used in table included in the dataset
        /// Includes options for setting filter 
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static DataSet GetAllRecordsForGAListViewByOwnerIncludingDropDownDetails(GADataClass dataClass, GADataRecord owner, DateTime startDate, DateTime endDate, string filter)
        {
            // BusinessClass bc = Make(dataClass);
            if (owner == null)
                owner = new GADataRecord(1, GADataClass.GAFlag);
            //get main data
            DataSet ds = DataAccess.DataAccessListViewFactory.Make(dataClass).GetByOwnerAndTimeSpan(owner, startDate, endDate, filter);                                                 //bc.GetByOwner(owner, null);

            //add lookup data
            AppUtils.FieldDescription[] fds = AppUtils.FieldDefintion.GetFieldDescriptions(dataClass);
            foreach (AppUtils.FieldDescription fd in fds)
            {
                if (fd.ControlType.ToUpper() == "DROPDOWNLIST" || fd.ControlType.ToUpper() == "POSTBACKDROPDOWNLIST")
                {
                    System.Data.DataSet dsl = BusinessLayer.Lists.GetListsRowIdByCategory(fd.ListCategory);
                    dsl.Tables[0].TableName = "galists_" + fd.ListCategory;
                    ds.Merge(dsl.Tables[0]);
                }
            }

            return ds;
        }


		public static DataSet GetRecordSetAllDetailsByDataRecord(GADataRecord dataRecord) 
		{
            // Tor 201611 Security 20161219 set SubContectRecord to owner of dataRecord
            // Tor 20170323 recover statements below
            //GADataRecord owner = GASystem.BusinessLayer.DataClassRelations.GetOwner(dataRecord);
            //if (owner != null)
            //{
            //    GASystem.AppUtils.SessionManagement.SetCurrentSubContext(owner);
            //}

            //else
            //{
            //    GASystem.AppUtils.SessionManagement.SetCurrentSubContext(new GADataRecord(1, GADataClass.GAFlag));
            //}

            return DataAccess.DataAccessReportViewFactory.Make(dataRecord.DataClass).GetByRowId(dataRecord.RowId);
		}

		
		/// <summary>
		/// Get all records for a set of rowids. Rowids are passed as a ArrayList of integers
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="RowIds"></param>
		/// <returns></returns>
		public static DataSet GetRecordSetAllDetailsForRowIds(GADataClass dataClass, System.Collections.ArrayList rowIds) 
		{
            return DataAccess.DataAccessReportViewFactory.Make(dataClass).GetByRowIds(rowIds);
		}

		/// <summary>
		/// Get all records for a set of rowids. Rowids are passed as a comma seperated string
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="RowIds"></param>
		/// <returns></returns>
		public static DataSet GetRecordSetAllDetailsForRowIds(GADataClass dataClass, string rowIds) 
		{
            BusinessClass bc = Make(dataClass);

            string[] rowidArray = rowIds.Split(',');
            int[] rows = new int[rowidArray.Length];
            try
            {

                for (int t = 0; t < rowidArray.Length; t++)
                {
                    rows[t] = int.Parse(rowidArray[t]);
                }

            }
            catch (Exception ex)
            {
                rows = new int[0];
            }

            return bc.getByRowIds(rows, null);
       
		}

        //private static DataSet fillDataSet(string sql, GADataClass DataClass) 
        //{
        //    System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection(GASystem.DataAccess.DataUtils.getConnectionString());
        //    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(sql, myConnection);
			
			
        //    DataSet ds = new DataSet();
			
			
			
        //    da.Fill(ds, DataClass.ToString());
        //    myConnection.Close();
        //    return ds;
        //}



        ////TO BE REMOVED

        //private static DataSet fillDataSet(System.Data.SqlClient.SqlCommand command, GADataClass DataClass)
        //{
        //    System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection(GASystem.DataAccess.DataUtils.getConnectionString());
        //    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter();
        //    command.Connection = myConnection;
        //    da.SelectCommand = command;

        //    DataSet ds = new DataSet();

        //    myConnection.Open();

        //    da.Fill(ds, DataClass.ToString());
        //    myConnection.Close();
        //    return ds;
        //}



        /// <summary>
        /// Create a new businessclass of correct type
        /// </summary>
        /// <param name="DataClass"></param>
        /// <returns></returns>
		public static BusinessClass Make(GADataClass DataClass) 
		{
			//check license
			if (DataClass != GADataClass.GALicense)
				License.ValidateLicense();
			
			if (DataClass == GADataClass.GAAction)
				return new Action();

			if (DataClass == GADataClass.GADailyEmployeeCount)
				return new DailyEmployeeCount();

			if (DataClass == GADataClass.GAEmployment)
				return new Employment();

			if (DataClass == GADataClass.GAFile)
				return new File();

            // Tor 20140418 required after start using GASuperClassLinks instead of GAClass to decide permission
            if (DataClass == GADataClass.GAFlag)
                return new Flag();
            
            if (DataClass == GADataClass.GALists)
				return new Lists();
			if (DataClass == GADataClass.GAListCategory)
				return new ListCategory();

			if (DataClass == GADataClass.GAMeansOfContact)
				return new MeansOfContact();

			if (DataClass == GADataClass.GAUser)
				return new User();
			
			if (DataClass == GADataClass.GAReportInstance)
				return new ReportInstance();
			if (DataClass == GADataClass.GAFileContent)
				return new FileContent();

			if (DataClass == GADataClass.GAWorkitem)
				return new Workitem();

            if (DataClass == GADataClass.GAActionWorkitemView)
                return new ActionWorkitemView();
            if (DataClass == GADataClass.GARemedialActionView)
                return new RemedialActionView();

            
           
			//else
			return new GenericBusinnesClass(DataClass);


		}
				
	}
}

