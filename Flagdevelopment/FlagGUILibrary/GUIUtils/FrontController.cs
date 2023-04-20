using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.GAControls;
using GASystem;
using GASystem.AppUtils;
using FlagGUILibrary.WebControls.ListControls;

namespace GASystem.GAGUI.GUIUtils
{
	/// <summary>
	/// GA Front controller. Parses querystring and displays the appropriate usercontrols.
	/// </summary>
	public enum GAGUIAction {SelectRecord, EditRecord, NewRecord, ListRecords};
	
	public class FrontController : System.Web.UI.WebControls.WebControl
	{
		protected GASystem.GAControls.EditForm.AbstractDetailsForm MyFormDetails;
		protected GASystem.GAControls.ListData.ListClassDataRecords MyListClassDataRecords;
		protected GASystem.GAControls.ListData.ListClassAllWithinDataRecords MyListClassAllWithinDataRecords;

		protected GASystem.DataContextInfo DataContextInfoControl;
		protected GASystem.GAControls.ViewDataClass MyViewDataClass;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolderMain;
		
		protected GADataClass myDataClass;
		protected GADataRecord myOwner;
		protected GADataRecord myDataRecord = null;
		private bool showAllRecordsWithin = false;
		
		protected bool hasEditPermissions = true;    //changed to true by JOF see comment in onload event
		protected bool hasCreatePermissions = true;   //changed to true by JOF see comment in onload event

		protected GASystem.GUIUtils.QuerystringUtils myQueryString;

		protected GASystem.AppUtils.ClassDescription myClassDescription;

		private bool createMultipleRecords = false;
		int  RowId = -1;

		
		public FrontController(GADataClass DataClass)
		{
			myDataClass = DataClass;
			myClassDescription = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataClass);
			//
			// TODO: Add constructor logic here
			//
		}
		
		protected override void OnLoad(EventArgs e)
		{
		    //clear session datacache if this is not a postback

            if (!this.Page.IsPostBack)
                foreach (string dataClass in Enum.GetNames(typeof(GADataClass)))
                    this.Page.Session["RecordsDataSet" + dataClass] = null;

			//check for edit permissions
			//hasEditPermissions = utils.Security.HasEditPermissions(this.ModuleId);
			myQueryString = new GASystem.GUIUtils.QuerystringUtils(myDataClass, this.Page.Request);
			RowId = myQueryString.GetRowId();

            // Tor 201611 Security 20161122 set correct owner record to check 
            // if rowid=-1 then this is top toplevel recordtype or all under home has been clicked
            // if ClassDescription.ApplyAdditionalAccessControl is true
            //      set SetCurrentSubContext(1, GADataClass.GAFlag);
            // else set SetCurrentSubContext(GADataContext.InitialContextRecord());
            // Tor 20170222 start Rollback 20161122 
            //GADataRecord a = new GADataRecord(RowId, myDataClass);
            //if (RowId > -1)
            //// Tor 201611 Security 201161204 set current subcontext to owner of requested record
            //{
            //    a = GASystem.BusinessLayer.DataClassRelations.GetOwner(a);
            //    SessionManagement.SetCurrentSubContext(a);
            //    // 20161214 SessionManagement.SetCurrentSubContext(RowId, myDataClass);
            //}
            //else
            //{
            //    ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(myDataClass);
            //    if (cd.ApplyAdditionalAccessControl)
            //    { // top of top level, owned by GAFlag
            //        SessionManagement.SetCurrentSubContext(1, GADataClass.GAFlag);
            //    }
            //    else
            //    { // owner is current Home:  
            //        SessionManagement.SetCurrentSubContext(SessionManagement.GetCurrentDataContext().InitialContextRecord);
            //    }
            //}
            // Tor 20170222 End Rollback 20161122 
            // Tor 201611 Security 20161122 End

			myOwner = GASystem.AppUtils.GUIQueryString.GetOwner(this.Page.Request);
			
			//FW0910: Default these to false. Values are set below inside the if-else-statements
			hasEditPermissions = false;//Security.HasUpdatePermissionInContext(myOwner, myDataClass) ;//Security.HasUpdatePermission(new GADataRecord(RowId, myDataClass));
			hasCreatePermissions =  false;//Security.HasCreatePermission(myDataClass, SessionManagement.GetCurrentDataContext().SubContextRecord);

			//bool editRecord =  myQueryString.EditRecord && hasEditPermissions;
			//bool newRecord = (RowId == 0) && hasCreatePermissions;

//			Commented out by JOF, subcontext might be empty at this stage, casuing an exception. need to make this check at different locations through the 
//			load event. Do we need to rewrite this load event??
//			//FW1607: New security functionality. Use GA Security to determine edit/new rights
//			hasEditPermissions = Security.HasUpdatePermission(new GADataRecord(RowId, myDataClass));
//			hasCreatePermissions =  Security.HasCreatePermission(myDataClass, SessionManagement.GetCurrentDataContext().SubContextRecord);

			createMultipleRecords = myQueryString.createMultipleRecords;

			if (RowId > -1) 
				LoadSingleRecord();
			else 
				LoadList();
			base.OnLoad (e);
		}

