using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer.DefaultValues
{
    class DateTimeNowUTCValue : IDefaultValue
    {
        #region IDefaultValue Members

        public object GetValue()
        {
            return System.DateTime.UtcNow;
        }

        #endregion

    }
}
