using System;
using System.Collections.Generic;
using System.Text;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.GAControls;
using GASystem;
using GASystem.AppUtils;
using System.Web.UI.WebControls;
using Telerik.WebControls;
using System.Globalization;


namespace GASystem.GAGUI.GUIUtils
{
    public class LeftMenuFrontController : System.Web.UI.WebControls.WebControl
    {
        private GADataClass myDataClass;
        private ClassDescription myClassDescription;
        private RadTabStrip  leftMenuTabs;
        private RadMultiPage leftMenuPages;
        private PlaceHolder placeHolderContent;
        private PlaceHolder placeHolderTabs;

        private const string LEFTMENUPAGES = "FlagLeftMenuPages";
        private const string LEFTMENUTABS = "FlagLeftMenuTabs";
        private const string SELECTEDINDEX = "selectedindex";

        private Tab workitemTab;
        private Tab navigationTab;

        public LeftMenuFrontController(GADataClass dataClass)
		{
			myDataClass = dataClass;
			myClassDescription = ClassDefinition.GetClassDescriptionByGADataClass(dataClass);
		}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
          


            this.Controls.Add(createPlaceholders());

            leftMenuPages = new RadMultiPage();
            leftMenuPages.ID = LEFTMENUPAGES;
            this.placeHolderContent.Controls.Add(leftMenuPages);
            leftMenuPages.PageViews.Add(createWorkItemPage());
            leftMenuPages.PageViews.Add(createHomeContextPage());
            leftMenuTabs = new RadTabStrip();
            leftMenuTabs.AutoPostBack = true;
            leftMenuTabs.ID = LEFTMENUTABS;
            this.placeHolderTabs.Controls.Add(GASystem.GUIUtils.HTMLLiteralTags.CreateTextElement("<div/>"));
            this.placeHolderTabs.Controls.Add(leftMenuTabs);
            this.placeHolderTabs.Controls.Add(GASystem.GUIUtils.HTMLLiteralTags.CreateTextElement("<div/>"));
            leftMenuTabs.MultiPageID = LEFTMENUPAGES;
            leftMenuTabs.Skin = "FlagVerticalTabs";


            workitemTab = new Tab();
            leftMenuTabs.Tabs.Add(workitemTab);
         

            navigationTab = new Tab();
            leftMenuTabs.Tabs.Add(navigationTab);

            leftMenuTabs.Tabs[0].Selected = true;
            leftMenuPages.PageViews[0].Selected = true;
            leftMenuTabs.Orientation = RadTabStripOrientation.VerticalLeftToRight;

            leftMenuTabs.TabClick += new TabStripEventHandler(leftMenuTabs_TabClick);
        }

        void leftMenuTabs_TabClick(object sender, TabStripEventArgs e)
        {
            selectedIndex = e.Tab.Index;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            leftMenuPages.SelectedIndex = selectedIndex;
            leftMenuTabs.SelectedIndex = leftMenuPages.SelectedIndex;   //make sure that pages and tabs are in sync.

            string currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            if ("zh" == currentLanguage)
            {// 中文导航
                if (selectedIndex == 0)
                {
                    workitemTab.ImageUrl = "~/images/tabstrip/workitembluecn.png";
                    navigationTab.ImageUrl = "~/images/tabstrip/navigationorangecn.png";
                }
                else
                {
                    workitemTab.ImageUrl = "~/images/tabstrip/workitemorangecn.png";
                    navigationTab.ImageUrl = "~/images/tabstrip/navigationbluecn.png";
                }

            }
            else
            {// 英文及其他导航
                if (selectedIndex == 0)
                {
                    workitemTab.ImageUrl = "~/images/tabstrip/workitemblue.png";
                    navigationTab.ImageUrl = "~/images/tabstrip/navigationorange.png";
                }
                else
                {
                    workitemTab.ImageUrl = "~/images/tabstrip/workitemorange.png";
                    navigationTab.ImageUrl = "~/images/tabstrip/navigationblue.png";
                }

            }
        }

        private System.Web.UI.Control createPlaceholders()
        {
            placeHolderContent = new PlaceHolder();
            placeHolderTabs = new PlaceHolder();

            //create elements
            Table table = new Table();
            TableRow row = new TableRow();
            TableCell cellContent = new TableCell();
            TableCell cellTabs = new TableCell();

            //set childeren
            table.Controls.Add(row);
            row.Controls.Add(cellContent);
            row.Controls.Add(cellTabs);
            cellContent.Controls.Add(placeHolderContent);
            cellTabs.Controls.Add(placeHolderTabs);

            //set style
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.BorderWidth = 0;
            cellContent.CssClass = "FlagLeftMenu_Content";
            cellTabs.CssClass = "FlagLeftMenu_Tabs";

            return table;

        }




        private PageView createHomeContextPage()
        {
            System.Web.UI.Control homeContext = this.Page.LoadControl("~/gagui/GAControls/HomeContext.ascx");
            
            
            PageView page = new PageView();
            page.Controls.Add(homeContext);
            return page;
        }


        private PageView createWorkItemPage()
        {
            System.Web.UI.Control workitemList = this.Page.LoadControl("~/gagui/GAControls/Workflow/WorkitemShortListControl.ascx");
            

            PageView page = new PageView();
            page.Controls.Add(workitemList);
            return page;
        }

        private int selectedIndex
        {
            get 
            {
                if (this.Page.Session[this.ID + SELECTEDINDEX] == null)
                    return 0;
                try
                {
                    int index = (int)this.Page.Session[this.ID + SELECTEDINDEX];
                    return index;
                }
                catch
                {
                    return 0;
                }
            
            }
            set { this.Page.Session[this.ID + SELECTEDINDEX] = value; }
        }

    }
}
