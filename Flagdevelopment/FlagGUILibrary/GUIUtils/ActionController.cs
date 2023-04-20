using System;
using System.Web.UI.WebControls;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace GASystem.GUIUtils
{
	/// <summary>
	/// Summary description for ActionController.
	/// </summary>
	public class ActionController : WebControl
	{
		private System.Web.UI.Control detailsControl;
		
		public ActionController()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private GADataRecord ownerRecord 
		{
			get 
			{
				return null==ViewState["GADataRecordownerRecord" + this.ID] ? null : (GADataRecord)ViewState["GADataRecordownerRecord" + this.ID];
			}
			set 
			{
				ViewState["GADataRecordownerRecord" + this.ID] = value;
			}
		}

		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState (savedState);
			if (ownerRecord == null) 
			{
				//TODO replace with proper error handling
				Label labelControl = new Label();
				labelControl.Text = "owner record not found";
				detailsControl = labelControl;
			}
			else
			{
				detailsControl = GetActionOwnerDetailsControl(ownerRecord);
			}
			
			this.Controls.Add(detailsControl);

		}



		protected override void OnInit(EventArgs e)
		{
			
			base.OnInit (e);
		}


		private System.Web.UI.Control GetActionOwnerDetailsControl(GADataRecord Owner) 
		{
			GASystem.GAControls.ViewDataClass returnControl;
			returnControl = (GASystem.GAControls.ViewDataClass)this.Page.LoadControl("~/gagui/gacontrols/viewdataclass.ascx");
			returnControl.DataRecord = Owner;
			returnControl.DisplayEditLink = true;
			returnControl.DisplayReportLink = true;

//			returnControl.EditRecord +=new GASystem.GAGUIEvents.GACommandEventHandler(returnControl_EditRecord);

			returnControl.GenerateView();
			return returnControl;
		}

		public void SetActionId(int ActionId) 
		{
			ownerRecord = DataClassRelations.GetOwner(new GADataRecord(ActionId, GADataClass.GAAction));
			detailsControl = GetActionOwnerDetailsControl(ownerRecord);
			this.Controls.Clear();
			this.Controls.Add(detailsControl);
		}

		private void returnControl_EditRecord(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			GADataRecord dataRecord = ((GASystem.GAControls.ViewDataClass)sender).DataRecord;
			PageDispatcher.GotoDataRecordDetailsPage(this.Page.Response, dataRecord.DataClass, dataRecord.RowId, 
				GASystem.BusinessLayer.DataClassRelations.GetOwner(dataRecord));
		}
	}
}
