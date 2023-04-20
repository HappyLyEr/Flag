namespace GASystem.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;

	/// <summary>
	///		Summary description for SubTabsNavigation.
	/// </summary>
	public class SubTabsNavigation : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataList TabList;
//		private ArrayList TabStripData = new ArrayList();

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		private ArrayList TabStripData
		{
			get
			{
				return null == ViewState["SubTabStripData"+this.ID] ? new ArrayList() : (ArrayList) ViewState["SubTabStripData"+this.ID];
			}
			set
			{
				ViewState["SubTabStripData"+this.ID] = value;
			}
		}

		public void ClearTabStrip() 
		{
			TabStripData.Clear();
		}

		public void AddTab(string URL, string Text) 
		{
			ArrayList tabsData = TabStripData;
			tabsData.Add(new SubTabDetails(URL, Text));
			TabStripData = tabsData;
		}

		public void SetupSubTabStrip() 
		{
			TabList.DataSource = TabStripData;

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

		protected override void OnPreRender(EventArgs e)
		{
			SetupSubTabStrip();
			if (TabStripData != null)
				TabList.DataBind();
			base.OnPreRender (e);

		}

	}

	[Serializable]
	public class SubTabDetails 
	{
		public string URL;
		public string URLText;

		public SubTabDetails(string URL, string URLText) 
		{
			this.URL = URL;
			this.URLText = URLText;
		}
	}
}
