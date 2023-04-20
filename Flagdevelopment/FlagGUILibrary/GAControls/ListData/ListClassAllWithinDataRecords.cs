using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.AppUtils;
using GASystem.GAExceptions;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using GASystem.GAGUIEvents;
using log4net;
using log4net.Appender;
using log4net.Config;
using GASystem.GUIUtils;

namespace GASystem.GAControls.ListData
{
	/// <summary>
	/// Summary description for ListClassDataRecords.
	/// </summary>
	public class ListClassAllWithinDataRecords : System.Web.UI.WebControls.WebControl
	{
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ListClassAllWithinDataRecords));

		private GADataRecord myOwner;
		private GADataClass myDataClass;
		private ListDataRecords myListDataRecords;
		private bool hasEditPermission = false;  //set editpermission default to false
		private bool hasCreatePermission = false;
//		private GASystem.GUIUtils.DataFilter.FilterBuilder myFilterBuilder;

		
		public ListClassAllWithinDataRecords(GADataClass DataClass, GADataRecord Owner)
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
			base.OnInit (e);
			XmlConfigurator.Configure(new System.IO.FileInfo(this.Page.Server.MapPath("~/bin/Log4NetConfig.xml")));
			try
			{
//				myFilterBuilder = new GASystem.GUIUtils.DataFilter.FilterBuilder(myDataClass);
//				this.Controls.Add(myFilterBuilder.FilterControl);
//				myFilterBuilder.FilterControl.SetFilterClicked += new EventHandler(FilterControl_SetFilterClicked);
//				
				
				myListDataRecords = (ListDataRecords)this.Page.LoadControl("~/gagui/UserControls/ListDataRecords.ascx");
				
				//ReportBuilder rb = new ReportBuilder();
				
//				if (!this.Page.IsPostBack)
//					myFilterBuilder.SetDefaultFilter();

		//		string filter = myFilterBuilder.GetFilterString();
				string filter = string.Empty;


                //end test  ***************************************
                myListDataRecords.Owner = myOwner;
                myListDataRecords.BaseURL = GASystem.GUIUtils.LinkUtils.GenerateURLForListAll(myDataClass.ToString());
				myListDataRecords.DataClass = myDataClass.ToString();
				myListDataRecords.DisplayNewButton = false; //not allowed create in "AllWithin"
                myListDataRecords.DisplayEditButton = hasEditPermission;
				myListDataRecords.DisplaySelectButton = false;
				myListDataRecords.DisplayFilter = true;
				this.Controls.Add(myListDataRecords);

             


				//myListDataRecords.RefreshGrid();
//				myListDataRecords.NewRecordClicked += new GACommandEventHandler(myListDataRecords_NewRecordClicked);
			}
			catch (GAException gaEx)
			{
				DisplayUserMessageError(gaEx.Message);	
			}
			catch(Exception ex)
			{
				_logger.Error(ex.Message, ex);
				DisplayUserMessageError(Localization.GetExceptionMessage("UnhandledError") + ": " + ex.Message);
			}
		}

//		private string getFilter() 
//		{
//			string filter = string.Empty;
//			if (Page.Request["refclass"] != null) 
//			{
//				filter += Page.Request["reffield"].ToString() + " = " + Page.Request["refclassrowid"];
//			}
//			return filter;
//		}

		protected override void OnLoad(EventArgs e)
		{
			
			base.OnLoad (e);
           // my


            if (!Page.IsPostBack)
                myListDataRecords.setDefaultFilter();

            string myfilter = myListDataRecords.getFilter();
        //    BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(myDataClass);
            System.Data.DataSet records = BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(myDataClass, myOwner, System.DateTime.MinValue, System.DateTime.MaxValue, myfilter);
            myListDataRecords.RecordsDataSet = records;


			myListDataRecords.RefreshGrid();
		}


		protected override void OnPreRender(EventArgs e)
		{
            if (myListDataRecords.ListUpdated)
            {

                //if (!Page.IsPostBack)
                //    myListDataRecords.SetDefaultFilter();

                string myfilter = myListDataRecords.getFilter();
                
            //    BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(myDataClass);
                System.Data.DataSet records = BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(myDataClass, myOwner, System.DateTime.MinValue, System.DateTime.MaxValue, myfilter);
                myListDataRecords.RecordsDataSet = records;
                myListDataRecords.RefreshGrid();
            }

//			myListDataRecords.RecordsDataSet.Tables[0].DefaultView.RowFilter = myFilterBuilder.GetFilterString();
//			myListDataRecords.RefreshGrid();
			base.OnPreRender (e);
		}

		private void myListDataRecords_NewRecordClicked(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			PageDispatcher.GotoDataRecordDetailsPage(this.Page.Response, myDataClass, 0, myOwner);
		}

//		private void FilterControl_SetFilterClicked(object sender, EventArgs e)
//		{
//			myListDataRecords.CurrentPage = 0;
//		}
	}
}
