using GASystem.GAExceptions;
using GASystem.BusinessLayer.Utils;

namespace GASystem
{
	using System;
	using System.Collections;
	using GASystem.DataModel;
	using GASystem.AppUtils;
	using GASystem.BusinessLayer;
    using GASystem.GUIUtils;

	/// <summary>
	///		Summary description for MemberDataTabs.
	/// </summary>
	public class MemberDataTabs : System.Web.UI.UserControl
	{
		
		protected GASystem.Tabsnavigation TabsnavigationControl;
		protected System.Web.UI.WebControls.Panel DataListPanel;
		protected System.Web.UI.WebControls.PlaceHolder RecordListPlaceHolder;
		private GASystem.ListDataRecords myListDataRecords;
        // Tor 201611 Security 20161122 (never referenced) private bool IsSetupItemCalled = false;
		private bool displayEdit = false;
		private bool displayNew = false;
        private bool _displayWorkflowTabs = true;

		private void Page_Load(object sender, System.EventArgs e)
		{
			//if (Page.IsPostBack && TabsnavigationControl.Count > 0) {

        
           
            if (Page.IsPostBack)
            {
                SetupItemList(false);     //recreate list on postback
            }




            //if (!Page.IsPostBack)
            //    myListDataRecords.setDefaultFilter();

            //string myfilter = myListDataRecords.getFilter();
            //System.Data.DataSet records = BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(CurrentDataClass, this.Owner, System.DateTime.MinValue, System.DateTime.MaxValue, myfilter);
            //myListDataRecords.RecordsDataSet = records;


            //myListDataRecords.RefreshGrid();


		}

		public bool DisplayEditButton 
		{
			set {displayEdit = value;}
			get {return displayEdit;}
		}

		public bool DisplayNewButton 
		{
			set {displayNew = value;}
			get {return displayNew;}
		}

        public bool DisplayWorkflowTabs
        {
            set { _displayWorkflowTabs = value; }
            get { return _displayWorkflowTabs; }
        }

		public void SetupTabsNavigation()
		{
			if (null==Owner) return;

            // Tor 20150928 GetNextLevelTabDataClasses(GADataRecord Owner) only returns tabs where the user has read of create permissions 
            //			ArrayList memberList = BusinessLayer.DataClassRelations.GetNextLevelTabDataClasses(Owner.DataClass);
            ArrayList memberList = BusinessLayer.DataClassRelations.GetNextLevelTabDataClasses(Owner);
			if (null==memberList) return;

			TabsnavigationControl.Clear();
			
			//Filter out any members where user don't have readaccess
			ArrayList memberListSecure = new ArrayList();
			foreach (String  member in memberList)
			{
				//Only display tabs if user has readAccess to data on tab
				
				//FW 1910: This feature is disabled. Currently users always has access to member datarecords (there is no deny acces feature)
				//if (Security.HasReadPermissionOnDataClassInContext(Owner, GADataRecord.ParseGADataClass(member)))
                
                
     
                    if (!(!DisplayWorkflowTabs && member == GADataClass.GAAction.ToString() || !DisplayWorkflowTabs && member == GADataClass.GAWorkitem.ToString()))
                        memberListSecure.Add(member);
			}
			
			foreach (String  member in memberListSecure)
			{
				string navigationURL = GUIUtils.LinkUtils.GenerateURLForSingleRecordView(this.Owner.DataClass.ToString(), this.Owner.RowId.ToString(), member);
                TabsnavigationControl.AddTab(member, breakTabLabel(Localization.GetGuiElementTextPlural(member)), navigationURL);
			}
			if (!this.Page.IsPostBack)
				TabsnavigationControl.SetupTabstrips();

    		
			if (memberListSecure.Count>0)
				
				if (!this.IsPostBack) 
				{
					//DisplayNewButton = Security.HasCreatePermission(GADataRecord.ParseGADataClass((String)memberListSecure[0]), Owner);
					//DisplayEditButton = Security.HasUpdatePermissionInContext(GADataRecord.ParseGADataClass((String)memberListSecure[0]), Owner);
					//				DisplayPanel((String)memberListSecure[0]); //default to first panel on initial load
				}
				else 
				{
					//DisplayNewButton = Security.HasCreatePermission(CurrentDataClass, Owner);
					//DisplayEditButton = Security.HasUpdatePermissionInContext(CurrentDataClass, Owner);
					//DisplayPanel(CurrentDataClass.ToString());
				}
			else 
			{
				//SetPanelVisible(-1); //Hide all panels
				RecordListPlaceHolder.Visible = false;

			}

			
		}

