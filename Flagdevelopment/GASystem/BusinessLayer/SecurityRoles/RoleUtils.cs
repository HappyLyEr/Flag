using System;
using System.Collections;
using System.Collections.Generic;
using GASystem.AppUtils;
using GASystem.DataModel;
using System.Data;
using GASystem.DataAccess;
using GASystem.DataAccess.Security;

namespace GASystem.BusinessLayer.SecurityRoles
{
	/// <summary>
	/// Summary description for RoleUtils.
	/// </summary>
	public class RoleUtils
	{
		GADataTransaction _transaction = null;

		public RoleUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public RoleUtils(GADataTransaction Transaction) 
		{
			_transaction = Transaction;
		}

		public FlagRole[] GetRolesForUser() 
		{
            List<FlagRole> roleList = new List<FlagRole>();
			//add default role
			roleList.Add(new FlagRole(-1, null, _transaction)); 

			//int personnelId = User.GetPersonnelIdByLogonId(GASystem.AppUtils.GAUsers.GetUserId());

			UserDS uds = User.GetUserByLogonId(GASystem.AppUtils.GAUsers.GetUserId());
			if (uds.GAUser.Rows.Count == 0)
				throw new GAExceptions.GASecurityException("Current logged on user is not a valid Flag user");

            int personnelId = uds.GAUser[0].PersonnelRowId;
            bool isCompanyUser = false;
            //add Flag Reader role
            if (!uds.GAUser[0].IsIsCompanyUserNull() && uds.GAUser[0].IsCompanyUser == true)
            {
                isCompanyUser = true;
                roleList.Add(new FlagRole(-2, null, _transaction));
            }

            //get all engagements for user
            //TODO rewrite using GAEmploymentPathView
            GASystem.DataModel.EmploymentPathViewDS eds = EmploymentPathView.GetEmploymentByPersonnelId(personnelId, _transaction);
            foreach (GASystem.DataModel.EmploymentPathViewDS.GAEmploymentPathViewRow row in eds.GAEmploymentPathView)
            {
                if (!row.IsRoleListsRowIdNull())
                {
                    GADataRecord dataRecord = new GADataRecord(row.OwnerClassRowId, GADataRecord.ParseGADataClass(row.OwnerClass));

                    FlagRole userRole = null;
                    if (isCompanyUser)
                        userRole = new FlagRole(row.RoleListsRowId, dataRecord, _transaction);
                    else
                    {
                        DateTime toDate = DateTime.MaxValue;
                        if (!row.IsToDateNull())
                            toDate = row.ToDate;

                        if (IsRoleToTakeDateRange(row.RoleListsRowId))
                        {
                            //replace the -1 role as it can't be filtered by date range. it will be added as FlagNonCompanyRole with date range
                            if (roleList.Count > 0 && roleList[0].RoleId == -1 && !(roleList[0] is FlagNonCompanyRole))
                            {
                                roleList.RemoveAt(0);
                                roleList.Add(new FlagNonCompanyRole(-1, dataRecord, _transaction, row.FromDate, toDate, null));
                            }
                        }

                        userRole = new FlagNonCompanyRole(row.RoleListsRowId, dataRecord, _transaction, row.FromDate, toDate, row.path);
                    }

                    roleList.Add(userRole);
                    //checkForLinkedRole(roleList, userRole);
                }
            }

            if (eds.GAEmploymentPathView.Rows.Count == 0 && isCompanyUser == false)
            {
                if (roleList.Count > 0 && roleList[0].RoleId == -1) //removes the all role if not a company user. it must have a personnel assignment
                {
                    roleList.RemoveAt(0);
                }
            }

            return roleList.ToArray();
		}

        /// <summary>
        /// if non-company user has a role marked as apply date range
        /// </summary>
        /// <param name="roleListsRowId"></param>
        /// <returns></returns>
        private bool IsRoleToTakeDateRange(int roleListsRowId)
        {
            ListsDS roleList = Lists.GetListsByListsRowId(roleListsRowId);

            if (roleList.GALists.Count > 0)
                return !roleList.GALists[0].IsSwitchFree1Null() && roleList.GALists[0].SwitchFree1;

            return false;
        }