		/// <summary>
		/// Display a single record in view, new or edit
		/// </summary>
		protected void LoadSingleRecord() 
		{
            bool newRecord = (RowId == 0);   // && hasCreatePermissions;
            //bool editRecord =  myQueryString.EditRecord;  //  && hasEditPermissions;
            if (!newRecord) hasEditPermissions = Security.HasUpdatePermission(new GADataRecord(RowId, myDataClass)) && Security.HasEditPermissionOnRecord(new GADataRecord(RowId, myDataClass));
            bool editRecord = myQueryString.EditRecord && hasEditPermissions;
            //bool newRecord = (RowId == 0);   // && hasCreatePermissions;

			if (newRecord)
			{
				DoUserAction("NEW");
				SetupEditForm(0);			
			}
			else 
			{
				//SessionManagement.SetCurrentSubContext(new GADataRecord(RowId, myDataClass));
				myDataRecord = new GADataRecord(RowId, myDataClass);
				myOwner = DataClassRelations.GetOwner(myDataRecord);
				if (editRecord) 
				{
					SetupEditForm(RowId);
					DoUserAction("EDIT");
				} 
				else
				{
					hasEditPermissions = Security.HasUpdatePermission(new GADataRecord(RowId, myDataClass)) && Security.HasEditPermissionOnRecord(new GADataRecord(RowId, myDataClass));
					SetupMyViewDataClass(RowId);
					DoUserAction("VIEW");
				}
			}
		}

		/// <summary>
		/// Display a list of records
		/// </summary>
		protected void LoadList() 
		{
//			if (myOwner = null)
//				myOwner = SessionManagement.GetCurrentDataContext().SubContextRecord;   //TODO, change this to get from querystring or similar

			
			
			if (IsTopLevelClass) 
			{
                // Tor 20140320 top classes to be checked with ownerclass GAFlag - then do not look for owner
                //hasEditPermissions = Security.HasUpdatePermissionInContext(myDataClass, null); 
                //hasCreatePermissions =  Security.HasCreatePermission(myDataClass, null);
                //myOwner = null;  //top level
                if (myDataClass.ToString() == "GAFlag")
                {
                    hasEditPermissions = Security.HasUpdatePermissionInContext(myDataClass, null);
                    hasCreatePermissions = Security.HasCreatePermission(myDataClass, null);
                }
                else
                {
                    GADataRecord classOwner = new GADataRecord(1, GADataClass.GAFlag);
                    hasEditPermissions = Security.HasUpdatePermissionInContext(myDataClass, classOwner);
                    hasCreatePermissions = Security.HasCreatePermission(myDataClass, classOwner);
                }
				//SessionManagement.SetCurrentSubContext(null);
				SetupList();
				DoUserAction("LIST");
			} 
			else 
			{
							
				if (myOwner == null)     //no owner specified in querystring, show list of all records within home context based on home context
				{
					myOwner = SessionManagement.GetCurrentDataContext().InitialContextRecord;
					
                    //test 20081028, added editpermissions setting for all whitin

					//changed 10.01.2007 JOF. listing all records within, can not show edit and create permission based on top level permission
                    hasEditPermissions = Security.HasUpdateWithinPermissionInContext(myDataClass, myOwner); //   Security.HasUpdatePermissionInContext(myDataClass, null); 
					hasCreatePermissions = false; // Security.HasCreatePermission(myDataClass, null);
										
					SetupListAllWithinRecord();
					DoUserAction("LIST");  //change this
				} 
				else   //owner specified in querystring, show all records for a specific owner
				{
					hasEditPermissions = Security.HasUpdatePermissionInContext(myDataClass, myOwner); 
					hasCreatePermissions =  Security.HasCreatePermission(myDataClass, myOwner);

					//SessionManagement.SetCurrentSubContext(myOwner);
					SetupList();
					DoUserAction("LIST");
									
				}
			}
		}

