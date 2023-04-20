using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace GASystem.WebControls.EditControls
{
	/// <summary>
	/// Summary description for FileMimetype.
	/// </summary>
	[ValidationPropertyAttribute("TextValue")]
	public class FileMimetype : System.Web.UI.WebControls.WebControl
	{
		public enum FileExtensions{doc, xls};
		
		public string Value 
		{
			get
			{
				if (this.Page.Request.Files.Count == 0)
					return string.Empty;  //no files posted return emptystring
				return getContentType(this.Page.Request.Files[0]);
			}
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
			//this control does not need to render any output. 
			//It is only present to return part values for mimetype of posted files
			

			base.OnInit (e);
		}


		/// <summary>
		/// Method for getting the mimetype of the posted file. Finds the mimetype based on the file extension. 
		/// For extensions that are not known it uses the mimetype specified by the client
		/// </summary>
		/// <param name="PostedFile"></param>
		/// <returns></returns>
		private string getContentType(System.Web.HttpPostedFile PostedFile) 
		{
			string fileExtension = string.Empty;
			if (PostedFile.FileName.LastIndexOf(".") > 0) //checking for greater then 0 in case name starts with a dot
			{
				int indexOfDot = PostedFile.FileName.LastIndexOf(".");
				if (PostedFile.FileName.Length - 1 > indexOfDot)   //does file name end with a dot
				{
					fileExtension = PostedFile.FileName.Substring(indexOfDot+1);

					try 
					{
						FileExtensions fileExt = (FileExtensions) Enum.Parse(typeof(FileExtensions), fileExtension);
						switch (fileExt) 
						{
							case FileExtensions.xls :
								return "application/vnd.ms-excel";
							case FileExtensions.doc:
								return "application/msword";
						}

					} 
					catch (ArgumentException ex) 
					{
						return PostedFile.ContentType;
					}	
				}
			} 
			//default
			return PostedFile.ContentType;
		}


	}
}
