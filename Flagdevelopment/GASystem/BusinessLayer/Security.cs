using System;
using System.Collections;
using System.Data;
using System.Reflection;
using GASystem.AppUtils;
using GASystem.GAExceptions;
using GASystem.DataAccess.Security;
using GASystem.DataModel;
using log4net;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Security.
	/// </summary>
	public class Security
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(Security));

		public Security()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static RolesDS GetAllSecurityRoles()
		{
			RolesDS returnRolesData = null;

			try
			{
				returnRolesData = GASecurityDb_new.GetAllSecurityRoles();
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			return returnRolesData;
		}

		public static RolesDS GetAllSecurityRolesForArcLink(GADataClass Owner, GADataClass Member)
		{
			RolesDS returnRolesData = new RolesDS();
			try
			{
				DataSet tmpRolesDataSet = GASecurityDb_new.GetAllSecurityRolesForArcLink(Owner, Member);

				if (tmpRolesDataSet.Tables[0].Rows.Count==0) 
					return returnRolesData;

				//dataset contains one column containing a semicolon-separated list for roleIds (eg. ;3;3;6;7;8;)
				String roleList = tmpRolesDataSet.Tables[0].Rows[0][0].ToString();
				ArrayList roleArray = new ArrayList();
				bool addAllRole = false;
				bool addCompanyUserRole = false;

				foreach (String role in roleList.Split( new char[] {';'} )) 
				{
					if (role == "" || role==null) 
						continue;
					
					if (role.Equals("-1"))
						addAllRole = true;

					if (role.Equals("-2"))
						addCompanyUserRole = true;

					roleArray.Add(role);
				}
				returnRolesData = GASecurityDb_new.GetSecurityRolesFromRoleList((string[])roleArray.ToArray(typeof(string)));

				//Special handling for the "ALL" role. This role is not defined in the database
				if (addAllRole) 
				{
					RolesDS.RolesRow rolesrow = returnRolesData.Roles.NewRolesRow();
					rolesrow.RoleID = -1;
					rolesrow.RoleName = AppUtils.Localization.GetGuiElementText("All");
					returnRolesData.Roles.AddRolesRow(rolesrow);
				}

				//Special handling for the "CompanyUser" role. This role is not defined in the database
				if (addCompanyUserRole) 
				{
					RolesDS.RolesRow rolesrow = returnRolesData.Roles.NewRolesRow();
					rolesrow.RoleID = -2;
					rolesrow.RoleName = AppUtils.Localization.GetGuiElementText("Company User");
					returnRolesData.Roles.AddRolesRow(rolesrow);
				}

			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			return returnRolesData;

		} 

		public static RolesDS GetAllSecurityRolesForDataClass(GADataClass DataClass)
		{
			RolesDS returnRolesData = new RolesDS();
			try
			{
				DataSet tmpRolesDataSet = GASecurityDb_new.GetAllSecurityRolesForDataClass(DataClass);

				if (tmpRolesDataSet.Tables[0].Rows.Count==0) 
					return returnRolesData;

				//dataset contains one column containing a semicolon-separated list for roleIds (eg. ;3;3;6;7;8;)
				String roleList = tmpRolesDataSet.Tables[0].Rows[0][0].ToString();
				ArrayList roleArray = new ArrayList();
				bool addAllRole = false;

				foreach (String role in roleList.Split( new char[] {';'} )) 
				{
					if (role == "" || role==null) 
						continue;
					
					if (role.Equals("-1"))
						addAllRole = true;

					roleArray.Add(role);
				}
				returnRolesData = GASecurityDb_new.GetSecurityRolesFromRoleList((string[])roleArray.ToArray(typeof(string)));

				//Special handling for the "ALL" role. This role is not defined in the database
				if (addAllRole) 
				{
					RolesDS.RolesRow rolesrow = returnRolesData.Roles.NewRolesRow();
					rolesrow.RoleID = -1;
					rolesrow.RoleName = AppUtils.Localization.GetGuiElementText("All");
					returnRolesData.Roles.AddRolesRow(rolesrow);
				}

			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			return returnRolesData;

		} 

		public static DataRecordRolePermissionsDS GetSecurityRolesAccessForArcLink(GADataClass Owner, GADataClass Member)
		{
			DataRecordRolePermissionsDS rolePermissionsData = new DataRecordRolePermissionsDS();
			try
			{
				RolesDS rolesSet = GetAllSecurityRolesForArcLink(Owner, Member);
				foreach (RolesDS.RolesRow row in rolesSet.Tables[0].Rows)
				{
					String role =  row.RoleID.ToString();
					DataRecordRolePermissionsDS tmpDataSet =  GASecurityDb_new.ArcLinkAccessForRole(Owner, Member, role);
					rolePermissionsData.DataRecordRolePermissions.ImportRow(tmpDataSet.DataRecordRolePermissions[0]);
				}
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			return rolePermissionsData;
		}

		public static DataRecordRolePermissionsDS GetSecurityRolesAccessForDataClass(GADataClass DataClass)
		{
			DataRecordRolePermissionsDS rolePermissionsData = new DataRecordRolePermissionsDS();
			try
			{
				RolesDS rolesSet = GetAllSecurityRolesForDataClass(DataClass);
				foreach (RolesDS.RolesRow row in rolesSet.Tables[0].Rows)
				{
					String role =  row.RoleID.ToString();
					DataRecordRolePermissionsDS tmpDataSet =  GASecurityDb_new.DataRecordAccessForRole(DataClass, role);
					rolePermissionsData.DataRecordRolePermissions.ImportRow(tmpDataSet.DataRecordRolePermissions[0]);
				}
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			return rolePermissionsData;
		}

		public static void UpdateArcLinkRolePermissions(DataRecordRolePermissionsDS RolePermissions, GADataClass Owner, GADataClass Member)
		{
			try
			{
				GASecurityDb_new.UpdateArcLinkRolePermissions(RolePermissions, Owner, Member, null);
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
		}

		public static void UpdateDataRecordRolePermissions(DataRecordRolePermissionsDS RolePermissions, GADataClass DataClass)
		{
			try
			{
				GASecurityDb_new.UpdateDataRecordRolePermissions(RolePermissions, DataClass, null);
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
		}

		public static void UpdateArcLinkRolePermissions(GADataClass Owner, GADataClass Member, string RoleId, bool HasRead, bool HasUpdate, bool HasCreate, bool HasDelete)
		{
			try
			{
				DataRecordRolePermissionsDS rolePermissionsData = Security.GetSecurityRolesAccessForArcLink(Owner, Member);
				DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row = (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow) rolePermissionsData.DataRecordRolePermissions.Rows.Find(RoleId);
				if (row==null)
					return;
				row.HasRead = HasRead;
				row.HasUpdate = HasUpdate;
				row.HasCreate = HasCreate;
				row.HasDelete = HasDelete;
				DataAccess.Security.GASecurityDb_new.UpdateArcLinkRolePermissions(rolePermissionsData, Owner, Member, null);		
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			
		}

		public static void UpdateDataRecordRolePermissions(GADataClass DataClass, string RoleId, bool HasRead, bool HasUpdate, bool HasCreate, bool HasDelete)
		{
			try
			{
				DataRecordRolePermissionsDS rolePermissionsData = Security.GetSecurityRolesAccessForDataClass(DataClass);
				DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row = (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow) rolePermissionsData.DataRecordRolePermissions.Rows.Find(RoleId);
				row.HasRead = HasRead;
				row.HasUpdate = HasUpdate;
				row.HasCreate = HasCreate;
				row.HasDelete = HasDelete;
				DataAccess.Security.GASecurityDb_new.UpdateDataRecordRolePermissions(rolePermissionsData, DataClass, null);		
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			
		}


		public static void AddArcLinkRolePermissions(GADataClass Owner, GADataClass Member, string RoleId, bool HasRead, bool HasUpdate, bool HasCreate, bool HasDelete)
		{
			try
			{
				DataRecordRolePermissionsDS rolePermissionsData = Security.GetSecurityRolesAccessForArcLink(Owner, Member);
				DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row = rolePermissionsData.DataRecordRolePermissions.NewDataRecordRolePermissionsRow();
				row.HasRead = HasRead;
				row.HasUpdate = HasUpdate;
				row.HasCreate = HasCreate;
				row.HasDelete = HasDelete;
				row.RoleId = RoleId;
				rolePermissionsData.DataRecordRolePermissions.Rows.Add(row);
				DataAccess.Security.GASecurityDb_new.UpdateArcLinkRolePermissions(rolePermissionsData, Owner, Member, null);			
			}
			catch (System.Data.ConstraintException ex)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("Security.AddDataRecordRolePermissions_1"), ex);
				GAex.SetDebugMessage((MethodInfo) MethodInfo.GetCurrentMethod(), new object[] {Owner, Member, RoleId, HasRead, HasUpdate, HasCreate, HasDelete});
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			catch (Exception ex1)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("Security.AddDataRecordRolePermissions"), ex1);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
		}

		public static void AddDataRecordRolePermissions(GADataClass DataClass, string RoleId, bool HasRead, bool HasUpdate, bool HasCreate, bool HasDelete)
		{
			try
			{
				DataRecordRolePermissionsDS rolePermissionsData = Security.GetSecurityRolesAccessForDataClass(DataClass);
				DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row = rolePermissionsData.DataRecordRolePermissions.NewDataRecordRolePermissionsRow();
				row.HasRead = HasRead;
				row.HasUpdate = HasUpdate;
				row.HasCreate = HasCreate;
				row.HasDelete = HasDelete;
				row.RoleId = RoleId;
				rolePermissionsData.DataRecordRolePermissions.Rows.Add(row);
				DataAccess.Security.GASecurityDb_new.UpdateDataRecordRolePermissions(rolePermissionsData, DataClass, null);			
			}
			catch (System.Data.ConstraintException ex)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("Security.AddDataRecordRolePermissions_1"), ex);
				GAex.SetDebugMessage((MethodInfo) MethodInfo.GetCurrentMethod(), new object[] {DataClass, RoleId, HasRead, HasUpdate, HasCreate, HasDelete});
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			catch (Exception ex1)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("Security.AddDataRecordRolePermissions"), ex1);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
		}


		public static bool IsGAAdministrator()
		{
			if (!GASecurityDb_new.GASecurityIsOn) return true;
			return GASecurityDb_new.IsGAAdministrator();
		}

        // Tor 20171115 Added 
        public static bool IsAdministrators()
        {
            if (!GASecurityDb_new.GASecurityIsOn) return true;
            return GASecurityDb_new.IsAdministrators();
        }
        
        public static bool IsGAHostAdministrator()
		{
			return true;
		}
	

		public static bool HasReadPermissionOnDataClassInContext(GADataRecord Context, GADataClass MemberClass) 
		{
            // Tor 201611 Security 20161122 
            GASecurityDb_new securityDb = new GASecurityDb_new(MemberClass, null);
            // Tor Rollback 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Context, MemberClass, null);
            // Tor 201611 Security 20161122			return securityDb.HasReadInContext(Context);
            return securityDb.HasReadInContext(Context);
        }

		private static bool RequiresAdditionalSecurity(GADataClass DataClass) 
		{
			//changed by JOF 211105, uses classdescription instead of traversing
			
			return AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataClass).ApplyAdditionalAccessControl;

			
			
//			//Traverse classes and determine if given class uses aditional accesscontrol
//			ClassDS classes = DataAccess.Class.GetAllClasses();
//			foreach (ClassDS.GAClassRow classRow in classes.GAClass.Rows) 
//			{
//				if (classRow.Class.Equals(DataClass.ToString()))
//					return classRow.ApplyAdditionalAccessControl;
//			}
//			return false;
		}


		/// <summary>
		/// Criteria for read:
		/// 1. User must have an engagement on the owner of the given record (or its parents) in order to get read permissions
		/// 2. User is member of a group that has readpermissions on the given record
		/// </summary>
		/// <param name="Record"></param>
		/// <returns></returns>
		public static bool HasReadPermission(GADataRecord Record)
		{
			if (!GASecurityDb_new.GASecurityIsOn) return true;
			if (IsGAAdministrator()) return true;
			
			//find the owner of this Record
            SuperClassDS ownerData = (new SuperClassDb()).GetSuperClassByMember(Record.RowId, Record.DataClass);
			GADataRecord owner = null;
			if (ownerData.GASuperClass.Rows.Count>0)
				owner = new GADataRecord(ownerData.GASuperClass[0].OwnerClassRowId, GADataRecord.ParseGADataClass(ownerData.GASuperClass[0].OwnerClass));

            // Tor 201611 Security 20161122 
            GASecurityDb_new securityDb = new GASecurityDb_new(Record.DataClass, null);
            // Tor 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Record, GADataRecord.ParseGADataClass("NullClass"), null);
            // Tor Rollback 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Record, Record.DataClass, null);

			bool requiresAdditionalSecurity = RequiresAdditionalSecurity(Record.DataClass);

			//If toplevel class (no owner) or requiresAdditionalSecurity, apply static security
			if (owner==null || requiresAdditionalSecurity) 
			{
                // Tor 20140819 check access with GASuperClassLinks instead of GAClass
                // return securityDb.HasStaticAccessToDataClass(GAAccessType.Read);
                // Tor 201611 Security 20161121
                return securityDb.HasStaticAccessToDataClass(GAAccessType.Read, Record, owner);
                // Tor 20170323 recover to line above return securityDb.HasStaticAccessToDataClass(GAAccessType.Read, Record.DataClass, owner.DataClass);
            }
			else 
			{
				//Security settings depends on context
				return securityDb.HasReadInContext(owner);
	
			}
		}

		/// <summary>
		/// Criteria for update:
		/// 1. User must have an engagement on the owner of the given record (or its parents) in order to get update permissions
		/// 2. User is member of a group that has updatepermissions on the given records dataclass
		/// </summary>
		/// <param name="Record"></param>
		/// <returns></returns>
		public static bool HasUpdatePermission(GADataRecord Record)
		{
			if (!GASecurityDb_new.GASecurityIsOn) return true;
			if (IsGAAdministrator()) 
				return true;

			//find the owner of this Record
			GADataRecord owner = DataClassRelations.GetOwner(Record);

            // Tor 201611 Security 20161122 
            GASecurityDb_new securityDb = new GASecurityDb_new(Record.DataClass, null);
            // Tor 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Record, GADataRecord.ParseGADataClass("NullClass"), null);
            // Tor Rollback 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Record, Record.DataClass, null);

            //bool requiresAdditionalSecurity = RequiresAdditionalSecurity(Record.DataClass);
			//If toplevel class (no owner) or requiresAdditionalSecurity, apply static security
            //if (owner==null || requiresAdditionalSecurity)
            if (owner == null || RequiresAdditionalSecurity(Record.DataClass)) 

			{
                // Tor 20140320 changed permission lookup from GAClass to GASuperClassLinks lookup - needs record and owner to check access to
                //				return securityDb.HasStaticAccessToDataClass(GAAccessType.Update);
                // Tor 201611 Security 20161121 
                return securityDb.HasStaticAccessToDataClass(GAAccessType.Update, Record, owner);
                // Tor 20170323 recover to line above return securityDb.HasStaticAccessToDataClass(GAAccessType.Update, Record.DataClass, owner.DataClass);

			}
			else 
			{
				//Security settings depends on context
				//return securityDb.HasUpdateInArcLink(owner);
				return securityDb.HasAccessInArcLink(Record, GAAccessType.Update);
			}
		}

		/// <summary>
		/// Criteria for update in context:
		/// 1. User must have an engagement in context or on any of its  parents
		/// 2. At least one of the roles aquired throught engagements in 1) must have updatepermissions on the given dataclass
		/// </summary>
		/// <param name="Owner">The context were we want to update</param>
		/// <param name="DataClass">The dataclass that we want to update</param>
		/// <returns></returns>
		public static bool HasUpdatePermissionInArcLink(GADataClass DataClass, GADataRecord Owner)
		{
			if (!GASecurityDb_new.GASecurityIsOn) return true;
			if (IsGAAdministrator()) return true;

            // Tor 201611 Security 20161122 
            GASecurityDb_new securityDb = new GASecurityDb_new(DataClass, null);
            // Tor Rollback 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Owner, DataClass, null);

//			bool requiresAdditionalSecurity = RequiresAdditionalSecurity(DataClass);

			//If toplevel class (no owner) or requiresAdditionalSecurity, apply static security
            if (Owner == null || RequiresAdditionalSecurity(DataClass))
//                if (Owner == null || requiresAdditionalSecurity)
                {
                // Tor 20140819 check permission in GASuperClassLinks in stead of in GAClass
				// return securityDb.HasStaticAccessToDataClass(GAAccessType.Update);
                return securityDb.HasStaticAccessToDataClass(GAAccessType.Update, DataClass, GADataRecord.ParseGADataClass(Owner.DataClass.ToString()));
			}
			else 
			{
				return securityDb.HasUpdateInArcLink(Owner);

				//Security settings depends on context
				//return securityDb.HasUpdateInContext(Owner);
	
			}
		}
		
		/// <summary>
		/// Criteria for update in context:
		/// 1. User must have an engagement in context or on any of its  parents
		/// 2. At least one of the roles aquired throught engagements in 1) must have updatepermissions on the given arclink
		/// </summary>
		/// <param name="Owner">The context were we want to update</param>
		/// <param name="DataClass">The dataclass that we want to update</param>
		/// <returns></returns>
		public static bool HasUpdatePermissionInContext(GADataClass DataClass, GADataRecord Owner)
		{
			if (!GASecurityDb_new.GASecurityIsOn) return true;
			if (IsGAAdministrator()) return true;

            // Tor 201611 Security 20161122 
            GASecurityDb_new securityDb = new GASecurityDb_new(DataClass, null);
            // Tor Rollback 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Owner, DataClass, null);

//			bool requiresAdditionalSecurity = RequiresAdditionalSecurity(DataClass);

			//If toplevel class (no owner) or requiresAdditionalSecurity, apply static security
            if (Owner == null || RequiresAdditionalSecurity(DataClass))
//                if (Owner == null || requiresAdditionalSecurity) 
			{
                // Tor 20140819 check permission in GASuperClassLinks instead of in GAClass
				// return securityDb.HasStaticAccessToDataClass(GAAccessType.Update);
                return securityDb.HasStaticAccessToDataClass(GAAccessType.Update, DataClass, GADataRecord.ParseGADataClass(Owner.DataClass.ToString()));
			}
			else 
			{
				return securityDb.HasUpdateInContext(Owner);

				//Security settings depends on context
				//return securityDb.HasUpdateInContext(Owner);
	
			}
		}

        /// <summary>
        /// Criteria for update in context:
        /// 1. User must have an engagement in context or on any of its  parents
        /// 2. At least one of the roles aquired throught engagements in 1) must have updatepermissions on the given arclink
        /// </summary>
        /// <param name="Owner">The context were we want to update</param>
        /// <param name="DataClass">The dataclass that we want to update</param>
        /// <returns></returns>
        public static bool HasUpdateWithinPermissionInContext(GADataClass DataClass, GADataRecord Owner)
        {
            if (!GASecurityDb_new.GASecurityIsOn) return true;
            if (IsGAAdministrator()) return true;

            // Tor 201611 Security 20161122 
            GASecurityDb_new securityDb = new GASecurityDb_new(DataClass, null);
            // Tor Rollback 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Owner, DataClass, null);

            return securityDb.HasUpdateWithinInContext(Owner);

        }


		/// <summary>
		/// Determine if user is allowed to create records of the given dataclass under the given owner. 
		/// 
		/// If the given owner is null,
		/// or the given dataclass requires additional security (configured in GAClass) the folling rule is used, we check if user
		/// has "static access" to the class. See method HasStaticAccessToClass for a definition "static access" <seealso cref="HasStaticAccessToDataClass"/>
		/// 
		/// If the owner is not null and the given dataclass does not require aditional security, not context security rules are applied:
		/// 1. User must have an engagement on the owner (or its parents) in order to get create permissions
		/// 2. One of the roles acuired through these engagements must match one of the roles given create access to the given dataclass
		/// 
		/// In order
		/// </summary>
		/// <param name="DataClass"></param>
		/// <returns></returns>
		public static bool HasCreatePermission(GADataClass DataClass, GADataRecord Owner)
		{
			if (!GASecurityDb_new.GASecurityIsOn || IsGAAdministrator()) return true;
			

			//Added by JOF backward compatibility. New toplevel is GAFlag. TODO: implement GAFlag as top in all code. Remove usage of null as top indicator

			if (Owner == null) 
				Owner = new GADataRecord(1, GADataClass.GAFlag);


			//for dataclasses that don't belong to a context (defined in SuperClassLinks), DNN userroles
			//controls access. Lookup this property in GAClass 
//			bool requiresAdditionalSecurity = RequiresAdditionalSecurity(DataClass);;

			//For toplevel classes, static usergroups (from DNN) controls access
            if (Owner == null || RequiresAdditionalSecurity(DataClass))
//                if (Owner == null || requiresAdditionalSecurity) 
			{
                // Tor 201611 Security 20161122 
                GASecurityDb_new securityDb = new GASecurityDb_new(DataClass, null);
                // Tor Rollback 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Owner, DataClass, null);
                // Tor 20140819 replace security lookup in GAClass for lookup in GASuperClassLinks
				// return securityDb.HasStaticAccessToDataClass(GAAccessType.Create);
                return securityDb.HasStaticAccessToDataClass(GAAccessType.Create, DataClass, GADataRecord.ParseGADataClass(Owner.DataClass.ToString()));
			}
			else 
			{
                // Tor 201611 Security 20161122 
                GASecurityDb_new securityDb = new GASecurityDb_new(DataClass, null);
                // Tor Rollback 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Owner, DataClass, null);

				return securityDb.HasCreateInArcLink(Owner);
				
				/*bool engagementIsOk = false;
				bool roleIsOk = false;

				string[] engagements = GASecurityDb_new.GetUserEngagementsAsArray();
				foreach (string engagement in engagements)
				{
					string ownerPathPart = Owner.DataClass.ToString() + "-" + Owner.RowId;
					if (engagement.IndexOf(ownerPathPart) > -1)
					{
						engagementIsOk = true;
						break;
					}
				}
				
				RolesDS rolesData = DataAccess.Security.GASecurityDb_new.GetUserGroups();
				DataRecordRolePermissionsDS recordRolePermissionsData = GetSecurityRolesAccessForDataClass(Owner.DataClass);
				foreach (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow rolePermissionRow in recordRolePermissionsData.DataRecordRolePermissions.Rows)
				{
					if (rolePermissionRow.RoleId.Equals("-1"))
						return true;

					foreach (RolesDS.RolesRow roleRow in rolesData.Roles.Rows)
					{
						if (roleRow.RoleID.ToString().Equals(rolePermissionRow.RoleId) && rolePermissionRow.HasCreate)
						{
							roleIsOk = true;
							break;
						}
					}
					if (roleIsOk)
						break;
				}*/
				//return  engagementIsOk && roleIsOk;
			}
		}

		/// <summary>
		/// Criteria for update:
		/// 1. User must have an engagement on the owner of the given record (or its parents) in order to get update permissions
		/// 2. User is member of a group that has deletepermissions on the given record
		/// </summary>
		/// <param name="Record"></param>
		/// <returns></returns>
		public static bool HasDeletePermission(GADataRecord Record)
		{
			if (!GASecurityDb_new.GASecurityIsOn) return true;
			if (IsGAAdministrator()) return true;

			//find the owner of this Record
            SuperClassDS ownerData = (new SuperClassDb()).GetSuperClassByMember(Record.RowId, Record.DataClass);
			GADataRecord owner = null;
			if (ownerData.GASuperClass.Rows.Count>0)
				owner = new GADataRecord(ownerData.GASuperClass[0].OwnerClassRowId, GADataRecord.ParseGADataClass(ownerData.GASuperClass[0].OwnerClass));

            // Tor 201611 Security 20161122 
            GASecurityDb_new securityDb = new GASecurityDb_new(Record.DataClass, null);
            // Tor 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Record, GADataRecord.ParseGADataClass("NullClass"), null);
            // Tor Rollback 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(Record, Record.DataClass, null);

//			bool requiresAdditionalSecurity = RequiresAdditionalSecurity(Record.DataClass);;

			//If toplevel class (no owner) or requiresAdditionalSecurity, apply static security
            if (owner == null || RequiresAdditionalSecurity(Record.DataClass))
//            if (owner==null || requiresAdditionalSecurity) 
			{
                // Tor 20140819 check permissions using GASuperClassLinks in stead of GAClass
				// return securityDb.HasStaticAccessToDataClass(GAAccessType.Delete);
                // Tor 201611 Security 20161121
                return securityDb.HasStaticAccessToDataClass(GAAccessType.Delete, Record, owner);
                // Tor 20170323 recover to line above return securityDb.HasStaticAccessToDataClass(GAAccessType.Delete, Record.DataClass, owner.DataClass);
			}
			else 
			{
				//Security settings depends on context
				return securityDb.HasDeleteInArcLink(owner);
			}
		}

		/// <summary>
		/// Checks whether there are any record specific access limitation to record
		/// </summary>
		/// <param name="DataRecord"></param>
		/// <returns></returns>
		public static bool HasEditPermissionOnRecord(GADataRecord DataRecord) 
		{
			BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(DataRecord.DataClass);
			return bc.HasPermissionToRecord(DataRecord.RowId);
		}

        /// <summary>
        /// determine if a the current user has create permissions with the given roleID
        /// </summary>
        public static bool HasCreatePermissionByRoleId(GADataClass dataClass, GADataRecord owner, int roleId)
        {
            if (owner == null)
            {
                owner = new GADataRecord(1, GADataClass.GAFlag);
            }

            GASecurityDb_new securityDb = new GASecurityDb_new(dataClass, null);

            SecurityRoles.RoleUtils roleUtils = new GASystem.BusinessLayer.SecurityRoles.RoleUtils(null);
            SecurityRoles.FlagRole[] userRoles = roleUtils.GetRolesForUser();

            if (userRoles.Length == 0)
                return false;

            //if owner record is on gaflag we are on the top level. need to handle this specially because a gaflag record has no owner
            string ownerPath = "/";
            if (owner.DataClass != GADataClass.GAFlag)
            {
                SuperClassDS sds = GASystem.BusinessLayer.DataClassRelations.GetByMember(owner);
                ownerPath = sds.GASuperClass[0].Path + owner.DataClass.ToString() + "-" + owner.RowId.ToString() + "/";
            }

            foreach (GASystem.BusinessLayer.SecurityRoles.FlagRole userRole in userRoles)
            {
                string contextPath = "/";

                if (userRole.Context != null)
                {
                    contextPath = userRole.Context.DataClass.ToString() + "-" + userRole.Context.RowId.ToString() + "/";
                }

                if (userRole.RoleId == roleId && ownerPath.IndexOf(contextPath) > -1)
                    return true;
            }

            return false;
        }
	}
}
