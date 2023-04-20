using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using GASystem.DataModel;
using GASystem.BusinessLayer;

namespace GASystem.DataAccess.Security
{
	/// <summary>
	/// Summary description for GASecurity.
	/// </summary>
	public class GASecurityDb
	{
		//Flag for turning of security (for development and debug purposes)
		public static readonly bool GASecurityIsOn = false;

		private enum GASecurityGroups {GAAdministrator};
		
		private static string _selectDataRecordAccessForRole = @"Select '' as RoleName, '{0}' as RoleId, ISNULL(CHARINDEX(';{0};', ';'+ReadRoles+';'), 0) as HasRead, 
																ISNULL(CHARINDEX(';{0};', ';'+UpdateRoles+';'), 0) as HasUpdate, 
																ISNULL(CHARINDEX(';{0};', ';'+CreateRoles+';'), 0) as HasCreate,
																ISNULL(CHARINDEX(';{0};', ';'+DeleteRoles+';'), 0) as HasDelete
																FROM GASuperClass WHERE MemberClass = '{1}' AND MemberClassRowId={2}";
		
	//	private static string _updatePermissions_old = @"UPDATE SuperClass SET {0}Roles={1} WHERE MemberClass = '{1}' AND MemberClassRowId={2}";
	
		private static string _selectPermissionsForMember = @"SELECT ReadRoles, UpdateRoles, CreateRoles, DeleteRoles FROM GASuperClass WHERE MemberClass = '{0}' AND MemberClassRowId={1}";
		private static string _updatePermissionsForMember = @"UPDATE GASuperClass SET ReadRoles='{0}', UpdateRoles='{1}', CreateRoles='{2}', DeleteRoles='{3}' WHERE MemberClass = '{4}' AND MemberClassRowId={5}";
		private static string _updatePermissionsForMemberCascading = @"UPDATE GASuperClass SET ReadRoles='{0}', UpdateRoles='{1}', CreateRoles='{2}', DeleteRoles='{3}' 
                                                                        WHERE (MemberClass = '{4}' 
                                                                        AND MemberClassRowId={5}) 
                                                                        OR GASuperClass.ReadRoles = '{4}-{5}'
                                                                        OR GASuperClass.UpdateRoles = '{4}-{5}'
                                                                        OR GASuperClass.CreateRoles = '{4}-{5}'
                                                                        OR GASuperClass.DeleteRoles = '{4}-{5}'
                                                                        OR GASuperClass.TextFree1 = '{4}-{5}'
                                                                        OR GASuperClass.TextFree2 = '{4}-{5}'";
        //(gasuperclass.path like '%{4}-{5}/%') has been replaced in the statement above";

		private static string _selectPersonnelEmployments = @"select OwnerClass + '-' + CAST(OwnerClassRowId as nvarchar(20))  from GAEmployment INNER JOIN GASuperClass ON EmploymentRowId=MemberClassRowId WHERE Personnel={0}";

		//Gitt en person (1) og en dataklasse (2), list rowId og roleId for alle rows den personen har tilgang til. 
		//
		// 

		private static string _selectAccessableRowIdAndRoleIds = 	
			@"SELECT GAsc.OwnerClassRowId AS AccessableRowId, RoleListsRowId
					FROM 
					(select Path, RoleListsRowId  from GAEmployment INNER JOIN GASuperClass ON EmploymentRowId=MemberClassRowId AND MemberClass='GAEmployment' WHERE Personnel={0}) EmploymentPaths 
					INNER JOIN GASuperClass GAsc ON GAsc.Path LIKE EmploymentPaths.Path + '%' WHERE GAsc.OwnerClass = '{1}'
					UNION
					SELECT GAsc.MemberClassRowId AS AccessableRowId, RoleListsRowId
					FROM 
					(select Path, RoleListsRowId   from GAEmployment INNER JOIN GASuperClass ON EmploymentRowId=MemberClassRowId AND MemberClass='GAEmployment' WHERE Personnel={0}) EmploymentPaths 
					INNER JOIN GASuperClass GAsc ON GAsc.Path LIKE EmploymentPaths.Path + '%' WHERE GAsc.MemberClass = '{1}'";


