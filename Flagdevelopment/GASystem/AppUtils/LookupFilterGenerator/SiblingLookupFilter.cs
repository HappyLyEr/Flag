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
	/// Summary description for SiblingLookupFilter.
	/// </summary>
	public class SiblingLookupFilter : GeneralLookupFilter
	{
		const int MAX_OWNERLOOKUPSTEPS = 10;
		const string _WHERE_STATEMENTBASE = @"{0} in ( select distinct {1} from {2} e inner join gasuperclass s on e.{3} = s.memberclassrowid 
												where s.memberclass = '{2}' and s.ownerclass = '{4}'
												and s.ownerclassrowid = {5} and {6})"; 
						//{0} = <lookupclassrowidfield>
						//{1} = <siblingfield>
						//{2} = <siblingclass>
						//{3} = <siblingclassrowidfield> 
						//{4} = <ownerclass>
						//{5} = <ownerclassrowid>
						//{6} = datefilter
        const string _ALLOWNERS_WHERE_STATEMENTBASE = @"{0} in (select distinct {1} from {2} e inner join GASuperClass s on e.{3} = s.MemberClassRowId 
                                                    where {4} and s.MemberClass='{2}' and {5} )";
						//{0} = <lookupclassrowidfield>
						//{1} = <siblingfield>
						//{2} = <siblingclass>
						//{3} = <siblingclassrowidfield> 
						//{4} = <all selected ownerclasses with correspinding rowids >
						//{5} = datefilter

		string _lookupFilter;
		GADataClass _siblingClass;
		string _siblingField;
		GADataRecord _owner = null;
		FieldDescription _lookupFieldFd = null;
        // 20131223 Tor checked owners list (with and without siblings)
        List<GADataRecord> _ownerDataRecordsChecked = new List<GADataRecord>();
        // 20131223 Tor Found sibling owners list
        List<GADataRecord> _ownerDataRecordsFound = new List<GADataRecord>();
        
		// create sibling filter for current owner level
		public SiblingLookupFilter(GADataClass LookupDataClass, GADataRecord LookupOwner, string LookupFilter, string LookupField)
		{
			Owner = LookupOwner;
            try 
			{
				//parse lookupfilter setting to get all parameters;
				_lookupFilter = this.GetLookupfilterFilterPart(LookupFilter);
				parseLookupFilter();
				setSiblingOwner(LookupOwner," ");

				string lookupClassRowIdField = LookupDataClass.ToString().Substring(2) + "rowid";
				string siblingClassRowIdField = _siblingClass.ToString().Substring(2) + "rowid";
				// Tor 20131214 replaced by statement below: lookupFieldFd = FieldDefintion.GetFieldDescription(LookupField, LookupDataClass.ToString());
                _lookupFieldFd = FieldDefintion.GetFieldDescription(_siblingField,_siblingClass.ToString());

                // Tor 20140909 replaced for cases where the siblingclass does not use LookupClass (might be a dropdown field): string filter = string.Format(_WHERE_STATEMENTBASE, _lookupFieldFd.LookupTable.Substring(2) + "rowid", _siblingField, _siblingClass.ToString(), siblingClassRowIdField, _owner.DataClass.ToString(), _owner.RowId.ToString(), getDateFilter());
                string filter = string.Format(_WHERE_STATEMENTBASE, LookupField, _siblingField, _siblingClass.ToString(), siblingClassRowIdField, _owner.DataClass.ToString(), _owner.RowId.ToString(), getDateFilter());
                Filter = filter;
			} 
			catch (Exception ex) 
			{
				//there was an error parsing the string do not set a filter. Will display all records
				Filter = string.Empty;
			}
		}

        // Tor 20131217 added create sibling filter for all owners from LookupOwner and upwards including owner date checks
        public SiblingLookupFilter(GADataClass LookupDataClass, GADataRecord LookupOwner, string LookupFilter, string LookupField
            , string something)
        {
            // List containing all valid owner records (class and rowid)
            Owner = LookupOwner;
            try
            {
                //parse lookupfilter setting to get all parameters;
                _lookupFilter = this.GetLookupfilterFilterPart(LookupFilter);
                parseLookupFilter();
                setSiblingOwner(LookupOwner, "CheckDates");
                string lookupClassRowIdField = LookupDataClass.ToString().Substring(2) + "rowid";
                string siblingClassRowIdField = _siblingClass.ToString().Substring(2) + "rowid";
                // Tor 20131214 replaced by statement below: lookupFieldFd = FieldDefintion.GetFieldDescription(LookupField, LookupDataClass.ToString());
                _lookupFieldFd = FieldDefintion.GetFieldDescription(_siblingField, _siblingClass.ToString());

                // get all direct owners all levels

                // get all current owners with siblingclass 
                getOwners(_owner, "dateCheck", "Current");
                // getOwners(Owner, "dateCheck", "Current");
                // !!!! over ny getAllOwners(Owner, "dateCheck", "Current");

                // create owner filter by traversing List<GADataRecord> _ownerDataRecordsFound 

                string ownerFilter=String.Empty;
                foreach (GADataRecord mrecord in _ownerDataRecordsFound)
                {
                    ownerFilter = ownerFilter + "(s.OwnerClass='" + mrecord.DataClass.ToString() + "' and " + "s.OwnerClassRowId=" + Convert.ToString(mrecord.RowId) + ") or";
                }
                if (_ownerDataRecordsFound.Count > 0)
                {
                    // replace last "or" with ")"
                    ownerFilter = " (" + ownerFilter.Substring(0, ownerFilter.Length - 2) + ")";
                }
                else
                {
                    ownerFilter = "1=1 ";
                }

                // Tor 20140908 Changed to apply the correct parameters
                string filter = string.Format(_ALLOWNERS_WHERE_STATEMENTBASE
                    , LookupField //_lookupFieldFd.LookupTable.Substring(2) + "rowid"
                    , _siblingField //_myLookupclassrowidfield
                    , _siblingClass.ToString() //_siblingField
                    , _siblingClass.ToString().Substring(2)+"RowId" //_siblingClass.ToString()
                    , ownerFilter
                    , getDateFilter());


                //string filter = string.Format(_ALLOWNERS_WHERE_STATEMENTBASE
                //    , _lookupFieldFd.LookupTable.Substring(2) + "rowid"
                //    , _siblingField
                //    , _siblingClass.ToString()
                //    , siblingClassRowIdField
                //    , ownerFilter
                //    , getDateFilter());
                Filter = filter;
            }

            catch (Exception ex)
            {
                //there was an error parsing the string do not set a filter. Will display all records
                Filter = string.Empty;
            }
        }

        private void getOwners(GADataRecord Owner, string dateCheck, string current)
        {
            GADataRecord currentOwner = Owner;
            while (currentOwner != null && currentOwner.DataClass != GADataClass.GAFlag)
            {
                handleOwner(currentOwner);
                // get owner of current record
                currentOwner = GASystem.BusinessLayer.DataClassRelations.GetOwner(currentOwner);
            }
        }

        private void handleOwner(GADataRecord currentOwner)
        {
            bool isfound = false;
            if (_ownerDataRecordsChecked.Count == 0)
            {
                _ownerDataRecordsChecked.Add(new GADataRecord(currentOwner.RowId,currentOwner.DataClass));
            }
            else
            {
                if (!_ownerDataRecordsChecked.Contains(currentOwner)) 
                {
                    _ownerDataRecordsChecked.Add(new GADataRecord(currentOwner.RowId,currentOwner.DataClass));
                }
                //foreach (GADataRecord record in _ownerDataRecordsChecked)
                //{
                //    if ((record.DataClass.ToString() == currentOwner.DataClass.ToString()) && (record.RowId == currentOwner.RowId))
                //    {
                //        isfound = true;
                //    }
                //}
                //if (!isfound)
                //{
                //    _ownerDataRecordsChecked.Add(new GADataRecord(currentOwner.RowId,currentOwner.DataClass));
                //}
            }

            if (!_ownerDataRecordsFound.Contains(currentOwner)) 
            {
            //foreach (GADataRecord record in _ownerDataRecordsFound)
            //{
            //    if ((record.DataClass.ToString() == currentOwner.DataClass.ToString()) && (record.RowId == currentOwner.RowId))
            //    {
            //        isfound = true;
            //    }
            //}
            //if (!isfound)
            //{
            //  if owner class has from and to dates and dates is current
                if (GASystem.BusinessLayer.Utils.RecordSetUtils.IsRecordCurrent(currentOwner, System.DateTime.Now))
                {
                    // if ownerclass has siblingclass
                    if (GASystem.BusinessLayer.DataClassRelations.IsDataClassValidMember(currentOwner.DataClass, _siblingClass))
                    {
                        _ownerDataRecordsFound.Add(new GADataRecord(currentOwner.RowId, currentOwner.DataClass));
                        getManyToManyOwners(currentOwner);
                    }
                }
            }
        }

        //private bool checkIfOwnerHasSiblingClassAndIsCurrent(GADataRecord currentOwner) 
        //{
        //    // if owner class has from and to dates and dates are current
        //    if (GASystem.BusinessLayer.Utils.RecordSetUtils.IsRecordCurrent(currentOwner, System.DateTime.Now))
        //    {
        //        // if ownerclass has siblingclass
        //        if (GASystem.BusinessLayer.DataClassRelations.IsDataClassValidMember(currentOwner.DataClass, _siblingClass))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //private void parseLookupFilter(GADataRecord currentOwner) 
        //{ 
        //// if has siblingclass and valid dates

        //}

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

        // Tor 20131218 - changed method to include checking owner dates (from - to) if exists 
        private void setSiblingOwner(GADataRecord owner, string checkDates)
        {
            GADataRecord tmpOwner = owner;
            //travers owners, check for siblingclass. If found set owner and return from method
            for (int i = 0; i < MAX_OWNERLOOKUPSTEPS; i++)
            {
                foreach (string dataClass in DataClassRelations.GetNextLevelDataClasses(tmpOwner.DataClass))
                {
                    if (dataClass == _siblingClass.ToString())
                    {
                        _owner = tmpOwner;
                        // Tor 131217 : added date check
                        //if (checkDates == "CheckDates")
                        //{
                        //    // check if found record is current
                        //    if (GASystem.BusinessLayer.Utils.RecordSetUtils.IsRecordCurrent(tmpOwner, System.DateTime.Now))
                        //    {
                        //        _ownerDataRecordsFound.Add(new GADataRecord(tmpOwner.RowId,tmpOwner.DataClass));
                        //    }
                        //    _owner = tmpOwner;
                        //}
                        return;
                    }
                }
                tmpOwner = DataClassRelations.GetOwner(tmpOwner);
                if (tmpOwner == null)
                    throw new GAExceptions.GAException("Error setting sibling filter. Requested sibling not found");
            }
            throw new GAExceptions.GAException("Error setting sibling filter. Requested sibling not within owners");
        }


        /// <summary>
        /// Tor 20131225 Get all direct and many to many (indirect/referenced) owners - from gasuperclass, gasuperclasslinks and gaclass (many to many links)
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="siblingClass"></param>
        /// <param name="includeCheck"></param>
        /// <param name="AllorCurrent"></param>
 
