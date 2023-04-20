using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using GASystem.DataModel;
using GASystem.DataAccess;
using log4net;
using log4net.Appender;
using log4net.Config;
using GASystem.BusinessLayer;
using GASystem.AppUtils;

namespace GASystem.DataAccess.Security
{
	
	public enum GAAccessType {Read, Create, Update, Delete, UpdateWithin}

	public enum GASecurityGroups {GAAdministrator};
	
	/// <summary>
	/// Summary description for GASecurity.
	/// </summary>
	public class GASecurityDb_new
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(GASecurityDb_new));
		
		private GADataClass _dataClass;
		private GADataTransaction _transaction = null;


		//Flag for turning off security (for development and debug purposes)
		public static readonly bool GASecurityIsOn = true;



		//private enum GASecurityGroups {GAAdministrator}    changed to public enum by JOF 29.8.2006
		
		private static string _selectDataClassAccessForRole = @"Select '' as RoleName, '{0}' as RoleId, ISNULL(CHARINDEX(';{0};', ';'+ReadRoles+';'), 0) as HasRead, 
																ISNULL(CHARINDEX(';{0};', ';'+UpdateRoles+';'), 0) as HasUpdate, 
																ISNULL(CHARINDEX(';{0};', ';'+CreateRoles+';'), 0) as HasCreate,
																ISNULL(CHARINDEX(';{0};', ';'+DeleteRoles+';'), 0) as HasDelete
																FROM GAClass WHERE Class = '{1}'";
		
		private static string _selectArcLinkAccessForRole = @"Select '' as RoleName, '{0}' as RoleId, ISNULL(CHARINDEX(';{0};', ';'+ReadRoles+';'), 0) as HasRead, 
																ISNULL(CHARINDEX(';{0};', ';'+UpdateRoles+';'), 0) as HasUpdate, 
																ISNULL(CHARINDEX(';{0};', ';'+CreateRoles+';'), 0) as HasCreate,
																ISNULL(CHARINDEX(';{0};', ';'+DeleteRoles+';'), 0) as HasDelete
																FROM GASuperClassLinks WHERE OwnerClass = '{1}' AND MemberClass = '{2}'";
		

	//	private static string _updatePermissions_old = @"UPDATE SuperClass SET {0}Roles={1} WHERE MemberClass = '{1}' AND MemberClassRowId={2}";
	
		private static string _selectPermissionsForMember = @"SELECT ReadRoles, UpdateRoles, CreateRoles, DeleteRoles FROM GAClass WHERE Class = '{0}'";
		private static string _updatePermissionsForClass = @"UPDATE GAClass SET ReadRoles='{0}', UpdateRoles='{1}', CreateRoles='{2}', DeleteRoles='{3}' WHERE Class = '{4}'";

		private static string _updatePermissionsForArcLink = @"UPDATE GASuperClassLinks SET ReadRoles='{0}', UpdateRoles='{1}', CreateRoles='{2}', DeleteRoles='{3}' WHERE OwnerClass = '{4}' AND MemberClass='{5}'";
		
		
		private static string _selectPersonnelEmployments = @"select OwnerClass + '-' + CAST(OwnerClassRowId as nvarchar(20))  from GAEmployment INNER JOIN GASuperClass ON EmploymentRowId=MemberClassRowId WHERE Personnel={0}";

		private static string _selectPersonnelEmploymentRoles = @"select distinct rolelistsrowid as RoleId, GAListValue as RoleName, GAListDescription as [Description] from gaemployment inner join GALists on ListsRowId=rolelistsrowid where personnel={0}";


		private static string _selectAccessableRowId = @" SELECT * FROM ({0}) MyQuery INNER JOIN (
			SELECT GAsc.OwnerClassRowId AS AccessableRowId
			FROM 
			(select Path, RoleListsRowId  from GAEmployment INNER JOIN GASuperClass ON EmploymentRowId=MemberClassRowId AND MemberClass='GAEmployment' WHERE Personnel={1} AND RoleListsRowId IN ({4})) EmploymentPaths 
			INNER JOIN GASuperClass GAsc ON GAsc.Path LIKE EmploymentPaths.Path + '%' WHERE GAsc.OwnerClass = '{2}'
			UNION
			SELECT   GAsc.MemberClassRowId AS AccessableRowId
			FROM 
			(select Path, RoleListsRowId   from GAEmployment INNER JOIN GASuperClass ON EmploymentRowId=MemberClassRowId AND MemberClass='GAEmployment' WHERE Personnel={1} AND RoleListsRowId IN ({4})) EmploymentPaths 
			INNER JOIN GASuperClass GAsc ON GAsc.Path LIKE EmploymentPaths.Path + '%' WHERE GAsc.MemberClass = '{2}'
 			 ) AccessQuery ON AccessQuery.AccessableRowId=MyQuery.{3}RowId ";

		//1. Select all path's where a person has an employment with roles {4}. Filter result on a given owner {1}-{2}
		private static string _selectAccessUnderOwner = @"select Path, RoleListsRowId from GAEmployment INNER JOIN GASuperClass ON EmploymentRowId=MemberClassRowId AND MemberClass='GAEmployment' 
                                                            WHERE Personnel={0} 
                                                            AND RoleListsRowId IN ({3}) 
                                                            AND (GASuperClass.ReadRoles = '{3}-{2}'
                                                                 OR GASuperClass.UpdateRoles = '{3}-{2}'
                                                                 OR GASuperClass.CreateRoles = '{3}-{2}'
                                                                 OR GASuperClass.DeleteRoles = '{3}-{2}'
                                                                 OR GASuperClass.TextFree1 = '{3}-{2}'
                                                                 OR GASuperClass.TextFree2 = '{3}-{2}')";
        //(gasuperclass.path like '%{3}-{2}/%') has been replaced in the statement above";

        AppUtils.ClassDescription myClassDescription;

		public GASecurityDb_new(GADataClass DataClass, GADataTransaction Transaction)
		{
			_dataClass = DataClass;
			_transaction = Transaction;
            myClassDescription = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
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
			return GASystem.AppUtils.GAUsers.GetUserId();
			//return HttpContext.Current.User.Identity.Name;

			


		}

		/// <summary>
		/// Returns static usergroups for the current user. These groups are
		/// assoisiated with the user in the external authentication provider (in our case DNNNuke)
		/// In order to add groups to the user, use DNN useradmin and add add secuirtyRoles to the user
		/// </summary>
		/// <returns></returns>
		public static RolesDS GetUserGroups()
		{
			IGASecurityProvider securityProvider = GetSecurityProvider();
			return securityProvider.GetUserGroups(GetCurrentUserId());
		}

		/// <summary>
		/// Get a security provider. Default is a DNNSecurity Provider. 
		/// Returns a NoneWeb Security provider if not used on a HttpContext.
		/// </summary>
		/// <returns></returns>
		private static IGASecurityProvider GetSecurityProvider()
		{
			if (HttpContext.Current == null)
				return new NoneWebSecurityProvider();
				
			return new DNNSecurityProvider(DataUtils.getDnnConnectionString()); 
		}


		public static RolesDS GetAllUserEngagementRoles() 
		{
			int personnelRowId = GetPersonnelRowIdByLogonId(GetCurrentUserId());
			string sql = string.Format(_selectPersonnelEmploymentRoles, personnelRowId);
			RolesDS RolesData = new RolesDS();
			SqlDataAdapter da = new SqlDataAdapter(sql, DataUtils.GetConnection(null) );
			da.Fill(RolesData, "Roles");
			return RolesData;
		}

		private static int GetPersonnelRowIdByLogonId(string LogonId) 
		{
			int personnelRowId = 0;
			UserDS userData = User.GetUserByLogonId(LogonId);
			if (userData.GAUser.Rows.Count>0)
				personnelRowId = userData.GAUser[0].PersonnelRowId;

			return personnelRowId;
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

		

		private static String GetRolesAsSqlArray(String[] RoleList)
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

		
		/// <summary>
		/// Return a dataset with all GA roles. These roles are currently stored in the database table GALists (category='ER').
		/// NOTE: Rolenames with the follwoing pattern: -*- (starting with a dash and ending with a dash), are not returned!
		/// This allows us to define roles in GALists that are not part of the security concept. Typically used for roles like "-other-"
		/// 
		/// </summary>
		/// <returns></returns>
		public static RolesDS GetAllSecurityRoles()
		{
			string sql = @"SELECT ListsRowId AS RoleId, GAListValue AS RoleName, GAListDescription AS Description
							FROM GALists
							WHERE (GAListCategory = 'ER') AND (NOT (GAListValue LIKE '-%-'))";
			RolesDS rolesDSdata = new RolesDS();
			String connection = DataUtils.getConnectionString();
			
			SqlDataAdapter da = new SqlDataAdapter(sql, connection);
//			if (_transaction != null)
//				da.SelectCommand.Transaction = (SqlTransaction) _transaction.Transaction;
			da.Fill(rolesDSdata, "Roles");
			return rolesDSdata;
		}

		//Returns a list of all secuirty roles for a DataClass
		public static DataSet GetAllSecurityRolesForDataClass(GADataClass DataClass)
		{
			String sql = "select ( isNull(ReadRoles,'')  + isNull(UpdateRoles,'') + isNull(CreateRoles,'') + isNull(DeleteRoles,'') ) from GAClass where Class='{0}' ";
			sql = string.Format(sql, DataClass.ToString());
			return DataUtils.convertDataReaderToDataSet(DataUtils.executeSelect(sql));
		}

		//Returns a list of all secuirty roles for a DataClass
		public static DataSet GetAllSecurityRolesForArcLink(GADataClass Owner, GADataClass Member)
		{
			String sql = "select ( isNull(ReadRoles,'')  + isNull(UpdateRoles,'') + isNull(CreateRoles,'') + isNull(DeleteRoles,'') ) from GAsuperClassLinks where OwnerClass='{0}' and MemberClass='{1}'";
			sql = string.Format(sql, Owner.ToString(), Member.ToString());
			return DataUtils.convertDataReaderToDataSet(DataUtils.executeSelect(sql));
		}
		

		



		/// <summary>
		/// Given a set of GA secuirty role ids, return a dataset with full details on the given roles.
		/// Look in GetAllSecurityRoles() for details on where GA roles are stored. <seealso cref="GetAllSecurityRoles"/>
		/// </summary>
		/// <param name="SecurityRoles">Array of roleIds</param>
		/// <returns>A RolesDS containing roleinformation for the given roles</returns>
		public static RolesDS GetSecurityRolesFromRoleList(string[] SecurityRoles)
		{
			RolesDS RolesData = GetAllSecurityRoles();
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
		/// Return a dataset containing permissions on a SuperClassLinks(ArcLink) for å given role
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="SecurityRole"></param>
		/// <returns></returns>
		public static DataRecordRolePermissionsDS ArcLinkAccessForRole(GADataClass Owner, GADataClass Member, string SecurityRole)
		{
			String sql = string.Format(_selectArcLinkAccessForRole, SecurityRole, Owner.ToString(), Member.ToString());
			DataRecordRolePermissionsDS dataRecordRolePermissionsDate = new DataRecordRolePermissionsDS();
			String connection = DataUtils.getConnectionString();
			
			SqlDataAdapter da = new SqlDataAdapter(sql, connection);
			da.Fill(dataRecordRolePermissionsDate, "DataRecordRolePermissions");

			
			
			//The _selectDataClassAccessForRole does not retrieive rolenames. We must look this rolename up in the RoleList
			DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row = dataRecordRolePermissionsDate.DataRecordRolePermissions[0];
			
			if (!row.RoleId.Equals("-1") && !row.RoleId.Equals("-2")) 
			{
				RolesDS tmpDs = GetSecurityRolesFromRoleList(new string[] {row.RoleId});
				row.RoleName = tmpDs.Roles[0].RoleName;
				
			}
			else 
			{
				//Special handling for the All role (roleId = "-1" ) and Company User role (-2)
				if (row.RoleId.Equals("-2"))
					row.RoleName = AppUtils.Localization.GetGuiElementText("CompanyUser");
				else
					row.RoleName = AppUtils.Localization.GetGuiElementText("All");
			}

			return dataRecordRolePermissionsDate;
		}

		/// <summary>
		/// Return a dataset containing permissions on a dataclass for å given role
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="SecurityRole"></param>
		/// <returns></returns>
		public static DataRecordRolePermissionsDS DataRecordAccessForRole(GADataClass DataClass, string SecurityRole)
		{
			String sql = string.Format(_selectDataClassAccessForRole, SecurityRole, DataClass.ToString());
			DataRecordRolePermissionsDS dataRecordRolePermissionsDate = new DataRecordRolePermissionsDS();
			String connection = DataUtils.getConnectionString();
			
			SqlDataAdapter da = new SqlDataAdapter(sql, connection);
			da.Fill(dataRecordRolePermissionsDate, "DataRecordRolePermissions");

			
			
			//The _selectDataClassAccessForRole does not retrieive rolenames. We must look this rolename up in the RoleList
			DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row = dataRecordRolePermissionsDate.DataRecordRolePermissions[0];
			
			if (!row.RoleId.Equals("-1")) 
			{
				RolesDS tmpDs = GetSecurityRolesFromRoleList(new string[] {row.RoleId});
				row.RoleName = tmpDs.Roles[0].RoleName;
				
			}
			else 
			{
				//Special handling for the All role (roleId = "-1" )
				row.RoleName = AppUtils.Localization.GetGuiElementText("All");
			}

			return dataRecordRolePermissionsDate;
		}

		public static void UpdateArcLinkRolePermissions(DataRecordRolePermissionsDS RolePermissions, GADataClass Owner, GADataClass Member, GADataTransaction Transaction)
		{
			//First build roleLists for permissions. rolelists are ; separated
			StringBuilder readRoles = new StringBuilder();
			StringBuilder updateRoles = new StringBuilder();
			StringBuilder createRoles = new StringBuilder();
			StringBuilder deleteRoles = new StringBuilder();

			//rolelist must start with ;
			readRoles.Append(";");
			updateRoles.Append(";");
			createRoles.Append(";");
			deleteRoles.Append(";");

			foreach (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row in RolePermissions.DataRecordRolePermissions)
			{
				if (row.HasRead) readRoles.Append(row.RoleId).Append(";");
				if (row.HasUpdate) updateRoles.Append(row.RoleId).Append(";");
				if (row.HasCreate) createRoles.Append(row.RoleId).Append(";");
				if (row.HasDelete) deleteRoles.Append(row.RoleId).Append(";");
				
			}

			//Roles that are not set, (only contain ; ) are set to blank stringbuilder object
			if (readRoles.Length==1) readRoles = new StringBuilder();
			if (updateRoles.Length==1) updateRoles = new StringBuilder();
			if (createRoles.Length==1) createRoles = new StringBuilder();
			if (deleteRoles.Length==1) deleteRoles = new StringBuilder();

			//			if (readRoles.Length>0) readRoles.pr

			string sql = string.Format(_updatePermissionsForArcLink, readRoles.ToString(), updateRoles.ToString(), createRoles.ToString(), deleteRoles.ToString(), Owner.ToString(), Member.ToString());
			DataUtils.executeNoneQuery(sql, Transaction);
		}

		public static void UpdateDataRecordRolePermissions(DataRecordRolePermissionsDS RolePermissions, GADataClass DataClass, GADataTransaction Transaction)
		{
			//First build roleLists for permissions. rolelists are ; separated
			StringBuilder readRoles = new StringBuilder();
			StringBuilder updateRoles = new StringBuilder();
			StringBuilder createRoles = new StringBuilder();
			StringBuilder deleteRoles = new StringBuilder();

			//rolelist must start with ;
			readRoles.Append(";");
			updateRoles.Append(";");
			createRoles.Append(";");
			deleteRoles.Append(";");

			foreach (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row in RolePermissions.DataRecordRolePermissions)
			{
				if (row.HasRead) readRoles.Append(row.RoleId).Append(";");
				if (row.HasUpdate) updateRoles.Append(row.RoleId).Append(";");
				if (row.HasCreate) createRoles.Append(row.RoleId).Append(";");
				if (row.HasDelete) deleteRoles.Append(row.RoleId).Append(";");
				
			}

			//Roles that are not set, (only contain ; ) are set to blank stringbuilder object
			if (readRoles.Length==1) readRoles = new StringBuilder();
			if (updateRoles.Length==1) updateRoles = new StringBuilder();
			if (createRoles.Length==1) createRoles = new StringBuilder();
			if (deleteRoles.Length==1) deleteRoles = new StringBuilder();

//			if (readRoles.Length>0) readRoles.pr

			string sql = string.Format(_updatePermissionsForClass, readRoles.ToString(), updateRoles.ToString(), createRoles.ToString(), deleteRoles.ToString(), DataClass.ToString());
			DataUtils.executeNoneQuery(sql, Transaction);
		}

        //public ClassRoleAccessDS GetRoleAccessForDataClass()
        //{
        //    return GetRoleAccessForDataClass(_dataClass);
        //}

        //Returns a row from the GAClass table, containing a column for readroles, updateroles, createroles and deleteroles
        public ClassRoleAccessDS GetRoleAccessForDataClass(GADataClass DataClass)
        {
            ClassRoleAccessDS classRoleAccessData = new ClassRoleAccessDS();
            string sql = "SELECT ClassRowId, Class, isNull(ReadRoles,'') as ReadRoles,  isNull(UpdateRoles,'') as UpdateRoles ,isNull(CreateRoles,'') as CreateRoles, isNull(DeleteRoles,'') as DeleteRoles FROM GAClass WHERE Class ='{0}'";
            sql = string.Format(sql, DataClass.ToString());
            SqlConnection connection = DataUtils.GetConnection(_transaction);

            SqlDataAdapter da = new SqlDataAdapter(sql, connection);
            if (_transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)_transaction.Transaction;

            da.Fill(classRoleAccessData, "GAClassRoleAccess");

            return classRoleAccessData;
        }
		
        ///// <summary>
        ///// Get the union of all accessroles for a dataclass. The union is calculated by
        ///// recursivly traversing DataClass parents via GASuperclasslink
        ///// </summary>
        ///// <returns></returns>
        //public ClassRoleAccessDS GetRoleAccessForDataClassAndParents() 
        //{
        //    StringBuilder readRoles = new StringBuilder();
        //    StringBuilder createRoles = new StringBuilder();
        //    StringBuilder updateRoles = new StringBuilder();
        //    StringBuilder deleteRoles = new StringBuilder();
        //    readRoles.Append(";");
        //    createRoles.Append(";");
        //    updateRoles.Append(";");
        //    deleteRoles.Append(";");

        //    //ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClassAndParents(_dataClass);

        //    ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);



			
        //    foreach (ClassRoleAccessDS.GAClassRoleAccessRow row in classRoleAccessData.GAClassRoleAccess.Rows) 
        //    {
        //        foreach (String listElement in row.ReadRoles.ToString().Split( new char[] {';'} )) 
        //        {
        //            if (listElement == "" || listElement==null || readRoles.ToString().IndexOf(";"+listElement+";")>-1) 
        //                continue;
	
        //            readRoles.Append(listElement).Append(";");
        //        }

        //        foreach (String listElement in row.CreateRoles.ToString().Split( new char[] {';'} )) 
        //        {
        //            if (listElement == "" || listElement==null || createRoles.ToString().IndexOf(";"+listElement+";")>-1) 
        //                continue;
					
        //            createRoles.Append(listElement).Append(";");
        //        }

        //        foreach (String listElement in row.UpdateRoles.ToString().Split( new char[] {';'} )) 
        //        {
        //            if (listElement == "" || listElement==null || updateRoles.ToString().IndexOf(";"+listElement+";")>-1) 
        //                continue;
		
        //            updateRoles.Append(listElement).Append(";");
        //        }

        //        foreach (String listElement in row.DeleteRoles.ToString().Split( new char[] {';'} )) 
        //        {
        //            if (listElement == "" || listElement==null || deleteRoles.ToString().IndexOf(";"+listElement+";")>-1) 
        //                continue;
					
        //            deleteRoles.Append(listElement).Append(";");
        //        }
        //    }

        //    //Roles that are not set, (only contain ; ) are set to blank stringbuilder object
        //    if (readRoles.Length==1) readRoles = new StringBuilder();
        //    if (updateRoles.Length==1) updateRoles = new StringBuilder();
        //    if (createRoles.Length==1) createRoles = new StringBuilder();
        //    if (deleteRoles.Length==1) deleteRoles = new StringBuilder();

        //    ClassRoleAccessDS classRoleAccessDataAggr = new ClassRoleAccessDS();
        //    ClassRoleAccessDS.GAClassRoleAccessRow newrow = classRoleAccessDataAggr.GAClassRoleAccess.NewGAClassRoleAccessRow();
			
        //    newrow.Class = _dataClass.ToString();
        //    newrow.ReadRoles = readRoles.ToString();
        //    newrow.CreateRoles = createRoles.ToString();
        //    newrow.UpdateRoles = updateRoles.ToString();
        //    newrow.DeleteRoles = deleteRoles.ToString();

        //    classRoleAccessDataAggr.GAClassRoleAccess.Rows.Add(newrow);
        //    return classRoleAccessDataAggr;

        //}


        //private ClassRoleAccessDS GetRoleAccessForDataClassAndParents(GADataClass DataClass)
        //{
        //    ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClass(DataClass);

        //    //SuperClassDb.GetSuperClassByMember(DataClass);
        //    SuperClassLinksDS superClassData = SuperClassDb.GetSuperClassLinksByMember(DataClass);
        //    if (superClassData.GASuperClassLinks.Rows.Count > 0)
        //    {
        //        foreach (SuperClassLinksDS.GASuperClassLinksRow row in superClassData.GASuperClassLinks.Rows)
        //        {

        //            GADataClass parentClass = GADataRecord.ParseGADataClass(row.OwnerClass);

        //            //If member==parent, don't explore further up that path..
        //            if (parentClass == DataClass)
        //                continue;

        //            ClassRoleAccessDS tmpClassRoleAccessData = GetRoleAccessForDataClassAndParents(parentClass);
        //            foreach (ClassRoleAccessDS.GAClassRoleAccessRow accessRow in tmpClassRoleAccessData.GAClassRoleAccess.Rows)
        //            {
        //                classRoleAccessData.GAClassRoleAccess.ImportRow(accessRow);
        //            }

        //        }
        //    }
        //    return classRoleAccessData;

        //    /*ClassRoleAccessDS classRoleAccessData = new ClassRoleAccessDS();
        //    string sql = "SELECT ClassRowId, Class, isNull(ReadRoles,'') as ReadRoles,  isNull(UpdateRoles,'') as UpdateRoles ,isNull(CreateRoles,'') as CreateRoles, isNull(DeleteRoles,'') as DeleteRoles FROM GAClass WHERE Class ='{0}'";
        //    sql = string.Format(sql, DataClass.ToString());
        //    SqlConnection connection = DataUtils.GetConnection(_transaction);
			
        //    SqlDataAdapter da = new SqlDataAdapter(sql, connection);
        //    da.Fill(classRoleAccessData, "GAClassRoleAccess");

        //    return classRoleAccessData;*/
        //}

		public String GetReadRolesForDataClass(GADataClass dataClass) 
		{
            //String readRoles = "";
            //ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClassAndParents();
            //if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0)
            //    readRoles =  classRoleAccessData.GAClassRoleAccess[0].ReadRoles.ToString();
            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(dataClass);
            return cd.ReadRoles;
		}

		public String GetReadRolesForDataClassInContext(GADataRecord Context, GADataClass DataClass) 
		{
			String readRoles = "";
			ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClassInContext(Context, DataClass);
			if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0)
				readRoles =  classRoleAccessData.GAClassRoleAccess[0].ReadRoles.ToString();

			return readRoles;
		}


		public String GetUpdateRolesForDataClass(GADataClass dataClass) 
		{
            //String updateRoles = "";
            //ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClassAndParents();
            //if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0)
            //    updateRoles =  classRoleAccessData.GAClassRoleAccess[0].UpdateRoles.ToString();
            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(dataClass);
            return cd.UpdateRoles;
		}

		public String GetUpdateRolesForArcLink(GADataClass Owner, GADataClass DataClass) 
		{
			String updateRoles = "";
			ClassRoleAccessDS classRoleAccessData = GetRoleAccessForArcLink(Owner, DataClass);
			if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0)
				if (!classRoleAccessData.GAClassRoleAccess[0].IsUpdateRolesNull())
                    updateRoles =  classRoleAccessData.GAClassRoleAccess[0].UpdateRoles.ToString();

			return updateRoles;
		}

		public String GetUpdateRolesForDataClassInContext(GADataRecord Context, GADataClass DataClass) 
		{
			String updateRoles = "";
			ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClassInContext(Context, DataClass);
			if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0)
				updateRoles =  classRoleAccessData.GAClassRoleAccess[0].UpdateRoles.ToString();

			return updateRoles;
		}

		public String GetCreateRolesForDataClass(GADataClass dataClass) 
		{
            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(dataClass);
            return cd.CreateRoles;
		}

		public String GetCreateRolesForArcLink(GADataClass Owner, GADataClass DataClass) 
		{
			String createRoles = "";
			ClassRoleAccessDS classRoleAccessData = GetRoleAccessForArcLink(Owner, DataClass);
			if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0)
				createRoles =  classRoleAccessData.GAClassRoleAccess[0].CreateRoles.ToString();

			return createRoles;
		}
		
		public String GetCreateRolesForDataClassInContext(GADataRecord Context, GADataClass DataClass) 
		{
			String createRoles = "";
			ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClassInContext(Context, DataClass);
			if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0)
				createRoles =  classRoleAccessData.GAClassRoleAccess[0].CreateRoles.ToString();

			return createRoles;
		}

		

		public String GetDeleteRolesForDataClass(GADataClass dataClass) 
		{
            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(dataClass);
            return cd.DeleteRoles;
		}

		public String GetDeleteRolesForArcLink(GADataClass Owner, GADataClass DataClass) 
		{
			String deleteRoles = "";
			ClassRoleAccessDS classRoleAccessData = GetRoleAccessForArcLink(Owner, DataClass);
			if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0)
				deleteRoles =  classRoleAccessData.GAClassRoleAccess[0].DeleteRoles.ToString();

			return deleteRoles;
		}

        public String GetUpdateWithinRolesForArcLink(GADataClass Owner, GADataClass DataClass)
        {
            String updateWithinRoles = "";
            ClassRoleAccessDS classRoleAccessData = GetRoleAccessForArcLink(Owner, DataClass);
            if (classRoleAccessData.GAClassRoleAccess.Rows.Count > 0)
                updateWithinRoles = classRoleAccessData.GAClassRoleAccess[0].UpdateWithinRoles.ToString();

            return updateWithinRoles;
        }

		public String GetDeleteRolesForDataClassInContext(GADataRecord Context, GADataClass DataClass) 
		{
			String deleteRoles = "";
			ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClassInContext(Context, DataClass);
			if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0)
				deleteRoles =  classRoleAccessData.GAClassRoleAccess[0].DeleteRoles.ToString();

			return deleteRoles;
		}

		private static String ConvertToSqlNumberList(String semicolonseparatedList)  
		{
			if (null==semicolonseparatedList || semicolonseparatedList.Length==0) 
				return "";
		
			StringBuilder sqlArray = new StringBuilder();
			
			foreach (String listElement in semicolonseparatedList.Split( new char[] {';'} )) 
			{
				if (listElement == "" || listElement==null) //prohibit blank roles from being authorized
					continue;
				sqlArray.Append(listElement).Append(", ");
			}
			sqlArray.Remove(sqlArray.Length-2, 2);
			return sqlArray.ToString();
		}

	

		/// <summary>
		/// Get all superclass members where a user has access given his roles and engagements
		/// This sql can be joined with any query containing a rowId in order to filter out row that user does not have
		/// readpermission on
		/// </summary>
		private static void GetAllMembersWithReadAccess(GADataClass DataClass)
		{
			string sql = @"SELECT * FROM dbo.GASuperClass WHERE dbo.GASuperClass.MemberClass = N'{0}'\n AND ({1})\n ";
			
			
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

		/*	StringBuilder roleConstraints = new StringBuilder();
			foreach (string roleId in userRoles)
			{
				roleConstraints.Append("';'+dbo.GASuperClass.ReadRoles+';' LIKE '%;").Append(roleId).Append(";%' \n");
				roleConstraints.Append(" OR ");
			}
			roleConstraints.Remove(roleConstraints.Length-5,4); //remove last OR statement
*/
			sql = string.Format(sql, DataClass.ToString(), pathConstraints.ToString() );
		}

		private static string[] GetUserGroupIdsAsArray()
		{
			RolesDS userGroups = GetUserGroups();
			ArrayList groupsArray = new ArrayList();
			foreach (RolesDS.RolesRow row in userGroups.Roles)
				groupsArray.Add(row.RoleID);

			return (string[]) groupsArray.ToArray(typeof(string));
		}

	/*	public String AppendSecurityFilterQueryForRead(string sql) 
		{
			return AppendSecurityFilterQueryForRead(sql, null); 
		}
	*/	
		//
		public bool HasStaticAccessToDataClass(GAAccessType AccessType) 
	    {

            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
         
			String accessRoles = "";

            if (AccessType == GAAccessType.Read)
                accessRoles = cd.ReadRoles;
            if (AccessType == GAAccessType.Create)
                accessRoles = cd.CreateRoles;
            if (AccessType == GAAccessType.Update)
                accessRoles = cd.UpdateRoles;
            if (AccessType == GAAccessType.Delete)
                accessRoles = cd.DeleteRoles;

			if (accessRoles.IndexOf(";-1;")>-1)
				return true;

            //Get current user roles
            RolesDS rolesData = GetAllUserEngagementRoles();
			foreach (RolesDS.RolesRow row in rolesData.Roles.Rows) 
			{	
				//If current user have one of the readAccessRoles, access is allowed->return true
				if (accessRoles.IndexOf(";"+row.RoleID+";")>-1)
					return true;
			}
			return false;
 	    }


