namespace gadnnmodules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.DataModel;
	using GASystem.BusinessLayer;
	using GASystem.AppUtils;



	/// <summary>
	///		Summary description for HomeContext.
	/// </summary>
	public class HomeContext : DotNetNuke.Entities.Modules.PortalModuleBase
	{
		//protected System.Web.UI.WebControls.DataGrid DataGrid1;
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.WebControls.Label lblError;
		protected GASystem.GAControls.ShortCutLinks MyShortCutLinks;

		private string GetDataRecordName(GADataRecord DataRecord)
		{
			string ownerName;
			DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(DataRecord);
			if (ds.Tables[0].Columns.Contains("name"))
				ownerName = ds.Tables[0].Rows[0]["Name"].ToString();
			else
				ownerName = string.Empty; //TODO make sure all tables has a name column or implement a method for getting columns with purpose name;
			return ownerName;
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			// Put user code to initialize the page here
            if (CurrentContext != GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord) 
            {
                if (GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord != null) 
                {
                    CurrentContext = GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord;
                    HyperLink1.Text = GASystem.AppUtils.Localization.GetGuiElementText("Home") + ": ";
                    String rowIdName = CurrentContext.DataClass.ToString() + "RowId";
                    rowIdName = rowIdName.Substring(2);
                    String tabId = System.Configuration.ConfigurationManager.AppSettings.Get(CurrentContext.DataClass.ToString() + "Details" + "TabId");

                    // Tor 20120815 Added: use DefaultTabId if tabid for current class is empty
                    if (tabId == null) tabId = System.Configuration.ConfigurationManager.AppSettings.Get("DefaultTabId");

                    // Tor 20150311 added &dataclass=CurrentContext.DataClass.ToString() at the end to make the link work for defaultTabId classes	HyperLink1.NavigateUrl = "~/Default.aspx?tabId="+tabId+"&"+rowIdName+"="+CurrentContext.RowId;
                    HyperLink1.NavigateUrl = "~/Default.aspx?tabId=" + tabId + "&" + rowIdName + "=" + CurrentContext.RowId + "&dataclass=" + CurrentContext.DataClass.ToString();
                    try 
                    {
                        HyperLink1.Text += GASystem.AppUtils.Localization.GetGuiElementText(CurrentContext.DataClass.ToString()) + " " + GetDataRecordName(CurrentContext);
                        //DataGrid1.DataSource =  GetMembers(); //GASystem.BusinessLayer.DataClassRelations.GetNextLevelDataClasses(CurrentContext.DataClass);
                        //DataGrid1.DataBind();
						
                        MyShortCutLinks.GenerateLinks();
                    }
                    catch (Exception ex) 
                    {
                        //TODO change this to a gaexception
                        lblError.Text = "</br>" + ex.Message;
                        //DataGrid1.Visible = false;
                        MyShortCutLinks.Visible = false;
                    }
                } 
                else
                {
                    //TODO remove this, should never get here, handle differently
                    CurrentContext = null;
                    HyperLink1.Text = "Home";
                    HyperLink1.NavigateUrl = "~/Default.aspx?tabId=1";
                }
            } 
		}

		private System.Data.DataSet GetMembers() 
		{
			DataSet ds = new DataSet();
			DataTable dt = new DataTable("memberclasses");
			dt.Columns.Add("dataclass");
			dt.Columns.Add("newdataclassname");
			foreach (object dataClass in GASystem.BusinessLayer.DataClassRelations.GetNextLevelDataClasses(CurrentContext.DataClass))
				dt.Rows.Add(new string[] { dataClass.ToString(), String.Format(Localization.GetGuiElementText("NewRecord"), Localization.GetGuiElementText(dataClass.ToString()))});
			ds.Tables.Add(dt);
			return ds;
		}


		private GADataRecord CurrentContext 
		{
			get 
			{
				return ViewState["CurrentContext"] == null ? null : (GADataRecord)ViewState["CurrentContext"];
			}
			set 
			{
				ViewState["CurrentContext"] = value;
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
