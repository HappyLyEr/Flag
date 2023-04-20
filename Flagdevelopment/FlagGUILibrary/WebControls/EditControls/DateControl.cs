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
	public class DateControl : System.Web.UI.WebControls.WebControl, INamingContainer
	{
        //private Calendar _calendar;
        ////private Button _butShowHide;
        ////private LinkButton _butShowHide;
        ////private ImageButton _butShowHide;
        //private System.Web.UI.WebControls.HyperLink _butShowHide;
        //private TextBox txtBox;
        //private short tabIndex = 0;
        Telerik.WebControls.RadDatePicker _calendar;

		public DateTime Value 
		{
			get
			{
				return (DateTime)_calendar.SelectedDate;
			}

			set
			{
				_calendar.SelectedDate = value;
				
				
			}
		}

		public override short TabIndex
		{
			get
			{
				return _calendar.TabIndex;
				//return tabIndex;
			}
			set
			{
				_calendar.TabIndex = value;
				//tabIndex = value;
			}
		}


		public string TextValue 
		{
			get {return _calendar.SelectedDate.ToString();}
		}

		public bool IsNull 
		{
            get { return _calendar.SelectedDate == null;  } // return _calendar.IsEmpty; }
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			_calendar = new Telerik.WebControls.RadDatePicker();
            _calendar.ID = this.ID + "datepicker";
            _calendar.Calendar.Skin = "FlagCalendar";
			//_calendar.Visible = false;
			//_calendar.seSelectionMode = CalendarSelectionMode.Day;
			//_calendar.SelectionChanged += new EventHandler(_calendar_SelectionChanged);

			//ParentValueControlId
			//_butShowHide = new LinkButton();
			//_butShowHide = new ImageButton();
            //_butShowHide = new HyperLink();
            //_butShowHide.ImageUrl = "~/gagui/images/dateicon.gif";
            ////_butShowHide.Target = "_new";
            ////_butShowHide.Text = "Show - Hide";

            //_butShowHide.ImageUrl = "~/gagui/images/dateicon.gif";
            ////_butShowHide.CausesValidation = false;

			//_butShowHide.Click += new EventHandler(_butShowHide_Click);
			//_butShowHide.Click += new ImageClickEventHandler(_butShowHide_Click);

		//	custValidator = new CustomValidator();
		//	custValidator.Text = "Invalid date";
		//	custValidator.ServerValidate += new ServerValidateEventHandler(custValidator_ServerValidate);

            //this.Controls.Add(txtBox);
            //this.Controls.Add(_butShowHide);
			this.Controls.Add(_calendar);

			
		//	this.Controls.Add(custValidator);
		}

        //protected override void OnPreRender(EventArgs e)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<script language=javascript>");

        //    sb.Append("function openPickerWindow" + this.ID+ "() {");
        //    sb.Append("Fwin = window.open('" + this.Page.Request.ApplicationPath + "/gagui/WebForms/datepicker.aspx?ParentValueControlId=" + this.ClientID + "&date=" + Page.Server.UrlEncode(txtBox.Text) + "', 'pick_dialog_date', 'toolbar=no, directories=no, location=no, status=no, menubar=no, resizable=yes, scrollbars=yes, width=400, height=600');");
        //    sb.Append("if (Fwin && !Fwin.closed) Fwin.focus();}");
			
        //    sb.Append("</script>");
        //    if(!Page.IsClientScriptBlockRegistered("openpickerwindow" + this.ID))
        //        Page.RegisterClientScriptBlock("openpickerwindow" + this.ID, sb.ToString());
			
			
        //    base.OnPreRender (e);
        //    //_butShowHide.NavigateUrl = "~/gagui/webforms/datepicker.aspx?ParentValueControlId=" + this.ClientID + "&date=" + Page.Server.UrlEncode(txtBox.Text);
        //    _butShowHide.NavigateUrl = "javascript:openPickerWindow" + this.ID+ "()";
        //}


		protected override void CreateChildControls()
		{
			//txtBox.TabIndex = tabIndex;
			base.CreateChildControls ();
		}

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
//		protected override void Render(HtmlTextWriter output)
//		{
//			output.Write(Text);
		//		}

        //private void _butShowHide_Click(object sender, ImageClickEventArgs e)
        //{
        //    _calendar.Visible = !_calendar.Visible;
        //}

//		private void custValidator_ServerValidate(object source, ServerValidateEventArgs args)
//		{
//			DateControl tmpCont = (DateControl)this.Parent.FindControl("datetesttest");
//			GASystem.GUIUtils.ValidationControl.ItemGreaterThan validator = new GASystem.GUIUtils.ValidationControl.ItemGreaterThan();
//			args.IsValid = validator.Validate(this, tmpCont);
//			custValidator.Text = validator.ErrorText + " which is todays date";
//		}

        //private void _calendar_SelectionChanged(object sender, EventArgs e)
        //{
        //    txtBox.Text = ((Calendar)sender).SelectedDate.ToShortDateString();
        //    //custValidator.Validate();
        //    _calendar.Visible = false;
        //}

        //private void txtBox_TextChanged(object sender, EventArgs e)
        //{
        //    string newText = ((TextBox)sender).Text;
        //    if (newText == string.Empty)
        //        _calendar.SelectedDates.Clear();
        //    else 
        //    {
        //        if (GAUtils.IsDate(newText)) 
        //        {
        //            this.Value = Convert.ToDateTime(newText);
        //        }
        //    }
        //}

		
	}
}
