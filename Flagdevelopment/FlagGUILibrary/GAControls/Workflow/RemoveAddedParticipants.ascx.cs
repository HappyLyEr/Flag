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

    public class RemoveAddedParticipants : System.Web.UI.UserControl
    {
        protected Button removeAddedParticipants;
        protected Label lblRemoveAddedParticipantsMsg;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            removeAddedParticipants.Click += new EventHandler(removeAddedParticipants_Click);
        }

        void removeAddedParticipants_Click(object sender, EventArgs e)
        {
            GASystem.BusinessLayer.Workitem.RemoveAddedParticipants(this.WorkitemID);
        }

        private int _workitemId;
    
        public int WorkitemID
        {
            get { return _workitemId; }
            set { _workitemId = value; }
        }

        //private GASystem.BusinessLayer.Workitem.AcknowledgementStatus _ackStatus;

        //public GASystem.BusinessLayer.Workitem.AcknowledgementStatus AcknowledgementStatus
        //{
        //    get { return _ackStatus; }
        //    set { _ackStatus = value; }
        //}

        private bool _userOwnsWorkitem;

        public bool DoesUserOwnWorkitem
        {
            get { return _userOwnsWorkitem; }
            set { _userOwnsWorkitem = value; }
        }
	

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // Tor 20150107 set visible=false 
            //if user does not have acces or there are no added participants or workitem is not active or workitemtype is WorkflowStart

            if (!DoesUserOwnWorkitem
                || (GASystem.BusinessLayer.Workitem.getAddedParticipants(WorkitemID).ToString() == string.Empty && GASystem.BusinessLayer.Workitem.getAddedParticipantRoles(WorkitemID).ToString() == string.Empty)
                || !GASystem.BusinessLayer.Workitem.getIfWorkitemIsActive(WorkitemID))
            {
                this.Visible = false;
                return;
            }
        }
    }
}