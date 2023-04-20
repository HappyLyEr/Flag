using System;
using System.Web.UI.WebControls;
using GASystem.AppUtils;
using GASystem.GUIUtils;
using System.Web.UI;
using System.Web;

namespace GASystem.GUIUtils.DataFilter.EditElements
{
	/// <summary>
	/// Controls for editing filters on general textboxes
	/// </summary>
	public class GeneralEditWebControl :IEditWebControl, System.Web.UI.INamingContainer
	{
		protected CheckBox conditionCheckBox;
        protected Label _fieldName;
        protected Literal _containsText;
        protected TextBox _conditionText;
        protected DropDownList _operatorSelector;

        private string _filterOperator;
        private string _filterCondition;

        private FieldDescription _fd;
		
		public GeneralEditWebControl(FieldDescription fd)
		{
            _fd = fd;
            //
			// TODO: Add constructor logic here
			//
			conditionCheckBox = new CheckBox();
			_fieldName = new Label();
			_containsText = new Literal();
			_conditionText = new TextBox();
            _conditionText.CssClass = "FilterConditionText";
            _conditionText.ID = fd.FieldId.Replace(" ", string.Empty) + "conditiontext";
			_operatorSelector = new DropDownList();

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
            } else 
            {
                _filterOperator = fd.FilterOperator;
                _filterCondition = fd.FilterCondition;
            }


			_fieldName.Text =  GASystem.AppUtils.Localization.GetCaptionText(fd.DataType);
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
			populateOperatorSelector();
			this.Controls.Add(_operatorSelector);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			_conditionText.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID +  "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");
			
			this.Controls.Add(_conditionText);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Tr));

			
		}

		private void populateOperatorSelector() 
		{
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.Contains.ToString()), FilterOperator.Contains.ToString())); //"like"));
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.Equal.ToString()), FilterOperator.Equal.ToString()));  // "="));
			
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
		}

		public override string FieldNameDisplay
		{
			get {  return _fieldName.Text; }
		}

		public override string Condition
		{
			get 
			{
				if (ConditionOperator == FilterOperator.Contains.ToString())
					return "'%" + _conditionText.Text + "%'"; 
				return "'" + _conditionText.Text + "'"; 
			}
		}

        public override string ConditionText
        {
            get { return _conditionText.Text; }
        }

		public override  string ConditionOperator 
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
