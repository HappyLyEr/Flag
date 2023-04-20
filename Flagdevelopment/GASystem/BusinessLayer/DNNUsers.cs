using System;
using GASystem.DataModel;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for DNNUsers.
	/// </summary>
	public class DNNUsers
	{
		public DNNUsers()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DNNUsersDS GetDNNUserByUserId(int UserId) 
		{
			return GASystem.DataAccess.DNNUsersDb.GetDNNUserByUserId(UserId);
		}
	}
}
