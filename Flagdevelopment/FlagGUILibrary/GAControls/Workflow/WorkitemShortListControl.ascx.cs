namespace GASystem.GAControls.Workflow
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.DataModel;
	using GASystem.BusinessLayer;

	/// <summary>
	///		Summary description for WorkitemShortList.
	/// </summary>
	public class WorkitemShortListControl : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label lblHeader;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
		protected System.Web.UI.WebControls.Label lblError;
		protected GASystem.GAControls.ListData.ListWorkitemAllDataRecords MyListRoleRecords;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolderMain;   //used to list all workitems assigned to users role
		//protected GASystem.DataContextInfo DataContextInfoControl;
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
        protected Label workitemLabel;
		
		

		private const string WORKITEMERRORCACHE = "WORKITEMERRORCACHE";

		private void Page_Load(object sender, System.EventArgs e)
		{
            workitemLabel.Text = GASystem.AppUtils.Localization.GetGuiElementTextPlural(GADataClass.GAWorkitem.ToString()).ToUpper();
            
            // Put user code to initialize the page here
			string userId = System.Web.HttpContext.Current.User.Identity.Name;
		//	SetContextInfo();	
			HyperLink1.NavigateUrl = GASystem.GUIUtils.LinkUtils.GenerateURLForListMyWorkitems();
			HyperLink1.Text = GASystem.AppUtils.Localization.GetGuiElementText("ListMyWorkitems");
            HyperLink1.CssClass = "FlagLinkButton";
			//if there was an error contacting the workitem server, cache the error message and wait 30 sec. before retrying
			if (this.Page.Cache[WORKITEMERRORCACHE] == null) 
			{
				try 
				{
					GADataRecord owner = null;
					MyListRoleRecords = new GASystem.GAControls.ListData.ListWorkitemAllDataRecords(owner, System.Web.HttpContext.Current.User.Identity.Name);
					MyListRoleRecords.UserHasEditPermission = false;
					this.PlaceHolderMain.Controls.Add(MyListRoleRecords);
					
			
				} 
				catch (Exception ex)
				{	
					//TODO replace with GAerror handling 
					lblError.Text = ex.Message;
					this.Page.Cache.Insert(WORKITEMERRORCACHE, lblError.Text, null, System.DateTime.Now.AddSeconds(30), System.TimeSpan.Zero);
					return;
				}
			} 
			else 
			{
				lblError.Text = this.Page.Cache[WORKITEMERRORCACHE].ToString();
			}
			
			
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
