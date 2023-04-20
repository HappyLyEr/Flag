using System;
using GASystem.DataModel;


namespace GASystem.DataAccess.Security 
{
	/// <summary>
	/// Summary description for ApplicationSecurityProvider.
	/// </summary>
	public class ApplicationSecurityProvider : IGASecurityProvider	
	{
		public ApplicationSecurityProvider()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public RolesDS GetUserGroups(string UserId) 
		{
			return getRole();
		}

		public RolesDS GetAllGroups() 
		{
			return getRole();
		}
		
		private RolesDS getRole() 
		{
			RolesDS ds = new RolesDS();
			RolesDS.RolesRow row = ds.Roles.NewRolesRow();
			row.RoleID = 1;
			row.RoleName = "GAAdministrator";
			ds.Roles.Rows.Add(row);
			ds.Roles.AcceptChanges();
			return ds;
		}

	}
}
