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
	public class ListDateTimeControl : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private Calendar _calendar;
	//	private DropDownList timeDropDown;
		//private Button _butShowHide;
		//private ImageButton _butShowHide;
		private HyperLink _butShowHide;
	//	private System.Web.UI.WebControls.HyperLink _butShowHide;
		private TextBox txtBox;
		private TextBox txtBoxTime;
		private short tabIndex = 0;

		public DateTime Value 
		{
			get
			{
				DateTime selectedDateTime = _calendar.SelectedDate;
				DateTime selectedTime = Convert.ToDateTime(txtBoxTime.Text);
				selectedDateTime = selectedDateTime.Add(selectedTime.TimeOfDay);
				//selectedDateTime.Minute = selectedTime.Minute;
				return selectedDateTime;
			}

			set
			{
				_calendar.SelectedDate = value;
				_calendar.VisibleDate = value;
			//	timeDropDown.SelectedIndex = value.Hour;
				txtBoxTime.Text = value.ToString("t");
				txtBox.Text = value.ToShortDateString();
			}
		}

		public override short TabIndex
		{
			get
			{
				return txtBox.TabIndex;
				//return tabIndex;
			}
			set
			{
				txtBox.TabIndex = value;
				//tabIndex = value;
			}
		}


		public string TextValue 
		{
			get {return txtBox.Text;}
		}

		public bool IsNull 
		{
			get {return _calendar.SelectedDates.Count == 0; }
		}

		protected override void OnInit(EventArgs e)
		{
		base.OnInit (e);
			txtBox = new TextBox();
			txtBox.Text = ""; //set a default value so that client side validation picks up value="" from the first child in this control
			txtBox.ID = "date";
			txtBox.Width = new Unit(11*8+"px");
			txtBox.EnableViewState = true;
			txtBox.TextChanged += new EventHandler(txtBox_TextChanged);

			txtBoxTime = new TextBox();
			txtBoxTime.Text = ""; //set a default value so that client side validation picks up value="" from the first child in this control
			txtBoxTime.ID = "time";
			txtBoxTime.Width =  new Unit(11*8+"px");
			txtBoxTime.EnableViewState = true;
			
//			txtBoxTime.TextChanged += new EventHandler(txtBoxTime_TextChanged);
			
			_calendar = new Calendar();
			_calendar.Visible = false;
			
			_calendar.SelectionMode = CalendarSelectionMode.Day;
			_calendar.SelectionChanged += new EventHandler(_calendar_SelectionChanged);

			string[] times = new string[24];
//			timeDropDown = new DropDownList();
//			for (int x=0;x<24;x++) 
//			{
//				
//				times[x] = ((DateTime)(DateTime.Today + new TimeSpan(x*TimeSpan.TicksPerHour))).ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern);
//			}
//			
//
//			timeDropDown.Visible = false;
//			timeDropDown.DataSource = times;
//			timeDropDown.DataBind();
//			timeDropDown.AutoPostBack = true;
//			timeDropDown.SelectedIndexChanged += new EventHandler(timeDropDown_SelectedIndexChanged);
//




//			_butShowHide = new ImageButton();
//			_butShowHide.ImageUrl = "~/images/dateicon.gif";
//			_butShowHide.CausesValidation = false;
//			_butShowHide.Click += new ImageClickEventHandler(_butShowHide_Click);
//			
			_butShowHide = new HyperLink();
            _butShowHide.ImageUrl = "~/gagui/images/dateicon.gif";
			




		//	custValidator = new CustomValidator();
		//	custValidator.Text = "Invalid date";
		//	custValidator.ServerValidate += new ServerValidateEventHandler(custValidator_ServerValidate);

			this.Controls.Add(txtBox);
			this.Controls.Add(txtBoxTime);
			this.Controls.Add(_butShowHide);
//			Table dateTable = new Table();
//			TableRow tr = new TableRow();
//			dateTable.Controls.Add(tr);
//			TableCell td = new TableCell();
//			tr.Controls.Add(td);
//			td.Controls.Add(_calendar);
//			td = new TableCell();
//			td.VerticalAlign = VerticalAlign.Top;
//			tr.Controls.Add(td);
		//	td.Controls.Add(timeDropDown);

			this.Controls.Add(_calendar);
			//this.Controls.Add(_calendar);
			//this.Controls.Add(timeDropDown);


		//	this.Controls.Add(custValidator);
		}

		protected override void OnPreRender(EventArgs e)
		{
			int hour = 0;
			try 
			{
				DateTime ts = Convert.ToDateTime(txtBoxTime.Text);
				//TimeSpan ts = TimeSpan.Parse(txtBoxTime.Text.Trim());
				hour = ts.Hour;
			} 
			catch {}
			
			
			StringBuilder sb = new StringBuilder();
			sb.Append("<script language=javascript>");

			sb.Append("function openPickerWindow" + this.ID+ "(valueControlId, date, hour) {");

			
			sb.Append("Fwin = window.open('" + this.Page.Request.ApplicationPath + "/gagui/WebForms/datetimepicker.aspx?ParentValueControlId='+valueControlId+'&date='+date+'&time='+hour+'', 'pick_dialog_datetime', 'toolbar=no, directories=no, location=no, status=no, menubar=no, resizable=yes, scrollbars=yes, width=400, height=600');");
			sb.Append("if (Fwin && !Fwin.closed) Fwin.focus();}");
			
			sb.Append("</script>");
			if(!Page.IsClientScriptBlockRegistered("openpickerwindow" + this.ID))
				Page.RegisterClientScriptBlock("openpickerwindow" + this.ID, sb.ToString());
			
			
			
//			if (!this.Page.IsPostBack) 
//			{
//				if (txtBoxTime.Text != string.Empty) 
//				{
//					try 
//					{
//						DateTime ts = Convert.ToDateTime(txtBoxTime.Text);
//						//TimeSpan ts = TimeSpan.Parse(txtBoxTime.Text.Trim());
//						timeDropDown.SelectedIndex = ts.Hour;
//					} 
//					catch {}
//				}
//			}
			base.OnPreRender (e);
			_butShowHide.NavigateUrl = "javascript:openPickerWindow" + this.ID+ "('"+this.ClientID+"', '"+Page.Server.UrlEncode(txtBox.Text)+"', '"+hour+"')";
		}


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



