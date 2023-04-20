using System;
using System.Web.UI.WebControls;
using GASystem.GUIUtils;
using System.Web.UI;
using System.Collections;
using System.Web;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.DataFilter.EditElements
{
	/// <summary>
	/// Summary description for GeneralEditWebControl.
	/// </summary>
	public class ListEditWebControl :IEditWebControl, System.Web.UI.INamingContainer
	{
		CheckBox conditionCheckBox;
		Label _fieldName;

		Literal _containsText;
		DropDownList _conditionText;
		
		DropDownList _operatorSelector;

		private string _filterOperator;
		private string _filterCondition;
        private FieldDescription _fd;
		
		public ListEditWebControl(FieldDescription fd)
		{
			//
			// TODO: Add constructor logic here
			//
            _fd = fd;
			conditionCheckBox = new CheckBox();
			_fieldName = new Label();
			_containsText = new Literal();
			_conditionText = new DropDownList();
            _conditionText.CssClass = "FilterConditionDropDown";
			_operatorSelector = new DropDownList();
            
			this.FieldName = fd.TableId + "." + fd.FieldId;
			this.ListCategory = fd.ListCategory;


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
			
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			this.Controls.Add(conditionCheckBox);
			conditionCheckBox.ID = this.FieldName + "checkbox";
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));
			
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			this.Controls.Add(_fieldName);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));
			
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
            // ZhangBaisheng 20210318 GARemedialActionView / Overdue Period
            if (_fd.FieldId == "OverDuelistRowId")
                overDue_populateOperatorSelector();
            else 
                populateOperatorSelector();
			this.Controls.Add(_operatorSelector);
			_operatorSelector.ID = this.FieldName + "operatorSelector";
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));

            // Tor 20140623 call GetList with current class and ownerclass to decide for each listitem if it is to be part of the list to display to the user
            //ArrayList listItems = CodeTables.GetList(ListCategory, GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord);
            ArrayList listItems = CodeTables.GetList(ListCategory, _fd.TableId, _fd.FieldId, GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord);

			CodeTables.BindCodeTable(_conditionText, listItems);
			_conditionText.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID +  "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");
		
			this.Controls.Add(_conditionText);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Tr));
		}

		private void populateOperatorSelector() 
		{
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.Equal.ToString()), FilterOperator.Equal.ToString()));  // "="));
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.NotIncluding.ToString()), FilterOperator.NotIncluding.ToString()));  // "not ="));			
		}

        private void overDue_populateOperatorSelector()
        {
            _operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.GreaterThan.ToString()), FilterOperator.GreaterThan.ToString()));
            //_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.Between.ToString()), FilterOperator.Between.ToString()));
            _operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.LessThan.ToString()), FilterOperator.LessThan.ToString()));
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

		public override string FieldName 
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
                if (_fd.FieldId == "OverDuelistRowId")
                {
                    if (_conditionText.SelectedItem.Text == "1 Week")
                        return "7";
                    else if (_conditionText.SelectedItem.Text == "2 Weeks")
                        return "14";
                    else if (_conditionText.SelectedItem.Text == "1 Month")
                        return "30";
                    else if (_conditionText.SelectedItem.Text == "3 Months")
                        return "91";
                    else if (_conditionText.SelectedItem.Text == "6 Months")
                        return "182";
                    else if (_conditionText.SelectedItem.Text == "1 Year")
                        return "365";
                    else if (_conditionText.SelectedItem.Text == "2 Years")
                        return "730";
                    else
                        return "0";
                }
                else
                {
                    //if (ConditionOperator == "like")
                    //    return "'%" + _conditionText.SelectedItem.Text + "%'"; 
                    //return "'" + _conditionText.SelectedItem.Text + "'"; 

                    if (ConditionOperator == "like")
                        return "'%" + _conditionText.SelectedItem.Value + "%'";
                    return _conditionText.SelectedItem.Value;                    
                }
			}
		}

        public override string ConditionText
        {
            get { return _conditionText.SelectedItem.Text;}
        }

		public override string ConditionOperator 
		{
			get {return _operatorSelector.SelectedValue.ToString();}
		}

		public override void SetDefaultFilter()
		{
			//_operatorSelector.Items.Add();
			foreach (ListItem item in _operatorSelector.Items)
				if (item.Value == _filterOperator) 
				{
                    _operatorSelector.ClearSelection();
                    
                    item.Selected = true;
					conditionCheckBox.Checked = true;
                }
                else item.Selected = false;


			foreach (ListItem item in _conditionText.Items)
				if (item.Text == _filterCondition) 
				{
                    _conditionText.ClearSelection();

					item.Selected = true;
					conditionCheckBox.Checked = true;
                }
                else item.Selected = false;

			return;
		}

		public override void SetDefaultFilter(string FilterOperator, string FilterCondition)
		{
			//_operatorSelector.Items.Add();
			foreach (ListItem item in _operatorSelector.Items)
				if (item.Value.ToUpper() == FilterOperator.ToUpper())
				{
					item.Selected = true;
					conditionCheckBox.Checked = true;
                }
                else item.Selected = false;

			foreach (ListItem item in _conditionText.Items)
				if (item.Text.ToUpper() == FilterCondition.ToUpper())
				{
					item.Selected = true;
					conditionCheckBox.Checked = true;
                }
                else item.Selected = false;

			return;
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
