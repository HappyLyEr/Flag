using System;
using GASystem;
using GASystem.DataModel;

namespace GASystem.DataAccess.Utils.SQLView
{
	/// <summary>
	/// Returns a general sql statement for a view in flag. Views using this class must have a path column linked in from the 
	/// path collumn in gasuperclass. Path column is used for filtering by owner context 
	/// </summary>
	public class GeneralSQLView : ISQLView
	{

		public GeneralSQLView(AppUtils.ClassDescription cd, GADataRecord Owner) : base(cd, Owner)
		{
		}

		public override string GetSQLViewQuery()
		{
            //generate base sql from fielddefinition
            string selectSqlOwner = SQLGenerateUtils.GenerateSelectListViewFromFieldDefinition(GADataRecord.ParseGADataClass(this.ClassDesc.DataClassName));
            
            //string sqlview = "select * from {0} where path like '%{1}-{2}/%'";
            string sqlview;
            if ("GAActionWorkitemView" == this.ClassDesc.DataClassName)
            {
                sqlview = selectSqlOwner + " AND (GAActionWorkitemView.OwnerClass = '{0}' AND GAActionWorkitemView.OwnerClassRowId = {1})";
            }
            else
            {
                sqlview = selectSqlOwner + " AND (PathLevel1 = '{0}-{1}' OR PathLevel2 = '{0}-{1}' OR PathLevel3 = '{0}-{1}' OR PathLevel4 = '{0}-{1}' OR PathLevel5 = '{0}-{1}' OR PathLevel6 = '{0}-{1}')";    
            }
			string sql=  string.Format(sqlview, Owner.DataClass.ToString(), Owner.RowId.ToString());
			string dateFilter = GenerateDateFilter();
			if (dateFilter != string.Empty)
				sql += " and " + dateFilter;
			return sql;
		}

	}
}
