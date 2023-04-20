using System;
using GASystem;
using GASystem.DataModel;

namespace GASystem.DataAccess.Utils.SQLView
{
	/// <summary>
	/// Returns a general sql statement for a view in flag. Views using this class must have a path column linked in from the 
	/// path collumn in gasuperclass. Path column is used for filtering by owner context 
	/// </summary>
	public class FlagClassDefinedSQLView : ISQLView
	{
        private const string SQLDNNDATABASENAME = "<%DNNDatabaseName%>";

        public FlagClassDefinedSQLView(AppUtils.ClassDescription cd, GADataRecord Owner) : base(cd, Owner)
		{
		}

		public override string GetSQLViewQuery()
		{
            //generate base sql from fielddefinition
            string selectSqlOwner = SQLGenerateUtils.GenerateSelectListViewFromFieldDefinition(GADataRecord.ParseGADataClass(this.ClassDesc.DataClassName));

            string subQuery = " (" + ClassDesc.ViewSQL + ") as " + ClassDesc.DataClassName + " ";

            selectSqlOwner = selectSqlOwner.Replace("from  ", "from ");
            selectSqlOwner = selectSqlOwner.Replace("from " + ClassDesc.DataClassName, " from " + subQuery);

            selectSqlOwner = selectSqlOwner.Replace(SQLDNNDATABASENAME, Utils.DatabaseSettings.DNNDatabaseName);

            //string sqlview = "select * from {0} where path like '%{1}-{2}/%'";
            
            //string sqlview = selectSqlOwner + " AND (gasuperclass.ReadRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.UpdateRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.CreateRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.DeleteRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.TextFree1 = '{0}-{1}'"
            //                                + " OR gasuperclass.TextFree2 = '{0}-{1}')";
            //and replaced again with the statement below 
            
            string sqlview = selectSqlOwner + " and path like '%{0}-{1}/%'"; //has been changed with statement above

			string sql=  string.Format(sqlview, Owner.DataClass.ToString(), Owner.RowId.ToString());
			string dateFilter = GenerateDateFilter();
			if (dateFilter != string.Empty)
				sql += " and " + dateFilter;
			return sql;
		}


        public override string getByRowId(int rowid)
        {
            string selectSqlOwner = SQLGenerateUtils.GenerateSelectListViewFromFieldDefinition(GADataRecord.ParseGADataClass(this.ClassDesc.DataClassName));

            string subQuery = " (" + ClassDesc.ViewSQL + ") as " + ClassDesc.DataClassName + " ";

            selectSqlOwner = selectSqlOwner.Replace("from  ", "from ");
            selectSqlOwner = selectSqlOwner.Replace("from " + ClassDesc.DataClassName, " from " + subQuery);

            selectSqlOwner = selectSqlOwner.Replace(SQLDNNDATABASENAME, Utils.DatabaseSettings.DNNDatabaseName);

            //string sqlview = "select * from {0} where path like '%{1}-{2}/%'";

            //string sqlview = selectSqlOwner + " AND (gasuperclass.ReadRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.UpdateRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.CreateRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.DeleteRoles = '{0}-{1}'"
            //                                + " OR gasuperclass.TextFree1 = '{0}-{1}'"
            //                                + " OR gasuperclass.TextFree2 = '{0}-{1}')";
            //and replaced again with the statement below 

            //string sqlview = selectSqlOwner + " and path like '%{0}-{1}/%'"; //has been changed with statement above


            return selectSqlOwner + " and " + _cd.DataClassName.Substring(2) + "RowId = " + rowid.ToString();
        }
	}
}
