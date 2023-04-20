using System;
using System.Web.UI.WebControls;
using GASystem.GUIUtils;
using System.Web.UI;
using System.Collections;
using GASystem.AppUtils;
using GASystem.DataModel;
using Telerik.WebControls;
using GASystem.BusinessLayer;
using System.Data;
using System.Web;

namespace GASystem.GUIUtils.DataFilter.EditElements
{
	/// <summary>
	/// Summary description for GeneralEditWebControl.
	/// </summary>
	public class WorkitemResponsibleEditWebControl :IEditWebControl, System.Web.UI.INamingContainer
	{
		CheckBox conditionCheckBox;
		Label _fieldName;

		Literal _containsText;
		RadComboBox _conditionText;
		
		DropDownList _operatorSelector;

		private string _filterOperator;
		private string _filterCondition;

        private FieldDescription _fd;

        public WorkitemResponsibleEditWebControl(FieldDescription fd)
		{
			//
			// TODO: Add constructor logic here
			//
            _fd = fd;
			conditionCheckBox = new CheckBox();
			_fieldName = new Label();
			_containsText = new Literal();
			_conditionText = new RadComboBox();
            _conditionText.ID = fd.FieldId + "workitemresponsibledropdown";
            _conditionText.Skin = "FlagCombo";
			_operatorSelector = new DropDownList();
            this.FieldName = fd.TableId + "." + fd.FieldId;
			this.ListCategory = fd.ListCategory;
			//_filterOperator = fd.FilterOperator;
			//_filterCondition = fd.FilterCondition;

            if (HttpContext.Current.Session != null && HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-filteroperator"] != null)
                _filterOperator = HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-filteroperator"].ToString();
            else
                _filterOperator = fd.FilterOperator;


            if (HttpContext.Current.Session != null && HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-filtercondition"] != null)
                _filterCondition = HttpContext.Current.Session[fd.TableId + "-" + fd.FieldId + "-filtercondition"].ToString();
            else
                _filterCondition = fd.FilterCondition;



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
			populateOperatorSelector();
			this.Controls.Add(_operatorSelector);
			_operatorSelector.ID = this.FieldName + "operatorSelector";
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateStartTag(HtmlTextWriterTag.Td, ELEMENTCSSCLASS));
			//ArrayList listItems = CodeTables.GetList(ListCategory, GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord);
			//CodeTables.BindCodeTable(_conditionText, listItems);
            _conditionText.OnClientFocus = "document.getElementById('" + conditionCheckBox.ClientID + "').checked = true;";
		
			this.Controls.Add(_conditionText);
            
			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Td));

			this.Controls.Add(HTMLLiteralTags.CreateEndTag(HtmlTextWriterTag.Tr));


            if (!this.Page.IsPostBack)
                GeneratePersonnelDropDown();
		}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
        }

		private void populateOperatorSelector() 
		{
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.Equal.ToString()), FilterOperator.Equal.ToString()));  // "="));
			_operatorSelector.Items.Add(new ListItem(GASystem.AppUtils.Localization.GetGuiElementText(FilterOperator.Contains.ToString()), FilterOperator.Contains.ToString()));  // "like %condition%"));
			
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


                if (ConditionOperator == FilterOperator.Contains.ToString())
                    return "'%;" + _conditionText.SelectedValue + ";%'";
                return "';" + _conditionText.SelectedValue + ";'";

			}
		}

        public override string ConditionText
        {
            get {
                if (_conditionText.SelectedItem == null)
                    return string.Empty; 
                return _conditionText.SelectedItem.Text;
            }
        }

        public override string ConditionDisplay
        {
            get {
                if (_conditionText.SelectedItem == null)
                    return null;
                return _conditionText.SelectedItem.Text; 
            
            }
        }

		public override string ConditionOperator 
		{
			get {return _operatorSelector.SelectedValue.ToString();}
		}

		public override void SetDefaultFilter()
		{

            SetDefaultFilter(_filterOperator, _filterCondition);
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


            foreach (Telerik.WebControls.RadComboBoxItem item in _conditionText.Items)
				if (item.Text.ToUpper() == FilterCondition.ToUpper())
				{
					item.Selected = true;
					conditionCheckBox.Checked = true;
                }
                else item.Selected = false;


			return;
		}

        /// <summary>
        /// Add person lookupfield control
        /// </summary>
        /// <param name="fieldDesc">Fielddescription</param>
        /// <returns></returns>
        public void GeneratePersonnelDropDown()
        {

            AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter = new AppUtils.LookupFilterGenerator.PersonnelLoginFilter();

            GASystem.DataModel.GADataClass myDataClass = GASystem.DataModel.GADataClass.GAPersonnel;

            //BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(myDataClass);

            //DataSet ds = bc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(new GADataRecord(1, GADataClass.GAFlag), System.DateTime.MinValue, System.DateTime.MaxValue, lookupFilter.Filter);


            DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassWithinOwner(myDataClass, new GADataRecord(1, GADataClass.GAFlag), lookupFilter.Filter, System.DateTime.MinValue, System.DateTime.MaxValue);

           ds.Tables[0].DefaultView.Sort = "familyname"; ;
         

            _conditionText.AllowCustomText = false;

            _conditionText.MarkFirstMatch = true;
            _conditionText.Height = new Unit(300, UnitType.Pixel);
          //  _conditionText.Sort = Telerik.WebControls.RadComboBoxSort.Ascending;
            string currentValueText = string.Empty;
            String[] displayTexts = { "familyname", "Givenname" };
            foreach (DataRowView row in ds.Tables[0].DefaultView) // ds.Tables[0].Rows)
            {
                string displayText = string.Empty;
                foreach (string aValue in displayTexts)
                {
                    displayText += row[aValue].ToString() + " ";
                }
                displayText = displayText.Trim();
                Telerik.WebControls.RadComboBoxItem item = new Telerik.WebControls.RadComboBoxItem(displayText, row[myDataClass.ToString().Substring(2)+"rowid"].ToString());
                _conditionText.Items.Add(item);

            }
            _conditionText.SelectedValue = currentValueText;

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
