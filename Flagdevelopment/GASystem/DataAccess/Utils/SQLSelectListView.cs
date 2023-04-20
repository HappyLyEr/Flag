using System;
using System.Collections;
using System.Web;
using GASystem.DataAccess.Security;
using GASystem.DataModel;
using log4net;
using log4net.Appender;
using log4net.Config;


namespace GASystem.DataAccess.Utils
{
	/// <summary>
	/// Summary description for SQLSelect.
	/// </summary>
    public class SQLSelectListView
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(SQLSelect));

		private ArrayList m_columns = new ArrayList();
		private string m_from;
		private string m_where;
        private string m_join;
		//private GADataRecord m_currentDateRecord;
		private int TableAliasId = 0;
		

		private static string _selectSqlFullDetailsMembers = @"SELECT {4}
										FROM {5} INNER JOIN
										GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId WHERE (dbo.GASuperClass.OwnerClassRowId = {2}) AND (dbo.GASuperClass.OwnerClass = '{3}' ) AND (dbo.GASuperClass.MemberClass = '{0}' )";
		
		private static string _selectSqlFullDetailsWithinMembers = @"SELECT {4}
										FROM {5} INNER JOIN
										GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId WHERE (dbo.GASuperClass.MemberClass = '{6}' )  
                                        AND (GASuperClass.ReadRoles = '{3}-{2}'
                                             OR GASuperClass.UpdateRoles = '{3}-{2}'
                                             OR GASuperClass.CreateRoles = '{3}-{2}'
                                             OR GASuperClass.DeleteRoles = '{3}-{2}'
                                             OR GASuperClass.TextFree1 = '{3}-{2}'
                                             OR GASuperClass.TextFree2 = '{3}-{2}')";
        //(GASuperClass.path like '%{3}-{2}/%') has been replaced in the statement above";
		
//        private static string _selectSqlFullDetailsWithinMembersSecure = @"SELECT {4}
//										FROM {5} INNER JOIN
//										GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId WHERE (dbo.GASuperClass.MemberClass = '{0}' )  AND path+'/' like '%{3}-{2}/%'";


