using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer.DefaultValues
{
    /// <summary>
    /// return a DateTime value object from DateTime parameter
    /// </summary>
    class GetDateTimeValue : IDefaultValue
    {
        DateTime _columnValue;

        public GetDateTimeValue(DateTime ColumnValue)
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
