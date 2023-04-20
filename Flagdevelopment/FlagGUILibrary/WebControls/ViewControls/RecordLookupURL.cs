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
	public class RecordLookupURL : System.Web.UI.WebControls.WebControl
	{
		private System.Web.UI.WebControls.HyperLink ALink;
        // Tor 20151130 JOF assistance - Blink added not to generate/show hyperlink when lookuptable="GALists"
        private System.Web.UI.WebControls.Label Blink;
        private string _linkText = string.Empty;
		private GASystem.DataModel.GADataRecord _dataRecord = null;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			ALink = new HyperLink();
            // Tor 20151130 JOF assistance - Blink added not to generate/show hyperlink when lookuptable="GALists"
            Blink = new Label();

			this.Controls.Add(ALink);
            // Tor 20151130 JOF assistance - Blink added not to generate/show hyperlink when lookuptable="GALists"
            this.Controls.Add(Blink);

		}

		public GASystem.DataModel.GADataRecord DataRecord 
		{
			set {_dataRecord = value; }
			get {return _dataRecord; }

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
			if (DataRecord != null) 
			{
                // Tor 20151130 JOF assistance - do not generate/show hyperlink when lookuptable="GALists"
                if (DataRecord.DataClass.ToString() == "GALists")
                {
                    Blink.Text = LinkText;
                    ALink.Visible = false;
                }
                else
                {
                    Blink.Visible = false;
                    ALink.Text = LinkText;
                    ALink.NavigateUrl = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordView(DataRecord.DataClass.ToString(), DataRecord.RowId.ToString());
                }
			} 
			else 
			{
				ALink.Visible = false;
			}
		}
		
	}
}
