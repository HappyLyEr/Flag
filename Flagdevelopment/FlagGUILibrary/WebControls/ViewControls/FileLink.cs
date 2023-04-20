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
	public class FileLink : System.Web.UI.WebControls.WebControl
	{
		private System.Web.UI.WebControls.HyperLink ALink;
        private System.Web.UI.WebControls.Label aLabel;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			ALink = new HyperLink();
			ALink.Target = "_new";
			ALink.Text = "View Document";
            this.Controls.Add(ALink);
            aLabel = new Label();
            this.Controls.Add(aLabel);
            aLabel.Visible = false;
		}

		public int DocumentRowId 
		{
			set 
			{
				ViewState["documentrowid"] = value;
			}	
			get 
			{
				if (ViewState["documentrowid"]==null) 
					return 0;
				return Convert.ToInt32(ViewState["documentrowid"]);
			}
		}

		public GASystem.DataModel.GADataClass DataClass 
		{
			set 
			{
				ViewState["DataClass"] = value;
			}	
			get 
			{
				return (GASystem.DataModel.GADataClass)ViewState["DataClass"];
			}
		}

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
	

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
            if (string.IsNullOrEmpty(Text))
                ALink.Visible = false;

            //do not display link if class is of type gaanalysis
            if (this.DataClass == GASystem.DataModel.GADataClass.GAAnalysis)
            {
                ALink.Visible = false;
                aLabel.Visible = true;
                aLabel.Text = this.Text;
            }
            
            ALink.NavigateUrl = "~/gagui/webforms/document.aspx?filerowid=" + DocumentRowId + "&dataclass=" + DataClass.ToString();
		}


		
	}
}
