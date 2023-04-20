using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Company.
	/// </summary>
	public class Company : BusinessClass
	{
		public Company()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GACompany;
		}

        // Tor 20161028 use standard update method in : BusinessClass
        //public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
        //{
        //    return UpdateCompany((CompanyDS)ds, transaction);
        //}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetCompanyByCompanyRowId(RowId);
		}

		public static CompanyDS GetAllCompanys()
		{
			return CompanyDb.GetAllCompanys();
		}
	

		public static CompanyDS GetCompanysByOwner(GADataRecord CompanyOwner)
		{
			return  CompanyDb.GetCompanysByOwner(CompanyOwner.RowId, CompanyOwner.DataClass);
		}

		public static CompanyDS GetCompanyByCompanyRowId(int CompanyRowId)
		{
			return CompanyDb.GetCompanyByCompanyRowId(CompanyRowId);
		}

		public static CompanyDS GetCompanyNoContentByCompanyRowId(int CompanyRowId)
		{
			return CompanyDb.GetCompanyNoContentByCompanyRowId(CompanyRowId);
		}


		public static CompanyDS GetNewCompany()
		{
			CompanyDS iDS = new CompanyDS();
			GASystem.DataModel.CompanyDS.GACompanyRow row = iDS.GACompany.NewGACompanyRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GACompany.Rows.Add(row);
			return iDS;
			
		}

		public static CompanyDS SaveNewCompany(CompanyDS CompanySet, GADataRecord CompanyOwner)
			// replaced
			//public static CompanyDS SaveNewCompany(CompanyDS CompanySet, GADataRecord CompanyOwner)
			//{
			//	CompanySet = UpdateCompany(CompanySet);
			//	DataClassRelations.CreateDataClassRelation(CompanyOwner.RowId, CompanyOwner.DataClass, CompanySet.GACompany[0].CompanyRowId, GADataClass.GACompany);
			//	return CompanySet;
			//}

		{
			//if (CompanySet.GACompany[0].IsDateAndTimeOfIncidentNull())
			//	CompanySet.GACompany[0].DateAndTimeOfIncident = System.DateTime.Now;
			//Generate ReferenceId
			//CompanySet.GACompany[0].IncidentId = IdGenerator.GenerateId(GADataClass.GACompany, CompanyOwner, CompanySet.GACompany[0].DateAndTimeOfIncident);
			
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				CompanySet = UpdateCompany(CompanySet, transaction);
				DataClassRelations.CreateDataClassRelation(CompanyOwner.RowId, CompanyOwner.DataClass, CompanySet.GACompany[0].CompanyRowId, GADataClass.GACompany, transaction);
				//add member classes
				//Utils.StoreObject.AddMemberClasses(new GADataRecord(CompanySet.GACompany[0].CompanyRowId, GADataClass.GACompany), transaction);
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
			return CompanySet;
		}

		public static CompanyDS UpdateCompany(CompanyDS CompanySet)
		{
			return UpdateCompany(CompanySet, null);
		}
		public static CompanyDS UpdateCompany(CompanyDS CompanySet, GADataTransaction transaction)
		{
			return CompanyDb.UpdateCompany(CompanySet, transaction);
		}
		

	}
}
