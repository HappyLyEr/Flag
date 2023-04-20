using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Cost.
	/// </summary>
	public class GenericBusinnesClass : BusinessClass
	{
//		public GenericBusinnesClass()
//		{
//			//
//			// TODO: Add constructor logic here
//			//
//			DataClass = GADataClass.GACost;
//		}

		public GenericBusinnesClass(GADataClass dataClass)
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = dataClass;
		}

		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return base.GetByRowId(RowId, null);
		}
			
	}
}
