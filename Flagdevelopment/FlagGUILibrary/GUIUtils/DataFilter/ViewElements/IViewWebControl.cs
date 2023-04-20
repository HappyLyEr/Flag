using System;
using System.Web;

namespace GASystem.GUIUtils.DataFilter.ViewElements
{
	/// <summary>
	/// Summary description for IViewWebControl.
	/// </summary>
	public abstract class IViewWebControl : System.Web.UI.WebControls.WebControl, System.Web.UI.INamingContainer
	{
		public IViewWebControl()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public abstract string FilterString
		{
			get;
			set;
		}

     
		/// <summary>
		/// generate the sql statement for this element
		/// </summary>
		/// <param name="FieldName"></param>
		/// <param name="Operator"></param>
		/// <param name="Condition"></param>
		public abstract void GenerateFilterString(string FieldName, FilterOperator Operator, string Condition ); 

		/// <summary>
		/// Generate a human friendly string for displaying the sql statement for this element
		/// </summary>
		/// <param name="FieldNameDisplay"></param>
		/// <param name="Operator"></param>
		/// <param name="Condition"></param>
		public abstract void GenerateDispalyString(string FieldNameDisplay, FilterOperator Operator, string Condition ); 

		public abstract void Reset();

		public bool enabledElement
		{
			get {return null==ViewState["enabledElement"+this.ID] ? false : (bool) ViewState["enabledElement"+this.ID];}
			set {ViewState["enabledElement"+this.ID] = value;}
		}

        public abstract void SetSession(bool enabled, FilterOperator Operator, string Condition);

	}
}
