using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace GASystem.WebControls.ViewControls
{
	/// <summary>
	/// Summary description for FileLink.
	/// </summary>
	public class FileLookupURL : System.Web.UI.WebControls.WebControl
	{
		private System.Web.UI.WebControls.HyperLink ALink;
		private string _linkText = string.Empty;
		public int _fileRowId;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			ALink = new HyperLink();
			ALink.Target = "_new";
			
			this.Controls.Add(ALink);
		}

		public int FileRowId
		{
			set 
			{
				_fileRowId = value;
			}	
			get 
			{
				return _fileRowId;
			}
		}

		public string LinkText 
		{
			set 
			{
				_linkText = value;
			}	
			get 
			{
				return _linkText;
			}
		}
		

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			if (FileRowId != -1) 
			{
				ALink.Text = LinkText;
				ALink.NavigateUrl = "~/gagui/webforms/document.aspx?filerowid=" + FileRowId + "&dataclass=GAFile";
			} 
			else 
			{
				ALink.Visible = false;
			}
		}
		
	}
}
