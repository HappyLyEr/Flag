using System;
using System.Data;
using GASystem.DataAccess.Security;
using GASystem.GAExceptions;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Collections;
using GASystem.AppUtils;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Abstract superclass for GA business classes that manages GA tables. Contains some default operations for managing the 
	/// table data, and some abstract operations that must be overrided by GA business classses.
	/// </summary>
	public abstract  class BusinessClass
	{
		GADataClass m_dataClass;
        private GADataClass _dataClass;
        private bool _isWorkflowTriggeredWhenUpdated;

        public bool IsWorkflowTriggeredWhenUpdated
        {
            get { return _isWorkflowTriggeredWhenUpdated; }
            set { _isWorkflowTriggeredWhenUpdated = value; }
        }


		/// <summary>
		/// Constructor
		/// </summary>
		public BusinessClass()
		{


			//
			// TODO: Add constructor logic here
			//
			
		}

        public DataSet CopyRecordWithChildren(GADataRecord RecordToCopy, GADataRecord NewOwner, GADataTransaction Transaction)
        {
            BusinessClass businessClass = Utils.RecordsetFactory.Make(RecordToCopy.DataClass);

            DataSet dataSetCopy = CopySingleRecord(RecordToCopy, NewOwner, Transaction);
            DataSet returnSet = businessClass.CommitDataSet(dataSetCopy, NewOwner, Transaction);

            int newRecordRowId = (int)returnSet.Tables[0].Rows[0][RecordToCopy.DataClass.ToString().Substring(2) + "RowId"];
            GADataRecord dataSetCopyRecord = new GADataRecord(newRecordRowId, RecordToCopy.DataClass);

            SuperClassDS superClassSet = SuperClassDb.GetSuperClassByOwner(RecordToCopy.RowId, RecordToCopy.DataClass.ToString(), Transaction);
            foreach (SuperClassDS.GASuperClassRow row in superClassSet.Tables[0].Rows)
            {
                if (row.MemberClass != GADataClass.GAAction.ToString())  //do not copy actions, action should be created using gastoreobject
                {
                    GADataRecord tmpRecordToCopy = new GADataRecord(row.MemberClassRowId, GADataRecord.ParseGADataClass(row.MemberClass));
                    CopyRecordWithChildren(tmpRecordToCopy, dataSetCopyRecord, Transaction);
                }
            }
            return returnSet;
        }

        public DataSet CopyRecordWithChildren(GADataRecord RecordToCopy, GADataRecord NewOwner)
        {
            DataSet returnSet = null;
            GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
            try
            {
                returnSet = CopyRecordWithChildren(RecordToCopy, NewOwner, transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                transaction.Connection.Close();
            }
            return returnSet;
        }

        public DataSet CopySingleRecord(GADataRecord RecordToCopy, GADataRecord newOwner, GADataTransaction Transaction)
        {
            BusinessClass businessClass = Utils.RecordsetFactory.Make(RecordToCopy.DataClass);
            DataSet copyFromSet = businessClass.GetByRowId(RecordToCopy.RowId, Transaction);
            DataSet copyToSet = businessClass.GetNewRecord(newOwner, Transaction);

            int i = 0;
            object[] itemsArray = new object[copyFromSet.Tables[0].Columns.Count];
            foreach (DataColumn column in copyFromSet.Tables[0].Columns)
            {
                
                if (column.ColumnName.ToLower() != (RecordToCopy.DataClass.ToString().Substring(2).ToLower() + "rowid") && column.ColumnName.IndexOf("guid") < 0) 
                {
                    //keep default values generated in newly created row
                    //get value from newly created row first
                    itemsArray[i] = copyToSet.Tables[0].Rows[0].ItemArray[i];
                    
                    //if null use values from copied row
                    if ( itemsArray[i] == DBNull.Value || itemsArray[i] == null)
                        itemsArray[i] = copyFromSet.Tables[0].Rows[0].ItemArray[i];
                }
                i++;
            }
            copyToSet.Tables[0].Rows[0].ItemArray = itemsArray;
            return copyToSet;
        }
		/// <summary>
		/// For the current dataclass delete the row with rowid RowId. The property DataClass sets the GA table from 
		/// which this row should be deleted.
		/// </summary>
		/// <param name="RowId">Rowid of datarow to be deleted</param>
		/// <returns></returns>
		public virtual void DeleteRow(int RowId) 
		{
			//TODO include transaction !!!!!!!!
			
			
			//get owner
			GADataRecord owner = DataClassRelations.GetOwner(new GADataRecord(RowId, this.DataClass));
			//get reference to the row to be deleted
			DataSet ds = this.GetByRowId(RowId);
			
			//TODO use transaction object
			//delete link in arc
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			
			try 
			{

				if (owner != null)
					DataClassRelations.DeleteDataClassRelation(owner, new GADataRecord(RowId, this.DataClass), transaction);
				//delete row
				ds.Tables[0].Rows[0].Delete();
				this.Update(ds, transaction);

				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				throw ex;
			}
			finally 
			{
				transaction.Connection.Close();
			}
		}


        public virtual void UpdateFromList(Hashtable updatedValues)
        {
            string rowidColumn = this.DataClass.ToString().Substring(2) + "rowid";
            int rowid = (int)updatedValues[rowidColumn];
            System.Data.DataSet ds = this.GetByRowId(rowid);
            foreach (DictionaryEntry entry in updatedValues) 
            {
                if (entry.Key.ToString().ToUpper() != rowidColumn.ToUpper())
                    if (entry.Value != null && entry.Value.ToString() != string.Empty )
                        ds.Tables[0].Rows[0][entry.Key.ToString()] = entry.Value;
                    else
                        ds.Tables[0].Rows[0][entry.Key.ToString()] = DBNull.Value;
            }
            this.CommitDataSet(ds); // Tor 20161020
        }

        /// <summary>
        /// Create a new record based in the updated record when updating from list values.
        /// Used for creating creating a new record where the record type has a from data and todate.
        /// If the todate is changed is a new record created with a today equal to the previous records 
        /// fromdate + 1day, all other values are copied, but the todate is set to null
        /// </summary>
        /// <param name="updatedValues"></param>
        public virtual void UpdateFromListAndCreateNew(Hashtable updatedValues, GADataRecord owner)
        {
            string rowidColumn = this.DataClass.ToString().Substring(2) + "rowid";
            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(this.DataClass);

            if (!cd.hasDateToField() && !cd.hasDateFromField())
            {
                //record does not have a from and todate field, update existing record and return;
                UpdateFromList(updatedValues);
                return;
            }

            //get existing record
            int rowid = (int)updatedValues[rowidColumn];
            System.Data.DataSet ds = this.GetByRowId(rowid);


             //check that fromdate is not null
            if (ds.Tables[0].Rows[0][cd.DateFromField] == DBNull.Value || updatedValues[cd.DateFromField] == null)
                return;



            //if fromdate is not updated, update existing record and return
            System.DateTime exitingFromDate = (System.DateTime)ds.Tables[0].Rows[0][cd.DateFromField];
            System.DateTime updatedFromDate = (System.DateTime)updatedValues[cd.DateFromField];
            if (exitingFromDate.Year == updatedFromDate.Year && exitingFromDate.Month == updatedFromDate.Month && exitingFromDate.Day == updatedFromDate.Day)
            {
                UpdateFromList(updatedValues);
                return;
            }

            //if fromdate is changed to a earlier date, update existing record and return
            if (updatedFromDate.CompareTo(exitingFromDate) < 0)
            {
                UpdateFromList(updatedValues);
                return;
            }


            
         
            //create new record
            System.Data.DataSet dsnew = this.GetNewRecord(owner);

            //get fielddefinitions
            FieldDescription[] fds = GASystem.AppUtils.FieldDefintion.GetFieldDescriptions(this.DataClass);

            //set values in new record first from existing record, then from hashtable
            foreach (FieldDescription fd in fds) 
            {
                if (fd.FieldId.ToUpper() != this.DataClass.ToString().Substring(2).ToUpper() + "ROWID")
                    dsnew.Tables[0].Rows[0][fd.FieldId] = ds.Tables[0].Rows[0][fd.FieldId];
            }
            foreach (DictionaryEntry entry in updatedValues)
            {
                if (entry.Key.ToString().ToUpper() != rowidColumn.ToUpper())
                    if (entry.Value != null)
                        dsnew.Tables[0].Rows[0][entry.Key.ToString()] = entry.Value;
                    else
                        dsnew.Tables[0].Rows[0][entry.Key.ToString()] = DBNull.Value;
            }



            //set dates
            ds.Tables[0].Rows[0][cd.DateToField] = ((System.DateTime)dsnew.Tables[0].Rows[0][cd.DateFromField]).AddDays(-1);
            this.SaveNew(dsnew, owner);

            //commit existing record
            this.CommitDataSet(ds);


        }


		/// <summary>
		/// Update business class using data from a dataset. Use a trasaction object to make this update a part of 
		/// a trasaction
		/// </summary>
		/// <param name="ds">DataSet with updated data</param>
		/// <param name="transaction">Transaction object</param>
		public virtual DataSet Update(DataSet ds, GADataTransaction transaction) 
		{
			return UpdateDataSet(ds, transaction); 
		}
		

		/// <summary>
		/// Update business class using data from a dataset
		/// </summary>
		/// <param name="ds">DataSet with updated data</param>
		/// <returns>Updated dataset</returns>
		public virtual DataSet Update(DataSet ds)
		{
			return Update(ds, null);
		}

		/// <summary>
		/// Call datalayer for updating dataset. Uses DataAccess.dataaccess class
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="transaction"></param>
		/// <returns>Dataset containing the updated dataset</returns>
        public virtual DataSet UpdateDataSet(DataSet ds, GADataTransaction transaction)
        {
            return UpdateDataSet(ds, null, transaction);
        }

        // Tor 20150409 Added overload method to pass GADataRecord Owner for GARecordHistory Reporting
        public virtual DataSet UpdateDataSet(DataSet ds, GASystem.DataModel.GADataRecord Owner,GADataTransaction transaction) 
		{
            // JOF 20141019: call metode for ?legge updaterte verdier i dataset ved lagring
            // TODO: er de mulig ?sende  med eier?
            // Tor 20150108 Get current record owner (if update multiline, each record might have different owner
            // then http://localhost/flagDev/default.aspx?tabid=238&dataclass=GAManageChange)
        //    GADataRecord owner = GASystem.AppUtils.CodeTables
            // on new: Auditrowid=0&edit=true&dataclass=GAAudit&ownerclass=GACompany&ownerrowid=4054
            // on edit: &AuditRowId=12000069&edit=true&dataclass=GAAudit
            // on edit multiple from all...: &dataclass=GAManageChange
            // on edit multiple under tab: &dataclass=GAManageChange&ownerclass=GALocation&ownerrowid=4048
//            System.Web.UI.WebControls.HttpContext a = System.Web.UI.WebControls.HttpContext.Current;

            //string dataClass = a.Request.QueryString.Get("dataclass");
            //string ownerDataClass = a.Request.QueryString.Get("ownerclass");
            //string ownerDataClassRowId = a.Request.QueryString.Get("ownerrowid");
            //string dataClassRowId = a.Request.QueryString.Get(dataClass.Substring(2) + "RowId");
            //string ifEdit = a.Request.QueryString.Get("edit");

            // if update from web
            // if 
            // Tor 20170215 Get Owner GADatarecord
            GADataRecord myOwner = Owner;
            if (myOwner == null)
            {
                GADataRecord aa = GASystem.DataModel.GADataRecord.GetGADataRecordFromFirstRow(ds, DataClass);
                // Tor 20170327 myOwner = DataClassRelations.GetOwner(aa);
                // Tor if null is returned, the record in the dataset has been deleted
                if (aa!=null)
                myOwner = DataClassRelations.GetOwner(aa, transaction);
            }
            // Tor 20170328 If myOwner is null the record has been deleted
            if (myOwner!=null)
            ApplyOnUpdateValues(ds, myOwner, transaction);
            // Tor 2070215 ApplyOnUpdateValues(ds, null, transaction);
            
            // Tor 20140724 ds inneholder alle data i denne record - de fleste tilfeller er det bare en record
            // sjekk om det skal skrives til GAHistoryRecord this.m_dataClass inneholder dataklasse i datasettet
            // lag ny metode ala ApplyDefaultValues(ds, owner, transaction); i denne klassen for ?finne ut hvilke felter som er endret eller on det er en ny record
            // sjekk hvordan finne ut om det er en ny record og hvordan finne rowid 
			DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(this.m_dataClass, transaction);

            // Tor 20150114 Moved from GASystem/DataAccess/DataAccess.cs method Update(DataSet GADataSet)
            //Verify that user has access
            // Tor 20141229 Add code for GAHistoryLog here
            _dataClass = m_dataClass;
            System.Collections.ArrayList columns = new System.Collections.ArrayList();
            foreach (DataColumn column in ds.Tables[_dataClass.ToString()].Columns)
            {
                if (column.ColumnName.ToLower() != "rowguid")
                    if (column.ColumnName.ToLower() != "[rowguid]")
                        columns.Add(column.ColumnName);
            }

            // Tor 20170328 If myOwner is null the record has been deleted
            if (myOwner != null)
            {
                // Tor 20160206 If record has Vertical and Vertical has been updated then Update Vertical in 'Active' GAWorkitems 
                ClassDescription cd_dataClass = ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
                if (cd_dataClass.hasVerticalField())
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row.RowState == DataRowState.Modified) UpdateVerticalInWorkitems(row, Owner, cd_dataClass, transaction);
                    }
            // Tor 20160206 End

            // Tor 20161003 Add SuperClass ChangedBy and DateChanged for each modified record
                SuperClassAttributes.UpdateGASuperClassChangedBy(ds, transaction);
                GASystem.BusinessLayer.RecordHistoryLog.createRecordHistoryLogFromNewOrUpdatedRecord(ds, Owner, transaction);
            }

			DataSet dsresult = gada.Update(ds);
			return dsresult;
		}

        // Tor 20170410 method to update dataset without using parameters from GAFieldDefinitions etc
        public virtual DataSet UpdateDataSetNoOwner(DataSet ds, GADataTransaction transaction)
        {
            DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(this.m_dataClass, transaction);
            _dataClass = m_dataClass;
            System.Collections.ArrayList columns = new System.Collections.ArrayList();
            foreach (DataColumn column in ds.Tables[_dataClass.ToString()].Columns)
            {
                if (column.ColumnName.ToLower() != "rowguid")
                    if (column.ColumnName.ToLower() != "[rowguid]")
                        columns.Add(column.ColumnName);
            }
            DataSet dsresult = gada.Update(ds);
            return dsresult;
        }

        // Tor 20140724 method for storing insert/update in GAHistoryRecord
        public void CreateHistoryRecord(DataSet ds, GADataTransaction transaction)
        { 
            // check if class has trigger
            // if not return
            // if new record, set history record new record=true, else false
            // compare updated fields with selected fields
            // get current logged on user
        }


		/// <summary>
		/// Update all active workitems under owner
		/// </summary>
		/// <param name="RowId">Rowid for the record to get</param>
		/// <returns></returns>
        /// 
        private void UpdateVerticalInWorkitems(DataRow row, GASystem.DataModel.GADataRecord Owner, ClassDescription cd,GADataTransaction transaction)
        {
            // get vertical field id
            // if class has GAAction memberClass
            string verticalFieldId = cd.VerticalFieldName;

            //int OldAttributeValue = Convert.ToInt32(row[verticalFieldId, DataRowVersion.Original]);

            //int NewAttributeValue = Convert.ToInt32(row[verticalFieldId, DataRowVersion.Current]);

            // if vertical value is changed
            if (!row[verticalFieldId, DataRowVersion.Original].ToString().Equals( row[verticalFieldId, DataRowVersion.Current].ToString()))
            {
                // update all active member GAWorkitems with new vertical value
                //int newVertical = Convert.ToInt32( row[verticalFieldId, DataRowVersion.Current]);
//                string myclass=cd.DataClassName.ToString();
//                string @myRowIdColumnName=myclass.Substring(2,myclass.Length-2)+"RowId";
//                int myRowId=Convert.ToInt32( row[myclass.Substring(2,myclass.Length-2)+"RowId"]);
//                GADataClass dataclass=GADataRecord.ParseGADataClass(myclass);
//                GADataRecord record = new GADataRecord(myRowId, GADataRecord.ParseGADataClass(myclass));
                
//                GADataRecord record = new GADataRecord(myRowId, dataclass);
                if (row[verticalFieldId, DataRowVersion.Current] == DBNull.Value)
                {Workitem.updateVerticalInAllActiveWorkitemsUnderOwner(new GADataRecord(Convert.ToInt32(row[cd.DataClassName.ToString().Substring(2) + "RowId"]), GADataRecord.ParseGADataClass(cd.DataClassName.ToString()))
                    , -1 /* to enforce storing dbnull */
                    , transaction);
                }
                else
                {
                    Workitem.updateVerticalInAllActiveWorkitemsUnderOwner(
                        new GADataRecord(Convert.ToInt32(row[cd.DataClassName.ToString().Substring(2) + "RowId"]), GADataRecord.ParseGADataClass(cd.DataClassName.ToString()))
                        , Convert.ToInt32(row[verticalFieldId, DataRowVersion.Current])
                        , transaction);
                }
            }
        }

		public virtual DataSet GetByRowId(int RowId)
		{
			return GetByRowId(RowId, null);
		}


		/// <summary>
		/// Get single row table in a dataset for the current businessclass using a transaction
		/// </summary>
		/// <param name="RowId"></param>
		/// <param name="transaction"></param>
		/// <returns></returns>
		public virtual DataSet GetByRowId(int RowId, GADataTransaction transaction) 
		{
            // Tor 201611 Security 20161215 
            DataAccess.DataAccess GAData = new GASystem.DataAccess.DataAccess(this.DataClass, transaction);
            // Tor 201703 Rollback DataAccess.DataAccess GAData = new GASystem.DataAccess.DataAccess(this.DataClass, RowId, transaction);
			DataSet ds = GAData.GetByRowId(RowId);
			if (ds.Tables[this.DataClass.ToString()].Rows.Count == 0)
				throw new GAException("Datarecord " + DataClass.ToString() + "-" + RowId.ToString() + " not found");
			return ds;
		}

        /// <summary>
        /// Get single row table in a dataset for the current GADataRecord using a transaction
        /// </summary>
        /// <param name="GADataRecord"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        // Tor 20150409 added because I could not find the method elsewhere 
        public static DataSet GetByGADataRecord(GADataRecord record, GADataTransaction transaction)
        {
            DataAccess.DataAccess GAData = new GASystem.DataAccess.DataAccess(record.DataClass, transaction);
            DataSet ds = GAData.GetByRowId(record.RowId);
            if (ds.Tables[record.DataClass.ToString()].Rows.Count == 0)
                throw new GAException("Datarecord " + record.DataClass.ToString() + "-" + record.RowId.ToString() + " not found");
            return ds;
        }
        /// <summary>
        /// Get a number of records based on an array of rowids
        /// </summary>
        /// <param name="rowIds">Array of rowids</param>
        /// <param name="transaction">GA Transaction</param>
        /// <returns>Dataset containg the requested rows</returns>
        public virtual DataSet getByRowIds(int[] rowIds, GADataTransaction transaction )
        {
            DataAccess.DataAccess GAData = new GASystem.DataAccess.DataAccess(this.DataClass, transaction);
            return GAData.GetByRowIds(rowIds);
        }


		/// <summary>
		/// Get the name of a datarecord. Uses class definition to find the column containing the name
		/// </summary>
		/// <param name="RowId">Rowid of the datarecord we would like to find the name for</param>
		/// <returns>Datarecord name. Or an empty string if the name is not found</returns>
		public virtual string GetDataRecordName(int RowId)
		{
			ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(this.m_dataClass);
			DataSet ds = GetByRowId(RowId);
			if (ds.Tables[0].Rows.Count > 0)
				if (ds.Tables[0].Columns.Contains(cd.ObjectName))
					return ds.Tables[0].Rows[0][cd.ObjectName].ToString();
			return string.Empty;
		}

		public virtual DataSet GetEmptyDataSet() 
		{
			DataAccess.DataAccess GAData = new GASystem.DataAccess.DataAccess(this.DataClass, null);
			return GAData.GetEmptyDataSet();
		}



		public virtual DataSet GetByOwner(GADataRecord Owner, GADataTransaction transaction)
		{
			DataAccess.DataAccess GAData = DataAccessFactory.Make(this.DataClass, transaction);
			return GAData.GetByOwner(Owner.RowId, Owner.DataClass);
		}


        public virtual DataSet GetByOwnerAndTimeSpan(GADataRecord Owner, System.DateTime StartDate, System.DateTime EndDate, GADataTransaction transaction)
        {
            DataAccess.DataAccess GAData = DataAccessFactory.Make(this.DataClass, transaction);
            return GAData.GetByOwnerAndTimeSpan(Owner, StartDate, EndDate);
        }


        public virtual DataSet GetByOwnerAndTimeSpan(GADataRecord Owner, System.DateTime StartDate, System.DateTime EndDate, String filter, GADataTransaction transaction)
        {
            DataAccess.DataAccess GAData = DataAccessFactory.Make(this.DataClass, transaction);
            return GAData.GetByOwnerAndTimeSpan(Owner, StartDate, EndDate, filter);
        }

        public int GetNumberOfRowsByOwnerAndTimeSpan(GADataRecord Owner, System.DateTime StartDate, System.DateTime EndDate, string filter, GADataTransaction transaction)
        {
            DataAccess.DataAccess GAData = DataAccessFactory.Make(this.DataClass, transaction);
            return GAData.GetNumberOfRowsByOwnerAndTimeSpan(Owner, StartDate, EndDate, filter);
        }


		public virtual DataSet GetManyToManyRecordsByOwnerAndTimeSpan(GADataRecord Owner, System.DateTime StartDate, System.DateTime EndDate, GADataTransaction transaction) 
		{
			DataAccess.DataAccess GAData = DataAccessFactory.Make(this.DataClass, transaction);
			return GAData.GetManyToManyRecordsByOwnerAndTimeSpan(Owner, StartDate, EndDate);
		}

      
		/// <summary>
		/// Save new record. Calls commitdataset. Provided for backward compatability. User should call 
		/// CommitDataSet
		/// </summary>
		/// <param name="ds">Dataset holding the record to be saved</param>
		/// <param name="Owner">Onwer record for the new record</param>
		/// <returns>GADataRecord holding a reference (GAClass and rowid) of the saved record</returns>
		public virtual DataSet SaveNew(DataSet ds, GADataRecord Owner)
		{
			return this.CommitDataSet(ds, Owner);
		}
		
		/// <summary>
		/// Save new record. Expects row 0 of the table in the dataset related to the current instance of this
		/// business class to hold a new record. Validates this record and saves it to the database. Uses 
		/// transaction object.
		/// </summary>
		/// <param name="ds">Dataset holding the record to be saved</param>
		/// <param name="Owner">Owner record for the new record</param>
		/// <returns>GADataRecord holding a reference (GAClass and rowid) of the saved record</returns>
		public virtual DataSet SaveNew(DataSet ds, GADataRecord Owner,  GADataTransaction transaction)
		{
		
			Utils.IdGenerator idgen = new GASystem.BusinessLayer.Utils.IdGenerator(this.DataClass, Owner, transaction);
			idgen.ApplyReferenceId(ds);

			
			DataRow dr = ds.Tables[this.DataClass.ToString()].Rows[0];
			// Tor 20150409 replaced this with the below to enable writing GARecordHistoryLog ds = this.UpdateDataSet(ds, transaction);
            ds = this.UpdateDataSet(ds, Owner, transaction);

			//if owner is null, set owner to gaflag-1 : TODO change all code to implement gaflag correctly
			if (Owner == null)
				Owner = new GADataRecord(1, GADataClass.GAFlag);

			    
			//TODO add checks: is null owner valid? is owner valid?
			if (Owner != null)
			{
				DataClassRelations.CreateDataClassRelation(Owner.RowId, Owner.DataClass, getRowRowId(dr) , this.DataClass, transaction);
			}
			
			//add member classes
			Utils.StoreObject.AddMemberClasses(new GADataRecord(getRowRowId(dr), this.DataClass), ds, transaction);
            // Tor 20141208 add GARecordHistoryLog
//            RecordHistoryLog.SaveNewRecordHistoryLog(ds, Owner);
			return ds;
		}

		/// <summary>
		/// Save new record. Expects n new (rowstate=added) rows of the table in the dataset related to the current instance of this
		/// business class to hold a new record. Validate records and saves them to the database. Uses 
		/// transaction object.
		/// </summary>
		/// <param name="ds">Dataset holding the record to be saved</param>
		/// <param name="Owner">Owner record for the new record</param>
		/// <returns>GADataRecord holding a reference (GAClass and rowid) of the saved record</returns>
		public virtual DataSet SaveNewMultiple(DataSet ds, GADataRecord Owner,  GADataTransaction transaction)
		{
			//TODO add method for setting default values
			//DataRow dr = ds.Tables[this.DataClass.ToString()].Rows[0];

            // Tor 20140824 Add reference id
                //Utils.IdGenerator idgen = new GASystem.BusinessLayer.Utils.IdGenerator(this.DataClass, Owner, transaction);
                //idgen.ApplyReferenceId(ds);
            // Tor 20140824 end

            // Tor 20180419 Add reference id
            Utils.IdGenerator idgen = new GASystem.BusinessLayer.Utils.IdGenerator(this.DataClass, Owner, transaction);
            idgen.ApplyReferenceId(ds);
            // Tor 20180419 end
            
            ds = this.Update(ds, transaction);
            
			foreach (DataRow dr in ds.Tables[this.DataClass.ToString()].Rows) 
			{
				//TODO add checks: is null owner valid? is owner valid?
				if (Owner != null)
				{
					DataClassRelations.CreateDataClassRelation(Owner.RowId, Owner.DataClass, getRowRowId(dr) , this.DataClass, transaction);
				}
			
			
				//add member classes
				Utils.StoreObject.AddMemberClasses(new GADataRecord(getRowRowId(dr), this.DataClass), transaction);
			}

			return ds;
		}


        /// <summary>
		/// Get a new record for the current business class. 
		/// </summary>
		/// <returns>Dataset containing a table with the new row</returns>
        public virtual DataSet GetNewRecord(GADataRecord owner)
        {
            return GetNewRecord(owner, null);
        }

		
		/// <summary>
		/// Get a new record for the current business class. 
		/// </summary>
		/// <returns>Dataset containing a table with the new row</returns>
		public virtual DataSet GetNewRecord(GADataRecord owner, GADataTransaction transaction) 
		{
			//use dataaccess to get new record of correct gadataclass type;
			DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(this.m_dataClass, null);
			DataSet ds = gada.GetNewRecord();
			
            // Tor 20140827 If owner==null and dataclass is owned by GAFlag, set owner data record = GAFlag/1
            GADataRecord classOwner = owner;
            if (owner == null)
            {
                SuperClassLinksDS Owner = SuperClassDb.GetSuperClassLinksByMember(this.m_dataClass);
                
                string a = "";
                if (Owner.Tables[0].Rows.Count == 1)    //jof 20150510  - jof, when listselected, owner superclasslinks may not return a result
                    a = Owner.Tables[0].Rows[0]["OwnerClass"].ToString();
                
                //if (Owner.Tables[0].Rows[0].Table.ToString() == "GAFlag")
                if (Owner.Tables.Count == 1 && a == "GAFlag")
                {
                    classOwner = new GADataRecord(1, GADataClass.GAFlag);
                }
            }
            //apply default values;
            ApplyDefaultValues(ds, classOwner, transaction);
            // Tor 20140827 end ApplyDefaultValues(ds, owner, transaction);

			return ds;
		}
		

        /// <summary>
		/// Apply default values to a new datarecord,
		/// TODO merge this into the getnewrecord method
		/// </summary>
		/// <param name="RecordSet"></param>
        public virtual void ApplyDefaultValues(DataSet RecordSet, GADataRecord owner)
        {
            ApplyDefaultValues(RecordSet, owner, null);
        }
		/// <summary>
		/// Apply default values to a new datarecord,
		/// TODO merge this into the getnewrecord method
		/// </summary>
		/// <param name="RecordSet"></param>
        public virtual void ApplyDefaultValues(DataSet RecordSet, GADataRecord owner, GADataTransaction transaction) 
		{
			//get fielddef
			GASystem.AppUtils.FieldDescription[] fds = GASystem.AppUtils.FieldDefintion.GetFieldDescriptions(m_dataClass);
            DefaultValues.DefaultValueFactory valueFactory = new GASystem.BusinessLayer.DefaultValues.DefaultValueFactory();
            foreach (GASystem.AppUtils.FieldDescription fd in fds) 
			{
                if (fd.CopyFromFieldId != null && fd.CopyFromFieldId != string.Empty)
                {
                    RecordSet.Tables[m_dataClass.ToString()].Rows[0][fd.FieldId] = valueFactory.Make(fd, owner, transaction).GetValue();
                }
			}
		}

        /// <summary>
        /// Apply values to an datarecord on save
        /// TODO merge this into the getnewrecord method
        /// </summary>
        /// <param name="RecordSet"></param>
        public virtual void ApplyOnUpdateValues(DataSet RecordSet, GADataRecord owner, GADataTransaction transaction)
        {
            //get fielddef
            GASystem.AppUtils.FieldDescription[] fds = GASystem.AppUtils.FieldDefintion.GetFieldDescriptions(m_dataClass);

            // Tor 20150420 drop this function on delete record in dataset.
            foreach (DataRow row in RecordSet.Tables[0].Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    return;
                }
            }
                

            DefaultValues.DefaultValueFactory valueFactory = new GASystem.BusinessLayer.DefaultValues.DefaultValueFactory();

            // Tor 20170215 catch fields to be updated from other fields in the record - must be run before MakeOnUpdate to enable setting source field to another value with <OnUpdate
            foreach (DataRow row in RecordSet.Tables[0].Rows)
            {
                if (row.RowState == DataRowState.Added)
                {
                    foreach (GASystem.AppUtils.FieldDescription fd in fds)
                    {
                        if (fd.CopyFromFieldId != null && fd.CopyFromFieldId != string.Empty 
                            && fd.CopyFromFieldId.IndexOf("<") != 0) //reference to field in same record contains only fieldname
                            RecordSet.Tables[m_dataClass.ToString()].Rows[0][fd.FieldId] 
                                = valueFactory.MakeFromCurrentRecord(fd, owner, transaction, RecordSet, row, this.DataClass).GetValue();
                    }
                 }
            }
 

            foreach (GASystem.AppUtils.FieldDescription fd in fds)
            {
                if (fd.CopyFromFieldId != null && fd.CopyFromFieldId != string.Empty && fd.CopyFromFieldId.IndexOf("<%OnUpdate:") == 0)
                {
                    // Tor 20170412 Catch GAOpprotunity.IntFree2 and set to 0 if value is null
                    if (RecordSet.Tables[m_dataClass.ToString()].ToString() == "GAOpportunity" && RecordSet.Tables[m_dataClass.ToString()].Rows[0]["IntFree2"] == DBNull.Value)
                    {
                        RecordSet.Tables[m_dataClass.ToString()].Rows[0]["IntFree2"] = 0;
                    }
                    RecordSet.Tables[m_dataClass.ToString()].Rows[0][fd.FieldId] = valueFactory.MakeOnUpdate(fd, owner, transaction, RecordSet, this.DataClass).GetValue();
                }
            }

        }





		/// <summary>
		/// Given for the current datarecord given by this class and memberrowid find all records above of type ownerdataclass
		/// linking to this record.
		/// </summary>
		/// <param name="OwnerDataClass"></param>
		/// <param name="Date"></param>
		/// <param name="MemberRowId"></param>
		/// <returns></returns>
		public virtual DataSet GetManyToManyOwnerRecords(GADataClass OwnerDataClass, DateTime Date, int MemberRowId) 
		{
			DateTime startDate = new DateTime(Date.Year, Date.Month, Date.Day, 0,0,0);
			DateTime endDate = new DateTime(Date.Year, Date.Month, Date.Day, 23,59,59);

			//get all current datarecords of ownertype
			BusinessClass ownerbc = Utils.RecordsetFactory.Make(OwnerDataClass);
			DataSet resultds = ownerbc.GetEmptyDataSet();  //get a empty dataset;
			DataSet ods = ownerbc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(new GADataRecord(1, GADataClass.GAFlag), startDate, endDate, string.Empty);
			foreach (DataRow row in  ods.Tables[OwnerDataClass.ToString()].Rows) 
			{
				DataSet ds = this.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(new GADataRecord((int)row[OwnerDataClass.ToString().Substring(2) + "rowid"], OwnerDataClass), startDate, endDate, this.DataClass.ToString().Substring(2) + "rowid = " + MemberRowId.ToString() );
				if (ds.Tables[this.DataClass.ToString()].Rows.Count > 0)
				{
					foreach (DataRow dsrow in ds.Tables[this.DataClass.ToString()].Rows)
						if (dsrow.Table.Columns.Contains(this.DataClass.ToString().Substring(2) + "rowid") && (int)dsrow[this.DataClass.ToString().Substring(2) + "rowid"] == MemberRowId)
                            resultds.Tables[OwnerDataClass.ToString()].ImportRow(row);
				}
			}
			return resultds;

		}

        /// <summary>
        /// Find all records of type OwnerTypeClass linking to the record given by rowid using manytomany linking
        /// This method reuses the GetManyToManyOwnerRecords method. Data is returned as a list of gadatarecords.
        /// </summary>
        /// <param name="OwnerDataClass"></param>
        /// <param name="Date"></param>
        /// <param name="MemberRowId"></param>
        /// <returns></returns>
        public virtual System.Collections.Generic.List<GADataRecord> GetManyToManyOwnerRecordsList(GADataClass OwnerDataClass, DateTime Date, int MemberRowId)
        {
            System.Collections.Generic.List<GADataRecord> dataRecords = new System.Collections.Generic.List<GADataRecord>();
            DataSet ds = GetManyToManyOwnerRecords(OwnerDataClass, Date, MemberRowId);
            string rowIdColumnName = OwnerDataClass.ToString().Substring(2) + "rowid";
            if (!ds.Tables[0].Columns.Contains(rowIdColumnName))
                return dataRecords;   //dataset does not contain a rowid column. just return a empty set.TODO log.
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dataRecords.Add(new GADataRecord((int)row[rowIdColumnName],OwnerDataClass));
            }
            return dataRecords;
        }


	
		/// <summary>
		/// GADataClass identifier for the instance of this business class
		/// </summary>
		public virtual GADataClass DataClass 
		{
			set {m_dataClass = value;}
			get {return m_dataClass;}
		}

		public DataSet CommitDataSet(DataSet RecordSet)
		{
			return CommitDataSet(RecordSet, null);
		}

		/// <summary>
		/// Check whether user has spesific permissions to this record. 
		/// E.g special field settings in record denies access
		/// Must be used in combination with settings at class level
		/// </summary>
		/// <returns>bool</returns>
		public virtual bool HasPermissionToRecord(int RowId) 
		{
			//1. get record
			System.Data.DataSet ds = this.GetByRowId(RowId);
			//2. get all roles for user in record
			GASystem.DataModel.RolesDS rds = GASystem.DataAccess.Security.GASecurityDb_new.GetUserRolesForContext(new GASystem.DataModel.GADataRecord(RowId, this.DataClass));

			//3. get all listsvalues for record
			FieldDescription[] fds = FieldDefintion.GetFieldDescriptions(this.DataClass);
			ArrayList listsRowIds = new ArrayList();

			foreach(FieldDescription fd in fds) 
			{
                if (fd.ControlType.ToUpper().Trim() == "DROPDOWNLIST" || fd.ControlType.ToUpper().Trim() == "POSTBACKDROPDOWNLIST") 
				{
					if (ds.Tables[0].Columns.Contains(fd.FieldId))
						if (ds.Tables[0].Rows[0][fd.FieldId] != DBNull.Value)
							listsRowIds.Add((int)ds.Tables[0].Rows[0][fd.FieldId]);
				}
			}
			
			if (listsRowIds.Count == 0)     
				return true;				//there is no rowid limiting access


			int[] rowids = new int[listsRowIds.Count];
			listsRowIds.CopyTo(rowids);


			GASystem.DataModel.ListsDS lds = BusinessLayer.Lists.GetListsByListsRowIds(rowids);
			
			//4. check if any value denies user access

			bool hasAccess = true;

			//TODO this should be moved to the business layer
			foreach( GASystem.DataModel.ListsDS.GAListsRow row in lds.GALists.Rows )
			{
				
				//add security check
				if (!row.IsGroup5Null() && row.Group5.ToString() != string.Empty)
				{
						bool userHasRole = false;
						string validRoles = row.Group5.ToString();
						foreach (GASystem.DataModel.RolesDS.RolesRow roleRow in rds.Roles.Rows) 
						{
							if (validRoles.IndexOf(";" + roleRow.RoleID.ToString() + ";") > -1)
								userHasRole = true;
						}
						if (!userHasRole)
							hasAccess = false;
				} 
				
			}

			return hasAccess;

		}
		
		/// <summary>
		/// Updates a recordset. Check updatestatus (new, edit, delete) validates and commit changes.
		/// Does updates in a transaction.
		/// </summary>
		/// <param name="RecordSet"></param>
		/// <param name="Owner"></param>
		/// <returns></returns>
		public DataSet CommitDataSet(DataSet RecordSet, GADataRecord Owner)
		{
			
			
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
            //try
            //{
				DataRow dr = RecordSet.Tables[this.DataClass.ToString()].Rows[0];
				if (RecordSet.Tables[this.DataClass.ToString()].Rows.Count>1) 
				{
					if (dr.RowState==DataRowState.Added) 
					{
						RecordSet = this.SaveNewMultiple(RecordSet, Owner, transaction);
					}
					else
					{
						DataAccess.DataAccess dataAccess = DataAccess.DataAccessFactory.Make(this.DataClass, transaction);
						RecordSet = dataAccess.Update(RecordSet);
						//RecordSet = this.Update(RecordSet, transaction);
					}
				}
				else
				{
                    if (dr.RowState == DataRowState.Added || getRowRowId(dr) == 0)
                    {
                        RecordSet = this.SaveNew(RecordSet, Owner, transaction);
                    }
                    else
                    {
                        RecordSet = this.Update(RecordSet, transaction); // Tor 20161020 OK
                        if (this.IsWorkflowTriggeredWhenUpdated)
                        {
                            Utils.StoreObject.AddMemberClasses(new GADataRecord(getRowRowId(dr), this.DataClass), RecordSet, transaction);
                        }
                    }
				}


                //update galistsselected
                if (RecordSet.Tables.Contains(GADataClass.GAListsSelected.ToString()))
                {
                    int recordRowId = getRowRowId(RecordSet.Tables[this.DataClass.ToString()].Rows[0]);

                    DataSet listSelected = new DataSet();
                    //creating a copy in case any functions expects table to be on index 0
                    listSelected.Tables.Add(RecordSet.Tables[GADataClass.GAListsSelected.ToString()].Copy());
                    UpdateListsSelected(listSelected, new GADataRecord(recordRowId, this.DataClass), transaction);
                }




				transaction.Commit();
            //}
            //catch (GAException gaEx)
            //{
            //    transaction.Rollback();
            //    throw gaEx;
            //}
            //catch (Exception ex)
            //{
            //    transaction.Rollback();
            //    throw ex;
            //}
            //finally 
            //{
            //    transaction.Connection.Close();
            //}
			// Tor slutt
			return RecordSet;
		}

        /// <summary>
        /// commits a dataSet row by row 
        /// </summary>
        /// <param name="DataSet"></param>
        /// <param name="Owner"></param>
        /// <param name="commitAsSingle"></param>
        /// <returns></returns>
        public DataSet CommitDataSet(DataSet DataSet, GADataRecord Owner, bool commitAsSingle)
        {
            GADataTransaction transaction = GADataTransaction.StartGADataTransaction();

            try
            {
                DataTable table = DataSet.Tables[0];

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataTable tempTable = GetClearClonedDataTable(table);
                    DataRow newRow = fillDataRow(tempTable.NewRow(), table.Rows[i], this.DataClass.ToString());
                    tempTable.Rows.Add(newRow);

                    DataSet ds = new DataSet();
                    ds.Tables.Add(tempTable);

                    this.CommitDataSet(ds, Owner, transaction);
                }

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.RollbackAndClose();

                throw;
            }
            finally
            {
                if (transaction != null && transaction.Connection != null)
                {
                    transaction.Connection.Close();
                }
            }

            return DataSet;
        }

        private DataTable GetClearClonedDataTable(DataTable table)
        {
            if (table == null)
                return null;

            DataTable tempTable = table.Clone();

            if (tempTable.PrimaryKey.Length > 0)
            {
                tempTable.PrimaryKey[0].AutoIncrementSeed = -1;
                tempTable.PrimaryKey[0].AutoIncrementStep = -1;
                tempTable.PrimaryKey[0].AutoIncrementSeed = 0;
                tempTable.PrimaryKey[0].AutoIncrementStep = 1;
            }

            tempTable.Clear();

            return tempTable;
        }

        private void UpdateListsSelected(DataSet ds, GADataRecord owner, GADataTransaction transaction) 
		{
            foreach (ListsSelectedDS.GAListsSelectedRow row in ds.Tables[GADataClass.GAListsSelected.ToString()].Rows)
            {
                //if (row.RowState == DataRowState.Added)
                //    DataClassRelations.CreateDataClassRelation(owner.RowId, owner.DataClass, row.ListsSelectedRowId, GADataClass.GAListsSelected, transaction);
                if (row.RowState == DataRowState.Deleted)
                    DataClassRelations.DeleteDataClassRelation(owner, new GADataRecord((int)row["ListsSelectedRowId", DataRowVersion.Original], GADataClass.GAListsSelected), transaction);
            }

            DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(GADataClass.GAListsSelected, transaction);
			DataSet dsresult = gada.Update(ds);

            DataClassRelations.DeleteAllMembersInDataClassRelation(owner, GADataClass.GAListsSelected, transaction);

            foreach (ListsSelectedDS.GAListsSelectedRow row in dsresult.Tables[GADataClass.GAListsSelected.ToString()].Rows)
            {
               // if ( DataClassRelations.GetOwner(new GADataRecord(row.ListsSelectedRowId, GADataClass.GAListsSelected), transaction) == null)
                    DataClassRelations.CreateDataClassRelation(owner.RowId, owner.DataClass, row.ListsSelectedRowId, GADataClass.GAListsSelected, transaction);

            }
            
			
		}



        /// <summary>
        /// Updates a recordset. Check updatestatus (new, edit, delete) validates and commit changes.
        /// Does updates in a transaction.
        /// </summary>
        /// <param name="RecordSet"></param>
        /// <param name="Owner"></param>
        /// <returns></returns>
        public DataSet CommitDataSet(DataSet RecordSet, GADataRecord Owner, GADataTransaction Transaction)
        {
            GADataTransaction transaction = null;
            if (Transaction == null)
                transaction = GADataTransaction.StartGADataTransaction();
            else
                transaction = Transaction;
            try
            {
                DataRow dr = RecordSet.Tables[this.DataClass.ToString()].Rows[0];
                if (RecordSet.Tables[this.DataClass.ToString()].Rows.Count > 1)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        RecordSet = this.SaveNewMultiple(RecordSet, Owner, transaction);
                    }
                    else
                    {
                        DataAccess.DataAccess dataAccess = DataAccess.DataAccessFactory.Make(this.DataClass, transaction);
                        RecordSet = dataAccess.Update(RecordSet);
                        //RecordSet = this.Update(RecordSet, transaction);
                    }
                }
                else
                {
                    if (dr.RowState == DataRowState.Added || getRowRowId(dr) == 0)
                        RecordSet = this.SaveNew(RecordSet, Owner, transaction); // Tor 20161020
                    else
                        RecordSet = this.Update(RecordSet, transaction);
                }
                if (Transaction == null)
                    transaction.Commit();
            }
            catch (GAException gaEx)
            {
                if (Transaction == null)
                    transaction.Rollback();
                throw gaEx;
            }
            catch (Exception ex)
            {
                if (Transaction == null)
                    transaction.Rollback();
                throw ex;
            }
            finally
            {
                if (Transaction == null)
                    transaction.Connection.Close();
            }
            // Tor slutt
            return RecordSet;
        }


		private int getRowRowId(DataRow dr)
		{
			return (int)dr[this.DataClass.ToString().Substring(2)+"RowId"];	
		}
		
		public virtual DataSet GetAllRecordsWithinOwnerAndLinkedRecords(GADataRecord Owner) 
		{
			return GetAllRecordsWithinOwnerAndLinkedRecords(Owner, System.DateTime.MinValue, System.DateTime.MaxValue);
		}

		public virtual DataSet GetAllRecordsWithinOwnerAndLinkedRecords(GADataRecord Owner, string Filter) 
		{
			return GetAllRecordsWithinOwnerAndLinkedRecords(Owner, System.DateTime.MinValue, System.DateTime.MaxValue, Filter);
		}

		public virtual DataSet GetAllRecordsWithinOwnerAndLinkedRecords(GADataRecord Owner, DateTime StartDate, DateTime EndDate) 
		{
			return GetAllRecordsWithinOwnerAndLinkedRecords(Owner, StartDate, EndDate, string.Empty);
		}

		/// <summary>
		/// Return all records of this class type within Onwer. Returns record in the owner tree and records in trees
		/// linked via child records
		/// </summary>
		/// <param name="Owner"></param>
		/// <returns></returns>
		public virtual DataSet GetAllRecordsWithinOwnerAndLinkedRecords(GADataRecord Owner, DateTime StartDate, DateTime EndDate, string Filter)  
		{
			ArrayList ownerRecords = new ArrayList();
			ArrayList tmpOwnerRecord = new ArrayList();
			
			//GADataRecordDate ownerDate = new GADataRecordDate(Owner.RowId, Owner.DataClass);

            BusinessClass bcManyToMany = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(Owner.DataClass);
            GADataRecordDate ownerDate = bcManyToMany.GetDataRecordWithDate(Owner.RowId);                                //new GADataRecordDate((int)row[cd.ManyToManyField], lookupTableDataClass);
            if (StartDate > ownerDate.StartDate)
                ownerDate.StartDate = StartDate;
            if (EndDate < ownerDate.EndDate)
                ownerDate.EndDate = EndDate;
				


			//ownerDate.StartDate = StartDate;
			//ownerDate.EndDate = EndDate;

			tmpOwnerRecord.Add(ownerDate);
			//find child classes of type many to many
			
			//get first level
			ownerRecords = GetManyToManyRecords(tmpOwnerRecord);
			//get next level
			ownerRecords.AddRange(GetManyToManyRecords(ownerRecords));
			
			
			//get all record of dataclass for linked records
			DataSet ds = Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(this.DataClass, Owner, Filter, ownerDate.StartDate, ownerDate.EndDate);
			
			foreach(GADataRecordDate garecord in ownerRecords) 
			{
				DataSet dstmp = Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(this.DataClass, (GADataRecord)garecord, Filter, garecord.StartDate, garecord.EndDate);
				ds.Merge(dstmp.Tables[this.DataClass.ToString()]);
			}

			//check if any of the links are of wanted type, and add it.
			foreach (GADataRecordDate garecord in ownerRecords) 
			{
				if (garecord.DataClass == this.DataClass) 
				{
					ds.Merge(Utils.RecordsetFactory.GetRecordSetAllDetailsByDataRecord((GADataRecord)garecord).Tables[this.DataClass.ToString()]);
				}
			}
			
			return ds;
		}


		/// <summary>
		/// Return all records of this class type within Onwer. Returns record in the owner tree and records in trees
		/// linked via child records. Returns records for a single table.
		/// </summary>
		/// <param name="Owner"></param>
		/// <returns></returns>
		public virtual DataSet GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(GADataRecord Owner, DateTime StartDate, DateTime EndDate, string Filter)  
		{
			ArrayList ownerRecords = new ArrayList();
			ArrayList tmpOwnerRecord = new ArrayList();
			
			GADataRecordDate ownerDate = new GADataRecordDate(Owner.RowId, Owner.DataClass);

			ownerDate.StartDate = StartDate;
			ownerDate.EndDate = EndDate;



			tmpOwnerRecord.Add(ownerDate);
			//find child classes of type many to many
			
			//get first level
			ownerRecords = GetManyToManyRecords(tmpOwnerRecord);
			//get next level
			ownerRecords.AddRange(GetManyToManyRecords(ownerRecords));
			
			//get all record of dataclass for linked records
			DataSet ds = Utils.RecordsetFactory.GetRecordSetForDataClassWithinOwner(this.DataClass, Owner, Filter, ownerDate.StartDate, ownerDate.EndDate);
			
			
			foreach(GADataRecordDate garecord in ownerRecords) 
			{
				DataSet dstmp = Utils.RecordsetFactory.GetRecordSetForDataClassWithinOwner(this.DataClass, (GADataRecord)garecord, Filter, garecord.StartDate, garecord.EndDate);
				ds.Merge(dstmp.Tables[this.DataClass.ToString()]);
			}

			//check if any of the links are of wanted type, and add it.
			foreach (GADataRecordDate garecord in ownerRecords) 
			{
				if (garecord.DataClass == this.DataClass) 
				{
					ds.Merge(Utils.RecordsetFactory.GetRecordSetByDataRecord((GADataRecord)garecord).Tables[this.DataClass.ToString()]);
				}
			}
			
			return ds;
		}


        ///// <summary>
        ///// Return all records of this class type within Onwer. Returns record in the owner tree and records in trees
        ///// linked via child records. Returns records for a single table.
        ///// In addition is information for all dropdowns used in table included in the dataset
        ///// </summary>
        ///// <param name="Owner"></param>
        ///// <param name="StartDate"></param>
        ///// <param name="EndDate"></param>
        ///// <param name="Filter"></param>
        ///// <returns></returns>
        //public virtual DataSet GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(GADataRecord Owner, DateTime StartDate, DateTime EndDate, string Filter)
        //{
        //    //get main data
        //    DataSet ds = GetAllRecordsForGAListViewWithinOwnerAndLinkedRecords(Owner, StartDate, EndDate, Filter);
            
        //    //add lookup data
        //    FieldDescription[] fds = FieldDefintion.GetFieldDescriptions(this.DataClass);
        //    foreach (FieldDescription fd in fds) {
        //        if (fd.ControlType.ToUpper() == "DROPDOWNLIST")
        //        {
        //            System.Data.DataSet dsl = BusinessLayer.Lists.GetListsRowIdByCategory(fd.ListCategory);
        //            dsl.Tables[0].TableName = "galists_" + fd.ListCategory;
        //            ds.Merge(dsl.Tables[0]);
        //        }
        //    }

        //    return ds;
        //}


        /// <summary>
        /// Return all records of this class type within Onwer. Returns record in the owner tree and records in trees
        /// linked via child records. Returns records for a single table.
        /// </summary>
        /// <param name="Owner"></param>
        /// <returns></returns>
        public virtual DataSet GetAllRecordsForGAListViewWithinOwnerAndLinkedRecords(GADataRecord Owner, DateTime StartDate, DateTime EndDate, string Filter)
        {
            ArrayList ownerRecords = new ArrayList();
            ArrayList tmpOwnerRecord = new ArrayList();

            GADataRecordDate ownerDate = new GADataRecordDate(Owner.RowId, Owner.DataClass);

            ownerDate.StartDate = StartDate;
            ownerDate.EndDate = EndDate;



            tmpOwnerRecord.Add(ownerDate);
            //find child classes of type many to many

            //get first level
            ownerRecords = GetManyToManyRecords(tmpOwnerRecord);
            //get next level
            ownerRecords.AddRange(GetManyToManyRecords(ownerRecords));

            //get all record of dataclass for linked records
            DataSet ds = Utils.RecordsetFactory.GetRecordSetListViewAllDetailsForDataClassWithinOwner(this.DataClass, Owner, Filter, ownerDate.StartDate, ownerDate.EndDate);


            foreach (GADataRecordDate garecord in ownerRecords)
            {
                DataSet dstmp = Utils.RecordsetFactory.GetRecordSetListViewAllDetailsForDataClassWithinOwner(this.DataClass, (GADataRecord)garecord, Filter, garecord.StartDate, garecord.EndDate);
                ds.Merge(dstmp.Tables[this.DataClass.ToString()]);
            }

            ////check if any of the links are of wanted type, and add it.
            foreach (GADataRecordDate garecord in ownerRecords)
            {
                if (garecord.DataClass == this.DataClass)
                {
                    ds.Merge(Utils.RecordsetFactory.GetRecordSetListViewAllDetailsByDataRecord( (GADataRecord)garecord)); //   DataAccess.ReportView.GetRecordSetAllDetailsByDataRecord((GADataRecord)garecord));  //Utils.RecordsetFactory.GetRecordSetByDataRecord((GADataRecord)garecord).Tables[this.DataClass.ToString()]);
                }
            }

            return ds;
        }

		private ArrayList GetManyToManyRecords(ArrayList ownerRecords) 
		{
			ArrayList foundOwnerRecords = new ArrayList();
			//iterate through all ownerrecords
			foreach (object datarecord in ownerRecords) 
			{
				//find and check all next level classes
				ArrayList dataClasses = BusinessLayer.DataClassRelations.GetNextLevelDataClasses(((GADataRecord)datarecord).DataClass);
				foreach (string dataClassString in dataClasses) 
				{
					GADataClass dataClass = GADataRecord.ParseGADataClass(dataClassString);
					ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(dataClass);
					if (cd.isClassManyToManyLink()) 
					{
						//class is a link to other records. Get records and add to found records
						foundOwnerRecords.AddRange(GetLinkedManyToManyRecords(cd, (GADataRecordDate)datarecord));
					}
				}
			}

			


			return foundOwnerRecords;
		}

		private ArrayList GetLinkedManyToManyRecords(ClassDescription cd, GADataRecordDate Owner) 
		{
			//select records of manytomany classes
			ArrayList dataRecords = new ArrayList();
			GADataClass dataClass = GADataRecord.ParseGADataClass(cd.DataClassName);
			BusinessClass bc = 	BusinessLayer.Utils.RecordsetFactory.Make(dataClass);
			DataSet ds = bc.GetByOwnerAndTimeSpan(Owner, Owner.StartDate, Owner.EndDate, null);
//			bc.GetByOwner(
			//get field description for the linked table
			GASystem.AppUtils.FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(cd.ManyToManyField, cd.DataClassName);
			string lookupTable = fd.LookupTable;
			//string lookupTableKeyField = fd.LookupTableKey;

			//check for errors, if lookupTableKeyField does not exist or lookuptable is not defined return with empty data;
			if (lookupTable == string.Empty || !ds.Tables[dataClass.ToString()].Columns.Contains(cd.ManyToManyField))
				return dataRecords;

			GADataClass lookupTableDataClass = GADataRecord.ParseGADataClass(lookupTable);

			foreach (DataRow row in ds.Tables[dataClass.ToString()].Rows)
			{
				//build table of links, with date
				
                BusinessClass bcManyToMany = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(lookupTableDataClass);
				GADataRecordDate recordWithDate = bcManyToMany.GetDataRecordWithDate((int)row[cd.ManyToManyField]);                                //new GADataRecordDate((int)row[cd.ManyToManyField], lookupTableDataClass);
				if (Owner.StartDate >  recordWithDate.StartDate)
                    recordWithDate.StartDate = Owner.StartDate;
                if (Owner.EndDate < recordWithDate.EndDate)
				    recordWithDate.EndDate = Owner.EndDate;
				
				//set enddate
				if (cd.hasDateToField()) 
					if(ds.Tables[dataClass.ToString()].Columns.Contains(cd.DateToField) && row[cd.DateToField] != DBNull.Value) 
					{
						if ((DateTime)row[cd.DateToField] < Owner.EndDate)
							recordWithDate.EndDate = (DateTime)row[cd.DateToField];
					}

				//set startdate
				if (cd.hasDateFromField()) 
					if(ds.Tables[dataClass.ToString()].Columns.Contains(cd.DateFromField) && row[cd.DateFromField] != DBNull.Value) 
					{
						if ((DateTime)row[cd.DateFromField] > Owner.StartDate)
							recordWithDate.StartDate = (DateTime)row[cd.DateFromField];
					}
				dataRecords.Add(recordWithDate);
								
			}

			return dataRecords;
		}

        public GADataRecordDate GetDataRecordWithDate(int rowid)
        {
            GADataRecordDate dataRecord = new GADataRecordDate(rowid, this.DataClass);
            //check for default start and end time
            //TODO: should this class or functionality be moved to a different namespace?
            GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataClass);
            if (cd.DateFromField != string.Empty && cd.DateToField != string.Empty)
            {
                DataSet ds = this.GetByRowId(rowid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Columns.Contains(cd.DateToField))
                        if (ds.Tables[0].Rows[0][cd.DateToField] != DBNull.Value)
                            dataRecord.StartDate = (DateTime)ds.Tables[0].Rows[0][cd.DateToField];

                    if (ds.Tables[0].Columns.Contains(cd.DateFromField))
                        if (ds.Tables[0].Rows[0][cd.DateFromField] != DBNull.Value)
                            dataRecord.StartDate = (DateTime)ds.Tables[0].Rows[0][cd.DateFromField];

                }
            }
            return dataRecord;
        }

        public virtual ArrayList GetDistinctColumn(string columnname, string filter, GADataTransaction transaction)
        {
            ArrayList list = new ArrayList();
            
            DataAccess.DataAccess GAData = new GASystem.DataAccess.DataAccess(this.DataClass, transaction);
			DataSet ds = GAData.GetDistinctColumn(columnname, filter);
		
            if (ds.Tables.Count > 0)
                foreach (DataRow row in ds.Tables[0].Rows)
                    list.Add(row[columnname].ToString());
            return list;
        }

        // Tor 20150626 added to get rowid by column value
        public static ArrayList GetRowIdFromClassWithFilter(string ClassName, string columnname, string filter, GADataTransaction transaction)
            //GetDistinctColumn(string columnname, string filter, GADataTransaction transaction)
        {
            ArrayList list = new ArrayList();
            
//            DataAccess.DataAccess GAData = new GASystem.DataAccess.DataAccess(this.DataClass, transaction);
            DataAccess.DataAccess GAData = new GASystem.DataAccess.DataAccess(GADataRecord.ParseGADataClass(ClassName),transaction);

            DataSet ds =GAData.GetRowIdFromClassWithFilter(ClassName,columnname,filter);
            if (ds.Tables.Count > 0)
                foreach (DataRow row in ds.Tables[0].Rows)
                    list.Add(row[columnname].ToString());
            return list;
        }


        // Tor 20150310 added for ConsolConsumer to list all current at date owners above memberdatarecord including many to many records
        // when date is null, disregard date test
        // when ownerHasMemberDataClass, disregard owner has memberdataclass test
        public static System.Collections.Generic.List<GADataRecord> GetOwnerRecordsWithMemberClassByMemberDataRecordAndDate
            (GADataRecord memberDataRecord, 
            string ownerHasMemberDataClass,
            System.DateTime date)
        { 
            System.Collections.Generic.List<GADataRecord> ownerRecords = new System.Collections.Generic.List<GADataRecord>();
            // get all direct owners

            // get all direct Owners

            // get all manyToMany Owners

            return ownerRecords;
        }

        //moved from editDataRecord to reuse it sep-2020
        public static DataRow fillDataRow(DataRow targetRow, DataRow sourceRow, string rowDataClass)
        {
            string dataClassRowIdName = rowDataClass.Substring(2) + "RowId";

            foreach (DataColumn column in sourceRow.Table.Columns)
            {
                //don't copy rowid field (it is autoincrement, readonly)
                if (!column.ColumnName.Equals(dataClassRowIdName))
                {
                    targetRow[column.ColumnName] = sourceRow[column.ColumnName];
                }
            }
            return targetRow;
        }
	}
}
