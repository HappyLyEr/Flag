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
	public class ListWorkitemDataRecords : System.Web.UI.WebControls.WebControl
	{
		private GADataRecord myOwner;
		private GADataClass myDataClass = GADataClass.GAWorkitem;
		private ListDataRecords myListDataRecords;
		private bool hasEditPermission = false;  //set editpermission default to false
		private string myUserId;
		
		public ListWorkitemDataRecords(GADataRecord Owner, string UserId)
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
			myListDataRecords.DataClass = myDataClass.ToString();
			myListDataRecords.DisplayNewButton = false;
			myListDataRecords.DisplayEditButton = false;
			myListDataRecords.DisplaySelectButton = false;
            myListDataRecords.DisplayExportToExcelLink = false;
            myListDataRecords.DisplayCancelSortButton = false;
            myListDataRecords.DisplayFilter = true;
			this.Controls.Add(myListDataRecords);                
                
            //GASystem.BusinessLayer.Workitem.GetAllWorkitemsByLogonId(myUserId, this.Page.Cache);
			
			//myListDataRecords.RefreshGrid();

			//myListDataRecords.NewRecordClicked += new GASystem.GAGUIEvents.GACommandEventHandler(myListDataRecords_NewRecordClicked);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

            if (!Page.IsPostBack)
                myListDataRecords.setDefaultFilter();

            string myfilter = myListDataRecords.getFilter();

            myListDataRecords.RecordsDataSet = Workitem.GetAllWorkitemsDataSetByLogonId(myUserId, myfilter);

			myListDataRecords.RefreshGrid();
		}
        
		protected override void OnPreRender(EventArgs e)
		{
            if (myListDataRecords.ListUpdated)
            {
                string myfilter = myListDataRecords.getFilter();

                myListDataRecords.RecordsDataSet = Workitem.GetAllWorkitemsDataSetByLogonId(myUserId, myfilter);
                myListDataRecords.RefreshGrid();
            }

			base.OnPreRender (e);
		}	
	}
}
