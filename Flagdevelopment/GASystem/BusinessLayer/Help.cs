using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Help.
	/// </summary>
	public class Help : BusinessClass
	{
		public Help()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAHelp;
		}

        // Tor 20161028 use standard update method in : BusinessClass
        //public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
        //{
        //    return UpdateHelp((HelpDS)ds, transaction);
        //}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetHelpByHelpRowId(RowId);
		}

		public static HelpDS GetAllHelps()
		{
			return HelpDb.GetAllHelps();
		}
	

		public static HelpDS GetHelpsByOwner(GADataRecord HelpOwner)
		{
			return  HelpDb.GetHelpsByOwner(HelpOwner.RowId, HelpOwner.DataClass);
		}

		public static HelpDS GetHelpByHelpRowId(int HelpRowId)
		{
			return HelpDb.GetHelpByHelpRowId(HelpRowId);
		}

		public static HelpDS GetHelpByClass(GADataClass DataClass) 
		{
			return HelpDb.GetHelpByClass(DataClass);
		}

		public static HelpDS GetNewHelp()
		{
			HelpDS iDS = new HelpDS();
			GASystem.DataModel.HelpDS.GAHelpRow row = iDS.GAHelp.NewGAHelpRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GAHelp.Rows.Add(row);
			return iDS;
			
		}

		public static HelpDS SaveNewHelp(HelpDS HelpSet, GADataRecord HelpOwner)
		{
			//if (HelpSet.GAHelp[0].IsDateAndTimeOfIncidentNull())
			//	HelpSet.GAHelp[0].DateAndTimeOfIncident = System.DateTime.Now;
			//Generate ReferenceId
			//HelpSet.GAHelp[0].IncidentId = IdGenerator.GenerateId(GADataClass.GAHelp, HelpOwner, HelpSet.GAHelp[0].DateAndTimeOfIncident);
			
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				HelpSet = UpdateHelp(HelpSet, transaction);
				//DataClassRelations.CreateDataClassRelation(HelpOwner.RowId, HelpOwner.DataClass, HelpSet.GAHelp[0].HelpRowId, GADataClass.GAHelp, transaction);
				//add member classes
				//Utils.StoreObject.AddMemberClasses(new GADataRecord(HelpSet.GAHelp[0].HelpRowId, GADataClass.GAHelp), transaction);
				
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
			return HelpSet;
		}

		public static HelpDS UpdateHelp(HelpDS HelpSet)
		{
			return UpdateHelp(HelpSet, null);
		}
		public static HelpDS UpdateHelp(HelpDS HelpSet, GADataTransaction transaction)
		{
			return HelpDb.UpdateHelp(HelpSet, transaction);
		}
	}
}
