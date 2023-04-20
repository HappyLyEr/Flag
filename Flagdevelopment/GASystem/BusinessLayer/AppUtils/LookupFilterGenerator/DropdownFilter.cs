using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.DataAccess;


namespace GASystem.AppUtils.LookupFilterGenerator
{
    /// <summary>
    /// Specific filter for filtering on dropdownlist in lookuptable
    /// </summary>
    class DropdownFilter : GeneralLookupFilter
    {
        const string _DROPDOWN_WHERE_STATEMENTBASE = @"{0} in (select {0} from {1} where {2} in 
(select l.ListsRowId from GALists l where l.GAListCategory=N'{3}' and l.GAListValue {4} in ({5})))";
        //{0} = <_fieldnameInLookupTable> LookupTableKey
        //{1} = <_fieldnameInLookupTable> LookupTable
        //{2} = <_fieldnameInLookupTable> Dropdownlist field in LookupTable
        //{3} = <_GAFieldListCategory> listCategory for dropdownlistfield in lookuptable
            //{4} = <_notIn> not or blank
            //{5} = <_GAListValueList> GAListValues 
            //dropdownfilter:in-fieldnameInLookupTable-'Cold','CSEntry'
            //dropdownfilter:notin-fieldnameInLookupTable-'Cold','CSEntry'

		string _dropdownFilter;
        string _GAFieldListCategory;
        string _notIn;
        string _fieldnameInLookupTable;
        string _GAListValueList;
		FieldDescription _dropdownFieldFd = null;
        FieldDescription _lookupFieldFd = null;

        public DropdownFilter(GADataClass dataClass, GADataRecord owner, string lookupFilter, string field)
		{
            try
            {
                string dc = owner.DataClass.ToString();

                //parse dropdownfilter setting to get all parameters;
                _dropdownFilter = this.GetLookupfilterFilterPart(lookupFilter);
                parseDropdownFilter(dataClass,field);
                // get lookuptable field
                _dropdownFieldFd = FieldDefintion.GetFieldDescription(_fieldnameInLookupTable, owner.DataClass.ToString());
                _GAFieldListCategory = _dropdownFieldFd.ListCategory;
                string filter = string.Format(_DROPDOWN_WHERE_STATEMENTBASE  
                    ,field
                    ,owner.DataClass.ToString()
                    , _fieldnameInLookupTable 
                    , _GAFieldListCategory
                    , _notIn
                    , _GAListValueList
                    );
                Filter = filter;
            }
            catch (Exception ex)
            {
                //there was an error parsing the string do not set a filter. Will display all records
                Filter = string.Empty;
            }
        }

        /// <summary>
        /// Parse filter definition. 
        /// </summary>
        private void parseDropdownFilter(GADataClass dataclass, string field)
        {
            string[] dropdownFilterElements = _dropdownFilter.Split('-');

            if (dropdownFilterElements.Length != 3)
                throw new GAExceptions.GAException("Invalid dropdown filter definition in "+dataclass.ToString()+" where LookupTableKey="+field+" ");
 
            _notIn=dropdownFilterElements[0];
            if (_notIn.ToLower()=="notin")
            {_notIn="not";}
            else
            {_notIn = "";};
  
            _fieldnameInLookupTable = dropdownFilterElements[1];
            
            _GAListValueList = dropdownFilterElements[2];
            
            return;
        }

    }
}
