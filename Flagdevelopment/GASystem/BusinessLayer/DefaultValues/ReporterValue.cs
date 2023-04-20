using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer.DefaultValues
{
    class ReporterValue : IDefaultValue
    {
        #region IDefaultValue Members

        public object GetValue()
        {
            try
            {
                string logonId = AppUtils.GAUsers.GetUserId();
                return User.GetPersonnelIdByLogonId(logonId);
            }
            catch
            {
                return DBNull.Value;
            }

        }

        #endregion
    }
}
