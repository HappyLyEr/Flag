using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataAccess;

namespace GASystem.BusinessLayer.DefaultValues
{
    public class DefaultValueFactory
    {
        public IDefaultValue Make(GASystem.AppUtils.FieldDescription fd, GASystem.DataModel.GADataRecord owner, GADataTransaction transaction)
        {
            if (fd.CopyFromFieldId == "<%datetimenow%>")
                return new DateTimeNowValue();

            // Tor 20140604 Added to get UTC date time values 
            if (fd.CopyFromFieldId == "<%datetimenowUTC%>")
                return new DateTimeNowUTCValue();

            // Tor 20140604 Added to compute date values based on system dates
            if (fd.CopyFromFieldId.IndexOf("<%datecompute;") == 0) //date field is to be computed
                return new DateComputeValue(fd);
            
            if (fd.CopyFromFieldId == "<%getreporter%>")
                return new ReporterValue();

            if (fd.CopyFromFieldId.IndexOf("<%value=") == 0)  //field contains a value of type <%value=avalue%>
                return new StaticValue(fd);

            if (fd.CopyFromFieldId.IndexOf("<%parentvalue=") == 0)  //field contains a value of type <%value=avalue%>
                return new ParentValue(fd, owner, transaction);

            return new DBNullValue();
        }

        public IDefaultValue MakeOnUpdate(GASystem.AppUtils.FieldDescription fd, GASystem.DataModel.GADataRecord owner, GADataTransaction transaction, System.Data.DataSet RecordSet, GASystem.DataModel.GADataClass DataClass)
        {
            String copyFromFieldId = fd.CopyFromFieldId.Replace("OnUpdate:", "");

            fd.CopyFromFieldId = fd.CopyFromFieldId.Replace("OnUpdate:", "");

            if (copyFromFieldId == "<%datetimenow%>")
                return new DateTimeNowValue();

            // Tor 20140604 Added to get UTC date time values 
            if (copyFromFieldId == "<%datetimenowUTC%>")
                return new DateTimeNowUTCValue();

            // Tor 20140604 Added to compute date values based on system dates
            if (copyFromFieldId.IndexOf("<%datecompute;") == 0) //date field is to be computed
                return new DateComputeValue(fd);

            if (copyFromFieldId == "<%getreporter%>")
                return new ReporterValue();

            if (copyFromFieldId.IndexOf("<%value=") == 0)  //field starts with <%value= and contains a value of type <%value=avalue%>
                return new StaticValue(fd);

            if (copyFromFieldId.IndexOf("<%parentvalue=") == 0)  //field starts with <%parentvalue=
                return new ParentValue(fd, owner, transaction);

            if (copyFromFieldId.IndexOf("<%expression=") == 0)  //field starts with <%expression=
                return new EvaluateExpression(fd,DataClass,RecordSet);

            if (copyFromFieldId.IndexOf("<%lookupfield=") == 0)  //field starts with <%lookupfield=
                return new LookupField(fd,DataClass,RecordSet);

            if (copyFromFieldId.IndexOf("<%sql=") == 0)  //field starts with <%sql=
                return new SQLExpression(fd, owner, DataClass, RecordSet);

            return new DBNullValue();
        }

        // Tor 20170309 set column value from other column in the same record
        public IDefaultValue MakeFromCurrentRecord(GASystem.AppUtils.FieldDescription fd, GASystem.DataModel.GADataRecord owner
            , GADataTransaction transaction, System.Data.DataSet RecordSet,System.Data.DataRow row, GASystem.DataModel.GADataClass DataClass)
        {
            if (row[fd.FieldId]!=null)
            { // check if copyfromfield exists in this record and source and target is same format
                GASystem.AppUtils.FieldDescription cfd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(fd.CopyFromFieldId, DataClass.ToString());
                if (cfd != null && cfd.ColumnType == fd.ColumnType && row[cfd.FieldId]!=null)
                {
                    if (fd.ColumnType == "int") return new GetIntValue((int)row[cfd.FieldId]);
                    if (fd.ColumnType == "nvarchar" || fd.ColumnType == "ntext" || fd.ColumnType == "char" || fd.ColumnType == "nchar" ||
                        fd.ColumnType == "varchar") return new GetStringValue(row[cfd.FieldId].ToString());
                    if (fd.ColumnType == "datetime") return new GetDateTimeValue((DateTime)row[cfd.FieldId]);
                    if (fd.ColumnType == "bit") return new GetBitValue((Boolean)row[cfd.FieldId]);
                    if (fd.ColumnType == "smallint") return new GetSmallIntValue((Int16)row[cfd.FieldId]);
                }
            }
            return new DBNullValue();
        }
    }
}
