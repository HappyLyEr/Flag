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
	public class DatePeriodeEditWebControl :IEditWebControl, System.Web.UI.INamingContainer
	{
		CheckBox conditionCheckBox;
		Label _fieldName;

		Literal _containsText;
		GASystem.WebControls.EditControls.DateControl _conditionTextFrom;
        GASystem.WebControls.EditControls.DateControl _conditionTextTo;
		
		DropDownList _operatorSelector;
		private string _filterOperator;
		private string _filterCondition;
        private ClassDescription _cd;
        private FieldDescription _fd;

        public DatePeriodeEditWebControl(FieldDescription fd)
		{
			//
			// TODO: Add constructor logic here
			//
            _fd = fd;

			conditionCheckBox = new CheckBox();
			_fieldName = new Label();
			_containsText = new Literal();
            _conditionTextFrom = new GASystem.WebControls.EditControls.DateControl();
            _conditionTextTo = new GASystem.WebControls.EditControls.DateControl();
			_operatorSelector = new DropDownList();
            _cd = ClassDefinition.GetClassDescriptionByGADataClass(fd.TableId);
            this.FieldName = fd.TableId + "." + fd.FieldId + "-" + _cd.DateToField;
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



            FieldDescription fdTo = FieldDefintion.GetFieldDescription(_cd.DateToField, _cd.DataClassName);
            FieldDescription fdFrom = FieldDefintion.GetFieldDescription(_cd.DateFromField, _cd.DataClassName);
            _fieldName.Text = GASystem.AppUtils.Localization.GetCaptionText(fdFrom.DataType) + " " 
                +  GASystem.AppUtils.Localization.GetGuiElementText("or")  + " " + GASystem.AppUtils.Localization.GetCaptionText(fdTo.DataType);
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
			_operatorSelector.ID = this.FieldName + "operatorSelector";
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
            _conditionTextFrom.ID = _cd.DateFromField + "periode";
            _conditionTextFrom.Width = new Unit(80);
          //  _conditionTextFrom.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID + "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");
            this.Controls.Add(_conditionTextFrom);
           _conditionTextFrom.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID + "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");

           

           this.Controls.Add(HTMLLiteralTags.CreateTextElement("&nbsp;<span>" + GASystem.AppUtils.Localization.GetGuiElementText("To") + "</span>&nbsp;"));  //GASystem.AppUtils.Localization.GetGuiElementText("to");
			//_conditionText.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID +  "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");

            _conditionTextTo.ID = _cd.DateToField + "periode";
            _conditionTextTo.Width = new Unit(80);
         //   _conditionTextTo.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID + "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");
            this.Controls.Add(_conditionTextTo);
           _conditionTextTo.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID + "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");


           //add validator
           CompareValidator dateCompare = new CompareValidator();
           dateCompare.ControlToCompare = _conditionTextTo.ID;
           dateCompare.ControlToValidate = _conditionTextFrom.ID;
           dateCompare.Operator = ValidationCompareOperator.LessThanEqual;
           dateCompare.Type = ValidationDataType.Date;
           dateCompare.EnableClientScript = false;
           dateCompare.ErrorMessage = "<br/>" +  AppUtils.Localization.GetGuiElementText("StartDateLessThanEqualEndDate");
           this.Controls.Add(dateCompare);

			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Tr));
		}

		private void populateOperatorSelector() 
		{
            _operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.inPeriod.ToString()), FilterOperator.inPeriod.ToString())); 
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
                DateTime dtFrom = DateTime.Now;
                if (!_conditionTextFrom.IsNull)
                    dtFrom = _conditionTextFrom.Value;
                DateTime dtTo = DateTime.Now;
                if (!_conditionTextTo.IsNull)
                    dtTo = _conditionTextTo.Value;

                string conditionFrom = "'" + dtFrom.Year + "-" + dtFrom.Month + "-" + dtFrom.Day + "'";
                string conditionTo = "'" + dtTo.Year + "-" + dtTo.Month + "-" + dtTo.Day + "'";

                string conditionFull = generateDateSpanFilter(_cd.DataClassName + "." + _cd.DateFromField, _cd.DataClassName + "." + _cd.DateToField);
                conditionFull = conditionFull.Replace("@dateFrom", conditionFrom);
                conditionFull = conditionFull.Replace("@DateTo", conditionTo);

                return conditionFull;
			}
		}

        public override string ConditionText
        {
            get 
            {

                DateTime dtFrom = DateTime.Now;
                if (!_conditionTextFrom.IsNull)
                    dtFrom = _conditionTextFrom.Value;
                DateTime dtTo = DateTime.Now;
                if (!_conditionTextTo.IsNull)
                    dtTo = _conditionTextTo.Value;

                string conditionFrom = dtFrom.Year + "-" + dtFrom.Month + "-" + dtFrom.Day;
                string conditionTo = dtTo.Year + "-" + dtTo.Month + "-" + dtTo.Day;

                return conditionFrom + "t" + conditionTo;
            
            }
        }

		public override string ConditionOperator 
		{
			get {return _operatorSelector.SelectedValue.ToString();}
		}


        /// <summary>
        /// Should not be in use for this control
        /// </summary>
		public override void SetDefaultFilter()
		{
			return; 
            
            //SetDefaultFilter(FilterOperator.inPeriode
            
            //DateTime conditionDate;
			
            //    try
            //    {
            //        conditionDate = DateTime.Parse(_filterCondition);
            //        _conditionText.Value = conditionDate;
            //    }
            //    catch 
            //    {
            //        //TODO log 
            //    }
            //try 
            //{
            //    //_operatorSelector.Items.Add();
            //    foreach (ListItem item in _operatorSelector.Items)
            //        if (item.Value == _filterOperator) 
            //        {
            //            item.Selected = true;
            //            conditionCheckBox.Checked = true;
            //        }


				

            //    } 
            //catch 
            //{
            //    //ignore error TODO: log 
            //}

			
            //return;
		}


        public override void SetDefaultFilter(string FilterOperator, string FilterCondition)
        {
            DateTime conditionDateFrom;
            DateTime conditionDateTo;
            try
            {
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


                _conditionTextFrom.Value = conditionDateFrom;
                _conditionTextTo.Value = conditionDateTo;


            }
            catch
            {
                //ignore error TODO: log 
            }


            return;
        }

		protected override void OnPreRender(EventArgs e)
		{
			
			
		}

        private string generateDateSpanFilter(string DateFromField, string DateToField)
        {
            //todate in timespan
            string filterto = " ( @dateFrom <= _formDateField_  and _formDateField_ <= @DateTo     ) ";

            //from date in timespan
            string filterfrom = " ( @dateFrom <= _toDateField_  and _toDateField_ <= @DateTo     ) ";

            //timespan between start and end
            string filterin = " ( @dateFrom >= _formDateField_  and _toDateField_ >= @DateTo     ) ";

            //from is null
            string filterfromnull = " ( _formDateField_ is null and _toDateField_ >= @dateFrom     ) ";
            //to is null
            string filtertonull = " ( _toDateField_ is null  and _formDateField_ <= @DateTo     ) ";
            //both is null
            string filternull = " ( _toDateField_ is null  and _formDateField_ is null     ) ";

            //combined 
            string filter = " (" + filterto + " or " + filterfrom + " or " + filterin + " or " + filterfromnull + " or " + filtertonull + " or " + filternull + ") ";

            filter = filter.Replace("_formDateField_", DateFromField);
            filter = filter.Replace("_toDateField_", DateToField);

            return filter;
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
