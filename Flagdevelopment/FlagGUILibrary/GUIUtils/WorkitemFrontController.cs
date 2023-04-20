using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.GAControls;
using GASystem;
using GASystem.AppUtils;

namespace GASystem.GAGUI.GUIUtils
{
	/// <summary>
	/// GA Front controller. Parses querystring and displays the appropriate usercontrols.
	/// </summary>
	
	public class WorkitemFrontController : System.Web.UI.WebControls.WebControl
	{
		protected GASystem.GAControls.EditForm.AbstractDetailsForm MyFormDetails;
		protected GASystem.GAControls.ListData.ListClassDataRecords MyListClassDataRecords;
        
		protected GASystem.GAControls.ListData.ListClassAllWithinDataRecords MyListClassAllWithinDataRecords;
		protected GASystem.GAControls.ViewForm.ViewWorkItem myViewWorkItem;

		protected GASystem.DataContextInfo DataContextInfoControl;
		//protected GASystem.GAControls.ViewDataClass MyViewDataClass;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolderMain;
		
		protected GADataClass myDataClass;
		protected GADataRecord myOwner;
		protected GADataRecord myDataRecord = null;
		private bool showAllRecordsWithin = false;
		private bool filterByCurrentUser = true;
		
		protected bool hasEditPermissions = true;    //changed to true by JOF see comment in onload event
		protected bool hasCreatePermissions = true;   //changed to true by JOF see comment in onload event
        private bool hasEditPermissionsOnRecord = false;

		protected GASystem.GUIUtils.QuerystringUtils myQueryString;

		protected GASystem.AppUtils.ClassDescription myClassDescription;

		private bool createMultipleRecords = false;
		int  RowId = -1;
		
		public WorkitemFrontController(GADataClass DataClass)
		{
			myDataClass = DataClass;
			myClassDescription = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataClass);
			// TODO: Add constructor logic here
		}
		
