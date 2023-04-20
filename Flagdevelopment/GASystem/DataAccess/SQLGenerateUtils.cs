using System;
using System.Text;
using GASystem.DataAccess.Security;
using GASystem.DataModel;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for SQLGenerateUtils.
	/// </summary>
	public class SQLGenerateUtils
	{
		private static string _selectSqlMembers = @"SELECT     {0}.* 
										FROM       {0} INNER JOIN
										GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId
										WHERE     (dbo.GASuperClass.OwnerClassRowId = {2}) AND (dbo.GASuperClass.OwnerClass = '{3}' ) AND (dbo.GASuperClass.MemberClass = '{0}' )"	;

		private static string _selectSqlFileMembers = @"SELECT     {0}.FileRowId, {0}.FileId, {0}.FileName, {0}.Description, {0}.Owner, {0}.mimetype
										FROM       {0} INNER JOIN
										GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId
										WHERE     (dbo.GASuperClass.OwnerClassRowId = {2}) AND (dbo.GASuperClass.OwnerClass = '{3}' ) AND (dbo.GASuperClass.MemberClass = '{0}' )"	;
		private static string _selectSqlReportMembers = @"SELECT     {0}.ReportRowId, {0}.MeetingRowId, {0}.MeetingReferenceId, {0}.MeetingStartTime, {0}.MimeType, {0}.Text
										FROM       {0} INNER JOIN
										GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId
										WHERE     (dbo.GASuperClass.OwnerClassRowId = {2}) AND (dbo.GASuperClass.OwnerClass = '{3}' ) AND (dbo.GASuperClass.MemberClass = '{0}' )"	;

		private static string _selectSqlTextMembers = @"SELECT    {0}.{1}RowId, {0}.Header, substring({0}.[text], 0, 20) as [Text]
										FROM       {0} INNER JOIN
										GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId
										WHERE     (dbo.GASuperClass.OwnerClassRowId = {2}) AND (dbo.GASuperClass.OwnerClass = '{3}' ) AND (dbo.GASuperClass.MemberClass = '{0}' )"	;

//        private static string _selectSqlFullDetailsMembers = @"SELECT {4} 
//										FROM {0} INNER JOIN
//										GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId";

        //private static string _selectSqlFullDetailsMembersWhere = @" WHERE (dbo.GASuperClass.OwnerClassRowId = {2}) AND (dbo.GASuperClass.OwnerClass = '{3}' ) AND (dbo.GASuperClass.MemberClass = '{0}' )"	;

//        private static string _selectSqlOwnersRoleFullDetailsCurrentMembersWhere = 
//@"select {0}.* from {0} inner join GASuperClass on GASuperClass.MemberClass='{0}' and GASuperClass.MemberClassRowId={0}.{1}RowId
//where 1=1  and {0}.RoleListsRowId={2} and {0}.FromDate <= '{3}' and ({0}.ToDate is null or {0}.ToDate>='{3}') ";

        private static string _selectSqlOwnersRoleOrJobTitleFullDetailsCurrentMembersWhere =
@"select {0}.* from {0} inner join GASuperClass on GASuperClass.MemberClass='{0}' and GASuperClass.MemberClassRowId={0}.{1}RowId
where 1=1  and {0}.{4}={2} and {0}.FromDate <= '{3}' and ({0}.ToDate is null or {0}.ToDate>='{3}') ";

        //private static string _selectSqlOwnersMembersWhere =@" (GASuperClass.OwnerClass='{0}' and GASuperClass.OwnerClassRowId={0}) ";

	
		public SQLGenerateUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public static string SearchEmploymentsByOwnerRecordsDateAndRoleId(System.Collections.Generic.List<GADataRecord> owners, string memberClass, System.DateTime EmploymentDate, int roleId)
        {
            return SearchEmploymentsByOwnerRecordsDateAndRoleIdOrJobTitle(owners, memberClass, EmploymentDate, roleId, "RoleListsRowId");
        }

        public static string SearchEmploymentsByOwnerRecordsDateAndJobTitle(System.Collections.Generic.List<GADataRecord> owners, string memberClass, System.DateTime EmploymentDate, int roleId)
        {
            return SearchEmploymentsByOwnerRecordsDateAndRoleIdOrJobTitle(owners, memberClass, EmploymentDate, roleId, "JobDescription");
        }

        public static string SearchEmploymentsByOwnerRecordsDateAndRoleIdOrJobTitle(System.Collections.Generic.List<GADataRecord> owners, string memberClass, System.DateTime EmploymentDate, int roleId, string FieldName)
        {
            string dato = EmploymentDate.Year.ToString() + "-" + EmploymentDate.Month.ToString() + "-" + EmploymentDate.Day.ToString();
//            string sql = string.Format(_selectSqlOwnersRoleFullDetailsCurrentMembersWhere, new Object[] { memberClass, memberClass.Substring(2), roleId.ToString(), dato });
            string sql = string.Format(_selectSqlOwnersRoleOrJobTitleFullDetailsCurrentMembersWhere, new Object[] { memberClass, memberClass.Substring(2), roleId.ToString(), dato, FieldName });

            String FILTER = string.Empty; // create owner filter
            int i = -1;
            foreach (GADataRecord owner in owners)
            {
                i++;
                if (i > 0) FILTER = FILTER + " or ";
                FILTER = FILTER + " (GASuperClass.OwnerClass='" + owner.DataClass.ToString() + "' and GASuperClass.OwnerClassRowId=" + owner.RowId + ") ";
            }
            if (FILTER != string.Empty) FILTER = " and (" + FILTER + ")";
            sql = sql + FILTER;
            return sql;
        }

        public static string GetSelectSqlMembers(GADataClass MemberDataClass, int OwnerRowId, GADataClass OwnerDataClass)
		{
			string sql = "";
			if (MemberDataClass == GADataClass.GAFile)
			{
				sql = _selectSqlFileMembers;
			} 
			else if (MemberDataClass == GADataClass.GATextItem)
			{
				sql = _selectSqlTextMembers;
			}
			else if (MemberDataClass == GADataClass.GAReport)
			{
				sql = _selectSqlReportMembers;
			}
			else
			{
				sql = _selectSqlMembers;
			}
			return String.Format(sql, new Object[] { MemberDataClass, MemberDataClass.ToString().Substring(2), OwnerRowId, OwnerDataClass });
		}

		public static string GenerateSelectAllWithinFromFieldDefinition(GADataClass MemberDataClass, int OwnerRowId, GADataClass OwnerDataClass) 
		{
			Utils.SQLSelect sql = new GASystem.DataAccess.Utils.SQLSelect();
			String sqlStr = sql.GenerateSQLAllWithin(MemberDataClass, OwnerRowId, OwnerDataClass);
			return sqlStr;
		}


        /// <summary>
        /// SQL for listing all records of type memberdataclass in listview. 
        /// </summary>
        /// <param name="MemberDataClass"></param>
        /// <returns></returns>
        public static string GenerateSelectListViewFromFieldDefinition(GADataClass MemberDataClass)
        {
            Utils.SQLSelectListView sql = new GASystem.DataAccess.Utils.SQLSelectListView();
            return sql.GenerateSQL(MemberDataClass);
        }


        /// <summary>
        /// SQL for getting a single record in listview format
        /// </summary>
        /// <param name="DataRecord"></param>
        /// <returns></returns>
        public static string GenerateSelectListViewFromFieldDefinition(GADataRecord DataRecord)
        {
            Utils.SQLSelectListView sql = new GASystem.DataAccess.Utils.SQLSelectListView();
            return sql.GenerateSQL(DataRecord);
        }

        /// <summary>
        /// SQL for all records of type memberdataclass by owner in listview
        /// </summary>
        /// <param name="MemberDataClass"></param>
        /// <param name="OwnerRowId"></param>
        /// <param name="OwnerDataClass"></param>
        /// <returns></returns>
        public static string GenerateSelectListViewFromFieldDefinition(GADataClass MemberDataClass, int OwnerRowId, GADataClass OwnerDataClass)
        {
            Utils.SQLSelectListView sql = new GASystem.DataAccess.Utils.SQLSelectListView();
            return sql.GenerateSQL(MemberDataClass, OwnerRowId, OwnerDataClass);
        }

        /// <summary>
        /// SQL for all records of type memberdataclass within owner in listview
        /// </summary>
        /// <param name="MemberDataClass"></param>
        /// <param name="OwnerRowId"></param>
        /// <param name="OwnerDataClass"></param>
        /// <returns></returns>
        public static string GenerateSelectListViewAllWithinFromFieldDefinition(GADataClass MemberDataClass, int OwnerRowId, GADataClass OwnerDataClass) 
		{
            Utils.SQLSelectListView sql = new GASystem.DataAccess.Utils.SQLSelectListView();
            String sqlStr = sql.GenerateSQLAllWithin(MemberDataClass, OwnerRowId, OwnerDataClass);
			return sqlStr;
		}

		public static string GenerateSelectForGATableAllWithinOwner(GADataClass MemberDataClass, int OwnerRowId, GADataClass OwnerDataClass) 
		{
			Utils.SQLSelect sql = new GASystem.DataAccess.Utils.SQLSelect();
			String sqlStr = sql.GenerateSQLForGATableAllWithin(MemberDataClass, OwnerRowId, OwnerDataClass);
			return sqlStr;
		}

		public static string GenerateSelectFromFieldDefinition(GADataClass MemberDataClass, int OwnerRowId, GADataClass OwnerDataClass) 
		{
			Utils.SQLSelect sql = new GASystem.DataAccess.Utils.SQLSelect();
			return sql.GenerateSQL(MemberDataClass, OwnerRowId, OwnerDataClass);
		}

		public static string GenerateSelectFromFieldDefinition(GADataClass MemberDataClass)
		{
			Utils.SQLSelect sql = new GASystem.DataAccess.Utils.SQLSelect();
			return sql.GenerateSQL(MemberDataClass);
		}

		public static string GenerateSelectFromFieldDefinition(GADataRecord DataRecord) 
		{
			Utils.SQLSelect sql = new GASystem.DataAccess.Utils.SQLSelect();
			return sql.GenerateSQL(DataRecord);
		}

        //public static string selectSqlOwnersRoleFullDetailsCurrentMembersWhere(GADataClass member,List<GADataRecord> owner)
        //{
        //    String a = _selectSqlOwnersRoleFullDetailsCurrentMembersWhere;
        //}

		private static string GenerateCombinedColumns(GASystem.AppUtils.FieldDescription FieldDescriptionInfo) 
		{
			//add table alias to select columns
			string tableAlias = GenerateTableAlias(FieldDescriptionInfo);
			string columnSelect = "''";
			foreach (String columnName in FieldDescriptionInfo.GetLookupTableDisplayColumns())
			{
				columnSelect += " + " + tableAlias + "." +  columnName + " + ' ' ";
			}

			columnSelect += " as " + FieldDescriptionInfo.FieldId;

			return columnSelect;
		}
		
		//generate join statement for joined select. adds a uniqe tablealias in case the same table is referenced
		//twice or more from different columns
		private static string GenerateJoinStatement(GASystem.AppUtils.FieldDescription FieldDescriptionInfo) 
		{
			string tableAlias = GenerateTableAlias(FieldDescriptionInfo);
			string join = " left join {0} as {1} on {1}.{2} = {3}.{4} ";
			return string.Format(join, new object[] {FieldDescriptionInfo.LookupTable
													, tableAlias
													, FieldDescriptionInfo.LookupTable.Substring(2) + "RowId"
													, FieldDescriptionInfo.TableId
													, FieldDescriptionInfo.FieldId});
							 
		}

		//generete uniqe table alias. used for joined tables.
		private static string GenerateTableAlias(GASystem.AppUtils.FieldDescription FieldDescriptionInfo) 
		{
			return FieldDescriptionInfo.LookupTable + FieldDescriptionInfo.ColumnOrder;
		}
	}
}
