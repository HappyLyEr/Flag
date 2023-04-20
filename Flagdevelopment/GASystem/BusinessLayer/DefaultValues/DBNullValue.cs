using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer.DefaultValues
{
    class DBNullValue : IDefaultValue
    {
        #region IDefaultValue Members

        public object GetValue()
        {
            return DBNull.Value;
        }

        #endregion
    }
}
