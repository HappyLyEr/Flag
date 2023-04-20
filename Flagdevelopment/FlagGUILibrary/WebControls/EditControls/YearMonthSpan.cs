using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.AppUtils;
using System.Text;

namespace GASystem.WebControls.EditControls
{
	/// <summary>
	/// Summary description for DateControl.
	/// </summary>
	[ValidationPropertyAttribute("TextValue")]
	public class YearMonthSpan : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private TextBox txtYear;
		private TextBox txtMonth;
        // Tor 201611 Security 20161122 (never referenced) private short tabIndex = 0;

		public int Value 
		{
			get
			{
				int numberOfMonths = Int32.Parse(txtYear.Text) * 12;
				numberOfMonths += Int32.Parse(txtMonth.Text);
				return numberOfMonths;
			}

			set
			{
				int nM = 12;
				txtYear.Text = ((System.Int32)(value / nM)).ToString();
				txtMonth.Text = ((System.Int32)(value % nM)).ToString();
			}
		}

		public override short TabIndex
		{
			get
			{
				return txtYear.TabIndex;
				//return tabIndex;
			}
			set
			{
				txtYear.TabIndex = value;
				//tabIndex = value;
			}
		}

		/// <summary>
		/// Used for webcontrol asp.net validation. Note will not return the correct value of months worked. Will instead return a 
		/// string concatenated by year and month of the format {year}{month}. If both are numeric, this string will be numeric 
		/// and the test will pass.
		/// </summary>
		public string TextValue   //used for validation on control. Note will not return the correct value
		{
			get {return txtYear.Text + txtMonth.Text;}   
		}

		public bool IsNull 
		{
			get {return (txtYear.Text == string.Empty || txtMonth.Text == string.Empty ); }
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			txtYear = new TextBox();
			txtMonth = new TextBox();

			txtYear.EnableViewState = true;
			txtMonth.EnableViewState = true;
			txtYear.Columns = 3;
			txtMonth.Columns = 3;

			txtYear.Width = System.Web.UI.WebControls.Unit.Pixel(30);
			txtMonth.Width = System.Web.UI.WebControls.Unit.Pixel(30);

			txtYear.Text = "0";
			txtMonth.Text = "0";
			Literal litYear = new Literal();
			Literal litMonth = new Literal();
			litYear.Text = AppUtils.Localization.GetGuiElementText("Year");
			litMonth.Text = AppUtils.Localization.GetGuiElementText("Month");

			this.Controls.Add(litYear);
			this.Controls.Add(txtYear);
			this.Controls.Add(litMonth);
			this.Controls.Add(txtMonth);
			

			
		//	this.Controls.Add(custValidator);
		}

		
	}
}
