using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GASystem.DataModel;
namespace GASystem.Reports
{
	/// <summary>
	/// Summary description for crystalreport.
	/// </summary>
	public partial class crystalreport : System.Web.UI.Page
	{
		private int reportId;
		private int reportInstanceId;
		private int dataRecordRowId = 0;
		private string dataRecordDataClass = "";
		
		private GASystem.BusinessLayer.GAReport gaReport;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if (Request["ReportId"] != null)
				reportId = Convert.ToInt32(Request["ReportId"]);
			if (Request["rowid"] != null)
				dataRecordRowId = Convert.ToInt32(Request["rowid"]);
			if (Request["dataclass"] != null)
				dataRecordDataClass = Request["dataclass"].ToString();
			

			reportInstanceId = Convert.ToInt32(Request["reportinstanceid"]);


			//set export links
			
			//excelLink.NavigateUrl = "export.aspx?reportid=" + reportId.ToString() + "&reportinstanceid=" + reportInstanceId.ToString() + "&type=excel";
			//pdfLink.NavigateUrl = "export.aspx?reportid=" + reportId.ToString() + "&reportinstanceid=" + reportInstanceId.ToString() + "&type=pdf";

			//excelLink.NavigateUrl = Utils.URLGenerator.GenerateURLForSingleRecordDetails(new GASystem.DataModel.GADataRecord(
			

			gaReport = new GASystem.BusinessLayer.GAReport(reportId, Server.MapPath("~/gagui/Reports/CrystalForms/"));
			//gaReport.LoadReportAttributesFromInstanceValue(reportInstanceId);
			gaReport.ReportInstanceId = reportInstanceId;
			gaReport.DataRecord = new GASystem.DataModel.GADataRecord(dataRecordRowId, GASystem.DataModel.GADataRecord.ParseGADataClass(dataRecordDataClass));
			CrystalReportViewer1.ReportSource = gaReport.GenerateCrystalReportDocument();
			
			if (dataRecordDataClass != GASystem.DataModel.GADataClass.GAReportInstance.ToString()) 
			{
				excelLink.NavigateUrl = Utils.URLGenerator.GenerateURLForSingleRecordDetails(gaReport.DataRecord, Utils.ReportExportType.Excel);
				pdfLink.NavigateUrl = Utils.URLGenerator.GenerateURLForSingleRecordDetails(gaReport.DataRecord, Utils.ReportExportType.PDF);
			} 
			else 
			{
				excelLink.NavigateUrl = Utils.URLGenerator.GenerateURLForFullReport(new GADataRecord(reportInstanceId, GADataClass.GAReportInstance), new GADataRecord(reportId, GADataClass.GAReports), Utils.ReportExportType.Excel);
				pdfLink.NavigateUrl = Utils.URLGenerator.GenerateURLForFullReport(new GADataRecord(reportInstanceId, GADataClass.GAReportInstance), new GADataRecord(reportId, GADataClass.GAReports), Utils.ReportExportType.PDF);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
