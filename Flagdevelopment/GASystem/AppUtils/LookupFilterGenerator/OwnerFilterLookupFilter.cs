using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;


namespace GASystem.AppUtils.LookupFilterGenerator
{
	/// <summary>
	/// Summary description for OwnerFilterLookupFilter.
	/// </summary>
	public class OwnerFilterLookupFilter : GeneralLookupFilter
	{
		public OwnerFilterLookupFilter(GADataClass LookupDataClass, GADataRecord LookupOwner, string LookupFilter)
		{
			string filterbase = "({0} = '{1}' or {0} is null)";
			
			//ignore and use default values if LookupOwner is null
			if (LookupOwner == null)
				return;
			
			LookupFilter = this.GetLookupfilterFilterPart(LookupFilter);

			GADataRecord OwnerOfOwner = DataClassRelations.GetOwner(LookupOwner);

			if (LookupFilter != string.Empty) {
				Filter = string.Format(filterbase, LookupFilter, LookupOwner.DataClass.ToString());
			}
	

		}


	}
}
