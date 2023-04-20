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
	public class GetUserIdFromDBFieldAgent : AbstractAgent
	{
		public const string DATACLASS = "GAOwnerDataClass";
		public const string FIELDNAME = "GAFieldName";
		public const string FIELDVALUE = "GAFieldValue";
		private const int MAXNUMBEROFLEVELS = 20;
		
		private string fieldName;
		private GADataRecord owner;

		public GetUserIdFromDBFieldAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
			//get owner for current action
			owner = DataClassRelations.GetOwner(new GADataRecord(GetActionId(wi), GADataClass.GAAction));
			//get wi attribute values

			//expected dataclass, used to verify that the owner is of correct type
			string DataClass = utils.AttributeHelper.GetAttribute(wi, DATACLASS); 
			//fieldname to change value of
			fieldName = utils.AttributeHelper.GetAttribute(wi, FIELDNAME);

            System.Console.WriteLine("Starting GetUserIdFromDBFieldAgent Class: "+DataClass+" Field: "+fieldName);
			
			//new field value
			//fieldValue = utils.AttributeHelper.GetAttribute(wi, FIELDVALUE);

			if (DataClass != string.Empty)  // && !DataClass.Equals(owner.DataClass.ToString()))
			{
				//a dataclass i specified, walk the hierarchy and tyr to find a owner of specified type
				int curretLevel  = 0;

				while (!DataClass.ToUpper().Equals(owner.DataClass.ToString().ToUpper()))
				{
					owner = DataClassRelations.GetOwner(owner);
					curretLevel++;
					if (owner == null)
						throw new GASystem.GAExceptions.GAException("Owner of type " + DataClass.ToString() + " not found" );
					if (curretLevel >= MAXNUMBEROFLEVELS)
						throw new GASystem.GAExceptions.GAException("Maximun number of recursive searches for ownerclass exceeded" );
				}

			}

			//get field description
			AppUtils.FieldDescription fd = AppUtils.FieldDefintion.GetFieldDescription(fieldName, owner.DataClass.ToString());
			if (fd == null) 
			{
				//table - field combination does not exist in fielddefinition
				//TODO throw error ??
				throw new GASystem.GAExceptions.GAException("Owner dataclass and fieldname is not defined in fielddescription");
			
			}
			//get data
			System.Data.DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(owner);

			if (ds.Tables[0].Rows.Count == 0)
					throw new GASystem.GAExceptions.GAException("Owner record not found");

			
			//set to 0 if not found in database. Will throw and error when we are later trying to get the logonid
			int dbFieldValue = 0;
			if (ds.Tables[0].Rows[0][fieldName] != DBNull.Value)
				dbFieldValue = (int)ds.Tables[0].Rows[0][fieldName];


			string logonId = User.GetLogonIdByPersonnelRowId(dbFieldValue);
			utils.AttributeHelper.SetAttribute(wi, FIELDVALUE, logonId);
			
			return wi;
		}

//		public static void SetFieldValue(openwfe.workitem.InFlowWorkitem wi, string FieldValue) 
//		{
//			StringAttribute fieldNameKey = new StringAttribute(FIELDVALUE);
//			StringAttribute fieldValueKeyValue = new StringAttribute(FieldValue);
//
//			
//			wi.attributes[fieldNameKey] = fieldValueKeyValue;
//		}

		

		
		

	}
}
