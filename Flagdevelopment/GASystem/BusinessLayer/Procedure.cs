using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Procedure.
	/// </summary>
	public class Procedure : BusinessClass
	{
		public Procedure()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAProcedure;
		}

        // Tor 20161028 use standard update method in : BusinessClass
        //public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
        //{
        //    return UpdateProcedure((ProcedureDS)ds, transaction);
        //}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetProcedureByProcedureRowId(RowId);
		}

		public static ProcedureDS GetAllProcedures()
		{
			return ProcedureDb.GetAllProcedures();
		}
	

		public static ProcedureDS GetProceduresByOwner(GADataRecord ProcedureOwner)
		{
			return  ProcedureDb.GetProceduresByOwner(ProcedureOwner.RowId, ProcedureOwner.DataClass);
		}

        // Tor 20140524 new method to fetch std procedure for a given class
        public static ProcedureDS GetProcedureForGADataClass(string dataClass)
        {
            return ProcedureDb.GetProcedureForGADataClass(dataClass);
        }

        //  Gao 20230403 new method to fetch second procedure for a given class if it owns 2 procedures at same time
        public static ProcedureDS GetSecondProcedureForGADataClass(string dataClass)
        {
            return ProcedureDb.GetSecondProcedureForGADataClass(dataClass);
        }

		public static ProcedureDS GetProcedureByProcedureRowId(int ProcedureRowId)
		{
			return ProcedureDb.GetProcedureByProcedureRowId(ProcedureRowId);
		}

		public static ProcedureDS GetNewProcedure()
		{
			ProcedureDS iDS = new ProcedureDS();
			GASystem.DataModel.ProcedureDS.GAProcedureRow row = iDS.GAProcedure.NewGAProcedureRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GAProcedure.Rows.Add(row);
			return iDS;			
		}

		public static ProcedureDS SaveNewProcedure(ProcedureDS ProcedureSet, GADataRecord ProcedureOwner)
		{
		//	if (ProcedureSet.GAProcedure[0].IsDateAndTimeOfIncidentNull())
		//		ProcedureSet.GAProcedure[0].DateAndTimeOfIncident = System.DateTime.Now;
			//Generate ReferenceId
		//	ProcedureSet.GAProcedure[0].IncidentId = IdGenerator.GenerateId(GADataClass.GAProcedure, ProcedureOwner, ProcedureSet.GAProcedure[0].DateAndTimeOfIncident);
		
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				ProcedureSet = UpdateProcedure(ProcedureSet, transaction);
				DataClassRelations.CreateDataClassRelation(ProcedureOwner.RowId, ProcedureOwner.DataClass, ProcedureSet.GAProcedure[0].ProcedureRowId, GADataClass.GAProcedure, transaction);
				//add member classes
			//	Utils.StoreObject.AddMemberClasses(new GADataRecord(ProcedureSet.GAProcedure[0].ProcedureRowId, GADataClass.GAProcedure), transaction);
			
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
			return ProcedureSet;
		}

		public static ProcedureDS UpdateProcedure(ProcedureDS ProcedureSet)
		{
			return UpdateProcedure(ProcedureSet, null);
		}
		public static ProcedureDS UpdateProcedure(ProcedureDS ProcedureSet, GADataTransaction transaction)
		{
			return ProcedureDb.UpdateProcedure(ProcedureSet, transaction);
		}
	}
}
