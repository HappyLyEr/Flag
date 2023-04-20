using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer.DefaultValues
{
    /// <summary>
    /// return a string value object from string parameter

    /// get a value from the owner class. specified in fielddefinition by <%parentvalue=fieldname%>
    /// </summary>
    class GetStringValue : IDefaultValue
    {
        string _columnValue=string.Empty;

        public GetStringValue(string ColumnValue)
        {
            _columnValue = ColumnValue;
        }

        #region IDefaultValue Members

       public object GetValue()
        {
           return _columnValue.ToString();
        }
        #endregion
    }
}
