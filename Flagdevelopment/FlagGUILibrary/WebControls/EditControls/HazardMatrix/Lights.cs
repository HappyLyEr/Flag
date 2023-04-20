using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.ComponentModel;

namespace GASystem.WebControls.EditControls.HazardMatrix
{
	/// <summary>
	/// Summary description for Lights.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:Lights runat=server></{0}:Lights>")]
	public class Lights : System.Web.UI.WebControls.WebControl
	{
		public event EventHandler Expand;
		protected ImageButton img;
		private int _Value = 0;
		protected Label status;
	
		[Bindable(true), DefaultValue(Risk.GREEN)]
		public int Value
		{
			get
			{
				object val = _Value;
				return (val == null) ? (int)Risk.GREEN : (int)_Value;
			}

			set
			{
				_Value = value;
			}
		}



		public Lights()
		{
			DebugWrite("Lights CONSTRUCTOR");
		}

		protected override void OnInit(EventArgs e)
		{
			DebugWrite("OnInit START");
			DebugWrite("Value is " + this.Value);
			//if (this.Visible)
			//{
				if (this.Parent is HazardMatrixControl)
				{
					HazardMatrixControl parent = (HazardMatrixControl)this.Parent;
					parent.ValueChange += new EventHandler(parent_ValueChange);
				}
				CreateLights();
			//}
			DebugWrite("Value is " + this.Value);
			DebugWrite("OnInit END");
		}

		protected void CreateLights()
		{
			DebugWrite("CreateLights START");

			HtmlGenericControl statusDiv = new HtmlGenericControl("div");
			Controls.Add(statusDiv);

			img = new ImageButton();
			img.ImageUrl = getLightImageUrl(this.Value);
			img.ImageAlign = ImageAlign.AbsMiddle;
			img.Style.Add("padding", "4px");
			statusDiv.Controls.Add(img);

			status = new Label();
			status.Text = getLabelForRiskStatus(this.Value);
			statusDiv.Controls.Add(status);

			HtmlGenericControl h = new HtmlGenericControl("div");
			h.Style.Add("text-align", "right");
			Controls.Add(h);

			LinkButton lnk = new LinkButton();
			lnk.Text = "Determine risk";
			lnk.Style.Add("text-align", "right");
			lnk.Click += new System.EventHandler(this.Expand_Click);
			h.Controls.Add(lnk);

			DebugWrite("CreateLights END");
		}

		protected virtual void OnExpand(EventArgs e)
		{
			DebugWrite("OnExpand START");
			if (Expand != null)
			{
				Expand(this, e);
			}
			DebugWrite("OnExpand END");
		}

		private void Expand_Click(object sender, System.EventArgs e)
		{
			DebugWrite("Expand_Click START");
			OnExpand(e);
			DebugWrite("Expand_Click END");
		}

		protected string getLabelForRiskStatus(int risk)
		{
			switch (risk)
			{
				case (int)Risk.GREEN:
					return "LOW RISK. Manage for continous improvement";
				case (int)Risk.YELLOW:
					return "MEDIUM RISK. Incorporate risk reduction measures";
				case (int)Risk.RED:
					return "HIGH RISK. Intolerable";
				default:
					return "";
			}
		}

		protected string getLightImageUrl(int risk)
		{
			string ImageUrl = "";
			switch (risk)
			{
				case (int)Risk.GREEN:
					ImageUrl = "greenlight.gif";
					break;
				case (int)Risk.YELLOW:
					ImageUrl = "yellowlight.gif";
					break;
				case (int)Risk.RED:
					ImageUrl = "redlight.gif";
					break;
				default:
					ImageUrl = "graylight.gif";
					break;
			}
			return ImageUrl;
		}

		private void parent_ValueChange(object sender, EventArgs e)
		{
			DebugWrite("parent_ValueChange START");
			UpdateLights();
			DebugWrite("parent_ValueChange END");
		}

		protected void UpdateLights()
		{
			img.ImageUrl = getLightImageUrl(this.Value);
			status.Text = getLabelForRiskStatus(this.Value);
		}

		private void DebugWrite(String txt)
		{
			Debug.WriteLine("[Light.cs]" + txt);
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			DebugWrite("Render START");
			DebugWrite("VALUE is " + this.Value);
			base.Render(output);
			DebugWrite("Render END");
		}
	}
}
