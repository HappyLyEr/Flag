using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer.DefaultValues
{
    /// <summary>
    /// return an int value object from int parameter
    /// </summary>
    class GetIntValue : IDefaultValue
    {
        int _columnValue;

        public GetIntValue(int ColumnValue)
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
