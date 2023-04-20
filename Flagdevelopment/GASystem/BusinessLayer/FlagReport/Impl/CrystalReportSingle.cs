using System;
using GASystem.DataModel;
using CrystalDecisions.Shared;
using GASystem.BusinessLayer.FlagReport;


namespace GASystem.BusinessLayer.FlagReport.Impl
{
	/// <summary>
	/// Summary description for CrystalReportSingle.
	/// </summary>
	public class CrystalReportSingle : AbstractFlagReport
	{
		private GASystem.BusinessLayer.GAReport gaReport;
		
		public CrystalReportSingle(ReportDescripton rd, string TempPath) : base(rd, TempPath)
		{
			
		}

		public CrystalReportSingle(ReportDescripton rd, string TempPath, GADataRecord DataRecord) : base(rd, TempPath)
		{
			this.DataRecord = DataRecord;
		}

		public override string createReport()
		{
			gaReport = new GASystem.BusinessLayer.GAReport(ReportDesc.ReportId, ReportUtils.CrystalFormsRepository.ToString());
			//gaReport.LoadReportAttributesFromInstanceValue(reportInstanceId);
            gaReport.TempPath = this.TempPath;
			gaReport.ReportInstanceId = ReportDesc.ReportInstanceId;
			gaReport.DataRecord = this.DataRecord;
			gaReport.Owner = GASystem.BusinessLayer.DataClassRelations.GetOwner(gaReport.DataRecord);
			gaReport.ReportContext = GASystem.BusinessLayer.DataClassRelations.GetOwner(new GASystem.DataModel.GADataRecord(ReportDesc.ReportInstanceId, DataModel.GADataClass.GAReportInstance));
			//CrystalReportViewer1.ReportSource = 
			CrystalDecisions.CrystalReports.Engine.ReportDocument cbsMain = gaReport.GenerateCrystalReportDocument();
		        
         
			//set export type and headers
			string exportFileName = "report." + ReportUtils.getFileExtension(this.ExportType);
			CrystalDecisions.Shared.ExportFormatType crystalExportType = ReportUtils.getCrystalExportFormatType(this.ExportType);
			string mimetype = ReportUtils.getMimeType(this.ExportType);

			


			DiskFileDestinationOptions diskOpt = new DiskFileDestinationOptions();
			string fileName = base.generateFileName(base.TempPath);
			diskOpt.DiskFileName = fileName;


			cbsMain.ExportOptions.DestinationOptions = diskOpt;
			cbsMain.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
			cbsMain.ExportOptions.ExportFormatType = crystalExportType;
			cbsMain.Export();
			cbsMain.Close();

			
			return fileName;

		}

		public override DocumentType ExportType
		{
			get 
			{
				return DocumentType.PDF;    //crystal report does only support pdf type. override any other settings
			}
		}
	}
}
