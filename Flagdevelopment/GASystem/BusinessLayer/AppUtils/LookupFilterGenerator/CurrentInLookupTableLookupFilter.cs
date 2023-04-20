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
    /// Specific filter for filtering only current records (records with current from and to dates)
    /// </summary>
    class CurrentInLookupTableLookupFilter : GeneralLookupFilter
    {

        const string _CURRENT_WHERE_STATEMENTBASE = @"{0} in (select distinct e.{1} from {2} e where {3} )";
                        //{0} = <lookupclassrowidfield>
						//{1} = <siblingfield>
						//{2} = <siblingclass>
						//{3} = datefilter 

		string _lookupFilter;
        GADataClass _siblingClass;
        string _siblingField;
		FieldDescription _lookupFieldFd = null;

        // Tor 20140829 main parts of method below is copied from CurrentLookupFilter
        public CurrentInLookupTableLookupFilter(GADataClass dataClass, GADataRecord owner, string lookupFilter, string field)
		{
            try
            {
                //parse lookupfilter setting to get all parameters;
                _lookupFilter = this.GetLookupfilterFilterPart(lookupFilter);

                parseLookupFilter();
//                setSiblingOwner(LookupOwner, "CheckDates");
                string lookupClassRowIdField = dataClass.ToString().Substring(2) + "rowid";
                string siblingClassRowIdField = _siblingClass.ToString().Substring(2) + "rowid";
                _lookupFieldFd = FieldDefintion.GetFieldDescription(_siblingField,_siblingClass.ToString());

                //const string _CURRENT_WHERE_STATEMENTBASE = @"{0} in (select distinct e.{1} from {2} e where {3} )";
                ////{0} = <lookupclassrowidfield>
                ////{1} = <siblingfield>
                ////{2} = <siblingclass>
                ////{3} = datefilter 
                string filter = string.Format(_CURRENT_WHERE_STATEMENTBASE
                    ,field // Tor 20140909 replaces: , _lookupFieldFd.TableId.Substring(2) + "rowid"
                    , _siblingField
                    , _siblingClass.ToString()
                    , getDateFilter());
                Filter = filter;
            }

            catch (Exception ex)
            {
                //there was an error parsing the string do not set a filter. Will display all records
                Filter = string.Empty;
            }

            
		}

            /// <summary>
        /// Parse sibling filter definition. Definition must be of format siblingclass-siblingfield
        /// </summary>
        private void parseLookupFilter()
        {
            string[] siblingElements = _lookupFilter.Split('-');

            if (siblingElements.Length != 2)
                throw new GAExceptions.GAException("Invalid definition sibling class and field for sibling filter");

            _siblingClass = GADataRecord.ParseGADataClass(siblingElements[0]);
            _siblingField = siblingElements[1];
            return;
        }
        private string getDateFilter()
        {
            string dateFilter = string.Empty;
            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(_siblingClass);
            if (cd.hasDateFromField() && cd.hasDateToField())
            {
                dateFilter = GASystem.BusinessLayer.Utils.RecordsetFactory.generateDateSpanFilter(cd.DateFromField, cd.DateToField);
                System.DateTime today = System.DateTime.Now;
                System.DateTime tomorrow = today.AddDays(1);

                dateFilter = dateFilter.Replace("@dateFrom", "'" + today.ToString("yyyyMMdd") + "'");   //formatted to international safe format
                dateFilter = dateFilter.Replace("@DateTo", "'" + tomorrow.ToString("yyyyMMdd") + "'");


            }
            else
            {
                dateFilter = " 1=1 ";
            }

            return dateFilter;

        }

    }
}
