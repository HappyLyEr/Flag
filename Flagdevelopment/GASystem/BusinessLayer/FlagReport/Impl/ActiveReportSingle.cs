using System;
using GASystem.BusinessLayer;
using GASystem.BusinessLayer.FlagReport;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Export.Pdf;
using GASystem.DataModel;
using System.Data;


namespace GASystem.BusinessLayer.FlagReport.Impl
{
	/// <summary>
	/// Summary description for ActiveReport.
	/// </summary>
	public class ActiveReportSingle : AbstractFlagReport
	{
		private GADataRecord _ownerRecord;
		private bool IsDataRecordTopLevelClass = false;
		private ActiveReport3 rpt;

		
		
		public ActiveReportSingle(ReportDescripton rd, string TempPath) : base(rd, TempPath) {}

		public ActiveReportSingle(ReportDescripton rd, string TempPath, GADataRecord DataRecord) : base(rd, TempPath)
		{
			this.DataRecord = DataRecord;
		}

		public override string createReport()
		{
			rpt = new ActiveReport3();
			rpt.LoadLayout(ReportUtils.ActiveFormsRepository + ReportDesc.TemplateFile);
            DataSet dsReport = DataAccess.ReportView.GetRecordSetAllDetailsByDataRecord(DataRecord);
            patchHTMLData(dsReport, DataRecord.DataClass);
            rpt.DataSource = dsReport;
			rpt.DataMember = DataRecord.DataClass.ToString();
			_ownerRecord = GASystem.BusinessLayer.DataClassRelations.GetOwner(this.DataRecord);
			setReportParameters(rpt, DataRecord, base.ReportDesc.ReportInstanceId);

			GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataRecord.DataClass);
				
			if (cd != null)
				IsDataRecordTopLevelClass = cd.IsTop;


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
						System.Data.DataSet ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(GADataRecord.ParseGADataClass(rptSub.DataMember), DataRecord);
						patchHTMLData(ds, GADataRecord.ParseGADataClass(rptSub.DataMember));
						rptSub.DataSource = ds;
						//rptSub.DataMember = rptSubCtl.ReportName;
						setReportParameters(rptSub, DataRecord, base.ReportDesc.ReportInstanceId);
						rptSubCtl.Report = rptSub;

                        //////set default font
                        //foreach (ARControl subcontrol in rptSub.Sections["detail"].Controls)
                        //{
                        //    if (subcontrol.GetType() == typeof(RichTextBox))
                        //    {
                        //        RichTextBox richText = (RichTextBox)subcontrol;
          
                        //        richText.SelectionFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Regular);
                        //        richText.SelectionHangingIndent = (float)2;
                        //    }
                        //}
					}
				}
			}

			rpt.Run();

			string fileName = this.generateFileName(this.TempPath);

            if (ExportType == DocumentType.PDF)
            {
                PdfExport pdf = new PdfExport();
                pdf.Version = PdfVersion.Pdf13;
               
                pdf.Export(rpt.Document, fileName);
            }
            else
            {
                //DEFAULT TO HTML TYPE
                ExportType = DocumentType.HTML;
                DataDynamics.ActiveReports.Export.Html.HtmlExport htmle = new DataDynamics.ActiveReports.Export.Html.HtmlExport();
                //htmle.IncludePageMargins = true;
                
                htmle.Export(rpt.Document, fileName);

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

        //public override DocumentType ExportType
        //{
        //    get 
        //    {
        //        //return DocumentType.PDF;    //active report is only supported for pdf for the time beeing
                
        //    }
        //}

	}
}
