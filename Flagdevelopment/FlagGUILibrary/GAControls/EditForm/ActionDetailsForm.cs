using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.AppUtils;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using GASystem.GAControls.UtilityControls;
using GASystem.UserControls;
using GASystem.GUIUtils;

namespace GASystem.GAControls.EditForm
{
	/// <summary>
	/// Summary description for GeneralDetailsForm.
	/// </summary>
	public class ActionDetailsForm : AbstractDetailsForm
	{

        // Tor 201611 Security 20161122 (never referenced) private GASystem.GAControls.UtilityControls.ProcedureSelector MyProcedureSelector;
        //		(never referenced) private CheckBox startWorkflowCheckBox;
        //	(never referenced) private Label lblStartWorkflow;
		private GASystem.ViewDataRecord myViewDataRecord;
        // Tor 201611 Security 20161122 (never referenced) private Label newActionMsg;
		private Label validationMsg;
		private GAControls.ActionOwnerContext myActionOwnerContext;
		
		
		private Panel viewPanel;
//		private  HyperLink lblDisplayViewPanel;
//		private HyperLink lblHideViewPanel;
//		

		public ActionDetailsForm(GADataRecord DataRecord) : base(DataRecord)
		{
			//
			// TODO: Add constructor logic here
			//
			//this.myEditDataRecord.
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			
			
			if (this.DataRecord.RowId > 0 )
				this.OwnerRecord = DataClassRelations.GetOwner(this.OwnerRecord);
			//{
				
				
//				newActionMsg = new Label();
//				newActionMsg.Text =
//					String.Format(Localization.GetGuiElementText("NewRecordForOwner"),
//					              Localization.GetGuiElementText(this.DataRecord.DataClass.ToString()), Localization.GetGuiElementText(this.OwnerRecord.DataClass.ToString()));
//			
//				this.PlaceHolderTop.Controls.Add(newActionMsg);

					
				myActionOwnerContext = (GAControls.ActionOwnerContext)this.Page.LoadControl("~/gagui/GAControls/Workflow/ActionOwnerContext.ascx");
				PlaceHolderTop.Controls.Add(myActionOwnerContext);
				

			


				
				viewPanel = new Panel();
				viewPanel.ID = "viewpanel";
				this.PlaceHolderTop.Controls.Add(viewPanel);
				
//				lblHideViewPanel = new HyperLink();
//				//lblHideViewPanel.NavigateUrl = "#";
//				lblHideViewPanel.Text = Localization.GetGuiElementText("Hide") + " " +
//				                           Localization.GetGuiElementText(this.OwnerRecord.DataClass.ToString());
//				lblHideViewPanel.CssClass = "linkLabel";
//				lblHideViewPanel.Attributes.Add("onclick", "javascript:hideviewpanel();return false;");
//				viewPanel.Controls.Add(lblHideViewPanel);
//				
				myViewDataRecord = (ViewDataRecord)UserControlUtils.GetUserControl(UserControlType.ViewDataRecord, this.Page);
				myViewDataRecord.DataClass = this.OwnerRecord.DataClass.ToString();
				myViewDataRecord.RecordDataSet =
					GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(this.OwnerRecord);

            //get lists selected for this control

            // Change by Lars Heskje, 20.03.2011. Will avoid duplicat gAListsSelected due to caching
                if (!myViewDataRecord.RecordDataSet.Tables.Contains("GAListsSelected"))
                {
                    GASystem.BusinessLayer.BusinessClass bcListsSelected = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAListsSelected);
                    DataSet dsListsSelected = bcListsSelected.GetByOwner(this.OwnerRecord, null);
                    myViewDataRecord.RecordDataSet.Tables.Add(dsListsSelected.Tables[GADataClass.GAListsSelected.ToString()].Copy());
                }


				viewPanel.Controls.Add(myViewDataRecord);
				myViewDataRecord.SetupForm(this.OwnerRecord.DataClass.ToString());
				
			
				viewPanel.Controls.Add(GASystem.GUIUtils.HTMLLiteralTags.CreateBRTag());
				Label newRecordLabel = new Label();
				if (this.DataRecord.RowId == 0)
					newRecordLabel.Text = String.Format( Localization.GetGuiElementText("NewRecord"), Localization.GetGuiElementText(this.DataRecord.DataClass.ToString()));
				else
					newRecordLabel.Text = Localization.GetGuiElementText(this.DataRecord.DataClass.ToString());
				viewPanel.Controls.Add(newRecordLabel);


