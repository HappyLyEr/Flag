using System;

namespace GASystem.DataAccess.Utils
{
	/// <summary>
	/// Summary description for DatabaseSettings.
	/// </summary>
	public class DatabaseSettings
	{
		public DatabaseSettings()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		/// <summary>
		/// Get DNNDatabaseName from web.config. Throw gaexception if it is missing
		/// </summary>
		public static string DNNDatabaseName 
		{
			get 
			{
// Tor 20160127				string dnnName = System.Configuration.ConfigurationManager.AppSettings.Get("DNNDatabaseName");
                string dnnName = new GASystem.AppUtils.FlagSysResource().GetResourceString("DNNDatabaseName");

				if (dnnName == null)
					throw new GAExceptions.GAException("DNNDatabaseName settings not found in config files");
				return dnnName;
			}
		}

	}
}
