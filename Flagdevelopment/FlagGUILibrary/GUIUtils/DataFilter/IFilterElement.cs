using System;


namespace GASystem.GUIUtils.DataFilter
{
// tor 20150211 added filter operator DateEqualTo
    public enum FilterOperator { isChecked, isNotChecked, GreaterThan, LessThan, Contains, Equal, NotIncluding, isCurrent, inPeriod, Between, DateEqualTo }
	/// <summary>
	/// Summary description for IFilterElement.
	/// </summary>
	public abstract class IFilterElement
	{
		//GASystem.AppUtils.FieldDescription fd;
		
		public IFilterElement(GASystem.AppUtils.FieldDescription fd)
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public IFilterElement() 
		{
		}

		public abstract string GetFilterString();

		public abstract ViewElements.IViewWebControl ViewWebControl 
		{
			get;
		
		}
		
		public abstract EditElements.IEditWebControl EditWebControl 
		{
			get;
			
		}

		public abstract void GenerateFilterString();

		
		
	}
}
