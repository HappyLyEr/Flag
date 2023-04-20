using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataModel;
using GASystem.AppUtils;


namespace GASystem.BusinessLayer.DefaultValues
{
    class LookupField: IDefaultValue
    {
        GASystem.AppUtils.FieldDescription _fd;
       GADataClass _dc;
        System.Data.DataSet _recordSet;

        public LookupField(GASystem.AppUtils.FieldDescription fd, GADataClass dc, System.Data.DataSet RecordSet)
        {
            _fd = fd;
            _dc = dc;
            _recordSet = RecordSet;
            
        }


        #region IDefaultValue Members

        public object GetValue()
        {
            String lookupfieldstring = getLookupFieldString(_fd.CopyFromFieldId);
            String localField = getLocalField(lookupfieldstring);
            String remoteField = getRemoteField(lookupfieldstring);

            FieldDescription fdlocal = FieldDefintion.GetFieldDescription(localField, _dc.ToString());
            int remoteRowId = Int32.Parse(_recordSet.Tables[0].Rows[0][localField].ToString());

            //return remote value:
            BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(GASystem.DataModel.GADataRecord.ParseGADataClass(fdlocal.LookupTable));
            System.Data.DataSet ds = bc.GetByRowId(remoteRowId);

            return ds.Tables[0].Rows[0][remoteField];
            


        }

        #endregion

        /// <summary>
        /// return avalue part of a CopyFromFieldId value pair in the format <%value=avalue%>
        /// </summary>
        /// <param name="valuepair"></param>
        /// <returns></returns>
        private string getLookupFieldString(string valuepair)
        {
            //remove <%value= and %>
            string avalue = valuepair.TrimEnd().Replace("<%lookupfield=", "").Replace("%>", "");
            return avalue;
        }

        private string getLocalField(String fieldPair)
        {
            return fieldPair.Split('.')[0].ToString();
        }

        private string getRemoteField(String fieldPair)
        {
            return fieldPair.Split('.')[1].ToString();
        }

    }
}
