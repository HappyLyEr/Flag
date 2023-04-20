using System;

namespace GASystem.BusinessLayer.Utils
{
	using GASystem;
	using GASystem.DataModel;
	using System.Data;
	using GASystem.DataAccess;

	/// <summary>
	/// Summary description for ObjectStore.
	/// </summary>
	public class StoreObject
	{
		public StoreObject()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void AddMemberClasses(GADataRecord Owner) 
		{
			AddMemberClasses(Owner, null);
		}

		/// <summary>
		/// Add default members for a gadatarecord. Called by Businessclass.SaveNew
		/// </summary>
		/// <param name="Owner"></param>
		/// <param name="transaction"></param>
		public static void AddMemberClasses(GADataRecord Owner, GADataTransaction transaction) 
		{
			//get list of object to add
			
			StoreObjectDS dsMembers = StoreObjectDb.GetStoreObjectsByOwnerClass(Owner.DataClass.ToString(), transaction);
			if (dsMembers.GAStoreObject.Rows.Count == 0)
				return; //no members to add
			
			//add members
			foreach (StoreObjectDS.GAStoreObjectRow memberRow in dsMembers.GAStoreObject.Rows) 
			{
				//get attributes
				StoreAttributeDS dsAttributes = StoreAttributeDb.GetStoreAttributesByOwner(new GADataRecord(memberRow.StoreObjectRowId, GADataClass.GAStoreObject)); 
				//create new row
				BusinessClass bc =  RecordsetFactory.Make(GADataRecord.ParseGADataClass(memberRow.MemberClass));

                DataSet newRecord = bc.GetNewRecord(Owner, transaction);
				//bc.ApplyDefaultValues(newRecord, Owner);
				
				//DataSet newRecord = RecordsetFactory.GetNewRecord(GADataRecord.ParseGADataClass(memberRow.MemberClass));
				//add attributes
				foreach (StoreAttributeDS.GAStoreAttributeRow attributeRow in dsAttributes.GAStoreAttribute.Rows ) 
				{
					if (newRecord.Tables[0].Columns.Contains(attributeRow.AttributeName))
						newRecord.Tables[0].Rows[0][attributeRow.AttributeName] = attributeRow.AtrributeValue.ToString();
				}
				newRecord = bc.SaveNew(newRecord, Owner, transaction);
					
					//RecordsetFactory.UpdateDataSet(GADataRecord.ParseGADataClass(memberRow.MemberClass), newRecord);
				//add superclass relation
				//DataClassRelations.CreateDataClassRelation(Owner.RowId,Owner.DataClass,Convert.ToInt32(newRecord.Tables[0].Rows[0][memberRow.MemberClass.Substring(2) + "RowId"]), GADataRecord.ParseGADataClass(memberRow.MemberClass));
			}
		}
		
		
		/// <summary>
		/// Add default members for a gadatarecord. Called by Businessclass.SaveNew
		/// </summary>
		/// <param name="Owner"></param>
		/// <param name="transaction"></param>
		public static void AddMemberClasses(GADataRecord Owner, DataSet recordSet, GADataTransaction transaction) 
		{
			//get list of object to add
			
			StoreObjectDS dsMembers = StoreObjectDb.GetStoreObjectsByOwnerClass(Owner.DataClass.ToString(), transaction);
			if (dsMembers.GAStoreObject.Rows.Count == 0)
				return; //no members to add
            if (Owner.DataClass == GADataClass.GALocation)
            {
                object typeRowIdObj = recordSet.Tables[0].Rows[0]["TypeOfLocationListsRowId"];
                ListsDS listDS = Lists.GetListsByListsRowId((int)typeRowIdObj);
                string desc = listDS.GALists[0].GAListDescription;
                if (desc != "Node Vessel" && desc != "Streamer Vessel" && desc != "Source Vessel" && desc != "Accommodation Vessel")
                {
                    return;
                }
            }
			
			//add members
			foreach (StoreObjectDS.GAStoreObjectRow memberRow in dsMembers.GAStoreObject.Rows) 
			{
				//get attributes
				StoreAttributeDS dsAttributes = StoreAttributeDb.GetStoreAttributesByOwner(new GADataRecord(memberRow.StoreObjectRowId, GADataClass.GAStoreObject)); 
				//create new row
				BusinessClass bc =  RecordsetFactory.Make(GADataRecord.ParseGADataClass(memberRow.MemberClass));

				DataSet newRecord = bc.GetNewRecord(Owner, transaction);
				//bc.ApplyDefaultValues(newRecord, Owner);
				
				//DataSet newRecord = RecordsetFactory.GetNewRecord(GADataRecord.ParseGADataClass(memberRow.MemberClass));
				//add attributes
				foreach (StoreAttributeDS.GAStoreAttributeRow attributeRow in dsAttributes.GAStoreAttribute.Rows ) 
				{
					if (newRecord.Tables[0].Columns.Contains(attributeRow.AttributeName))
					{
						//if marked for get from parent, get data from passed record set
						if (!attributeRow.IsSwitchFree1Null() && attributeRow.SwitchFree1 == true)
						{
							if (attributeRow.IsAtrributeValueNull())  //get from user typed data
								//if (recordSet.Tables[memberRow.MemberClass].Rows[0][attributeRow.AttributeName] != DBNull.Value)
									newRecord.Tables[0].Rows[0][attributeRow.AttributeName] = recordSet.Tables[memberRow.MemberClass].Rows[0][attributeRow.AttributeName];
							else{ //get form ownerfield
								    if (recordSet.Tables[Owner.DataClass.ToString()].Rows[0][attributeRow.AtrributeValue] != DBNull.Value) 
								    {
                                      //  recordSet.Tables[Owner.DataClass.ToString()].Columns[attributeRow.AtrributeValue].
                                        
                                        //get value from parent table. adjusted by maxlength if needed.
                                        GASystem.AppUtils.FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(attributeRow.AttributeName, memberRow.MemberClass);
                                        int maxLength = fd.DataLength;
                                        if (maxLength < 2)
                                            newRecord.Tables[0].Rows[0][attributeRow.AttributeName] = recordSet.Tables[Owner.DataClass.ToString()].Rows[0][attributeRow.AtrributeValue];
                                        else
                                        {
                                            string newValue = recordSet.Tables[Owner.DataClass.ToString()].Rows[0][attributeRow.AtrributeValue].ToString();
                                            if (maxLength > 0 && newValue.Length > maxLength)
                                                newValue = newValue.Substring(0, maxLength);
                                            newRecord.Tables[0].Rows[0][attributeRow.AttributeName] = newValue;
                                        }
								    }
								}
							
						} 
						else
						{
							if (!attributeRow.IsAtrributeValueNull()) 
							{
								//get data from parameters set in gaattribute
								string attValue = attributeRow.AtrributeValue.ToString();
							
								//Special handling of gaaction startpending message. Add logonid to stratup string
								//TODO make more generic
								if (attValue.Trim() == "StartPending:" )
									attValue += GASystem.AppUtils.GAUsers.GetUserId();
							
								newRecord.Tables[0].Rows[0][attributeRow.AttributeName] = attValue;
							}
						}
					}
				}
				
				//add data from dataset
				newRecord = bc.SaveNew(newRecord, Owner, transaction);
					
			}
		}
	}
}
