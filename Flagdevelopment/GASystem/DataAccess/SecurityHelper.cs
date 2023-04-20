using System;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for SecurityHelper.
	/// </summary>
	public class SecurityHelper
	{
		public SecurityHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Returns the userId of the current user
		/// </summary>
		/// <returns></returns>
		public static int GetUserId()
		{
			int userId = 0;
			try
			{
				String userIdString = System.Web.HttpContext.Current.User.Identity.Name;
				if (userIdString!=null && userIdString.Length>0)
					userId = int.Parse(userIdString);
			}
			catch (Exception ex)
			{

			}
			return userId;
		}

	}
}
