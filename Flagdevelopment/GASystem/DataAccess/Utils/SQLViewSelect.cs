using System;
using System.Collections;
using System.Web;
using GASystem.DataAccess.Security;
using GASystem.DataModel;

namespace GASystem.DataAccess.Utils
{
	/// <summary>
	/// Summary description for SQLViewSelect.
	/// </summary>
	public class SQLViewSelect
	{
		

		private ArrayList m_columns = new ArrayList();
		private string m_from;
        private string m_where;
        private string m_join;
		//private GADataRecord m_currentDateRecord;
		private int TableAliasId = 0;
		

		private static string _selectSqlFullDetailsMembers = @"SELECT {0} FROM {1} WHERE 1 = 1 ";
		
//        private static string _selectSqlFullDetailsWithinMembers = @"SELECT {4}
//										                        FROM {5} INNER JOIN
//										                        GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId 
//                                                                WHERE (dbo.GASuperClass.MemberClass = '{0}' )  
//                                                                AND (GASuperClass.ReadRoles = '{3}-{2}'
//                                                                OR GASuperClass.UpdateRoles = '{3}-{2}'
//                                                                OR GASuperClass.CreateRoles = '{3}-{2}'
//                                                                OR GASuperClass.DeleteRoles = '{3}-{2}'
//                                                                OR GASuperClass.TextFree1 = '{3}-{2}'
//                                                                OR GASuperClass.TextFree2 = '{3}-{2}')";
        //(GASuperClass.path like '%{3}-{2}/%') has been replaced in the statement above";
		
//        private static string _selectSqlFullDetailsWithinMembersSecure = @"SELECT {4}
//										                                FROM {5} INNER JOIN
//										                                GASuperClass ON {0}.{1}RowId = GASuperClass.MemberClassRowId 
//                                                                        WHERE (dbo.GASuperClass.MemberClass = '{0}')  
//                                                                        AND 
//                                                                        (
//                                                                            GASuperClass.ReadRoles = '{3}-{2}'
//                                                                            OR GASuperClass.UpdateRoles = '{3}-{2}'
//                                                                            OR GASuperClass.CreateRoles = '{3}-{2}'
//                                                                            OR GASuperClass.DeleteRoles = '{3}-{2}'
//                                                                            OR GASuperClass.TextFree1 = '{3}-{2}'
//                                                                            OR GASuperClass.TextFree2 = '{3}-{2}'
//                                                                        )";
        //(gasuperclass.path like '%{3}-{2}/%') has been replaced in the statement above";
		
		//Join a given query against the AccessControl, and only return rows where the is a match
		//This will only return rows where the user has roles
		/*	private static string _selectAllSecure = @"SELECT * FROM ({0}) MyQuery
								 LEFT OUTER JOIN GAAccessControl ON GAAccessControl.GARowId = MyQuery.{2}RowId
								 WHERE GAAccessControl.DNNRole IN ({3})";
	*/
		private static string _selectAccessableRowId = @" SELECT * FROM ({0}) MyQuery 
                                                        INNER JOIN (SELECT DISTINCT dbo.GASuperClass.MemberClassRowId as AccessableRowId
														FROM dbo.GASuperClass inner join dbo.GAAccessControl on 
                                                        (
                                                            dbo.GASuperClass.Path LIKE dbo.GAAccessControl.Path + '%' 
                                                            OR (MemberClass=GAClass 
                                                            AND MemberClassRowId=GARowId))
														    WHERE 
                                                            (
                                                                dbo.GASuperClass.MemberClass = N'{1}' 
                                                                AND 
                                                                (
                                                                    dbo.GASuperClass.Path LIKE '%{3}%'
                                                                    /*
                                                                        GASuperClass.ReadRoles = '{3}'
                                                                        OR GASuperClass.UpdateRoles = '{3}'
                                                                        OR GASuperClass.CreateRoles = '{3}'
                                                                        OR GASuperClass.DeleteRoles = '{3}'
                                                                        OR GASuperClass.TextFree1 = '{3}'
                                                                        OR GASuperClass.TextFree2 = '{3}'
                                                                    */
                                                                )
														        AND dbo.GAAccessControl.DnnRole IN ({4})
                                                            ) 
														) 
                                                        AccessQuery ON AccessQuery.AccessableRowId=MyQuery.{2}RowId ";


