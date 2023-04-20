using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GASystem.BusinessLayer.Documents;

namespace GASystem.GAGUI.GUIUtils
{
    [ToolboxData("<{0}:DocumentReviewFrontController runat=server></{0}:DocumentReviewFrontController>")]
    public class DocumentReviewFrontController : WebControl
    {
        private Label lblDocumentName;
        private DocumentInfo myDocInfo;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            lblDocumentName.Text = DocInfo.DisplayName;

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            lblDocumentName = new Label();
            this.Controls.Add(lblDocumentName);
        }

        public DocumentInfo DocInfo
        {
            set { myDocInfo = value; }
            get { return myDocInfo; }
        }

    }
}
