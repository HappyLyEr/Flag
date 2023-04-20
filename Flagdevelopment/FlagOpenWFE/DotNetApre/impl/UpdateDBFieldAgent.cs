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
	public class UpdateDBFieldAgent : AbstractAgent
	{
		public const string DATACLASS = "GAOwnerDataClass";
		public const string FIELDNAME = "GAFieldName";
		public const string FIELDVALUE = "GAFieldValue";
		
		private string fieldName;
		private string fieldValue;
        private string fieldTable;
		private GADataRecord owner;

		public UpdateDBFieldAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
//			System.Console.WriteLine("Starting Update Database Field Agent");
			//get owner for current action
			owner = DataClassRelations.GetOwner(new GADataRecord(GetActionId(wi), GADataClass.GAAction));
			//get wi attribute values

			//expected dataclass, used to verify that the owner is of correct type
			string DataClass = utils.AttributeHelper.GetAttribute(wi, DATACLASS);
            fieldTable = DataClass;
			//fieldname to change value of
			fieldName = utils.AttributeHelper.GetAttribute(wi, FIELDNAME);
			//new field value
			fieldValue = utils.AttributeHelper.GetAttribute(wi, FIELDVALUE);
            System.Console.WriteLine("Update Database Field Agent. Class:"+fieldTable+" Field: "+fieldName+" Value: "+fieldValue);

			if (DataClass != string.Empty && !DataClass.Equals(owner.DataClass.ToString()))
			{
				//just return
				//TODO throw error ?
				System.Console.WriteLine("Action owner dataclass (" + DataClass + ") does not match owner defined in workitem");
				return wi;
			}

			//get field description
			AppUtils.FieldDescription fd = AppUtils.FieldDefintion.GetFieldDescription(fieldName, owner.DataClass.ToString());
			if (fd == null) 
			{
				//table - field combination does not exist in fielddefinition
				//just return
				//TODO throw error ??
                System.Console.WriteLine("Owner dataclass (" + DataClass + ") fieldname (" + fieldName + ") is not defined in fielddescription");
				return wi;
			}

			//if control type for field is dropdownlist, get correct listrowid
			if (fd.ControlType.ToUpper()  == "DROPDOWNLIST") 
				fieldValue = BusinessLayer.Lists.GetListsRowIdByCategoryAndKey(fd.ListCategory, fieldValue).ToString();

			UpdateField();
			System.Console.WriteLine("Finished");


			return wi;
		}

		private void UpdateField() 
		{
            System.Console.WriteLine("Updating field: " + fieldTable + "." + fieldName + " to value: " + fieldValue + " ");
			BusinessLayer.BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(owner.DataClass);
			DataSet ds = bc.GetByRowId(owner.RowId);
			//DataSet ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(owner);
			if (!ds.Tables[owner.DataClass.ToString()].Columns.Contains(fieldName))
				return; //todo throw exception??
			try 
			{
				ds.Tables[owner.DataClass.ToString()].Rows[0][fieldName] = fieldValue;
				//BusinessLayer.Utils.RecordsetFactory.UpdateDataSet(owner.DataClass, ds);
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
