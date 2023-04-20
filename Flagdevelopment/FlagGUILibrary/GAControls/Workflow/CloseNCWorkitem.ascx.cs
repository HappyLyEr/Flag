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
    using GASystem.BusinessLayer.Utils;

    /// <summary>
    ///	当关闭Non-Conformance Workitem时进行处理的用户控件
    /// </summary>
    public class CloseNCWorkitem : System.Web.UI.UserControl
    {
        protected System.Web.UI.WebControls.Panel Panel1;
        protected System.Web.UI.WebControls.Label lblMessage;
        protected System.Web.UI.WebControls.PlaceHolder placeHolderCloseWorkitem;

        protected System.Web.UI.WebControls.Button btnCompleted;
        protected System.Web.UI.WebControls.Button btnReject;
        protected System.Web.UI.WebControls.Button btnDissatisfy;

        protected System.Web.UI.WebControls.TableRow tableRowFinding;
        protected System.Web.UI.WebControls.Label lblFinding;
        protected System.Web.UI.WebControls.TextBox tbFinding;

        protected System.Web.UI.WebControls.TableRow tableRowCorrection;
        protected System.Web.UI.WebControls.Label lblCorrection;
        protected System.Web.UI.WebControls.RequiredFieldValidator tbCorrectionValidator;
        protected System.Web.UI.WebControls.TextBox tbCorrection;

        protected System.Web.UI.WebControls.TableRow tableRowCorrectiveAction;
        protected System.Web.UI.WebControls.Label lblCorrectiveAction;
        protected System.Web.UI.WebControls.RequiredFieldValidator tbCorrectiveActionValidator;
        protected System.Web.UI.WebControls.TextBox tbCorrectiveAction;

        protected System.Web.UI.WebControls.TableRow tableRowReviewofCorrectiveActions;
        protected System.Web.UI.WebControls.Label lblReviewofCorrectiveActions;
        protected System.Web.UI.WebControls.RequiredFieldValidator tbReviewofCorrectiveActionsValidator;
        protected System.Web.UI.WebControls.TextBox tbReviewofCorrectiveActions;

        protected System.Web.UI.WebControls.TableRow tableRowReasons;
        protected System.Web.UI.WebControls.Label lblReasons;
        protected System.Web.UI.WebControls.RequiredFieldValidator tbReasonsValidator;
        protected System.Web.UI.WebControls.TextBox tbReasons;

        protected System.Web.UI.WebControls.Label lblWorkitemReaction;

        protected System.Web.UI.WebControls.TableRow tableRowInfo;
        protected System.Web.UI.WebControls.PlaceHolder InfoPlaceHolder;

        private GASystem.DataModel.WorkitemDS myDataSet;
        private int _workitemId;
        private bool canCloseWorkitem = false;
        private bool _isFromRejectAcknowledge = false;
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
        }
        #endregion

        public int WorkitemId
        {
            get { return _workitemId; }
            set { _workitemId = value; }
        }

        public GASystem.DataModel.WorkitemDS DataRecordSet
        {
            set { myDataSet = value; }
            get { return myDataSet; }
        }

        public bool UserCanCloseWorkitem
        {
            get { return canCloseWorkitem; }
        }

        /// <summary>
        /// 是否是从 Reject Acknowledge	点击过来的
        /// </summary>
        public bool IsFromRejectAcknowledge
        {
            get { return _isFromRejectAcknowledge; }
            set { _isFromRejectAcknowledge = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            btnCompleted.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.completed.ToString());
            btnReject.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.rejected.ToString());
            btnDissatisfy.Text = AppUtils.Localization.GetGuiElementText(GASystem.BusinessLayer.Workitem.ActorsReply.dissatisfied.ToString());
            lblFinding.Text = AppUtils.Localization.GetCaptionText("WhatIsTheNonConformity");
            lblCorrection.Text = AppUtils.Localization.GetCaptionText("WhatDidYouDoNC");
            lblCorrectiveAction.Text = AppUtils.Localization.GetCaptionText("CorrectiveAction");
            lblReviewofCorrectiveActions.Text = AppUtils.Localization.GetCaptionText("RvwCrctvActns");
            lblReasons.Text = AppUtils.Localization.GetCaptionText("ReasonsToRejectNC");
            try
            {
                int actionId = myDataSet.GAWorkitem[0].ActionRowId;
                GADataRecord actionOwner = BusinessLayer.DataClassRelations.GetOwner(new GADataRecord(actionId, GADataClass.GAAction));
                DataSet nocDS = BusinessLayer.BusinessClass.GetByGADataRecord(actionOwner, null);                

                tbFinding.Text = nocDS.Tables[0].Rows[0]["WhatIsTheIssue"].ToString();
                tbCorrection.Text = nocDS.Tables[0].Rows[0]["WhatSomething"].ToString();
                tbCorrectiveAction.Text = nocDS.Tables[0].Rows[0]["WhatDoYouRecommend"].ToString();
                tbReviewofCorrectiveActions.Text = nocDS.Tables[0].Rows[0]["QAManagerResponse"].ToString();
                lblWorkitemReaction.Text = AppUtils.Localization.GetCaptionText("WorkitemReaction");

                if (actionOwner != null)
                {
                    ArrayList members = GASystem.BusinessLayer.DataClassRelations.GetNextLevelInViewDataClasses(actionOwner);
                    foreach (string member in members)
                    {
                        GADataClass memberDataClass = GADataRecord.ParseGADataClass(member);
                        if (memberDataClass == GADataClass.GAFileContent)
                        {
                            this.InfoPlaceHolder.Controls.Add(new ViewDetailsList.GeneralViewDetail(actionOwner, memberDataClass));
                        }
                    }
                }

                SetTableRowVisiable();

                try
                {
                    string workitemCompletionStatus = myDataSet.GAWorkitem[0].WorkitemStatus;

                    //string actorsReply = _wi.attributes[new openwfe.workitem.StringAttribute(BusinessLayer.Workitem.ACTORSREPLY)].ToString();
                    string actorsReply = myDataSet.GAWorkitem[0].ActorsReply;

                    if (actorsReply != string.Empty && workitemCompletionStatus == GASystem.BusinessLayer.Workitem.WorkitemStatus.Active.ToString())
                    //update visible buttons settings if ACTORSREPLY attribute exists and is not empty
                    {
                        //btnCompleted.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.completed.ToString()) > -1;
                        //btnReject.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.rejected.ToString()) > -1;
                        btnDissatisfy.Visible = actorsReply.IndexOf(BusinessLayer.Workitem.ActorsReply.dissatisfied.ToString()) > -1;
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

        private void SetTableRowVisiable()
        {
            // 发起人initiator 显示逻辑
            if (myDataSet.GAWorkitem[0].ExpressionId == "0.2.0.11")
            {
                tbCorrectionValidator.Enabled = false;
                tbCorrection.ReadOnly = true;
                tbCorrectiveActionValidator.Enabled = false;
                tbCorrectiveAction.ReadOnly = true;
                tbReasonsValidator.Enabled = false;
                tableRowReasons.Visible = false;
            }

            // Responsable Job Title 显示逻辑
            if (myDataSet.GAWorkitem[0].ExpressionId == "0.1.0.17")
            {
                if (myDataSet.GAWorkitem[0].ReplyResult == "dissatisfied")
                {
                    // 不满足条件退回来的
                    tbReviewofCorrectiveActionsValidator.Enabled = false;
                    tbReviewofCorrectiveActions.ReadOnly = true;
                    tbReasonsValidator.Enabled = false;
                    tableRowReasons.Visible = false;
                }
                else
                {
                    if (IsFromRejectAcknowledge)
                    {
                        tableRowInfo.Visible = false;
                        tbCorrectionValidator.Enabled = false;
                        tableRowCorrection.Visible = false;
                        tbCorrectiveActionValidator.Enabled = false;
                        tableRowCorrectiveAction.Visible = false;
                        tbReasonsValidator.Enabled = true;
                        tableRowReasons.Visible = true;
                        btnCompleted.Visible = false;
                        btnReject.Visible = true;
                    }
                    else
                    {
                        tableRowInfo.Visible = true;
                        tbCorrectionValidator.Enabled = true;
                        tableRowCorrection.Visible = true;
                        tbCorrectiveActionValidator.Enabled = true;
                        tableRowCorrectiveAction.Visible = true;
                        tbReasonsValidator.Enabled = false;
                        tableRowReasons.Visible = false;
                        btnCompleted.Visible = true;
                        btnReject.Visible = false;
                    }
                    // 第一次处理
                    tbReviewofCorrectiveActionsValidator.Enabled = false;
                    tableRowReviewofCorrectiveActions.Visible = false;
                }
            }

            if (tableRowReviewofCorrectiveActions.Visible && tbReviewofCorrectiveActions.ReadOnly == false)
            {
                tbReviewofCorrectiveActions.Text = "";
            }
        }


        #region Forward task

        private void ForwardTask(string Result)
        {
            if (!this.Page.IsValid)
                return;

            int actionId = myDataSet.GAWorkitem[0].ActionRowId;
            GADataRecord actionOwner = BusinessLayer.DataClassRelations.GetOwner(new GADataRecord(actionId, GADataClass.GAAction));
            DataSet nocDS = BusinessLayer.BusinessClass.GetByGADataRecord(actionOwner, null); 
            string note = "";
            if (tbReviewofCorrectiveActions.Visible)
            {
                note = tbReviewofCorrectiveActions.Text;
            }
            else if (tbCorrection.Visible && tbCorrectiveAction.Visible)
            {
                note = tbCorrection.Text + "\n" + tbCorrectiveAction.Text;
            }
            else if (tbReasons.Visible)
            {
                note = tbReasons.Text;
            }

            if (tbReviewofCorrectiveActions.Visible)
            {
                nocDS.Tables[0].Rows[0]["QAManagerResponse"] = tbReviewofCorrectiveActions.Text;
            }
            if (tbCorrection.Visible && tbCorrectiveAction.Visible)
            {
                nocDS.Tables[0].Rows[0]["WhatSomething"] = tbCorrection.Text;
                nocDS.Tables[0].Rows[0]["WhatDoYouRecommend"] = tbCorrectiveAction.Text;
            }
            if (tbReasons.Visible)
            {
                nocDS.Tables[0].Rows[0]["nTextFree3"] = tbReasons.Text;
            }
            BusinessClass bc = RecordsetFactory.Make(GADataClass.GANonConformanceView);
            bc.CommitDataSet(nocDS);
            GASystem.BusinessLayer.Workitem.ProceedWorkFlow(WorkitemId, Result, note);
            //		this.Page.Cache.Remove(GASystem.BusinessLayer.Workitem.WORKITEM_CACHE);  //remove the workitems from cache, forces a refresh
            PageDispatcher.GotoDataRecordViewPage(this.Page.Response, GADataClass.GAWorkitem, WorkitemId, null);

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

        #endregion
    }
}
