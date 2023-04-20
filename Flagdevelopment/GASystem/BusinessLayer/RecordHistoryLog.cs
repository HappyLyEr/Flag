using System;
using System.Data;
using GASystem.DataModel;
using GASystem.DataAccess;
using GASystem.AppUtils;
using GASystem.BusinessLayer;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for RecordHistoryLog.
	/// </summary>
	public class RecordHistoryLog : BusinessClass // Tor 20150108 new class
	{
        public RecordHistoryLog()
		{
			//
			// TODO: Add constructor logic here
			//
            DataClass = GADataClass.GARecordHistoryLog;
		}

        public static RecordHistoryLogDS GetAllRecordHistoryLogsSorted()
        {
            return RecordHistoryLogDb.GetAllRecordHistoryLogsSorted();
        }
        public static RecordHistoryLogDS GetAllRecordHistoryLogsToPersonSorted()
        {
            return RecordHistoryLogDb.GetAllRecordHistoryLogsToPersonSorted();
        }
        public static RecordHistoryLogDS GetAllRecordHistoryLogsToRoleSorted()
        {
            return RecordHistoryLogDb.GetAllRecordHistoryLogsToRoleSorted();
        }
        public static RecordHistoryLogDS GetNewRecordHistoryLog()
		{
			RecordHistoryLogDS iDS = new RecordHistoryLogDS();
			GASystem.DataModel.RecordHistoryLogDS.GARecordHistoryLogRow row = iDS.GARecordHistoryLog.NewGARecordHistoryLogRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GARecordHistoryLog.Rows.Add(row);
			return iDS;
		}

		public static RecordHistoryLogDS SaveNewRecordHistoryLog(RecordHistoryLogDS RecordHistoryLogSet, GADataRecord RecordHistoryLogOwner)
		{
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				RecordHistoryLogSet = UpdateRecordHistoryLog(RecordHistoryLogSet, transaction);
                // Tor 20150108 NO superclass record for GAHistoryLog DataClassRelations.CreateDataClassRelation(RecordHistoryLogOwner.RowId, RecordHistoryLogOwner.DataClass, RecordHistoryLogSet.GARecordHistoryLog[0].RecordHistoryLogRowId, GADataClass.GARecordHistoryLog, transaction);
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
			return RecordHistoryLogSet;
		}

		public static RecordHistoryLogDS UpdateRecordHistoryLog(RecordHistoryLogDS RecordHistoryLogSet, GADataTransaction transaction)
		{
            // Tor 20141208 Changed return RecordHistoryLogDb.UpdateRecordHistoryLog(RecordHistoryLogSet, transaction);
            // to code below
            GADataTransaction transactionLocal = GADataTransaction.StartGADataTransaction();
            try
            {
                DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(GADataRecord.ParseGADataClass("GARecordHistoryLog"), transactionLocal);
                System.Data.DataSet dsresult = gada.Update(RecordHistoryLogSet);
                transactionLocal.Commit();
            }
            catch (Exception ex)
            {
                transactionLocal.Rollback();
                throw ex;
            }
            finally
            {
                transactionLocal.Connection.Close();
            }
            return RecordHistoryLogSet;

//			return RecordHistoryLogDb.UpdateRecordHistoryLog(RecordHistoryLogSet, transaction);
		}

        //public static void UpdateRecordHistoryLog(GADataRecord owner, GADataRecord member, GADataTransaction transaction)
        //{
        //    // check in GASuperClassLinks if new record is to be reported
        //    //GADataClass ownerDC = new GADataClass(owner.DataClass);
        //    //GADataClass memberDC = new GADataClass(member.DataClass);
        //    //ownerDC = GADataRecord.ParseGADataClass(owner.DataClass.ToString());

        //    SuperClassLinksDS sds = GASystem.DataAccess.SuperClassDb.GetSuperClassLinksByOwnerAndMember(GADataRecord.ParseGADataClass(owner.DataClass.ToString()),GADataRecord.ParseGADataClass(member.DataClass.ToString()));
        //    //if (ds.Tables[0].Rows[0][fieldId.FieldId] )
        //    if ((bool)sds.Tables[0].Rows[0]["SwitchFree3"])
        //    {


        //    }
        //}

        /// <summary>
        /// Summary description for RecordHistoryLog.createRecordHistoryLogFromNewOrUpdatedRecord
        /// </summary>
        public static void createRecordHistoryLogFromNewOrUpdatedRecord(DataSet ds, GASystem.DataModel.GADataRecord Owner,GADataTransaction transaction)
        {
            GADataClass m_dataClass=GADataRecord.ParseGADataClass(ds.Tables[0].ToString());// Owner.DataClass.ToString());;
            GADataClass _dataClass=m_dataClass;

            System.Collections.ArrayList columns = new System.Collections.ArrayList();
            foreach (DataColumn column in ds.Tables[_dataClass.ToString()].Columns)
            {
                if (column.ColumnName.ToLower() != "rowguid")
                    if (column.ColumnName.ToLower() != "[rowguid]")
                        columns.Add(column.ColumnName);
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                //dette skal flyttes opp på business!!!
                if (row.RowState == DataRowState.Added)
                { // new record
                    if (Owner != null)
                    { //owner record exists - Get GASuperClassLinks record to check if new record is to be reported
                        GADataClass ownerdataClass = GADataRecord.ParseGADataClass(Owner.DataClass.ToString());
                        if (GASystem.BusinessLayer.Utils.RecordSetUtils.IfReportOnOwnerMemberDataClass(_dataClass, ownerdataClass))
                        { // report on new owner/member dataclass
                            // Tor 20141229 check if Class is to be reported on (GAClass.Caption_nb_No is not empty) and test for each column if it is in the report column list (GAClass.Caption_nb_No)
                            //                    GADataSet.Tables[0].TableName
                            //                    this._dataClass
                            // create new historyLog entry
                            // if table is to be reported on
                            // string myOwnerDataClass = ownerdataClass.ToString();
                            // get owner record and classdescription (to retrieve owner name attribute later)
                            // Tor 20150409 test
                            ClassDescription ocd = ClassDefinition.GetClassDescriptionByGADataClass(ownerdataClass.ToString());
                            DateTime dateTimeUpdate = DateTime.UtcNow;
                            // get ownerclass fielddefinition and owner record to find who to report to
                            FieldDescription[] ofdList = GASystem.AppUtils.FieldDefintion.GetFieldDescriptions(ownerdataClass); //( myOwnerDataClass);
                            DataSet ods = GetByGADataRecord(Owner, transaction);
                            System.Collections.ArrayList reportToPerson = new System.Collections.ArrayList(); // field to get values from
                            System.Collections.ArrayList reportToRole = new System.Collections.ArrayList();   // field to get values from

                            // find report to in GAFieldDefinitions
                            foreach (FieldDescription fieldId in ofdList)
                            {
                                //                                if (fieldId.ReportUpdateTo && (int)row[fieldId.FieldId]>0)
                                if (fieldId.ReportUpdateTo && ((int)ods.Tables[0].Rows[0][fieldId.FieldId] > 0))

                                    if (fieldId.LookupTable != string.Empty) //(fieldId.ListCategory=string.Empty) //&& (fieldId.LookupTable!=string.Empty))
                                    {
                                        reportToPerson.Add(ods.Tables[0].Rows[0][fieldId.FieldId]);
                                        //                                            reportToPerson.Add(row[fieldId.FieldId]);
                                    }
                                    else
                                    {
                                        if (fieldId.ListCategory != string.Empty) // && (fieldId.LookupTable=string.Empty))
                                            //                                        reportToRole.Add(fieldId.FieldId);
                                            reportToRole.Add(ods.Tables[0].Rows[0][fieldId.FieldId]);
                                        //                                            reportToRole.Add(row[fieldId.FieldId]);
                                    }
                            }
                            // find report to in GAClass
                            if (ocd.ReportRecordChangesToRole != string.Empty)
                            {
                                string[] report2Role = ocd.ReportRecordChangesToRole.Split(';');
                                //                            System.Collections.ArrayList reportToRolesFromcd = new System.Collections.ArrayList(); // RoleListsRowId
                                foreach (string s in report2Role)
                                {
                                    if (s != string.Empty)
                                    {
                                        // get ListsRowId from GALists and add to list
                                        // Tor 20170313 Responsible changed from Role ER to Title TITL  if (Lists.GetListsRowIdByCategoryAndKey("er", s) > 0)
                                        if (Lists.GetListsRowIdByCategoryAndKey("TITL", s) > 0)
                                        {
                                            // Tor 20170313 Responsible changed from Role ER to Title TITL reportToRole.Add(Lists.GetListsRowIdByCategoryAndKey("er", s));
                                            reportToRole.Add(Lists.GetListsRowIdByCategoryAndKey("TITL", s));
                                        }
                                    }
                                }
                            }
                            //foreach (DataColumn column in ds.Tables[_dataClass.ToString()].Columns)
                            //{
                            //    if (column.ColumnName.ToLower() != "rowguid")
                            //        if (column.ColumnName.ToLower() != "[rowguid]")
                            //            columns.Add(column.ColumnName);
                            //}

                            // anybody to report NEW RECORD to ?

                            if (reportToRole.Count > 0 || reportToPerson.Count > 0)
                            { // report new record, fill recordHistoryLog attributes:
                                //reportToRoleListsRowId,
                                //reportToPersonnelRowId, 
                                //reportOnClass, x
                                //reportOnClassRowId, x
                                //classOwner, x
                                //classOwnerRowId x
                                //classMember x
                                //isNewRecord x
                                //TextFree1 (newRecordHeader) x


                                string myReportOnClass = ownerdataClass.ToString();
                                int myReportOnClassRowId = Owner.RowId; // (int)ds.Tables[0].Rows[0][_dataClass.ToString().Substring(2) + "RowId"];
                                //SuperClassDS ods = (new SuperClassDb()).GetSuperClassByMember(reportOnClassRowId, _dataClass);
                                // class owner is owner of reportOnOwner which is Owner
                                // get reportOnClass owner record 
                                GADataRecord myClassOwnerRecord = GASystem.BusinessLayer.DataClassRelations.GetOwner(Owner);
                                string myClassOwner = myClassOwnerRecord.DataClass.ToString();
                                int myClassOwnerRowId = myClassOwnerRecord.RowId;
                                bool myIsNewRecord = true;
                                int myChangedByPersonnelRowId = GASystem.DataAccess.Security.GASecurityDb_new.GetPersonnelRowIdByLogonId(GASystem.DataAccess.Security.GASecurityDb_new.GetCurrentUserId().ToString());
                                ClassDescription mcd = ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
                                string myNewRecordHeader = row[mcd.ObjectName].ToString();
                                string myMemberDataClass = _dataClass.ToString();
                                string myDataClass = ds.Tables[0].TableName.ToString();

                                foreach (int personnelRowid in reportToPerson)
                                {
                                    RecordHistoryLog RecordHistoryLog = new RecordHistoryLog();
                                    RecordHistoryLogDS rds = (RecordHistoryLogDS)RecordHistoryLog.GetNewRecordHistoryLog();
                                    RecordHistoryLogDS.GARecordHistoryLogRow newRow = (RecordHistoryLogDS.GARecordHistoryLogRow)rds.GARecordHistoryLog.Rows[0];
                                    newRow.reportOnClass = myReportOnClass;// reportOnClass;
                                    newRow.reportOnClassRowId = myReportOnClassRowId; // reportOnClassRowId;
                                    newRow.classOwner = myClassOwner; // ownerdataClass.ToString();// myOwnerDataClass; // classOwner;
                                    newRow.classOwnerRowId = myClassOwnerRowId; // classOwnerRowId;
                                    newRow.classMember = myMemberDataClass; // _dataClass.ToString(); // reportOnClass;
                                    newRow.classMemberRowId = 0; // non existant for new records
                                    newRow.isNewRecord = myIsNewRecord; // isNewRecord;
                                    //  newRow.FieldId = FieldId;
                                    //   newRow.OldAttributeValue = OldAttributeValue;
                                    // get ownerClass GAClass.ObjectName field
                                    //newRow.NewAttributeValue = NewAttributeValue;
                                    newRow.DateTimeFieldIdChanged = dateTimeUpdate;
                                    newRow.ChangedByPersonnelRowId = myChangedByPersonnelRowId;
                                    newRow.TextFree1 = myNewRecordHeader;
                                    //newRow.nTextFree3 = nTextFree3; // job roles from GAClass
                                    newRow.ViewTypeListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("RT", "GARecordHistoryLog");
                                    newRow.reportToPersonnelRowId = personnelRowid;
                                    RecordHistoryLog.UpdateRecordHistoryLog(rds, transaction);
                                }

                                foreach (int roleListsRowId in reportToRole)
                                {
                                    RecordHistoryLog RecordHistoryLog = new RecordHistoryLog();
                                    RecordHistoryLogDS rds = (RecordHistoryLogDS)RecordHistoryLog.GetNewRecordHistoryLog();
                                    RecordHistoryLogDS.GARecordHistoryLogRow newRow = (RecordHistoryLogDS.GARecordHistoryLogRow)rds.GARecordHistoryLog.Rows[0];
                                    newRow.reportOnClass = myReportOnClass;// reportOnClass;
                                    newRow.reportOnClassRowId = myReportOnClassRowId; // reportOnClassRowId;
                                    newRow.classOwner = myClassOwner; // ownerdataClass.ToString();// myOwnerDataClass; // classOwner;
                                    newRow.classOwnerRowId = myClassOwnerRowId; // classOwnerRowId;
                                    newRow.classMember = myMemberDataClass; // _dataClass.ToString(); // reportOnClass;
                                    newRow.classMemberRowId = 0; // non existant for new records
                                    newRow.isNewRecord = myIsNewRecord; // isNewRecord;
                                    //  newRow.FieldId = FieldId;
                                    //   newRow.OldAttributeValue = OldAttributeValue;
                                    // get ownerClass GAClass.ObjectName field
                                    //newRow.NewAttributeValue = NewAttributeValue;
                                    newRow.DateTimeFieldIdChanged = dateTimeUpdate;
                                    newRow.ChangedByPersonnelRowId = myChangedByPersonnelRowId;
                                    newRow.TextFree1 = myNewRecordHeader;
                                    //newRow.nTextFree3 = nTextFree3; // job roles from GAClass
                                    newRow.ViewTypeListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("RT", "GARecordHistoryLog");
                                    newRow.reportToRoleListsRowId = roleListsRowId;
                                    RecordHistoryLog.UpdateRecordHistoryLog(rds, transaction);
                                }
                            }
                        }
                    }

                    //if (!_security.HasCreateInContext( ))
                    // throw new GASecuirtyException();
                }
                if (row.RowState == DataRowState.Modified)
                {
                    // if (!_security.HasUpdateOnDataRecord())
                    //	throw new GASecurityException();
                    // get table 
                    // Tor 20141229 check if Class is to be reported on (GAClass.Caption_nb_No is not empty) and test for each column if it is in the report column list (GAClass.Caption_nb_No)
                    //                    GADataSet.Tables[0].TableName
                    //                    this._dataClass
                    // create new historyLog entry
                    // if table is to be reported on
                    string myDataClass = ds.Tables[0].TableName.ToString();
                    ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(_dataClass);
                    if (cd.ReportUpdatesToRecordHistoryLog)
                    {
                        DateTime dateTimeUpdate = DateTime.UtcNow;
                        FieldDescription[] fdList = GASystem.AppUtils.FieldDefintion.GetFieldDescriptions(_dataClass.ToString());
                        System.Collections.ArrayList reportToPerson = new System.Collections.ArrayList(); // field to get values from
                        System.Collections.ArrayList reportToRole = new System.Collections.ArrayList();   // field to get values from

                        // find report to in GAFieldDefinitions
                        foreach (FieldDescription fieldId in fdList)
                        {
                            // Tor 20181231 if (fieldId.ReportUpdateTo && ((int)ds.Tables[0].Rows[0][fieldId.FieldId] > 0))
                            if (fieldId.ReportUpdateTo
                                && ds.Tables[0].Rows[0][fieldId.FieldId] != DBNull.Value
                                && ((int)ds.Tables[0].Rows[0][fieldId.FieldId] > 0))
                                if (fieldId.LookupTable != string.Empty) //(fieldId.ListCategory=string.Empty) //&& (fieldId.LookupTable!=string.Empty))
                                {
                                    reportToPerson.Add(ds.Tables[0].Rows[0][fieldId.FieldId]);
                                }
                                else
                                {
                                    if (fieldId.ListCategory != string.Empty) // && (fieldId.LookupTable=string.Empty))
                                        //                                        reportToRole.Add(fieldId.FieldId);
                                        reportToRole.Add(ds.Tables[0].Rows[0][fieldId.FieldId]);
                                }
                        }

                        // find report to in GAClass
                        if (cd.ReportRecordChangesToRole != string.Empty)
                        {
                            string[] report2Role = cd.ReportRecordChangesToRole.Split(';');
                            //                            System.Collections.ArrayList reportToRolesFromcd = new System.Collections.ArrayList(); // RoleListsRowId
                            foreach (string s in report2Role)
                            {
                                if (s != string.Empty)
                                {
                                    // get ListsRowId from GALists and add to list
                                    // Tor 20170313 Responsible changed from Role ER to Title TITL  if (Lists.GetListsRowIdByCategoryAndKey("er", s) > 0) if (Lists.GetListsRowIdByCategoryAndKey("er", s) > 0)
                                    if (Lists.GetListsRowIdByCategoryAndKey("TITL", s) > 0)
                                    {
                                        // Tor 20170313 Responsible changed from Role ER to Title TITL  reportToRole.Add(Lists.GetListsRowIdByCategoryAndKey("er", s));
                                        reportToRole.Add(Lists.GetListsRowIdByCategoryAndKey("TITL", s));
                                    }
                                }
                            }
                        }
                        foreach (DataColumn column in ds.Tables[_dataClass.ToString()].Columns)
                        {
                            if (column.ColumnName.ToLower() != "rowguid")
                                if (column.ColumnName.ToLower() != "[rowguid]")
                                    columns.Add(column.ColumnName);
                        }

                        // anybody to report to ?
                        if (reportToRole.Count > 0 || reportToPerson.Count > 0)
                        {
                            foreach (string columnName in columns)
                                if (row[columnName, DataRowVersion.Original].ToString() != row[columnName, DataRowVersion.Current].ToString())
                                {
                                    FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(columnName, _dataClass.ToString());
                                    // if Table/Column is to be reported on
                                    if (fd != null && fd.ReportOnFieldUpdate)
                                    // Tor 20170327 do not report on field if not defined in GAFieldDefinitions if (fd.ReportOnFieldUpdate)
                                    {
                                        // then create new historyLog entry with values
                                        string reportOnClass = _dataClass.ToString();
                                        int reportOnClassRowId = (int)ds.Tables[0].Rows[0][_dataClass.ToString().Substring(2) + "RowId"];
                                        SuperClassDS ods = (new SuperClassDb()).GetSuperClassByMember(reportOnClassRowId, _dataClass, transaction);
                                        string classOwner = ods.Tables[0].Rows[0]["OwnerClass"].ToString();
                                        int classOwnerRowId = Convert.ToInt32(ods.Tables[0].Rows[0]["OwnerClassRowId"]);
                                        bool isNewRecord = false;
                                        string FieldId = columnName;
                                        string OldAttributeValue = row[columnName, DataRowVersion.Original].ToString();
                                        string NewAttributeValue = row[columnName, DataRowVersion.Current].ToString();
                                        int ChangedByPersonnelRowId = GASystem.DataAccess.Security.GASecurityDb_new.GetPersonnelRowIdByLogonId(GASystem.DataAccess.Security.GASecurityDb_new.GetCurrentUserId().ToString());
                                        string nTextFree3 = cd.ReportRecordChangesToRole.ToString(); // job roles from GAClass

                                        foreach (int personnelRowid in reportToPerson)
                                        {
                                            RecordHistoryLog RecordHistoryLog = new RecordHistoryLog();
                                            RecordHistoryLogDS rds = (RecordHistoryLogDS)RecordHistoryLog.GetNewRecordHistoryLog();
                                            RecordHistoryLogDS.GARecordHistoryLogRow newRow = (RecordHistoryLogDS.GARecordHistoryLogRow)rds.GARecordHistoryLog.Rows[0];
                                            newRow.reportOnClass = reportOnClass;
                                            newRow.reportOnClassRowId = reportOnClassRowId;
                                            newRow.classOwner = classOwner;
                                            newRow.classOwnerRowId = classOwnerRowId;
                                            newRow.classMember = reportOnClass;
                                            newRow.classMemberRowId = reportOnClassRowId;
                                            newRow.isNewRecord = isNewRecord;
                                            newRow.FieldId = FieldId;
                                            newRow.OldAttributeValue = OldAttributeValue;
                                            newRow.NewAttributeValue = NewAttributeValue;
                                            newRow.DateTimeFieldIdChanged = dateTimeUpdate;
                                            newRow.ChangedByPersonnelRowId = ChangedByPersonnelRowId;
                                            newRow.nTextFree3 = nTextFree3; // job roles from GAClass
                                            newRow.reportToPersonnelRowId = personnelRowid;
                                            newRow.ViewTypeListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("RT", "GARecordHistoryLog");
                                            RecordHistoryLog.UpdateRecordHistoryLog(rds, transaction);
                                        }

                                        foreach (int roleListsRowId in reportToRole)
                                        {
                                            RecordHistoryLog RecordHistoryLog = new RecordHistoryLog();
                                            RecordHistoryLogDS rds = (RecordHistoryLogDS)RecordHistoryLog.GetNewRecordHistoryLog();
                                            RecordHistoryLogDS.GARecordHistoryLogRow newRow = (RecordHistoryLogDS.GARecordHistoryLogRow)rds.GARecordHistoryLog.Rows[0];
                                            newRow.reportOnClass = reportOnClass;
                                            newRow.reportOnClassRowId = reportOnClassRowId;
                                            newRow.classOwner = classOwner;
                                            newRow.classOwnerRowId = classOwnerRowId;
                                            newRow.classMember = reportOnClass;
                                            newRow.classMemberRowId = reportOnClassRowId;
                                            newRow.isNewRecord = isNewRecord;
                                            newRow.FieldId = FieldId;
                                            newRow.OldAttributeValue = OldAttributeValue;
                                            newRow.NewAttributeValue = NewAttributeValue;
                                            newRow.DateTimeFieldIdChanged = dateTimeUpdate;
                                            newRow.ChangedByPersonnelRowId = ChangedByPersonnelRowId;
                                            // find report to in GAClass
                                            newRow.nTextFree3 = nTextFree3; // job roles from GAClass
                                            newRow.reportToRoleListsRowId = roleListsRowId;
                                            newRow.ViewTypeListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("RT", "GARecordHistoryLog");
                                            RecordHistoryLog.UpdateRecordHistoryLog(rds, transaction);
                                        }

                                        // save record for all receivers.
                                        // Tor 20141229 write to historylog Tablename, ColumnName, old value, new value, datatimeUTC

                                    }
                                }
                        }
                    }
                }
            }

        }


	}
}
