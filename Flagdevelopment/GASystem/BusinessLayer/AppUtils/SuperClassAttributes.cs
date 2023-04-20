using System;
using System.Data;
using GASystem.DataAccess.Security;
using GASystem.GAExceptions;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Collections;

namespace GASystem.AppUtils
{
    // Tor 201611 Security 20161028 new class to update GASuperClass attributes
    public class SuperClassAttributes
    {
        public static void UpdateGASuperClassChangedBy(DataSet ds, GADataTransaction transaction)
        {
            // Tor 201611 Security 20161003 Add SuperClass ChangedBy and DateChanged for each modified record
            int personnelRowId = GASystem.DataAccess.Security.GASecurityDb_new.GetPersonnelRowIdByLogonId(GASystem.DataAccess.Security.GASecurityDb_new.GetCurrentUserId().ToString());
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    SuperClassDb.UpdateChangedByAndDateChanged(row, GADataRecord.ParseGADataClass(row.Table.TableName.ToString()), personnelRowId, transaction);
                }
            }
        }
    }
}
