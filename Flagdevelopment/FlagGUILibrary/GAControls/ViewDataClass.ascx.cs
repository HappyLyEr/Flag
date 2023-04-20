using System.Collections;
using GASystem.BusinessLayer;
using GASystem.DataAccess;

namespace GASystem.GAControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.DataModel;
	using GASystem.GAGUIEvents;
    using GASystem.AppUtils;
using log4net;
    using GASystem.GUIUtils;
    using GASystem.GAControls.Workflow;
	/// <summary>
	///		Summary description for ViewDataClass.
	/// </summary>
	public class ViewDataClass : System.Web.UI.UserControl
	{
		protected GASystem.ViewDataRecord MyViewDataRecord;
        //protected System.Web.UI.WebControls.Image ImageEdit;
        //protected System.Web.UI.WebControls.Image ImagePrint;
		protected GASystem.MemberDataTabs MyMemberDataTabs;
		protected System.Web.UI.WebControls.PlaceHolder InfoPlaceHolder;
        protected System.Web.UI.WebControls.PlaceHolder workItemPlaceHolder;
        protected System.Web.UI.WebControls.PlaceHolder warningPlaceholder;
        protected System.Web.UI.WebControls.PlaceHolder messagePlaceholder;
// Tor 20150226	changed to HyperLink
        //protected System.Web.UI.WebControls.LinkButton EditLinkTop;
        protected System.Web.UI.WebControls.HyperLink EditLinkTop;
        // Tor 20150226 End
        protected System.Web.UI.WebControls.LinkButton AddRemedialAction;
		protected System.Web.UI.WebControls.HyperLink ReportLinkTop;
        //protected System.Web.UI.WebControls.LinkButton EditLink;
        //protected System.Web.UI.WebControls.HyperLink ReportLink;
		protected System.Web.UI.WebControls.Image ImageEditTop;
		protected System.Web.UI.WebControls.Image ImagePrintTop;
        protected System.Web.UI.WebControls.Image ImageAddRemedialAction;
        protected System.Web.UI.WebControls.HyperLink HelpLink;
        

        
		

		public event GACommandEventHandler EditRecord;

		private static readonly ILog _logger = LogManager.GetLogger(typeof(ViewDataClass));

        private bool _displayWorkflowTabs = true;
        private string lastCommand = string.Empty;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if (this.IsPostBack && DataRecord != null)  //adding check for null here because it might be a postback from editform
			{
				setupView();	
			}
			//set norwegian captions
            //EditLink.Text = GASystem.AppUtils.Localization.GetGuiElementText("Edit");
            //ReportLink.Text = GASystem.AppUtils.Localization.GetGuiElementText("PrintReport");
			EditLinkTop.Text = GASystem.AppUtils.Localization.GetGuiElementText("Edit");
			ReportLinkTop.Text = GASystem.AppUtils.Localization.GetGuiElementText("PrintReport");

            AddRemedialAction.Text = GASystem.AppUtils.Localization.GetGuiElementText("AddRemedialAction");
            
            //AddRemedialAction.Visible = false;  //Hide for the time being, need better rules on when to display

            GeneralQueryStringUtils querystring = new GeneralQueryStringUtils(this.Page.Request);
            lastCommand = querystring.getSingleAlphaNumericQueryStringParam("lastCmd");
           
		}

       

		public GADataRecord Owner
		{
			get
			{
				return null==ViewState["GADataRecordOwner"+this.ID] ? null : (GADataRecord) ViewState["GADataRecordOwner"+this.ID];
			}
			set
			{
				ViewState["GADataRecordOwner"+this.ID] = value;
			}
		}

		public GADataRecord DataRecord
		{
			get
			{
				return ViewState["GADataRecordDataRecord"+this.ID] == null ? null : (GADataRecord) ViewState["GADataRecordDataRecord"+this.ID];
			}
			set
			{
				ViewState["GADataRecordDataRecord"+this.ID] = value;
			}
		}

		public void GenerateView() 
		{
            //set help link
            HelpLink.Text = Localization.GetGuiElementText("Help");
            HelpLink.NavigateUrl = GUIUtils.LinkUtils.GenerateHelpLink(this.DataRecord.DataClass.ToString());
			
            
            
            MyMemberDataTabs.Owner = DataRecord;
            MyMemberDataTabs.DisplayWorkflowTabs = this.DisplayWorkflowTabs;
			MyMemberDataTabs.SetupTabsNavigation();
			MyMemberDataTabs.DisplayEditButton = false;
			
		    AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(this.DataRecord.DataClass);
			this.DisplayReportLink = cd.HasReport;

            //workItemPlaceHolder.Controls.Add(new GASystem.GAControls.Workflow.WorkitemNotifierWebControl(this.DataRecord));

		
		}

        protected override void OnPreRender(EventArgs e)
        {
            if (DataRecord != null && !this.IsPostBack)
            {
                setupView();
            }

            AddRemedialAction.Visible = BusinessLayer.DataClassRelations.IsDataClassValidMember(this.DataRecord.DataClass, GADataClass.GAAction) && Security.HasCreatePermission(GADataClass.GAAction, this.DataRecord);
            ImageAddRemedialAction.Visible = AddRemedialAction.Visible;

            // Tor 20150226 below moved from OnInit
            //			// Tor 20150226 Start removed line to avoid postback, replaced by line below
            // this.EditLinkTop.Click += new System.EventHandler(this.EditLink_Click);
            if (this.DataRecord != null)
                this.EditLinkTop.NavigateUrl = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordDetails(this.DataRecord.DataClass.ToString(), this.DataRecord.RowId.ToString());
            // Tor 20150226 End


            //show lastcommand info
            if (!this.Page.IsPostBack && lastCommand != string.Empty)
            {
                string lastCmdMsg = Localization.GetUserMessageText(this.DataRecord.DataClass.ToString() + "_" + lastCommand);
                if (lastCmdMsg != string.Empty)
                {
                    Label lastCmdMessage = new Label();
                    lastCmdMessage.Text = lastCmdMsg;
                    lastCmdMessage.CssClass = "form_cmdmessage";
                    messagePlaceholder.Controls.Add(lastCmdMessage);
                    messagePlaceholder.Visible = true;
                }

            }




            base.OnPreRender(e);
        }


		private void setupView() 
		{
			try 
			{
                bool hasRecord = true;
				MyViewDataRecord.RecordDataSet = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsByDataRecord(DataRecord);

                if (MyViewDataRecord.RecordDataSet.Tables.Count > 0 &&
                    MyViewDataRecord.RecordDataSet.Tables[0].Rows.Count == 0)
                {
                    Label child = new Label();
                    child.CssClass = "MessageNormal";
                    child.Text = Localization.GetErrorText("NotFoundOrReadAccess");
                    MyViewDataRecord.Controls.Add(child);

                    DisplayReportLink = false;

                    hasRecord = false;
                }

                if (hasRecord)
                {
                    //get lists selected for this control
                    GASystem.BusinessLayer.BusinessClass bcListsSelected = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAListsSelected);
                    DataSet dsListsSelected = bcListsSelected.GetByOwner(this.DataRecord, null);
                    MyViewDataRecord.RecordDataSet.Tables.Add(dsListsSelected.Tables[GADataClass.GAListsSelected.ToString()].Copy());


                    MyViewDataRecord.DataClass = DataRecord.DataClass.ToString();

                    // Tor 20140320 Added ownerclass. 
                    // Parameter required to setup form correctly ref field GAFieldDefinitions.HideIfOwnerClass (TextFree3)
                    // get owner record
                    //                GADataRecord member = new GADataRecord();
                    //                member.DataClass=this.DataRecord.DataClass;
                    //                member.RowId=this.DataRecord.RowId;
                    GADataRecord owner;
                    if (DataRecord.DataClass != GADataClass.GAFlag)
                    {
                        owner = DataClassRelations.GetOwner(DataRecord);
                        MyViewDataRecord.Owner = owner;
                        MyViewDataRecord.SetupForm(owner.DataClass.ToString());
                    }
                    // Tor 20140320 end
                }

                //add list of overlapping records for exposed hours
                if (this.DataRecord.DataClass == GADataClass.GAExposedHoursGroupView)
                    ShowOverLappingRecords(DataRecord);

				AddDataRecordLists();
				
				if (DataRecord.DataClass != GADataClass.GAReportInstance) 
				{
				//	ReportLink.NavigateUrl = GASystem.Reports.Utils.URLGenerator.GenerateURLForSingleRecordDetails(DataRecord, GASystem.Reports.Utils.ReportExportType.PDF);
                    if (DataRecord.DataClass == GADataClass.GAAnalysis)
                        ReportLinkTop.NavigateUrl = String.Format(GASystem.Reports.Utils.URLGenerator.GenerateURLFormatStringForAnalysisCube(), DataRecord.RowId.ToString());
                    else
                        ReportLinkTop.NavigateUrl = GASystem.Reports.Utils.URLGenerator.GenerateURLForSingleRecordDetails(DataRecord, GASystem.Reports.Utils.ReportExportType.PDF);
				}
				else 
				{
				//	ReportLink.NavigateUrl = GASystem.Reports.Utils.URLGenerator.GenerateURLForFullReport(DataRecord, Owner, GASystem.Reports.Utils.ReportExportType.PDF);
                       ReportLinkTop.NavigateUrl = GASystem.Reports.Utils.URLGenerator.GenerateURLForFullReport(DataRecord, Owner, GASystem.Reports.Utils.ReportExportType.PDF);
				}

               
               
			} 
			catch (Exception ex) 
			{
				DisplayUserMessage("</br>" + AppUtils.Localization.GetErrorText("StandardGAErrorMessage"), GASystem.UserMessage.UserMessageType.Error);
				_logger.Error(ex.Message, ex);

			}

		}

        private void ShowOverLappingRecords(GADataRecord dataRecord)
        {
            //get raw datarecord
            BusinessLayer.BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(dataRecord.DataClass);
            DataSet dataSet = bc.GetByRowId(dataRecord.RowId);


            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(DataRecord.DataClass);
            if (!dataSet.Tables[0].Columns.Contains(cd.DateFromField) || !dataSet.Tables[0].Columns.Contains(cd.DateFromField))
                return;

            //get dates
            DateTime dateFrom = dataSet.Tables[0].Rows[0][cd.DateFromField] == DBNull.Value ? DateTime.MinValue : (DateTime)dataSet.Tables[0].Rows[0][cd.DateFromField];
            DateTime dateTo = dataSet.Tables[0].Rows[0][cd.DateToField] == DBNull.Value ? new DateTime(9000,1,1) : (DateTime)dataSet.Tables[0].Rows[0][cd.DateToField];
            int departmentCategory = dataSet.Tables[0].Rows[0]["DepartmentCategoryListsRowId"] == DBNull.Value ? -1 : (int)dataSet.Tables[0].Rows[0]["DepartmentCategoryListsRowId"];
            int vertical = dataSet.Tables[0].Rows[0]["bunksinroom"] == DBNull.Value ? -1 : (int)dataSet.Tables[0].Rows[0]["bunksinroom"];
            
            //create filter
            string filter = "DepartmentCategoryListsRowId = " + departmentCategory.ToString();
            filter = filter + " AND bunksinroom = " + vertical.ToString();
            filter += " and " + DataRecord.DataClass.ToString().Substring(2) + "rowid <> " + dataRecord.RowId.ToString(); 
            //check owner
            if (Owner == null)
                Owner = BusinessLayer.DataClassRelations.GetOwner(DataRecord, null);



            ListData.ListClassByOwnerAndTimeSpan overlappingList = new GASystem.GAControls.ListData.ListClassByOwnerAndTimeSpan(DataRecord.DataClass, this.Owner, dateFrom, dateTo, filter);

            overlappingList.HeaderText = "<br/>" + AppUtils.Localization.GetGuiElementText("WarningPersonnelCountOverlaps");
                
           
            this.warningPlaceholder.Controls.Add(overlappingList);
            overlappingList.GenerateControl();
            this.warningPlaceholder.Visible = overlappingList.ContainsRecords;
        }

		/// <summary>
		/// Add lists with more data about the datarecord. These are lists that should no be i the tabs
		/// </summary>
		private void AddDataRecordLists()
		{
            // Tor 20150924 replaced with GetNextLevelInViewDataClasses(this.DataRecord) which also checks permissions and if class is to be listed (specified in GASuperClassLinks.nTextFree1)
            //			ArrayList members = GASystem.BusinessLayer.DataClassRelations.GetNextLevelInViewDataClasses(this.DataRecord.DataClass);
            ArrayList members = GASystem.BusinessLayer.DataClassRelations.GetNextLevelInViewDataClasses(this.DataRecord);
			foreach (string member in members)
			{
				GADataClass memberDataClass = GADataRecord.ParseGADataClass(member);
                // Tor 20150924 permissiontest has been performed in .GetNextLevelInViewDataClasses(this.DataRecord)
                // if (Security.HasReadPermissionOnDataClassInContext(this.DataRecord, memberDataClass))
                this.InfoPlaceHolder.Controls.Add(new ViewDetailsList.GeneralViewDetail(DataRecord, memberDataClass));
			}
		}
		

		public bool DisplayReportLink 
		{
            get { return ReportLinkTop.Visible; }
			set {
				//ReportLink.Visible = value;
				ReportLinkTop.Visible = value;
				//this.ImagePrint.Visible = value;
				this.ImagePrintTop.Visible = value;
				
			}
		}

		public bool DisplayEditLink 
		{
            get { return EditLinkTop.Visible; }
			set 
			{
                //EditLink.Visible = value;
                //ImageEdit.Visible = value;
				EditLinkTop.Visible = value;
				ImageEditTop.Visible = value;
				
				//MyMemberDataTabs.DisplayEditButton = value;
				//MyMemberDataTabs.DisplayNewButton = value;
			}
		}

        public bool DisplayWorkflowTabs
        {
            set { _displayWorkflowTabs = value; }
            get { return _displayWorkflowTabs; }
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);

            messagePlaceholder = new PlaceHolder();
            messagePlaceholder.Visible = false;
          
            workItemPlaceHolder.Controls.AddAt(0, messagePlaceholder);


            warningPlaceholder = new PlaceHolder();
            warningPlaceholder.Visible = false;
            InfoPlaceHolder.Controls.AddAt(0, warningPlaceholder);

		}
		
		

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            // Tor 20150226 below moved to prerender
//			// Tor 20150226 Start removed line to avoid postback, replaced by line below
            // this.EditLinkTop.Click += new System.EventHandler(this.EditLink_Click);
            //                            this.EditLinkTop.NavigateUrl = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordDetails(this.DataRecord.DataClass.ToString(), this.DataRecord.RowId.ToString());

        // Tor 20150226 End
            this.AddRemedialAction.Click += new EventHandler(AddRemedialAction_Click);
		//	this.EditLink.Click += new System.EventHandler(this.EditLink_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}

        void AddRemedialAction_Click(object sender, EventArgs e)
        {
            PageDispatcher.GotoDataRecordNewPage(this.Page.Response, GADataClass.GAAction, this.DataRecord);
        }
		#endregion

		private void EditLink_Click(object sender, System.EventArgs e)
		{
			
			PageDispatcher.GotoDataRecordDetailsPage(this.Page.Response, DataRecord.DataClass, DataRecord.RowId, this.Owner);
//			if(EditRecord != null) 
//			{
//				GACommandEventArgs GAEventArgs = new GACommandEventArgs();
//				GAEventArgs.CommandName = "EditRecord";
//				GAEventArgs.CommandStringArgument = DataRecord.RowId.ToString();
//				GAEventArgs.CommandIntArgument = DataRecord.RowId;
//				EditRecord(this, GAEventArgs);
//			}
		}

		protected void DisplayUserMessage(string Message, UserMessage.UserMessageType MessageType)
		{
			try
			{
				UserMessage userMessageControl = (UserMessage) this.Page.LoadControl("~/gagui/UserControls/UserMessage.ascx");
				userMessageControl.MessageType = MessageType;
				userMessageControl.MessageText = Message;
				this.Controls.Add(userMessageControl);
			}
			catch (Exception e)
			{
				_logger.Error(e.Message, e);	
			}
		}


        
	}
}
