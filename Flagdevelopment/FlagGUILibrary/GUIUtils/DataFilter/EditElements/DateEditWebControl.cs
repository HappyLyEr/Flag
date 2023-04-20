using System;
using System.Web.UI.WebControls;
using GASystem.GUIUtils;
using System.Web.UI;
using System.Collections;
using GASystem.AppUtils;
using System.Web;

namespace GASystem.GUIUtils.DataFilter.EditElements
{
	/// <summary>
	/// Edit controls for setting filters on date and datetime controls in lists
	/// </summary>
	public class DateEditWebControl :IEditWebControl, System.Web.UI.INamingContainer
	{
		CheckBox conditionCheckBox;
		Label _fieldName;

		Literal _containsText;
		GASystem.WebControls.EditControls.DateControl _conditionText;
        GASystem.WebControls.EditControls.DateControl _conditionTextBetween;
        private System.Web.UI.WebControls.Panel _placeHolderBetween;
        private Label betweenLabel;
        DropDownList _operatorSelector;
		private string _filterOperator;
		private string _filterCondition;
        private FieldDescription _fd;
		
		public DateEditWebControl(FieldDescription fd)
		{
			//
			// TODO: Add constructor logic here
			//
            _fd = fd;
			conditionCheckBox = new CheckBox();
			_fieldName = new Label();
			_containsText = new Literal();
			_conditionText = new GASystem.WebControls.EditControls.DateControl();
            _conditionTextBetween = new GASystem.WebControls.EditControls.DateControl();
            betweenLabel = new Label();
            _placeHolderBetween = new Panel();
            _placeHolderBetween.CssClass = "FieldContentCell_Between";
			_operatorSelector = new DropDownList();
            this.FieldName = fd.TableId + "." + fd.FieldId;
			//_filterOperator = fd.FilterOperator;
			//_filterCondition = fd.FilterCondition;
            bool useSessionFilter = false;

            if (HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-usesessionfilter"] != null)
                useSessionFilter = (bool)HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-usesessionfilter"];

            if (useSessionFilter)
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-filteroperator"] != null)
                    _filterOperator = HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-filteroperator"].ToString();
                else
                    _filterOperator = string.Empty;

                if (HttpContext.Current.Session != null && HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-filtercondition"] != null)
                    _filterCondition = HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-filtercondition"].ToString();
                else
                    _filterCondition = string.Empty;
            }
            else
            {
                _filterOperator = fd.FilterOperator;
                _filterCondition = fd.FilterCondition;
            }

			_fieldName.Text = GASystem.AppUtils.Localization.GetCaptionText(fd.DataType);
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Tr));
			
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS ));
			this.Controls.Add(conditionCheckBox);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));
			
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			this.Controls.Add(_fieldName);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));
//			
//			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td));
//			this._containsText.Text = " " + GASystem.AppUtils.Localization.GetGuiElementText("contains") + " ";
//			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			populateOperatorSelector();
			this.Controls.Add(_operatorSelector);
            _operatorSelector.ID = GetFieldNameIdentifier() + "operatorSelector";
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
            _conditionText.ID = GetFieldNameIdentifier();
			_conditionText.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID +  "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");
			this.Controls.Add(_conditionText);
			_conditionText.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID +  "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");


            betweenLabel.Text = "&nbsp;" + AppUtils.Localization.GetGuiElementText("and") + "&nbsp;";
            betweenLabel.ID = GetFieldNameIdentifier() + "betweenlabel";
            this.Controls.Add(betweenLabel);
            
            //_placeHolderBetween.ID = this.FieldName + "placeholderbetween";
            
           // this.Controls.Add(_placeHolderBetween);

            //_placeHolderBetween.Controls.Add(GUIUtils.HTMLLiteralTags.CreateTextElement("&nbsp;"  +  AppUtils.Localization.GetGuiElementText("and") +  "&nbsp;" ));

            _conditionTextBetween.ID = GetFieldNameIdentifier() + "between";
            _conditionTextBetween.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID + "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");
            this.Controls.Add(_conditionTextBetween);
            _conditionTextBetween.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID + "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");

            //add validator
            CompareValidator dateCompare = new CompareValidator();
            dateCompare.ControlToCompare = _conditionTextBetween.ID;
            dateCompare.ControlToValidate = _conditionText.ID;
            dateCompare.Operator = ValidationCompareOperator.LessThanEqual;
            dateCompare.Type = ValidationDataType.Date;
            dateCompare.EnableClientScript = false;
            dateCompare.ErrorMessage = "<br/>" + AppUtils.Localization.GetGuiElementText("StartDateLessThanEqualEndDate");
            this.Controls.Add(dateCompare);

			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Tr));
		}

		private void populateOperatorSelector() 
		{
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.GreaterThan.ToString()), FilterOperator.GreaterThan.ToString())); 
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.LessThan.ToString()), FilterOperator.LessThan.ToString()));
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.isCurrent.ToString()), FilterOperator.isCurrent.ToString()));
            _operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.Between.ToString()), FilterOperator.Between.ToString()));
            // tor 20150211 added filter operator DateEqualTo
            _operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.DateEqualTo.ToString()), FilterOperator.DateEqualTo.ToString()));

			_operatorSelector.Attributes.Add("onChange", GetJavascriptFuntionName() + "();");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");
			_operatorSelector.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID +  "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");
		}

		public override bool enabledElement
		{
			get
			{
				return conditionCheckBox.Checked;
			}
			set
			{
				conditionCheckBox.Checked = value;
			}
		}

		public override  string FieldName 
		{
			get {return null==ViewState["FieldName"+this.ID] ? string.Empty :  ViewState["FieldName"+this.ID].ToString();}
			set 
			{
				ViewState["FieldName"+this.ID] = value;
			//	_fieldName.Text = GASystem.AppUtils.Localization.GetCaptionText(value);
		
			}
			
			//set {_fieldName.Text = value;}
			//get {return _fieldName.Text;}
		}

		public override string FieldNameDisplay
		{
			get {  return _fieldName.Text; }
		}

		public string ListCategory 
		{
			set {ViewState["ListCategory"+this.ID] = value;}
			get {return null==ViewState["ListCategory"+this.ID] ? string.Empty :  ViewState["ListCategory"+this.ID].ToString();}
		}

		public override string Condition
		{
			get 
			{
                if (ConditionOperator != FilterOperator.Between.ToString())
                {
                    // tor 20150211 added conditiontext for DateEqualTo (changed again 20150205 - removed start stop ' when DateEqualTo)
                    if (ConditionOperator == FilterOperator.DateEqualTo.ToString())
                    {
                        DateTime dt = DateTime.Now;
                        if (!_conditionText.IsNull)
                        {
                            dt = _conditionText.Value;
                        }

                        // Tor 20150205 string a = "'" + dt.Year + "-" + dt.Month + "-" + dt.Day + " 00:00:00' AND '" + dt.Year + "-" + dt.Month + "-" + dt.Day + " 23:59:59";
                        // Tor 20150205 return "'" + dt.Year + "-" + dt.Month + "-" + dt.Day + " 00:00:00' AND '" + dt.Year + "-" + dt.Month + "-" + dt.Day + " 23:59:59'";
                        string a = dt.Year + "-" + dt.Month + "-" + dt.Day + " 00:00:00' AND '" + dt.Year + "-" + dt.Month + "-" + dt.Day + " 23:59:59";
                        return dt.Year + "-" + dt.Month + "-" + dt.Day + " 00:00:00' AND '" + dt.Year + "-" + dt.Month + "-" + dt.Day + " 23:59:59";
                    }
                    else
                    {
                        DateTime dt = DateTime.Now;
                        if (!_conditionText.IsNull)
                            dt = _conditionText.Value;
                        return dt.Year + "-" + dt.Month + "-" + dt.Day;
                    }
                }
                else
                {
                    DateTime dtleft = DateTime.Now;
                    DateTime dtrigth = DateTime.Now;
                    if (!_conditionText.IsNull && !_conditionTextBetween.IsNull)
                    {
                        dtleft = _conditionText.Value;
                        dtrigth = _conditionTextBetween.Value;
                    }

                    return "'" + dtleft.Year + "-" + dtleft.Month + "-" + dtleft.Day + "' AND '" +
                        dtrigth.Year + "-" + dtrigth.Month + "-" + dtrigth.Day + "'";

                }
			}
		}

        public override string ConditionText
        {
            get 
            {

                if (ConditionOperator != FilterOperator.Between.ToString())
                {
                    DateTime dt = DateTime.Now;
                    if (!_conditionText.IsNull)
                        dt = _conditionText.Value;
                    return dt.Year + "-" + dt.Month + "-" + dt.Day;
                }
                else
                {
                    DateTime dtleft = DateTime.Now;
                    DateTime dtrigth = DateTime.Now;
                    if (!_conditionText.IsNull && !_conditionTextBetween.IsNull)
                    {
                        dtleft = _conditionText.Value;
                        dtrigth = _conditionTextBetween.Value;
                    }
                    return dtleft.Year + "-" + dtleft.Month + "-" + dtleft.Day + "t" +
                        dtrigth.Year + "-" + dtrigth.Month + "-" + dtrigth.Day;
                }

                //if (ConditionOperator != FilterOperator.Between.ToString())
                //{
                //    DateTime dt = DateTime.Now;
                //    if (!_conditionText.IsNull)
                //        dt = _conditionText.Value;
                //    return dt.Year + "-" + dt.Month + "-" + dt.Day;
                //}
                //else
                //{
                //    DateTime dtleft = DateTime.Now;
                //    DateTime dtrigth = DateTime.Now;
                //    if (!_conditionText.IsNull && !_conditionTextBetween.IsNull)
                //    {
                //        dtleft = _conditionText.Value;
                //        dtrigth = _conditionTextBetween.Value;
                //    }
                //    return dtleft.ToShortDateString() + " " + AppUtils.Localization.GetGuiElementText("and") + " " + dtrigth.ToShortDateString();
                //}
            }
        }

		public override string ConditionOperator 
		{
			get {return _operatorSelector.SelectedValue.ToString();}
		}

		public override void SetDefaultFilter()
		{
			
			
                try
                {
                    if (_filterCondition.Contains("t"))
                    {
                        string[] dates = _filterCondition.Split('t');
                        DateTime betweenStart = DateTime.Parse(dates[0]);
                        DateTime betweenEnd = DateTime.Parse(dates[1]);
                        _conditionText.Value = betweenStart;
                        _conditionTextBetween.Value = betweenEnd;
                    }
                    else
                    {
                        DateTime conditionDate;
                        conditionDate = DateTime.Parse(_filterCondition);
                        _conditionText.Value = conditionDate;
                    }
                }
                catch 
                {
                    //TODO log 
                }
			try 
			{
				//_operatorSelector.Items.Add();
				foreach (ListItem item in _operatorSelector.Items)
                    if (item.Value == _filterOperator)
                    {
                        item.Selected = true;
                        conditionCheckBox.Checked = true;
                    }
                    else {
                        item.Selected = false;
                    }


				

				} 
			catch 
			{
				//ignore error TODO: log 
			}

			
			return;
		}


        public override void SetDefaultFilter(string FilterOperator, string FilterCondition)
        {
            DateTime conditionDate;
            try
            {
                if (FilterOperator.ToUpper() == GASystem.GUIUtils.DataFilter.FilterOperator.Between.ToString().ToUpper())
                {
                    DateTime conditionDateFrom;
                    DateTime conditionDateTo;
                    
                    string[] conditionDates = FilterCondition.Split('t');

                    conditionDateFrom = DateTime.Parse(conditionDates[0]);
                    conditionDateTo = DateTime.Parse(conditionDates[1]);

                    //_operatorSelector.Items.Add();
                    foreach (ListItem item in _operatorSelector.Items)
                        if (item.Value == FilterOperator)
                        {
                            item.Selected = true;
                            conditionCheckBox.Checked = true;
                        }
                        else item.Selected = false;


                    _conditionText.Value = conditionDateFrom;
                    _conditionTextBetween.Value = conditionDateTo;


                }
                else
                {
                   conditionDate = DateTime.Parse(FilterCondition);


                    //_operatorSelector.Items.Add();
                   foreach (ListItem item in _operatorSelector.Items)
                       if (item.Value == FilterOperator)
                       {
                           item.Selected = true;
                           conditionCheckBox.Checked = true;
                       }
                       else item.Selected = false;


                    _conditionText.Value = conditionDate;
                }
            }
            catch
            {
                //ignore error TODO: log 
            }


            return;
        }

		protected override void OnPreRender(EventArgs e)
		{
			
			base.OnPreRender (e);
			//add javascript code for hidding dateselector and tick enabled 
			string jscode = "<script language=\"javascript\">\n";
			jscode += "function " + GetJavascriptFuntionName() + "() {\n";
			jscode +=  "var operatorList = document.getElementById('" + _operatorSelector.ClientID +  "');";
			//jscode += "document.getElementById('" + _operatorSelector.ClientID +  "').selectedIndex"
//			jscode += "alert(\"clicked selector\" + operatorList.selectedIndex);";
//			jscode += "alert(\"value selector\" + operatorList.options[operatorList.selectedIndex].value);";
	
			jscode += "if (operatorList.options[operatorList.selectedIndex].value == 'isCurrent') { ";
            jscode += "   document.getElementById('" + _conditionTextBetween.ClientID + "').style.visibility = 'hidden';    ";
			jscode += "   document.getElementById('" + _conditionText.ClientID +  "').style.visibility = 'hidden';   } ";

            jscode += "else if (operatorList.options[operatorList.selectedIndex].value == 'Between') { ";
            jscode += "   document.getElementById('" + _conditionTextBetween.ClientID + "').style.visibility = 'visible';    ";
            jscode += "   document.getElementById('" + _conditionText.ClientID + "').style.visibility = 'visible';   } ";
			
            
            jscode += "else     {";
            jscode += "   document.getElementById('" + _conditionTextBetween.ClientID + "').style.visibility = 'hidden';    ";
            
			jscode += "   document.getElementById('" + _conditionText.ClientID +  "').style.visibility = 'visible';   } ";
            jscode += "    document.getElementById('" + betweenLabel.ClientID + "').style.visibility =  document.getElementById('" + _conditionTextBetween.ClientID + "').style.visibility ";
            jscode += "     ";
			jscode += "}\n </script>";
			this.Page.ClientScript.RegisterStartupScript(typeof(DateEditWebControl), GetJavascriptFuntionName(), jscode);

			//set start visibility on conditiontext control
			if (_operatorSelector.SelectedValue == FilterOperator.isCurrent.ToString()) 
			{
				_conditionText.Style.Add("visibility", "hidden");   //using style to hide the control. Setting the visibility properties stops 
                _conditionTextBetween.Style.Add("visibility", "hidden");
                betweenLabel.Style.Add("visibility", "hidden");
                //control from being rendered on page
			}
            else if (_operatorSelector.SelectedValue == FilterOperator.Between.ToString())
            {
                _conditionText.Style.Add("visibility", "visible");  //controls remebers stylesetting postback from page were previous filteroperator was
                _conditionTextBetween.Style.Add("visibility", "visible");
                betweenLabel.Style.Add("visibility", "visible");													//isCurrent hides the control because the previous value was hidden. setting it specifically stops this behaviour.
            }
            else
            {
                _conditionText.Style.Add("visibility", "visible");  //controls remebers stylesetting postback from page were previous filteroperator was
                _conditionTextBetween.Style.Add("visibility", "hidden");
                betweenLabel.Style.Add("visibility", "hidden");
            }

		}

		private string GetJavascriptFuntionName() 
		{
            string functionName = "SetEditDateVisibility" + FieldName.Replace(".", string.Empty); ;
			return functionName;
		}

        private string GetFieldNameIdentifier()
        {
            return FieldName.Replace(".", string.Empty); 
        }

        public override void ResetToDefaulFilter()
        {
            _filterOperator = _fd.FilterOperator;
            _filterCondition = _fd.FilterCondition;
            this.enabledElement = false;
            SetDefaultFilter();
        }
	}
}
