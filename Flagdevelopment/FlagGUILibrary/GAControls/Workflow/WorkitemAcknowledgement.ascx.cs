namespace GASystem.GAControls.Workflow
{
    using System;
    using System.Data;
    using System.Configuration;
    using System.Collections;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Web.UI.HtmlControls;

    public class WorkitemAcknowledgement : System.Web.UI.UserControl
    {
        protected Button acknowledge;
        protected Button acknowledgeReject;
        protected Label lblAcknowledgeMsg;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            acknowledge.Click += new EventHandler(acknowledge_Click);
            acknowledgeReject.Click += new EventHandler(acknowledgeReject_Click);

        }

        void acknowledgeReject_Click(object sender, EventArgs e)
        {
            GASystem.BusinessLayer.Workitem.RejectAcknowledgeWorkitem(this.WorkitemID);
            AcknowledgementStatus = GASystem.BusinessLayer.Workitem.AcknowledgementStatus.AcknowledgementRejected;
            
           
        }

        void acknowledge_Click(object sender, EventArgs e)
        {
            GASystem.BusinessLayer.Workitem.AcknowledgeWorkitem(this.WorkitemID);
            AcknowledgementStatus = GASystem.BusinessLayer.Workitem.AcknowledgementStatus.Acknowledged;
        }

        private int _workitemId;
    
        public int WorkitemID
        {
            get { return _workitemId; }
            set { _workitemId = value; }
        }

        private GASystem.BusinessLayer.Workitem.AcknowledgementStatus _ackStatus;

        public GASystem.BusinessLayer.Workitem.AcknowledgementStatus AcknowledgementStatus
        {
            get { return _ackStatus; }
            set { _ackStatus = value; }
        }

        private bool _userOwnsWorkitem;

        public bool DoesUserOwnWorkitem
        {
            get { return _userOwnsWorkitem; }
            set { _userOwnsWorkitem = value; }
        }
	

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // if (!DoesUserOwnWorkitem) 
            // Tor 20150107 do not show if workitemtype="workflowengine" or workitem is not active
            if (!DoesUserOwnWorkitem || !GASystem.BusinessLayer.Workitem.getIfWorkitemIsActive(this.WorkitemID))
            {
                this.Visible = false;
                return;
            }


            
            if (_ackStatus == GASystem.BusinessLayer.Workitem.AcknowledgementStatus.Acknowledged)
            {
                lblAcknowledgeMsg.Text = "This workitem is acknowledged";
                acknowledge.Visible = false;
                acknowledgeReject.Visible = false;
            }
            if (_ackStatus == GASystem.BusinessLayer.Workitem.AcknowledgementStatus.NoAcknowledgementNeeded)
            {
                this.Visible = false;
            }
            if (_ackStatus == GASystem.BusinessLayer.Workitem.AcknowledgementStatus.AwaitingAcknowledgement)
            {
                lblAcknowledgeMsg.Text = "You need to acknowlegde this workitem";
            }
            if (_ackStatus == GASystem.BusinessLayer.Workitem.AcknowledgementStatus.AcknowledgementRejected)
            {
                lblAcknowledgeMsg.Text = "Acknowledgment rejected";
                acknowledge.Visible = false;
                acknowledgeReject.Visible = false;
            }


        }

    }
}