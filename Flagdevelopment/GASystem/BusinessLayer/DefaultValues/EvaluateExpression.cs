using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GASystem.DataModel;
using GASystem.AppUtils;
using NCalc;

namespace GASystem.BusinessLayer.DefaultValues
{
    class EvaluateExpression: IDefaultValue
    {
        GASystem.AppUtils.FieldDescription _fd;
       GADataClass _dc;
        System.Data.DataSet _recordSet;

        public EvaluateExpression(GASystem.AppUtils.FieldDescription fd, GADataClass dc, System.Data.DataSet RecordSet)
        {
            _fd = fd;
            _dc = dc;
            _recordSet = RecordSet;
            
        }
           
        #region IDefaultValue Members

        public object  GetValue()
        {
 	        string aexpression = getExpressionString(_fd.CopyFromFieldId);

            FieldDescription[] fds =  FieldDefintion.GetFieldDescriptions(_dc);
            foreach (FieldDescription fd in fds)
            {
                aexpression = aexpression.Replace(fd.FieldId, _recordSet.Tables[0].Rows[0][fd.FieldId].ToString());
            }
            //if (_fd.FieldId.Equals("TimeToReachEAV") || _fd.FieldId.Equals("TimeToReachELV"))
            //{
            //    double expRes = (double)Evaluate2(aexpression);
            //    int hours = (int)expRes;
            //    int min = (int)((expRes * 60) % 60);
            //    return new DateTime(2000, 1, 1, hours, min, 0);
                                  
            //} 
            return formatExp(Evaluate2(aexpression));
        }

        #endregion

        private object formatExp(object result)
        {
            object formatedValue = result;

            switch (_fd.ControlType.ToUpper().TrimEnd())			  //JOF added trimend, easy to accidentally add whitespace in the database
            {
                case "FLOAT":
                    int numDigits = 2;
                    if (_fd.Dataformat.StartsWith("float"))
                    {
                        try
                        {
                            numDigits = int.Parse(_fd.Dataformat.Replace("float", ""));
                            formatedValue = Math.Round((double)result, numDigits);
                        }
                        catch
                        {
                            formatedValue = result;
                        }

                    }
                    break;
                default:
                    formatedValue = result;
                    break;
            }
                return formatedValue;
        }



        private double Evaluate(string expression) {
          DataTable loDataTable = new DataTable();
          DataColumn loDataColumn = new DataColumn("Eval", typeof (double), expression);
          loDataTable.Columns.Add(loDataColumn);
          loDataTable.Rows.Add(0);
          return (double) (loDataTable.Rows[0]["Eval"]);
        }

        private object Evaluate2(string expression)
        {
            Expression exp = new Expression(expression);
            return exp.Evaluate();
        }

        /// <summary>
        /// return avalue part of a CopyFromFieldId value pair in the format <%value=avalue%>
        /// </summary>
        /// <param name="valuepair"></param>
        /// <returns></returns>
        private string getExpressionString(string valuepair)
        {
            //remove <%value= and %>
            string avalue = valuepair.TrimEnd().Replace("<%expression=", "").Replace("%>", "");
            return avalue;
        }

}
}
