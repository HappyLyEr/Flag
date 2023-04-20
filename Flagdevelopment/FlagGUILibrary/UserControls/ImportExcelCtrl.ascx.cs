using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.GAGUI.GAGUIEvents;
using System;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.WebControls;
using File = GASystem.BusinessLayer.File;

namespace GASystem
{


    /// <summary>
    ///		Summary description for ImportExcelCtrl.
    /// </summary>
    public partial class ImportExcelCtrl : System.Web.UI.UserControl
    {
        private string _validationImportExcelText;
        private string _InvalidFileTypeText;
        private static readonly object EventClosePanel = new object();
        private static readonly object EventSuccessfullyImported = new object();

        protected RadUpload RadUploadXLS;
        protected LinkButton BtnProcessImportXLS;
        protected LinkButton btnClosePanel;
        protected CustomValidator CustomValidatorXLS;
        protected Label LblSuccessMessage;

        //Provide a way to store the dataset in viewState
        [Bindable(true)]
        public String DataClass
        {
            get
            {
                object obj = ViewState["DataClass" + typeof(ImportExcelCtrl).ToString()];
                return (String)obj ?? null;
            }
            set
            {
                ViewState["DataClass" + typeof(ImportExcelCtrl).ToString()] = value;
            }
        }

        //Provide a way to store the owner in viewState
        [Bindable(true)]
        public GASystem.DataModel.GADataRecord Owner
        {
            get
            {
                object obj = ViewState["Owner" + typeof(ImportExcelCtrl).ToString()];
                return (GADataRecord)obj ?? null;
            }
            set
            {
                ViewState["Owner" + typeof(ImportExcelCtrl).ToString()] = value;
            }
        }

        [Bindable(true)]
        public string Text
        {
            get
            {
                object obj = ViewState["Text" + typeof(ImportExcelCtrl).ToString()];
                return (string)obj ?? "";
            }
            set
            {
                ViewState["Text" + typeof(ImportExcelCtrl).ToString()] = value;
            }
        }

        [Bindable(true)]
        public string ValidationImportExcelText
        {
            set { _validationImportExcelText = value; }
            get { return _validationImportExcelText; }
        }

        [Bindable(true)]
        public bool IsFinished
        {
            get
            {
                object obj = this.ViewState["IsFinished+" + typeof(ImportExcelCtrl).ToString()];
                return (bool?)obj ?? false;
            }
            set
            {
                this.ViewState["IsFinished+" + typeof(ImportExcelCtrl).ToString()] = (object)value;
            }
        }

        [Bindable(true)]
        public bool IsOpenModalView
        {
            get
            {
                object obj = this.ViewState["IsOpenModalView+" + typeof(ImportExcelCtrl).ToString()];
                return (bool?)obj ?? false;
            }
            set
            {
                this.ViewState["IsOpenModalView+" + typeof(ImportExcelCtrl).ToString()] = (object)value;
            }
        }

        protected string InvalidFileTypeText
        {
            get { return _InvalidFileTypeText; }
            set { _InvalidFileTypeText = value; }
        }

        public event EventHandler OnClosePanel
        {
            add
            {
                this.Events.AddHandler(ImportExcelCtrl.EventClosePanel, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(ImportExcelCtrl.EventClosePanel, (Delegate)value);
            }
        }

        public event EventHandler OnSuccessfullyImported
        {
            add
            {
                this.Events.AddHandler(ImportExcelCtrl.EventSuccessfullyImported, (Delegate)value);
            }
            remove
            {
                this.Events.RemoveHandler(ImportExcelCtrl.EventSuccessfullyImported, (Delegate)value);
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        private void ClosePanel_OnClick(EventArgs e)
        {
            EventExecutor(ImportExcelCtrl.EventClosePanel, e);
        }

        private void Excel_OnSuccessfullyImported(GAEventArgs<string> e)
        {
            EventExecutor(ImportExcelCtrl.EventSuccessfullyImported, e);
        }

        private void EventExecutor<T>(object eventType, T args) where T : EventArgs
        {
            EventHandler eventHandler = (EventHandler)this.Events[eventType];

            if (eventHandler == null)
                return;

            eventHandler((object)this, args);
        }

        internal string GetTempFolder()
        {
            return File.TemporaryPath;
            //return System.IO.Path.GetTempPath();
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
            this.EnableViewState = true;
            RadUploadXLS.TargetPhysicalFolder = GetTempFolder();
            ValidationImportExcelText = InvalidFileTypeText = GASystem.AppUtils.Localization.GetErrorText("ImportExcelInvalidExcelFile");
            Text = GASystem.AppUtils.Localization.GetGuiElementText("importFromExcel");
            this.LblSuccessMessage.Visible = false;
            this.btnClosePanel.Click += BtnClosePanel_Click;
            this.RadUploadXLS.Localization["Clear"] = GASystem.AppUtils.Localization.GetGuiElementText("ClearInputFile");

            this.Load += new System.EventHandler(this.Page_Load);
        }


        #endregion


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected void btnProcessImportXLS_OnClick(object sender, EventArgs e)
        {
            GASystem.DataModel.GADataClass dataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(this.DataClass);

            if (RadUploadXLS.UploadedFiles == null || RadUploadXLS.UploadedFiles.Count <= 0)
            {
                CustomValidatorXLS.Visible = true;
                this.IsOpenModalView = true;
                return;
            }

            UploadedFile file = RadUploadXLS.UploadedFiles[0];

            ExcelImport xls = new ExcelImport(dataClass, this.Owner);

            string uploadedFile = Path.Combine(RadUploadXLS.TargetPhysicalFolder, file.FileName);

            ExcelImportResult result = xls.ProcessExcelFileImport(uploadedFile);

            if (result.Success)
            {
                this.IsFinished = true;
                this.LblSuccessMessage.Visible = true;
                this.LblSuccessMessage.Text = result.Message;

                this.Excel_OnSuccessfullyImported(new GAEventArgs<string>(result.Message));
            }
            else
            {
                this.ValidationImportExcelText = result.Message;
                CustomValidatorXLS.Visible = true;
                CustomValidatorXLS.IsValid = false;
                LblSuccessMessage.Visible = false;
            }

            this.IsOpenModalView = true;
        }

        protected void BtnClosePanel_Click(object sender, EventArgs e)
        {
            this.IsOpenModalView = false;
            this.ClosePanel_OnClick(e);
        }
    }
}