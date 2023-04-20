namespace GASystem.GAControls.ViewForm
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.BusinessLayer;
	using GASystem.DataModel;
	using GASystem.GAControls;
	using GASystem;
	using GASystem.AppUtils;
    using Telerik.WebControls;

	/// <summary>
	///		Summary description for ViewWorkItem.
	/// </summary>
	public class ViewWorkItem : System.Web.UI.UserControl
	{
		//protected System.Web.UI.WebControls.PlaceHolder placeHolderAction;
		protected System.Web.UI.WebControls.PlaceHolder placeHolderActionOwner;


		protected System.Web.UI.WebControls.PlaceHolder placeHolderWorkitem;

		protected GASystem.GAControls.ViewDataClass MyViewDataClass;
        protected GASystem.GAControls.ViewDataClass MyActionViewDataClass;
     

		//protected GASystem.ViewDataRecord ActionViewDataClass;
		protected GASystem.GAControls.ActionOwnerContext myActionOwnerContext;
		protected GAControls.Workflow.WorkitemStatus wStatus;
		//protected GAControls.ViewDataClass 
        protected GASystem.ViewDataRecord ViewDataRecordWorkitemHeader;

		private bool hasEditPermissions = false;
		
		private RadTabStrip formPagesTabStrip;
        private RadMultiPage formPagesMultiPage;
       

        private int workitemRowId;
        private bool hasEditPermissionsOnRecord = false;

        private PageView pageViewOwner;
        private PageView pageViewWorkitem;
        private PageView pageViewAction;


		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
           

      
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
			
        
			
		//	myActionOwnerContext = (GAControls.ActionOwnerContext)this.Page.LoadControl("~/gagui/GAControls/Workflow/ActionOwnerContext.ascx");
			MyViewDataClass = (ViewDataClass)this.Page.LoadControl("~/gagui/GAControls/ViewDataClass.ascx");
			wStatus = (GAControls.Workflow.WorkitemStatus)this.Page.LoadControl("~/gagui/GAControls/workflow/workitemstatus.ascx");
	//		ActionViewDataClass = (GASystem.ViewDataRecord)UserControls.UserControlUtils.GetUserControl(UserControls.UserControlType.ViewDataRecord, this.Page);
            ViewDataRecordWorkitemHeader = (ViewDataRecord)this.Page.LoadControl("~/gagui/UserControls/viewDataRecord.ascx");

            MyActionViewDataClass = (ViewDataClass)this.Page.LoadControl("~/gagui/GAControls/ViewDataClass.ascx");

			//this.placeHolderActionOwner.Controls.Add(myActionOwnerContext);	
		   // myActionOwnerContext.DisplayPanelId = "viewpanel";
           
            //display only the field defined as workitems objectname in workitemheader info above paging.
            ClassDescription cdw = ClassDefinition.GetClassDescriptionByGADataClass(GADataClass.GAWorkitem);
            ViewDataRecordWorkitemHeader.AddColumnToDisplay(cdw.ObjectName);



            //add to tabstrip
            formPagesTabStrip = new RadTabStrip();
            formPagesTabStrip.ID = "editform_pagesPlaceHolder";
            formPagesTabStrip.Skin = "FlagWizard";
 //           formPagesTabStrip.EnableViewState = false;
            placeHolderWorkitem.Controls.Add(formPagesTabStrip);

            //add general workiteminfo    
            placeHolderWorkitem.Controls.Add(ViewDataRecordWorkitemHeader);


            //add pages
            formPagesMultiPage = new RadMultiPage();
            formPagesMultiPage.ID = "editform_pagesPlaceHolderMultipage";
 //           formPagesMultiPage.EnableViewState = false;
            placeHolderWorkitem.Controls.Add(formPagesMultiPage);
            formPagesTabStrip.MultiPageID = formPagesMultiPage.ID;

            //first page
            formPagesTabStrip.Tabs.Add(new Tab(AppUtils.Localization.GetGuiElementText("GAWorkitem")));
            formPagesTabStrip.SelectedIndex = 0;
            formPagesMultiPage.SelectedIndex = 0;
            pageViewWorkitem = new PageView();
  //          pageViewWorkitem.EnableViewState = false;
            pageViewWorkitem.ID = "pageviewworkitem";


            //pageViewWorkitem.Controls.Add(createEmptyLabelSeparator(AppUtils.Localization.GetGuiElementText(GADataClass.GAWorkitem.ToString()), "firstpage"));

            formPagesMultiPage.PageViews.Add(pageViewWorkitem);

            //second page
            formPagesTabStrip.Tabs.Add(new Tab(AppUtils.Localization.GetCaptionText("OwnerClass")));
            formPagesTabStrip.SelectedIndex = 0;
            formPagesMultiPage.SelectedIndex = 0;
            pageViewOwner = new PageView();

            pageViewOwner.ID = "pageviewactionowner";
            formPagesMultiPage.PageViews.Add(pageViewOwner);


            //third page - action details
            formPagesTabStrip.Tabs.Add(new Tab(AppUtils.Localization.GetGuiElementText(GADataClass.GAAction.ToString())));
            formPagesTabStrip.SelectedIndex = 0;
            formPagesMultiPage.SelectedIndex = 0;
            pageViewAction = new PageView();
            formPagesMultiPage.PageViews.Add(pageViewAction);




            pageViewWorkitem.Controls.Add(wStatus);
            setupView(workitemRowId);

		}

        private HtmlTable createEmptyLabelSeparator(string separatorText, string separatorId)
        {
            //create rows
            HtmlTable tbl = new HtmlTable();
            tbl.ID = "editform_seperator" + separatorId;
            tbl.Attributes.Add("class", "EditForm_Table_wizardSeparator");
            tbl.Border = 0;
            tbl.CellPadding = 0;
            tbl.CellSpacing = 0;

            HtmlTableRow tblRow;

            tblRow = new HtmlTableRow();
            tblRow.ID = tbl.ID + separatorId;
            tbl.Rows.Add(tblRow);

            //label cell
            HtmlTableCell cell = new HtmlTableCell();
            cell.ID = separatorId + "seperatorc1labels";
            tblRow.Cells.Add(cell);

            Label lbl = new Label();
            lbl.ID = "Label_seperator" + separatorId;
            cell.Controls.Add(lbl);
            lbl.Text = separatorText;

            //cell.Attributes.Add("class", "FieldViewLabelCell");

            return tbl;
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

		public bool HasEditPermissions 
		{
			set {hasEditPermissions = value;}
			get {return hasEditPermissions;}
		}
        
        public bool HasEditPermissionsOnRecord 
		{
			set {hasEditPermissionsOnRecord = value;}
			get {return hasEditPermissionsOnRecord;}
		}


		public void SetupMyViewDataClass(int RowId) 
		{
            workitemRowId = RowId;
        }

        private void setupView(int RowId) {
			int actionId = -1;
			wStatus.Message = string.Empty;
			try 
			{

                MyViewDataClass.DisplayEditLink = HasEditPermissions; 
				wStatus.WorkitemId = RowId;
				//				wStatus.Workitem = wi;
				BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);
				WorkitemDS wds = (WorkitemDS) bc.GetByRowId(RowId);
				actionId = wds.GAWorkitem[0].ActionRowId;
				
                
                wStatus.DataRecordSet = wds;
				wStatus.HasEditPermission = hasEditPermissions;
                wStatus.HasEditPermissionsOnRecord = HasEditPermissionsOnRecord;

                ViewDataRecordWorkitemHeader.RecordDataSet = wds;
                ViewDataRecordWorkitemHeader.DataClass = GASystem.DataModel.GADataClass.GAWorkitem.ToString();
                // Tor 20140320 Added ownerclass. Parameter required to setup form correctly. Field GAFieldDefinitions.HideIfOwnerClass (TextFree3)
                ViewDataRecordWorkitemHeader.SetupForm("tor");


			} 
			catch (Exception ex) 
			{
				wStatus.Message += ex.Message;
			}
					
			if (actionId == -1)   //TODO replace this with a try-catch and gaexception
			{
				wStatus.Message +=string.Format(Localization.GetGuiElementText("NoActionInWorkitem"));
				return;
			}
			

			//verify that gaaction record exists. May not be replicated yet.
			GADataRecord actionRecord = new GASystem.DataModel.GADataRecord(actionId, GADataClass.GAAction);
			if (BusinessLayer.Utils.RecordSetUtils.DoesRecordExist(actionRecord)) 
			{
				//get owner of action
				GADataRecord actionOwner = BusinessLayer.DataClassRelations.GetOwner(new GADataRecord(actionId, GADataClass.GAAction));
				//display owner of action
				//myActionOwnerContext.SetActionId(actionId);

                string ownerName = getNameForDataRecord(actionOwner);
                string actionOwnerText = GASystem.AppUtils.Localization.GetGuiElementText(actionOwner.DataClass.ToString()) + " " + ownerName;

                pageViewOwner.Controls.Add(createEmptyLabelSeparator(actionOwnerText, "secondpage"));

                pageViewOwner.Controls.Add(MyViewDataClass);
                MyViewDataClass.DisplayWorkflowTabs = false;
				MyViewDataClass.DataRecord = actionOwner;
				MyViewDataClass.GenerateView();
	
			} 
			else 
			{
		
				MyViewDataClass.DataRecord = null;  //set datarecord to null, control will then not be rendered
				wStatus.ErrorMessage =string.Format(Localization.GetErrorText("RecordNotFound"), Localization.GetGuiElementText(GADataClass.GAAction.ToString()));
				
			}

            //add myaction to page
            pageViewAction.Controls.Add(MyActionViewDataClass);
            MyActionViewDataClass.DataRecord = new GADataRecord(actionId, GADataClass.GAAction);
            MyActionViewDataClass.DisplayEditLink = Security.HasUpdatePermission(new GADataRecord(actionId, GADataClass.GAAction)) && Security.HasEditPermissionOnRecord(new GADataRecord(actionId, GADataClass.GAAction));
            MyActionViewDataClass.DisplayReportLink = false;
            MyActionViewDataClass.DisplayWorkflowTabs = false;
            
            MyActionViewDataClass.GenerateView();
			
			
			
		}

        private string getNameForDataRecord(GADataRecord DataRecord)
        {
            string ownerName;
            DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(DataRecord);
            AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataRecord.DataClass);

            if (cd.ObjectName != string.Empty)
            {
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(cd.ObjectName))
                    ownerName = ds.Tables[0].Rows[0][cd.ObjectName].ToString();
                else
                    ownerName = "";

            }
            else
            {
                ownerName = "";
            }
            return ownerName;
        }

	
	}
}
