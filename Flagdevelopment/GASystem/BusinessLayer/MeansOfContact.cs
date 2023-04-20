using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for MeansOfContact.
	/// </summary>
	public class MeansOfContact : BusinessClass
	{
		public MeansOfContact()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAMeansOfContact;
		}

        // Tor 20161028 use standard update in : BusinessClass
        //public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
        //{
        //    return UpdateMeansOfContact((MeansOfContactDS)ds, transaction);
        //}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetMeansOfContactByMeansOfContactRowId(RowId);
		}

		public static MeansOfContactDS GetAllMeansOfContacts()
		{
			return MeansOfContactDb.GetAllMeansOfContacts();
		}
	

		public static MeansOfContactDS GetMeansOfContactsByOwner(GADataRecord MeansOfContactOwner)
		{
			return  MeansOfContactDb.GetMeansOfContactsByOwner(MeansOfContactOwner.RowId, MeansOfContactOwner.DataClass);
		}

		public static MeansOfContactDS GetMeansOfContactsByOwnerAndDeviceTypeId(int OwnerRowId, GADataClass OwnerClass, string DeviceType ) 
		{
			int deviceTypeId = Lists.GetListsRowIdByCategoryAndKey("DT", DeviceType);
			return MeansOfContactDb.GetMeansOfContactsByOwnerAndDeviceTypeId(OwnerRowId, OwnerClass, deviceTypeId);

		}

        public static string GetContactAddressByOwnerAndDeviceTypeId(int OwnerRowId, GADataClass OwnerClass, string DeviceType, GADataTransaction transaction) 
		{
			int deviceTypeId = Lists.GetListsRowIdByCategoryAndKey("DT", DeviceType);
			return MeansOfContactDb.GetContactAddressByOwnerAndDeviceTypeId(OwnerRowId, OwnerClass, deviceTypeId, transaction);
		}

        // Tor 20160331 method for retrieving eMail address by personnelRowId
        public static string GetEmailContactAddressByPersonnelRowId(int OwnerRowId, GADataTransaction transaction)
        {
            string DeviceType = "e-mail 1";
            int deviceTypeId = Lists.GetListsRowIdByCategoryAndKey("DT", DeviceType);
            return MeansOfContactDb.GetContactAddressByOwnerAndDeviceTypeId(OwnerRowId, DataModel.GADataClass.GAPersonnel, deviceTypeId, transaction);
        }

        public static MeansOfContactDS GetMeansOfContactByMeansOfContactRowId(int MeansOfContactRowId)
		{
			return MeansOfContactDb.GetMeansOfContactByMeansOfContactRowId(MeansOfContactRowId);
		}

		public static MeansOfContactDS GetNewMeansOfContact()
		{
			MeansOfContactDS iDS = new MeansOfContactDS();
			GASystem.DataModel.MeansOfContactDS.GAMeansOfContactRow row = iDS.GAMeansOfContact.NewGAMeansOfContactRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GAMeansOfContact.Rows.Add(row);
			return iDS;			
		}

		public static MeansOfContactDS SaveNewMeansOfContact(MeansOfContactDS MeansOfContactSet, GADataRecord MeansOfContactOwner)
		{
		//	if (MeansOfContactSet.GAMeansOfContact[0].IsDateAndTimeOfIncidentNull())
		//		MeansOfContactSet.GAMeansOfContact[0].DateAndTimeOfIncident = System.DateTime.Now;
			//Generate ReferenceId
		//	MeansOfContactSet.GAMeansOfContact[0].IncidentId = IdGenerator.GenerateId(GADataClass.GAMeansOfContact, MeansOfContactOwner, MeansOfContactSet.GAMeansOfContact[0].DateAndTimeOfIncident);
		
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				MeansOfContactSet = UpdateMeansOfContact(MeansOfContactSet, transaction);
				DataClassRelations.CreateDataClassRelation(MeansOfContactOwner.RowId, MeansOfContactOwner.DataClass, MeansOfContactSet.GAMeansOfContact[0].MeansOfContactRowId, GADataClass.GAMeansOfContact, transaction);
				//add member classes
			//	Utils.StoreObject.AddMemberClasses(new GADataRecord(MeansOfContactSet.GAMeansOfContact[0].MeansOfContactRowId, GADataClass.GAMeansOfContact), transaction);
			
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
			return MeansOfContactSet;
		}

		public static MeansOfContactDS UpdateMeansOfContact(MeansOfContactDS MeansOfContactSet)
		{
			return UpdateMeansOfContact(MeansOfContactSet, null);
		}
		public static MeansOfContactDS UpdateMeansOfContact(MeansOfContactDS MeansOfContactSet, GADataTransaction transaction)
		{
			return MeansOfContactDb.UpdateMeansOfContact(MeansOfContactSet, transaction);
		}
	}
}