        /// <summary>
        /// Tries to insert a single break <br/> in tab label so that long labels uses to lines.
        /// </summary>
        /// <param name="tabLabel"></param>
        /// <returns></returns>
        private string breakTabLabel(string tabLabel)
        {
            //return if empty string
            if (tabLabel == string.Empty)
                return tabLabel;

            tabLabel = tabLabel.Trim();
            string[] words = tabLabel.Split(' ');
            //return if there are no spaces in the string;
            if (words.Length == 1)
                return tabLabel;
           
            //simple break if two words only
            if (words.Length == 2)
                return tabLabel.Replace(" ", "<br/>");

            //find the middle space
            int indexOfMiddle = tabLabel.Length / 2;
            int currentSpace = words[0].Length;
            int currentBreak = 0;
            int currentBreakDistance = indexOfMiddle;
            int currentIndex = 0;
            for (int t = 0; t < words.Length-1; t++) 
            {
                currentIndex += words[t].Length + 1; //adding 1 in order to compensate for space character;
                if (Math.Abs(indexOfMiddle - currentIndex) < currentBreakDistance) 
                {
                    currentBreakDistance = Math.Abs(indexOfMiddle - currentIndex);
                    currentBreak = t;
                }
            }

            //combine new string;
            tabLabel = string.Empty;
            for (int t = 0; t < words.Length; t++) 
            {
                tabLabel += words[t];
                if (t == currentBreak) 
                    tabLabel += "<br/>";
                else
                    tabLabel += " ";
            }

           return tabLabel;

        }


		private void DisplayPanel(String PanelName)
		{

			CurrentDataClass = GADataRecord.ParseGADataClass(PanelName);
		}


        //private void SetupItemList(GADataClass DataClass) 
        //{
        //    IsSetupItemCalled = true;
			

        //    CurrentDataClass = DataClass;
        //}



		private void SetupItemList(bool resetFilter) 
		{
			if (CurrentDataClass == GADataClass.NullClass)
				return;   //no subtab defined. Do not add control
			
			try
			{
                myListDataRecords.ID = "tablist_" + CurrentDataClass;
                myListDataRecords.Owner = this.Owner;
				//MyListDataRecords.ClearGrid(); 
				//MyListDataRecords.RecordsDataSet = BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassByOwner(DataClass, Owner);

                //BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(CurrentDataClass);
                
                
                //MyListDataRecords.RecordsDataSet = RecordsetFactory.GetRecordSetListViewAllDetailsForDataClassWithinOwner(CurrentDataClass, Owner);
				//GetRecordSetAllDetailsForDataClassByOwner
				bool dataClassChanged = myListDataRecords.DataClass != CurrentDataClass.ToString();
				myListDataRecords.DataClass = CurrentDataClass.ToString();
				myListDataRecords.DisplayFilter = true;
				//MyListDataRecords.AddFilterControl(dataClassChanged);
				myListDataRecords.NewRecordClicked += new GASystem.GAGUIEvents.GACommandEventHandler(MyListDataRecords_NewRecordClicked);
               
				RecordListPlaceHolder.Controls.Add(myListDataRecords);
				myListDataRecords.DisplayEditButton = Security.HasUpdatePermissionInArcLink(this.CurrentDataClass, Owner); //this.DisplayEditButton;
				myListDataRecords.DisplayNewButton = Security.HasCreatePermission(this.CurrentDataClass, Owner); //this.DisplayNewButton;
                if (resetFilter)
                {
                    myListDataRecords.setDefaultFilter();
                    myListDataRecords.ResetSort = true;
                }
                
                string myfilter = myListDataRecords.getFilter();
                //myListDataRecords.RecordsDataSet = BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(CurrentDataClass, Owner, System.DateTime.MinValue, System.DateTime.MaxValue, myfilter);
                if (resetFilter || this.CurrentDataClass == GADataClass.GAWorkitem)
                    myListDataRecords.RecordsDataSet = BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewByOwnerIncludingDropDownDetails(CurrentDataClass, Owner, myfilter);
               
				myListDataRecords.RefreshGrid();
			}
			catch (GAException gaEx)
			{
				Console.Write(gaEx);
			}
		}

