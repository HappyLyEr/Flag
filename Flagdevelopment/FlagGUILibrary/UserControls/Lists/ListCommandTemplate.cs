using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using GASystem.DataModel;

namespace GASystem.UserControls.Lists
{
   
    

    public class ListCommandTemplate : System.Web.UI.UserControl
    {
        private bool editMode = false;
        private string _editSelectedText = string.Empty;
        private string _editCountText = string.Empty;
        private string _updateSelectedText = string.Empty;
        private string _updateAndCreateNewSelectedText = string.Empty;
        private string _refreshRecordsText = string.Empty;
        private string _cancelEditText = string.Empty;
        private string _exportToExcelText = string.Empty;
        private string _importFromExcelText = string.Empty;

        private bool displayEditLink = false;
        private bool displayEditCountLink = false;
        private bool _displayExportToExcelLink = false;
        private bool _displayImportFromExcelLink = false;
        private bool _showUpdateAndCreateNewLink = false;
        private bool _showUpdateLink = false;


        protected LinkButton btnCancelSort;



        public string CancelEditText
        {
            get { return _cancelEditText; }
            set { _cancelEditText = value; }
        }

        public string ExportToExcelText
        {
            get { return _exportToExcelText; }
            set { _exportToExcelText = value; }
        }

        public string ImportFromExcelText
        {
            get { return _importFromExcelText; }
            set { _importFromExcelText = value; }
        }

        public string EditSelectedText
        {
            get { return _editSelectedText; }
            set { _editSelectedText = value; }
        }

        public string EditCountText
        {
            get { return _editCountText; }
            set { _editCountText = value; }
        }

        public string UpdateSelectedText
        {
            get { return _updateSelectedText; }
            set { _updateSelectedText = value; }
        }

        public string UpdateAndCreateNewSelectedText
        {
            get { return _updateAndCreateNewSelectedText; }
            set { _updateAndCreateNewSelectedText = value; }
        }

        private bool _displayCancelSortLink;

        public bool DisplayCancelSortLink
        {
            get { return _displayCancelSortLink; }
            set { _displayCancelSortLink = value; }
        }
	

        //public string RefreshRecordsText
        //{
        //    get { return _refreshRecordsText; }
        //    set { _refreshRecordsText = value; }
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CancelEditText = GASystem.AppUtils.Localization.GetGuiElementText("CancelEditText");
            EditSelectedText = GASystem.AppUtils.Localization.GetGuiElementText("EditSelectedText");
            UpdateSelectedText = GASystem.AppUtils.Localization.GetGuiElementText("UpdateSelectedText");
            UpdateAndCreateNewSelectedText = GASystem.AppUtils.Localization.GetGuiElementText("UpdateAndCreateNewSelectedText");
            ExportToExcelText = GASystem.AppUtils.Localization.GetGuiElementText("exporttoexcel");
            EditCountText = GASystem.AppUtils.Localization.GetGuiElementText("EditCountText");
           // RefreshRecordsText = GASystem.AppUtils.Localization.GetGuiElementText("RefreshRecordsText");
            btnCancelSort.Text = GASystem.AppUtils.Localization.GetGuiElementText("CancelSort");
            ImportFromExcelText = GASystem.AppUtils.Localization.GetGuiElementText("importFromExcel");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
      
        }

        public bool IsEditMode
        {
            get { return editMode; }
            set { editMode = value; }
        }

        public bool DisplayEditLink
        {
            get { return displayEditLink; }
            set { displayEditLink = value; }
        }

        public bool DisplayEditCountLink
        {
            get { return displayEditCountLink; }
            set { displayEditCountLink = value; }
        }

        public bool DisplayUpdateAndCreateNewLink
        {
            get { return _showUpdateAndCreateNewLink; }
            set { _showUpdateAndCreateNewLink = value; }
        }

        public bool DisplayUpdateLink
        {
            get { return _showUpdateLink; }
            set { _showUpdateLink = value; }
        }

        //public bool DisplayEditAndCreateNewLink
        //{
        //    get { return ShowEditAndCreateNewLink && IsEditMode; }
        //}
       


        public bool DisplayExportToExcelLink
        {
            get { return _displayExportToExcelLink; }
            set { _displayExportToExcelLink = value; }
        }

        public bool DisplayImportFromExcelLink
        {
            get { return _displayImportFromExcelLink; }
            set { _displayImportFromExcelLink = value; }
        }
    }


}
