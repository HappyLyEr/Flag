using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GASystem.DataModel;
using System.Data.SqlClient;
using GASystem.AppUtils;


namespace GASystem.BusinessLayer.DefaultValues
{
    class SQLExpression : IDefaultValue
    {
        GASystem.AppUtils.FieldDescription _fd;
        GASystem.DataModel.GADataRecord _owner;
        GADataClass _dc;
        System.Data.DataSet _recordSet;

        public SQLExpression(GASystem.AppUtils.FieldDescription fd, GASystem.DataModel.GADataRecord owner, GADataClass dc, System.Data.DataSet RecordSet)
        {
            _fd = fd;
            _owner = owner;
            _dc = dc;
            _recordSet = RecordSet;
        }
           
        #region IDefaultValue Members

        public object  GetValue()
        {
            string aexpression = getExpressionString(_fd.CopyFromFieldId.ToString(), _recordSet);
            string returnType = string.Empty;
            if (aexpression.Contains("/*int*/"))
            {
                returnType = "int";
                aexpression = aexpression.Replace("/*int*/", "");
            }
            else
            {
                if (aexpression.Contains("/*string*/"))
                {
                    returnType = "string";
                    aexpression = aexpression.Replace("/*string*/", "");
                }
                else
                {
                    if (aexpression.Contains("/*datetime*/"))
                    {
                        returnType = "datetime";
                        aexpression = aexpression.Replace("/*datetime*/", "");
                    }
                    else
                    {
                        if (aexpression.Contains("/*bit*/"))
                        {
                            returnType = "bit";
                            aexpression = aexpression.Replace("/*bit*/", "");
                        }
                        // Tor 20180308 add type float
                        else if (aexpression.Contains("/*float*/"))
                        {
                            returnType = "float";
                            aexpression = aexpression.Replace("/*float*/", "");
                        }
                    }
                }
            }

            if (returnType==string.Empty || 
                (returnType=="string" && !(_fd.ColumnType.ToString() == "nvarchar" || _fd.ColumnType.ToString() == "ntext")))
                return DBNull.Value;
            
            SqlConnection myConnection = new SqlConnection(GASystem.DataAccess.DataUtils.getConnectionString());
            string stringValue=string.Empty;
            int? intValue=0; 
            bool? boolValue=false; 
            DateTime? datetimeValue=DateTime.MinValue;
            // Tor 20180308 add type float
            float? floatValue = 0;
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(aexpression, myConnection);

                stringValue = returnType == "string" ? myCommand.ExecuteScalar().ToString() : string.Empty;
                intValue = returnType == "int" ? (int)myCommand.ExecuteScalar() : (int?)null;
                boolValue = returnType == "bit" ? (bool)myCommand.ExecuteScalar() : (bool?)null;
                datetimeValue = returnType == "datetime" ? (DateTime)myCommand.ExecuteScalar() : (DateTime?)null;
                // Tor 20180308 add type float
                floatValue = returnType == "float" ? (float)myCommand.ExecuteScalar() : (float?)null;
            }
            catch (Exception ex)
            {
                string exmsg = ex.Message;
            }
            finally
            {
                myConnection.Close();
            }

            if (returnType == "int") return intValue;
            if (returnType == "bit") return boolValue;
            if (returnType == "string") return stringValue;
            if (returnType == "datetime") return datetimeValue;

            return null;
        }

        #endregion
        
        /// <summary>
        /// return avalue part of a CopyFromFieldId value pair in the format <%value=avalue%>
        /// </summary>
        /// <param name="valuepair"></param>
        /// <returns></returns>
        private string getExpressionString(string valuepair, System.Data.DataSet RecordSet)
        {
            //remove <%value= and %>
            string avalueOriginal = valuepair;
            string avalue = valuepair.TrimEnd().Replace("<%sql=", "");
            String[] fieldElements = avalue.Split(new string[] { "<%sql%row.", "<%parentvalue=" } , StringSplitOptions.None);
            List<string> foundElements= new List<string>();
            if (fieldElements.Length > 1)
            {
                for (int t = 1; t<fieldElements.Length;t++ )
                {
                    string fieldElement = fieldElements[t];
                    fieldElement=fieldElement.Substring(0,fieldElement.IndexOf("%>"));
                    foundElements.Add(fieldElement);
                }

                foreach (string fieldName in foundElements)
                {
                    FieldDescription field = GASystem.AppUtils.FieldDefintion.GetFieldDescription(fieldName, _dc.ToString());
                    string addHyphen = (field.ColumnType == "ntext" || field.ColumnType == "nvarchar" || field.ColumnType == "char") ? "'" : "";
                    //avalue=avalue.Replace("<%sql%row." + fieldName + "%>", addHyphen + _recordSet.Tables[0].Rows[0][fieldName].ToString()) + addHyphen;
                    // Tor 20160825 fails when ColumnType=bit statement above replaced by below
                    if (avalue.IndexOf("<%sql%row.") >= 0)
                    {
                        string resultValue = _recordSet.Tables[0].Rows[0][fieldName].ToString();
                        if (field.ColumnType == "bit")
                        {
                            resultValue = resultValue.Replace("True", "1").Replace("False", "0");
                        }
                        avalue = avalue.Replace("<%sql%row." + fieldName + "%>", addHyphen + resultValue + addHyphen);
                    }
                    else if (avalue.IndexOf("<%parentvalue=") >= 0)
                    {
                        string resultValue = GetParentValue(fieldName).ToString();
                        if (field.ColumnType == "bit")
                        {
                            resultValue = resultValue.Replace("True", "1").Replace("False", "0");
                        }
                        avalue = avalue.Replace("<%parentvalue=" + fieldName + "%>", addHyphen + resultValue + addHyphen);
                    }
                }
                avalue = avalue.Replace("%>", "");
            }
            return avalue;
        }

        private object GetParentValue(string fieldName)
        {
            if (_owner == null)
                return DBNull.Value;

            BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(_owner.DataClass);
            System.Data.DataSet ds = bc.GetByRowId(_owner.RowId, null);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(fieldName))
                return ds.Tables[0].Rows[0][fieldName];

            //default
            return DBNull.Value;
        }
    }
}
