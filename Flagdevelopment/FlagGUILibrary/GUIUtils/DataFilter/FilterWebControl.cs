using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using GASystem.GUIUtils;
using GASystem.DataModel;
using Telerik.WebControls;
using GASystem.BusinessLayer.Utils;
using GASystem.BusinessLayer;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.DataFilter
{
	/// <summary>
	/// Summary description for FilterWebControl.
	/// </summary>
	public class FilterWebControl : WebControl, System.Web.UI.INamingContainer
	{
		Panel FilterPanel;
		PlaceHolder editPlaceHolder;
		//System.Web.UI.WebControls.PlaceHolder viewPlaceHolder;
        System.Web.UI.WebControls.Panel viewPlaceHolder;
		LinkButton setFilterButton;
        LinkButton resetFilterButton;
        LinkButton saveFilter;
        LinkButton deleteFilter;

        DropDownList filterDropDown;
        bool updateFilterDropDown = true;
        


		GASystem.UserControls.SubTabsNavigation filterShortCut;

		public event EventHandler SetFilterClicked;
        public event EventHandler ResetFilterClicked;
        public event EventHandler SaveFilterClicked;
        public event EventHandler<GASystem.GAGUI.GAGUIEvents.GAEventArgs<int>>  DropDownFilterSelected;
        public event EventHandler<GASystem.GAGUI.GAGUIEvents.GAEventArgs<int>> DeleteFilterClicked;


		//public event EventHandler ExportToExcelClicked;
		LinkButton displayFilter;
		//LinkButton exportExcel;
		Label ViewInfoText;
		private Panel editPanelPlaceHolder; 
		private GADataClass _dataClass;

		protected const string HEADERCSSCLASS = "FilterLabelCell";
        protected const string HEADERCONDITIONCSSCLASS = "FilterLabelCellCondition";
		protected const string PANELCSSCLASS = "FilterPanel";
        protected const string PANELTABLECSSCLASS = "FilterPanelTable";
        protected const string EDITTABLECSSCLASS = "FilterEditTable";
		protected const string INFONONEFILTERELEMENTS = "infoFilterNoElements";
		protected const string INFOONEFILTERELEMENTS = "infoFilterOneElement";
		protected const string INFOMULTIPLEFILTERELEMENTS = "infoFilterMultipleElements";
        protected const string FLAGLINKCSS = "FlagLinkButton";


        //test hiding and showing
        private HyperLink lnkDisplayViewPanel = new HyperLink();
        protected System.Web.UI.WebControls.Panel PlaceHolderShow;
        protected System.Web.UI.WebControls.Panel PlaceHolderHide;
        private HyperLink lnkHideViewPanel = new HyperLink();
        private Panel viewPanel = new Panel();
        //end test hiding and showing

        //new placeholders
        private PlaceHolder filterCommandsPlaceHolder = new PlaceHolder();
        private PlaceHolder commandlinePlaceHolder = new PlaceHolder();

		public FilterWebControl()
		{
			//
			// TODO: Add constructor logic here 
			// Add two placeholders one for view and one for edit
			// handle view change and filter setting
			//
			//this.CssClass = PANELCSSCLASS;
		
			FilterPanel = new Panel();
			editPanelPlaceHolder = new  Panel();

			editPlaceHolder = new PlaceHolder();
			AddHeadersToEditControl();
		//	viewPlaceHolder = new PlaceHolder();
            viewPlaceHolder = new Panel();
            setFilterButton = new LinkButton();
            setFilterButton.CssClass = "FlagLinkButtonCombined";
            setFilterButton.Style.Add("padding-left", "20px");
            setFilterButton.Style.Add("margin-bottom", "10px");

            saveFilter = new LinkButton();
            saveFilter.CssClass = "FlagLinkButtonCombined";
            saveFilter.Text = GASystem.AppUtils.Localization.GetGuiElementText("ApplySave");

            deleteFilter = new LinkButton();
            deleteFilter.CssClass = "FlagLinkButtonCombined";
            deleteFilter.Text = GASystem.AppUtils.Localization.GetGuiElementText("DeleteFilter");

            resetFilterButton = new LinkButton();
            resetFilterButton.CssClass = "FlagLinkButton";
            resetFilterButton.Style.Add("padding-left", "10px");
            resetFilterButton.Text = GASystem.AppUtils.Localization.GetGuiElementText("DefaultSelection");


			displayFilter = new LinkButton();
		//	exportExcel = new LinkButton();
			
            
            //filterselection dropdown
            filterDropDown = new DropDownList();
            filterDropDown.AutoPostBack = true;
            
            
            ViewInfoText = new Label();
			


		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			this.Controls.Add(FilterPanel);
			filterShortCut = (UserControls.SubTabsNavigation)UserControls.UserControlUtils.GetUserControl(UserControls.UserControlType.SubTabsNavigation, this.Page);

			FilterPanel.Controls.Add(filterShortCut);
//			filterShortCut.AddTab("http://www.norsolutions.com/", "Norsolutions");
			
			//create table
			HtmlTable table = new HtmlTable();
            table.Border = 0;
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.Width = "100%";
            table.Attributes.Add("class", PANELTABLECSSCLASS);
			FilterPanel.Controls.Add(table);
			
			HtmlTableRow row1 = new HtmlTableRow();
			table.Controls.Add(row1);
	
			HtmlTableRow row2 = new HtmlTableRow();
			table.Controls.Add(row2);
            HtmlTableRow row3 = new HtmlTableRow();
            table.Controls.Add(row3);

			HtmlTableCell cell11 = new HtmlTableCell();
			row1.Controls.Add(cell11);
		
			HtmlTableCell cell12 = new HtmlTableCell();
			row1.Controls.Add(cell12);
			HtmlTableCell cell21 = new HtmlTableCell();
			row2.Controls.Add(cell21);
            HtmlTableCell cell22 = new HtmlTableCell();
			row2.Controls.Add(cell22);
			HtmlTableCell cell13 = new HtmlTableCell();
			row1.Controls.Add(cell13);

            HtmlTableCell cell31 = new HtmlTableCell();
            row3.Controls.Add(cell31);
            cell31.ColSpan = 3;
		
	//		HtmlTableCell cell22 = new HtmlTableCell();
	//		row2.Controls.Add(cell22);
			
			//add controls to cells
			cell11.VAlign = "top";
			cell11.Controls.Add(ViewInfoText);
            
			
		//	cell11.Controls.Add(viewPlaceHolder);

			cell12.VAlign = "top";
			cell12.Controls.Add(viewPlaceHolder);
          

			cell13.VAlign = "top";
			cell13.Align = "right";
		
			//cell13.Controls.Add(displayFilter);
			cell13.Controls.Add(HTMLLiteralTags.CreateBRTag());
            //cell13.Controls.Add(resetFilterButton);



            PlaceHolder radditpanelPlaceHolder = new PlaceHolder();



			cell21.ColSpan = 1;
            cell22.ColSpan = 2;

            filterDropDown.SelectedIndexChanged += new EventHandler(filterDropDown_SelectedIndexChanged);
            filterDropDown.CssClass = "filterSelectionDropDown";

            cell21.Controls.Add(commandlinePlaceHolder);
            cell21.Style.Add("padding-left", "20px");
            cell22.Controls.Add(filterCommandsPlaceHolder);
            cell22.VAlign = "right";
            cell22.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
            
            filterCommandsPlaceHolder.Controls.Add(filterDropDown);
            filterCommandsPlaceHolder.Controls.Add(resetFilterButton);

            


           // cell21.Controls.Add(HTMLLiteralTags.CreateBRTag());
			cell31.Controls.Add(radditpanelPlaceHolder);

			
			editPanelPlaceHolder.Controls.Add(getFilterText());
            editPanelPlaceHolder.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Table, EDITTABLECSSCLASS));
            editPanelPlaceHolder.Controls.Add(editPlaceHolder);
            
            
            editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Tr));
            editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateTextElement("<td colspan=\"4\">"));
            editPlaceHolder.Controls.Add(setFilterButton);
            editPlaceHolder.Controls.Add(saveFilter);
            editPlaceHolder.Controls.Add(deleteFilter);
            editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));
            editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Tr));
			
            
            editPanelPlaceHolder.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Table));
            
            editPanelPlaceHolder.Controls.Add(HTMLLiteralTags.CreateBRTag());
            
   //test jof. put edit in rad panel

            //RadPanelbar radp = new RadPanelbar();
            //radditpanelPlaceHolder.Controls.Add(radp);

            //radp.ExpandMode = PanelbarExpandMode.MultipleExpandedItems;
            //radp.Skin = "FlagFilter";
            //RadPanelItem radpitem = new RadPanelItem();
            //radpitem.Text = GASystem.AppUtils.Localization.GetGuiElementText("setfilter");
            //radp.Items.Add(radpitem);
            //RadPanelItem radpitemtemplate = new RadPanelItem();
            //radpitem.Items.Add(radpitemtemplate);
           // radpitemtemplate.Controls.Add(editPanelPlaceHolder);




			displayFilter.Text = GASystem.AppUtils.Localization.GetGuiElementText("setfilter");
	//		exportExcel.Text = GASystem.AppUtils.Localization.GetGuiElementText("exporttoexcel");
            displayFilter.CssClass = FLAGLINKCSS;
    //        exportExcel.CssClass = FLAGLINKCSS;


			FilterPanel.CssClass = PANELCSSCLASS;
			


			
			setFilterButton.Text = "<img src=\"gagui/Images/search.gif\"/>" +  GASystem.AppUtils.Localization.GetGuiElementText("applyFilter");
			setFilterButton.CausesValidation = true;
            saveFilter.CausesValidation = true;


			displayFilter.Click += new EventHandler(displayFilter_Click);
			//			AddBRElement(FilterPanel);
			//			setFilterButton.Visible = false;
			
			
			setFilterButton.Click += new EventHandler(setFilterButton_Click);
	//		exportExcel.Click += new EventHandler(exportExcel_Click);
            saveFilter.Click += new EventHandler(saveFilter_Click);
            deleteFilter.Click += new EventHandler(deleteFilter_Click);

            resetFilterButton.Click += new EventHandler(resetFilterButton_Click);



            //test show hide using jscript
            filterCommandsPlaceHolder.Controls.Add(lnkDisplayViewPanel);
            filterCommandsPlaceHolder.Controls.Add(lnkHideViewPanel);

            radditpanelPlaceHolder.Controls.Add(viewPanel);

            lnkHideViewPanel.Text = GASystem.AppUtils.Localization.GetGuiElementText("setfilter");
            lnkDisplayViewPanel.Text = GASystem.AppUtils.Localization.GetGuiElementText("setfilter");


            lnkHideViewPanel.CssClass = "FlagLinkButton";



            lnkDisplayViewPanel.CssClass = "FlagLinkButton";



            viewPanel.Controls.Add(editPanelPlaceHolder);
            
            registerJScripyVisibilitySetting(viewPanel.ClientID, lnkDisplayViewPanel.ClientID, lnkHideViewPanel.ClientID);
            

            //END test show hide using jscript




		}



        private int filterDropDownSelectedValue = -1;

        public int FilterDropDownSelectedValue
        {
            get { return filterDropDownSelectedValue; }
            set { filterDropDownSelectedValue = value; }
        }
	

        void filterDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownFilterSelected != null && filterDropDown.SelectedValue != @"-1")
            {
               DropDownFilterSelected(this, new GASystem.GAGUI.GAGUIEvents.GAEventArgs<int>(int.Parse(filterDropDown.SelectedValue)));
            }
            updateFilterDropDown = false;

            //BusinessClass bc = RecordsetFactory.Make(GADataClass.GAClassFilter);
            //int selectedValue = int.Parse(filterDropDown.SelectedValue);
            
            //ClassFilterDS ds = (ClassFilterDS)bc.GetByRowId(selectedValue);

            //filterDropDown.Items.Clear();
            //fillFilterList();

            //foreach (ListItem item in filterDropDown.Items)
            //    if (item.Value == selectedValue.ToString())
            //        item.Selected = true;

            //build filter
           
        }



        void deleteFilter_Click(object sender, EventArgs e)
        {
            if (DeleteFilterClicked != null && filterDropDown.SelectedValue != "-1")
                DeleteFilterClicked(this, new GASystem.GAGUI.GAGUIEvents.GAEventArgs<int>(int.Parse(filterDropDown.SelectedValue)));
        }


        void saveFilter_Click(object sender, EventArgs e)
        {
            if (SaveFilterClicked != null && this.Page.IsValid)
              SaveFilterClicked(this, EventArgs.Empty);
        }

        void resetFilterButton_Click(object sender, EventArgs e)
        {
            if (ResetFilterClicked != null)
                ResetFilterClicked(this, EventArgs.Empty);
        }

		private void AddHeadersToEditControl() 
		{
        //    editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Table, EDITTABLECSSCLASS));

           
            
            
            editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Tr));
			
			
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, HEADERCSSCLASS));
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateTextElement(Localization.GetGuiElementText("SeEnabled")));
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));
			
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, HEADERCSSCLASS));
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateTextElement(Localization.GetGuiElementText("SeFieldName")));
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));
			
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, HEADERCSSCLASS));
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateTextElement(Localization.GetGuiElementText("SeOperator")));
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

            editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, HEADERCONDITIONCSSCLASS));
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateTextElement(Localization.GetGuiElementText("SeCondition")));
			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			editPlaceHolder.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Tr));

       
           
        }

		public GADataClass DataClass 
		{
			get{return _dataClass;}
			set{_dataClass = value;}
		}
		

		private Literal getFilterText() 
		{
			Literal filterHelp = new Literal();
			filterHelp.Text = "<h3>" + GASystem.AppUtils.Localization.GetGuiElementText("datafilterhelptext") + "</h3>";
			return filterHelp;
			//this.editPlaceHolder.Controls.Add(filterHelp);
			//setFilterButton.Text = GASystem.AppUtils.Localization.GetGuiElementText("setfilter");
			//this.Controls.Add(setFilterButton);
		}

		public virtual void OnSetFilter() 
		{
			
		}

		/// <summary>
		/// Add a webcontrol to the editplaceholder. Format of this area is controlled by the builder
		/// </summary>
		/// <param name="EditWebControl"></param>
		public void AddEditElement(EditElements.IEditWebControl EditWebControl) 
		{
			editPlaceHolder.Controls.Add(EditWebControl);
		//	AddBRElement(editPlaceHolder);
		}

		/// <summary>
		/// Add a webcontrol to the viewplace holder. Formating is controlled by the builder.
		/// </summary>
		/// <param name="ViewWebControl"></param>
		public void AddViewElement(ViewElements.IViewWebControl ViewWebControl) 
		{
            ViewWebControl.CssClass = "FilterPanel_ViewElement";
            
            viewPlaceHolder.Controls.Add(ViewWebControl);
			//AddBRElement(viewPlaceHolder);
		}

		private void AddBRElement(System.Web.UI.Control ParentControl) 
		{
			Literal br = new Literal();
			br.Text = "<br/>";
			ParentControl.Controls.Add(br);
		}

		private void setFilterButton_Click(object sender, EventArgs e)
		{
			if (SetFilterClicked != null && this.Page.IsValid)
				SetFilterClicked(this, EventArgs.Empty);
            displayViewPanel = true;
		}


        private void fillFilterList()
        {
            filterDropDown.Items.Clear();
            filterDropDown.ClearSelection();
            
            BusinessClass bc = RecordsetFactory.Make(GADataClass.GAClassFilter);
            GADataRecord owner = new GADataRecord(GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(this.DataClass).RowId, GADataClass.GAClass);
 
            ClassFilterDS ds = (ClassFilterDS)bc.GetByOwner(owner, null);
            int personnelrowid = GASystem.GAGUI.GUIUtils.SessionData.SessionInfo.UserPersonnelRowId;
            filterDropDown.Items.Add(new ListItem("", "-1"));
          

            
            foreach (ClassFilterDS.GAClassFilterRow row in ds.GAClassFilter.Rows) 
            {
                if (row.PersonnelRowId == personnelrowid || row.PersonnelRowId < 1)
                    filterDropDown.Items.Add(new ListItem(row.Comment, row.ClassFilterRowId.ToString()));
            }
        }

		protected override void OnPreRender(EventArgs e)
		{
			//add shortcuts
			GADataRecord owner = GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord;

			bool isView = !(owner == null) &&  !(this.Page.Request[owner.DataClass.ToString().Substring(2) + "rowid"] == null);
			filterShortCut.ClearTabStrip();
			foreach(GASystem.UserControls.SubTabDetails filterTab in FilterUtils.GetFilterShortCutsForDataClass(this.DataClass, isView )) {

				filterShortCut.AddTab(filterTab.URL, filterTab.URLText);
			}
			//calculate number of active filter elements
			int filterCount = 0;
			foreach (ViewElements.IViewWebControl viewControl in viewPlaceHolder.Controls) 
				if (viewControl.FilterString != string.Empty) 
                {
                    filterCount++;
                    viewControl.Visible = true;
                }       
                else
                {
                    viewControl.Visible = false;
                }

					
			
			//set info label
			if (filterCount == 0)
				ViewInfoText.Text = GASystem.AppUtils.Localization.GetGuiElementText(INFONONEFILTERELEMENTS) + "<br/>";
			if (filterCount == 1)
				ViewInfoText.Text = GASystem.AppUtils.Localization.GetGuiElementText(INFOONEFILTERELEMENTS) + "<br/>";
			if (filterCount > 1)
				ViewInfoText.Text = GASystem.AppUtils.Localization.GetGuiElementText(INFOMULTIPLEFILTERELEMENTS) + "<br/>";

            ViewInfoText.CssClass = "FilterPanel_Info";
            ViewInfoText.Text = ViewInfoText.Text.ToUpper();
			
			
			

            SetUpDisplayLinks();
            setShowHideLinksVisibility(viewPanel, lnkHideViewPanel, lnkDisplayViewPanel);

            //filterlist
            if (updateFilterDropDown)
                fillFilterList();

            if (filterDropDown.Items.Count > 1)
                filterDropDown.Visible = true;
            else filterDropDown.Visible = false;

            bool dropDownHasValue = false;
            foreach (ListItem item in filterDropDown.Items)
                if (item.Value == FilterDropDownSelectedValue.ToString())
                    dropDownHasValue = true;

            deleteFilter.Visible = false;
            if (dropDownHasValue)
            {
                filterDropDown.SelectedValue = FilterDropDownSelectedValue.ToString();
                
                if (FilterDropDownSelectedValue != -1)
                    deleteFilter.Visible = true;
            }
        


			base.OnPreRender (e);
		}

		private void displayFilter_Click(object sender, EventArgs e)
		{
		//	displayFilter.Visible = false;
			//toggle display of editcontroll
			editPanelPlaceHolder.Visible = !editPanelPlaceHolder.Visible;

//			editPlaceHolder.Visible = !editPlaceHolder.Visible;
//			setFilterButton.Visible = editPlaceHolder.Visible;
		}

