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
    using GASystem.GUIUtils;
	/// <summary>
	///		Summary description for AddworkitemComment
	/// </summary>
    public class AddWorkitemComment : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Panel Panel1;
        protected System.Web.UI.WebControls.Button btnCancel;
        protected System.Web.UI.WebControls.Button btnSave;
        protected System.Web.UI.WebControls.TextBox commentText;
		protected System.Web.UI.WebControls.Label lblMessage;
	    protected System.Web.UI.WebControls.PlaceHolder placeHolderCloseWorkitem;
        protected System.Web.UI.WebControls.RequiredFieldValidator commentTextValidator;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		//	AddLookupField();
		//	AddRoles();
			
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
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnCancel.Click += new System.EventHandler(this.LinkButton1_Click);
			

		}
		#endregion

        public int WorkitemId 
		{
			get {return (int)ViewState["workitemid"];}
			set {ViewState["workitemid"] = value;}
		}


        private void LinkButton1_Click(object sender, System.EventArgs e)
        {
            this.Visible = false;
            //Linkbutton2.Visible = true;
        }


        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (this.Page.IsValid)
            {
                BusinessLayer.Workitem wbc = (BusinessLayer.Workitem)BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);
                wbc.addNotes(this.commentText.Text, this.WorkitemId);
                // Tor 20190131 send email notification to responsible and action initiator
                wbc.addNotesNotification(this.commentText.Text, this.WorkitemId);

                PageDispatcher.GotoDataRecordViewPage(this.Page.Response, GADataClass.GAWorkitem, WorkitemId, null);
            }
        }

	}
}