        //private void reloadListRecordSet() 
        //{
        //    MyListDataRecords.RecordsDataSet = RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(CurrentDataClass, Owner);
        //    MyListDataRecords.RefreshGrid();	
        //}

		public GADataClass CurrentDataClass
		{
			get
			{
				return ViewState["DataClass"+this.ID] == null ? GADataClass.NullClass : (GADataClass) ViewState["DataClass"+this.ID];
			}
			set
			{
				ViewState["DataClass"+this.ID] = value;
			}
		}

		
		protected override void OnPreRender(EventArgs e)
		{
            //if (IsSetupItemCalled)
            //    SetupItemList();
			base.OnPreRender (e);

            if (myListDataRecords.ListUpdated)
            {
                //BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(CurrentDataClass);
                string myfilter = myListDataRecords.getFilter();
                myListDataRecords.RecordsDataSet = BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewByOwnerIncludingDropDownDetails(CurrentDataClass, Owner, myfilter);
                myListDataRecords.RefreshGrid();
            }
		}





		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
				
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
            myListDataRecords = (GASystem.ListDataRecords)UserControls.UserControlUtils.GetUserControl(UserControls.UserControlType.ListDataRecords, this.Page);
				
            TabsnavigationControl.TabIndexChanged += new GASystem.GAGUIEvents.GACommandEventHandler(TabsnavigationControl_TabIndexChanged);

            InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		//	this.lnkEditMultiline.Click += new System.EventHandler(this.lnkEditMultiline_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion



		/// <summary>
		/// Sets or gets the dataclass of these actions owners. Every action must be owned by some dataclass, and in order
		/// to create new actions this control must know which dataclass should own them
		/// </summary>
		public GADataRecord Owner
		{
			get
			{
				return (GADataRecord) ViewState["Owner"+this.ID];
			}
			set
			{
				ViewState["Owner"+this.ID] = value;
			}
		}

		private void TabsnavigationControl_TabIndexChanged(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
            DisplayViewList();
            DisplayPanel(e.CommandStringArgument);
            //reset current page on tab change
            RecordListPlaceHolder.Controls.Clear();
            myListDataRecords.clearViewStates();
            myListDataRecords = null;
            myListDataRecords = (GASystem.ListDataRecords)UserControls.UserControlUtils.GetUserControl(UserControls.UserControlType.ListDataRecords, this.Page);
           // MyListDataRecords.ID = "tablist_" + e.CommandStringArgument;
            SetupItemList(true);
          //  MyListDataRecords.CurrentPage = 0;
           
		}


		private void MyListDataRecords_NewRecordClicked(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			PageDispatcher.GotoDataRecordDetailsPage(Response, GASystem.DataModel.GADataRecord.ParseGADataClass(myListDataRecords.DataClass), 0, Owner);
		}


		private void DisplayViewList() 
		{
			DataListPanel.Visible = true;
	
		}

	}
}
