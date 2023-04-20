using GASystem.BusinessLayer;

namespace GASystem.GAControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.DataModel;

	/// <summary>
	///		Summary description for WorkflowStatus.
	/// </summary>
	public class WorkflowStatus : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Panel PanelStartWorkFlow;
		protected System.Web.UI.WebControls.LinkButton LinkButtonStartWorkFlow;
		protected System.Web.UI.WebControls.Panel PanelWorkFlowStarted;
		protected System.Web.UI.WebControls.Label LabelWorkflowStartDate;
		protected System.Web.UI.WebControls.HyperLink HrefReportDetails;
		protected System.Web.UI.WebControls.Label LabelWorkFlowStatus;

		private ActionDS _actionRecordSet;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here

		}

		protected override void OnPreRender(EventArgs e)
		{
			if (ActionDataSet != null) 
			{
				if (ActionDataSet.GAAction[0].IsWorkflownameNull()) 
				{
					LabelWorkFlowStatus.Text = "There is no workflow defined for this action";
					PanelStartWorkFlow.Visible = false;
					PanelWorkFlowStarted.Visible = false;
				}
				else 
				{
					LabelWorkFlowStatus.Text = "This action is based on the workflow " + ActionDataSet.GAAction[0].Workflowname;
					if(ActionDataSet.GAAction[0].IsWorkflowIdNull()) 
					{
						PanelStartWorkFlow.Visible = true;
						PanelWorkFlowStarted.Visible = false;
					} 
					else 
					{
						PanelStartWorkFlow.Visible = false;
						PanelWorkFlowStarted.Visible = true;
						string navigateUrl;
						navigateUrl = "gagui/webforms/workflowreport.aspx?image=true&zoom=100&application=" + System.Configuration.ConfigurationSettings.AppSettings.Get("GASkeltaApplicationName") + "&executionid=" + ActionDataSet.GAAction[0].WorkflowId;
						HrefReportDetails.NavigateUrl = "javascript:openReportWindow('" + navigateUrl + "');";
					}
				} //end if isworkflownamenull
			}
			base.OnPreRender (e);
		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.LinkButtonStartWorkFlow.Click += new System.EventHandler(this.LinkButtonStartWorkFlow_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void LinkButtonStartWorkFlow_Click(object sender, System.EventArgs e)
		{
			//ActionDataSet.GAAction[0].WorkflowId = GAWorkflow.WorkflowEngine.StartWorkFlow(GASystem.AppUtils.GAUsers.GetUserId(), ActionDataSet.GAAction[0].ActionRowId);
			ActionDataSet.GAAction[0].WorkflowId = GAWorkflow.OWFEWorkFlowEngine.StartWorkFlow(GASystem.AppUtils.GAUsers.GetUserId(), ActionDataSet.GAAction[0].ActionRowId);

			//TODO should we clear the workitem cache at this point?
			
		}	

		public ActionDS ActionDataSet
		{
			get
			{
				//return null==ViewState["ActionDataSet"+this.ID] ? null : (ActionDS) ViewState["ActionDataSet"+this.ID];
				return _actionRecordSet;
			}
			set
			{
				//ViewState["ActionDataSet"+this.ID] 
				_actionRecordSet = value;
			}
		}
	}
}
