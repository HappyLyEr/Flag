using System;
using GASystem.DataModel;

namespace GASystem.Reports.Utils
{
	/// <summary>
	/// Summary description for URLGenerator.
	/// </summary>
	public enum ReportExportType {PDF, Excel}
	
	public class URLGenerator
	{
		public URLGenerator()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //public static string GenerateURLForSingleRecordDetails(GADataRecord DataRecord) 
        //{
        //    //"export.aspx?reportid=" + reportId.ToString() + "&reportinstanceid=" + reportInstanceId.ToString() + "&type=excel";
        //    int reportId = 0;
        //    int reportInstanceId = 0;
			
        //    return "~/gagui/reports/crystalreport.aspx?reportid=" + reportId + "&reportinstanceid=" + reportInstanceId + "&rowid=" + DataRecord.RowId + "&dataclass=" + DataRecord.DataClass.ToString();
        //}

		public static string GenerateURLForSingleRecordDetails(GADataRecord DataRecord, ReportExportType ExportType) 
		{
			//"export.aspx?reportid=" + reportId.ToString() + "&reportinstanceid=" + reportInstanceId.ToString() + "&type=excel";
			int reportId = 0;
			int reportInstanceId = 0;
			
			return "~/gagui/reports/export.aspx?reportid=" + reportId + "&reportinstanceid=" + reportInstanceId + "&rowid=" + DataRecord.RowId + "&dataclass=" + DataRecord.DataClass.ToString() + "&type=" + ExportType.ToString();
		}

        public static string GenerateURLForSingleRecordDetailsFormatString(string DataClass, ReportExportType ExportType)
        {
            //"export.aspx?reportid=" + reportId.ToString() + "&reportinstanceid=" + reportInstanceId.ToString() + "&type=excel";
            //int reportId = 0;
            //int reportInstanceId = 0;
            if (DataClass.ToUpper() == GADataClass.GAReportInstance.ToString().ToUpper())
                return "~/gagui/reports/export.aspx?reportinstanceid={0}&rowid={0}&dataclass=" + DataClass + "&type=" + ExportType.ToString();

            return "~/gagui/reports/export.aspx?reportinstanceid=0&rowid={0}&dataclass=" + DataClass + "&type=" + ExportType.ToString();

        }

		public static string GenerateURLForFullReport(GADataRecord ReportIntanceDataRecord, GADataRecord ReportDataRecord) 
		{
			//return "~/gagui/reports/crystalreport.aspx?reportid=" + ReportDataRecord.RowId + "&reportinstanceid=" + ReportIntanceDataRecord.RowId + "&rowid=" + ReportIntanceDataRecord.RowId + "&dataclass=" + ReportIntanceDataRecord.DataClass.ToString();
			return "~/gagui/reports/crystalreport.aspx?reportinstanceid=" + ReportIntanceDataRecord.RowId + "&rowid=" + ReportIntanceDataRecord.RowId + "&dataclass=" + ReportIntanceDataRecord.DataClass.ToString();
		}
		public static string GenerateURLForFullReport(GADataRecord ReportIntanceDataRecord, GADataRecord ReportDataRecord, ReportExportType ExportType) 
		{
			//return "~/gagui/reports/export.aspx?reportid=" + ReportDataRecord.RowId + "&reportinstanceid=" + ReportIntanceDataRecord.RowId + "&rowid=" + ReportIntanceDataRecord.RowId + "&dataclass=" + ReportIntanceDataRecord.DataClass.ToString() + "&type=" + ExportType.ToString();
			return "~/gagui/reports/export.aspx?reportinstanceid=" + ReportIntanceDataRecord.RowId + "&rowid=" + ReportIntanceDataRecord.RowId + "&dataclass=" + ReportIntanceDataRecord.DataClass.ToString() + "&type=" + ExportType.ToString();
		}


        /// <summary>
        /// Format string URL for showing Analysis cube. Need one parameter: analysis rowid
        /// </summary>
        /// <returns></returns>
        public static string GenerateURLFormatStringForAnalysisCube()
        {
            return "~/gagui/webforms/olap.aspx?analysisrowid={0}";
        }

	}
}
