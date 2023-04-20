using System;
using System.Web.UI.WebControls;
using GASystem.GUIUtils;
using System.Web.UI;
using GASystem.AppUtils;
using System.Web;

namespace GASystem.GUIUtils.DataFilter.EditElements
{
	/// <summary>
	/// Edit controls for setting filters on checkbox elements in lists
	/// </summary>
	public class CheckboxEditWebControl :IEditWebControl, System.Web.UI.INamingContainer
	{
		CheckBox conditionCheckBox;
		Label _fieldName;
		Literal _containsText;
		DropDownList _operatorSelector;
		FieldDescription _fd;
		private string _filterOperator;

		
		public CheckboxEditWebControl(FieldDescription fd)
		{
			//
			// TODO: Add constructor logic here
			//
			conditionCheckBox = new CheckBox();
			_fieldName = new Label();
			_containsText = new Literal();
			_operatorSelector = new DropDownList();
			_fd = fd;
            this.FieldName = fd.TableId + "." + fd.FieldId;
			//_filterOperator = fd.FilterOperator;
            bool useSessionFilter = false;

            if (HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-usesessionfilter"] != null)
                useSessionFilter = (bool)HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-usesessionfilter"];

            if (useSessionFilter)
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-filteroperator"] != null)
                    _filterOperator = HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-filteroperator"].ToString();
                else
                    _filterOperator = string.Empty;

            }
            else
            {
                _filterOperator = fd.FilterOperator;
            }



			_fieldName.Text = GASystem.AppUtils.Localization.GetCaptionText(_fd.DataType);
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
			_operatorSelector.Attributes.Add("onFocus", "document.getElementById('" + conditionCheckBox.ClientID +  "').checked = true;");//"document.getElementById(" + conditionCheckBox.ClientID +  ").checked = true;");

			this.Controls.Add(_operatorSelector);
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));
			
			//dummy element
			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Tr));

			
		}

		private void populateOperatorSelector() 
		{
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.isChecked.ToString()), FilterOperator.isChecked.ToString()));  //" = true"));
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.isNotChecked.ToString()), FilterOperator.isNotChecked.ToString()));  //" = false"));
			
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
		}

		public override string FieldNameDisplay
		{
			get { return _fieldName.Text; }
		}


		/// <summary>
		/// Get condition string for this element
		/// </summary>
		public override string Condition
		{
			get 
			{
				return string.Empty;
				
			}
		}

        public override string ConditionText
        {
            get { return string.Empty; }
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
        }

        public override void ResetToDefaulFilter()
        {
            _filterOperator = _fd.FilterOperator;
            this.enabledElement = false;
            SetDefaultFilter();
        }
		
	}
}
