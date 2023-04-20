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
using GASystem.DataModel;
using GASystem.AppUtils;
using GASystem.BusinessLayer;

namespace GASystem.WebForms
{
	/// <summary>
	/// Summary description for EditArcLinkRolePermission.
	/// </summary>
	public class EditArcLinkRolePermission : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label ArcLinkLabel;
		protected GASystem.UserControls.EditArcLinksAccessRoles EditArcLinksAccessRoles1;
	
		private void Page_Load(object sender, System.EventArgs e)
		{

            if (!Page.IsPostBack)
            {
                String ownerClass = Request["Owner"];
                String memberClass = Request["Member"];

                EditArcLinksAccessRoles1.Owner = GADataRecord.ParseGADataClass(ownerClass);
                EditArcLinksAccessRoles1.Member = GADataRecord.ParseGADataClass(memberClass);

                EditArcLinksAccessRoles1.RefreshControl();
                ArcLinkLabel.Text = String.Format(Localization.GetGuiElementText("ArcLinksAccessRolesHeading"), Localization.GetGuiElementText(memberClass + "Plur"), Localization.GetGuiElementText(ownerClass));
            }
			
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
            // Tor 201611 Security 20161209 Check if gaadministrator to open page
            // Tor 20171115 Security Checki if gaadministrator or administrators to open page
            //if (GASystem.BusinessLayer.Security.IsGAAdministrator())
            if (GASystem.BusinessLayer.Security.IsGAAdministrator() || GASystem.BusinessLayer.Security.IsAdministrators())
            {
                InitializeComponent();
                base.OnInit(e);
            }
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
