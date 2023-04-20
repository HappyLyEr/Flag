using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GASystem.DataModel;
using System.Collections;

namespace GASystem.DataAccess
{
    public interface IDataAccessReportView
    {
        DataSet GetAll();
        DataSet GetByOwner(GADataRecord owner, string filter);
        //DataSet GetByOwnerAndTimeSpan(GADataRecord owner, System.DateTime startDate, System.DateTime endDate, string filter);
        DataSet GetRecordsWithinOwner(GADataRecord owner, string filter, System.DateTime startDate, System.DateTime endDate);
        DataSet GetByRowId(int rowId);
        DataSet GetByRowIds(ArrayList rowIds);
    }
}
