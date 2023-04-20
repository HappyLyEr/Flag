using GASystem.GAExceptions;
using log4net;

namespace GASystem
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.BusinessLayer;
	using GASystem.AppUtils;
	using System.Reflection;
	using GASystem.DataModel;
	using GASystem.GAControls;
	using GASystem.GAGUIEvents;

	/// <summary>
	///		Summary description for EditDataRecord.
	/// </summary>
	public class CreateMultipleRecords : EditDataRecord 
	{

		protected System.Web.UI.WebControls.Label lblAddGroup;

		
		public CreateMultipleRecords() : base() 
		{
		}

	


		protected override Control CreateLookupFieldMultipleControl(DataColumn c, Control placeHolder)
		{
			//Only support multiple select when in new-mode
			if (c.Table.Rows[0].RowState==DataRowState.Added) 
				return  AddLookupFieldMultiSelect(c, placeHolder);
			
			return AddLookupField(c, placeHolder);
		}

		private void InitializeComponent()
		{

		}

		protected override bool DisplayFieldOnForm(FieldDescription fd)
		{
			if (fd.HideInDetail)   //don't bother with fields that should be hidden in details form. Do not display it
				return false;
			
			bool displayControl = false;
			if (fd.ControlType.ToUpper().TrimEnd() == GADataControl.LookupFieldMultiple.ToString().ToUpper())
				displayControl = true;

            if (fd.ControlType.ToUpper().TrimEnd() == GADataControl.LookupFieldGroups.ToString().ToUpper())
                displayControl = true;

            if (fd.ControlType.ToUpper().TrimEnd() == GADataControl.LookupField.ToString().ToUpper())
                displayControl = true;

            if (fd.ControlType.ToUpper().TrimEnd() == GADataControl.Textbox.ToString().ToUpper())
                displayControl = true;

			if (fd.ControlType.ToUpper().TrimEnd() == GADataControl.Date.ToString().ToUpper())
				displayControl = true;

			if (fd.ControlType.ToUpper().TrimEnd() == GADataControl.DateTime.ToString().ToUpper())
				displayControl = true;

			if (fd.ControlType.ToUpper().TrimEnd() == GADataControl.Checkbox.ToString().ToUpper())
				displayControl = true;

			return displayControl;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
		
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			lblAddGroup.Text = string.Format(GASystem.AppUtils.Localization.GetGuiElementText("PleaseAddGroupOfPersons"), 
				GASystem.AppUtils.Localization.GetGuiElementTextPlural(this.DataClass));
		}


	}
}
