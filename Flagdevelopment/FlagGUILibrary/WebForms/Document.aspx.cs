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
using System.IO;

namespace GASystem.WebForms
{
	/// <summary>
	/// Summary description for Document.
	/// </summary>
	public class Document : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			int fileRowId = Convert.ToInt32(Request["filerowid"].ToString());
			
			GASystem.DataModel.GADataClass dataClass = GASystem.DataModel.GADataClass.GAFile;
			if (Request["dataclass"] != null ) {
				try 
				{
					dataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(Request["dataclass"].ToString());
				}
				catch {}
			}

			
			DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(new GADataRecord(fileRowId, dataClass));
				
				
			//GASystem.BusinessLayer.File.GetFileByFileRowId(fileRowId);
			//byte[] buffer = ds.GAFile[0].Content;
			
			//StreamReader sr = File.OpenText(ds.GAFile[0].url);
			//byte[] buffer = (byte[])ds.Tables[0].Rows[0]["Content"];
			Response.ContentType = ds.Tables[0].Rows[0]["Mimetype"].ToString();
			

			//bug fix. If there is no extension on the file it may open incorrectly. use url filname as filename


		

			Response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[0]["url"].ToString());
			//Response.OutputStream.Write(buffer, 0, buffer.Length);
			//sr.fl
			//Response.OutputStream.w
			//Response.OutputStream.Flush();
			Response.WriteFile(GASystem.BusinessLayer.File.URLPath + ds.Tables[0].Rows[0]["url"].ToString());
			Response.End();
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
