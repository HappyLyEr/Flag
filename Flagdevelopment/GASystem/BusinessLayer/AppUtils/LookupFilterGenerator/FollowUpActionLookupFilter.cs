using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;


namespace GASystem.AppUtils.LookupFilterGenerator
{
	/// <summary>
	/// Summary description for OwnerFilterLookupFilter.
	/// </summary>
	public class FollowUpActionLookupFilter : GeneralLookupFilter
	{
        public FollowUpActionLookupFilter(GADataClass LookupDataClass, GADataRecord LookupOwner, string LookupFilter)
		{
			string filterbase = "( {0} is null)";
			
			//ignore and use default values if LookupOwner is null
			if (LookupOwner == null)
				return;
			
			LookupFilter = this.GetLookupfilterFilterPart(LookupFilter);

			GADataRecord OwnerOfOwner = DataClassRelations.GetOwner(LookupOwner);

			if (LookupFilter != string.Empty) {
				Filter = string.Format(filterbase, LookupFilter);
			}
	

		}


	}
}
