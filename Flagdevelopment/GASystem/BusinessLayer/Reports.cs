using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Reports.
	/// </summary>
	public class Reports : BusinessClass
	{
		public Reports()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAReports;
		}

        // Tor 20161028 use standard update method in : BusinessClass
        //public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
        //{
        //    return UpdateReports((ReportsDS)ds, transaction);
        //}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetReportsByReportsRowId(RowId);
		}

		public static ReportsDS GetAllReports()
		{
			return ReportsDb.GetAllReports();
		}
	

		public static ReportsDS GetReportssByOwner(GADataRecord ReportsOwner)
		{
			return  ReportsDb.GetReportsByOwner(ReportsOwner);
		}

		public static ReportsDS GetReportsByReportsRowId(int ReportsRowId)
		{
			return ReportsDb.GetReportsByRowId(ReportsRowId);
		}

		public static ReportsDS GetReportsByInstanceId(int InstanceId) 
		{
			return ReportsDb.GetReportsByInstanceId(InstanceId);
		}

		public static ReportsDS GetSingleClassReportsByClassName(string DataClass) 
		{
			return ReportsDb.GetSingleClassReportsByClassName(DataClass);
		}


		public static ReportsDS GetNewReports()
		{
			ReportsDS iDS = new ReportsDS();
			GASystem.DataModel.ReportsDS.GAReportsRow row = iDS.GAReports.NewGAReportsRow();
			//	set default values for non-null attributes
			//	row.EmploymentRowId = 0;
			//	row.DateTimeFrom = DateTime.Today;
			iDS.GAReports.Rows.Add(row);
			return iDS;
			
		}

		public static ReportsDS SaveNewReports(ReportsDS ReportsSet, GADataRecord ReportsOwner)
		{
//			if (ReportsSet.GAReports[0].IsDateAndTimeOfIncidentNull())
//				ReportsSet.GAReports[0].DateAndTimeOfIncident = System.DateTime.Now;
			//Generate ReferenceId
//			ReportsSet.GAReports[0].IncidentId = IdGenerator.GenerateId(GADataClass.GAReports, ReportsOwner, ReportsSet.GAReports[0].DateAndTimeOfIncident);

			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				ReportsSet = UpdateReports(ReportsSet, transaction);
				DataClassRelations.CreateDataClassRelation(ReportsOwner.RowId, ReportsOwner.DataClass, ReportsSet.GAReports[0].ReportsRowId, GADataClass.GAReports, transaction);
				//add member classes
				Utils.StoreObject.AddMemberClasses(new GADataRecord(ReportsSet.GAReports[0].ReportsRowId, GADataClass.GAReports), transaction);
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
			return ReportsSet;
		}

		public static ReportsDS UpdateReports(ReportsDS ReportsSet)
		{
			return UpdateReports(ReportsSet, null);
		}
		public static ReportsDS UpdateReports(ReportsDS ReportsSet, GADataTransaction transaction)
		{
			return ReportsDb.UpdateReports(ReportsSet, transaction);
		}
	}
}
