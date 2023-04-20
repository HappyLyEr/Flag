using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer.DefaultValues
{
    class StaticValue : IDefaultValue
    {
        GASystem.AppUtils.FieldDescription _fd;
        
        public StaticValue(GASystem.AppUtils.FieldDescription fd)
        {
            _fd = fd;
        }

        #region IDefaultValue Members

        public object GetValue()
        {

            string avalue = getValueString(_fd.CopyFromFieldId);
            if (_fd.ControlType.ToUpper() == "DROPDOWNLIST" || _fd.ControlType.ToUpper() == "POSTBACKDROPDOWNLIST")
            {
                //control is a galist, get rowid from galist;
                int anumber = Lists.GetListsRowIdByCategoryAndKey(_fd.ListCategory, avalue);
                if (anumber != -1) //Lists.GetListsRowIdByCategoryAndKey returns -1 if no match found
                    return anumber;
                else
                    return DBNull.Value;

            }
            else
            {
                return avalue;
            }
        }

        /// <summary>
        /// return avalue part of a CopyFromFieldId value pair in the format <%value=avalue%>
        /// </summary>
        /// <param name="valuepair"></param>
        /// <returns></returns>
        private string getValueString(string valuepair)
        {
            //remove <%value= and %>
            string avalue = valuepair.TrimEnd().Replace("<%value=", "").Replace("%>", "");
            return avalue;
        }

        #endregion
    }
}
