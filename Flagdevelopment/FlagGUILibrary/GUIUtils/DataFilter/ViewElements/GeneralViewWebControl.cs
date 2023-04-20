using System;
using System.Web.UI;
using GASystem.AppUtils;

namespace GASystem.GUIUtils.DataFilter.ViewElements
{
	/// <summary>
	/// General control for displaying the filterstring. //TODO refactor sql string generating to a seperate class in bussiness layer
	/// </summary>
	public class GeneralViewWebControl : IViewWebControl
	{
		System.Web.UI.WebControls.Label sqlLabel;
		System.Web.UI.WebControls.Literal br;

        private string _tablename;

        public string TableName
        {
            get { return _tablename; }
            set { _tablename = value; }
        }

        private string _fdfieldname;

        public string FdFieldName
        {
            get { return _fdfieldname; }
            set { _fdfieldname = value; }
        }

        private string _fieldname;

        public string FieldName
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }

        public GeneralViewWebControl(FieldDescription fd)
		{
			//
			// TODO: Add constructor logic here
			//
			sqlLabel = new System.Web.UI.WebControls.Label();
			br = new System.Web.UI.WebControls.Literal();
			sqlLabel.Visible = false;
			br.Text = "<br/>";
			br.Visible = false;

            this.TableName = fd.TableId;
            this.FdFieldName = fd.FieldId;
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			
			this.Controls.Add(sqlLabel);
			this.Controls.Add(br);
		}

		public string DisplayString
		{
			get
			{
				return sqlLabel.Text;
			}
			set
			{
				sqlLabel.Text = generateConditionDisplayString(value);
				if (value != string.Empty) 
				{
					br.Visible = true;
					sqlLabel.Visible = true;
                    this.Visible = true;
				} 
				else 
				{
					br.Visible = false;
					sqlLabel.Visible = false;
                    this.Visible = false;
				}
			}
		}

        //public virtual string FieldName 
        //{
        //    get {return null==ViewState["FieldName"+this.ID] ? string.Empty :  ViewState["FieldName"+this.ID].ToString();}
        //    set {ViewState["FieldName"+this.ID] = value;}
        //}

		public virtual string Condition 
		{
			get {return null==ViewState["Condition"+this.ID] ? string.Empty : ViewState["Condition"+this.ID].ToString();}
			set {ViewState["Condition"+this.ID] = value;}
		}

		public virtual string ConditionOperator 
		{
			get {return null==ViewState["ConditionOperator"+this.ID] ? string.Empty : ViewState["ConditionOperator"+this.ID].ToString();}
			set {ViewState["ConditionOperator"+this.ID] = value;}
		}

		public override string FilterString 
		{
			get {return null==ViewState["FilterString"+this.ID] ? string.Empty :  ViewState["FilterString"+this.ID].ToString();}
			set {ViewState["FilterString"+this.ID] = value;}
		}

		/// <summary>
		/// Geneerate sql filter string for this element
		/// </summary>
		public virtual void GenerateFilterString() 
		{
		//	this.Page.Session[]
            
            
            FilterString = FieldName + " " + ConditionOperator + " " + Condition;
		}

		/// <summary>
		/// Geneerate sql filter string for this element
		/// </summary>
		/// <param name="FieldName"></param>
		/// <param name="Operator"></param>
		/// <param name="Condition"></param>
		public override void GenerateFilterString(string FieldName, FilterOperator Operator, string Condition )  
		{
			this.FieldName = FieldName;
			this.ConditionOperator = getSqlOperator(Operator);
			this.Condition = Condition;
			GenerateFilterString();
		}

		/// <summary>
		/// Generate a human friendly display string based on conditon and operator
		/// </summary>
		public override void GenerateDispalyString(string FieldNameDisplay, FilterOperator Operator, string Condition ) 
		{
			DisplayString = FieldNameDisplay + " " + GASystem.AppUtils.Localization.GetGuiElementText(Operator.ToString()).ToLower()  + " " + Condition;
		}

        public override void SetSession(bool enabled, FilterOperator Operator, string Condition)
        {
            this.Page.Session[_tablename + "-" + this.FdFieldName + "-usesessionfilter"] = true;
            if (enabled)
            {
                this.Page.Session[_tablename + "-" + this.FdFieldName + "-filteroperator"] = Operator;
                this.Page.Session[_tablename + "-" + this.FdFieldName + "-filtercondition"] = Condition;
            }
            else
            {
                this.Page.Session[_tablename + "-" + this.FdFieldName + "-filteroperator"] = null;
                this.Page.Session[_tablename + "-" + this.FdFieldName + "-filtercondition"] = null;
            }
        }

	
		/// <summary>
		/// Reset all variables and set enabledelement to false
		/// </summary>
		public override void Reset() 
		{
			FieldName = string.Empty;
			Condition = string.Empty;
			ConditionOperator = string.Empty;
			FilterString = string.Empty;
			DisplayString = string.Empty;
			enabledElement = false;
		}

		
		/// <summary>
		/// Get sgl operator based on FilterOperator enum
		/// </summary>
		/// <param name="Operator"></param>
		/// <returns></returns>
		protected virtual string getSqlOperator(FilterOperator Operator) 
		{
			if (Operator == FilterOperator.Contains)
				return " like ";
			if (Operator == FilterOperator.Equal)
				return " = ";
			if (Operator == FilterOperator.GreaterThan)
				return " > ";
			if (Operator == FilterOperator.isChecked)
				return " = 1 ";
			if (Operator == FilterOperator.isNotChecked)
				return " = 0 ";
			if (Operator == FilterOperator.LessThan)
				return " < ";
			if (Operator == FilterOperator.NotIncluding)
				return " != ";
            if (Operator == FilterOperator.Between)
                return " BETWEEN ";
            // tor 20150211 added operator DateEqualTo : set to between date:00:00:000 and date: 23:59:59.999 
            if (Operator == FilterOperator.DateEqualTo)
                return " BETWEEN ";

			throw new GAExceptions.GAException("Invalid filter operator");
		}

		/// <summary>
		/// Generate display friendly string of condition
		/// </summary>
		/// <param name="Condition"></param>
		/// <returns></returns>
		private string generateConditionDisplayString(string Condition) 
		{
			//currently we are only removing the % sign
			return Condition.Replace("%", string.Empty);
		}
     }
}