		private void DoUserAction(string userAction)
		{
			if (userAction.ToUpper().Equals("LIST"))
			{
				PlaceHolderMain.Visible = true;
				//MyViewDataClass.Visible = false;
				SetContextInfo(false);
			}
			else if (userAction.ToUpper().Equals("EDIT"))
			{
				PlaceHolderMain.Visible = true;
				//MyViewDataClass.Visible = false;
				SetContextInfo(true);
			}
			else if (userAction.ToUpper().Equals("NEW"))
			{
				PlaceHolderMain.Visible = true;
				//MyViewDataClass.Visible = false;
				SetContextInfoNewRecord();
			}
			else if (userAction.ToUpper().Equals("VIEW")) 
			{	
				PlaceHolderMain.Visible = true;
				MyViewDataClass.Visible = true;
				SetContextInfo(true);
			}
		}

		private void SetContextInfo(bool SingleRecord)
		{
			//GADataRecord owner =  SessionManagement.GetCurrentDataContext().SubContextRecord;
			DataContextInfoControl.CurrentDataClass = myDataClass;
			//DataContextInfoControl.OwnerDataRecord = owner;
			if (myDataRecord != null)
				DataContextInfoControl.ContextDataRecord = myDataRecord;    // SessionManagement.GetCurrentDataContext().SubContextRecord;
			else
                // Tor 20140709 if myOwner is null, 
                // if myDataRecord class = top level class and myDataRecord class owner is GAFlag then set myOwner to GAFlag/1 - for top level classes
                if (GASystem.BusinessLayer.Class.GetClassIsTop(myDataClass) && GASystem.BusinessLayer.Utils.RecordSetUtils.IfMemberDataClassHasOwnerDataClass(myDataClass, GADataClass.GAFlag))
                {
                    DataContextInfoControl.ContextDataRecord = new GADataRecord(1, GADataClass.GAFlag);
                }
                else
                {
                    DataContextInfoControl.ContextDataRecord = myOwner;
                }
            
// Tor 20140709                DataContextInfoControl.ContextDataRecord = myOwner;

            DataContextInfoControl.CurrentIsSingleRecord = SingleRecord;
			DataContextInfoControl.ViewAllRecordsWithin = showAllRecordsWithin; //  !IsTopLevelClass && (myOwner == null);
			DataContextInfoControl.SetupContextInfo();
			DataContextInfoControl.NewRecord = false;
		}
		
		private void SetContextInfoNewRecord()
        {
            // Tor 20140827 If owner==null and dataclass is owned by GAFlag, set owner data record = GAFlag/1
            if (myOwner == null)
            {
                SuperClassLinksDS Owner = GASystem.DataAccess.SuperClassDb.GetSuperClassLinksByMember(myDataClass);
                string a = Owner.Tables[0].Rows[0]["OwnerClass"].ToString();

                //if (Owner.Tables[0].Rows[0].Table.ToString() == "GAFlag")
                if (Owner.Tables.Count == 1 && a == "GAFlag")
                {
                    myOwner = new GADataRecord(1, GADataClass.GAFlag);
                }
            }


            SetContextInfo(true);
			DataContextInfoControl.OwnerDataRecord = myOwner; // SessionManagement.GetCurrentDataContext().SubContextRecord;
			DataContextInfoControl.NewRecord = true;
		}

