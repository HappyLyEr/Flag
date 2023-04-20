//====================================================================
// This file is generated as part of Web project conversion.
// The extra class 'TabStripsDetails' in the code behind file in 'usercontrols\Tabsnavigation.ascx.cs' is moved to this file.
//====================================================================




namespace GASystem
 {

	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;
	using GASystem.AppUtils;

	[Serializable]
	public class TabStripsDetails
	{
		public TabStripsDetails(String TabId , String TabCaption, string NavigationURL)
		{
			this.TabId = TabId;
			this.TabCaption = TabCaption;
			this.NavigationURL = NavigationURL;
			
		}
		public String TabId;
		public String TabCaption;
		public string NavigationURL;
		
		

	}

}