using System;
using GASystem;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using System.Data;

namespace GASystem.DotNetApre.impl
{
	/// <summary>
	/// Agent for setting a new date value to any date field in the owner of the current action
	/// Expected workitem attributes:
    /// result field 
	///		GAFieldName - base field 
    ///     GAUnitType -  unit types (see array)
	///		GAFieldValue - units (+ or -)
    ///     GAUpdateFieldName
    /// 
	/// </summary>
	public class UpdateDateAgent : AbstractAgent
	{
        public const string FIELDNAME = "GAFieldName";
        public const string UNITTYPE = "GAUnitType";
        public const string FIELDVALUE = "GAFieldValue";
        public const string UPDATEFIELDNAME= "GAUpdateFieldName";
		
		private string fieldName;
        private string unitType;
        private string fieldValue;
        private string updatefieldName;
		private GADataRecord owner;
        private int addUnits = 0;
        private string[] _unitTypes = { "W", "2W", "3W", "M", "3M", "4M", "6M", "Y", "2Y", "D", "H" };
        private int[] _units ={ 1, 2, 3, 4, 6, 1, 2, 0, 0 };
		public UpdateDateAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
			System.Console.WriteLine("Starting Update Date Agent");
			//get owner for current action
			owner = DataClassRelations.GetOwner(new GADataRecord(GetActionId(wi), GADataClass.GAAction));
			//get wi attribute values
            string DataClass = utils.AttributeHelper.GetAttribute(wi, owner.DataClass.ToString());
			fieldName = utils.AttributeHelper.GetAttribute(wi, FIELDNAME);
            unitType = utils.AttributeHelper.GetAttribute(wi, UNITTYPE);
            fieldValue = utils.AttributeHelper.GetAttribute(wi, FIELDVALUE);
            updatefieldName = utils.AttributeHelper.GetAttribute(wi, UPDATEFIELDNAME);

            // check if base and result fields exist in the database, and if they are date fields
            //get field description
            if (fieldName != "")
            {
                AppUtils.FieldDescription fdSource = AppUtils.FieldDefintion.GetFieldDescription(fieldName, owner.DataClass.ToString());
                if (fdSource == null)
                {
                    System.Console.WriteLine("Owner dataclass and source fieldname is not defined in fielddescription");
                    return wi;
                }
                if (fdSource.ControlType.ToUpper() != "DATETIME" && fdSource.ControlType != "DATE")
                {
                    System.Console.WriteLine("Owner dataclass source fieldname is not a datetime field");
                    return wi;
                }
            }

            AppUtils.FieldDescription fdTarget = AppUtils.FieldDefintion.GetFieldDescription(updatefieldName, owner.DataClass.ToString());
            if (fdTarget == null )
            {
                System.Console.WriteLine("Owner dataclass target fieldname is not defined in fielddescription");
                return wi;
            }
            if (fdTarget.ControlType.ToUpper() != "DATETIME" && fdTarget.ControlType.ToUpper() != "DATE")
            {
                System.Console.WriteLine("Owner dataclass target fieldname is not a datetime field");
                return wi;
            }

            // check unit type should be the same as in GAListCategory=DCRF (added D and H to enable adding days and hours)
            if (Array.IndexOf(_unitTypes,unitType.ToString())<0)
            {
                System.Console.WriteLine("Illegal Unit Type: "+unitType+" in add date ");
                return wi;
            }

            if (fieldValue == "")
            {
                addUnits = _units[Array.IndexOf(_unitTypes, unitType.ToString())];
            }
            else
            {
                bool result = int.TryParse(fieldValue, out addUnits);
                if (!result)
                {
                    System.Console.WriteLine("Illegal number og units: " + fieldValue + " in add date ");
                    return wi;
                }
            }

            UpdateField();
			System.Console.WriteLine("Finished");

            return wi;
		}

		private void UpdateField() 
		{
			BusinessLayer.BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(owner.DataClass);
			DataSet ds = bc.GetByRowId(owner.RowId);
            if (!ds.Tables[owner.DataClass.ToString()].Columns.Contains(updatefieldName))
                return; //todo throw exception??
            if (fieldName!="" && !ds.Tables[owner.DataClass.ToString()].Columns.Contains(fieldName))
                return; //todo throw exception??

            DateTime baseDate = DateTime.UtcNow;
            try 
			{
                if (fieldName != "")
                {
                    baseDate = (ds.Tables[0].Rows[0][fieldName] != DBNull.Value)
                        ? (DateTime)(ds.Tables[0].Rows[0][fieldName])
                        : DateTime.UtcNow;
                }
                DateTime resultDate;
                if (unitType == "W" || unitType == "2W" || unitType == "3W") resultDate = baseDate.AddDays(addUnits).Date;
                else
                    if (unitType == "M" || unitType == "3M" || unitType == "4M" || unitType == "6M") resultDate = baseDate.AddMonths(addUnits).Date;
                    else
                        if (unitType == "Y" || unitType == "2Y") resultDate = baseDate.AddYears(addUnits).Date;
                        else
                            if (unitType == "D") resultDate = baseDate.AddDays(addUnits).Date;
                            else
                                if (unitType == "H") resultDate = baseDate.AddHours(addUnits);
                                else
                                    return; //invalid unit type - todo throw exception??
                
                System.Console.WriteLine("Updating field: " + owner.DataClass.ToString() + "." + updatefieldName + " to value: " + resultDate.ToString() + " ");

                ds.Tables[owner.DataClass.ToString()].Rows[0][updatefieldName] = resultDate;
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
