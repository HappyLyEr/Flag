using System;
using System.Data;
using GASystem.DataModel;
using GASystem.DataAccess;
using log4net;
using System.Collections;
using GASystem.DataAccess.Security;
using GASystem.BusinessLayer;
using GASystem.BusinessLayer.Utils;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer
{
    public class CreateRecordFromClass : BusinessClass
    {

        public static void CreateRecordFromClassRecord(CreateRecordFromClassDS dsFromTo, GADataRecord ownersOwner, GADataRecord recordToCopyFrom
            , DataSet dsFrom
            , int fromClass, int toClass, string copyToClass)
        // create toClass record from fromClass record
        {
            GASystem.DataAccess.GADataTransaction transaction = GASystem.DataAccess.GADataTransaction.StartGADataTransaction();
            // create new record including setting default values
            BusinessClass bc = RecordsetFactory.Make(GADataRecord.ParseGADataClass(copyToClass));
            DataSet newRecord = bc.GetNewRecord(ownersOwner, transaction);
            int intValue=0;

            // for each row in dsFromTo
            //      if isValue - check type etc and set row value
            //      else set row value

            foreach (CreateRecordFromClassDS.GACreateRecordFromClassRow crow in dsFromTo.GACreateRecordFromClass)
            {
                if (newRecord.Tables[0].Columns.Contains(crow.CopyToFieldId))
                {
                    //if marked for get from parent, get data from passed record set
                    if (!crow.IsisPasteValueNull() && crow.isPasteValue == true)
                    {
                        // get value to insert
                        // get value / key in copyFromFieldId
                        GASystem.AppUtils.FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(crow.CopyToFieldId, copyToClass);
                        string myControlType= !(fd.ControlType == null) ? fd.ControlType.ToUpper() : string.Empty ;
                        if (myControlType.Contains("DROPDOWNLIST") )
//                        if (fd.ControlType.Con =="DROPDOWNLIST" || fd.ControlType.ToUpper=="POSTBACKDROPDOWNLIST") 
                        {
                            newRecord.Tables[0].Rows[0][crow.CopyToFieldId]=GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey(fd.ListCategory.ToString(),crow.CopyFromFieldId.ToString());
                        }
                        else
                            if (myControlType.Contains("LOOKUPFIELD") )
                            {
                                ArrayList list = GASystem.BusinessLayer.BusinessClass.GetRowIdFromClassWithFilter(fd.LookupTable.ToString(),""
                                    ,fd.LookupTableDisplayValue.ToString()+"="+crow.CopyFromFieldId.ToString(),transaction);
                                if (list!=null) newRecord.Tables[0].Rows[0][crow.CopyToFieldId]=list[0];
                            }
                            else
                            {
                                //  recordSet.Tables[Owner.DataClass.ToString()].Columns[attributeRow.AtrributeValue].
                                //get value from parent table. adjusted by maxlength if needed.
                                if (fd.ColumnType=="datetime")
                                {
//                                    DateTime myDate=crow.CopyFromFieldId;
                                // ToDo validate
                                    newRecord.Tables[0].Rows[0][crow.CopyToFieldId]=crow.CopyFromFieldId;
                                }
                                else
                                {
                                    int maxLength = fd.DataLength;
                                    if (maxLength < 2)
    //                                    newRecord.Tables[0].Rows[0][attributeRow.AttributeName] = recordSet.Tables[Owner.DataClass.ToString()].Rows[0][attributeRow.AtrributeValue];
                                        newRecord.Tables[0].Rows[0][crow.CopyToFieldId]=crow.CopyFromFieldId;
                                    else
                                    {
                                        string newValue=crow.CopyFromFieldId.ToString();
                                        if (maxLength > 0 && newValue.Length > maxLength) newValue = newValue.Substring(0, maxLength);
                                        newRecord.Tables[0].Rows[0][crow.CopyToFieldId] = newValue;
                                    }
                                }
                            }

                    }

                    else //copy data from source record
                    {
                        if (!crow.IsCopyToFieldIdNull()) newRecord.Tables[0].Rows[0][crow.CopyToFieldId.ToString()]=dsFrom.Tables[0].Rows[0][crow.CopyFromFieldId]; 
                    }
                }
            }
            newRecord = bc.SaveNew(newRecord, ownersOwner, transaction);
            transaction.Commit();
            return;
        }

        public static CreateRecordFromClassDS GetCreateRecordFromClass(int fromClass, int toClass) 
            // returns all records where from class is fromClass and to class is toClass
        {
            return GetCreateRecordFromClass(fromClass,toClass,null);
        }
        
        public static CreateRecordFromClassDS GetCreateRecordFromClass(int fromClass, int toClass, GADataTransaction transaction) // returns all documents where workflow is not running
        {
            CreateRecordFromClassDS ds = GASystem.DataAccess.CreateRecordFromClassDb.GetCreateRecordFromClass(fromClass,toClass,transaction);
            return ds;
        }
    }
}
