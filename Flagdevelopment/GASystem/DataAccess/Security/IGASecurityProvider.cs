using System;
using GASystem.DataModel;

namespace GASystem.DataAccess.Security
{
	/// <summary>
	/// Summary description for IGASecurityProvider.
	/// </summary>
	public interface IGASecurityProvider
	{
	
		RolesDS GetUserGroups(string UserId);

		RolesDS GetAllGroups();
	}
}