//        private void exportExcel_Click(object sender, EventArgs e)
//        {
//            if (ExportToExcelClicked != null)
//                ExportToExcelClicked(this, EventArgs.Empty);
//////
////			this.Context.Items.Add("exportfilter", 
////			this.Page.Server.Transfer("~/gagui/webforms/toexcel.aspx");
//        }

        public bool EditVisible
        {
            get { return editPanelPlaceHolder.Visible; }
            set { editPanelPlaceHolder.Visible = value; }
        }


		public void ExportToExcel(string Filter, GADataClass DataClass) 
		{
			this.Context.Items.Add("exportfilter",  Filter);
			this.Context.Items.Add("dataclass", DataClass);
			this.Page.Server.Transfer("~/gagui/webforms/toexcel.aspx");
		}


        /// <summary>
        /// Register javascript for hiding and showing filter panel
        /// </summary>
        /// <param name="displayPanelID">ClientId of panel to show and hide</param>
        /// <param name="showPanelLinkId">ClientId of link or other control containing link for showing the panel</param>
        /// <param name="hidePanelLinkId">ClientId of link or other control containing link for hiding the panel</param>
        private void registerJScripyVisibilitySetting(string displayPanelID, string showPanelLinkId, string hidePanelLinkId)
        {
            string jscode = "<script language=\"javascript\">\n";
            jscode += "function setviewpanelvisible() {\n";
            jscode += "var rescontrol = document.getElementById('" + displayPanelID + "');\n";
            //lblDisplayViewPanel
            jscode += "var displayLabel = document.getElementById('" + showPanelLinkId + "');\n";
            jscode += "var hideLabel = document.getElementById('" + hidePanelLinkId + "');\n";
            jscode += "rescontrol.style.display  = 'inline'; ";
            jscode += "displayLabel.style.display  = 'none'; ";
            jscode += "hideLabel.style.display  = 'inline'; ";
            //visible

            jscode += "}\n </script>";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), displayPanelID + "setviewpanelvisible" + this.ID, jscode);

           

            //hideviewpanel
            jscode = "<script language=\"javascript\">\n";
            jscode += "function hideviewpanel() {\n";
            jscode += "var rescontrol = document.getElementById('" + displayPanelID + "');\n";
            jscode += "var displayLabel = document.getElementById('" + showPanelLinkId + "');\n";
            jscode += "var hideLabel = document.getElementById('" + hidePanelLinkId + "');\n";


            jscode += "displayLabel.style.display  = 'inline'; ";
            jscode += "rescontrol.style.display  = 'none'; ";
            jscode += "hideLabel.style.display  = 'none'; ";
            //visible

            jscode += "}\n </script>";
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), displayPanelID + "hideviewpanel" + this.ID, jscode);


        }

        private void setShowHideLinksVisibility(WebControl displayPanel, WebControl PlaceHolderHide, WebControl PlaceHolderShow)
        {

            if (!displayViewPanel)
            {
                PlaceHolderHide.Style.Add("display", "none");
                PlaceHolderShow.Style.Add("display", "inline");
                displayPanel.Style.Add("display", "none");
            }
            else
            {
                PlaceHolderHide.Style.Add("display", "block");
                PlaceHolderShow.Style.Add("display", "none");
                displayPanel.Style.Add("display", "block");
            }
                
        }

        private void SetUpDisplayLinks()
        {
            //lblDisplayViewPanel = new HyperLink();
            //lblDisplayViewPanel.NavigateUrl = "#";
          //  lnkDisplayViewPanel.ID = "lblDisplayViewPanel";
          //  lnkDisplayViewPanel.CssClass = "linkLabel";
            lnkDisplayViewPanel.Attributes.Add("onclick", "javascript:setviewpanelvisible();return false;");
           // this.PlaceHolderShow.Controls.Add(lnkDisplayViewPanel);
           
            //lblHideViewPanel = new HyperLink();
            //lblHideViewPanel.NavigateUrl = "#";
         //   lnkHideViewPanel.CssClass = "linkLabel";
            lnkHideViewPanel.Attributes.Add("onclick", "javascript:hideviewpanel();return false;");
           // this.PlaceHolderHide.Controls.Add(lblHideViewPanel);
           
        }

        public void AddControlToCommandLine(WebControl webControl)
        {
            this.commandlinePlaceHolder.Controls.Add(webControl);
        }

        private bool displayViewPanel
        {
            get
            {
                return ViewState["displayViewPanel"] == null ? false : (bool)ViewState["displayViewPanel"];
            }
            set
            {
                ViewState["displayViewPanel"] = value;
            }
        }
	}
}
