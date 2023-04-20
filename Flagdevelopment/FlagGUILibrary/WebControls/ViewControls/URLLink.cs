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
	public class URLLink : System.Web.UI.WebControls.WebControl
	{
		private System.Web.UI.WebControls.HyperLink ALink;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			ALink = new HyperLink();
			ALink.Target = "_new";
			ALink.Text = "View Document";
			this.Controls.Add(ALink);
		}

		public string URL 
		{
			set 
			{
				ViewState["URL"] = value;
			}	
			get 
			{
				if (ViewState["URL"]==null) 
					return string.Empty;
				return ViewState["URL"].ToString();
			}
		}

        public string URLText
        {
            set
            {
                ViewState["URLText"] = value;
            }
            get
            {
                if (ViewState["URLText"] == null)
                    return string.Empty;
                return ViewState["URLText"].ToString();
            }
        }
		

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
            if (URL.Contains("@"))
            {
                ALink.NavigateUrl = "mailto:" + URL;
            }
            else
            {
                string baseUrl = Page.Request.Url.Scheme + "://" + Page.Request.Url.Authority + Page.Request.ApplicationPath.TrimEnd('/') + "/";
                if (!URL.StartsWith("http"))
                    ALink.NavigateUrl = baseUrl + URL;
                else
                    ALink.NavigateUrl = URL;
            }
            if (URLText != string.Empty)
                ALink.Text = URLText;
        }
		
	}
}
