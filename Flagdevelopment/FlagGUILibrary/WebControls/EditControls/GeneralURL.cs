using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.AppUtils;

namespace GASystem.WebControls.EditControls
{
	/// <summary>
	/// Summary description for Numeric.
	/// </summary>
	[ValidationPropertyAttribute("TextValue")]
    public class GeneralURL : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		
		private TextBox txtBoxURL;
        private TextBox txtBoxDesc;
        private Label lblURL;
        private Label lblDesc;
        // Tor 201611 Security 20161122 (never referenced) private CustomValidator custValidator;
        // Tor 201611 Security 20161122 (never referenced) private RequiredFieldValidator valReq;
        // Tor 201611 Security 20161122 (never referenced) private int _numberBase = 1;


		protected override void OnInit(EventArgs e)
		{
            txtBoxURL = new TextBox();
            lblDesc = new Label();
            lblURL = new Label();
            txtBoxDesc = new TextBox();
            lblURL.Text = Localization.GetGuiElementText("URLText") + ":";
            lblDesc.Text = Localization.GetGuiElementText("DescriptionText");
            this.Controls.Add(lblURL);
            this.Controls.Add(txtBoxURL);
            this.Controls.Add(lblDesc);
            this.Controls.Add(txtBoxDesc);

			base.OnInit (e);
		}

     

	
		public string TextValue 
		{
			get {return "<a url=\"" + txtBoxURL.Text + "\">" + txtBoxDesc.Text + "</a>";}
		}

        public string Text
        {
            get 
            {
                string baseUrl = Page.Request.Url.Scheme + "://" + Page.Request.Url.Authority + Page.Request.ApplicationPath.TrimEnd('/') + "/";

                //txtBoxURL.Text = txtBoxURL.Text.Replace(baseUrl, "");
                //string flagURL = System.Configuration.ConfigurationManager.AppSettings.Get("flagbaseurl");
                string flagURL = (new GASystem.AppUtils.FlagSysResource().GetResourceString("flagbaseurl"));
                txtBoxURL.Text = txtBoxURL.Text.Replace(baseUrl, flagURL);
                return "<a target=\"_new\" href=\"" + txtBoxURL.Text + "\">" + txtBoxDesc.Text + "</a>"; //txtBoxURL.Text + "#" + txtBoxDesc.Text; 


            }
            set
            {
                string tmparef = value.Replace("<a url=\"", "");
                tmparef = tmparef.Replace("<a target=\"_new\" href=\"", "");
                tmparef = tmparef.Replace("</a>", "");
                tmparef = tmparef.Replace("\">", "#");
                string[] aref = tmparef.Split("#".ToCharArray(), StringSplitOptions.None);
                if (aref.Length == 2)
                {
                    txtBoxURL.Text = aref[0];
                    txtBoxDesc.Text = aref[1];
                }
                else
                {
                    txtBoxURL.Text = value;
                    txtBoxDesc.Text = value;
                }

               
              

            }

        }

        public bool IsNull
        {
            get
            {

                return false;
            }
        }

        public override Unit Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
                txtBoxURL.Width = value;
                txtBoxDesc.Width = value;
            }
        }
//		public override short TabIndex
//		{
//			get
//			{
//				return TabIndex;
//			}
//			set
//			{
//				TabIndex = value;
//			}
//		}

	}
}
