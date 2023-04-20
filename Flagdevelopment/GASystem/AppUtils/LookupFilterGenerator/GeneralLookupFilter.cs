using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace GASystem.AppUtils.LookupFilterGenerator
{
	/// <summary>
	/// General class for getting owner and filter to be used for lookup fields.
	/// Takes a defult owner and filter in the construtors. Returns these values as is
	/// </summary>
	public class GeneralLookupFilter : ILookupFilter
	{
		private GADataRecord _owner = null;
		private string _filter = string.Empty;


		public GeneralLookupFilter(GADataClass LookupDataClass, GADataRecord LookupOwner, string LookupFilter)
		{
			Owner = LookupOwner;
			//Filter = LookupFilter;
		}

		/// <summary>
		/// default constructor. Should not be generally used
		/// </summary>
		public GeneralLookupFilter()
		{

		}

		public virtual GADataRecord Owner 
		{
			set {_owner = value;}
			get {return _owner;}
		}

		public virtual string Filter 
		{
			set {_filter = value;}
			get {return _filter;}
		}
		
		public virtual string FilterDescription 
		{
			get {return string.Empty;}
		}

		public virtual bool CanDisableFilter 
		{
			get {return false;}
		}



		protected string GetLookupfilterFilterPart(string LookupFilter) 
		{
			int splitIndex = LookupFilter.IndexOf(":");
			if (splitIndex > -1) 
				return LookupFilter.Substring(splitIndex+1);
			return LookupFilter;
		}
	}
}