//        private static string _selectWithInPath = @"SELECT * FROM {1} MyQuery INNER JOIN ( 
//														select Path AS AccessPath, RoleListsRowId  
//														from GAEmployment INNER JOIN GASuperClass ON EmploymentRowId=MemberClassRowId  
//														WHERE Personnel={2} AND MemberClass='GAEmployment') AccessPaths ON AccessPaths.Path LIKE Path+'%' ";




        public SQLSelectListView()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        /// <summary>
        /// generate the select list for a list of datarecords, uses fielddefinition hideinsummary to limit included fields
        /// </summary>
        /// <param name="DataClass"></param>
		private void PopulateUsingFieldDescription(GADataClass DataClass) 
		{
			//m_currentDateRecord = DataRecord;
			GASystem.AppUtils.FieldDescription[] fds = GASystem.AppUtils.FieldDefintion.GetFieldDescriptions(DataClass);
	
			m_from = DataClass.ToString();
			foreach (GASystem.AppUtils.FieldDescription fd in fds) 
			{
                if (!fd.HideInSummary || !fd.HideInExcel)
                {
                    if (fd.FieldId != DataClass.ToString().Substring(2) + "RowId")   //TODO bugfix, some tables has rowid column defined in fd. ignore here since it is added earlier
                    {
                        if ((fd.ControlType.ToUpper() == "LOOKUPFIELD" || fd.ControlType.ToUpper() == "LOOKUPFIELDEDIT" || fd.ControlType.ToUpper() == "LOOKUPFIELDVIEW" || fd.ControlType.ToUpper() == "LOOKUPFIELDMULTIPLE" || fd.ControlType.ToUpper() == "RESPONSIBLE")
                                && fd.LookupTable != string.Empty)
                        {
                            m_columns.Add(GenerateCombinedColumns(fd, fd.TableId) + " as " + fd.FieldId);
                            m_columns.Add(fd.TableId + "." + fd.FieldId + " as " + fd.FieldId + "_keyid");
                        }
                        else if (fd.ControlType.ToUpper() == "FILELOOKUPFIELD" && fd.LookupTable != string.Empty)
                        {
                            m_columns.Add(GenerateCombinedColumns(fd, fd.TableId) + " as " + fd.FieldId);
                            m_columns.Add(fd.TableId + "." + fd.FieldId + " as " + fd.FieldId + "_rowid");
                        }
                        else if (fd.ControlType.ToUpper() == "DROPDOWNLIST" || fd.ControlType.ToUpper() == "POSTBACKDROPDOWNLIST"
                            // Tor 20171215 added DROPDOWNLISTMULTIPLE
                       //|| fd.ControlType.ToUpper() == "DROPDOWNLISTMULTIPLE"
                            )
                        {
                            m_columns.Add(GenerateColumnForLists(fd, fd.TableId) + " as " + fd.FieldId + "_displayfield");
                            m_columns.Add(fd.TableId + "." + fd.FieldId);
                        }
                        else
                        {
                            m_columns.Add(fd.TableId + "." + fd.FieldId);

                            //m_from += GenerateJoinStatement(fd);
                        }

                    }
                }
			}
		}

       
		/// <summary>
		/// Genereate SQL for a singel DataRecord
		/// </summary>
		/// <param name="DataRecord">DataRecord</param>
		/// <returns>sql string</returns>
		public string GenerateSQL(GADataRecord DataRecord) 
		{
			m_columns.Clear();
			m_from = "";
		    m_where = "";
		    m_join = "";
			m_columns.Add(DataRecord.DataClass.ToString() + "." + DataRecord.DataClass.ToString().Substring(2) + "RowId");
			PopulateUsingFieldDescription(DataRecord.DataClass);
			
			string selectSql = ArrayListToString(m_columns, ",");
			m_where = DataRecord.DataClass.ToString() + "." + DataRecord.DataClass.ToString().Substring(2) + "RowId" + " = " + DataRecord.RowId.ToString();
			string sql = "select " + selectSql + " from " +  m_from + " where " +  m_where;
			return sql;
		}

		public string WithSecurity(string sql, GADataClass DataClass, String Roles)
		{
			return sql;
		}

		/// <summary>
		/// Generate SQL for getting all records with details of type GADataClass with in owner datarecord
		/// </summary>
		/// <param name="MemberDataClass">Record types to get</param>
		/// <param name="OwnerRowId">Owner rowid</param>
		/// <param name="OwnerDataClass">Owner GADataClass type</param>
		/// <returns>sql string</returns>
		public string GenerateSQLAllWithin(GADataClass MemberDataClass, int OwnerRowId, GADataClass OwnerDataClass) 
		{
            //get class definition
            AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(MemberDataClass);
            


			m_columns.Clear();
			m_from = "";
			m_where = "";
			m_join = "";
			m_columns.Add(MemberDataClass.ToString() + "." + MemberDataClass.ToString().Substring(2) + "RowId");
			PopulateUsingFieldDescription(MemberDataClass);
			
			string selectSql = ArrayListToString(m_columns, ",");
			//m_where = DataRecord.DataClass.ToString().Substring(2) + "RowId" + " = " + DataRecord.RowId.ToString();
			//string sql = "select " + selectSql + " from " +  m_from + " where " +  m_where;
			
			//add select different sql based on whether or not it is a view?????  todo for moving sqlview to this select
			string sql = _selectSqlFullDetailsWithinMembers;
            string superClassMemberClass = MemberDataClass.ToString();
            if (cd.IsView && cd.VirtualClassAttributeName != string.Empty)
                superClassMemberClass = cd.VirtualClassAttributeName;

            sql = String.Format(sql, new Object[] { MemberDataClass, MemberDataClass.ToString().Substring(2), OwnerRowId, OwnerDataClass, selectSql, m_from, superClassMemberClass });
			
            //hack  workitem may have multipe owners. If workitem, return only records where gaaction is the owner
            if (MemberDataClass == GADataClass.GAWorkitem)
                sql += " and gasuperclass.ownerclass = 'gaaction' ";



			return sql;
		}


		/// <summary>
		/// Generate SQL for getting all records of type GADataClass owned by a DataRecord
		/// </summary>
		/// <param name="MemberDataClass">Record types to get</param>
		/// <param name="OwnerRowId">Owner rowid</param>
		/// <param name="OwnerDataClass">Owner GADataClass type</param>
		/// <returns>sql string</returns>
		public string GenerateSQL(GADataClass MemberDataClass, int OwnerRowId, GADataClass OwnerDataClass) 
		{
			
            m_columns.Clear();
			m_from = "";
			m_where = "";
			m_join = "";
			m_columns.Add(MemberDataClass.ToString() + "." + MemberDataClass.ToString().Substring(2) + "RowId");
			PopulateUsingFieldDescription(MemberDataClass);
			
			string selectSql = ArrayListToString(m_columns, ",");
			//m_where = DataRecord.DataClass.ToString().Substring(2) + "RowId" + " = " + DataRecord.RowId.ToString();
			//string sql = "select " + selectSql + " from " +  m_from + " where " +  m_where;
			string sql = _selectSqlFullDetailsMembers;
			sql = String.Format(sql, new Object[] {MemberDataClass, MemberDataClass.ToString().Substring(2), OwnerRowId, OwnerDataClass, selectSql, m_from  });

            //hack  workitem may have multipe owners. If workitem, return only records where gaaction is the owner
            if (MemberDataClass == GADataClass.GAWorkitem)
                sql += " and gasuperclass.ownerclass = 'gaaction' ";



			return sql;
		}

		/// <summary>
		/// Generate SQL for getting all records of type GADataClass
		/// </summary>
		/// <param name="MemberDataClass">Record types to get</param>
		/// <returns>sql string</returns>
		public string GenerateSQL(GADataClass MemberDataClass) 
		{
			m_columns.Clear();
			m_from = "";
			m_where = "";
			m_join = "";
			m_columns.Add(MemberDataClass.ToString() + "." + MemberDataClass.ToString().Substring(2) + "RowId");
			PopulateUsingFieldDescription(MemberDataClass);
			string selectSql = ArrayListToString(m_columns, ",");
			//m_where = DataRecord.DataClass.ToString().Substring(2) + "RowId" + " = " + DataRecord.RowId.ToString();
			//string sql = "select " + selectSql + " from " +  m_from + " where " +  m_where;
			string sql = "select " + selectSql + " from " +  m_from + " where 1=1";
			return sql;
		}


		private string ArrayListToString(ArrayList List, string Seperator) 
		{
			if (List.Count == 0)
				return string.Empty;
			
			string temp = List[0].ToString();
			for (int t = 1; t < List.Count; t++) 
			{
				temp += Seperator + " " + List[t].ToString();
			}
			return temp;
		}

		private string GenerateCombinedColumns(GASystem.AppUtils.FieldDescription FieldDescriptionInfo, string CurrentTableAlias) 
		{
			//add table alias to select columns
			string tableAlias = GenerateTableAlias(FieldDescriptionInfo);
			//add lookuptable to from statement
			m_from += GenerateJoinStatement(FieldDescriptionInfo, tableAlias, CurrentTableAlias);

			string columnSelect = "''";
			foreach (String columnName in FieldDescriptionInfo.GetLookupTableDisplayColumns())
			{
				GASystem.AppUtils.FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(columnName, FieldDescriptionInfo.LookupTable);
				if (fd.LookupTable == string.Empty)
					columnSelect += " + " + tableAlias + "." +  columnName + " + ' ' ";
				else 
				{
					columnSelect += " + " + GenerateCombinedColumns(fd, tableAlias);
				}	
			}

			return columnSelect;
		}

		private string GenerateJoinStatement(GASystem.AppUtils.FieldDescription FieldDescriptionInfo, string TableAlias, string ParentTableAlias) 
		{
			string join = " left join {0} as {1} on {1}.{2} = {3}.{4} ";
            return string.Format(join, new object[] {SQLSelect.GetLocalizedListTableJoin(FieldDescriptionInfo)
														, TableAlias
														, FieldDescriptionInfo.LookupTableKey         //.LookupTable.Substring(2) + "RowId"
														, ParentTableAlias
														, FieldDescriptionInfo.FieldId});
							 
		}

		private string GenerateTableAlias(GASystem.AppUtils.FieldDescription FieldDescriptionInfo) 
		{
			TableAliasId++;
            return FieldDescriptionInfo.LookupTable + FieldDescriptionInfo.ColumnOrder; // TableAliasId;
		}
		

		private string GenerateColumnForLists(GASystem.AppUtils.FieldDescription FieldDescriptionInfo, string CurrentTableAlias) 
		{
			//add table alias to select columns
			string tableAlias = GenerateTableAliasForList(FieldDescriptionInfo);
			//add lookuptable to from statement
			m_from += GenerateJoinStatementForLists(FieldDescriptionInfo, tableAlias, CurrentTableAlias);

			string columnSelect = tableAlias + ".GAListDescription";
			

			return columnSelect;
		}

		private string GenerateJoinStatementForLists(GASystem.AppUtils.FieldDescription FieldDescriptionInfo, string TableAlias, string ParentTableAlias) 
		{
			string join = " left join {0} as {1} on {1}.{2} = {3}.{4} ";
			return string.Format(join, new object[] {GADataClass.GALists.ToString()
														, TableAlias
														, GADataClass.GALists.ToString().Substring(2) + "RowId"
														, ParentTableAlias
														, FieldDescriptionInfo.FieldId});
							 
		}
		
		private string GenerateTableAliasForList(GASystem.AppUtils.FieldDescription FieldDescriptionInfo) 
		{
			TableAliasId++;
            return "lists" + +FieldDescriptionInfo.ColumnOrder; //TableAliasId;
		}

	
		
	}
}
