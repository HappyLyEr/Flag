using System;
using System.Web;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Summary description for GAUsers.
	/// </summary>
	public class GAUsers
	{
		public GAUsers()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		/// <summary>
		/// Get userid from GA using the current application/http context
		/// </summary>
		/// <returns></returns>
		public static string GetUserId() 
		{
			//TODO return based on the platform we are running on
			if (HttpContext.Current != null)
				return System.Web.HttpContext.Current.User.Identity.Name;

			return string.Empty;
			//return System.Web.HttpContext.Current.User.Identity.Name;
		}

		
	}
}
