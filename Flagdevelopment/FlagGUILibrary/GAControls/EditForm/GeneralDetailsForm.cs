using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.AppUtils;
using GASystem.GAExceptions;
using GASystem.BusinessLayer.Utils;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using log4net;
using GASystem.GUIUtils;

namespace GASystem.GAControls.EditForm
{
	/// <summary>
	/// Summary description for GeneralDetailsForm.
	/// </summary>
	public class GeneralDetailsForm : AbstractDetailsForm
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(GeneralDetailsForm));
		
		public GeneralDetailsForm(GADataRecord DataRecord) : base(DataRecord)
		{
			//
			// TODO: Add constructor logic here
			//
			
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(this.Page.Server.MapPath("~/bin/Log4NetConfig.xml")));
		}

		protected override void EditRecordSave(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
            //JOF 20141019 commented out for simpler error handling
            //try  
            //{
				BusinessClass bc = RecordsetFactory.Make(DataRecord.DataClass);
				//TODO add validation code here  make a call to bc.validate.


                bool saveMultipleRows = e.CommandDataSetArgument.Tables[DataRecord.DataClass.ToString()].Rows.Count > 1;
				
				GADataRecord dispatchToRecord = DataRecord;
                string lastCommand = "update";

				if (DataRecord.RowId == 0) 
				{
					//add data from extra records here?
					
					DataSet ds = bc.CommitDataSet(e.CommandDataSetArgument, this.OwnerRecord);
                    dispatchToRecord = new GADataRecord((int)ds.Tables[DataRecord.DataClass.ToString()].Rows[0][bc.DataClass.ToString().Substring(2) + "RowId"], bc.DataClass);
					lastCommand = "new";
	
				} 
				else 
				{
                    bc.IsWorkflowTriggeredWhenUpdated = myEditDataRecord.IsWorkflowTriggeredWhenUpdated;
					bc.CommitDataSet(e.CommandDataSetArgument);
				}//
				if (saveMultipleRows) 
				{
					PageDispatcher.GotoDataRecordViewPage(this.Page.Response, this.OwnerRecord.DataClass, this.OwnerRecord.RowId, null, dispatchToRecord.DataClass, lastCommand);
				}
				else 
				{
					PageDispatcher.GotoDataRecordViewPage(this.Page.Response, dispatchToRecord.DataClass, dispatchToRecord.RowId, null, lastCommand);
				}
				
				
            //}
            //catch (GAValidationException gaValEx)
            //{
            //    DisplayUserMessage(gaValEx.Message, UserMessage.UserMessageType.ValidationError);
            //}
            //catch (GAException gaEx)
            //{
            //    DisplayUserMessage(gaEx.Message, UserMessage.UserMessageType.Error);
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex.Message, ex);
            //    DisplayUserMessage(Localization.GetExceptionMessage("UnhandledError") + ": " + ex.Message, UserMessage.UserMessageType.Error);
            //    //": " + ex.Message added temporary by JOF for debugging TODO remove it
            //}
		}


		protected override void EditRecordCancel(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			if (DataRecord.RowId != 0)
				PageDispatcher.GotoDataRecordViewPage(this.Page.Response, DataRecord.DataClass, DataRecord.RowId, null);	
			else if (this.OwnerRecord == null)
				PageDispatcher.GotoDataRecordListPage(this.Page.Response, DataRecord.DataClass);
			else
				PageDispatcher.GotoDataRecordViewPage(this.Page.Response, OwnerRecord.DataClass, OwnerRecord.RowId, null, this.DataRecord.DataClass); // GADataRecord.ParseGADataClass(DataClass), SessionManagement.GetCurrentDataContext().SubContextRecord); 
			
		}

        protected override void EditRecordDelete(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
        {
            //GADataRecord owner = DataClassRelations.GetOwner(SessionManagement.GetCurrentDataContext().SubContextRecord);
            
            BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(this.DataRecord.DataClass);
            bc.DeleteRow(this.DataRecord.RowId);
            if (OwnerRecord == null)
                PageDispatcher.GotoDataRecordListPage(this.Page.Response, this.DataRecord.DataClass);
            else
                PageDispatcher.GotoDataRecordViewPage(this.Page.Response, OwnerRecord.DataClass, OwnerRecord.RowId, null, this.DataRecord.DataClass); // GADataRecord.ParseGADataClass(DataClass), SessionManagement.GetCurrentDataContext().SubContextRecord); 
			
        }

		protected override void TemplateSelected(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			String id = e.CommandStringArgument;
			
			GADataRecord record = new GADataRecord(int.Parse(id), GADataClass.GACrisis);
			_logger.Debug("Id of selected template: " + id);
			_logger.Debug("Owner is: " + this.OwnerRecord.DataClass + "-" + this.OwnerRecord.RowId);
			BusinessClass bc = RecordsetFactory.Make(GADataClass.GACrisis);
			DataSet crisis = bc.CopyRecordWithChildren(record, this.OwnerRecord);
			GADataRecord newCrisisRecord = GADataRecord.GetGADataRecordFromFirstRow(crisis, GADataClass.GACrisis);

			PageDispatcher.GotoDataRecordDetailsPage(this.Page.Response, GADataClass.GACrisis, newCrisisRecord.RowId, this.OwnerRecord);

		}



	}
}
