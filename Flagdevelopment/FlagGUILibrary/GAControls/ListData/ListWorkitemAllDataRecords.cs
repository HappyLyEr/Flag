using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.DataModel;
using GASystem.BusinessLayer;

namespace GASystem.GAControls.ListData
{
	/// <summary>
	/// Summary description for ListClassDataRecords.
	/// </summary>
	public class ListWorkitemAllDataRecords : System.Web.UI.WebControls.WebControl
	{
		private GADataRecord myOwner;
		private GADataClass myDataClass = GADataClass.GAWorkitem;
		//private ListDataRecords myListDataRecords;
		//private WebControls.ListControls.FlagDataGrid myListDataRecords;
        private ListDataRecords myListDataRecords;
		private bool hasEditPermission = false;  //set editpermission default to false
		private string myUserId;


		
		public ListWorkitemAllDataRecords(GADataRecord Owner, string UserId)
		{
			myOwner = Owner;
			myUserId = UserId;			
		}

		public bool UserHasEditPermission 
		{
			set {hasEditPermission = value;}
			get {return hasEditPermission;}
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			myListDataRecords = (ListDataRecords)this.Page.LoadControl("~/gagui/UserControls/ListDataRecords.ascx");
//			myListDataRecords = new GASystem.WebControls.ListControls.FlagDataGrid();
			myListDataRecords.DataClass = myDataClass.ToString();
           // myListDataRecords.ListIdentifier = "workitemshortlist";
			//myListDataRecords.DisplayNewButton = false;
			myListDataRecords.DisplayEditButton = false;
			myListDataRecords.DisplaySelectButton = false;
            myListDataRecords.DisplayExportToExcelLink = false;
            myListDataRecords.AddColumnToDisplay("Subject");    //limiting display to subject column only
		//	myListDataRecords.AddColumnToDisplay("Subject");
			this.Controls.Add(myListDataRecords);
			

			//	myListDataRecords.NewRecordClicked += new GASystem.GAGUIEvents.GACommandEventHandler(myListDataRecords_NewRecordClicked);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
		//	myListDataRecords.RefreshGrid();
            WorkitemDS wds = Workitem.GetAllWorkitemsByLogonId(myUserId, this.Page.Cache);
            //wds.Merge(Workitem.GetAllWorkitemsForUserRoles(myUserId, this.Page.Cache));
            //wds.GAWorkitem.DefaultView.Sort =  "DispatchTime";   //TODO get this from GAClass
            myListDataRecords.CustomSort = "DispatchTime DESC";
            myListDataRecords.DisplayCancelSortButton = false;
            myListDataRecords.ListIdentifier = "workitemshortlist";
            myListDataRecords.RecordsDataSet = wds;


            //myListDataRecords.RefreshGrid();
            myListDataRecords.RefreshGrid();

		}


		protected override void OnPreRender(EventArgs e)
		{
            if (myListDataRecords.ListUpdated)
            {
                WorkitemDS wds = Workitem.GetAllWorkitemsByLogonId(myUserId, this.Page.Cache);
                wds.GAWorkitem.DefaultView.Sort = "DispatchTime";   //TODO get this from GAClass
                myListDataRecords.RecordsDataSet = wds;
                myListDataRecords.RefreshGrid();

            }
			
			base.OnPreRender (e);
		}

	
	}
}
