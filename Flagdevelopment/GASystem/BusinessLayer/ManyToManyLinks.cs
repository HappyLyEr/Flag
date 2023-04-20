using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataModel;
using GASystem.DataAccess;


namespace GASystem.BusinessLayer
{
    class ManyToManyLinks
    {
        public static List<GADataClass> GetManyToManyLinksOwnerClassByMemberClass(GADataClass dataClass)
        {
            List<GADataClass> dataClasses = new List<GADataClass>();
            foreach (ManyToManyLinksDS.GAManyToManyLinksRow row in ManyToManyLinksDb.GetManyToManyLinksByMemberClass(dataClass).GAManyToManyLinks)
            {
                dataClasses.Add(GADataRecord.ParseGADataClass(row.LinkOwnerClass));
            }
            return dataClasses;
        }

        /// <summary>
        /// Get all many to many links pointing to a given datarecord, filtered by date
        /// </summary>
        /// <param name="memberDataRecord"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<GADataRecord> GetManyToManyOwnerDataRecordsByMemberDataRecordAndDate(GADataRecord memberDataRecord, System.DateTime date)
        {
            List<GADataRecord> foundDataRecords = new List<GADataRecord>();
            //get all possible links
            ManyToManyLinksDS dsLinkTypes = ManyToManyLinksDb.GetManyToManyLinksByMemberClass(memberDataRecord.DataClass);
            if (dsLinkTypes.GAManyToManyLinks.Rows.Count == 0)
                return foundDataRecords;   //no possible links return empty set
            
            //find links at given that date
            foreach (ManyToManyLinksDS.GAManyToManyLinksRow row in dsLinkTypes.GAManyToManyLinks)
            {
                GASystem.DataAccess.DataAccess da = new GASystem.DataAccess.DataAccess(GADataRecord.ParseGADataClass(row.LinkOwnerClass), null);
                string filter = " " + row.LinkOwnerClassField + " = " + memberDataRecord.RowId.ToString() + " " ;
                System.Data.DataSet dsLinks = da.GetRecordsWithinOwner(new GADataRecord(1, GADataClass.GAFlag), filter, date, date);
                string linkRowIdColumn = row.LinkOwnerClass.Substring(2) + "rowid";
                if (dsLinks.Tables[0].Columns.Contains(linkRowIdColumn))
                    foreach (System.Data.DataRow linkRow in dsLinks.Tables[0].Rows)
                    {
                        foundDataRecords.Add(new GADataRecord((int)linkRow[linkRowIdColumn], GADataRecord.ParseGADataClass(row.LinkOwnerClass)));
                    }
                
            }
            return foundDataRecords;
        }


    }
}
