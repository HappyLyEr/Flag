using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace GASystem.WebControls.ViewControls
{
	/// <summary>
	/// Summary description for WorkflowStarted.
	/// </summary>
	public class WorkflowStarted : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private string workflowId = string.Empty;
		//private int actionId;
		private Label statusLabel = new Label();
		private Label workflowStatus = new Label();
		//public Button startWorkflow = new Button();
		//public Button testButton = new Button();
		private Label workitemLog = new Label();

		public WorkflowStarted()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public string Value 
		{
			get {return workflowId;}
			set {workflowId = value;}
		}

		public int ActionId 
		{
			get 
			{
				if (ViewState["workflowactionid"] == null)
					return 0;
				return (int)ViewState["workflowactionid"];
			}
			set {
				ViewState["workflowactionid"] = value;
			}
		}

		
		protected override void OnInit(EventArgs e)
		{
			
			//statusLabel = new Label();
		
			base.OnInit (e);

			//this.startWorkflow.Click += new EventHandler(startWorkflow_Click);
			workflowStatus.CssClass="UserMessageError";
			workflowStatus.ForeColor = System.Drawing.Color.Red;
			this.Controls.Add(statusLabel);
			//this.Controls.Add(startWorkflow);
			this.Controls.Add(workflowStatus);
			this.Controls.Add(workitemLog);
			
		}



		protected override void OnPreRender(EventArgs e){
			base.OnLoad (e);
			BusinessLayer.Action myAction = new GASystem.BusinessLayer.Action();
			bool ActionHasWorkflow = myAction.HasWorkflow(ActionId);

			if (ActionHasWorkflow)  
			{
				if (Value != string.Empty && Value != GASystem.BusinessLayer.Action.START_WORKFLOW_AUTOMATICALLY) 
				{
					statusLabel.Text =  GASystem.AppUtils.Localization.GetCaptionText("WorkflowStarted");
			//		startWorkflow.Visible = false;
					
				} 
				else 
				{
					statusLabel.Text = GASystem.AppUtils.Localization.GetCaptionText("WorkflowNotStarted") + "<br/>";
					if (Value ==GASystem.BusinessLayer.Action.START_WORKFLOW_AUTOMATICALLY) 
					{
						workflowStatus.Text = "</br>" + GASystem.AppUtils.Localization.GetErrorText("FailedToStartWorkflow"); 
						workflowStatus.Visible = true;
						
					}

//					//statusLabel.ForeColor = System.Drawing.Color.Red;
//					startWorkflow.Text = GASystem.AppUtils.Localization.GetCaptionText("StartWorkflow");
//
//					
//					startWorkflow.Visible = true;
				}
			} 
			else 
			{
//				statusLabel.Text = GASystem.AppUtils.Localization.GetCaptionText("NoWorkflowForPRocedure") + "</br>";
//				startWorkflow.Visible = false;
			}
			workitemLog.Text = GASystem.BusinessLayer.Workitem.GetWorkitemLogByAction(ActionId);

		}


//		private void startWorkflow_Click(object sender, EventArgs e)
//		{
//			
//			workflowStatus.Text = "<br/>";
//			//todo add check for permission to start workflow
//			try 
//			{
////				try 
////				{
//					GAWorkflow.OWFEWorkFlowEngine.SetStartPending(GASystem.AppUtils.GAUsers.GetUserId(), ActionId);
//
////	commented out by JOF 060605. No need for this no that we are not starting the workflow directly from the web app
////				} 
////				catch (System.Net.WebException ex) 
////				{
////					if (ex.Message.IndexOf("500") > -1)   //owfe bug workaround, if the rest server has been resting for too long 
////					{									  //it might return a 500 error. Try starting again
////						GAWorkflow.OWFEWorkFlowEngine.StartWorkFlow(GASystem.AppUtils.GAUsers.GetUserId(), ActionId);
////					} 
////					else 
////					{
////						throw ex;
////					}
////				}
//				GASystem.AppUtils.PageDispatcher.GotoDataRecordViewPage(this.Page.Response, GASystem.DataModel.GADataClass.GAAction, ActionId, null);
//			} 
//			catch (System.Net.WebException ex) 
//			{
//				if (ex.Message.IndexOf("404") > -1) 
//				{
//					workflowStatus.Text+=GASystem.AppUtils.Localization.GetErrorText("CannotFindWorkflow");   
//				} 
//				else 
//				{
//					workflowStatus.Text+=GASystem.AppUtils.Localization.GetErrorText("FailedToStartWorkflow"); 
//						
//				}
//			}
//
//			catch (Exception ex) 
//			{
//				workflowStatus.Text+=ex.Message;
//			}
//			
//		}

		
	}
}