		//Gitt en person (1), dataklasse (2) og en kommaseparert liste oer rolleid'er list rowId for alle rows den personen har tilgang til and den gitte dataklassen
		/*@"SELECT GAsc.OwnerClassRowId AS AccessableRowId
		 FROM 
		(select Path, RoleListsRowId  from GAEmployment INNER JOIN GASuperClass ON EmploymentRowId=MemberClassRowId AND MemberClass='GAEmployment' WHERE Personnel={0} AND RoleListsRowId IN ({2})) EmploymentPaths 
		INNER JOIN GASuperClass GAsc ON GAsc.Path LIKE EmploymentPaths.Path + '%' WHERE GAsc.OwnerClass = '{1}'
		UNION
		SELECT   GAsc.MemberClassRowId AS AccessableRowId
		 FROM 
		(select Path, RoleListsRowId   from GAEmployment INNER JOIN GASuperClass ON EmploymentRowId=MemberClassRowId AND MemberClass='GAEmployment' WHERE Personnel={0} AND RoleListsRowId IN ({2})) EmploymentPaths 
		INNER JOIN GASuperClass GAsc ON GAsc.Path LIKE EmploymentPaths.Path + '%' WHERE GAsc.MemberClass = '{1}' ";
		*/
		

		//Join a given query against the AccessControl, and only return rows where the is a match
		//This will only return rows where the user has roles
		/*	private static string _selectAllSecure = @"SELECT * FROM ({0}) MyQuery
								 LEFT OUTER JOIN GAAccessControl ON GAAccessControl.GARowId = MyQuery.{2}RowId
								 WHERE GAAccessControl.DNNRole IN ({3})";
	*/
		/*		private static string _selectAccessableRowId = @" SELECT * FROM ({0}) MyQuery INNER JOIN (SELECT DISTINCT dbo.GASuperClass.MemberClassRowId as AccessableRowId
																 FROM       dbo.GASuperClass inner join dbo.GAAccessControl on (dbo.GASuperClass.Path LIKE dbo.GAAccessControl.Path + '%' OR (MemberClass=GAClass AND MemberClassRowId=GARowId))
																WHERE     (dbo.GASuperClass.MemberClass = N'{1}' AND dbo.GASuperClass.Path LIKE '%{3}%'
																			AND dbo.GAAccessControl.DnnRole IN ({4})) 
																			 ) AccessQuery ON AccessQuery.AccessableRowId=MyQuery.{2}RowId ";
		*/



		public GASecurityDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static IGASecurityProvider GetSecurityProvider()
		{
			return new DNNSecurityProvider(DataUtils.getDnnConnectionString()); 
		}

		public static bool IsGAAdministrator()
		{
			if (!GASecurityIsOn) return true;
			return IsUserInGroupName(GASecurityGroups.GAAdministrator.ToString());
		}

		public static bool IsUserInGroupId(string GroupId)
		{
			if (!GASecurityIsOn) return true;
			RolesDS rolesData = GetUserGroups();
			foreach (RolesDS.RolesRow row in rolesData.Roles.Rows)
			{
				if (row.RoleID.ToString().Equals(GroupId)) 
					return true;
			}
			return false;
		}

		public static bool IsUserInGroupName(string GroupName)
		{
			if (!GASecurityIsOn) return true;
			RolesDS rolesData = GetUserGroups();
			foreach (RolesDS.RolesRow row in rolesData.Roles.Rows)
			{
				if (row.RoleName.Equals(GroupName)) 
					return true;
			}
			return false;
		}

		public static string GetCurrentUserId()
		{
			return HttpContext.Current.User.Identity.Name;
		}

		/// <summary>
		/// Returns roles for the current user
		/// </summary>
		/// <returns></returns>
		public static RolesDS GetUserGroups()
		{
			IGASecurityProvider securityProvider = GetSecurityProvider();
			return securityProvider.GetUserGroups(GetCurrentUserId());
		}

