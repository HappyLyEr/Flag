using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GASystem.DataModel;

namespace GASystem.DataAccess
{
    public interface IDataAccessListView
    {
        DataSet GetAll();
        DataSet GetByOwner(GADataRecord owner, string filter);
        DataSet GetByOwnerAndTimeSpan(GADataRecord owner, System.DateTime startDate, System.DateTime endDate, string filter);
        DataSet GetRecordsWithinOwner(GADataRecord owner, string filter, System.DateTime startDate, System.DateTime endDate);
        DataSet GetByDataRecord(GADataRecord dataRecord);
    }
}