		protected override void OnLoad(EventArgs e)
		{		
			//check for edit permissions
			//hasEditPermissions = utils.Security.HasEditPermissions(this.ModuleId);
			myQueryString = new GASystem.GUIUtils.QuerystringUtils(myDataClass, this.Page.Request);
			
			RowId = myQueryString.GetRowId();
			myOwner = GASystem.AppUtils.GUIQueryString.GetOwner(this.Page.Request);
			filterByCurrentUser = myQueryString.FilterWorkitemByCurrentUser;
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
			bool editRecord =  myQueryString.EditRecord;  //  && hasEditPermissions;
			bool newRecord = (RowId == 0);   // && hasCreatePermissions;

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

                hasEditPermissions = Security.HasUpdatePermission(new GADataRecord(RowId, myDataClass));
                hasEditPermissionsOnRecord = Security.HasEditPermissionOnRecord(new GADataRecord(RowId, myDataClass));
					
                if (editRecord) 
				{
					SetupEditForm(RowId);
					DoUserAction("EDIT");
				} 
				else
				{                   
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
				hasEditPermissions = Security.HasUpdatePermissionInContext(myDataClass, null); 
				hasCreatePermissions =  Security.HasCreatePermission(myDataClass, null);
				
				myOwner = null;  //top level
				//SessionManagement.SetCurrentSubContext(null);
				SetupList();
				DoUserAction("LIST");
			} 
			else 
			{
				if (myOwner == null)     //no owner specified in querystring, show list of all records within home context based on home context
				{
					myOwner = SessionManagement.GetCurrentDataContext().InitialContextRecord;
					
					//changed 10.01.2007 JOF. listing all records within, can not show edit and create permission based on top level permission
					hasEditPermissions = false; //   Security.HasUpdatePermissionInContext(myDataClass, null); 
					hasCreatePermissions = false; // Security.HasCreatePermission(myDataClass, null);

					//SessionManagement.SetCurrentSubContext(SessionManagement.GetCurrentDataContext().InitialContextRecord);
					//DoUserAction("LISTALL");
					if (filterByCurrentUser)
						SetupListCurrentUser();
					else
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
				myViewWorkItem.Visible = true;
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
                DataContextInfoControl.ContextDataRecord = myOwner;

            //owners of workitem are actions, we would like to display the actions owner as pwner when viewing info
                
            DataContextInfoControl.OwnerDataRecord = DataClassRelations.GetOwner(myOwner);
            DataContextInfoControl.CurrentIsSingleRecord = SingleRecord;
			DataContextInfoControl.ViewAllRecordsWithin = showAllRecordsWithin; //  !IsTopLevelClass && (myOwner == null);
			DataContextInfoControl.SetupContextInfo();
			DataContextInfoControl.NewRecord = false;
		}
		
		private void SetContextInfoNewRecord()
		{
			SetContextInfo(true);

            DataContextInfoControl.OwnerDataRecord = myOwner; // SessionManagement.GetCurrentDataContext().SubContextRecord;
			DataContextInfoControl.NewRecord = true;
		}

		protected void SetupMyViewDataClass(int RowId)   //TODO  clean up
		{
			//hasEditPermissions = Security.HasUpdatePermission(new GADataRecord(RowId, myDataClass)) && Security.HasEditPermissionOnRecord(new GADataRecord(RowId, myDataClass));
            ////check if permissions on workitem workitem overrides hierarcial permission
            //if (!hasEditPermissions)
            //    hasEditPermissions = Workitem.HasEditPermission(GASystem.AppUtils.GAUsers.GetUserId(), RowId);
            
            myViewWorkItem.SetupMyViewDataClass(RowId);
            myViewWorkItem.HasEditPermissions = hasEditPermissions;
            myViewWorkItem.HasEditPermissionsOnRecord = hasEditPermissionsOnRecord;

			this.PlaceHolderMain.Controls.Add(myViewWorkItem);
		}

		protected void SetupList()
		{
			//GADataRecord owner = SessionManagement.GetCurrentDataContext().SubContextRecord;
			MyListClassDataRecords = new GASystem.GAControls.ListData.ListClassDataRecords(GADataClass.GAActionWorkitemView, myOwner);
			MyListClassDataRecords.UserHasCreatePermission = hasCreatePermissions;
			MyListClassDataRecords.UserHasEditPermission = hasEditPermissions;
			this.PlaceHolderMain.Controls.Add(MyListClassDataRecords);
		}

		private void SetupListCurrentUser()
		{
			try 
			{
				System.Web.UI.WebControls.Literal userText = new System.Web.UI.WebControls.Literal();
				userText.Text = "<br/>" +  AppUtils.Localization.GetGuiElementText("WorkitemsAssignedToYou") + "<br/>"; //"<br/>Workitems assigned to you<br/>";
				this.PlaceHolderMain.Controls.Add(userText);
//				myOwner = new GADataRecord(1, GADataClass.GAFlag); //set owner to top node, list all workitems
				//GADataRecord owner = SessionManagement.GetCurrentDataContext().SubContextRecord;
				GASystem.GAControls.ListData.ListWorkitemDataRecords MyListWorkitemDataRecords = new GASystem.GAControls.ListData.ListWorkitemDataRecords(null, System.Web.HttpContext.Current.User.Identity.Name);
				MyListWorkitemDataRecords.UserHasEditPermission = false; //must go via view
				this.PlaceHolderMain.Controls.Add(MyListWorkitemDataRecords);			
			} 
			catch (Exception ex)
			{
				//MyListClassDataRecords.Visible = false;
				System.Web.UI.WebControls.Label userText = new System.Web.UI.WebControls.Label();
				userText.Text = ex.Message;
				userText.CssClass="UserMessageError";
				this.PlaceHolderMain.Controls.Add(userText);			
			}
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
			//GADataRecord owner = SessionManagement.GetCurrentDataContext().SubContextRecord;
			
			bool hasCreateOrEditPermissions = true;   //TODO check against participant
			hasEditPermissions = Security.HasUpdatePermission(new GADataRecord(RowId, myDataClass)) && Security.HasEditPermissionOnRecord(new GADataRecord(RowId, myDataClass));
				
			//check if permissions on workitem workitem overrides hierarcial permission
			if (!hasEditPermissions)
				hasEditPermissions = Workitem.HasEditPermission(GASystem.AppUtils.GAUsers.GetUserId(), RowId);

			//			if (RowId == 0) 
			//				hasCreateOrEditPermissions = Security.HasCreatePermission(myDataClass, owner);
			//			else
			//				hasCreateOrEditPermissions = Security.HasUpdatePermission(new GADataRecord(RowId, myDataClass)) && Security.HasEditPermissionOnRecord(new GADataRecord(RowId, myDataClass));
			//			
						if (!hasCreateOrEditPermissions)
							throw new GAExceptions.GASecurityException(GASystem.AppUtils.Localization.GetErrorText("Security.NoAEditccess"));

			MyFormDetails = GASystem.GAControls.EditForm.GADetailsFormFactory.Make(new GADataRecord(RowId, myDataClass));

			MyFormDetails.OwnerRecord = null;
			//MyFormDetails = new GASystem.GAControls.EditForm.GeneralDetailsForm(new GADataRecord(PersonnelRowId, GADataClass.GAPersonnel));
			PlaceHolderMain.Controls.Add(MyFormDetails);
			
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
				myViewWorkItem = (GASystem.GAControls.ViewForm.ViewWorkItem) this.Page.LoadControl("~/gagui/GAControls/ViewForm/ViewWorkItem.ascx");
			
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
