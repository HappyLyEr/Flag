namespace GASystem
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Collections;
	using GASystem.AppUtils;
    using Telerik.WebControls;

	/// <summary>
	///		Summary description for Tabsnavigation.
	/// </summary>
	public class Tabsnavigation : System.Web.UI.UserControl
	{
        protected RadTabStrip classTabStrip;
	

		public event GAGUIEvents.GACommandEventHandler TabIndexChanged;

		private void Page_Load(object sender, System.EventArgs e)
		{
            //if (Page.IsPostBack && TabStripData.Count > 0) 
            //{
            //    TabList.DataSource = TabStripData;
            //    TabList.DataBind();
            //}
            //    //SetupTabstrips();

		}


		public void SetupTabstrips()
		{
            if (!this.Page.IsPostBack)
            {

                if (null != TabStripData && TabStripData.Count > 0)
                {

                    //add tabs;
                    foreach (TabStripsDetails tabDetail in TabStripData)
                    {
                        classTabStrip.Tabs.Add(new Tab(tabDetail.TabCaption, tabDetail.TabId));
                    }



                    //if (-1 == TabList.SelectedIndex)
                    //{
                        //TabList.SelectedIndex = 0; 	//TODO test by iterating trough tabstripdata to match with subclass in querystring.
                        int selectedTab = 0;
                        string subClass = this.Page.Request["subclass"] == null ? string.Empty : this.Page.Request["subclass"].ToString();
                        for (int n = 0; n < TabStripData.Count; n++)
                        {
                            if (((TabStripsDetails)TabStripData[n]).TabId.ToUpper().Equals(subClass.ToUpper()))
                                selectedTab = n;

                        }

                        SetTab(selectedTab);
                    //}


                    //TabList.DataSource = TabStripData;
                    //TabList.DataBind();

                }
            }
			
		}

		private void SetTab(int Index)
		{
			//TabList.SelectedIndex = Index;
            classTabStrip.SelectedIndex = Index;
       
			SelectedTab = Index;

			if (null!=TabIndexChanged)
			{
				GAGUIEvents.GACommandEventArgs eventArgs = new GAGUIEvents.GACommandEventArgs();
				TabStripsDetails tab = (TabStripsDetails) TabStripData[Index];
				eventArgs.CommandStringArgument = tab.TabId;
				eventArgs.CommandIntArgument = Index;
				TabIndexChanged(this, eventArgs);
			}
		}

		private int SelectedTab 
		{
			get {return ViewState["SelectedTab"] == null ? -1 : (int)ViewState["SelectedTab"];}
			set {ViewState["SelectedTab"] = value;}

		}

		public void AddTab(string TabId, string TabCaption, string NavigationURL)
		{
			ArrayList tabsData = TabStripData;
			tabsData.Add(new TabStripsDetails(TabId, TabCaption, NavigationURL));
			TabStripData = tabsData;
		}	


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//

			InitializeComponent();
            classTabStrip.AutoPostBack = true;
            classTabStrip.ReorderTabRows = false;
            //classTabStrip.Align = TabStripAlign.Justify;
            classTabStrip.TabClick += new TabStripEventHandler(classTabStrip_TabClick);
			base.OnInit(e);
		}

        void classTabStrip_TabClick(object sender, TabStripEventArgs e)
        {
            SetTab(e.Tab.Index);
        }
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            //this.TabList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.TabList_ItemCommand);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

        //private void TabList_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        //{
        //    if (e.CommandName == "tab")
        //    {
        //        //lstVipTabs.SelectedIndex = e.Item.ItemIndex;
        //        SetTab(e.Item.ItemIndex);
        //        SetupTabstrips();
        //    }
        //}


		public int Count 
		{
			get 
			{
					return TabStripData.Count;
			}
		}

		private ArrayList TabStripData
		{
			get
			{
				return null == ViewState["TabStripData"+this.ID] ? new ArrayList() : (ArrayList) ViewState["TabStripData"+this.ID];
			}
			set
			{
				ViewState["TabStripData"+this.ID] = value;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
            //if (TabStripData.Count > 0) 
            //{
            //    TabList.SelectedIndex = SelectedTab;
            //    TabList.DataBind();
            //}
			base.OnPreRender (e);
		}



		public void Clear() 
		{
			TabStripData = null;
		}
		
		
	}

	[Serializable]
	public class TabStripsDetails
	{
		public TabStripsDetails(String TabId , String TabCaption, string NavigationURL)
		{
			this.TabId = TabId;
			this.TabCaption = TabCaption;
			this.NavigationURL = NavigationURL;
			
		}
		public String TabId;
		public String TabCaption;
		public string NavigationURL;
		
		

	}

	

}