		public static String[] GetUserEngagementsAsArray()
		{
			int personnelRowId = 0;
			string logoId = HttpContext.Current.User.Identity.Name;
			UserDS userData = User.GetUserByLogonId(logoId);
			if (userData.GAUser.Rows.Count>0)
				personnelRowId = userData.GAUser[0].PersonnelRowId;

			string sql = string.Format(_selectPersonnelEmployments, personnelRowId);
			DataSet employmentsData =  DataUtils.convertDataReaderToDataSet(DataUtils.executeSelect(sql));
			
			ArrayList employmentsArray = new ArrayList();
			foreach (DataRow row in employmentsData.Tables[0].Rows)
			{
				employmentsArray.Add(row[0].ToString());
			}
			
			return (string[]) employmentsArray.ToArray(typeof(string));
			//return new string[] {"GACompany-1"};
		}

		public static String GetUserRolesAsSqlArray()
		{
			RolesDS rolesData = GetUserGroups();
			ArrayList rolesArray = new ArrayList();
			foreach (RolesDS.RolesRow row in rolesData.Roles.Rows)
			{
				rolesArray.Add(row.RoleID);
			}
			return GetRolesAsSqlArray((string[])rolesArray.ToArray(typeof(string)));
			
		}

		public static String GetRolesAsSqlArray(String[] RoleList)
		{
			if (RoleList==null || RoleList.Length==0)
				return "''";

			StringBuilder roles = new StringBuilder();
			foreach (String role in RoleList)
			{
				roles.Append("'").Append(role).Append("', ");
			}
			roles.Remove(roles.Length-2, 2);
			return roles.ToString();
		}

		public static RolesDS GetAllSecurityRoles()
		{
			IGASecurityProvider securityProvider = GetSecurityProvider();
			return securityProvider.GetAllGroups();
			
		}

		//Returns a list of all secuirty roles for a DataRecord
		public static DataSet GetAllSecurityRolesForDataRecord(GADataRecord Record)
		{
			String sql = "select ( isNull(ReadRoles,'')  + isNull(UpdateRoles,'') + isNull(CreateRoles,'') + isNull(DeleteRoles,'') ) from GASuperClass where MemberClass='{0}' AND MemberClassRowId={1}";
			sql = string.Format(sql, Record.DataClass.ToString(), Record.RowId);
			return DataUtils.convertDataReaderToDataSet(DataUtils.executeSelect(sql));
		}

		//Returns a row containing read, update, create and delete. Each field contain a semicolon-separated list of roleIds with
		//the respective permission
		public static DataSet GetRolePermissionsForDataRecord(GADataRecord Record)
		{
			String sql = "select isNull(ReadRoles,''),  isNull(UpdateRoles,'') ,isNull(CreateRoles,''), isNull(DeleteRoles,'')  from GASuperClass where MemberClass='{0}' AND MemberClassRowId={1}";
			sql = string.Format(sql, Record.DataClass.ToString(), Record.RowId);
			return DataUtils.convertDataReaderToDataSet(DataUtils.executeSelect(sql));
		}

		/// <summary>
		/// Return a dataset containing roles from DNN. Dataset contain one row for each SecurityRoleId passed in parameter array
		/// </summary>
		/// <param name="SecurityRoles">Array of roleIds</param>
		/// <returns></returns>
		public static RolesDS GetSecurityRolesFromRoleList(string[] SecurityRoles)
		{
			RolesDS RolesData = GetSecurityProvider().GetAllGroups();
			ArrayList removeRow = new ArrayList();
			//Remove all rows that does not match roles in SecurityRoles parameter
			foreach (RolesDS.RolesRow row in RolesData.Roles.Rows)
			{
				bool foundIt = false;
				
				foreach (string roleId in SecurityRoles)
				{
					if (row.RoleID.ToString().Equals(roleId))
					{
						foundIt = true;
						break;
					}
    			}
				if (!foundIt)
					removeRow.Add(row);

			}

			foreach (RolesDS.RolesRow row in removeRow)
			{
				RolesData.Roles.RemoveRolesRow(row);
			}

			return RolesData;
		}