//		private void _butShowHide_Click(object sender, ImageClickEventArgs e)
//		{
//			_calendar.Visible = !_calendar.Visible;
//			timeDropDown.Visible = _calendar.Visible;
//		}



//		private void custValidator_ServerValidate(object source, ServerValidateEventArgs args)
//		{
//			DateControl tmpCont = (DateControl)this.Parent.FindControl("datetesttest");
//			GASystem.GUIUtils.ValidationControl.ItemGreaterThan validator = new GASystem.GUIUtils.ValidationControl.ItemGreaterThan();
//			args.IsValid = validator.Validate(this, tmpCont);
//			custValidator.Text = validator.ErrorText + " which is todays date";
//		}

		private void _calendar_SelectionChanged(object sender, EventArgs e)
		{
			txtBox.Text = ((Calendar)sender).SelectedDate.ToShortDateString();
			//custValidator.Validate();
		//	_calendar.Visible = false;
		}

		private void txtBox_TextChanged(object sender, EventArgs e)
		{
			string newText = ((TextBox)sender).Text;
			if (newText == string.Empty)
				_calendar.SelectedDates.Clear();
			else 
			{
				if (GAUtils.IsDate(newText)) 
				{
					_calendar.SelectedDate = Convert.ToDateTime(newText);
					//this.Value = Convert.ToDateTime(newText);
				}
			}
		}

//		private void txtBoxTime_TextChanged(object sender, EventArgs e)
//		{
//			try 
//			{
//				DateTime ts = Convert.ToDateTime(txtBoxTime.Text);
//				//TimeSpan ts = TimeSpan.Parse(txtBoxTime.Text.Trim());
//				timeDropDown.SelectedIndex = ts.Hour;
//			} 
//			catch {}
//		}

//		private void timeDropDown_SelectedIndexChanged(object sender, EventArgs e)
//		{
//			txtBoxTime.Text = timeDropDown.SelectedItem.ToString();
//		}
	}
}
