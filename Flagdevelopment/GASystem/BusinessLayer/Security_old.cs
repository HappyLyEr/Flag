using System;
using System.Collections;
using System.Data;
using System.Reflection;
using GASystem.AppUtils;
using GASystem.GAExceptions;
using GASystem.DataAccess.Security;
using GASystem.DataModel;
using log4net;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Security_old.
	/// </summary>
	public class Security_old
	{
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Security_old));

		public Security_old()
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
				returnRolesData = GASecurityDb.GetAllSecurityRoles();
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			return returnRolesData;
		}

		public static RolesDS GetAllSecurityRolesForDataRecord(GADataRecord Record)
		{
			RolesDS returnRolesData = new RolesDS();
			try
			{
				DataSet tmpRolesDataSet = GASecurityDb.GetAllSecurityRolesForDataRecord(Record);

				if (tmpRolesDataSet.Tables[0].Rows.Count==0) 
					return returnRolesData;

				//dataset contains one column containing a semicolon-separated list for roleIds (eg. 3;3;6;7;8;)
				String roleList = tmpRolesDataSet.Tables[0].Rows[0][0].ToString();
				ArrayList roleArray = new ArrayList();
				foreach (String role in roleList.Split( new char[] {';'} )) 
				{
					if (role == "" || role==null) 
						continue;

					roleArray.Add(role);
				}
				returnRolesData = GASecurityDb.GetSecurityRolesFromRoleList((string[])roleArray.ToArray(typeof(string)));
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			return returnRolesData;

		}

		public static DataRecordRolePermissionsDS GetSecurityRolesAccessForDataRecord(GADataRecord Record)
		{
			DataRecordRolePermissionsDS rolePermissionsData = new DataRecordRolePermissionsDS();
			try
			{
				RolesDS rolesSet = GetAllSecurityRolesForDataRecord(Record);
				foreach (RolesDS.RolesRow row in rolesSet.Tables[0].Rows)
				{
					String role =  row.RoleID.ToString();
					DataRecordRolePermissionsDS tmpDataSet =  GASecurityDb.DataRecordAccessForRole(Record, role);
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

		public static void UpdateDataRecordRolePermissions(DataRecordRolePermissionsDS RolePermissions, GADataRecord Record)
		{
			try
			{
				GASecurityDb.UpdateDataRecordRolePermissions(RolePermissions, Record, null);
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
		}

		public static void UpdateDataRecordRolePermissions(GADataRecord Record, string RoleId, bool HasRead, bool HasUpdate, bool HasCreate, bool HasDelete)
		{
			try
			{
				DataRecordRolePermissionsDS rolePermissionsData = Security_old.GetSecurityRolesAccessForDataRecord(Record);
				DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row = (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow) rolePermissionsData.DataRecordRolePermissions.Rows.Find(RoleId);
				row.HasRead = HasRead;
				row.HasUpdate = HasUpdate;
				row.HasCreate = HasCreate;
				row.HasDelete = HasDelete;
				DataAccess.Security.GASecurityDb.UpdateDataRecordRolePermissions(rolePermissionsData, Record, null);		
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			
		}

		public static void AddDataRecordRolePermissions(GADataRecord Record, string RoleId, bool HasRead, bool HasUpdate, bool HasCreate, bool HasDelete)
		{
			try
			{
				DataRecordRolePermissionsDS rolePermissionsData = Security_old.GetSecurityRolesAccessForDataRecord(Record);
				DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row = rolePermissionsData.DataRecordRolePermissions.NewDataRecordRolePermissionsRow();
				row.HasRead = HasRead;
				row.HasUpdate = HasUpdate;
				row.HasCreate = HasCreate;
				row.HasDelete = HasDelete;
				row.RoleId = RoleId;
				rolePermissionsData.DataRecordRolePermissions.Rows.Add(row);
				DataAccess.Security.GASecurityDb.UpdateDataRecordRolePermissions(rolePermissionsData, Record, null);			
			}
			catch (System.Data.ConstraintException ex)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("Security_old.AddDataRecordRolePermissions_1"), ex);
				GAex.SetDebugMessage((MethodInfo) MethodInfo.GetCurrentMethod(), new object[] {Record, RoleId, HasRead, HasUpdate, HasCreate, HasDelete});
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			catch (Exception ex1)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("Security_old.AddDataRecordRolePermissions"), ex1);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
		}


		public static bool IsGAAdministrator()
		{
			if (!GASecurityDb.GASecurityIsOn) return true;
			return GASecurityDb.IsGAAdministrator();
		}

		public static bool IsGAHostAdministrator()
		{
			return true;
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
			if (!GASecurityDb.GASecurityIsOn) return true;
			if (IsGAAdministrator()) 
				return true;

			RolesDS rolesData = DataAccess.Security.GASecurityDb.GetUserGroups();
			DataRecordRolePermissionsDS recordRolePermissionsData = GetSecurityRolesAccessForDataRecord(Record);
			foreach (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow rolePermissionRow in recordRolePermissionsData.DataRecordRolePermissions.Rows)
			{
				foreach (RolesDS.RolesRow roleRow in rolesData.Roles.Rows)
				{
					if (roleRow.RoleID.ToString().Equals(rolePermissionRow.RoleId) && rolePermissionRow.HasRead)
					{
						return true;
					}
				}
			}
			return  false;
		}

		/// <summary>
		/// Criteria for update:
		/// 1. User must have an engagement on the owner of the given record (or its parents) in order to get update permissions
		/// 2. User is member of a group that has updatepermissions on the given record
		/// </summary>
		/// <param name="Record"></param>
		/// <returns></returns>
		public static bool HasUpdatePermission(GADataRecord Record)
		{
			if (!GASecurityDb.GASecurityIsOn) return true;
			if (IsGAAdministrator()) 
				return true;

			RolesDS rolesData = DataAccess.Security.GASecurityDb.GetUserGroups();
			DataRecordRolePermissionsDS recordRolePermissionsData = GetSecurityRolesAccessForDataRecord(Record);
			foreach (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow rolePermissionRow in recordRolePermissionsData.DataRecordRolePermissions.Rows)
			{
				foreach (RolesDS.RolesRow roleRow in rolesData.Roles.Rows)
				{
					if (roleRow.RoleID.ToString().Equals(rolePermissionRow.RoleId) && rolePermissionRow.HasUpdate)
					{
						return true;
					}
				}
			}
			return  false;
		}

		/// <summary>
		/// Criteria for create:
		/// 1. User must have an engagement on the owner (or its parents) in order to get create permissions
		/// 2. User is member of a group that has createpermissions on the given owner
		/// </summary>
		/// <param name="DataClass"></param>
		/// <returns></returns>
		public static bool HasCreatePermission(GADataClass DataClass, GADataRecord Owner)
		{
			if (!GASecurityDb.GASecurityIsOn) return true;
			if (IsGAAdministrator()) 
				return true;

			bool engagementIsOk = false;
			bool roleIsOk = false;

			string[] engagements = GASecurityDb.GetUserEngagementsAsArray();
			foreach (string engagement in engagements)
			{
				string ownerPathPart = Owner.DataClass.ToString() + "-" + Owner.RowId;
				if (engagement.IndexOf(ownerPathPart) > -1)
				{
					engagementIsOk = true;
					break;
				}
			}

			RolesDS rolesData = DataAccess.Security.GASecurityDb.GetUserGroups();
			DataRecordRolePermissionsDS recordRolePermissionsData = GetSecurityRolesAccessForDataRecord(Owner);
			foreach (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow rolePermissionRow in recordRolePermissionsData.DataRecordRolePermissions.Rows)
			{
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
			}
			return  engagementIsOk && roleIsOk;
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
			if (!GASecurityDb.GASecurityIsOn) return true;
			if (IsGAAdministrator()) 
				return true;

			RolesDS rolesData = DataAccess.Security.GASecurityDb.GetUserGroups();
			DataRecordRolePermissionsDS recordRolePermissionsData = GetSecurityRolesAccessForDataRecord(Record);
			foreach (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow rolePermissionRow in recordRolePermissionsData.DataRecordRolePermissions.Rows)
			{
				foreach (RolesDS.RolesRow roleRow in rolesData.Roles.Rows)
				{
					if (roleRow.RoleID.ToString().Equals(rolePermissionRow.RoleId) && rolePermissionRow.HasDelete)
					{
						return true;
					}
				}
			}
			return  false;
		}

	
	}
}
