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
	/// Summary description for Security.
	/// </summary>
	public class Security_new
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(Security_new));

		public Security_new()
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

	/*	public static RolesDS GetAllSecurityRolesForDataRecord(GADataRecord Record)
		{
			RolesDS returnRolesData = new RolesDS();
			try
			{
				DataSet tmpRolesDataSet = GASecurityDb_new.GetAllSecurityRolesForDataClass(Record.DataClass);

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
				returnRolesData = GASecurityDb_new.GetSecurityRolesFromRoleList((string[])roleArray.ToArray(typeof(string)));
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			return returnRolesData;

		}*/ 

		public static RolesDS GetAllSecurityRolesForDataClass(GADataClass DataClass)
		{
			RolesDS returnRolesData = new RolesDS();
			try
			{
				DataSet tmpRolesDataSet = GASecurityDb_new.GetAllSecurityRolesForDataClass(DataClass);

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
				returnRolesData = GASecurityDb_new.GetSecurityRolesFromRoleList((string[])roleArray.ToArray(typeof(string)));
			}
			catch (Exception e)
			{
				GAException GAex = new GAException(Localization.GetExceptionMessage("StandardGAErrorMessage"), e);
				_logger.Error(GAex.Message, GAex);
				throw GAex;
			}
			return returnRolesData;

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


	/*	public static DataRecordRolePermissionsDS GetSecurityRolesAccessForDataRecord(GADataRecord Record)
		{
			DataRecordRolePermissionsDS rolePermissionsData = new DataRecordRolePermissionsDS();
			try
			{
				RolesDS rolesSet = GetAllSecurityRolesForDataRecord(Record);
				foreach (RolesDS.RolesRow row in rolesSet.Tables[0].Rows)
				{
					String role =  row.RoleID.ToString();
					DataRecordRolePermissionsDS tmpDataSet =  GASecurityDb_new.DataRecordAccessForRole(Record.DataClass, role);
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
		}*/

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

		public static void UpdateDataRecordRolePermissions(GADataClass DataClass, string RoleId, bool HasRead, bool HasUpdate, bool HasCreate, bool HasDelete)
		{
			try
			{
				DataRecordRolePermissionsDS rolePermissionsData = Security_new.GetSecurityRolesAccessForDataClass(DataClass);
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

		public static void AddDataRecordRolePermissions(GADataClass DataClass, string RoleId, bool HasRead, bool HasUpdate, bool HasCreate, bool HasDelete)
		{
			try
			{
				DataRecordRolePermissionsDS rolePermissionsData = Security_new.GetSecurityRolesAccessForDataClass(DataClass);
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
			if (!GASecurityDb_new.GASecurityIsOn) return true;
			if (IsGAAdministrator()) 
				return true;

			RolesDS rolesData = DataAccess.Security.GASecurityDb_new.GetUserGroups();
			DataRecordRolePermissionsDS recordRolePermissionsData = GetSecurityRolesAccessForDataClass(Record.DataClass);
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
			if (!GASecurityDb_new.GASecurityIsOn) return true;
			if (IsGAAdministrator()) 
				return true;

			RolesDS rolesData = DataAccess.Security.GASecurityDb_new.GetUserGroups();
			DataRecordRolePermissionsDS recordRolePermissionsData = GetSecurityRolesAccessForDataClass(Record.DataClass);
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
			if (!GASecurityDb_new.GASecurityIsOn) return true;
			if (IsGAAdministrator()) 
				return true;

			bool engagementIsOk = false;
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
			if (!GASecurityDb_new.GASecurityIsOn) return true;
			if (IsGAAdministrator()) 
				return true;

			RolesDS rolesData = DataAccess.Security.GASecurityDb_new.GetUserGroups();
			DataRecordRolePermissionsDS recordRolePermissionsData = GetSecurityRolesAccessForDataClass(Record.DataClass);
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
