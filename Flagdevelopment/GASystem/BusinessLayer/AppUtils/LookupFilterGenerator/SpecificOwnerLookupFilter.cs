using System;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace GASystem.AppUtils.LookupFilterGenerator
{
	/// <summary>
	/// Summary description for SpecificOwnerLookupFilter.
	/// </summary>
	public class SpecificOwnerLookupFilter : GeneralLookupFilter
	{
		
		/// <summary>
		/// Constructor for setting specific owner based on data from field definition. A owner is passed to the 
		/// constructor in the format owner-ownerid. Data from fielddefinition is retrived by the factory method 
		/// creating this class.
		/// </summary>
		/// <param name="LookupDataClass"></param>
		/// <param name="LookupOwner"></param>
		/// <param name="LookupFilter">lookup filter from fielddefinition in format owner-ownerid</param>
		public SpecificOwnerLookupFilter(GADataClass LookupDataClass, GADataRecord LookupOwner, string LookupFilter)
		{
			//AppUtils.FieldDescription fd = 
			LookupFilter = this.GetLookupfilterFilterPart(LookupFilter);

			if (LookupFilter != string.Empty)
			{
				int splitId = LookupFilter.IndexOf("-");
				if (splitId != -1) 
				{
					string ownerClass = LookupFilter.Substring(0, splitId);
					int ownerId = int.Parse(LookupFilter.Substring(splitId+1));
					Owner = new GADataRecord(ownerId, GADataRecord.ParseGADataClass(ownerClass));
				}
			}
			Filter = string.Empty;
		}
	}
}
