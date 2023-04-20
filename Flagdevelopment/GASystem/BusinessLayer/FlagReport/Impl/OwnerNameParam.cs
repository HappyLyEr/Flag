using System;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using System.Data;

namespace GASystem.BusinessLayer.FlagReport.Impl
{
	/// <summary>
	/// OwnerNameParam. Returns the name of the owner record of the datarecord. If the report is based
	/// on a report instance. Then the name is the owner of the report instance.
	/// </summary>
	public class OwnerNameParam : IParameter
	{
		private string _ownerName = string.Empty;

		public OwnerNameParam(GADataRecord DataRecord)
		{
			GADataRecord owner = DataClassRelations.GetOwner(DataRecord);
			if (owner != null) 
			{
				DataSet dsOwner = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(owner);
				AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(owner.DataClass);

				if (dsOwner.Tables[0].Columns.Contains(cd.ObjectName)) 
				{
					_ownerName = dsOwner.Tables[0].Rows[0][cd.ObjectName].ToString();
				}
			}
		}

		#region IParameter Members

		public object GetValue()
		{
			// TODO:  Add OwnerNameParam.GetValue implementation
			return _ownerName;
		}

		#endregion
	}
}
