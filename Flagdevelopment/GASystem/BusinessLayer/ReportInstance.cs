using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for ReportInstance.
	/// </summary>
	public class ReportInstance : BusinessClass
	{
		public ReportInstance()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAReportInstance;
		}

        // Tor 20161028 use standard update method in : BusinessClass
        //public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
        //{
        //    return UpdateReportInstance((ReportInstanceDS)ds, transaction);
        //}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetReportInstanceByReportInstanceRowId(RowId);
		}

		public static ReportInstanceDS GetAllReportInstances()
		{
			return ReportInstanceDb.GetAllReportInstances();
		}
	

		public static ReportInstanceDS GetReportInstancesByOwner(GADataRecord ReportInstanceOwner)
		{
			return  ReportInstanceDb.GetReportInstancesByOwner(ReportInstanceOwner.RowId, ReportInstanceOwner.DataClass);
		}

		public static ReportInstanceDS GetReportInstanceByReportInstanceRowId(int ReportInstanceRowId)
		{
			return ReportInstanceDb.GetReportInstanceByReportInstanceRowId(ReportInstanceRowId);
		}

		public static ReportInstanceDS GetNewReportInstance()
		{
			ReportInstanceDS iDS = new ReportInstanceDS();
			GASystem.DataModel.ReportInstanceDS.GAReportInstanceRow row = iDS.GAReportInstance.NewGAReportInstanceRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GAReportInstance.Rows.Add(row);
			return iDS;			
		}

		public static ReportInstanceDS SaveNewReportInstance(ReportInstanceDS ReportInstanceSet, GADataRecord ReportInstanceOwner)
		{
			// if (ReportInstanceSet.GAReportInstance[0].IsDateAndTimeOfIncidentNull())
			//	ReportInstanceSet.GAReportInstance[0].DateAndTimeOfIncident = System.DateTime.Now;
			//Generate ReferenceId
			// ReportInstanceSet.GAReportInstance[0].IncidentId = IdGenerator.GenerateId(GADataClass.GAReportInstance, ReportInstanceOwner, ReportInstanceSet.GAReportInstance[0].DateAndTimeOfIncident);
			
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				ReportInstanceSet = UpdateReportInstance(ReportInstanceSet, transaction);
				DataClassRelations.CreateDataClassRelation(ReportInstanceOwner.RowId, ReportInstanceOwner.DataClass, ReportInstanceSet.GAReportInstance[0].ReportInstanceRowId, GADataClass.GAReportInstance, transaction);
				//add member classes
			//	Utils.StoreObject.AddMemberClasses(new GADataRecord(ReportInstanceSet.GAReportInstance[0].ReportInstanceRowId, GADataClass.GAReportInstance), transaction);
			
				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				throw ex;
			}
			finally 
			{
				transaction.Connection.Close();
			}
			return ReportInstanceSet;
		}

		public static ReportInstanceDS UpdateReportInstance(ReportInstanceDS ReportInstanceSet)
		{
			return UpdateReportInstance(ReportInstanceSet, null);
		}
		public static ReportInstanceDS UpdateReportInstance(ReportInstanceDS ReportInstanceSet, GADataTransaction transaction)
		{
			return ReportInstanceDb.UpdateReportInstance(ReportInstanceSet, transaction);
		}

		public static AppUtils.DateRangeGenerator.AbstractDateRange GetDateRange(int ReportInstanceRowId)
		{
			DataModel.ReportInstanceDS instanceDs = GetReportInstanceByReportInstanceRowId(ReportInstanceRowId);

			//calculate date
			if (instanceDs.GAReportInstance.Rows.Count == 0)
				throw new Exception("Report instance not found");  //TODO replace with ga exception

			AppUtils.DateRangeGenerator.AbstractDateRange dateRange;
			AppUtils.DateRangeGenerator.DateRangeEnum dateRangeEnum;

			dateRangeEnum = AppUtils.DateRangeGenerator.DateRangeEnum.AdHoc;
			//HACK: if there is a fromdate and todate specified, always use this. TODO: change in a later version
			if (instanceDs.GAReportInstance[0].IsDateFromNull() || instanceDs.GAReportInstance[0].IsDateToNull()) 
			{
				if (!instanceDs.GAReportInstance[0].IsReportDateFormatListsRowIdNull()) 
				{
					DataModel.ListsDS lds = Lists.GetListsByListsRowId(instanceDs.GAReportInstance[0].ReportDateFormatListsRowId);
					if (lds.GALists.Rows.Count == 0)
						throw new Exception("Invalid report date format type");  //TODO replace with ga exception
			
				
			
					dateRangeEnum = AppUtils.DateRangeGenerator.DateRangeFactory.ParseDateRangeEnum(lds.GALists[0].GAListValue);
				} 
				else 
				{
					dateRangeEnum = AppUtils.DateRangeGenerator.DateRangeEnum.AdHoc;
				}
			}


			if (instanceDs.GAReportInstance[0].IsDateFromNull() || instanceDs.GAReportInstance[0].IsDateToNull()) 
			{
				dateRange = AppUtils.DateRangeGenerator.DateRangeFactory.Make(dateRangeEnum);
			} 
			else 
			{
				dateRange = AppUtils.DateRangeGenerator.DateRangeFactory.Make(dateRangeEnum, instanceDs.GAReportInstance[0].DateFrom, instanceDs.GAReportInstance[0].DateTo);
			}
			

            //adjust date based on owner record
            GADataRecord instanceOwner = GASystem.BusinessLayer.DataClassRelations.GetOwner(new GADataRecord(ReportInstanceRowId,GADataClass.GAReportInstance));
            AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(instanceOwner.DataClass);
            if (!cd.hasDateFromField())
                return dateRange;        //no from date defined, return standard set

            BusinessClass bcOwner = Utils.RecordsetFactory.Make(instanceOwner.DataClass);
            System.Data.DataSet dsOwner = bcOwner.GetByRowId(instanceOwner.RowId);

            if(dsOwner.Tables[0].Rows.Count == 0)
                return dateRange;           //cant find owner, return default daterange instead

            DateTime newDateFrom = dateRange.GetDateFrom();
            DateTime newDateTo = dateRange.GetDateTo();

            //set new datefrom
            if (dsOwner.Tables[0].Columns.Contains(cd.DateFromField) && dsOwner.Tables[0].Rows[0][cd.DateFromField] != DBNull.Value)
                if ((DateTime)dsOwner.Tables[0].Rows[0][cd.DateFromField] > newDateFrom)
                    newDateFrom = (DateTime)dsOwner.Tables[0].Rows[0][cd.DateFromField];

            //set new dateto
           if (dsOwner.Tables[0].Columns.Contains(cd.DateToField) && dsOwner.Tables[0].Rows[0][cd.DateToField] != DBNull.Value)
               if ((DateTime)dsOwner.Tables[0].Rows[0][cd.DateToField] < newDateTo)
                    newDateTo = (DateTime)dsOwner.Tables[0].Rows[0][cd.DateToField];
            
            //check that dateto is not greater than current dateandtime
            if (newDateTo > System.DateTime.Now)
                newDateTo = System.DateTime.Now;


            dateRange = AppUtils.DateRangeGenerator.DateRangeFactory.Make(AppUtils.DateRangeGenerator.DateRangeEnum.AdHoc, newDateFrom, newDateTo);

			return dateRange;
		}

		
	}
}
