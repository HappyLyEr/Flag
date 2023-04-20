using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using log4net;

namespace GASystem.GAControls.EditForm
{
	/// <summary>
	/// Summary description for ReportInstanceDetailsFrom.
	/// </summary>
	public class ReportInstanceDetailsFrom : GeneralDetailsForm
	{

		private ListDataRecords myListDataRecords;

		public ReportInstanceDetailsFrom(GADataRecord DataRecord) : base(DataRecord)
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected override void OnInit(EventArgs e)
		{
			//base.OnInit (e);
		
			myEditDataRecord = GetEditFormControl();
			
			//handle events from editdatarecord
//			myEditDataRecord.EditRecordSave += new GASystem.GAGUIEvents.GACommandEventHandler(EditRecordSave);
//			myEditDataRecord.EditRecordCancel += new GASystem.GAGUIEvents.GACommandEventHandler(EditRecordCancel);
			

			this.Controls.Add(PlaceHolderTop);
			this.Controls.Add(myEditDataRecord);
		//	myEditDataRecord.Visible = false;
			if (DataRecord.RowId == 0)   //the full list of records must always be generated when rowid = 0, incuding for pages where it is not displayed in order to support postback. (we are not using viewstate on this control)
			{
				myListDataRecords = (ListDataRecords)this.Page.LoadControl("~/gagui/UserControls/ListDataRecords.ascx");
				myListDataRecords.ID = "reportslist";
			
				this.Controls.Add(myListDataRecords);
				myListDataRecords.DataClass = GADataClass.GAReports.ToString();
				myListDataRecords.DisplayNewButton = false;;
				myListDataRecords.DisplayEditButton = false;
				myListDataRecords.DisplaySelectButton = false;
				myListDataRecords.DisplaySelectPostBackButton = true;
				myListDataRecords.DisplayFilter = true;
				//myListDataRecords.RecordsDataSet = BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassWithinOwner(GADataClass.GAReports, null, string.Empty);
				myListDataRecords.RecordsDataSet = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(GADataClass.GAReports, null, string.Empty);
			
//				myListDataRecords.SelectRecordClicked += new GASystem.GAGUIEvents.GACommandEventHandler(myListDataRecords_SelectRecordClicked);
				myListDataRecords.RefreshGrid();
				if (this.Page.IsPostBack) //list of reports are never displayed after a postback, 
				{
					myListDataRecords.Visible = false;
					myEditDataRecord.Visible = true;
				} else
					myEditDataRecord.Visible = false;
			}
	
			if (this.Page.IsPostBack) //edit form is always display after a postback
			{
				myEditDataRecord.Visible = true;
			//	if (myEditDataRecord.RecordDataSet != null)
			//		myEditDataRecord.SetupForm();
			}

			if (!this.Page.IsPostBack && DataRecord.RowId != 0) //display form if it is edit only
			{
				SetupEditForm() ;
				myEditDataRecord.Visible = true;
			}
		}

		private void SetupEditForm() 
		{
			//this.Controls.Add(myEditDataRecord);
			myEditDataRecord.DataClass = DataRecord.DataClass.ToString();
			
			if (DataRecord.RowId == 0) 
			{
				//newrecord
					
				//myEditDataRecord.RecordDataSet = bc.GetNewRecord();
	
			}
			else 
			{
				//myEditDataRecord.RecordDataSet = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(myDataRecord);
				GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(DataRecord.DataClass);		
				myEditDataRecord.RecordDataSet = bc.GetByRowId(DataRecord.RowId);
			}

			myEditDataRecord.SetupForm();
			myEditDataRecord.Visible = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			if (!this.Page.IsPostBack && DataRecord.RowId == 0) 
				myListDataRecords.RefreshGrid();
		}

		private void myListDataRecords_SelectRecordClicked(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			
			GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAReportInstance);
			myEditDataRecord.DataClass = DataRecord.DataClass.ToString();
			ReportInstanceDS ds = (ReportInstanceDS)bc.GetNewRecord();
			ds.GAReportInstance[0].ReportId = e.CommandIntArgument;
			myEditDataRecord.RecordDataSet = ds;
			SetupEditForm();
		}

		protected override void EditRecordCancel(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			if (DataRecord.RowId != 0)
				AppUtils.PageDispatcher.GotoDataRecordViewPage(this.Page.Response, DataRecord.DataClass, DataRecord.RowId, null);	
			else if (this.OwnerRecord == null)
				GASystem.AppUtils.PageDispatcher.GotoDataRecordListPage(this.Page.Response, DataRecord.DataClass);
			else
				GASystem.AppUtils.PageDispatcher.GotoDataRecordViewPage(this.Page.Response, OwnerRecord.DataClass, OwnerRecord.RowId, null, this.DataRecord.DataClass); // GADataRecord.ParseGADataClass(DataClass), SessionManagement.GetCurrentDataContext().SubContextRecord); 
			
		}

		protected override void EditRecordSave(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			base.EditRecordSave(sender, e);
		}


	}
}
