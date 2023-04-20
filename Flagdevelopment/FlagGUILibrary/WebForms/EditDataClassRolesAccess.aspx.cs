using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GASystem.BusinessLayer;
using GASystem.AppUtils;
using GASystem.DataModel;

namespace GASystem.WebForms
{
	/// <summary>
	/// Summary description for EditDataClassRolesAccess.
	/// </summary>
	public class EditDataClassRolesAccess : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblDataClass;
		protected System.Web.UI.WebControls.DropDownList DataClassSelector;
		protected GASystem.UserControls.EditClassAccessRoles EditClassAccessRoles1;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
				lblDataClass.Text = Localization.GetGuiElementText(lblDataClass.Text);
				PopulateDataClassDropdown();
			}
		}

		private void PopulateDataClassDropdown() 
		{
			
			DataClassSelector.DataSource = DataClassRelations.GetAlleClasses();
			DataClassSelector.DataValueField = "Class";
			DataClassSelector.DataTextField = "Class";
			DataClassSelector.DataBind();
			
			DataClassSelector.SelectedIndex = 0;
			DataClassSelector_SelectedIndexChanged(this, new System.EventArgs());

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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.DataClassSelector.SelectedIndexChanged += new System.EventHandler(this.DataClassSelector_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void DataClassSelector_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			EditClassAccessRoles1.DataClass = GADataRecord.ParseGADataClass(DataClassSelector.SelectedItem.Text);
			EditClassAccessRoles1.RefreshControl();

		
		}
	}
}