		public SQLViewSelect()
		{
			//
			// TODO: Add constructor logic here
			//
		}



		private void PopulateUsingFieldDescription(string ViewName) 
		{
			//m_currentDateRecord = DataRecord;
			GASystem.AppUtils.FieldDescription[] fds = GASystem.AppUtils.FieldDefintion.GetFieldDescriptions(ViewName);
	
			m_from = ViewName;
			foreach (GASystem.AppUtils.FieldDescription fd in fds) 
			{
					
//				if (fd.FieldId != DataClass.ToString().Substring(2) + "RowId")   //TODO bugfix, some tables has rowid column defined in fd. ignore here since it is added earlier
//				{
					if (fd.ControlType.ToUpper() == "FILECONTENT")
						m_columns.Add("null as " + fd.FieldId);
                    else if (fd.ControlType.ToUpper() == "DROPDOWNLIST" || fd.ControlType.ToUpper() == "POSTBACKDROPDOWNLIST"
                        // Tor 20171215 added DROPDOWNLISTMULTIPLE
                        //|| fd.ControlType.ToUpper() == "DROPDOWNLISTMULTIPLE"
                        )
						m_columns.Add(GenerateColumnForLists(fd, fd.TableId) + " as " + fd.FieldId);
                    // added "LOOKUPFIELDMULTIPLE" and "LOOKUPFIELDVIEW"to else if ((fd.ControlType.ToUpper() == "LOOKUPFIELD" || fd.ControlType.ToUpper() == "LOOKUPFIELDEDIT" || fd.ControlType.ToUpper() == "RESPONSIBLE") && fd.LookupTable != string.Empty)
                    else if ((fd.ControlType.ToUpper() == "LOOKUPFIELD" || fd.ControlType.ToUpper() == "LOOKUPFIELDMULTIPLE" || fd.ControlType.ToUpper() == "LOOKUPFIELDVIEW" || fd.ControlType.ToUpper() == "LOOKUPFIELDEDIT" || fd.ControlType.ToUpper() == "RESPONSIBLE") && fd.LookupTable != string.Empty)
						m_columns.Add(GenerateCombinedColumns(fd, fd.TableId) + " as " + fd.FieldId);	
					else 
					{
						m_columns.Add(fd.TableId + "." + fd.FieldId);
						
						//m_from += GenerateJoinStatement(fd);
					}						

//				}
			}
		}

		/// <summary>
		/// Genereate SQL for a singel DataRecord
		/// </summary>
		/// <param name="DataRecord">DataRecord</param>
		/// <returns>sql string</returns>
//		public string GenerateSQL(GADataRecord DataRecord) 
//		{
//			m_columns.Clear();
//			m_from = "";
//			m_where = "";
//			m_join = "";
//			m_columns.Add(DataRecord.DataClass.ToString() + "." + DataRecord.DataClass.ToString().Substring(2) + "RowId");
//			PopulateUsingFieldDescription(DataRecord.DataClass);
//			
//			string selectSql = ArrayListToString(m_columns, ",");
//			m_where = DataRecord.DataClass.ToString() + "." + DataRecord.DataClass.ToString().Substring(2) + "RowId" + " = " + DataRecord.RowId.ToString();
//			string sql = "select " + selectSql + " from " +  m_from + " where " +  m_where;
//			return sql;
//		}

		public string WithSecurity(string sql, GADataClass DataClass, String Roles)
		{
			return sql;
		}