//        private void getAllOwners(GADataRecord owner, string includeCheck, string AllorCurrent)
//        {
//            GADataRecord gadtmpOwner = owner;
//            GADataClass gadtmpOwnerClass;
//            bool isFound;
////            string topClass="GAFlag";
////            while (gadtmpOwner != null && gadtmpOwner.DataClass.ToString()  != topClass)
//            while (gadtmpOwner != null && gadtmpOwner.DataClass != GADataClass.GAFlag)
//            {
//                // find direct owners filtered with optional date filter
//                gadtmpOwnerClass = gadtmpOwner.DataClass; 
                
//                getManyToManyOwners(gadtmpOwner);
//                // if gadtmpOwner has siblingclass
//                if (GASystem.BusinessLayer.DataClassRelations.IsDataClassValidMember(gadtmpOwnerClass, _siblingClass))
//                {
//                    // if "Current", then check from and to date
//                    if (AllorCurrent == "Current")
//                    {
//                        if (GASystem.BusinessLayer.Utils.RecordSetUtils.IsRecordCurrent(gadtmpOwner,System.DateTime.Now))
//                        {
//                            // record is current, add to list
//                            isFound = addDataRecordToDatarecordList(gadtmpOwner);
//                        }
//                    }
//                    else
//                    {
//                        // no date check, add record to list
//                        isFound = addDataRecordToDatarecordList(gadtmpOwner);
//                    }
//                }
//                // get owner of current record
//                gadtmpOwner = GASystem.BusinessLayer.DataClassRelations.GetOwner(gadtmpOwner);
//            }
//        }
        /// <summary>
        /// Tor 20131228 Add 
        /// </summary>
        /// <param name="owner"></param>
        private void getManyToManyOwners(GADataRecord owner)
        {
            GADataRecord nextLevelRecord;
            // Tor 20131223 many to many sibling owners list
            List<GADataRecord> manyToManyOwnerDataRecordsFound = new List<GADataRecord>();
            // returns next level upwards from owner data record where many to many record is current 
            manyToManyOwnerDataRecordsFound = GASystem.BusinessLayer.ManyToManyLinks.GetManyToManyOwnerDataRecordsByMemberDataRecordAndDate(owner, System.DateTime.Now);
            // for each many to many record found
            foreach (GADataRecord mrecord in manyToManyOwnerDataRecordsFound)
            {
                // has record been checked before?

                //{
                    //foreach (GADataRecord mmrecord in _ownerDataRecordsFound)
                    //{ 
                    //    if (mrecord.RowId==mmrecord.RowId && mrecord.DataClass==mmrecord.DataClass)
                    //        // found many to many record is already in the found records list
                    //    {
                    //        isfoundmtm=true;
                    //    }
                    //}
                
                //}
                //if (!isfoundmtm)
                //else
                //{

                // if record has not been checked
                if (!_ownerDataRecordsChecked.Contains(mrecord))
                {
                    // add record to checked records
                    _ownerDataRecordsChecked.Add(new GADataRecord( mrecord.RowId, mrecord.DataClass));
                    // does class have siblingclass
                    if (GASystem.BusinessLayer.DataClassRelations.IsDataClassValidMember(mrecord.DataClass, _siblingClass))
                    {
                        // check if record has valid dates 
                        if (GASystem.BusinessLayer.Utils.RecordSetUtils.IsRecordCurrent(mrecord,System.DateTime.Now))
                        { 
                            // add reckord to found records
                            _ownerDataRecordsFound.Add(new GADataRecord( mrecord.RowId,mrecord.DataClass));
                        }
                    }
                    // get current record owner
                    nextLevelRecord = GASystem.BusinessLayer.DataClassRelations.GetOwner(mrecord);
                    // if record found and record not GAFlag class record
                    if (nextLevelRecord != null && nextLevelRecord.DataClass != GADataClass.GAFlag)
                    {
                        // record exists and dataclass is not GAFlag
                        // check owner
                        getOwners(nextLevelRecord, "dateCheck", "Current");
                    }

                }
            }
        }
        //private void getManyToManyOwners(GADataRecord owner)
        //{
        //    GADataRecord momRecord = owner;
        //    // returns next level upwards from owner data record where many to many record is current 
        //    manyToManyOwnerDataRecordsFound = GASystem.BusinessLayer.ManyToManyLinks.GetManyToManyOwnerDataRecordsByMemberDataRecordAndDate(owner, System.DateTime.Now);
        //    bool isfoundmtm = false;
        //    // for each many to many record found
        //    foreach (GADataRecord mrecord in manyToManyOwnerDataRecordsFound)
        //    {
        //        // for each previously found record
        //        foreach (GADataRecord mmrecord in _ownerDataRecordsFound)
        //        {
        //            if (mrecord.RowId == mmrecord.RowId && mrecord.DataClass == mmrecord.DataClass)
        //            // found many to many record is already in the found records list
        //            {
        //                isfoundmtm = true;
        //            }
        //        }
        //        if (!isfoundmtm)
        //        {
        //            if (GASystem.BusinessLayer.DataClassRelations.IsDataClassValidMember(mrecord.DataClass, _siblingClass))
        //            // does found many to many class have siblingclass ?
        //            {
        //                // check if class has siblingclass
        //                if (GASystem.BusinessLayer.DataClassRelations.IsDataClassValidMember(mrecord.DataClass, _siblingClass))
        //                {
        //                }

        //                // if record not already there and class has siblingclass
        //                _ownerDataRecordsFound.Add(new GADataRecord(mrecord.RowId, mrecord.DataClass));
        //            }
        //        }
        //    }
        //}
        /// <summary>
        /// Tor 20131228 Add datarecord to datarecord list if not already there
        /// </summary>
        //private bool addDataRecordToDatarecordList(GADataRecord gadtmpOwner)
        //{
        //    bool isfound = false;
        //    foreach (GADataRecord record in _ownerDataRecordsFound)
        //    {
        //        if ((record.DataClass.ToString() == gadtmpOwner.DataClass.ToString()) && (record.RowId == gadtmpOwner.RowId))
        //        {
        //            isfound = true;
        //        }
        //    }
        //    if (!isfound)
        //    {
        //        _ownerDataRecordsFound.Add(new GADataRecord(gadtmpOwner.RowId, gadtmpOwner.DataClass));

        //    }
        //    return isfound;
        
        //}
            				
		private string getDateFilter() 
		{
			string dateFilter = string.Empty;
			ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(_siblingClass);
			if (cd.hasDateFromField() && cd.hasDateToField()) 
			{
				dateFilter = GASystem.BusinessLayer.Utils.RecordsetFactory.generateDateSpanFilter(cd.DateFromField, cd.DateToField);
				System.DateTime today = System.DateTime.Now;
				System.DateTime tomorrow = today.AddDays(1);
				 
				dateFilter = dateFilter.Replace( "@dateFrom" ,"'" + today.ToString("yyyyMMdd") + "'");   //formatted to international safe format
				dateFilter = dateFilter.Replace( "@DateTo", "'" + tomorrow.ToString("yyyyMMdd") + "'");


			} 
			else 
			{
				dateFilter = " 1=1 ";
			}

			return dateFilter;

		}

		public override bool CanDisableFilter 
		{
			get {return true;}
		}

		public override string FilterDescription
		{
			get
			{
				return  string.Format(GASystem.AppUtils.Localization.GetGuiElementText("LookupFilterOn"), 
					GASystem.AppUtils.Localization.GetGuiElementText(this._lookupFieldFd.LookupTable.ToString()),
					GASystem.AppUtils.Localization.GetGuiElementText(this._siblingClass.ToString()));
			}
		}
	}
}

