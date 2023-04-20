using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer.DefaultValues
{
    /// <summary>
    /// get a value from the owner class. specified in fielddefinition by <%parentvalue=fieldname%>
    /// </summary>
    class ParentValue : IDefaultValue
    {
        GASystem.DataModel.GADataRecord _owner;
        GASystem.AppUtils.FieldDescription _fd;
        GADataTransaction _transaction;

        public ParentValue( GASystem.AppUtils.FieldDescription fd, GASystem.DataModel.GADataRecord owner, GADataTransaction transaction)
        {
            _owner = owner;
            _fd = fd;
            _transaction = transaction;
        }

        #region IDefaultValue Members

        /// <summary>
        /// Get value form parent
        /// </summary>
        /// <returns>
        /// Value from specified field in owner
        /// DBNull if owner is null or field does not exists
        /// </returns>
       public object GetValue()
        {
            if (_owner == null)
                return DBNull.Value;


            string avalue = getValueString(_fd.CopyFromFieldId);
            BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(_owner.DataClass);
            System.Data.DataSet ds = bc.GetByRowId(_owner.RowId, _transaction);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(avalue))
                return ds.Tables[0].Rows[0][avalue];
         
            //default
            return DBNull.Value;
        }

        #endregion

        /// <summary>
        /// return avalue part of a CopyFromFieldId value pair in the format <%value=avalue%>
        /// </summary>
        /// <param name="valuepair"></param>
        /// <returns></returns>
        private string getValueString(string valuepair)
        {
            //remove <%value= and %>
            string avalue = valuepair.TrimEnd().Replace("<%parentvalue=", "").Replace("%>", "");
            return avalue;
        }
    }
}
