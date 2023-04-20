using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace GASystem.DataAccess
{
    class ManyToManyLinksDb
    {
        private static string GAManyToManyLinksTableName = "GAManyToManyLinks";
        // Tor 20150221 changed 
        // "WHERE     (dbo.GAClass.ManyToManyField IS NOT NULL)" 
        // to "WHERE     (dbo.GAClass.ManyToManyField IS NOT NULL and dbo.GAClass.ManyToManyField!='')"
        
        private static string _selectManyToManyDefinitions =
             @" SELECT     dbo.GAClass.Class AS LinkOwnerClass, dbo.GAClass.ManyToManyField AS LinkOwnerClassField, 
                      dbo.GAFieldDefinitions.LookupTable AS LinkMemberClass, dbo.GAFieldDefinitions.LookupTableKey AS LinkMemberClassField
                FROM         dbo.GAClass LEFT OUTER JOIN
                      dbo.GAFieldDefinitions ON dbo.GAClass.Class = dbo.GAFieldDefinitions.TableId AND 
                      dbo.GAClass.ManyToManyField = dbo.GAFieldDefinitions.FieldId
                WHERE     (dbo.GAClass.ManyToManyField IS NOT NULL and dbo.GAClass.ManyToManyField!='')
                 ";

        public static ManyToManyLinksDS GetManyToManyLinksByMemberClass(GADataClass memberClass)
        {
            string whereStatement = " and GAFieldDefinitions.LookupTable = '" + memberClass.ToString() + "'";
            return fillDataSet(_selectManyToManyDefinitions + whereStatement);
        }

        private static ManyToManyLinksDS fillDataSet(string sql)
        {
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            ManyToManyLinksDS ds = new ManyToManyLinksDS();
            da.Fill(ds, GAManyToManyLinksTableName);
            myConnection.Close();
            return ds;
        }



    }
}
