using System;
using GASystem.BusinessLayer;
using GASystem.BusinessLayer.FlagReport;
using GASystem.DataModel;
using SautinSoft.HtmlToRtf;

namespace GASystem.BusinessLayer.FlagReport.Impl
{
	/// <summary>
	/// Summary description for CrystalSingelReport.
	/// </summary>
	public abstract class AbstractFlagReport : IFlagReport
	{
		private ReportDescripton _rd;
		private GASystem.DataModel.GADataRecord _dataRecord;
		private DocumentType _exportType;
		private string tmpPath; 

		
		
		public AbstractFlagReport(ReportDescripton rd, string TempPath)
		{
			//
			// TODO: Add constructor logic here
			//
			_rd = rd;
			tmpPath = TempPath;
			ExportType = rd.DocumentType;

		}

		public ReportDescripton ReportDesc 
		{
			get {return _rd;}
		}

		public override DocumentType[] getSupportedExportTypes()
		{
			return new DocumentType[] {DocumentType.PDF};
		}

//		public override string createReport()
//		{
//			return null;
//		}

		public string TempPath
		{
			get
			{
				return tmpPath;
			}
			set
			{
				tmpPath = value;
			}
		}

		public override DocumentType ExportType
		{
			get
			{
				return _exportType;
			}
			set
			{
				_exportType = value;
			}
		}

		public override GASystem.DataModel.GADataRecord DataRecord
		{
			get
			{
				return _dataRecord;
			}
			set
			{
				_dataRecord = value;
			}
		}

		protected string generateFileName(string TempPath) 
		{
			string newName = DataRecord.DataClass.ToString() + "_" + DataRecord.RowId.ToString();
			
			//add datetag
			DateTime dateTag = DateTime.Now;
			string dateTagString = dateTag.Ticks.ToString();

		
			return TempPath + newName + dateTagString + ".tmp";
		}

        /// <summary>
        /// Metod for cleaning html data in dataset so that active reports can display it correctly
        /// </summary>
        /// <param name="ds"></param>
        protected void patchHTMLData(System.Data.DataSet ds, GADataClass DataClass)
        {
            if (DataClass == GADataClass.GAMeetingText)
            {
                //try
                //{
                    if (ds.Tables[GADataClass.GAMeetingText.ToString()].Columns.Contains("Text"))
                    {
                        foreach (System.Data.DataRow row in ds.Tables[GADataClass.GAMeetingText.ToString()].Rows)
                        {
                            if (row["Text"] != DBNull.Value)
                            {
                                //commented out d.t test of HTML2RTF
                                
                                //string rowValue = row["Text"].ToString();

                                string rowValue = row["Text"].ToString();
                                



                                //rowValue = rowValue.Replace("\r", string.Empty);
                                //rowValue = rowValue.Replace("<p>\n&nbsp;\n</p>", "<p></p>");
                                //rowValue = rowValue.Replace("<p align=\"left\">\n&nbsp;\n</p>", "<p></p>");

                                ////remove &nbsp; at end of lines
                                //rowValue = rowValue.Replace("&nbsp;\n", string.Empty);

                                ////remove &nbsp;, they are not supported in AR anyway
                                //rowValue = rowValue.Replace("&nbsp;", " ");

                              //  rowValue = rowValue.Replace("<p>", "<p style=\"margin-top: 0px; margin-bottom: 0px; text-align:left; font-family: arial,helvetica,sans-serif; font-size: 10pt;  \">");
                              //  rowValue = rowValue.Replace("<p ", "<p style=\"margin-top: 0px; margin-bottom: 0px; text-align:left; font-family: arial,helvetica,sans-serif; font-size: 10pt;  \" ");
                                //rowValue = rowValue.Replace("<table ", "<table style=\"font-family: arial,helvetica,sans-serif; font-size: 10pt;  \" ");

                                ////try to reduce space before and after ul
                                //rowValue = rowValue.Replace("<p></p>\n<ul>", "<br/><ul>");
                                //rowValue = rowValue.Replace("</ul>\n<p></p>", "</ul><br/>");


                                ////add html start tag and default font size and type

                                //rowValue = "<html><body><font face=\"arial,helvetica,sans-serif\" size=\"2\">" + rowValue + "</font></body></html>";
                                
                                //row["Text"] = rowValue;




                                //Converter htlmrtf = new Converter();
                                //htlmrtf.OutputTextFormat = eOutputTextFormat.Rtf;
                                //htlmrtf.PageSize = ePageSize.A4;
                                //htlmrtf.PreserveImages = 0;
                                //rowValue = htlmrtf.ConvertString(rowValue);


                                //row["Text"] =  rowValue;


                                //temporary patch, use converting of file in order to avoid c++, c# memory bug in sautin

                                //generate temporary file names:

                                if (rowValue.Trim() != string.Empty)
                                {

                                    string convertFileName = generateFileName(this.TempPath) + "mt";

                                    //row["MeetingTextRowId"].ToString() + ;
                                    string outFileName = convertFileName.Replace("tmpmt", "rtf");



                                    Converter htlmrtf = new Converter();
                                    htlmrtf.OutputTextFormat = eOutputTextFormat.Rtf;
                                    htlmrtf.PageSize = ePageSize.A4;
                                    htlmrtf.PreserveImages = 0;
                                    System.IO.File.WriteAllText(convertFileName, rowValue);
                                    htlmrtf.ConvertFile(convertFileName, this.TempPath);
                                    rowValue = System.IO.File.ReadAllText(outFileName);
                                    //rowValue = htlmrtf.ConvertString(rowValue);

                                    try
                                    {
                                        System.IO.File.Delete(outFileName);
                                        System.IO.File.Delete(outFileName);
                                    }
                                    catch (System.IO.IOException ex)
                                    {
                                        //ignore file deletion in this case
                                        //TODO log
                                    }

                                    row["Text"] = rowValue;
                                }
                            }
                        }
                    }
                //}
                //catch (Exception ex)
                //{
                //    //TODO log
                //}
            }
        }
	}
}