//old version of AppendSecurityFilterQueryForRead commented out by jof 2.8.2006

//		public String AppendSecurityFilterQueryForRead(string sql)
//		{
//			AppUtils.ClassDescription myClassDescription = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
//			if (myClassDescription==null)
//				throw new Exception("No classdescription is defined for gaclass " + _dataClass.ToString() + ". Check data basetable GAClass");
//			
//			if (!GASecurityIsOn || IsGAAdministrator()) return sql;
//			
//			UserDS currentUser = UserDb.GetUserByLogonId(GASecurityDb_new.GetCurrentUserId(), _transaction);
//			
//			//By default, select nothing
//			string secureSql = " SELECT TOP 0 * FROM ("+sql+") MyQuery";
//
//			if (myClassDescription.ApplyAdditionalAccessControl) 
//			{
//				//if the class ApplyAdditionalAccessControl flag is set, this class is not
//				//part of a context and hence no context-security should be applied. Instead we
//				//statically check if user has access to class
//				if (HasStaticAccessToDataClass(GAAccessType.Read))
//					return sql;
//
//			} 
//			else  
//			{
//				//If required accessrole is -1 (all), readaccess is always allowed, not secuirty restriction is applied to the sql
//				ClassRoleAccessDS roleAccessData = GetRoleAccessForDataClassAndParents();
//				string accessRoles = roleAccessData.GAClassRoleAccess[0].ReadRoles;
//				if (accessRoles.IndexOf(";-1;")>-1)
//					return sql;
//				else 
//				{
//					int personellId = currentUser.GAUser[0].PersonnelRowId;
//					string rolesIds = ConvertToSqlNumberList(GetReadRolesForDataClass(_dataClass));
//					if (rolesIds.Length==0)
//						rolesIds = " 0 ";
//
//					secureSql = string.Format(_selectAccessableRowId, sql,  personellId, _dataClass.ToString(), _dataClass.ToString().Substring(2), rolesIds);
//					_logger.Warn(secureSql);
//				}
//			}
//			return secureSql;
//		}


		public String AppendSecurityFilterQueryForRead(string sql)
		{
			
			if (myClassDescription==null)
				throw new Exception("No classdescription is defined for gaclass " + _dataClass.ToString() + ". Check data basetable GAClass");
			
			if (!GASecurityIsOn || IsGAAdministrator()) return sql;
			
			//By default, select nothing
			string secureSql = " SELECT TOP 0 * FROM ("+sql+") MyQuery";

			if (myClassDescription.ApplyAdditionalAccessControl) 
			{
				//if the class ApplyAdditionalAccessControl flag is set, this class is not
				//part of a context and hence no context-security should be applied. Instead we
				//statically check if user has access to class
				if (HasStaticAccessToDataClass(GAAccessType.Read))
					return sql;

			} 
			else  
			{
				//If required accessrole is -1 (all), readaccess is always allowed, not secuirty restriction is applied to the sql
				//				ClassRoleAccessDS roleAccessData = GetRoleAccessForDataClassAndParents();
				//				string accessRoles = roleAccessData.GAClassRoleAccess[0].ReadRoles;
				//				if (accessRoles.IndexOf(";-1;")>-1)
				//					return sql;
				//				else 
				//				{
				//					int personellId = currentUser.GAUser[0].PersonnelRowId;
				//					string rolesIds = ConvertToSqlNumberList(GetReadRolesForDataClass(_dataClass));
				//					if (rolesIds.Length==0)
				//						rolesIds = " 0 ";

				//secureSql = string.Format(_selectAccessableRowId, sql,  personellId, _dataClass.ToString(), _dataClass.ToString().Substring(2), rolesIds);
				secureSql = generateSelectAccessableRowIdQuery(sql);
				_logger.Warn(secureSql);
				//				}
			}
			return secureSql;
		}

		private string generateSelectAccessableRowIdQuery(string MyQuery) 
		{
			//INNER JOIN {1} DataClassQuery On DataClassQuery.{2}RowId = MyQuery.{2}RowId 
			const string selectAccessableBase = @" SELECT MyQuery.* FROM ({0}) MyQuery INNER JOIN (
				select memberclassrowid from FlagArc FA INNER JOIN {1} DataClassQuery On DataClassQuery.{2}RowId = FA.memberclassrowid 
				where memberclass = '{4}' and ({3}) ) AccessQuery ON AccessQuery.memberclassrowid=MyQuery.{2}RowId "; 

			//generate where statment
			GASystem.BusinessLayer.SecurityRoles.RoleUtils roleUtils = new GASystem.BusinessLayer.SecurityRoles.RoleUtils(_transaction);
			GASystem.BusinessLayer.SecurityRoles.FlagRole[] userRoles =  roleUtils.GetRolesForUser();
			
			if (userRoles.Length == 0)
				throw new GAExceptions.GASecurityException("User has no roles");


            string accessMemberClass = _dataClass.ToString();
            if (myClassDescription.hasVirtualClass())
                accessMemberClass = myClassDescription.VirtualClassAttributeName;



			string wherestring = userRoles[0].GetWhereStatement(_dataClass.ToString(), Security.GAAccessType.Read, "FA");
			for (int t = 1; t < userRoles.Length; t++)
				wherestring += " or " + userRoles[t].GetWhereStatement(_dataClass.ToString(), Security.GAAccessType.Read, "FA");

            return string.Format(selectAccessableBase, MyQuery, _dataClass.ToString(), _dataClass.ToString().Substring(2), wherestring, accessMemberClass);

		}

		private string generateSelectAccessableRowIdQuery(string MyQuery, Security.GAAccessType AccessType) 
		{
			//INNER JOIN {1} DataClassQuery On DataClassQuery.{2}RowId = MyQuery.{2}RowId 
			const string selectAccessableBase = @" SELECT MyQuery.* FROM ({0}) MyQuery INNER JOIN (
				select memberclassrowid from FlagArc FA INNER JOIN {1} DataClassQuery On DataClassQuery.{2}RowId = FA.memberclassrowid 
				where memberclass = '{1}' and ({3}) ) AccessQuery ON AccessQuery.memberclassrowid=MyQuery.{2}RowId "; 

			//generate where statment
			GASystem.BusinessLayer.SecurityRoles.RoleUtils roleUtils = new GASystem.BusinessLayer.SecurityRoles.RoleUtils(_transaction);
			GASystem.BusinessLayer.SecurityRoles.FlagRole[] userRoles =  roleUtils.GetRolesForUser();

			if (userRoles.Length == 0)
				throw new GAExceptions.GASecurityException("User has no roles");

			string wherestring = userRoles[0].GetWhereStatement(_dataClass.ToString(), AccessType, "FA");
			for (int t = 1; t < userRoles.Length; t++)
				wherestring += " or " + userRoles[t].GetWhereStatement(_dataClass.ToString(), AccessType, "FA");

			return string.Format(selectAccessableBase, MyQuery, _dataClass.ToString(), _dataClass.ToString().Substring(2), wherestring);

		}

		
		/// <summary>
		/// Determine if user may read records in this context (under this owner)
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="Owner"></param>
		/// <returns></returns>
		public bool HasReadInContext(GADataRecord Owner) 
		{
			bool hasCreate = false;
			IDataReader reader = null;
			string secureSql = "";

			if (!GASecurityIsOn || IsGAAdministrator()) return true;
			
			try
			{
				UserDS currentUser = User.GetUserByLogonId(GetCurrentUserId());
				int personellId = currentUser.GAUser[0].PersonnelRowId;
				string readRoles = GetReadRolesForDataClassInContext(Owner, _dataClass);
				_logger.Debug("readroles for "+_dataClass+" under "+Owner.DataClass.ToString()+" is "+readRoles);
				//If the ALL role has readaccess to _dataClass, user has always access. Return true;
				if (readRoles.IndexOf(";-1;")>-1)
					return true;

				string rolesIds = ConvertToSqlNumberList(readRoles);
				if (rolesIds.Length==0)
					rolesIds = " 0 ";

				
				//Use same sql that determines which rows of a given dataclass we may access in a given context
				//If this sql returns at least one row, user have readAccess
				secureSql = string.Format(_selectAccessableRowId, "SELECT * FROM "+_dataClass,  personellId, _dataClass.ToString(), _dataClass.ToString().Substring(2), rolesIds);
				
				_logger.Debug(secureSql);
				
				reader = DataUtils.executeSelect(secureSql, _transaction);
				//if this query returns one or more rows, we have access!
				if (reader.Read())
					hasCreate = true;
			}
			catch (Exception ex) 
			{
				_logger.Error("An error occured while executing HasCreateOnDataClass. secureSql="+secureSql, ex);	
			}
			finally 
			{
				if (null!=reader)
					reader.Close();
			}
			return hasCreate;
		}

		/// <summary>
		/// Determine if user may create records in this context (under this owner)
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="Owner"></param>
		/// <returns></returns>
		public bool HasCreateInArcLink(GADataRecord Owner) 
		{
			bool hasCreate = false;
			IDataReader reader = null;
			string secureSql = "";

			if (!GASecurityIsOn || IsGAAdministrator()) return true;
			
			try
			{
//				UserDS currentUser = UserDb.GetUserByLogonId(GASecurityDb_new.GetCurrentUserId());
//				int personellId = currentUser.GAUser[0].PersonnelRowId;
				string createRoles = GetCreateRolesForArcLink(Owner.DataClass, _dataClass);
				_logger.Debug("createroles for "+_dataClass+" under "+Owner.DataClass.ToString()+" is "+createRoles);


                //If the ALL role has createaccess to _dataClass, user has always access. Return true;
                if (createRoles.IndexOf(";-1;") > -1)
                    return true;

				//if owner record is on gaflag we are on the top level. need to handle this specially because a gaflag record has no owner
				string ownerPath = "/";
				if (Owner.DataClass != GADataClass.GAFlag) 
				{
					SuperClassDS sds = GASystem.BusinessLayer.DataClassRelations.GetByMember(Owner);
					ownerPath = sds.GASuperClass[0].Path + Owner.DataClass.ToString() + "-" + Owner.RowId.ToString() + "/";
				}
				
				

                //string rolesIds = ConvertToSqlNumberList(createRoles);
                //if (rolesIds.Length==0)
                //    rolesIds = " 0 ";

				
				GASystem.BusinessLayer.SecurityRoles.RoleUtils roleUtils = new GASystem.BusinessLayer.SecurityRoles.RoleUtils(_transaction);
				GASystem.BusinessLayer.SecurityRoles.FlagRole[] userRoles =  roleUtils.GetRolesForUser();

				if (userRoles.Length == 0)
					throw new GAExceptions.GASecurityException("User has no roles");

				foreach (GASystem.BusinessLayer.SecurityRoles.FlagRole userRole in userRoles) 
				{
					string contextPath = "/";
					if (userRole.Context != null)
						contextPath = userRole.Context.DataClass.ToString() + "-" + userRole.Context.RowId.ToString() + "/";
					if ((createRoles.IndexOf(";" + userRole.RoleId.ToString() + ";")>-1) &&  (ownerPath.IndexOf(contextPath)>-1))
						hasCreate = true;
				}
						

				



//				//generate a select that "securly" selects the owner. The owner is selected if user have on of the createRoles (roleIds) 
//				//on any engagement on owner or its parents. If this select returns at least one row, user has create in context.
//				//This works because we know the owner exists. If we have create rigths, that owner will be returned by this sql
//				secureSql = string.Format(_selectAccessableRowId, "SELECT * FROM "+Owner.DataClass,  personellId, Owner.DataClass.ToString(), Owner.DataClass.ToString().Substring(2), rolesIds);
//			
//				
//				reader = DataUtils.executeSelect(secureSql, _transaction);
//				//if this query returns one or more rows, we have access!
//				if (reader.Read())
//					hasCreate = true;
			}
			catch (Exception ex) 
			{
				_logger.Error("An error occured while executing HasCreateInArcLink. secureSql="+secureSql, ex);	
			}
			finally 
			{
				if (null!=reader)
					reader.Close();
			}
			return hasCreate;
		}

		/// <summary>
		/// Determine if user may create records in this context (under this owner)
		/// </summary>
		/// <param name="DataClass"></param>
		/// <param name="Owner"></param>
		/// <returns></returns>
		public bool HasCreateInContext(GADataRecord Owner) 
		{
			bool hasCreate = false;
			IDataReader reader = null;
			string secureSql = "";

			if (!GASecurityIsOn || IsGAAdministrator()) return true;
			
			try
			{
				UserDS currentUser = User.GetUserByLogonId(GASecurityDb_new.GetCurrentUserId());
				int personellId = currentUser.GAUser[0].PersonnelRowId;
				string createRoles = GetCreateRolesForDataClassInContext(Owner, _dataClass);
				 _logger.Debug("createroles for "+_dataClass+" under "+Owner.DataClass.ToString()+" is "+createRoles);


				//If the ALL role has createaccess to _dataClass, user has always access. Return true;
				if (createRoles.IndexOf(";-1;")>-1)
					return true;

				string rolesIds = ConvertToSqlNumberList(createRoles);
				if (rolesIds.Length==0)
					rolesIds = " 0 ";

				//generate a select that "securly" selects the owner. The owner is selected if user have on of the createRoles (roleIds) 
				//on any engagement on owner or its parents. If this select returns at least one row, user has create in context.
				//This works because we know the owner exists. If we have create rigths, that owner will be returned by this sql
				secureSql = string.Format(_selectAccessableRowId, "SELECT * FROM "+Owner.DataClass,  personellId, Owner.DataClass.ToString(), Owner.DataClass.ToString().Substring(2), rolesIds);
			
				
				reader = DataUtils.executeSelect(secureSql, _transaction);
				//if this query returns one or more rows, we have access!
				if (reader.Read())
					hasCreate = true;
			}
			catch (Exception ex) 
			{
				_logger.Error("An error occured while executing HasCreateOnDataClass. secureSql="+secureSql, ex);	
			}
			finally 
			{
				if (null!=reader)
					reader.Close();
			}
			return hasCreate;
		}

        /// <summary>
        /// Determine if user may edit records within in this context (under this owner)
        /// </summary>
        /// <param name="DataClass"></param>
        /// <param name="Owner"></param>
        /// <returns></returns>
        public bool HasUpdateWithinInContext(GADataRecord Owner)
        {
            bool hasUpdateWithin = false;
            IDataReader reader = null;
            string secureSql = "";

            if (!GASecurityIsOn || IsGAAdministrator()) return true;

            try
            {
                UserDS currentUser = User.GetUserByLogonId(GASecurityDb_new.GetCurrentUserId());
                int personellId = currentUser.GAUser[0].PersonnelRowId;
                string updateWithinRoles = GetUpdateWithinRolesForArcLink(Owner.DataClass, _dataClass);
                _logger.Debug("createroles for " + _dataClass + " under " + Owner.DataClass.ToString() + " is " + updateWithinRoles);


                //If the ALL role has createaccess to _dataClass, user has always access. Return true;
                if (updateWithinRoles.IndexOf(";-1;") > -1)
                    return true;

                string rolesIds = ConvertToSqlNumberList(updateWithinRoles);
                if (rolesIds.Length == 0)
                    rolesIds = " 0 ";

                //generate a select that "securly" selects the owner. The owner is selected if user have on of the createRoles (roleIds) 
                //on any engagement on owner or its parents. If this select returns at least one row, user has create in context.
                //This works because we know the owner exists. If we have create rigths, that owner will be returned by this sql
                secureSql = string.Format(_selectAccessableRowId, "SELECT * FROM " + Owner.DataClass, personellId, Owner.DataClass.ToString(), Owner.DataClass.ToString().Substring(2), rolesIds);


                reader = DataUtils.executeSelect(secureSql, _transaction);
                //if this query returns one or more rows, we have access!
                if (reader.Read())
                    hasUpdateWithin = true;
            }
            catch (Exception ex)
            {
                _logger.Error("An error occured while executing HasCreateOnDataClass. secureSql=" + secureSql, ex);
            }
            finally
            {
                if (null != reader)
                    reader.Close();
            }
            return hasUpdateWithin;
        }

		
		/// <summary>
		/// Determine if user may update on dataclass in context (under this owner)
		/// </summary>
		/// <param name="DataClass"></param>
		/// <returns></returns>
		public bool HasUpdateInArcLink(GADataRecord Owner) 
		{
			bool hasUpdate = false;
			IDataReader reader = null;
			string secureSql = "";

			if (!GASecurityIsOn || IsGAAdministrator()) return true;
			
			try
			{
			//	UserDS currentUser = UserDb.GetUserByLogonId(GASecurityDb_new.GetCurrentUserId());
			//	int personellId = currentUser.GAUser[0].PersonnelRowId;
				string updateRoles = GetUpdateRolesForArcLink(Owner.DataClass, _dataClass);
				_logger.Debug("updateroles for "+_dataClass+" under "+Owner.DataClass.ToString()+" is "+updateRoles);

				//If the ALL role has readaccess to _dataClass, user has always access. Return true;
				if (updateRoles.IndexOf(";-1;")>-1)
					return true;

                //string rolesIds = ConvertToSqlNumberList(updateRoles);
                //if (rolesIds.Length==0)
                //    rolesIds = " 0 ";

				
                //secureSql = string.Format(_selectAccessableRowId, "SELECT * FROM "+_dataClass,  personellId, _dataClass.ToString(), _dataClass.ToString().Substring(2), rolesIds);
				

                ////				secureSql = string.Format(_selectAccessUnderOwner,  personellId, Owner.DataClass.ToString(), Owner.RowId.ToString(), rolesIds);
                //reader = DataUtils.executeSelect(secureSql, _transaction);
                ////if this query returns one or more rows, we have access!
                //if (reader.Read())
                //    hasUpdate = true;
                //if owner record is on gaflag we are on the top level. need to handle this specially because a gaflag record has no owner
                string ownerPath = "/";
                if (Owner.DataClass != GADataClass.GAFlag)
                {
                    SuperClassDS sds = GASystem.BusinessLayer.DataClassRelations.GetByMember(Owner);
                    ownerPath = sds.GASuperClass[0].Path + Owner.DataClass.ToString() + "-" + Owner.RowId.ToString() + "/";
                }

                //If the ALL role has createaccess to _dataClass, user has always access. Return true;
                if (updateRoles.IndexOf(";-1;") > -1)
                    return true;

                GASystem.BusinessLayer.SecurityRoles.RoleUtils roleUtils = new GASystem.BusinessLayer.SecurityRoles.RoleUtils(_transaction);
                GASystem.BusinessLayer.SecurityRoles.FlagRole[] userRoles = roleUtils.GetRolesForUser();

                if (userRoles.Length == 0)
                    throw new GAExceptions.GASecurityException("User has no roles");

                foreach (GASystem.BusinessLayer.SecurityRoles.FlagRole userRole in userRoles)
                {
                    string contextPath = "/";
                    if (userRole.Context != null)
                        contextPath = userRole.Context.DataClass.ToString() + "-" + userRole.Context.RowId.ToString() + "/";
                    if ((updateRoles.IndexOf(";" + userRole.RoleId.ToString() + ";") > -1) && (ownerPath.IndexOf(contextPath) > -1))
                        hasUpdate = true;
                }
					
			}
			catch (Exception ex) 
			{
				_logger.Error("An error occured while executing HasUpdateOnDataClass. secureSql="+secureSql, ex);	
			}
			finally 
			{
				if (null!=reader)
					reader.Close();
			}
			return hasUpdate;
		}


		/// <summary>
		/// Determine if user may the specified access has access to this datarecord
		/// </summary>
		public bool HasAccessInArcLink(GADataRecord DataRecord, Security.GAAccessType AccessType) 
		{
			bool hasUpdate = false;
			IDataReader reader = null;
			string secureSql = "";

			if (!GASecurityIsOn || IsGAAdministrator()) return true;
			
			try
			{
				const string securesql = "select {0} as {1}RowId";

				secureSql = generateSelectAccessableRowIdQuery(string.Format(securesql, DataRecord.RowId.ToString(), _dataClass.ToString().Substring(2)), AccessType);

				reader = DataUtils.executeSelect(secureSql, _transaction);
				//if this query returns one or more rows, we have access!
				if (reader.Read())
					hasUpdate = true;
			}
			catch (Exception ex) 
			{
				_logger.Error("An error occured while executing HasUpdateOnDataClass. secureSql="+secureSql, ex);	
			}
			finally 
			{
				if (null!=reader)
					reader.Close();
			}
			return hasUpdate;
		}


		/// <summary>
		/// Determine if user may update on dataclass in context (under this owner)
		/// </summary>
		/// <param name="DataClass"></param>
		/// <returns></returns>
		public bool HasUpdateInContext(GADataRecord Owner) 
		{
			bool hasUpdate = false;
			IDataReader reader = null;
			string secureSql = "";

			if (!GASecurityIsOn || IsGAAdministrator()) return true;
			
			try
			{
				UserDS currentUser = User.GetUserByLogonId(GASecurityDb_new.GetCurrentUserId());
				int personellId = currentUser.GAUser[0].PersonnelRowId;
				string updateRoles = GetUpdateRolesForDataClassInContext(Owner, _dataClass);
				_logger.Debug("updateroles for "+_dataClass+" under "+Owner.DataClass.ToString()+" is "+updateRoles);

				//If the ALL role has readaccess to _dataClass, user has always access. Return true;
				if (updateRoles.IndexOf(";-1;")>-1)
					return true;

				string rolesIds = ConvertToSqlNumberList(updateRoles);
				if (rolesIds.Length==0)
					rolesIds = " 0 ";

				secureSql = string.Format(_selectAccessableRowId, "SELECT * FROM "+_dataClass,  personellId, _dataClass.ToString(), _dataClass.ToString().Substring(2), rolesIds);

//				secureSql = string.Format(_selectAccessUnderOwner,  personellId, Owner.DataClass.ToString(), Owner.RowId.ToString(), rolesIds);
				reader = DataUtils.executeSelect(secureSql, _transaction);
				//if this query returns one or more rows, we have access!
				if (reader.Read())
					hasUpdate = true;
			}
			catch (Exception ex) 
			{
				_logger.Error("An error occured while executing HasUpdateOnDataClass. secureSql="+secureSql, ex);	
			}
			finally 
			{
				if (null!=reader)
					reader.Close();
			}
			return hasUpdate;
		}
		
		/// <summary>
		/// Determine if user may delete a record of current dataclass in this context (under this owner)
		/// </summary>
		/// <param name="DataClass"></param>
		/// <returns></returns>
		public bool HasDeleteInArcLink(GADataRecord Owner) 
		{
			bool hasDelete = false;
			IDataReader reader = null;
			string secureSql = "";

			if (!GASecurityIsOn || IsGAAdministrator()) return true;
			
			try
			{
				UserDS currentUser = User.GetUserByLogonId(GASecurityDb_new.GetCurrentUserId());
				int personellId = currentUser.GAUser[0].PersonnelRowId;
				string deleteRoles = GetDeleteRolesForArcLink(Owner.DataClass, _dataClass);
				_logger.Debug("deleteroles for "+_dataClass+" under "+Owner.DataClass.ToString()+" is "+deleteRoles);		
				//If the ALL role has readaccess to _dataClass, user has always access. Return true;
				if (deleteRoles.IndexOf(";-1;")>-1)
					return true;

				string rolesIds = ConvertToSqlNumberList(deleteRoles);
				if (rolesIds.Length==0)
					rolesIds = " 0 ";

				secureSql = string.Format(_selectAccessableRowId, "SELECT * FROM "+_dataClass,  personellId, _dataClass.ToString(), _dataClass.ToString().Substring(2), rolesIds);

				//secureSql = string.Format(_selectAccessUnderOwner,  personellId, Owner.DataClass.ToString(), Owner.RowId.ToString(), rolesIds);
				reader = DataUtils.executeSelect(secureSql, _transaction);
				//if this query returns one or more rows, we have access!
				if (reader.Read())
					hasDelete = true;
			}
			catch (Exception ex) 
			{
				_logger.Error("An error occured while executing HasDeleteOnDataClass. secureSql="+secureSql, ex);	
			}
			finally 
			{
				if (null!=reader)
					reader.Close();
			}
			return hasDelete;
		}

		/// <summary>
		/// Determine if user may delete a record of current dataclass in this context (under this owner)
		/// </summary>
		/// <param name="DataClass"></param>
		/// <returns></returns>
		public bool HasDeleteInContext(GADataRecord Owner) 
		{
			bool hasDelete = false;
			IDataReader reader = null;
			string secureSql = "";

			if (!GASecurityIsOn || IsGAAdministrator()) return true;
			
			try
			{
				UserDS currentUser = User.GetUserByLogonId(GASecurityDb_new.GetCurrentUserId());
				int personellId = currentUser.GAUser[0].PersonnelRowId;
				string deleteRoles = GetDeleteRolesForDataClassInContext(Owner, _dataClass);
				_logger.Debug("deleteroles for "+_dataClass+" under "+Owner.DataClass.ToString()+" is "+deleteRoles);		
				//If the ALL role has readaccess to _dataClass, user has always access. Return true;
				if (deleteRoles.IndexOf(";-1;")>-1)
					return true;

				string rolesIds = ConvertToSqlNumberList(deleteRoles);
				if (rolesIds.Length==0)
					rolesIds = " 0 ";

				secureSql = string.Format(_selectAccessableRowId, "SELECT * FROM "+_dataClass,  personellId, _dataClass.ToString(), _dataClass.ToString().Substring(2), rolesIds);

				//secureSql = string.Format(_selectAccessUnderOwner,  personellId, Owner.DataClass.ToString(), Owner.RowId.ToString(), rolesIds);
				reader = DataUtils.executeSelect(secureSql, _transaction);
				//if this query returns one or more rows, we have access!
				if (reader.Read())
					hasDelete = true;
			}
			catch (Exception ex) 
			{
				_logger.Error("An error occured while executing HasDeleteOnDataClass. secureSql="+secureSql, ex);	
			}
			finally 
			{
				if (null!=reader)
					reader.Close();
			}
			return hasDelete;
		}


		/// <summary>
		/// Get the union of all accessroles for a dataclass. The union is calculated by
		/// recursivly traversing DataClass parents via GASuperclasslink
		/// </summary>
		/// <returns></returns>
		public ClassRoleAccessDS GetRoleAccessForArcLink(GADataClass Owner,  GADataClass Member) 
		{
			
			ClassRoleAccessDS classRoleAccessData = new ClassRoleAccessDS();
            string sql = "SELECT SuperClassLinksRowId, MemberClass, isNull(ReadRoles,'') as ReadRoles,  isNull(UpdateRoles,'') as UpdateRoles ,isNull(CreateRoles,'') as CreateRoles, isNull(DeleteRoles,'') as DeleteRoles, isNull(textfree1,'') as UpdateWithinRoles  FROM GASuperClassLinks WHERE OwnerClass ='{0}' AND MemberClass='{1}'";
			sql = string.Format(sql, Owner.ToString(), Member.ToString());
			SqlConnection connection = DataUtils.GetConnection(_transaction);
			
			SqlDataAdapter da = new SqlDataAdapter(sql, connection);
			if (_transaction != null)
				da.SelectCommand.Transaction = (SqlTransaction) _transaction.Transaction;
			
			da.Fill(classRoleAccessData, "GAClassRoleAccess");

			return classRoleAccessData;
			
			
			/*StringBuilder readRoles = new StringBuilder();
			StringBuilder createRoles = new StringBuilder();
			StringBuilder updateRoles = new StringBuilder();
			StringBuilder deleteRoles = new StringBuilder();
			readRoles.Append(";");
			createRoles.Append(";");
			updateRoles.Append(";");
			deleteRoles.Append(";");

			//ArrayList dataClasses = GetDataClassAndParentsInContext(Context, dataClass);

			//ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClassAndParents(_dataClass);

			//foreach (GADataClass tmpDataClass in dataClasses) 
			//{
				ClassRoleAccessDS classRoleAccessData = GetRoleAccessForArcLink(Owner, Member);	
				if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0) 
				{
					ClassRoleAccessDS.GAClassRoleAccessRow row = classRoleAccessData.GAClassRoleAccess[0];
					foreach (String listElement in row.ReadRoles.ToString().Split( new char[] {';'} )) 
					{
						if (listElement == "" || listElement==null || readRoles.ToString().IndexOf(";"+listElement+";")>-1) 
							continue;
						readRoles.Append(listElement).Append(";");
					}

					
					foreach (String listElement in row.CreateRoles.ToString().Split( new char[] {';'} )) 
					{
						if (listElement == "" || listElement==null || createRoles.ToString().IndexOf(";"+listElement+";")>-1) 
							continue;
					
						createRoles.Append(listElement).Append(";");
					}

					foreach (String listElement in row.UpdateRoles.ToString().Split( new char[] {';'} )) 
					{
						if (listElement == "" || listElement==null || updateRoles.ToString().IndexOf(";"+listElement+";")>-1) 
							continue;
		
						updateRoles.Append(listElement).Append(";");
					}

					foreach (String listElement in row.DeleteRoles.ToString().Split( new char[] {';'} )) 
					{
						if (listElement == "" || listElement==null || deleteRoles.ToString().IndexOf(";"+listElement+";")>-1) 
							continue;
					
						deleteRoles.Append(listElement).Append(";");
					}

				}
				
			//}

			//Roles that are not set, (only contain ; ) are set to blank stringbuilder object
			if (readRoles.Length==1) readRoles = new StringBuilder();
			if (updateRoles.Length==1) updateRoles = new StringBuilder();
			if (createRoles.Length==1) createRoles = new StringBuilder();
			if (deleteRoles.Length==1) deleteRoles = new StringBuilder();

			ClassRoleAccessDS classRoleAccessDataAggr = new ClassRoleAccessDS();
			ClassRoleAccessDS.GAClassRoleAccessRow newrow = classRoleAccessDataAggr.GAClassRoleAccess.NewGAClassRoleAccessRow();
			
			newrow.Class = _dataClass.ToString();
			newrow.ReadRoles = readRoles.ToString();
			newrow.CreateRoles = createRoles.ToString();
			newrow.UpdateRoles = updateRoles.ToString();
			newrow.DeleteRoles = deleteRoles.ToString();

			classRoleAccessDataAggr.GAClassRoleAccess.Rows.Add(newrow);
			return classRoleAccessDataAggr;*/

		}



		/// <summary>
		/// Get the union of all accessroles for a dataclass. The union is calculated by
		/// recursivly traversing DataClass parents via GASuperclasslink
		/// </summary>
		/// <returns></returns>
		public ClassRoleAccessDS GetRoleAccessForDataClassInContext(GADataRecord Context,  GADataClass dataClass) 
		{
			StringBuilder readRoles = new StringBuilder();
			StringBuilder createRoles = new StringBuilder();
			StringBuilder updateRoles = new StringBuilder();
			StringBuilder deleteRoles = new StringBuilder();
			readRoles.Append(";");
			createRoles.Append(";");
			updateRoles.Append(";");
			deleteRoles.Append(";");

			ArrayList dataClasses = GetDataClassAndParentsInContext(Context, dataClass);

			//ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClassAndParents(_dataClass);

			foreach (GADataClass tmpDataClass in dataClasses) 
			{
				ClassRoleAccessDS classRoleAccessData = GetRoleAccessForDataClass(tmpDataClass);	
				if (classRoleAccessData.GAClassRoleAccess.Rows.Count>0) 
				{
					ClassRoleAccessDS.GAClassRoleAccessRow row = classRoleAccessData.GAClassRoleAccess[0];
					foreach (String listElement in row.ReadRoles.ToString().Split( new char[] {';'} )) 
					{
						if (listElement == "" || listElement==null || readRoles.ToString().IndexOf(";"+listElement+";")>-1) 
							continue;
						readRoles.Append(listElement).Append(";");
					}

					
					foreach (String listElement in row.CreateRoles.ToString().Split( new char[] {';'} )) 
					{
						if (listElement == "" || listElement==null || createRoles.ToString().IndexOf(";"+listElement+";")>-1) 
							continue;
					
						createRoles.Append(listElement).Append(";");
					}

					foreach (String listElement in row.UpdateRoles.ToString().Split( new char[] {';'} )) 
					{
						if (listElement == "" || listElement==null || updateRoles.ToString().IndexOf(";"+listElement+";")>-1) 
							continue;
		
						updateRoles.Append(listElement).Append(";");
					}

					foreach (String listElement in row.DeleteRoles.ToString().Split( new char[] {';'} )) 
					{
						if (listElement == "" || listElement==null || deleteRoles.ToString().IndexOf(";"+listElement+";")>-1) 
							continue;
					
						deleteRoles.Append(listElement).Append(";");
					}

				}
				
			}

			//Roles that are not set, (only contain ; ) are set to blank stringbuilder object
			if (readRoles.Length==1) readRoles = new StringBuilder();
			if (updateRoles.Length==1) updateRoles = new StringBuilder();
			if (createRoles.Length==1) createRoles = new StringBuilder();
			if (deleteRoles.Length==1) deleteRoles = new StringBuilder();

			ClassRoleAccessDS classRoleAccessDataAggr = new ClassRoleAccessDS();
			ClassRoleAccessDS.GAClassRoleAccessRow newrow = classRoleAccessDataAggr.GAClassRoleAccess.NewGAClassRoleAccessRow();
			
			newrow.Class = _dataClass.ToString();
			newrow.ReadRoles = readRoles.ToString();
			newrow.CreateRoles = createRoles.ToString();
			newrow.UpdateRoles = updateRoles.ToString();
			newrow.DeleteRoles = deleteRoles.ToString();

			classRoleAccessDataAggr.GAClassRoleAccess.Rows.Add(newrow);
			return classRoleAccessDataAggr;

		}

		public ArrayList GetDataClassAndParentsInContext(GADataRecord Owner, GADataClass dataClass) 
		{
			ArrayList returnArray = new ArrayList();
			returnArray.Add(dataClass);
			returnArray.Add(Owner.DataClass);

			//Get the owner's parents
            SuperClassDS parentObjects = (new SuperClassDb()).GetSuperClassByMember(Owner.RowId, Owner.DataClass, _transaction);

			if (parentObjects.GASuperClass.Rows.Count==0)
				return returnArray;
			else 
			{
				//Path example: /GACompany-8/GAProject-1/GAIncidentReport-19/
				String path = parentObjects.GASuperClass[0].Path;
				foreach (String dataRecord in path.Split(new char[] {'/'})) 
				{
					if (dataRecord==null || dataRecord.Length==0)
						continue;

					//dataRecord example: GAIncidentReport-19
					String dataClassName = dataRecord.Split(new char[] {'-'})[0];
					GADataClass tmpDataClass = GADataRecord.ParseGADataClass(dataClassName);
					returnArray.Add(tmpDataClass);

				}
			}
		
		/*	foreach (SuperClassDS.GASuperClassRow row in parentObjects.GASuperClass.Rows) 
			{
				ArrayList tmpDataClasses = GetDataClassAndParentsInContext(new GADataRecord(row.OwnerClassRowId, GADataRecord.ParseGADataClass(row.OwnerClass)), Owner.DataClass);
				if (tmpDataClasses.Count==0)
					continue;
			
				//make sure each dataclass is only added once
				foreach (GADataClass tmpDataClass in tmpDataClasses) 
				{
					if (!returnArray.Contains(tmpDataClass))
						returnArray.Add(tmpDataClass);
				}
			}
		*/
			return returnArray;
		}

		public static RolesDS GetUserRolesForContext(GASystem.DataModel.GADataRecord DataRecord) 
		{
//			1.	Get the gasuperclass path for the garecord by joining the ga table with gasuperclass.
//2.	Add the an extra identifier for the garecord to this path:
//garecordcontext = gasuperclass path + garecord class name + ‘-‘ + garecord rowid +  ‘/’    or  (/existingpath/<garrecordclass>-<garecordrowid>/)
//3.	Run this query:
//
//select e.RoleListRowId
//from GAEmployment e INNER JOIN GASuperClass s ON e.EmploymentRowId=s.MemberClassRowId 
//where s.memberclass = 'gaemployment' and 
//e.personnel = <personnelid> and
//	‘<garecordcontext>’ like s.path + ‘%’
			//if no security or gaadministrator, return all possible roles
			if (!GASecurityIsOn || IsGAAdministrator()) 
				return GetAllSecurityRoles();    
			
			UserDS currentUser = User.GetUserByLogonId(GASecurityDb_new.GetCurrentUserId());
			int personellId = currentUser.GAUser[0].PersonnelRowId;
			


			string sql = @"SELECT ListsRowId AS RoleId, GAListValue AS RoleName, GAListDescription AS Description
							FROM GALists
							WHERE ListsRowId in (
							select e.RoleListsRowId
							from GAEmployment e INNER JOIN GASuperClass s ON e.EmploymentRowId=s.MemberClassRowId 
							where s.memberclass = 'gaemployment' and 
							e.personnel = {0} and 
							'{1}' like s.path + '%'
							)";
			
			//get context
			GASystem.DataModel.SuperClassDS sds = GASystem.BusinessLayer.DataClassRelations.GetByMember(DataRecord);
			
			string path = "/";
			if (sds.GASuperClass.Rows.Count > 0)
			 path = sds.GASuperClass[0].Path;

			path = path + DataRecord.DataClass.ToString() + "-" + DataRecord.RowId.ToString() + "/";

			string roleSql = string.Format(sql, personellId.ToString(), path);
			
			RolesDS rolesDSdata = new RolesDS();
			String connection = DataUtils.getConnectionString();
			


			SqlDataAdapter da = new SqlDataAdapter(roleSql, connection);
			//			if (_transaction != null)
			//				da.SelectCommand.Transaction = (SqlTransaction) _transaction.Transaction;
			da.Fill(rolesDSdata, "Roles");
			return rolesDSdata;
				


		}

	}

	
}

