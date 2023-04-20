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
using GASystem.BusinessLayer.FlagReport;
using GASystem.GUIUtils;

namespace GASystem.GAControls.EditForm
{
	/// <summary>
	/// Summary description for GeneralDetailsForm.
	/// </summary>
	public class ReportDetailsForm : AbstractDetailsForm
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(GeneralDetailsForm));
		
		public ReportDetailsForm(GADataRecord DataRecord) : base(DataRecord)
		{
			//
			// TODO: Add constructor logic here
			//
			
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
		}

		protected override void EditRecordSave(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			try
			{
				
				
				
				
				BusinessClass bc = RecordsetFactory.Make(DataRecord.DataClass);
				//TODO add validation code here  make a call to bc.validate.
				
				GADataRecord dispatchToRecord = DataRecord;

				if (DataRecord.RowId == 0) 
				{
					//new record, add report data
					//GADataRecord reportOwner = SessionManagement.GetCurrentDataContext().SubContextRecord;
					ReportDescripton rd = ReportUtils.GetReportDescription(0, this.OwnerRecord.DataClass);
					IFlagReport flagReport = ReportFactory.Make(GASystem.BusinessLayer.File.TemporaryPath, rd);
					flagReport.ExportType = DocumentType.PDF;
					flagReport.DataRecord = this.OwnerRecord;
					string fileTempName = flagReport.createReport();



					
					
					
					ReportDS ReportDataSet = (ReportDS)e.CommandDataSetArgument;
//
//					GASystem.BusinessLayer.GAReport gaReport = new GASystem.BusinessLayer.GAReport(0, this.Page.Server.MapPath("~/gagui/Reports/CrystalForms/"));
//					gaReport.ReportInstanceId = 0;
//					gaReport.DataRecord = this.OwnerRecord;
//					gaReport.Owner = GASystem.BusinessLayer.DataClassRelations.GetOwner(gaReport.DataRecord);
//					CrystalDecisions.CrystalReports.Engine.ReportDocument cbsMain = gaReport.GenerateCrystalReportDocument();
//			
//					//CrystalDecisions.Shared.ExportFormatType crystalExportType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
//					//	mimetype =  "application/pdf";
//				
//	

					
					//copy temp file from report to report repository
					DateTime dateTag = DateTime.Now;
					string dateTagString = dateTag.Year.ToString() + dateTag.Month.ToString().PadLeft(2, '0') + dateTag.Day.ToString().PadLeft(2, '0') + dateTag.Hour.ToString().PadLeft(2, '0') + dateTag.Minute.ToString().PadLeft(2, '0') + dateTag.Second.ToString().PadLeft(2, '0');
//			
					
					string fileName = "Meeting" + dateTagString + ".pdf";
//					cbsMain.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, GASystem.BusinessLayer.File.URLPath + fileName);
					System.IO.File.Copy(fileTempName, GASystem.BusinessLayer.File.URLPath + fileName);
					System.IO.File.Delete(fileTempName);

					//set file details in datarecord
					ReportDataSet.GAReport[0].url = fileName;
					ReportDataSet.GAReport[0].MimeType = ReportUtils.getMimeType(DocumentType.PDF); //   "application/pdf";
//					System.IO.MemoryStream s = (System.IO.MemoryStream)cbsMain.ExportToStream(crystalExportType);
//					byte[] buffer = new byte[s.Length];  
//					s.Read(buffer,0,buffer.Length); 
//					ReportDataSet.GAReport[0].Content = buffer;


					DataSet ds = bc.CommitDataSet(ReportDataSet, this.OwnerRecord);
					dispatchToRecord = new GADataRecord((int)ds.Tables[0].Rows[0][bc.DataClass.ToString().Substring(2)+"RowId"], bc.DataClass);
				} 
				else 
				{
					bc.CommitDataSet(e.CommandDataSetArgument);
				}
				PageDispatcher.GotoDataRecordViewPage(this.Page.Response, dispatchToRecord.DataClass, dispatchToRecord.RowId, null);
			}
			catch (GAValidationException gaValEx)
			{
				DisplayUserMessage(gaValEx.Message, UserMessage.UserMessageType.ValidationError);
			}
			catch (GAException gaEx)
			{
				DisplayUserMessage(gaEx.Message, UserMessage.UserMessageType.Error);
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message, ex);
				DisplayUserMessage(Localization.GetExceptionMessage("UnhandledError"), UserMessage.UserMessageType.Error);
			}
		}


		protected override void EditRecordCancel(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			if (DataRecord.RowId != 0)
				PageDispatcher.GotoDataRecordViewPage(this.Page.Response, DataRecord.DataClass, DataRecord.RowId, null);	
			else if (this.OwnerRecord == null)
				PageDispatcher.GotoDataRecordListPage(this.Page.Response, DataRecord.DataClass);
			else
				PageDispatcher.GotoDataRecordViewPage(this.Page.Response, OwnerRecord.DataClass, OwnerRecord.RowId, null); // GADataRecord.ParseGADataClass(DataClass), SessionManagement.GetCurrentDataContext().SubContextRecord); 
			
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
            throw new Exception("The method or operation is not implemented.");
        }
	}
}
