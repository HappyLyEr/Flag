using System;
using System.Web.UI.WebControls;
using GASystem.AppUtils;
using GASystem.GUIUtils;
using System.Web.UI;
using System.Web;

namespace GASystem.GUIUtils.DataFilter.EditElements
{
	/// <summary>
	/// Summary description for GeneralEditWebControl.
	/// </summary>
	public class NumericEditWebControl :IEditWebControl, System.Web.UI.INamingContainer
	{
		CheckBox conditionCheckBox;
		Label _fieldName;
		Literal _containsText;
		TextBox _conditionText;
		DropDownList _operatorSelector;
		CompareValidator compVal;


        private string _filterOperator;
        private string _filterCondition;
        private FieldDescription _fd;
		
		public NumericEditWebControl(FieldDescription fd)
		{
			//
			// TODO: Add constructor logic here
			//
            _fd = fd;
			conditionCheckBox = new CheckBox();
			_fieldName = new Label();
			_containsText = new Literal();
			_conditionText = new TextBox();
            _conditionText.CssClass = "FilterConditionText";
			_operatorSelector = new DropDownList();
      //      _filterOperator = fd.FilterOperator;
      //      _filterCondition = fd.FilterCondition;

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

			 compVal = new CompareValidator();
			_fieldName.Text = GASystem.AppUtils.Localization.GetCaptionText(fd.DataType);
            this.FieldName = fd.TableId + "." + fd.FieldId;
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Tr));
			
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			this.Controls.Add(conditionCheckBox);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));
			
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			this.Controls.Add(_fieldName);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));
			
		
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			//this.Controls.Add(_containsText);
			populateOperatorSelector();
			this.Controls.Add(_operatorSelector);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			_conditionText.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID +  "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");
		
			this.Controls.Add(_conditionText);
			_conditionText.ID = this.FieldName + "condition";
			compVal.ControlToValidate = _conditionText.ID;
			compVal.Operator = ValidationCompareOperator.DataTypeCheck;
			compVal.Type = ValidationDataType.Integer;
			compVal.ErrorMessage ="<br/>" +  string.Format(GASystem.AppUtils.Localization.GetErrorText("InvalidFieldDataType"), "numeric");
			compVal.Display  = ValidatorDisplay.Dynamic;
			this.Controls.Add(compVal);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Tr));
		
		}

		private void populateOperatorSelector() 
		{
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.Equal.ToString()), FilterOperator.Equal.ToString()));  // "="));
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.GreaterThan.ToString()), FilterOperator.GreaterThan.ToString()));  // ">"));
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.LessThan.ToString()), FilterOperator.LessThan.ToString()));  // "<"));
	
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
				//_fieldName.Text = GASystem.AppUtils.Localization.GetCaptionText(value);
		
			}
			
			//set {_fieldName.Text = value;}
			//get {return _fieldName.Text;}
		}

		public override string FieldNameDisplay
		{
			get {  return _fieldName.Text; }
		}

		public override string Condition
		{
			get 
			{
				return _conditionText.Text;
			}
		}

        public override string ConditionText
        {
            get { return _conditionText.Text; }
        }

		public override string ConditionOperator 
		{
			get {return _operatorSelector.SelectedValue.ToString();}
		}

        public override void SetDefaultFilter()
        {
            foreach (ListItem item in _operatorSelector.Items)
                if (item.Value == _filterOperator)
                {
                    item.Selected = true;
                    conditionCheckBox.Checked = true;
                }
                else item.Selected = false;


            _conditionText.Text = _filterCondition;
        }

        public override void SetDefaultFilter(string FilterOperator, string FilterCondition)
        {
            foreach (ListItem item in _operatorSelector.Items)
                if (item.Value == FilterOperator)
                {
                    item.Selected = true;
                    conditionCheckBox.Checked = true;
                }
                else item.Selected = false;


            _conditionText.Text = FilterCondition;

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
