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
	public class GetResponsibleAgent : AbstractAgent
	{
		public const string DATACLASS = "GAOwnerDataClass";
		public const string FIELDNAME = "GAFieldName";
		public const string FIELDVALUE = "GAFieldValue";
		private const int MAXNUMBEROFLEVELS = 20;
		
		private string fieldName;
		private GADataRecord owner;

        public GetResponsibleAgent()
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

            System.Console.WriteLine("Get responsible Agent. Class: "+DataClass+" Field: "+fieldName);
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
            //get field description for dependant column
            AppUtils.FieldDescription fddep = AppUtils.FieldDefintion.GetFieldDescription(fd.DependsOnField, owner.DataClass.ToString());
            if (fddep == null) 
			{
				//table - field combination does not exist in fielddefinition
				//TODO throw error ??
				throw new GASystem.GAExceptions.GAException("Owner dataclass and dependsonfield is not defined in fielddescription");
			
			}

			//get data
            BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(owner.DataClass);


            System.Data.DataSet ds = bc.GetByRowId(owner.RowId);
                //GASystem.BusinessLayer.Utils.RecordsetFactory.g.GetRecordSetAllDetailsByDataRecord(owner);

			if (ds.Tables[0].Rows.Count == 0)
					throw new GASystem.GAExceptions.GAException("Owner record not found");

			//set empty string if database value is null
                Int32 responsibleUserID = -1;
            Int32 responsibleGroup = -1;

			if (ds.Tables[0].Rows[0][fieldName] != DBNull.Value)
                if (!Int32.TryParse(ds.Tables[0].Rows[0][fieldName].ToString(), out responsibleUserID))
                    responsibleUserID = -1;

            if (ds.Tables[0].Rows[0][fddep.FieldId] != DBNull.Value)
                if (!Int32.TryParse(ds.Tables[0].Rows[0][fddep.FieldId].ToString(), out responsibleGroup))
                    responsibleGroup = -1;


            if (-1 == responsibleUserID && -1 == responsibleGroup)
                throw new GASystem.GAExceptions.GAException("Responsible not set for specified owner");
			



			utils.AttributeHelper.SetAttribute(wi, FIELDVALUE, getResponsibleText(responsibleUserID, responsibleGroup));
			
			return wi;
		}


        private string getResponsibleText(Int32 responsibleUserID, Int32 responsibleGroup)
        {
            //user has higher priority than role. check for user settings first
            if (responsibleUserID != -1)
            {
                //get user
                return Workitem.USER_SUFFIX + User.GetLogonIdByPersonnelRowId(responsibleUserID);  // string.Empty;

            }

            if (responsibleGroup != -1)
            {
                //get user
                return Workitem.ROLE_SUFFIX + Lists.GetListValueByRowId(responsibleGroup,null);

            }

            throw new GASystem.GAExceptions.GAException("Responsible not set for specified owner");
			
        }

		
		

	}
}
