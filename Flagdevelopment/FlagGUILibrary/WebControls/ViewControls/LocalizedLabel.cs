using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace GASystem.WebControls.ViewControls
{
	/// <summary>
	/// Summary description for LocalizedLabel.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:LocalizedLabel runat=server></{0}:LocalizedLabel>")]
	public class LocalizedLabel : System.Web.UI.WebControls.WebControl
	{
		private Label locLabel;
	
		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			locLabel = new Label();
			this.Controls.Add(locLabel);
		}


		[Bindable(true), 
			Category("Appearance"), 
			DefaultValue("")] 
		public string Text 
		{
			get
			{
				return locLabel.Text;
			}

			set
			{
				locLabel.Text = GASystem.AppUtils.Localization.GetGuiElementText(value);
			}
		}

		
	}
}
