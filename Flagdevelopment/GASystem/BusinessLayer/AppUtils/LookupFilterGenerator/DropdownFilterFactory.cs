using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataModel;
using GASystem.BusinessLayer;

namespace GASystem.AppUtils.LookupFilterGenerator
{
    public class DropdownFilterFactory
    {
                // Tor 20131216 added siblingfilteralllevels to: public enum LookupFilterType {general, owner, ownerfilter, siblingfilter, personnelLogin, followupaction}
        public enum DropdownFilterType { general, owner, ownerfilter, siblingfilter, siblingfilteralllevels, personnelLogin, followupaction }

        public DropdownFilterFactory()
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
                case DropdownFilterType.owner:
                    return new SpecificOwnerLookupFilter(dataClass, owner, lookupFilter);
                case DropdownFilterType.ownerfilter:
                    return new OwnerFilterLookupFilter(dataClass, owner, lookupFilter);
                case DropdownFilterType.followupaction:
                    return new FollowUpActionLookupFilter(dataClass, owner, lookupFilter);
                case DropdownFilterType.siblingfilter:
                    return new SiblingLookupFilter(dataClass, owner, lookupFilter, field, true);
                // Tor 2013121: Added siblingfilteralllevels 
                case DropdownFilterType.siblingfilteralllevels:
                    return new SiblingLookupFilter(dataClass, owner, lookupFilter, field, "AllLevels");
                case DropdownFilterType.personnelLogin:
                    return new PersonnelLoginFilter();
                default:
                    return new GeneralLookupFilter(dataClass, owner, lookupFilter);
            }
        }

        public static DropdownFilterType getLookupFilterType(string lookupFilter)
        {
            foreach (DropdownFilterType fType in System.Enum.GetValues(typeof(DropdownFilterType)))
            {
                if (IsLookupStringOfType(fType, lookupFilter))
                    return fType;
            }
            return DropdownFilterType.general;
        }

        public static bool IsLookupStringOfType(DropdownFilterType filterType, string lookupFilter)
        {
            return lookupFilter.ToUpper().StartsWith(filterType.ToString().ToUpper() + ":");
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