				validationMsg = new Label();
				validationMsg.Text = string.Empty;
				validationMsg.ForeColor = System.Drawing.Color.Red;
				this.Controls.Add(GUIUtils.HTMLLiteralTags.CreateBRTag());
				this.Controls.Add(validationMsg);
			//}
		}
		
		

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			myActionOwnerContext.SetActionOwner(this.OwnerRecord);
			myActionOwnerContext.DisplayPanelId = viewPanel.ClientID;

		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			//setviewpanelvisible
//			string jscode = "<script language=\"javascript\">\n";
//			jscode += "function setviewpanelvisible() {\n";
//			jscode +=  "var rescontrol = document.getElementById('" + viewPanel.ClientID +  "');\n";
//			//lblDisplayViewPanel
//			jscode +=  "var displayLabel = document.getElementById('" + lblDisplayViewPanel.ClientID +  "');\n";
//			jscode += "rescontrol.style.display  = 'block'; ";
//			jscode += "displayLabel.style.display  = 'none'; ";
//			//visible
//
//			jscode += "}\n </script>";
//			this.Page.RegisterStartupScript("respuservisible" + this.ID, jscode);
//			
//			
//			
//			//hideviewpanel
//			jscode = "<script language=\"javascript\">\n";
//			jscode += "function hideviewpanel() {\n";
//			jscode +=  "var rescontrol = document.getElementById('" + viewPanel.ClientID +  "');\n";
//			jscode +=  "var displayLabel = document.getElementById('" + lblDisplayViewPanel.ClientID +  "');\n";
//			
//			jscode += "displayLabel.style.display  = 'block'; ";
//			jscode += "rescontrol.style.display  = 'none'; ";
//			//visible
//
//			jscode += "}\n </script>";
//			this.Page.RegisterStartupScript("setrolevisible" + this.ID, jscode);
			
				viewPanel.Style.Add("display", "none");
//				lblDisplayViewPanel.Style.Add("display", "block");
		
		}


		protected override void EditRecordSave(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(DataRecord.DataClass);
			
			GADataRecord dispatchToRecord = DataRecord;

			//check that a responsible is selected
			ActionDS ads = (ActionDS)e.CommandDataSetArgument;
			if (ads.GAAction[0].IsResponsibleNull() && ads.GAAction[0].IsResponsibleRoleListsRowIdNull()) 
			{
				validationMsg.Text = Localization.GetGuiElementText("PleaseSelectAnActionResponsible");
				return;
			}


			//abc.

			if (DataRecord.RowId == 0) 
			{
				//DataSet ds = bc.CommitDataSet(e.CommandDataSetArgument, this.OwnerRecord);
				ActionDS ds = (ActionDS) bc.CommitDataSet(e.CommandDataSetArgument, this.OwnerRecord);
				//GASystem.BusinessLayer.w
				
				try 
				{
					if (!ds.GAAction[0].IsProcedureRowIdNull())
						GAWorkflow.OWFEWorkFlowEngine.SetStartPending(GASystem.AppUtils.GAUsers.GetUserId(), ds.GAAction[0].ActionRowId);	
				} 
				catch (Exception ex)
				{
				}
				dispatchToRecord = new GADataRecord((int)ds.Tables[0].Rows[0][bc.DataClass.ToString().Substring(2)+"RowId"], bc.DataClass);
			} 
			else 
			{
				bc.CommitDataSet(e.CommandDataSetArgument);
			}

			PageDispatcher.GotoDataRecordViewPage(this.Page.Response, dispatchToRecord.DataClass, dispatchToRecord.RowId, null);

		}

		protected override void EditRecordCancel(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			if (DataRecord.RowId != 0)
				PageDispatcher.GotoDataRecordViewPage(this.Page.Response, DataRecord.DataClass, DataRecord.RowId, null);	
			else if (this.OwnerRecord == null)
				PageDispatcher.GotoDataRecordListPage(this.Page.Response, DataRecord.DataClass);
			else
				PageDispatcher.GotoDataRecordViewPage(this.Page.Response, OwnerRecord.DataClass, OwnerRecord.RowId, null); // GADataRecord.ParseGADataClass(DataClass), SessionManagement.GetCurrentDataContext().SubContextRecord); 
			
		}

        protected override void EditRecordDelete(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
        {
            //GADataRecord owner = DataClassRelations.GetOwner(SessionManagement.GetCurrentDataContext().SubContextRecord);

            BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(this.DataRecord.DataClass);
            bc.DeleteRow(this.DataRecord.RowId);
            if (OwnerRecord == null)
                PageDispatcher.GotoDataRecordListPage(this.Page.Response, this.DataRecord.DataClass);
            else
                PageDispatcher.GotoDataRecordViewPage(this.Page.Response, OwnerRecord.DataClass, OwnerRecord.RowId, null, this.DataRecord.DataClass); // GADataRecord.ParseGADataClass(DataClass), SessionManagement.GetCurrentDataContext().SubContextRecord); 

        }

		private void MyProcedureSelector_ProcedureSelected(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			ProcedureDS ds = (ProcedureDS)e.CommandDataSetArgument;
			ActionDS ads = (ActionDS)this.RecordDataSet;
			ads.GAAction[0].Subject = ds.GAProcedure[0].Shortname;
			ads.GAAction[0].Description = ds.GAProcedure[0].Description;
			ads.GAAction[0].Workflowname = ds.GAProcedure[0].Workflowname;
			this.myEditDataRecord.RecordDataSet = ads;
			this.myEditDataRecord.SetupForm();
		}

        protected override void TemplateSelected(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }
	}
}
