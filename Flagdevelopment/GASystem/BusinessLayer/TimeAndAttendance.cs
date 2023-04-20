using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for TimeAndAttendance.
	/// </summary>
	public class TimeAndAttendance : BusinessClass
	{
		public TimeAndAttendance()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GATimeAndAttendance;
		}

		public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
		{
			if (ds is TimeAndAttendanceDS)
				return UpdateTimeAndAttendance((TimeAndAttendanceDS)ds, transaction);
			else 
			{
				GASystem.DataAccess.DataAccess access = DataAccessFactory.Make(DataClass, transaction);
				return access.Update(ds);
			}
		}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetTimeAndAttendanceByTimeAndAttendanceRowId(RowId);
		}

		public static TimeAndAttendanceDS GetAllTimeAndAttendances()
		{
			return TimeAndAttendanceDb.GetAllTimeAndAttendances();
		}
	

		public static TimeAndAttendanceDS GetTimeAndAttendancesByOwner(GADataRecord TimeAndAttendanceOwner)
		{
			return  TimeAndAttendanceDb.GetTimeAndAttendancesByOwner(TimeAndAttendanceOwner.RowId, TimeAndAttendanceOwner.DataClass);
		}


		public static TimeAndAttendanceDS GetTimeAndAttendancesByOwnerAndDate(int OwnerRowId, GADataClass OwnerDataClass, System.DateTime EmploymentDate ) 
		{
			return TimeAndAttendanceDb.GetTimeAndAttendancesByOwnerAndDate(OwnerRowId, OwnerDataClass, EmploymentDate);
		}

		public static TimeAndAttendanceDS GetTimeAndAttendancesByOwnerAndDate(int OwnerRowId, GADataClass OwnerDataClass, System.DateTime StartDate, System.DateTime EndDate ) 
		{
			return TimeAndAttendanceDb.GetTimeAndAttendancesByOwnerAndDate(OwnerRowId, OwnerDataClass, StartDate, EndDate);
		}

		public static TimeAndAttendanceDS GetTimeAndAttendanceByTimeAndAttendanceRowId(int TimeAndAttendanceRowId)
		{
			return TimeAndAttendanceDb.GetTimeAndAttendanceByTimeAndAttendanceRowId(TimeAndAttendanceRowId);
		}

		public static TimeAndAttendanceDS GetNewTimeAndAttendance()
		{
			TimeAndAttendanceDS iDS = new TimeAndAttendanceDS();
			iDS.GATimeAndAttendance.Columns["TimeAndAttendanceRowId"].AutoIncrement = true;
			//iDS.GATimeAndAttendance.Columns["TimeAndAttendanceRowId"].AutoIncrementSeed = -1;
			//iDS.GATimeAndAttendance.Columns["TimeAndAttendanceRowId"].AutoIncrementStep = -1;
			

			GASystem.DataModel.TimeAndAttendanceDS.GATimeAndAttendanceRow row = iDS.GATimeAndAttendance.NewGATimeAndAttendanceRow();
		//	set default values for non-null attributes
			row.EmploymentRowId = 0;
			row.DateTimeFrom = DateTime.Today;
			iDS.GATimeAndAttendance.Rows.Add(row);
			return iDS;			
		}

		public static TimeAndAttendanceDS SaveNewTimeAndAttendance(TimeAndAttendanceDS TimeAndAttendanceSet, GADataRecord TimeAndAttendanceOwner)
		{
		//	if (TimeAndAttendanceSet.GATimeAndAttendance[0].IsDateAndTimeOfIncidentNull())
		//		TimeAndAttendanceSet.GATimeAndAttendance[0].DateAndTimeOfIncident = System.DateTime.Now;
			//Generate ReferenceId
		//	TimeAndAttendanceSet.GATimeAndAttendance[0].IncidentId = IdGenerator.GenerateId(GADataClass.GATimeAndAttendance, TimeAndAttendanceOwner, TimeAndAttendanceSet.GATimeAndAttendance[0].DateAndTimeOfIncident);
		
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				TimeAndAttendanceSet = UpdateTimeAndAttendance(TimeAndAttendanceSet, transaction);
				DataClassRelations.CreateDataClassRelation(TimeAndAttendanceOwner.RowId, TimeAndAttendanceOwner.DataClass, TimeAndAttendanceSet.GATimeAndAttendance[0].TimeAndAttendanceRowId, GADataClass.GATimeAndAttendance, transaction);
				//add member classes
			//	Utils.StoreObject.AddMemberClasses(new GADataRecord(TimeAndAttendanceSet.GATimeAndAttendance[0].TimeAndAttendanceRowId, GADataClass.GATimeAndAttendance), transaction);
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
			return TimeAndAttendanceSet;
		}

		public static TimeAndAttendanceDS UpdateTimeAndAttendance(TimeAndAttendanceDS TimeAndAttendanceSet)
		{
			return UpdateTimeAndAttendance(TimeAndAttendanceSet, null);
		}
		public static TimeAndAttendanceDS UpdateTimeAndAttendance(TimeAndAttendanceDS TimeAndAttendanceSet, GADataTransaction transaction)
		{
			return TimeAndAttendanceDb.UpdateTimeAndAttendance(TimeAndAttendanceSet, transaction);
		}
	}
}
