namespace GASystem.GAControls.Workflow
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.AppUtils;
	using GASystem.DataModel;
	using System.Collections;
		using GASystem.WebControls.EditControls;
    using GASystem.BusinessLayer;
    using GASystem.GUIUtils;
    using GASystem.DataAccess.Security;
	/// <summary>
	///		Summary description for DelegateWorkitem.
	/// </summary>
    public class CloseWorkitem : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.Label lblNotes;
	    protected System.Web.UI.WebControls.PlaceHolder placeHolderCloseWorkitem;

        protected System.Web.UI.WebControls.Button btnYes;
        protected System.Web.UI.WebControls.Button btnNo;
        protected System.Web.UI.WebControls.Button btnOk;
        protected System.Web.UI.WebControls.Button btnCompleted;
        protected System.Web.UI.WebControls.Button btnReject;
        protected System.Web.UI.WebControls.Button btnDissatisfy;
        // Tor 20141031 New buttons: 
        //add code in`: 
        //FlagGui\gagui\GAControls\Workflowcloseworkitem.ascx line (<asp:TableRow>)29+
        //FlagGUILibrary\GAControls\Workflow\CloseWorkitem.ascx.cs (here), InitializeComponent, OnPreRender start + line ca 196, add section at bottom of file
        //GASystem\BusinessLayer\workitem.cs add to enum ActorsReply
        protected System.Web.UI.WebControls.Button btnApprove;
        protected System.Web.UI.WebControls.Button btnProceed;
        protected System.Web.UI.WebControls.Button btnbtnFree1;
        protected System.Web.UI.WebControls.Button btnbtnFree2;
        protected System.Web.UI.WebControls.Button btnbtnFree3;
        protected System.Web.UI.WebControls.Button btnClose;

        protected System.Web.UI.WebControls.Label lblWorkitemReaction;
        protected System.Web.UI.WebControls.RequiredFieldValidator commentTextValidator;
        protected System.Web.UI.WebControls.TextBox closeComment;
        protected System.Web.UI.WebControls.TableRow tableRowNotes;

        private GASystem.DataModel.WorkitemDS myDataSet;
        private int _workitemId;
        private bool canCloseWorkitem = false;
        private string closeWorkitemInstruction = string.Empty;

        protected System.Web.UI.WebControls.Label lblErrorMessage;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Workitem workitem = new Workitem();

            bool hasUserAssignment = false;
            bool hasRoleAssigment = false;
            bool hasEditAssignmentToWorkitem = false;

            try
            {
                UserDS currentUser = User.GetUserByLogonId(GASecurityDb_new.GetCurrentUserId());
                int personellId = currentUser.GAUser[0].PersonnelRowId;
                hasUserAssignment = workitem.hasUserAssignmentToWorkitem(personellId, WorkitemId);
                hasRoleAssigment = workitem.hasRoleAssignmentToWorkitem(personellId, WorkitemId);
                // Tor 20170627 If user has user or role assignment to workitem, then he has edit assignment to workitem
                //hasEditAssignmentToWorkitem = workitem.hasEditAssignmentToWorkitem(personellId, WorkitemId);
                if (hasUserAssignment || hasRoleAssigment)
                    hasEditAssignmentToWorkitem = true;
                else
                    hasEditAssignmentToWorkitem = workitem.hasEditAssignmentToWorkitem(personellId, WorkitemId);

            }
            catch (Exception ex)
            {
                throw new GAExceptions.GAException(ex.Message);//TODO log;
            }

            canCloseWorkitem = hasUserAssignment || hasRoleAssigment || hasEditAssignmentToWorkitem;


        }
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		//	AddLookupField();
		//	AddRoles();
            //set button labels
        
           

			
		}


	

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
			//AddRoles();
            commentTextValidator.Text = "<br/>" + Localization.GetErrorText("FieldRequired");
			
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnCompleted.Click += new System.EventHandler(this.btnCompleted_Click);
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            this.btnDissatisfy.Click += new System.EventHandler(this.btnDissatisfy_Click);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
			// Tor 20130508 added buttons 
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            this.btnbtnFree1.Click += new System.EventHandler(this.btnbtnFree1_Click);
            this.btnbtnFree2.Click += new System.EventHandler(this.btnbtnFree2_Click);
            this.btnbtnFree3.Click += new System.EventHandler(this.btnbtnFree3_Click);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			

		}
		#endregion

        public int WorkitemId 
		{
			get {return _workitemId;}
            set { _workitemId = value; }
		}

        public GASystem.DataModel.WorkitemDS DataRecordSet
        {
            set { myDataSet = value; }
            get { return myDataSet; }
        }

        public bool DisplayNotes
        {
            set 
            {
                this.closeComment.Visible = value;
                
            }
            get { return this.closeComment.Visible; }
        }

        public bool UserCanCloseWorkitem
        {
            get { return canCloseWorkitem; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            btnCompleted.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.completed.ToString());
            btnOk.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.ok.ToString());
            btnNo.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.no.ToString());
            btnReject.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.rejected.ToString());
            btnDissatisfy.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.dissatisfied.ToString());
            btnYes.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.yes.ToString());
            btnApprove.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.approved.ToString());
            btnProceed.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.proceed.ToString());
            btnbtnFree1.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.btnFree1.ToString());
            btnbtnFree2.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.btnFree2.ToString());
            btnbtnFree3.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.btnFree3.ToString());
            btnClose.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.Close.ToString());

            lblNotes.Visible = this.DisplayNotes;
            tableRowNotes.Visible = this.DisplayNotes;
            try
            {
               int actionId = myDataSet.GAWorkitem[0].ActionRowId;


                System.Collections.ArrayList rowids = new System.Collections.ArrayList();

                //display action info
                //verify that gaaction record exists. May not be replicated yet.
                //DataModel.GADataRecord actionRecord = new GASystem.DataModel.GADataRecord(actionId, DataModel.GADataClass.GAAction);
                //if (BusinessLayer.Utils.RecordSetUtils.DoesRecordExist(actionRecord))
                //{
                //    DataModel.ActionDS ads = Action.GetActionByActionRowId(actionId);
                    lblWorkitemReaction.Text = AppUtils.Localization.GetCaptionText("WorkitemReaction");
                //}


                try
                {
                    string workitemCompletionStatus = myDataSet.GAWorkitem[0].WorkitemStatus;

                    //string actorsReply = _wi.attributes[new openwfe.workitem.StringAttribute(BusinessLayer.Workitem.ACTORSREPLY)].ToString();
                    string actorsReply = myDataSet.GAWorkitem[0].ActorsReply;

                    if (actorsReply != string.Empty && workitemCompletionStatus == GASystem.BusinessLayer.Workitem.WorkitemStatus.Active.ToString())   
                    //update visible buttons settings if ACTORSREPLY attribute exists and is not empty
                    {
                        btnNo.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.no.ToString()) > -1;
                        btnOk.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.ok.ToString()) > -1;
                        btnReject.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.rejected.ToString()) > -1;
                        btnDissatisfy.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.dissatisfied.ToString()) > -1;
                        btnYes.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.yes.ToString()) > -1;
                        btnCompleted.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.completed.ToString()) > -1;
                        // 20130506 new buttons
                        btnApprove.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.approved.ToString()) > -1;
                        btnProceed.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.proceed.ToString()) > -1;
                        btnbtnFree1.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.btnFree1.ToString()) > -1;
                        btnbtnFree2.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.btnFree2.ToString()) > -1; 
                        btnbtnFree3.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.btnFree3.ToString()) > -1;
                        btnClose.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.Close.ToString()) > -1;

                    }
                    //TODO create and set commands dynamically in oninit

                    if (workitemCompletionStatus != GASystem.BusinessLayer.Workitem.WorkitemStatus.Active.ToString())
                    {
                        this.Visible = false;
                    }


                }
                catch
                {
                    //error getting attriubute, ignore and use default display of buttons
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
                lblErrorMessage.Visible = true;
            }
            //find 
            base.OnPreRender(e);
        }


        #region Forward task

      
        private void ForwardTask(string Result)
        {
            if (!this.Page.IsValid)
                return;
            
            
            GASystem.BusinessLayer.Workitem.ProceedWorkFlow(WorkitemId, Result, closeComment.Text);
            //		this.Page.Cache.Remove(GASystem.BusinessLayer.Workitem.WORKITEM_CACHE);  //remove the workitems from cache, forces a refresh
            PageDispatcher.GotoDataRecordViewPage(this.Page.Response, GADataClass.GAWorkitem, WorkitemId, null);

        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.ok.ToString());
        }

        private void btnYes_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.yes.ToString());
        }

        private void btnNo_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.ok.ToString());
        }

        private void btnCompleted_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.completed.ToString());
        }

        private void btnReject_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.rejected.ToString());
        }

        private void btnDissatisfy_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.dissatisfied.ToString());
        }

        // 20130506 new buttons
        private void btnApprove_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.approved.ToString());
        }
        private void btnProceed_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.proceed.ToString());
        }

        private void btnbtnFree1_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.btnFree1.ToString());
        }
        private void btnbtnFree2_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.btnFree2.ToString());
        }
        private void btnbtnFree3_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.btnFree3.ToString());
        }
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            ForwardTask(GASystem.BusinessLayer.Workitem.ActorsReply.Close.ToString());
        }

        #endregion
    }
}
