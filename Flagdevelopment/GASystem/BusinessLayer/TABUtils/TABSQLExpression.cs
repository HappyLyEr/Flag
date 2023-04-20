using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GASystem.DataModel;
using System.Data.SqlClient;
using GASystem.AppUtils;


namespace GASystem.BusinessLayer.TABUtils
{
    class TABSQLExpression 
    {
        
        /// <summary>
        /// returns false (meaning hide TAB) when sql result returns record(s), else true
        /// </summary>
        /// <param name="owner record, superclasslinks row"></param>
        /// <returns></returns>
        public static bool isTabVisible(GADataRecord owner, SuperClassLinksDS.GASuperClassLinksRow row)
        {
            if (owner==null) return false;
            //example:
            //<%sql=select l.LocationRowId,l.Name,li.* 
            //from <%ownerClass%> l 
            //inner join GALists li on li.ListsRowId=l.TypeOfLocationListsRowId and li.GAListValue in ('Address') and li.GAListCategory='TL' 
            //where l. <%ownerClassRowId%>=<%row.RowId%>
            
            string sql = row.nTextFree1.ToString().Replace("<%sql=", "");
            sql = sql.Replace("<%ownerClass%>", owner.DataClass.ToString());
            sql = sql.Replace("<%ownerClassRowId%>", owner.DataClass.ToString().Substring(2)+"RowId");
            sql = sql.Replace("<%row.RowId%>", owner.RowId.ToString());

            SqlDataReader result=GASystem.DataAccess.DataUtils.executeSelect(sql);
            if (result.HasRows)
                while (result.Read()) 
                {   // returns first record 
                    if (result.GetInt32(0) > 0) return false;
                    else return true;
                }
//            return false;
            return true;
        }
    }
}
