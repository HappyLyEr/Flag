using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using GASystem.DataModel;

namespace GASystem.DataAccess.Security
{
	/// <summary>
	/// Security Provider used when running in standalone mode. e.g the consolconsumer. 
	/// The consolconsumer needs full access, so this provier returns one role of type gaadministrator
	/// </summary>
	public class NoneWebSecurityProvider : IGASecurityProvider
	{

		public NoneWebSecurityProvider()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region IGASecurityProvider Members

		public GASystem.DataModel.RolesDS GetUserGroups(string UserId)
		{
			
			return GAAdministratorRole();
		}

		public GASystem.DataModel.RolesDS GetAllGroups()
		{
			
			return GAAdministratorRole();
		}

		#endregion

		private GASystem.DataModel.RolesDS GAAdministratorRole() 
		{
			RolesDS RolesData = new RolesDS();
			RolesDS.RolesRow row = RolesData.Roles.NewRolesRow();
			RolesData.Roles.AddRolesRow(GASystem.DataAccess.Security.GASecurityGroups.GAAdministrator.ToString(), string.Empty);
			RolesData.Roles.AcceptChanges();
			return RolesData;
		}
	}
}
