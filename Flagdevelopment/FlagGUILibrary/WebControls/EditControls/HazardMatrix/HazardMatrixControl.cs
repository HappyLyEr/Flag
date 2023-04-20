using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Diagnostics;

namespace GASystem.WebControls.EditControls.HazardMatrix
{
	/// <summary>
	/// Summary description for HazardMatrixControl.
	/// </summary>
	[Designer(typeof(GASystem.WebControls.EditControls.HazardMatrix.HazardMatrixControlDesigner)), DefaultProperty("Value"), 
		ToolboxData("<{0}:HazardMatrixControl runat=server></{0}:HazardMatrixControl>")]
	public class HazardMatrixControl : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		public event EventHandler ValueChange;
		protected Label DebugLabel;
		protected Lights _lights = new Lights();
		protected Matrix _matrix = new Matrix();

		[Bindable(true), DefaultValue(Risk.GREEN)]
		public int Value
		{
			get
			{
				object val = ViewState["Value"];
				return (val == null) ? (int)Risk.GREEN : (int)ViewState["Value"];
			}

			set
			{
				ViewState["Value"] = value;
			}
		}

		public string Text 
		{
			get 
			{
				if (Value == 0)
					return "HIGH";
				else if (Value == 1)
					return "MEDIUM";
				else
					return "LOW";
			}

			set 
			{
				if (value == "HIGH")
					Value = 0;
				else if (value == "MEDIUM")
					Value = 1;
				else
					Value = 2;
			}

		}

		[Bindable(true),
		Category("Appearance"),
		DefaultValue(DisplayMode.COLLAPSED)]
		public DisplayMode mode
		{
			get
			{
				object val = ViewState["mode"];
				return (val == null) ? DisplayMode.COLLAPSED : (DisplayMode)ViewState["mode"];
			}

			set
			{
				ViewState["mode"] = value;
			}
		}

		public HazardMatrixControl()
		{
			DebugWrite("HazardMatrixControl CONSTRUCTOR");
		}

		protected override void OnInit(EventArgs e)
		{
			DebugWrite("OnInit START");
			this._lights.Expand += new System.EventHandler(this.ToggleControl);
			this._matrix.ValueChange += new System.EventHandler(this.UpdateValue);
			this._matrix.Collapse += new System.EventHandler(this.ToggleControl);
			DebugWrite("OnInit END");
		}

		protected override void OnLoad(EventArgs e)
		{
			DebugWrite("OnLoad START");
			_lights.Value = this.Value;
			_matrix.Value = this.Value;

			this.Controls.Add(_lights);
			this.Controls.Add(_matrix);
			DebugWrite("OnLoad END");
		}

		protected override void OnUnload(EventArgs e)
		{
			DebugWrite("OnUnload START");
			base.OnUnload(e);
			DebugWrite("OnUnload END");
		}


		protected override void OnPreRender(EventArgs e)
		{
			DebugWrite("OnPreRender START");
			if (mode == DisplayMode.EXPANDED) 
			{
				_lights.Visible = false;
				_matrix.Visible = true;
			} 
			else 
			{
				_lights.Visible = true;
				_matrix.Visible = false;
			}
			DebugWrite("OnPreRender END");
		}

		protected virtual void OnValueChange(EventArgs e)
		{
			DebugWrite("OnValueChange START");
			if (ValueChange != null)
			{
				ValueChange(this, e);
			}
			DebugWrite("OnValueChange END");
		}

		private void ValueChange_Perform(object sender, System.EventArgs e)
		{
			DebugWrite("ValueChange_Perform START");
			OnValueChange(e);
			DebugWrite("ValueChange_Perform END");
		}

		public string getDesignerHTML()
		{
			return "<img src=\"greenlight.gif\"/>";
		}

		protected void ToggleControl(object sender, System.EventArgs e)
		{
			ToggleControl();
		}

		protected void ToggleControl()
		{
			DebugWrite("ToggleControl START");
			mode = (mode == DisplayMode.EXPANDED) ? DisplayMode.COLLAPSED : DisplayMode.EXPANDED;
			DebugWrite("ToggleControl END");
		}

		protected void UpdateValue(object sender, System.EventArgs e)
		{
			DebugWrite("UpdateValue START");
			this.Value = _matrix.Value;
			_lights.Value = this.Value;
			ToggleControl();
			ValueChange_Perform(this, e);
			DebugWrite("UpdateValue END");
		}

		protected override void Render(HtmlTextWriter output)
		{
			DebugWrite("Render START");
			base.Render(output);
			DebugWrite("Render END");
		}


		private void DebugWrite(String txt)
		{
			Debug.WriteLine("[HazardMatrixControl.cs]" + txt);
		}
		
	}

	public enum DisplayMode { COLLAPSED, EXPANDED };
}
