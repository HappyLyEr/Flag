using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace GASystem.WebControls.EditControls
{
	/// <summary>
	/// Summary description for FileContent.
	/// </summary>
	[ValidationPropertyAttribute("TextValue")]
	public class FileContent : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		
		private System.Web.UI.HtmlControls.HtmlInputFile fileControl;
		
		public byte[] Value 
		{
			get
			{
				
				if (this.IsNull)
					return new byte[0];  //no files posted return "empty" byte array

				System.Web.HttpPostedFile httpFile = this.Page.Request.Files[0];
				httpFile.SaveAs("c:\\gafiles\\" + generateFileName(httpFile.FileName));
				System.IO.BufferedStream bf = new System.IO.BufferedStream(httpFile.InputStream);
				byte[] buffer = new byte[bf.Length];  
				bf.Read(buffer,0,buffer.Length);     

				return buffer;
			}
			
		}

		/// <summary>
		/// Takes a fully qualified file name and returns filename part only. 
		/// Replaces space with underscore
		/// TODO: remove all none alphanumeric names
		/// </summary>
		/// <param name="fullName"></param>
		private string generateFileName(string fullName) 
		{
			string newName = fullName;
			// find name if backslash is used
			if (newName.IndexOf("\\") != -1)
				newName = newName.Substring(newName.LastIndexOf("\\"));
			// find name if slash is used
			if (newName.IndexOf("/") != -1)
				newName = newName.Substring(newName.LastIndexOf("/"));

			newName = newName.Replace(" ", "_");

			//add datetag
			DateTime dateTag = DateTime.Now;
			string dateTagString = dateTag.Year.ToString() + dateTag.Month.ToString().PadLeft(2, '0') + dateTag.Day.ToString().PadLeft(2, '0') + dateTag.Hour.ToString().PadLeft(2, '0') + dateTag.Minute.ToString().PadLeft(2, '0');
			
			newName = newName.Replace(" ", "_");
			//check if name as a . extention
			if (newName.LastIndexOf(".") > 0) //checking for greater then 0 in case name starts with a dot
			{
				int indexOfDot = newName.LastIndexOf(".");
				string extention = newName.Substring(indexOfDot);
				newName = newName.Substring(0, indexOfDot) + dateTagString + extention;
			} 
			else 
			{
				//no extention, just add the datetag
				newName = newName + dateTagString;
			}
			
			return newName;
		}

		
		public string TextValue 
		{
			get {return string.Empty;}
		}

		public bool IsNull 
		{
			get 
			{
				if (this.Page.Request.Files.Count == 0 || this.Page.Request.Files[0].FileName == string.Empty  )
					return true;
				return false;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			fileControl = new System.Web.UI.HtmlControls.HtmlInputFile();
			this.Controls.Add(fileControl);
			base.OnInit (e);
		}


	}
}
