using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.AppUtils;
using System.Text;

namespace GASystem.WebControls.ViewControls
{
	/// <summary>
	/// Summary description for DateControl.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:LocalizedLabel runat=server></{0}:LocalizedLabel>")]
	public class YearMonthSpan : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private Label myLabel;
        // Tor 201611 Security 20161122 (never referenced) private short tabIndex = 0;
		private int numberOfMonths = 0;
		//private string displayString = "{0}: {1}  {2}: {3}"


		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public string Text 
		{
			get
			{
				return myLabel.Text;
			}

			set
			{
				myLabel.Text = value;
			}
		}

		

		public int Value 
		{
			get
			{
				return numberOfMonths;
			}

			set
			{
				numberOfMonths = value;
			}
		}

		

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			myLabel = new Label();
			this.Controls.Add(myLabel);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			string displayString = AppUtils.Localization.GetGuiElementText("YearMonthSpanView");
			int nM = 12;
			int years = numberOfMonths / nM;
			int months = numberOfMonths % nM;
			
			displayString = string.Format(displayString, years.ToString(), months.ToString());
			
			myLabel.Text = displayString;

		}

		
	}
}
