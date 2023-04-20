using System;

namespace GASystem.UserControls
{
	public enum UserControlType {EditDataRecord, ListDataRecords, SubTabsNavigation, ViewDataRecord, RelatedDataRecordField }
	
	/// <summary>
	/// Summary description for UserControlUtils.
	/// </summary>
	public class UserControlUtils
	{
		public UserControlUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Returns an instance of a flag UserControl
		/// </summary>
		/// <param name="ControlType"></param>
		/// <returns></returns>
		public static System.Web.UI.Control GetUserControl(UserControlType ControlType, System.Web.UI.Page Page) 
		{
			//this.Page.LoadControl("~/gagui/UserControls/UserMessage.ascx");
			

			if (ControlType == UserControlType.EditDataRecord)
				return Page.LoadControl("~/gagui/UserControls/EditDataRecord.ascx");
			if (ControlType == UserControlType.ListDataRecords)
				return Page.LoadControl("~/gagui/UserControls/ListDataRecords.ascx");
			if (ControlType == UserControlType.SubTabsNavigation)
				return Page.LoadControl("~/gagui/UserControls/SubTabsNavigation.ascx");
			if (ControlType == UserControlType.ViewDataRecord)
				return Page.LoadControl("~/gagui/UserControls/ViewDataRecord.ascx");
			if (ControlType == UserControlType.RelatedDataRecordField)
				return Page.LoadControl("~/gagui/UserControls/RelatedDataRecordField.ascx");
			
			return null;
		}
	}
}
