using System;
using DotNetNuke;
using GASystem;
using GASystem.GAControls;
using GASystem.UserControls;


namespace gadnnmodules.utils
{
	/// <summary>
	/// Summary description for Security.
	/// </summary>
	public class Security
	{
		public Security()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //public static void ApplyLinkDisplay(int ModuleId, ListDataRecords ListDataControl, ViewDataClass ViewDataClassControl) 
        //{
        //    if (DotNetNuke.Security.PortalSecurity.HasEditPermissions(ModuleId) )
        //    {
        //        ListDataControl.DisplayEditButton = true;
        //        ListDataControl.DisplayNewButton = false;
        //        ViewDataClassControl.DisplayEditLink = true;	
        //    } 
        //    else
        //    {
        //        ListDataControl.DisplayEditButton = false;
        //        ListDataControl.DisplayNewButton = false;
        //        ViewDataClassControl.DisplayEditLink = false;
        //    }
        //}

		public static bool HasEditPermissions(int moduleId, int tabId) 
		{
			return DotNetNuke.Security.PortalSecurity.HasEditPermissions(moduleId, tabId);
		}

	}
}
