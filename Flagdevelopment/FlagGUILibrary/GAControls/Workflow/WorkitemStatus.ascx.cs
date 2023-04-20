namespace GASystem.GAControls.Workflow
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.BusinessLayer;
	using GASystem.DataModel;
	using GASystem.AppUtils;

	/// <summary>
	///		Summary description for WorkitemStatus.
	/// </summary>
	public class WorkitemStatus : System.Web.UI.UserControl
	{
        private enum statusSubControls { closeWorkitem, closeNCWorkitem, addComment, delegateWorkitem, addParticipant, removeAddedParticipants }; // Tor 20150108 added removeAddedParticipants

        protected System.Web.UI.WebControls.Label lblErrorMessage;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected GASystem.GAControls.Workflow.DelegateWorkitem MyDelegateWorkitem;
		protected GASystem.GAControls.Workflow.AddWorkitemParticipant MyAddWorkitemParticipant;
        protected GASystem.GAControls.Workflow.RemoveAddedParticipants MyRemoveAddedParticipants; // Tor 20150108 added
        //protected WorkitemAcknowledgement MyWorkitemAcknowledgement;
        protected CloseWorkitem MyCloseWorkitem;
        protected CloseNCWorkitem MyCloseNCWorkitem;
        protected AddWorkitemComment MyAddWorkitemComment;
        protected System.Web.UI.WebControls.Label lblReactionStatus;
        protected System.Web.UI.WebControls.Label lblAcknowledgeStatus;
		
		protected System.Web.UI.WebControls.LinkButton lnkDelegate;
		protected System.Web.UI.WebControls.LinkButton lnkAddParticipant;
        protected System.Web.UI.WebControls.LinkButton lnkRemoveAddedParticipants; // tor 20150108 add link to click to remove added participants
        protected System.Web.UI.WebControls.LinkButton lnkCloseWorkitem;
        protected System.Web.UI.WebControls.LinkButton lnkCloseNCWorkitem;
        protected System.Web.UI.WebControls.LinkButton lnkAddComment;
        protected System.Web.UI.WebControls.LinkButton lnkAcknowledgeWorkitem;
        protected System.Web.UI.WebControls.LinkButton lnkRejectAcknowledgeWorkitem;
        protected System.Web.UI.WebControls.LinkButton lnkRejectAcknowledgeNCWorkitem;
        
        protected System.Web.UI.WebControls.Label txtDelegate;
        protected System.Web.UI.WebControls.Label lblAddNotes;
        protected System.Web.UI.WebControls.Label lblAcknowledge;
        protected System.Web.UI.WebControls.Label lblRejectAcknowledge;
        protected System.Web.UI.WebControls.Label lblRejectAcknowledgeNC;
        protected System.Web.UI.WebControls.Label lblCloseWorkitem;
        protected System.Web.UI.WebControls.Label lblCloseNCWorkitem;
        protected System.Web.UI.WebControls.Label lblRemoveAddedParticipants; // tor 20150108 add label remove added participants
        protected System.Web.UI.WebControls.Label separatorLabel;

        protected System.Web.UI.WebControls.Literal txtAddParticipant;
        protected System.Web.UI.WebControls.Literal txtEditWorkitem;
       // protected Label lblAssignedWorkitem;
		
		private GASystem.DataModel.WorkitemDS myDataSet;
		protected System.Web.UI.WebControls.HyperLink hyperLinkEdit;

		protected GASystem.ViewDataRecord MyViewDataRecord;
		

		private bool _hasEditPermissions = false;
        private bool hasEditPermissionsOnRecord = false;
        private bool hasAddedParticipants = false; // Tor 20150108
        private int _workitemId;

	//	private openwfe.workitem.InFlowWorkitem _wi;

		private void Page_Load(object sender, System.EventArgs e)
		{
	
			//set link labels
            txtAddParticipant.Text = AppUtils.Localization.GetGuiElementText("AddParticipant");
			txtDelegate.Text = AppUtils.Localization.GetGuiElementText("DelegateWorkitem");
            lblAddNotes.Text = AppUtils.Localization.GetGuiElementText("WorkitemAddNotes");
            lblAcknowledge.Text = AppUtils.Localization.GetGuiElementText("WorkitemAcknowledge");
            lblRejectAcknowledge.Text = AppUtils.Localization.GetGuiElementText("WorkitemRejectAcknowledge");
            lblRejectAcknowledgeNC.Text = AppUtils.Localization.GetGuiElementText("WorkitemRejectAcknowledge");
            lblCloseWorkitem.Text = AppUtils.Localization.GetGuiElementText("WorkitemCloseWorkitem");
            lblCloseNCWorkitem.Text = AppUtils.Localization.GetGuiElementText("WorkitemCloseWorkitem");
            lblRemoveAddedParticipants.Text = AppUtils.Localization.GetGuiElementText("RemoveAddedParticipants"); // Tor 20150108 added
            separatorLabel.Text = AppUtils.Localization.GetGuiElementText(GADataClass.GAWorkitem.ToString());

            //add workitem info
            //display all fields defined in fielddefinition except the object name
            ClassDescription cdw = ClassDefinition.GetClassDescriptionByGADataClass(GADataClass.GAWorkitem);

            foreach (System.Data.DataColumn column in DataRecordSet.Tables[GADataClass.GAWorkitem.ToString()].Columns)
               if (column.ColumnName != cdw.ObjectName)
                   MyViewDataRecord.AddColumnToDisplay(column.ColumnName);


           MyCloseWorkitem.WorkitemId = this.WorkitemId;
           MyCloseNCWorkitem.WorkitemId = this.WorkitemId;

           MyViewDataRecord.RecordDataSet = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsByDataRecord(new GADataRecord(this.WorkitemId, GADataClass.GAWorkitem)); //DataRecordSet;
			MyViewDataRecord.DataClass = GASystem.DataModel.GADataClass.GAWorkitem.ToString();
            // Tor 20140320 Added ownerclass. Parameter required to setup form correctly. Field GAFieldDefinitions.HideIfOwnerClass (TextFree3)
            MyViewDataRecord.SetupForm("tor");


            MyCloseWorkitem.DataRecordSet = DataRecordSet;
            MyCloseNCWorkitem.DataRecordSet = DataRecordSet;

           // MyWorkitemAcknowledgement.AcknowledgementStatus = Workitem.getWorkitemAcknowledgmentStatus((WorkitemDS.GAWorkitemRow)DataRecordSet.Tables[0].Rows[0]);
            this.AcknowledgementStatus = Workitem.getWorkitemAcknowledgmentStatus((WorkitemDS.GAWorkitemRow)DataRecordSet.Tables[0].Rows[0]);
         //   MyWorkitemAcknowledgement.AcknowledgementStatus = this.AcknowledgementStatus;
           
         //   MyWorkitemAcknowledgement.WorkitemID = this.WorkitemId;

		}

		private static bool isParticipantARole(string Participant) 
		{
			string[] participantParts =	Participant.Split("-".ToCharArray());
			if (participantParts[0] == "garole")
				return true;
			return false;
		}

        //
        private string workitemCompletionStatus;

        public string WorkitemCompletionStatus
        {
            get { return workitemCompletionStatus; }
            set { workitemCompletionStatus = value; }
        }

        //private properties used for dispalying info based on workitem status
        bool hasUserWorkitemAssignment = false; // Assigned to Person or Person has edit access through GASuperClassLinks
        bool hasUserAssignmentToWorkitem = false; // Assigned to Person 
        bool hasRoleAssignmentToWorkitem = false;
        bool hasEditAssignmentToWorkitem = false;
        // Tor 20190227 
        bool hasAddCommentAssignmentToWorkitem = false;



		protected override void OnPreRender(EventArgs e)
		{
			try 
			{
                workitemCompletionStatus = myDataSet.GAWorkitem[0].WorkitemStatus;
                
                //set workitemids on subcontrols
                MyDelegateWorkitem.WorkitemId = this.WorkitemId;
                MyAddWorkitemParticipant.WorkitemId = this.WorkitemId;
                MyAddWorkitemComment.WorkitemId = this.WorkitemId;
                //MyCloseWorkitem.WorkitemId = this.WorkitemId;
                MyRemoveAddedParticipants.WorkitemID = this.WorkitemId; // Tor 20150108 new button Remove Added Participants
                UserDS currentUser = User.GetUserByLogonId(GASystem.DataAccess.Security.GASecurityDb_new.GetCurrentUserId());
                int personnelId = currentUser.GAUser[0].PersonnelRowId;

                bool isWorkitemParticipant = false;
                Workitem workitem = new Workitem();

                // test
                hasAddCommentAssignmentToWorkitem =
    hasUserWorkitemAssignment
    || hasUserAssignmentToWorkitem
    || hasRoleAssignmentToWorkitem
    || hasEditAssignmentToWorkitem
    || isWorkitemParticipant
    || (!myDataSet.GAWorkitem[0].IsExtra2Null() 
    && myDataSet.GAWorkitem[0].Extra2.Contains(";" + personnelId.ToString() + ";")) // is added person ?
    || (!myDataSet.GAWorkitem[0].IsExtra4Null() // has added titles ?
    && workitem.IsPersonnelAddedRoleForWorkItem(personnelId, this.WorkitemId)); //check if user has added role

                // end test
                //optimalisation: only get workitem permissions if status is active, otherwise keep all permissions at false
                // Tor 20150108 and workflow type not = WorkflowStart
                if (workitemCompletionStatus == Workitem.WorkitemStatus.Active.ToString()
                    && myDataSet.GAWorkitem[0].TextFree1 != "WorkflowStart")
                {
                    hasUserWorkitemAssignment = workitem.hasAssigmentToWorkitem(personnelId, this.WorkitemId);
                    hasUserAssignmentToWorkitem = workitem.hasUserAssignmentToWorkitem(personnelId, this.WorkitemId);
                    hasRoleAssignmentToWorkitem = workitem.hasRoleAssignmentToWorkitem(personnelId, this.WorkitemId);
                    hasEditAssignmentToWorkitem = workitem.hasEditAssignmentToWorkitem(personnelId, this.WorkitemId);
                    // participants
                    isWorkitemParticipant = workitem.hasUserParticipantToWorkitem(personnelId, this.WorkitemId) // is person responsible person
                        || workitem.hasRoleParticipantToWorkitem(personnelId, this.WorkitemId); // has person responsible role
                    // Tor 20190228 User has add notes permission
                    hasAddCommentAssignmentToWorkitem =
                        hasUserWorkitemAssignment 
                        || hasUserAssignmentToWorkitem 
                        || hasRoleAssignmentToWorkitem 
                        || hasEditAssignmentToWorkitem 
                        || isWorkitemParticipant 
                        || (!myDataSet.GAWorkitem[0].IsExtra2Null() 
                            && myDataSet.GAWorkitem[0].Extra2.Contains(";" + personnelId.ToString() + ";")) // is added person ?
                        || (!myDataSet.GAWorkitem[0].IsExtra4Null() // has added titles ?
                            && workitem.IsPersonnelAddedRoleForWorkItem(personnelId, this.WorkitemId)); //check if user has added role
                }
				//set edit link url
				hyperLinkEdit.NavigateUrl = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordDetails(GADataClass.GAWorkitem.ToString(), WorkitemId.ToString());

                // Tor 20140917 avoid edit workitem because it will fail 
                // hyperLinkEdit.Visible = HasEditPermission;
                hyperLinkEdit.Visible = false;

                // Tor 20150108 added test on delegate, add participant, add comment, if workitem is active and workitem is not a workflow start workitem
                lnkDelegate.Visible = (hasUserWorkitemAssignment || HasEditPermission)
// Tor 20170831                    && !GASystem.BusinessLayer.Workitem.getIfWorkitemIsActive(this.WorkitemId);
                    && GASystem.BusinessLayer.Workitem.getIfWorkitemIsActive(this.WorkitemId);

                lnkAddParticipant.Visible = hasUserWorkitemAssignment || HasEditPermission;
                lnkAddComment.Visible = hasUserWorkitemAssignment || HasEditPermission || isWorkitemParticipant 
                    || hasAddCommentAssignmentToWorkitem // Tor 20190301 added
                    ;
                lnkCloseWorkitem.Visible = hasUserWorkitemAssignment;    // hasEditPermissionsOnRecord;
                lnkCloseNCWorkitem.Visible = hasUserWorkitemAssignment && ShowCloseNCWorkitem(myDataSet);
                if (lnkCloseNCWorkitem.Visible)
                {
                    lnkCloseWorkitem.Visible = false;
                }
                
                // Tor 20150108 user must have permission and workitem must have added participants and workitem must be active and not a workflowStart workitem
                hasAddedParticipants = false;
                // Tor 20170627 compress test
                //if (!myDataSet.GAWorkitem[0].IsExtra2Null())
                //{
                //    if (myDataSet.GAWorkitem[0].Extra2.ToString() != string.Empty)
                //        hasAddedParticipants = true;
                //}
                //if (!myDataSet.GAWorkitem[0].IsExtra4Null())
                //{
                //    if (myDataSet.GAWorkitem[0].Extra4.ToString() != string.Empty)
                //        hasAddedParticipants = true;
                //}

                if (!myDataSet.GAWorkitem[0].IsExtra2Null() && myDataSet.GAWorkitem[0].Extra2.ToString() != string.Empty)
                    hasAddedParticipants = true;
                else
                    if (!myDataSet.GAWorkitem[0].IsExtra4Null() && myDataSet.GAWorkitem[0].Extra4.ToString() != string.Empty)
                        hasAddedParticipants = true;

                lnkRemoveAddedParticipants.Visible = 
                    (hasUserWorkitemAssignment || hasRoleAssignmentToWorkitem) 
                    && hasAddedParticipants
                    // Tor 20170627 && !GASystem.BusinessLayer.Workitem.getIfWorkitemIsActive(this.WorkitemId);
                    && GASystem.BusinessLayer.Workitem.getIfWorkitemIsActive(this.WorkitemId);

				int actionId = myDataSet.GAWorkitem[0].ActionRowId;


				System.Collections.ArrayList rowids = new System.Collections.ArrayList();

				//display action info
				//verify that gaaction record exists. May not be replicated yet.
				DataModel.GADataRecord actionRecord = new GASystem.DataModel.GADataRecord(actionId, DataModel.GADataClass.GAAction);
				if (BusinessLayer.Utils.RecordSetUtils.DoesRecordExist(actionRecord)) 
				{
					DataModel.ActionDS ads = Action.GetActionByActionRowId(actionId);

                    //lblWorkitemReaction.Text = AppUtils.Localization.GetCaptionText("WorkitemReaction");

				}
		

				try 
				{
					

					if (HasEditPermission) 
					{
					
						//string actorsReply = _wi.attributes[new openwfe.workitem.StringAttribute(BusinessLayer.Workitem.ACTORSREPLY)].ToString();
						string actorsReply = myDataSet.GAWorkitem[0].ActorsReply;
						
						if (actorsReply != string.Empty && workitemCompletionStatus == GASystem.BusinessLayer.Workitem.WorkitemStatus.Active.ToString())   //update visible buttons settings if ACTORSREPLY attribute exists and is not empty
						{
                            //btnNo.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.no.ToString()) > -1;
                            //btnOk.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.ok.ToString()) > -1;
                            //btnReject.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.rejected.ToString()) > -1;
                            //btnYes.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.yes.ToString()) > -1;
                            //btnCompleted.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.completed.ToString()) > -1;
							lnkAddParticipant.Visible = HasEditPermission;
							lnkDelegate.Visible = HasEditPermission;
                            lnkCloseWorkitem.Visible = MyCloseWorkitem.UserCanCloseWorkitem;
                            lnkCloseNCWorkitem.Visible = MyCloseNCWorkitem.UserCanCloseWorkitem && ShowCloseNCWorkitem(myDataSet);
                            if (lnkCloseNCWorkitem.Visible)
                            {
                                lnkCloseWorkitem.Visible = false;
                            }
                            lnkAddComment.Visible = HasEditPermission || isWorkitemParticipant;
                            // tor 20141108 if workitem has added participants, and user has edit permission the add remove added participants button
                            // set link/button visible if workitem has either added participant persons (Extra2) or roles (Extra4)
                            //string a = myDataSet.GAWorkitem[0].Extra2.ToString();
                            //string b = myDataSet.GAWorkitem[0].Extra4.ToString();
                            lnkRemoveAddedParticipants.Visible = 
                                (hasUserWorkitemAssignment || hasRoleAssignmentToWorkitem) 
                                && hasAddedParticipants;
						}
						//TODO create and set commands dynamically in oninit
                    }

                    if (!hasUserWorkitemAssignment && workitemCompletionStatus == GASystem.BusinessLayer.Workitem.WorkitemStatus.Active.ToString())
                    {
                        lblReactionStatus.Text = "This workitem is open and assigned to the Job Title or Person below";
                        lblReactionStatus.Visible = true;
                    }

                //    MyWorkitemAcknowledgement.DoesUserOwnWorkitem = false;
                    if (hasUserWorkitemAssignment && workitemCompletionStatus == GASystem.BusinessLayer.Workitem.WorkitemStatus.Active.ToString())
                    {
                       // MyWorkitemAcknowledgement.DoesUserOwnWorkitem = true;
                        if (hasEditAssignmentToWorkitem)
                            lblReactionStatus.Text = GASystem.AppUtils.Localization.GetGuiElementText("hasEditAssignmentToWorkitem");
                        if (hasRoleAssignmentToWorkitem)
                            lblReactionStatus.Text = GASystem.AppUtils.Localization.GetGuiElementText("hasRoleAssignmentToWorkitem");
                        if (hasUserAssignmentToWorkitem)
                            lblReactionStatus.Text = GASystem.AppUtils.Localization.GetGuiElementText("hasUserAssignmentToWorkitem");
                         lblReactionStatus.Visible = true;
                    }
                    


					if (workitemCompletionStatus != GASystem.BusinessLayer.Workitem.WorkitemStatus.Active.ToString()) 
					{
                        if (!myDataSet.GAWorkitem[0].IsReplyResultNull())
                            lblReactionStatus.Text = string.Format(AppUtils.Localization.GetGuiElementText("WorkitemClosedWithResponse"), AppUtils.Localization.GetGuiElementText(myDataSet.GAWorkitem[0].ReplyResult.ToString()));
                        else
                            lblReactionStatus.Text = AppUtils.Localization.GetGuiElementText("WorkitemCompleted");
                        lblReactionStatus.Visible = true;
						lnkAddParticipant.Visible = false;
						lnkDelegate.Visible = false;
                        lnkCloseWorkitem.Visible = false;
                        lnkAddComment.Visible = false;
					}

                 



                    //set display on notes box. display only when the workitem is of type remedial
                    bool workitemIsRemedial = false;
                    if (!myDataSet.GAWorkitem[0].IsTextFree1Null())
                        workitemIsRemedial = myDataSet.GAWorkitem[0].TextFree1 == Workitem.WorkitemType.Remedial.ToString();

                    MyCloseWorkitem.DisplayNotes = workitemIsRemedial;

                    setAcknowledgementVisibility();
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
			base.OnPreRender (e);
		}

        private bool ShowCloseNCWorkitem(GASystem.DataModel.WorkitemDS ds)
        {
            if (ds.GAWorkitem[0].IsWorkflowNameNull())
            {
                return false;
            }
            if (ds.GAWorkitem[0].WorkflowName == "NonConformanceWorkflow_v1" || 
                ds.GAWorkitem[0].WorkflowName == "NonConformanceWorkflow_v2")
            {
                return false;
            }

            return ds.GAWorkitem[0].WorkflowName.Contains("NonConformanceWorkflow") &&
                   ds.GAWorkitem[0].TextFree1 == "Remedial";
        }

        private bool ShowRejectNCWorkitem(GASystem.DataModel.WorkitemDS ds)
        {
            return ShowCloseNCWorkitem(ds) &&
                ds.GAWorkitem[0].ExpressionId == "0.1.0.17" &&
                ds.GAWorkitem[0].ReplyResult != "dissatisfied";
        }

        private bool ShowRejectWorkitem(GASystem.DataModel.WorkitemDS ds)
        {
            if (ds.GAWorkitem[0].WorkflowName.StartsWith("NonConformanceWorkflow") && ds.GAWorkitem[0].ReplyResult == "dissatisfied")
                return false;
            return true;
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
			this.lnkDelegate.Click += new System.EventHandler(this.lnkDelegate_Click);
			this.lnkAddParticipant.Click += new System.EventHandler(this.lnkAddParticipant_Click);
            this.lnkAddComment.Click += new EventHandler(lnkAddComment_Click);
            this.lnkCloseWorkitem.Click += new EventHandler(lnkCloseWorkitem_Click);
            this.lnkCloseNCWorkitem.Click += new EventHandler(lnkCloseNCWorkitem_Click);
            this.lnkAcknowledgeWorkitem.Click += new EventHandler(lnkAcknowledgeWorkitem_Click);
            this.lnkRejectAcknowledgeWorkitem.Click += new EventHandler(lnkRejectAcknowledgeWorkitem_Click);
            this.lnkRejectAcknowledgeNCWorkitem.Click += new EventHandler(lnkRejectAcknowledgeNCWorkitem_Click);
            this.lnkRemoveAddedParticipants.Click += new EventHandler(lnkRemoveAddedParticipants_Click);
            this.Load += new System.EventHandler(this.Page_Load);
            
		}

       

       
		#endregion

		

		private void lnkDelegate_Click(object sender, System.EventArgs e)
		{
            setVisibleSubControl(statusSubControls.delegateWorkitem);
		}

		private void lnkAddParticipant_Click(object sender, System.EventArgs e)
		{
            setVisibleSubControl(statusSubControls.addParticipant);
		}

        void lnkCloseWorkitem_Click(object sender, EventArgs e)
        {
            setVisibleSubControl(statusSubControls.closeWorkitem);
        }

        void lnkCloseNCWorkitem_Click(object sender, EventArgs e)
        {
            setVisibleSubControl(statusSubControls.closeNCWorkitem);
            this.MyCloseNCWorkitem.IsFromRejectAcknowledge = false;
        }

        void lnkAddComment_Click(object sender, EventArgs e)
        {
            setVisibleSubControl(statusSubControls.addComment);
        }

        void lnkRejectAcknowledgeNCWorkitem_Click(object sender, EventArgs e)
        {
            setVisibleSubControl(statusSubControls.closeNCWorkitem);
            this.MyCloseNCWorkitem.IsFromRejectAcknowledge = true;
        }

        // Tor 20150108 added
        void lnkRemoveAddedParticipants_Click(object sender, EventArgs e)
        {
            GASystem.BusinessLayer.Workitem.RemoveAddedParticipants(this.WorkitemId);
            hasAddedParticipants = false;
            lnkRemoveAddedParticipants.Visible = false;
            MyViewDataRecord.RecordDataSet = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsByDataRecord(new GADataRecord(this.WorkitemId, GADataClass.GAWorkitem)); //DataRecordSet;
            MyViewDataRecord.ClearForm();
            MyViewDataRecord.SetupForm("tor");
            BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS wds = (WorkitemDS)bc.GetByRowId(this.WorkitemId);
            this.DataRecordSet = wds;
            //            setVisibleSubControl(statusSubControls.removeAddedParticipants);
        }

        void lnkRejectAcknowledgeWorkitem_Click(object sender, EventArgs e)
        {
            GASystem.BusinessLayer.Workitem.RejectAcknowledgeWorkitem(this.WorkitemId);
            AcknowledgementStatus = GASystem.BusinessLayer.Workitem.AcknowledgementStatus.AcknowledgementRejected;
            // tor 20150211 start: added code below to show updated performed above
            MyViewDataRecord.RecordDataSet = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsByDataRecord(new GADataRecord(this.WorkitemId, GADataClass.GAWorkitem)); //DataRecordSet;
            MyViewDataRecord.ClearForm();
            MyViewDataRecord.SetupForm("tor");
            BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS wds = (WorkitemDS)bc.GetByRowId(this.WorkitemId);
            this.DataRecordSet = wds;
            // tor 20150211 end: added code below to show updated performed above
        }

        void lnkAcknowledgeWorkitem_Click(object sender, EventArgs e)
        {
            GASystem.BusinessLayer.Workitem.AcknowledgeWorkitem(this.WorkitemId);
            AcknowledgementStatus = GASystem.BusinessLayer.Workitem.AcknowledgementStatus.Acknowledged;
            // tor 20150211 start: added code below to show updated performed above
            MyViewDataRecord.RecordDataSet = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsByDataRecord(new GADataRecord(this.WorkitemId, GADataClass.GAWorkitem)); //DataRecordSet;
            MyViewDataRecord.ClearForm();
            MyViewDataRecord.SetupForm("tor");
            BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS wds = (WorkitemDS)bc.GetByRowId(this.WorkitemId);
            this.DataRecordSet = wds;
            // tor 20150211 end: added code below to show updated performed above
        }

        private GASystem.BusinessLayer.Workitem.AcknowledgementStatus _ackStatus;

        public GASystem.BusinessLayer.Workitem.AcknowledgementStatus AcknowledgementStatus
        {
            get { return _ackStatus; }
            set { _ackStatus = value; }
        }


        private void setVisibleSubControl(statusSubControls subControl)
        {
            this.MyAddWorkitemParticipant.Visible = subControl == statusSubControls.addParticipant;
            this.MyDelegateWorkitem.Visible = subControl == statusSubControls.delegateWorkitem;
            this.MyCloseWorkitem.Visible = subControl == statusSubControls.closeWorkitem;
            this.MyCloseNCWorkitem.Visible = subControl == statusSubControls.closeNCWorkitem;
            this.MyAddWorkitemComment.Visible = subControl == statusSubControls.addComment;
        }

        private void setAcknowledgementVisibility()
        {
            bool doesUserOwnWorkitem = false;
            if (workitemCompletionStatus == GASystem.BusinessLayer.Workitem.WorkitemStatus.Active.ToString())
            {
                if (hasEditAssignmentToWorkitem)
                    doesUserOwnWorkitem = false;
                if (hasRoleAssignmentToWorkitem)
                    doesUserOwnWorkitem = true;
                if (hasUserAssignmentToWorkitem)
                    doesUserOwnWorkitem = true;
            }
            
            if (_ackStatus == GASystem.BusinessLayer.Workitem.AcknowledgementStatus.Acknowledged)
            {
                lblAcknowledgeStatus.Text = "This workitem is acknowledged";
                lblAcknowledgeStatus.Visible = true;
                lnkAcknowledgeWorkitem.Visible = false;
                lnkRejectAcknowledgeWorkitem.Visible = false;
                lnkRejectAcknowledgeNCWorkitem.Visible = false;
            }

            if (_ackStatus == GASystem.BusinessLayer.Workitem.AcknowledgementStatus.NoAcknowledgementNeeded)
            {
                lnkAcknowledgeWorkitem.Visible = false;
                lnkRejectAcknowledgeWorkitem.Visible = false;
                lnkRejectAcknowledgeNCWorkitem.Visible = false;
            }
            // Tor 20170627 do not display message if user does not own Workitem
            // if (_ackStatus == GASystem.BusinessLayer.Workitem.AcknowledgementStatus.AwaitingAcknowledgement)
            if (doesUserOwnWorkitem && _ackStatus == GASystem.BusinessLayer.Workitem.AcknowledgementStatus.AwaitingAcknowledgement)
            {
                lblAcknowledgeStatus.Text = "You need to acknowlegde this workitem";
                lblAcknowledgeStatus.Visible = true;
                lnkAcknowledgeWorkitem.Visible = true;
                lnkRejectAcknowledgeWorkitem.Visible = true && ShowRejectWorkitem(myDataSet);
                lnkRejectAcknowledgeNCWorkitem.Visible = ShowRejectNCWorkitem(myDataSet);
                if (lnkRejectAcknowledgeNCWorkitem.Visible)
                {
                    lnkRejectAcknowledgeWorkitem.Visible = false;
                }
            }

            // Tor 20170627 do not display message if user does not own Workitem
            // if (_ackStatus == GASystem.BusinessLayer.Workitem.AcknowledgementStatus.AcknowledgementRejected)
            if (doesUserOwnWorkitem && _ackStatus == GASystem.BusinessLayer.Workitem.AcknowledgementStatus.AcknowledgementRejected)
            {
                lblAcknowledgeStatus.Text = "Acknowledgment rejected";
                lblAcknowledgeStatus.Visible = true;
                lnkAcknowledgeWorkitem.Visible = false;
                lnkRejectAcknowledgeWorkitem.Visible = false;
                lnkRejectAcknowledgeNCWorkitem.Visible = false;
            }

            //does user have acknowledge permission
            if (!doesUserOwnWorkitem)
            {
                lnkAcknowledgeWorkitem.Visible = false;
                lnkRejectAcknowledgeWorkitem.Visible = false;
                lnkRejectAcknowledgeNCWorkitem.Visible = false;
                return;
            }
        }

		public int WorkitemId
		{
			set 
			{ 
				_workitemId = value;
				

			}
            get { return _workitemId; }
		} 

//		public openwfe.workitem.InFlowWorkitem Workitem
//		{
//			set { 
//				_wi = value;
//				}
//			get {return _wi;}
//		} 

		public GASystem.DataModel.WorkitemDS DataRecordSet 
		{
			set {myDataSet = value;}
			get {return myDataSet;}
		}

		public string Message 
		{
			set { lblMessage.Text = value;}
			get {return lblMessage.Text;}
		}

		public string ErrorMessage 
		{
			set { lblErrorMessage.Text = value;}
			get {return lblErrorMessage.Text;}
		}

		public bool HasEditPermission 
		{
			set {_hasEditPermissions = value;}
			get {return _hasEditPermissions;}
		}

        public bool HasEditPermissionsOnRecord
        {
            set { hasEditPermissionsOnRecord = value; }
            get { return hasEditPermissionsOnRecord; }
        }
	}
}