		/// <summary>
		/// Checks whether the UserRole belongs to a context having many to many links to a differnet context. 
		/// Creates and adds a linked role is there is many to many link
		/// </summary>
		/// <param name="RoleList"></param>
		/// <param name="UserRole"></param>
		private void checkForLinkedRole(ArrayList RoleList, FlagRole UserRole) 
		{
			//Linked roles requires dates. Create a dummy FlagLinkedRole and call overrided method
			FlagLinkedRole dummyRole = new FlagLinkedRole(UserRole.RoleId, UserRole.Context, _transaction, System.DateTime.MinValue, System.DateTime.MaxValue);
			checkForLinkedRole(RoleList, dummyRole);
		}

		/// <summary>
		/// Checks whether the UserRole belongs to a context having many to many links to a differnet context. 
		/// Creates and adds a linked role is there is many to many link
		/// </summary>
		/// <param name="RoleList"></param>
		/// <param name="UserRole"></param>
		private void checkForLinkedRole(ArrayList RoleList, FlagLinkedRole UserRole) 
		{
			if (UserRole.Context == null)  //no context, no dataclasses to check
				return;
			
			//create a GADataRecordDate class from FlagLinkedRole context
			GADataRecordDate owner = new GADataRecordDate(UserRole.Context.RowId, UserRole.Context.DataClass);
			owner.StartDate = UserRole.DateFrom;
			owner.EndDate = UserRole.DateTo;


			ArrayList dataClassList = DataClassRelations.GetNextLevelManyToManyDataClasses(UserRole.Context.DataClass);
			foreach (string dataClass in dataClassList) 
			{
				ArrayList recordList = GetLinkedManyToManyRecords(ClassDefinition.GetClassDescriptionByGADataClass(dataClass), owner);
				foreach (GADataRecordDate dataRecord in recordList) 
				{
					//add a role and check for new linked records for each dataRecord found
					//linked roles keeps the same RoleId as it originator. But with new context and date restriction
					FlagLinkedRole newRole = new FlagLinkedRole(UserRole.RoleId, dataRecord, _transaction, dataRecord.StartDate, dataRecord.EndDate);
					RoleList.Add(newRole);
					checkForLinkedRole(RoleList, newRole);
				}
			}



		}

		/// <summary>
		/// Get a list of linked many to many datarecords linked 
		/// </summary>
		/// <param name="cd"></param>
		/// <param name="Owner"></param>
		/// <returns></returns>
		private ArrayList GetLinkedManyToManyRecords(ClassDescription cd, GADataRecordDate Owner) 
		{
			//select records of manytomany classes
			ArrayList dataRecords = new ArrayList();
			GADataClass dataClass = GADataRecord.ParseGADataClass(cd.DataClassName);
			BusinessClass bc = 	BusinessLayer.Utils.RecordsetFactory.Make(dataClass);
			
			DataSet ds = bc.GetManyToManyRecordsByOwnerAndTimeSpan(Owner, Owner.StartDate, Owner.EndDate, _transaction);
			//			bc.GetByOwner(
			//get field description for the linked table
			GASystem.AppUtils.FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(cd.ManyToManyField, cd.DataClassName);
			string lookupTable = fd.LookupTable;
			//string lookupTableKeyField = fd.LookupTableKey;

			//check for errors, if lookupTableKeyField does not exist or lookuptable is not defined return with empty data;
			if (lookupTable == string.Empty || !ds.Tables[dataClass.ToString()].Columns.Contains(cd.ManyToManyField))
				return dataRecords;

			GADataClass lookupTableDataClass = GADataRecord.ParseGADataClass(lookupTable);

			foreach (DataRow row in ds.Tables[dataClass.ToString()].Rows)
			{
				//build table of links, with date
				
				GADataRecordDate recordWithDate = new GADataRecordDate((int)row[cd.ManyToManyField], lookupTableDataClass);
				recordWithDate.StartDate = Owner.StartDate;
				recordWithDate.EndDate = Owner.EndDate;
				
				//set enddate
				if (cd.hasDateToField()) 
					if(ds.Tables[dataClass.ToString()].Columns.Contains(cd.DateToField) && row[cd.DateToField] != DBNull.Value) 
					{
						if ((DateTime)row[cd.DateToField] < Owner.EndDate)
							recordWithDate.EndDate = (DateTime)row[cd.DateToField];
					}

				//set startdate
				if (cd.hasDateFromField()) 
					if(ds.Tables[dataClass.ToString()].Columns.Contains(cd.DateFromField) && row[cd.DateFromField] != DBNull.Value) 
					{
						if ((DateTime)row[cd.DateFromField] > Owner.StartDate)
							recordWithDate.StartDate = (DateTime)row[cd.DateFromField];
					}
				dataRecords.Add(recordWithDate);
								
			}

			return dataRecords;
		}

	}
}