		/// <summary>
		/// Generate SQL for getting all records of type GADataClass with in owner datarecord
		/// </summary>
		/// <param name="MemberDataClass">Record types to get</param>
		/// <param name="OwnerRowId">Owner rowid</param>
		/// <param name="OwnerDataClass">Owner GADataClass type</param>
		/// <returns>sql string</returns>
//		public string GenerateSQLAllWithin(GADataClass MemberDataClass, int OwnerRowId, GADataClass OwnerDataClass) 
//		{
//			m_columns.Clear();
//			m_from = "";
//			m_where = "";
//			m_join = "";
//			m_columns.Add(MemberDataClass.ToString() + "." + MemberDataClass.ToString().Substring(2) + "RowId");
//			PopulateUsingFieldDescription(MemberDataClass);
//			
//			string selectSql = ArrayListToString(m_columns, ",");
//			//m_where = DataRecord.DataClass.ToString().Substring(2) + "RowId" + " = " + DataRecord.RowId.ToString();
//			//string sql = "select " + selectSql + " from " +  m_from + " where " +  m_where;
//			string sql = _selectSqlFullDetailsWithinMembers;
//			sql = String.Format(sql, new Object[] {MemberDataClass, MemberDataClass.ToString().Substring(2), OwnerRowId, OwnerDataClass, selectSql, m_from  });
//			
//			//String tstSql = AppendSecurity2(sql, MemberDataClass, OwnerDataClass.ToString()+"-"+OwnerRowId.ToString(), GASecurityDb.GetUserRolesAsSqlArray());
//			HttpContext.Current.Trace.Warn(sql);
//			
//			return sql;
//		}

		
		/// <summary>
		/// Generate SQL for getting all records of type GADataClass owned by a DataRecord
		/// </summary>
		/// <param name="MemberDataClass">Record types to get</param>
		/// <param name="OwnerRowId">Owner rowid</param>
		/// <param name="OwnerDataClass">Owner GADataClass type</param>
		/// <returns>sql string</returns>
//		public string GenerateSQL(GADataClass MemberDataClass, int OwnerRowId, GADataClass OwnerDataClass) 
//		{
//			m_columns.Clear();
//			m_from = "";
//			m_where = "";
//			m_join = "";
//			m_columns.Add(MemberDataClass.ToString() + "." + MemberDataClass.ToString().Substring(2) + "RowId");
//			PopulateUsingFieldDescription(MemberDataClass);
//			
//			string selectSql = ArrayListToString(m_columns, ",");
//			//m_where = DataRecord.DataClass.ToString().Substring(2) + "RowId" + " = " + DataRecord.RowId.ToString();
//			//string sql = "select " + selectSql + " from " +  m_from + " where " +  m_where;
//			string sql = _selectSqlFullDetailsMembers;
//			sql = String.Format(sql, new Object[] {MemberDataClass, MemberDataClass.ToString().Substring(2), OwnerRowId, OwnerDataClass, selectSql, m_from  });
//			return sql;
//		}

		/// <summary>
		/// Generate SQL for getting all records of type GADataClass
		/// </summary>
		/// <param name="MemberDataClass">Record types to get</param>
		/// <returns>sql string</returns>
		public string GenerateSQL(string ViewName) 
		{
			m_columns.Clear();
			m_from = "";
			m_where = "";
			m_join = "";
			//m_columns.Add(MemberDataClass.ToString() + "." + MemberDataClass.ToString().Substring(2) + "RowId");
			PopulateUsingFieldDescription(ViewName);
			string selectSql = ArrayListToString(m_columns, ",");
			//m_where = DataRecord.DataClass.ToString().Substring(2) + "RowId" + " = " + DataRecord.RowId.ToString();
			//string sql = "select " + selectSql + " from " +  m_from + " where " +  m_where;
			string sql = string.Format(_selectSqlFullDetailsMembers, selectSql, m_from);
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
														, FieldDescriptionInfo.LookupTable.Substring(2) + "RowId"
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
            return "lists" + FieldDescriptionInfo.ColumnOrder; // TableAliasId;
		}

		/*	public static String AppendSecurity(string sql, GADataClass DataClass, string GroupArray)
			{
				return string.Format(_selectAllSecure, sql,  DataClass.ToString(), DataClass.ToString().Substring(2), GroupArray.ToString());
			}
	*/
		

		public static String AppendSecurity2(string sql, GADataClass DataClass, string Path,  string GroupArray)
		{
			
			return string.Format(_selectAccessableRowId, sql,  DataClass.ToString(), DataClass.ToString().Substring(2), Path, GroupArray.ToString());
		}

	}
}