		/// <summary>
		/// Return a dataset containing permissions on a datarecord for å given role
		/// </summary>
		/// <param name="Record"></param>
		/// <param name="SecurityRole"></param>
		/// <returns></returns>
		public static DataRecordRolePermissionsDS DataRecordAccessForRole(GADataRecord Record, string SecurityRole)
		{
			String sql = string.Format(_selectDataRecordAccessForRole, SecurityRole, Record.DataClass.ToString(), Record.RowId);
			DataRecordRolePermissionsDS dataRecordRolePermissionsDate = new DataRecordRolePermissionsDS();
			String connection = DataUtils.getConnectionString();
			
			SqlDataAdapter da = new SqlDataAdapter(sql, connection);
			da.Fill(dataRecordRolePermissionsDate, "DataRecordRolePermissions");

			//The rolenames are stored in DNN. To avoid database join, we 'manually' populate the dataset with rolenames from DNN
			//(there should only be one row in dataRecordRoleAccessDate!)
			DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row = dataRecordRolePermissionsDate.DataRecordRolePermissions[0];
			RolesDS tmpDs = GetSecurityRolesFromRoleList(new string[] {row.RoleId});
			row.RoleName = tmpDs.Roles[0].RoleName;

			return dataRecordRolePermissionsDate;
		}

		public static void UpdateDataRecordRolePermissions(DataRecordRolePermissionsDS RolePermissions, GADataRecord Record, GADataTransaction Transaction)
		{
			//First build roleLists for permissions
			StringBuilder readRoles = new StringBuilder();
			StringBuilder updateRoles = new StringBuilder();
			StringBuilder createRoles = new StringBuilder();
			StringBuilder deleteRoles = new StringBuilder();
			foreach (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row in RolePermissions.DataRecordRolePermissions)
			{
				if (row.HasRead) readRoles.Append(row.RoleId).Append(";");
				if (row.HasUpdate) updateRoles.Append(row.RoleId).Append(";");
				if (row.HasCreate) createRoles.Append(row.RoleId).Append(";");
				if (row.HasDelete) deleteRoles.Append(row.RoleId).Append(";");
				
			}

			string sql = string.Format(_updatePermissionsForMemberCascading, readRoles.ToString(), updateRoles.ToString(), createRoles.ToString(), deleteRoles.ToString(), Record.DataClass.ToString(), Record.RowId);
			DataUtils.executeNoneQuery(sql, Transaction);
		}

		public static void CopyDataRecordRolePermissions(GADataRecord FromRecord, GADataRecord ToRecord, GADataTransaction Transaction)
		{
			string sqlGet = string.Format(_selectPermissionsForMember, FromRecord.DataClass.ToString(), FromRecord.RowId);
			DataSet fromData = DataUtils.convertDataReaderToDataSet(DataUtils.executeSelect(sqlGet, Transaction));
			if (fromData.Tables[0].Rows.Count==0)
				return;
			string readRoles = fromData.Tables[0].Rows[0]["ReadRoles"].ToString();
			string updateRoles = fromData.Tables[0].Rows[0]["UpdateRoles"].ToString();
			string createRoles = fromData.Tables[0].Rows[0]["CreateRoles"].ToString();
			string deleteRoles = fromData.Tables[0].Rows[0]["DeleteRoles"].ToString();
			string updateSql = string.Format(_updatePermissionsForMember, readRoles, updateRoles, createRoles, deleteRoles, ToRecord.DataClass.ToString(), ToRecord.RowId);
			DataUtils.executeNoneQuery(updateSql, Transaction);
		}

