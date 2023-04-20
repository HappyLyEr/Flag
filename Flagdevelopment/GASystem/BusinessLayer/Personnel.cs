using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Personnel.
	/// </summary>
	public class Personnel : BusinessClass
	{
		public Personnel()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAPersonnel;
		}

        // Tor 20161028 use standard update method in : BusinessClass
        //public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
        //{
        //    return UpdatePersonnel((PersonnelDS)ds, transaction);
        //}
		
        //public override System.Data.DataSet GetByRowId(int RowId)
        //{			
            
        //    return GetPersonnelByPersonnelRowId(RowId);
        //}

        public string getPersonnelFullName(int personnelRowId)
        {
            PersonnelDS pds = (PersonnelDS)GetByRowId(personnelRowId);
            if (pds.GAPersonnel.Rows.Count > 0) {
                PersonnelDS.GAPersonnelRow row = (PersonnelDS.GAPersonnelRow)pds.GAPersonnel.Rows[0];
                return row.FamilyName + ' ' + row.GivenName;
            }
            //else throw exception
            throw new GAExceptions.GADataAccessException("Personnel record not found");
        }

		public static PersonnelDS GetAllPersonnels()
		{
			return PersonnelDb.GetAllPersonnels();
		}
	

		public static PersonnelDS GetPersonnelsByOwner(GADataRecord PersonnelOwner)
		{
			return  PersonnelDb.GetPersonnelsByOwner(PersonnelOwner.RowId, PersonnelOwner.DataClass);
		}

		public static PersonnelDS GetPersonnelByPersonnelRowId(int PersonnelRowId)
		{
		    return PersonnelDb.GetPersonnelByPersonnelRowId(PersonnelRowId);
		}

        // Tor 20170325 Email address copied from GAMeansOfContact to GAPersonnel
        public static string GetPersonnelEmailAddress(int PersonnelRowId)
        {
            PersonnelDS pds = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(PersonnelRowId);
            if (pds.Tables[0].Rows.Count > 0)
                if (pds.Tables[0].Rows[0]["TextFree1"] != DBNull.Value) return pds.Tables[0].Rows[0]["TextFree1"].ToString();
            return string.Empty;
        }

        // Tor 20170325 mobile phone number copied from GAMeansOfContact to GAPersonnel
        public static string GetPersonnelMobilePhoneNumber(int PersonnelRowId)
        {
            PersonnelDS pds = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(PersonnelRowId);
            //if (pds.Tables[0].Rows.Count > 0)
            if (pds.Tables[0].Rows[0]["TextFree2"] != DBNull.Value) return pds.Tables[0].Rows[0]["TextFree2"].ToString();
            return string.Empty;
        }

        // Tor 20140524 added for use by WorkflowStarter.WorkflowStarter.cs to build GAAction records
        public static PersonnelDS GetPersonnelByPersonnelGivenNameAndFamilyName(string GivenName, string Familyname)
        {
            return PersonnelDb.GetPersonnelByPersonnelGivenNameAndFamilyName(GivenName, Familyname);
        }

        // Tor 20150828 Check if person record exists - return first PersonnelRowId occurrence
        public static int ifPersonExists(int PersonnelRowId)
        {
            return PersonnelDb.ifPersonExists(PersonnelRowId);
        }

        public static int ifPersonExists(string GivenName, string FamilyName)
        {
            return PersonnelDb.ifPersonExists(GivenName, FamilyName);
        }
        
        public static PersonnelDS GetNewPersonnel()
		{
			PersonnelDS iDS = new PersonnelDS();
			GASystem.DataModel.PersonnelDS.GAPersonnelRow row = iDS.GAPersonnel.NewGAPersonnelRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GAPersonnel.Rows.Add(row);
			return iDS;			
		}

		public static PersonnelDS SaveNewPersonnel(PersonnelDS PersonnelSet, GADataRecord PersonnelOwner)
		{
		//	if (PersonnelSet.GAPersonnel[0].IsDateAndTimeOfIncidentNull())
		//		PersonnelSet.GAPersonnel[0].DateAndTimeOfIncident = System.DateTime.Now;
			//Generate ReferenceId
		//	PersonnelSet.GAPersonnel[0].IncidentId = IdGenerator.GenerateId(GADataClass.GAPersonnel, PersonnelOwner, PersonnelSet.GAPersonnel[0].DateAndTimeOfIncident);
		
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				PersonnelSet = UpdatePersonnel(PersonnelSet, transaction);
			//	DataClassRelations.CreateDataClassRelation(PersonnelOwner.RowId, PersonnelOwner.DataClass, PersonnelSet.GAPersonnel[0].PersonnelRowId, GADataClass.GAPersonnel, transaction);
				//add member classes
			//	Utils.StoreObject.AddMemberClasses(new GADataRecord(PersonnelSet.GAPersonnel[0].PersonnelRowId, GADataClass.GAPersonnel), transaction);
			
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
			return PersonnelSet;
		}

		// Tor 20050531 deleted 
		//public override GADataRecord SaveNew(System.Data.DataSet ds, GADataRecord Owner)
		//{
		//	PersonnelDS savedDs = SaveNewPersonnel((PersonnelDS)ds, Owner);
		//	return new GADataRecord(savedDs.GAPersonnel[0].PersonnelRowId, GADataClass.GAPersonnel);
		//}
		//
		//public override System.Data.DataSet GetNewRecord()
		//{
		//	return GetNewPersonnel();
		//}



		public static PersonnelDS UpdatePersonnel(PersonnelDS PersonnelSet)
		{
			return UpdatePersonnel(PersonnelSet, null);
		}
		public static PersonnelDS UpdatePersonnel(PersonnelDS PersonnelSet, GADataTransaction transaction)
		{
			return PersonnelDb.UpdatePersonnel(PersonnelSet, transaction);
		}
	}
}