		protected void SetupMyViewDataClass(int RowId)   //TODO  clean up
		{
			MyViewDataClass = (ViewDataClass)this.Page.LoadControl("~/gagui/GAControls/ViewDataClass.ascx");
			this.PlaceHolderMain.Controls.Add(MyViewDataClass);
            SessionManagement.SetCurrentSubContext(myDataRecord);	
			MyViewDataClass.DataRecord =  myDataRecord; // new GASystem.DataModel.GADataRecord(RowId, myDataClass);
			MyViewDataClass.DisplayEditLink = hasEditPermissions;
			MyViewDataClass.GenerateView();
		}

		protected void SetupList()
		{
			//GADataRecord owner = SessionManagement.GetCurrentDataContext().SubContextRecord;
			MyListClassDataRecords = new GASystem.GAControls.ListData.ListClassDataRecords(myDataClass, myOwner);
			MyListClassDataRecords.UserHasCreatePermission = hasCreatePermissions;
			MyListClassDataRecords.UserHasEditPermission = hasEditPermissions;
			this.PlaceHolderMain.Controls.Add(MyListClassDataRecords);
		}

		protected void SetupListAllWithinRecord() 
		{
			//GADataRecord owner = SessionManagement.GetCurrentDataContext().SubContextRecord;
			showAllRecordsWithin = true;
			MyListClassAllWithinDataRecords = new GASystem.GAControls.ListData.ListClassAllWithinDataRecords(myDataClass, myOwner);
			MyListClassAllWithinDataRecords.UserHasEditPermission = hasEditPermissions;
			MyListClassAllWithinDataRecords.UserHasCreatePermission = hasCreatePermissions;
			this.PlaceHolderMain.Controls.Add(MyListClassAllWithinDataRecords);
		}
		
		protected void SetupEditForm(int RowId)
		{
			//GADataRecord owner = SessionManagement.GetCurrentDataContext().SubContextRecord;   //TODO, change this to get from querystring or similar
			
			bool hasCreateOrEditPermissions = false;
			if (RowId == 0) 
				hasCreateOrEditPermissions = Security.HasCreatePermission(myDataClass, myOwner);
			else
				//TODO check whether this permission check is valid; should it check for arc?
				hasCreateOrEditPermissions = Security.HasUpdatePermission(new GADataRecord(RowId, myDataClass)) && Security.HasEditPermissionOnRecord(new GADataRecord(RowId, myDataClass));
			
			if (!hasCreateOrEditPermissions)
				throw new GAExceptions.GASecurityException(GASystem.AppUtils.Localization.GetErrorText("Security.NoAEditccess"));

			//TODO use myDataRecord. Extend this datarecord to support new record
			MyFormDetails = GASystem.GAControls.EditForm.GADetailsFormFactory.Make(new GADataRecord(RowId, myDataClass), createMultipleRecords);


			MyFormDetails.OwnerRecord = myOwner; // owner;
			//MyFormDetails = new GASystem.GAControls.EditForm.GeneralDetailsForm(new GADataRecord(PersonnelRowId, GADataClass.GAPersonnel));
			PlaceHolderMain.Controls.Add(MyFormDetails);
			
			if (RowId == 0) 
				SetContextInfoNewRecord();
			else
				SetContextInfo(true);
		}

	
		override protected void OnInit(EventArgs e)
		{
			try 
			{
				DataContextInfoControl = (DataContextInfo)this.Page.LoadControl("~/gagui/UserControls/DataContextInfo.ascx");
			//	MyViewDataClass = (ViewDataClass)this.Page.LoadControl("~/gagui/GAControls/ViewDataClass.ascx");
				PlaceHolderMain = new System.Web.UI.WebControls.PlaceHolder();
				this.Controls.Add(DataContextInfoControl);
				//TODO add a extra menu control here
				this.Controls.Add(PlaceHolderMain);
			//	this.Controls.Add(MyViewDataClass);
			
				base.OnInit(e);
			}
			catch (Exception ex) 
			{
				throw ex;
			}
		}
		
		public bool HasEditPermissions 
		{
			set {hasEditPermissions = value;}
			get {return hasEditPermissions;}
		}

	
		
		/// <summary>
		/// Checks whether the GADataClass for this element is top level.
		/// </summary>
		private bool IsTopLevelClass 
		{
			get 
			{
				return myClassDescription.IsTop;
			}
		}


	}
}