		/// <summary>
		/// Get all superclass members where a user has access given his roles and engagements
		/// This sql can be joined with any query containing a rowId in order to filter out row that user does not have
		/// readpermission on
		/// </summary>
		private static void GetAllMembersWithReadAccess(GADataClass DataClass)
		{
			string sql = @"SELECT * FROM dbo.GASuperClass WHERE dbo.GASuperClass.MemberClass = N'{0}'\n AND ({1})\n AND ({2}) ";
			
			
			string[] engagements = GetUserEngagementsAsArray(); //new string[] {"GAProject-1", "GAProject-3"};
			string[] userRoles = GetUserGroupIdsAsArray(); // new string[] {"11","1"};

			StringBuilder pathConstraints = new StringBuilder();
			foreach (string engagement in engagements)
			{
				pathConstraints.Append("(Path + '/' + MemberClass + '-' + CAST(MemberClassRowId as nvarChar(20))) + '/'");
				pathConstraints.Append(" LIKE '%").Append(engagement).Append("/%' \n");
				pathConstraints.Append(" OR ");
			}
			pathConstraints.Remove(pathConstraints.Length-5,4); //remove last OR statement

			StringBuilder roleConstraints = new StringBuilder();
			foreach (string roleId in userRoles)
			{
				roleConstraints.Append("';'+dbo.GASuperClass.ReadRoles+';' LIKE '%;").Append(roleId).Append(";%' \n");
				roleConstraints.Append(" OR ");
			}
			roleConstraints.Remove(roleConstraints.Length-5,4); //remove last OR statement

			sql = string.Format(sql, DataClass.ToString(), pathConstraints.ToString(), roleConstraints.ToString());
			
		}

		

		private static string[] GetUserGroupIdsAsArray()
		{
			RolesDS userGroups = GetUserGroups();
			ArrayList groupsArray = new ArrayList();
			foreach (RolesDS.RolesRow row in userGroups.Roles)
				groupsArray.Add(row.RoleID);

			return (string[]) groupsArray.ToArray(typeof(string));
		}

/*
		private static void AddPermissionForDataRecord(GADataRecord Record, string RoleId, DataRecordPermission Permission)
		{
			DataSet permissionsSet = GetRolePermissionsForDataRecord(Record);
			int columnIndex = 0;
			if (permissionsSet == DataRecordPermission.Read)
			{
				columnIndex = permissionsSet.Tables[0].Columns.IndexOf("ReadRoles");
				
				
			}
			else if (permissionsSet == DataRecordPermission.Create)
			{
				columnIndex = permissionsSet.Tables[0].Columns.IndexOf("CreateRoles");
				
			}
			else if (permissionsSet == DataRecordPermission.Update)
			{
				columnIndex = permissionsSet.Tables[0].Columns.IndexOf("CreateRoles");
				
			}
			else if (permissionsSet == DataRecordPermission.Delete)
			{
				columnIndex = permissionsSet.Tables[0].Columns.IndexOf("DeleteRoles");
				
			}


			String roles = permissionsSet.Tables[0].Rows[0][columnIndex].ToString();
			roles = AddRoleToRoleList(roles, RoleId);
			permissionsSet.Tables[0].Rows[0][columnIndex] = roles;
			
			String updateSql = string.Format(_updatePermissions_old, Permission.ToString(), Record.DataClass.ToString(), Record.RowId);
		
		}

		private static string AddRoleToRoleList(string roleList, string roleId)
		{
			ArrayList roleArray = new ArrayList();
			foreach (String role in roleList.Split( new char[] {';'} )) 
			{
				if (role == "" || role==null) 
					continue;

				//if RoleId already exists in list, return list
				if (role == roleId)
					return roleList;

				roleArray.Add(role);
			}
			roleArray.Add(roleId);
			return ConvertRoleArrayToRoleList((string[])roleArray.ToArray(typeof(string)));
		
		}

		private static string ConvertRoleArrayToRoleList(string[] RoleArray)
		{
			StringBuilder roleList = new StringBuilder();
			foreach (string role in RoleArray)
			{
				roleList.Append(role).Append(";");
			}
			return roleList.ToString();
		}

		private enum DataRecordPermission {Read, Create, Update, Delete};*/
	}

	
}

