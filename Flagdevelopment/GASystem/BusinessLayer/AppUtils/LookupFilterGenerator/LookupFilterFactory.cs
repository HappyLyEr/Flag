using System;
using GASystem.DataModel;
using GASystem.BusinessLayer;


namespace GASystem.AppUtils.LookupFilterGenerator
{
	
	/// <summary>
	/// Summary description for LookupFilterFactory.
	/// </summary>
	public class LookupFilterFactory
	{
        // Tor 20140204 added siblingfilteralllevels and current to: public enum LookupFilterType {general, owner, ownerfilter, siblingfilter, personnelLogin, followupaction}
        // Tor 20140829 added currentInLookupTable 
        // Tor 20180905 added dropdownfilter 
        public enum LookupFilterType 
        {
            general,
            literal,
            owner, 
            ownerfilter, 
            siblingfilter, 
            siblingfilteralllevels,
            siblingfiltercommonlocation,
            personnelLogin, 
            followupaction, 
            current, 
            currentInLookupTable, 
            dropdownfilter
        }

		public LookupFilterFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        /// <summary>
        /// Create an instance of lookupfilter. Uses dataClass and field to find and create the correct instance.
        /// </summary>
        /// <param name="dataClass">GADataClass to get lookupfilter for</param>
        /// <param name="field">Field in the table referenced by dataClass to get lookupfilter for</param>
        /// <param name="owner">Owner datarecord to generate filter for</param>
        /// <returns></returns>
		public static ILookupFilter Make(GADataClass dataClass, string field, GADataRecord owner)
		{
			string lookupFilter = getLookupFilterFromFieldDefinition(dataClass, field);

            return Make(dataClass, field, owner, lookupFilter);

		}

        /// <summary>
        /// Create an instance of lookupfilter. Uses dataClass and field to find and create the correct instance.
        /// </summary>
        /// <param name="dataClass">GADataClass to get lookupfilter for</param>
        /// <param name="field">Field in the table referenced by dataClass to get lookupfilter for</param>
        /// <param name="owner">Owner datarecord to generate filter for</param>
        /// <param name="lookupFilter">String specifing the lookupfilter to use</param>
        /// <returns></returns>
        public static ILookupFilter Make(GADataClass dataClass, string field, GADataRecord owner, string lookupFilter)
        {
           if (lookupFilter == string.Empty)
                return new GeneralLookupFilter(dataClass, owner, lookupFilter);

            switch (getLookupFilterType(lookupFilter))
            {
                case LookupFilterType.owner:
                    return new SpecificOwnerLookupFilter(dataClass, owner, lookupFilter);
                case LookupFilterType.ownerfilter:
                    return new OwnerFilterLookupFilter(dataClass, owner, lookupFilter);
                case LookupFilterType.followupaction:
                    return new FollowUpActionLookupFilter(dataClass, owner, lookupFilter);
                case LookupFilterType.siblingfilter:
                    return new SiblingLookupFilter(dataClass, owner, lookupFilter, field, true);
                // Tor 2013121: Added siblingfilteralllevels 
                case LookupFilterType.siblingfilteralllevels:
                    return new SiblingLookupFilter(dataClass, owner, lookupFilter, field,"AllLevels");
                case LookupFilterType.siblingfiltercommonlocation:
                    return new SiblingLookupFilter(dataClass, owner, lookupFilter, field, false);
                // Tor 20140204: Added current
                case LookupFilterType.current:
                    return new CurrentLookupFilter(dataClass, owner, lookupFilter, field);
                // Tor 20140829: Added currentInLookupTable
                case LookupFilterType.currentInLookupTable:
                    return new CurrentInLookupTableLookupFilter(dataClass, owner, lookupFilter, field);
                // Tor 20180905: Added dropdownfilter
                case LookupFilterType.dropdownfilter:
                    return new DropdownFilter(dataClass, owner, lookupFilter, field);
                case LookupFilterType.personnelLogin:
                    return new PersonnelLoginFilter();
                case LookupFilterType.literal:
                    return new LiteralLookupFilter(dataClass, owner, lookupFilter);
                default:
                    return new GeneralLookupFilter(dataClass, owner, lookupFilter);
            }
        }

		public static LookupFilterType getLookupFilterType(string lookupFilter) 
		{
			foreach (LookupFilterType fType in System.Enum.GetValues(typeof(LookupFilterType)))
			{
				if (IsLookupStringOfType(fType, lookupFilter))
					return fType;
			}
			return LookupFilterType.general;
		}

		public static bool IsLookupStringOfType(LookupFilterType filterType, string lookupFilter) 
		{
			return lookupFilter.ToUpper().StartsWith(filterType.ToString().ToUpper()+":");
		}

		/// <summary>
		/// Get lookup filter from fielddefinition for specified class and field
		/// </summary>
		/// <param name="DataClass"></param>
		/// <returns></returns>
		private static string getLookupFilterFromFieldDefinition(GADataClass DataClass, string OwnerField)
		{
			AppUtils.FieldDescription fd;
			try 
			{
				fd = AppUtils.FieldDefintion.GetFieldDescription(OwnerField, DataClass.ToString());
			} 
			catch 
			{
				//error getting field definition. Incorrect class field ? return empty filter def.
				return string.Empty;
			}
//			return lookupFilter;
			return fd.LookupFilter;
		}

		
	}
}
