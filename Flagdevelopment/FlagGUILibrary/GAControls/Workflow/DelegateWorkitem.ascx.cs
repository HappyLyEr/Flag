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
	///		Summary description for DelegateWorkitem.
	/// </summary>
	public class DelegateWorkitem : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Button LinkButton1;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolderDelegateTo;
        protected System.Web.UI.WebControls.TextBox delegateComment;
		private Responsible resp;
        protected System.Web.UI.WebControls.RequiredFieldValidator commentTextValidator;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
	//		AddLookupField();
			
		}


		private void AddDelegateControl() 
		{
            ////create dummy fielddesc
            FieldDescription fd = new FieldDescription();


            fd.TableId = GADataClass.GAWorkitem.ToString();
            fd.FieldId = "extra1";
            fd.LookupTableKey = "PersonnelRowId";
            fd.LookupTable = GADataClass.GAPersonnel.ToString();
            fd.LookupTableDisplayValue = "FamilyName GivenName";
            fd.LookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.LookupFilterType.personnelLogin.ToString() + ":";
			
            fd.DependantField = string.Empty;
            fd.DependsOnField = string.Empty;
            fd.CssClass = string.Empty;
            // Tor 20170313 Responsible changed from Role ER to Title TITL fd.ListCategory = "ER";
            fd.ListCategory = "TITL";

			
		
			resp = new Responsible();
			resp.ID = "delegateresp";

			//resp.ID = c.ColumnName;
			this.PlaceHolderDelegateTo.Controls.Add(resp);
			resp.AddRoleDropDown(fd);
			resp.AddLookupField(fd, string.Empty);
	

		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		//	AddRoles();
			AddDelegateControl();
            commentTextValidator.Text = "<br/>" + Localization.GetErrorText("FieldRequired");
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.LinkButton1.Click += new System.EventHandler(this.LinkButton1_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion



		private void LinkButton1_Click(object sender, System.EventArgs e)
		{
			this.Visible = false;
			//Linkbutton2.Visible = true;
		}


		private void btnSave_Click(object sender, System.EventArgs e)
		{
            if (!this.Page.IsValid)
                return;

            if (resp.IsResponsibleAnUser) 
			{
                GASystem.BusinessLayer.Workitem.DelegateToPerson(WorkitemId, int.Parse(resp.getResponsibleId()), delegateComment.Text);
			} 
			else 
			{
				//string selectedRole = ddRoles.SelectedItem.Text;
				try 
				{
                    GASystem.BusinessLayer.Workitem.DelegateToRole(WorkitemId, int.Parse(resp.getResponsibleId()), delegateComment.Text);
				} 
				catch (Exception ex) 
				{
					//TODO log
				}
			}
			PageDispatcher.GotoDataRecordViewPage(this.Page.Response, GADataClass.GAWorkitem, WorkitemId, null);
		}

		

		public int WorkitemId 
		{
			get {return (int)ViewState["workitemid"];}
			set {ViewState["workitemid"] = value;}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
            // Tor 20150107 set visible=false if workitem is not active or workitemtype is WorkflowStart
            if (!GASystem.BusinessLayer.Workitem.getIfWorkitemIsActive(this.WorkitemId))
            {
                this.Visible = false;
                return;
            }
			
			//repeating javascript registrastion for responsible control here. reg in control is not working properly //TODO check registerstartupscript for responsible control
			string jscode = "<script language=\"javascript\">\n";
			jscode += "function setuservisible() {\n";
			jscode +=  "var rescontrol = document.getElementById('" + this.resp.ClientID +  "');\n";
			jscode += "rescontrol.lastChild.style.display  = 'none'; ";
			jscode += "rescontrol.lastChild.previousSibling.style.display  = 'block'; ";
			//visible

			jscode += "}\n </script>";
			this.Page.ClientScript.RegisterStartupScript(typeof(DelegateWorkitem), "respuservisible" + this.ID, jscode);
			
			
			
			//setrolevisible
			jscode = "<script language=\"javascript\">\n";
			jscode += "function setrolevisible() {\n";
			jscode +=  "var rescontrol = document.getElementById('" + this.resp.ClientID +  "');\n";
			jscode += "rescontrol.lastChild.style.display  = 'block'; ";
			jscode += "rescontrol.lastChild.previousSibling.style.display  = 'none'; ";
			//visible

			jscode += "}\n </script>";
			this.Page.ClientScript.RegisterStartupScript(typeof(DelegateWorkitem),"setrolevisible" + this.ID, jscode);
		}

	}
}
