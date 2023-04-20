using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.AppUtils;
using GASystem.GAExceptions;
using GASystem.BusinessLayer.Utils;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using GASystem.GAGUIEvents;
using log4net;
using GASystem.GUIUtils;

namespace GASystem.GAControls.ListData
{
	/// <summary>
	/// Summary description for ListClassDataRecords.
	/// </summary>
	public class ListClassDataRecords : System.Web.UI.WebControls.WebControl
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(ListClassDataRecords));

		private GADataRecord myOwner;
		private GADataClass myDataClass;
		private ListDataRecords myListDataRecords;
		private bool hasEditPermission = false;  //set editpermission default to false
		private bool hasCreatePermission = false;  //set editpermission default to false
        // Tor 201611 Security 20161122 (never referenced) private GASystem.GUIUtils.DataFilter.FilterBuilder myFilterBuilder;


		
		public ListClassDataRecords(GADataClass DataClass, GADataRecord Owner)
		{
			myOwner = Owner;
			myDataClass = DataClass;
		}

		public bool UserHasEditPermission 
		{
			set {hasEditPermission = value;}
			get {return hasEditPermission;}
		}

		public bool UserHasCreatePermission 
		{
			set {hasCreatePermission = value;}
			get {return hasCreatePermission;}
		}

		private void DisplayUserMessageError(string Message)
		{
			try
			{
				UserMessage userMessageControl = (UserMessage) this.Page.LoadControl("~/gagui/UserControls/UserMessage.ascx");
				userMessageControl.MessageType = UserMessage.UserMessageType.Error;
				userMessageControl.MessageText = Message;
				this.Controls.Add(userMessageControl);
			}
			catch (Exception e)
			{
				_logger.Error(Localization.GetExceptionMessage("ErrorDisplayUserMessage"), e);
			}
		}

		protected override void OnInit(EventArgs e)
		{
			try
			{
//				base.OnInit (e);
//				myFilterBuilder = new GASystem.GUIUtils.DataFilter.FilterBuilder(myDataClass);
//				this.Controls.Add(myFilterBuilder.FilterControl);
//				myFilterBuilder.FilterControl.SetFilterClicked += new EventHandler(FilterControl_SetFilterClicked);
//				
				
				myListDataRecords = (ListDataRecords)this.Page.LoadControl("~/gagui/UserControls/ListDataRecords.ascx");
				myListDataRecords.DataClass = myDataClass.ToString();
				myListDataRecords.DisplayNewButton = hasCreatePermission;
				myListDataRecords.DisplayEditButton = hasEditPermission;
				myListDataRecords.DisplaySelectButton = false;
				myListDataRecords.DisplayFilter = true;
				myListDataRecords.Owner = myOwner;
			
				this.Controls.Add(myListDataRecords);
				
//				if (!this.Page.IsPostBack)
//					myFilterBuilder.SetDefaultFilter();


				myListDataRecords.DataClass = myDataClass.ToString();
				myListDataRecords.DisplayNewButton = hasCreatePermission;
				myListDataRecords.DisplayEditButton = hasEditPermission;
				myListDataRecords.DisplaySelectButton = false;
			
				//myListDataRecords.SortColumn = 

//				myListDataRecords.NewRecordClicked += new GACommandEventHandler(myListDataRecords_NewRecordClicked);
			}
			catch (GAException gaEx)
			{
				DisplayUserMessageError(gaEx.Message);	
			}
			catch(Exception ex)
			{
				_logger.Error(ex.Message, ex);
				DisplayUserMessageError(Localization.GetExceptionMessage("UnhandledError"));
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			

            if (!this.Page.IsPostBack)
            {
                myListDataRecords.setDefaultFilter();


            }
            string myFilter = myListDataRecords.getFilter();
            if (!this.Page.IsPostBack || myDataClass == GADataClass.GAWorkitem)
            {
                myListDataRecords.RecordsDataSet = RecordsetFactory.GetAllRecordsForGAListViewByOwnerIncludingDropDownDetails(myDataClass, myOwner, myFilter);
            }
            myListDataRecords.RefreshGrid();
            //}

		}


		protected override void OnPreRender(EventArgs e)
		{
//			myListDataRecords.RecordsDataSet.Tables[0].DefaultView.RowFilter = myFilterBuilder.GetFilterString();
//			myListDataRecords.RefreshGrid();
//			
			base.OnPreRender (e);

            if (myListDataRecords.ListUpdated)
            {
                string myFilter = myListDataRecords.getFilter();
                myListDataRecords.RecordsDataSet = RecordsetFactory.GetAllRecordsForGAListViewByOwnerIncludingDropDownDetails(myDataClass, myOwner, myFilter);
                myListDataRecords.RefreshGrid();	

            }
		}

		private void myListDataRecords_NewRecordClicked(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			PageDispatcher.GotoDataRecordDetailsPage(this.Page.Response, myDataClass, 0, myOwner);
		}

		
	}
}
