using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer.DefaultValues
{
    /// <summary>
    /// return a smallInt value object from smaillInt parameter
    /// </summary>
    class GetSmallIntValue : IDefaultValue
    {
        Int16 _columnValue;

        public GetSmallIntValue(Int16 ColumnValue)
        {
            _columnValue = ColumnValue;
        }

        #region IDefaultValue Members

       public object GetValue()
        {
           return _columnValue;
        }
        #endregion
    }
}
