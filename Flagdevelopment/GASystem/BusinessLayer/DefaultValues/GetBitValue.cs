using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer.DefaultValues
{
    /// <summary>
    /// return a bit value object from bit parameter
    /// </summary>
    class GetBitValue : IDefaultValue
    {
        Boolean _columnValue=false;

        public GetBitValue(Boolean ColumnValue)
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
