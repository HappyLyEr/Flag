using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GASystem.WebControls.ListControls
{
    class FlagListCommandItemTemplate : System.Object, ITemplate
    {
        const string LISTCOMMANDTEMPLATE = "~/gagui/usercontrols/lists/ListCommandTemplate.ascx";
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
            
        //}

        private bool _displayEditLink = false;
        private bool _inEditCountMode = false;
        private bool _inEditMode = false;
        private bool _displayExportToExcelLink = false;
        private bool _displayUpdateAndCreateNewLink = false;
        private bool _displayCancelSortLink = true;
        private bool _displayImportExcelLink = false;

        public FlagListCommandItemTemplate(bool inEditMode, bool displayEditLink, bool displayExportToExcelLink, bool displayUpdateAndCreateNewLink, bool inEditCountMode)
        {
            _inEditMode = inEditMode;
            _displayEditLink = displayEditLink;
            _displayExportToExcelLink = displayExportToExcelLink;
            _displayUpdateAndCreateNewLink = displayUpdateAndCreateNewLink;
            _inEditCountMode = inEditCountMode;
        }

        public FlagListCommandItemTemplate(bool inEditMode, bool displayEditLink, bool displayExportToExcelLink, bool displayUpdateAndCreateNewLink, bool inEditCountMode, bool displayCancelSortLink)
        {
            _inEditMode = inEditMode;
            _displayEditLink = displayEditLink;
            _displayExportToExcelLink = displayExportToExcelLink;
            _displayUpdateAndCreateNewLink = displayUpdateAndCreateNewLink;
            _inEditCountMode = inEditCountMode;
            _displayCancelSortLink = displayCancelSortLink;
        }

        public FlagListCommandItemTemplate(bool inEditMode, bool displayEditLink, bool displayExportToExcelLink, bool displayUpdateAndCreateNewLink, bool inEditCountMode, bool displayCancelSortLink, bool displayImportExcelLink)
        {
            _inEditMode = inEditMode;
            _displayEditLink = displayEditLink;
            _displayExportToExcelLink = displayExportToExcelLink;
            _displayUpdateAndCreateNewLink = displayUpdateAndCreateNewLink;
            _inEditCountMode = inEditCountMode;
            _displayCancelSortLink = displayCancelSortLink;
            _displayImportExcelLink = displayImportExcelLink;
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {

            

            GASystem.UserControls.Lists.ListCommandTemplate cmdTemplate = (GASystem.UserControls.Lists.ListCommandTemplate)container.Page.LoadControl(LISTCOMMANDTEMPLATE);
            cmdTemplate.IsEditMode = _inEditMode;
            cmdTemplate.DisplayEditLink = _displayEditLink && !_inEditMode;
            cmdTemplate.DisplayEditCountLink = _displayEditLink && !_inEditMode && _displayUpdateAndCreateNewLink;
            cmdTemplate.DisplayExportToExcelLink = _displayExportToExcelLink;
            cmdTemplate.DisplayImportFromExcelLink = _displayImportExcelLink;
            cmdTemplate.DisplayUpdateAndCreateNewLink = _displayUpdateAndCreateNewLink && _inEditCountMode;
            cmdTemplate.DisplayUpdateLink = _inEditMode && !_inEditCountMode;
            cmdTemplate.DisplayCancelSortLink = _displayCancelSortLink;
            container.Controls.Add(cmdTemplate);

        }

        #endregion






    }
}
