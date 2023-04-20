using System;
using GASystem;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using System.Data;

namespace GASystem.DotNetApre.impl
{
	/// <summary>
	/// Agent for setting a new value to any field in the owner of the current action
	/// Expected workitem attributes:
	///		GAOwnerDataClass - Expected owner of action, ignored if missing or empty
	///		GAFieldName - field name to update
	///		GAFieldValue - new value
	/// </summary>
	public class CreateNewRecordFromRecordAgent : AbstractAgent
	{
		public const string DATACLASS = "GAOwnerDataClass";
        public const string CREATEDATACLASSRECORD = "GACreateDataClassRecord";
        /*		public const string FIELDNAME = "GAFieldName";
                public const string FIELDVALUE = "GAFieldValue";
        */		
		private string fieldName;
        private string createRecordClass;

		private string fieldValue;
        private string fieldTable;
		private GADataRecord recordToCopyFrom;
        private GADataRecord ownersOwner;
        
        public CreateNewRecordFromRecordAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
			//get recordToCopyFrom for current action
			recordToCopyFrom = DataClassRelations.GetOwner(new GADataRecord(GetActionId(wi), GADataClass.GAAction));
            //get owner of current recordToCopyFrom - is to own the record to create
            ownersOwner = DataClassRelations.GetOwner(recordToCopyFrom);

            System.Console.WriteLine("Create New Record From Record Agent. Copy from: " + recordToCopyFrom.DataClass.ToString() + " " + recordToCopyFrom.RowId.ToString() + " to Class: " + utils.AttributeHelper.GetAttribute(wi, CREATEDATACLASSRECORD).ToString());
            //get wi attribute values

			//expected dataclass, used to verify that the owner is of correct type
			string DataClass = utils.AttributeHelper.GetAttribute(wi, DATACLASS);
            string CreateDataClass = utils.AttributeHelper.GetAttribute(wi,CREATEDATACLASSRECORD);
            createRecordClass = CreateDataClass;
            int FromDataClassRowId=GASystem.BusinessLayer.Class.GetClassRowIdByClassName(DataClass);
            int ToDataClassRowId=GASystem.BusinessLayer.Class.GetClassRowIdByClassName(CreateDataClass);
            if(FromDataClassRowId<1 || ToDataClassRowId<1) 
            {
                //just return
                //TODO throw error ?
                System.Console.WriteLine("From dataclassrowid or To dataclassrowid does not exist. From: " + DataClass + " To: " + CreateDataClass);
                return wi;
            }

            // check same ownerdataclass exist for DataClass and CreateDataClass
            SuperClassLinksDS dso = GASystem.DataAccess.SuperClassDb.GetSuperClassLinksByOwnerAndMember
                (GADataRecord.ParseGADataClass(ownersOwner.DataClass.ToString()), GADataRecord.ParseGADataClass(DataClass));
            SuperClassLinksDS dsc = GASystem.DataAccess.SuperClassDb.GetSuperClassLinksByOwnerAndMember
                (GADataRecord.ParseGADataClass(ownersOwner.DataClass.ToString()), GADataRecord.ParseGADataClass(CreateDataClass));

            if ((DataClass != string.Empty && !DataClass.Equals(recordToCopyFrom.DataClass.ToString())) 
                || ownersOwner == null || dso == null || dsc == null
                )
            {
                //just return
                //TODO throw error ?
                System.Console.WriteLine("Action owner dataclass does not match owner defined or Action OwnerDataClass record doew not exist or source and destination records does not have the same owner type");
                return wi;
            }

            // get source record
            DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(recordToCopyFrom);
            
            // create target record
            //create new row
            BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataRecord.ParseGADataClass(createRecordClass));
            
			//get field description
            AppUtils.FieldDescription[] fdFrom = AppUtils.FieldDefintion.GetFieldDescriptions(DataClass);
            AppUtils.FieldDescription[] fdTo = AppUtils.FieldDefintion.GetFieldDescriptions(CreateDataClass);

            if (fdFrom == null || fdTo == null) 
			{
				//table - field definition for From or To does not exist
				//just return
				//TODO throw error ??
				System.Console.WriteLine("Copy From "+DataClass+" or To "+CreateDataClass+" fieldname definitions does not exist");
				return wi;
			}

            // Check if all from and to fields are valid
            CreateRecordFromClassDS dsFromTo = GASystem.BusinessLayer.CreateRecordFromClass.GetCreateRecordFromClass(FromDataClassRowId, ToDataClassRowId);
            foreach (CreateRecordFromClassDS.GACreateRecordFromClassRow crow in dsFromTo.GACreateRecordFromClass)
            {
                if (!AppUtils.FieldDefintion.IfFieldExists(CreateDataClass, crow.CopyToFieldId.ToString())
                    || (!crow.isPasteValue && !AppUtils.FieldDefintion.IfFieldExists(DataClass, crow.CopyFromFieldId.ToString()))
                    )
                {
                    //table - field definition for From or To does not exist
                    //just return
                    //TODO throw error ??
                    System.Console.WriteLine("Copy From field name " + crow.CopyFromFieldId.ToString() + " or Copy to fieldname " + crow.CopyToFieldId.ToString() + " does not exist in field definitions");
                    return wi;
                }
            }

            // Input control completed

            GASystem.BusinessLayer.CreateRecordFromClass.CreateRecordFromClassRecord(dsFromTo, ownersOwner, recordToCopyFrom,
                ds, FromDataClassRowId, ToDataClassRowId, CreateDataClass);

            ////if control type for field is dropdownlist, get correct listrowid
            //if (fd.ControlType.ToUpper()  == "DROPDOWNLIST") 
            //    fieldValue = BusinessLayer.Lists.GetListsRowIdByCategoryAndKey(fd.ListCategory, fieldValue).ToString();

            //UpdateField();
            //System.Console.WriteLine("Finished");


			return wi;
		}

		private void UpdateField() 
		{
            System.Console.WriteLine("Updating field: " + fieldTable + "." + fieldName + " to value: " + fieldValue + " ");
			BusinessLayer.BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(recordToCopyFrom.DataClass);
			DataSet ds = bc.GetByRowId(recordToCopyFrom.RowId);
			if (!ds.Tables[recordToCopyFrom.DataClass.ToString()].Columns.Contains(fieldName))
				return; //todo throw exception??
			try 
			{
				ds.Tables[recordToCopyFrom.DataClass.ToString()].Rows[0][fieldName] = fieldValue;
				bc.CommitDataSet(ds);
			} 
			catch (Exception ex) 
			{
				System.Console.WriteLine(ex.Message);
			}
			return;
		}

		

		
		

	}
}
