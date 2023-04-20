using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace GASystem.WebControls.ViewControls
{
	/// <summary>
	/// Summary description for CheckBox.
	/// </summary>
	public class CheckBoxView : System.Web.UI.WebControls.WebControl
	{
		private CheckBox _checkBox = new CheckBox();
		
		protected override void OnInit(EventArgs e)
		{
			_checkBox.Enabled = false;  //only display user can not change value
			_checkBox.Visible = true;
			_checkBox.Checked = false;  //default value is not checked;
			this.Controls.Add(_checkBox);
			base.OnInit (e);
		}


		public CheckBoxView()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool Checked  
		{
			get {return _checkBox.Checked;}
			set {_checkBox.Checked = value;}
		}
	}
}
