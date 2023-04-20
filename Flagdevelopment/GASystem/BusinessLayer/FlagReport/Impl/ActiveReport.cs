using System;
using GASystem.BusinessLayer;
using GASystem.BusinessLayer.FlagReport;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Export.Pdf;
using DataDynamics.ActiveReports.Export.Xls;

using GASystem.DataModel;


namespace GASystem.BusinessLayer.FlagReport.Impl
{
	/// <summary>
	/// Summary description for ActiveReport.
	/// </summary>
	public class ActiveReport : AbstractFlagReport
	{
		private GADataRecord _ownerRecord;
		private bool IsDataRecordTopLevelClass = false;
		private ActiveReport3 rpt;

		
		
		public ActiveReport(ReportDescripton rd, string TempPath) : base(rd, TempPath) {}

		public ActiveReport(ReportDescripton rd, string TempPath, GADataRecord DataRecord) : base(rd, TempPath)
		{
			this.DataRecord = DataRecord;
		}

		public override string createReport()
		{
			/*
			For multirecord reports are DataRecord the gadatarecord for the report definition. The DataRecord we would like to build
			the report on is the ownerrecord of the DataRecord
			*/
			
			rpt = new ActiveReport3();
			rpt.LoadLayout(ReportUtils.ActiveFormsRepository + ReportDesc.TemplateFile); 
			_ownerRecord = GASystem.BusinessLayer.DataClassRelations.GetOwner(this.DataRecord);

			
			rpt.DataSource = DataAccess.ReportView.GetRecordSetAllDetailsByDataRecord(_ownerRecord);  
			rpt.DataMember = _ownerRecord.DataClass.ToString();
			
			setReportParameters(rpt, _ownerRecord, base.ReportDesc.ReportInstanceId);

			GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(_ownerRecord.DataClass);
				
			if (cd != null)
				IsDataRecordTopLevelClass = cd.IsTop;

			//get datarange
			AppUtils.DateRangeGenerator.AbstractDateRange dateRange = BusinessLayer.ReportInstance.GetDateRange(ReportDesc.ReportInstanceId);



			// add subreports

			//todo find all subreports !!!
			if (rpt.Sections["detail"] != null) {

				foreach(ARControl control in rpt.Sections["detail"].Controls) 
				{
					if (control.GetType() == typeof(SubReport)) 
					{
						SubReport rptSubCtl = (SubReport)control;
						DataDynamics.ActiveReports.ActiveReport3 rptSub = new DataDynamics.ActiveReports.ActiveReport3(); 
						rptSub.LoadLayout(ReportUtils.ActiveFormsRepository + rptSubCtl.ReportName + ".rpx");  
						
						GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataRecord.ParseGADataClass(rptSub.DataMember));
						System.Data.DataSet ds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(_ownerRecord, dateRange.GetDateFrom(), dateRange.GetDateTo());// BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(myDataClass, myOwner, filter);//rb.FindAllMembers(myOwner, myDataClass);
			
						ds.Tables[0].DefaultView.Sort = GASystem.BusinessLayer.FieldDefinition.GetSortOrderDefinitionForGADataClass(GADataRecord.ParseGADataClass(rptSub.DataMember));
						//System.Data.DataSet ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(  GetRecordSetAllDetailsForDataClassByOwner(GADataRecord.ParseGADataClass(rptSub.DataMember), _ownerRecord);
						patchHTMLData(ds, GADataRecord.ParseGADataClass(rptSub.DataMember));
						//ds.Tables[0].DefaultView.
						rptSub.DataSource = ds.Tables[0];  //.DefaultView;
						//rptSub.DataMember = rptSubCtl.ReportName;
						setReportParameters(rptSub, _ownerRecord, base.ReportDesc.ReportInstanceId);
						rptSubCtl.Report = rptSub;
					}
				}
			}

			rpt.Run();

			string fileName = this.generateFileName(this.TempPath);
			
			if (this.ExportType == DocumentType.Excel) 
			{
				XlsExport xls = new XlsExport();
				xls.RemoveVerticalSpace = true;
				xls.UseCellMerging = true;
				xls.Export(rpt.Document, fileName);
			} 
			else 
			{
				PdfExport pdf = new PdfExport();
				pdf.Export(rpt.Document, fileName);
			}
			return fileName;
		}

		

		/// <summary>
		/// Find and set all parameters for report
		/// </summary>
		/// <param name="reportDef"></param>
		/// <param name="DataRecord"></param>
		/// <param name="ReportInstance"></param>
		private void setReportParameters(DataDynamics.ActiveReports.ActiveReport3 reportDef, GADataRecord DataRecord, int ReportInstance) 
		{
			foreach (DataDynamics.ActiveReports.Parameter param in reportDef.Parameters) 
			{
				param.Value = ParameterFactory.Make(param.Key, DataRecord, ReportInstance).GetValue().ToString();
			}
		}



	}
}
