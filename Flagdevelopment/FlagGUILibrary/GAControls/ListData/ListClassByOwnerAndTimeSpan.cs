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
	public class ListClassByOwnerAndTimeSpan : System.Web.UI.WebControls.WebControl
	{
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ListClassByOwnerAndTimeSpan));
        protected Label headerText = new Label();

		private GADataRecord myOwner;
		private GADataClass myDataClass;
        private DateTime myDateFrom;
        private DateTime myDateTo;
        private string myFilter;
            
		private ListDataRecords myListDataRecords;
		private bool hasEditPermission = false;  //set editpermission default to false
		private bool hasCreatePermission = false;


        public ListClassByOwnerAndTimeSpan(GADataClass DataClass, GADataRecord Owner, System.DateTime dateFrom, System.DateTime dateTo, string filter)
		{
			myOwner = Owner;
			myDataClass = DataClass;
            myFilter = filter;
            myDateFrom = dateFrom;
            myDateTo = dateTo;
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

        public string HeaderText
        {
            get { return headerText.Text; }
            set { headerText.Text = value; }
        }

        private bool _containsRecords   = false;

        public bool ContainsRecords
        {
            get { return _containsRecords; }
            set { _containsRecords = value; }
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
			    


				myListDataRecords = (ListDataRecords)this.Page.LoadControl("~/gagui/UserControls/ListDataRecords.ascx");
				
			

               //end test  ***************************************
                myListDataRecords.Owner = myOwner;

				myListDataRecords.DataClass = myDataClass.ToString();

                myListDataRecords.DisplayEditButton = false;
                myListDataRecords.DisplayNewButton = false;
                

				myListDataRecords.DisplaySelectButton = false;
				myListDataRecords.DisplayFilter = false;
                myListDataRecords.DisplayExportToExcelLink = false;
                myListDataRecords.DisplayNewButton = false;
                headerText.CssClass = "FlagContext_SubCaptionFlagClass";
                this.Controls.Add(headerText);
           


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


		protected override void OnLoad(EventArgs e)
		{
			
			base.OnLoad (e);
           // my

           
		}

        public void GenerateControl() 
        {
             
            System.Data.DataSet records = BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewByOwnerIncludingDropDownDetails(myDataClass, myOwner, myDateFrom, myDateTo ,myFilter);
            myListDataRecords.RecordsDataSet = records;

           

			myListDataRecords.RefreshGrid();


            ContainsRecords = records.Tables[0].Rows.Count > 0;
            this.Visible = ContainsRecords;
        }



		protected override void OnPreRender(EventArgs e)
		{
            if (myListDataRecords.ListUpdated)
            {
                GenerateControl();
    
               // System.Data.DataSet records = BusinessLayer.Utils.RecordsetFactory.GetAllRecordsForGAListViewByOwnerIncludingDropDownDetails(myDataClass, myOwner);
               // myListDataRecords.RecordsDataSet = records;
               // myListDataRecords.RefreshGrid();

           

            }

            

			base.OnPreRender (e);
		}

		private void myListDataRecords_NewRecordClicked(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			PageDispatcher.GotoDataRecordDetailsPage(this.Page.Response, myDataClass, 0, myOwner);
		}

	}
}
