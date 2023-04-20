using System;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer
{
    class Flag : BusinessClass
    {
        		public Flag()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAFlag;
		}

		public static FlagDS GetFlagByFlagRowId(int RowId) 
		{
            return FlagDb.GetFlagByFlagRowId(RowId);
		}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetFlagByFlagRowId(RowId);
		}

    }
}
