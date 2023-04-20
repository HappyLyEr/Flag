using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace GASystem.GAControls.Workflow
{
    [ToolboxData("<{0}:WorkitemNotifierWebControl runat=server></{0}:WorkitemNotifierWebControl>")]
    public class WorkitemNotifierSingleWebControl : WebControl
    {
        // Tor 201611 Security 20161122 (never referenced) private int _workitemId;
        // Tor 201611 Security 20161122 (never referenced) private GADataRecord _dataRecord;
        private WorkitemDS.GAWorkitemRow wrow;
        private LinkButton linkCloseWorkitem = new LinkButton();
        private bool _displayWorkitemCompletion = false;
        private Label subject = new Label();
        private Label notes = new Label();
        private CloseWorkitem closeWorkitem;
        private Panel notifierPanel = new Panel();

        public WorkitemNotifierSingleWebControl(WorkitemDS.GAWorkitemRow workitemRow)
            : base()
        {

            wrow = workitemRow;
          
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DisplayWorkitemInfo();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
  
            notifierPanel.Style.Add("border-right", "#dde0e5 0px solid;");
            notifierPanel.Style.Add("border-top", "#dde0e5 1px solid;");
            notifierPanel.Style.Add("border-left", "#dde0e5 0px solid;");
            notifierPanel.Style.Add("border-bottom", "#dde0e5 1px solid;");
           


            notifierPanel.Style.Add(HtmlTextWriterStyle.Padding, "3px 3px 3px 3px");
            notifierPanel.Style.Add(HtmlTextWriterStyle.Margin, "3px 0px 3px 0px");



            this.Controls.Add(notifierPanel);

            notifierPanel.Controls.Add(subject);
            notifierPanel.Controls.Add(notes);
            notifierPanel.Controls.Add(linkCloseWorkitem);
            linkCloseWorkitem.Click += new EventHandler(linkCloseWorkitem_Click);
        }

        private void DisplayWorkitemInfo()
        {
            subject.Text = "<bold>" + wrow.Subject + "</bold><br/>";
            notes.Text = wrow.Notes + "<br/>"; 
            linkCloseWorkitem.Text = "Close Workitem";
            linkCloseWorkitem.CssClass = "FlagLinkButton";

            closeWorkitem = (CloseWorkitem)this.Page.LoadControl("~/gagui/GAControls/Workflow/closeworkitem.ascx");

            closeWorkitem.WorkitemId = wrow.WorkitemRowId;
            BusinessClass wbc = BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);

            closeWorkitem.DataRecordSet = (WorkitemDS)wbc.GetByRowId(wrow.WorkitemRowId);


            notifierPanel.Controls.Add(closeWorkitem);
            closeWorkitem.Visible = _displayWorkitemCompletion;
            
        }

        void linkCloseWorkitem_Click(object sender, EventArgs e)
        {
            _displayWorkitemCompletion = true;
        }

        protected override void OnPreRender(EventArgs e)
        {
           
            base.OnPreRender(e);

            if (closeWorkitem != null)
                closeWorkitem.Visible = _displayWorkitemCompletion;

        }
      
    }
}
