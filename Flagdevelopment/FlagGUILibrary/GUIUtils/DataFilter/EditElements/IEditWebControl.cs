using System;
using System.Web;

namespace GASystem.GUIUtils.DataFilter.EditElements
{
	/// <summary>
	/// Summary description for IEditWebControl.
	/// </summary>
	public abstract class IEditWebControl : System.Web.UI.WebControls.WebControl
	{
		protected const string ELEMENTCSSCLASS = "FilterContentCell";
		



//		public IEditWebControl() 
//		{
//			//
//			// TODO: Add constructor logic here
//			//
//		}


		public abstract bool enabledElement
		{
			get;
			set;
		}

		/// <summary>
		/// Set default filter on element if defined on field definition
		/// </summary>
		public abstract void SetDefaultFilter();

		/// <summary>
		/// Specify filter: TODO implement in all classes, make abstract
		/// </summary>
		/// <param name="FilterCondition"></param>
        public abstract void SetDefaultFilter(string FilterOperator, string FilterCondition);



        public abstract void ResetToDefaulFilter();
		/// <summary>
		/// Attribute for related field name
		/// </summary>
		public abstract string FieldName 
		{
			set;
			get;
		}
		
		/// <summary>
		/// Attribute for related field name
		/// </summary>
		public abstract string FieldNameDisplay 
		{
			get;
		}

		
		
		public abstract string ConditionOperator
		{
			get;
		}

		public abstract string Condition 
		{
			get;
		}

        public abstract string ConditionText
        {
            get;
        }

        public virtual string ConditionDisplay
        {
            get {return Condition;}
        }

		public FilterOperator GetFilterOperator() 
		{
			try 
			{
				return (FilterOperator) Enum.Parse(typeof(FilterOperator), this.ConditionOperator, true);
			} 
			catch (Exception ex)
			{
				throw new GAExceptions.GAException("Error getting filter operator", ex);
			}
		}

	}
}
