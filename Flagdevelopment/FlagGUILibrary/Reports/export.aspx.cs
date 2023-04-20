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
using CrystalDecisions.Shared;
using GASystem.BusinessLayer.FlagReport;

namespace GASystem.Reports
{
	/// <summary>
	/// Summary description for crystalreportpdf.
	/// </summary>
	public class crystalreportpdf : System.Web.UI.Page
	{
		private int reportId = 0;
		private int dataRecordRowId = 0;
		private string dataRecordDataClass = "";
		private int reportInstanceId;
        // Tor 201611 Security 20161122 (never referenced) private GASystem.BusinessLayer.GAReport gaReport;
        // Tor 201611 Security 20161122 (never referenced) private string exporttype;

	
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
	
			//create report handler instance
			ReportDescripton rd = ReportUtils.GetReportDescription(reportInstanceId, GASystem.DataModel.GADataRecord.ParseGADataClass(dataRecordDataClass));
			IFlagReport flagReport = ReportFactory.Make(GASystem.BusinessLayer.File.TemporaryPath, rd);
			//flagReport.ExportType = docType;
			flagReport.DataRecord = new GASystem.DataModel.GADataRecord(dataRecordRowId, GASystem.DataModel.GADataRecord.ParseGADataClass(dataRecordDataClass));
			string fileName = flagReport.createReport();

			// HttpContext.Current.Response
			Response.ClearContent();
			Response.ClearHeaders();
			//Response.ContentType = "application/pdf";
			Response.ContentType =  ReportUtils.getMimeType(flagReport.ExportType);
			Response.AddHeader("Content-Disposition", "inline; filename=" + "report" + ReportUtils.getFileExtension(flagReport.ExportType));
			Response.WriteFile(fileName);
			Response.Flush();
			
			//delete temporary file
			try 
			{
				System.IO.File.Delete(fileName);
			} 
			catch (System.Exception ex) 
			{
				//ignore file delete if it fails
				//TODO: log errormessage
			}

			Response.End();


		}

		
		/// <summary>
		/// Generates a uniqe filename for the temporary report
		/// </summary>
		/// <param name="fullName"></param>
		private string generateFileName() 
		{
			string newName = AppUtils.GAUsers.GetUserId() + "_" + dataRecordDataClass;
			
			//add datetag
			DateTime dateTag = DateTime.Now;
			string dateTagString = dateTag.Ticks.ToString();

				
				//dateTag.Year.ToString() + dateTag.Month.ToString().PadLeft(2, '0') + dateTag.Day.ToString().PadLeft(2, '0') + dateTag.Hour.ToString().PadLeft(2, '0') + dateTag.Minute.ToString().PadLeft(2, '0');
	

			newName = newName.Replace("\\", "_");  //remove backslashes in case username is of domain type;
	
		
			return GASystem.BusinessLayer.File.TemporaryPath + newName + dateTagString + ".tmp";
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
