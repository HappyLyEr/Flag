using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using GASystem.AppUtils;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for SuperClassDb.
	/// </summary>
	public class SuperClassDb
	{
		//private static SqlConnection myConnection;
		private static string _superclassSelectSql = @"SELECT * FROM GASuperClass ";
		private static string _superclassSelectCountSql = @"SELECT count(*) FROM GASuperClass ";
		private static string _superclassLinksSelectSql = @"SELECT * FROM GASuperClassLinks ";
		
		public SuperClassDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public SuperClassDS GetSuperClassByMember(int MemberRowId, GADataClass MemberDataClass)
		{
			return GetSuperClassByMember(MemberRowId, MemberDataClass, null);
		}

		public SuperClassDS GetSuperClassByMember(int MemberRowId, GADataClass MemberDataClass, GADataTransaction Transaction)
		{
            string appendSql = String.Format(" WHERE MemberClassRowId={0} AND MemberClass='{1}' ", MemberRowId, MemberDataClass.ToString());
            string ownersSql = _superclassSelectSql + appendSql;
            
            DataCache.ValidateCache(DataCache.DataCacheType.SuperClassByMember);

            SuperClassDS cachedObject = (SuperClassDS)DataCache.GetCachedObject(DataCache.DataCacheType.SuperClassByMember, appendSql);
            if (cachedObject != null)
                return cachedObject;

            SuperClassDS superClassData = new SuperClassDS();
            SqlConnection myConnection = DataUtils.GetConnection(Transaction);

            SqlDataAdapter da = new SqlDataAdapter(ownersSql, myConnection);
			if (Transaction != null)
				da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.Fill(superClassData, "GASuperClass");
            DataCache.AddCachedObject(DataCache.DataCacheType.SuperClassByMember, appendSql, superClassData); 
            return superClassData;
		}

		public static SuperClassDS GetSuperClassByOwner(int OwnerRowId, string OwnerDataClass)
		{
            string appendSql = String.Format(" WHERE OwnerClassRowId={0} AND OwnerClass='{1}' ", OwnerRowId, OwnerDataClass);
            string ownersSql = _superclassSelectSql + appendSql;

            DataCache.ValidateCache(DataCache.DataCacheType.SuperClassByMember);

            SuperClassDS cachedObject = (SuperClassDS)DataCache.GetCachedObject(DataCache.DataCacheType.SuperClassByOwner, appendSql);
            if (cachedObject != null)
                return cachedObject;

            SuperClassDS superClassData = new SuperClassDS();
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

			SqlDataAdapter da = new SqlDataAdapter(ownersSql, myConnection);
			da.Fill(superClassData, "GASuperClass");
            DataCache.AddCachedObject(DataCache.DataCacheType.SuperClassByOwner, appendSql, superClassData);
            return superClassData;
		}

        public static SuperClassDS GetSuperClassByOwner(int OwnerRowId, string OwnerDataClass, GADataTransaction Transaction)
        {
            string appendSql = String.Format(" WHERE OwnerClassRowId={0} AND OwnerClass='{1}' ", OwnerRowId, OwnerDataClass);
            string ownersSql = _superclassSelectSql + appendSql;

            DataCache.ValidateCache(DataCache.DataCacheType.SuperClassByMember);

            SuperClassDS cachedObject = (SuperClassDS)DataCache.GetCachedObject(DataCache.DataCacheType.SuperClassByOwner, appendSql);
            if (cachedObject != null)
                return cachedObject;

            SuperClassDS superClassData = new SuperClassDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

            SqlDataAdapter da = new SqlDataAdapter(ownersSql, myConnection);
            if (Transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)Transaction.Transaction;
            da.Fill(superClassData, "GASuperClass");
            DataCache.AddCachedObject(DataCache.DataCacheType.SuperClassByOwner, appendSql, superClassData);
            return superClassData;
        }

		public static SuperClassDS UpdateSuperClass(SuperClassDS SuperclassSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_superclassSelectSql, myConnection);

            if (Transaction != null)
            {
                da.SelectCommand.Transaction = (SqlTransaction)Transaction.Transaction;

            }
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
            
            
			da.Update(SuperclassSet, "GASuperClass");
			return SuperclassSet;
		}

		public static void UpdateMembersOwnerPath(string OldOwnerPath, string NewOwnerPath, GADataTransaction Transaction)
		{
			string sql =  "UPDATE GASuperClass SET Path = REPLACE(path, '<oldpath>', '<newpath>') WHERE Path LIKE '<oldpath>'";
			sql = sql.Replace("<oldpath>", OldOwnerPath);
			sql = sql.Replace("<newpath>", NewOwnerPath);

			SqlConnection myConnection = DataUtils.GetConnection(Transaction);

			SqlCommand cmd = new SqlCommand(sql, myConnection);
			if (Transaction != null)
				cmd.Transaction = (SqlTransaction) Transaction.Transaction;


			cmd.ExecuteNonQuery();

			
		}

        // Tor 201611 Security 20161003 add method to update ChangedBy and DateChanged for changed memberclass and memberclassRowId record
        public static void UpdateChangedByAndDateChanged(DataRow row, GADataClass dataClass, int personnelRowId, GADataTransaction Transaction)
        {
            string rowColumnName=dataClass.ToString().Substring(2)+"RowId";
            object rowId = row[rowColumnName];
            if (rowId != DBNull.Value)
            {
                SqlConnection myConnection;
                SqlCommand cmd;
                int RowID = (int)rowId;
                // Tor 20170323 changed sql sentence to use DateChanged=GETUTCDATE() instead of formatting a date and time string
                string sql = "UPDATE GASuperClass SET ChangedBy={0},DateChanged=GETUTCDATE() WHERE MemberClass='{1}' and MemberClassRowId={2}";
//                string sql = "UPDATE GASuperClass SET ChangedBy={0},DateChanged='{1}' WHERE MemberClass='{2}' and MemberClassRowId={3}";
                string mySql = String.Format(sql, personnelRowId.ToString()
//                    ,System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    ,dataClass.ToString(),RowID.ToString());
                if (Transaction == null)
                {
                    myConnection = new SqlConnection(DataUtils.getConnectionString());
                    myConnection.Open();
                    cmd = new SqlCommand(mySql, myConnection);
                    cmd.ExecuteNonQuery();
                    myConnection.Close();
                }
                else
                {
                    myConnection = DataUtils.GetConnection(Transaction);
                    cmd = new SqlCommand(mySql, myConnection);
                    if (Transaction != null)
                        cmd.Transaction = (SqlTransaction)Transaction.Transaction;
                    cmd.ExecuteNonQuery();
                }
            }
        }

		public static int GetNumberOfMembers(int OwnerRowId, string OwnerDataClass)
		{
			SuperClassDS superClassData = new SuperClassDS();
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

			string sql = String.Format(_superclassSelectCountSql + " WHERE OwnerClassRowId={0} AND OwnerClass='{1}' ", OwnerRowId, OwnerDataClass);
			SqlCommand command = new SqlCommand(sql, myConnection);
			myConnection.Open();
			int numberOfRows = (int)command.ExecuteScalar();
			myConnection.Close();
			
			//SqlDataAdapter da = new SqlDataAdapter(ownersSql, myConnection);
			//da.Fill(superClassData, "GASuperClass");
			return numberOfRows;
		}


		/// <summary>
		/// This command will take care of both delete, insert and update
		/// delete: every row in the dataset that is marked "deleted"
		/// insert: every row marked "new"
		/// update: every touched row (marked "updated")
		/// </summary>
		/// <param name="superclassSet"></param>
		public static SuperClassDS UpdateSuperClass(SuperClassDS SuperclassSet)
		{
			return UpdateSuperClass(SuperclassSet, null);
		}


	
		public static SuperClassLinksDS GetSuperClassLinksByOwner(string OwnerDataClass)
		{
            DataCache.ValidateCache(DataCache.DataCacheType.SuperClassLinksByOwner);

            SuperClassLinksDS cachedObject = (SuperClassLinksDS)DataCache.GetCachedObject(DataCache.DataCacheType.SuperClassLinksByOwner, OwnerDataClass);
            if (cachedObject != null)
                return cachedObject;

			SuperClassLinksDS superClassLinksData = new SuperClassLinksDS();
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			string membersSql = String.Format(_superclassLinksSelectSql + " WHERE OwnerClass='{0}' ORDER BY [SortOrder] ", OwnerDataClass);
			SqlDataAdapter da = new SqlDataAdapter(membersSql, myConnection);
			try
			{
				da.Fill(superClassLinksData, "GASuperClassLinks");		
			}
			catch (Exception ex)
			{
				Logger.LogError("GetSuperClassLinksByOwner",ex.Message, ex);
			}
            DataCache.AddCachedObject(DataCache.DataCacheType.SuperClassLinksByOwner, OwnerDataClass, superClassLinksData);
            return superClassLinksData;
		}

        // Tor 20151019 New method to return member Class array
        /// <summary>
        /// Return all memberclasses under ownerclass where memberclass is a use as home class and memberclass is not a class defined with a sql query in GAClass
        /// </summary>
        /// <param name="OwnerClass"></param>
        /// <returns>array</returns>
        
        public static ArrayList GetAllMemberClassesBelowOwnerClass(string OwnerDataClass)
        {
            ArrayList dataClassList = new ArrayList();
            if (OwnerDataClass == null) return dataClassList;
//            string _GetAllMemberClassesBelowOwnerClassSql = @"WITH MemberClasses(OwnerClass, MemberClass) AS 
//(SELECT s.OwnerClass, s.MemberClass FROM GASuperClassLinks AS s
//WHERE s.OwnerClass='{0}' and (s.SwitchFree2 /*Hide tab*/is null or s.SwitchFree2=0)
//UNION ALL SELECT s1.OwnerClass, s1.MemberClass FROM GASuperClassLinks AS s1 INNER JOIN MemberClasses d ON s1.OwnerClass=d.MemberClass
//WHERE (s1.SwitchFree2 is null or s1.SwitchFree2=0) )
//SELECT /*OwnerClass,*/ distinct MemberClass, l.GAListDescription FROM MemberClasses m
//inner join GAClass c on c.Class=m.MemberClass and (c.IsView is null or c.IsView=0) and c.SwitchFree2=1 /*use as home */ and (c.nTextFree2 /* select sentence for sql views*/ is null or LEN(CAST(c.nTextFree2 as nvarchar(max)))<5 /* class is not 'virtual' class */)
//inner join GALists l on l.GAListCategory='GEN' and l.GAListValue=m.MemberClass
//ORDER BY l.GAListDescription, m.MemberClass
//";

            // Tor 20160809 changed query to locate many to many classes

            string _GetAllMemberClassesBelowOwnerClassSql = @"WITH MemberClasses(OwnerClass, MemberClass) AS 
(SELECT s.OwnerClass, case when (c.ManyToManyField is not null and c.ManyToManyField like '%RowId') then cast('GA'+REPLACE(c.ManyToManyField,'RowId','') as nvarchar(100)) else CAST( s.MemberClass as nvarchar(100)) end as MemberClass
FROM GASuperClassLinks AS s 
inner join GAClass c on c.Class=s.MemberClass 
WHERE s.OwnerClass='{0}' and (s.SwitchFree2 /*Hide tab*/is null or s.SwitchFree2=0)
UNION ALL SELECT s1.OwnerClass
, case when (c1.ManyToManyField is not null and c1.ManyToManyField like '%RowId') then cast('GA'+REPLACE(c1.ManyToManyField,'RowId','') as nvarchar(100)) else CAST( s1.MemberClass as nvarchar(100)) end as MemberClass
FROM GASuperClassLinks AS s1 
INNER JOIN MemberClasses d ON s1.OwnerClass=d.MemberClass
inner join GAClass c1 on c1.Class=s1.MemberClass 
WHERE (s1.SwitchFree2 is null or s1.SwitchFree2=0) 
)
SELECT /*OwnerClass,*/ distinct MemberClass, l.GAListDescription FROM MemberClasses m
inner join GAClass c on c.Class=m.MemberClass and (c.IsView is null or c.IsView=0) and c.SwitchFree2=1 /*use as home */ and (c.nTextFree2 /* select sentence for sql views*/ is null or LEN(CAST(c.nTextFree2 as nvarchar(max)))<5 /* class is not 'virtual' class */)
inner join GALists l on l.GAListCategory='GEN' and l.GAListValue=m.MemberClass
ORDER BY l.GAListDescription, m.MemberClass
";
            string sql = string.Format(_GetAllMemberClassesBelowOwnerClassSql, OwnerDataClass.ToString());
            IDataReader reader = null;
            reader = DataUtils.executeSelect(sql);

            while (reader.Read())
            {
                dataClassList.Add(reader.GetString(0));
            }
            reader.Close();
            return dataClassList;
        }

        /// <summary>
		/// Return owner of given class
		/// </summary>
		/// <param name="MemberDataClass"></param>
		/// <returns></returns>
		public static SuperClassLinksDS GetSuperClassLinksByMember(GADataClass MemberDataClass)
		{
            DataCache.ValidateCache(DataCache.DataCacheType.SuperClassLinksByMember);

            SuperClassLinksDS cachedObject = (SuperClassLinksDS)DataCache.GetCachedObject(DataCache.DataCacheType.SuperClassLinksByMember, MemberDataClass.ToString());
            if (cachedObject != null)
                return cachedObject;

            SuperClassLinksDS superClassLinksData = new SuperClassLinksDS();
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			string membersSql = String.Format(_superclassLinksSelectSql + " WHERE MemberClass='{0}' ORDER BY [SortOrder] ", MemberDataClass);
			//Logger.LogInfo("GetSuperClassLinksByOwner: ",membersSql);
			SqlDataAdapter da = new SqlDataAdapter(membersSql, myConnection);
			da.Fill(superClassLinksData, "GASuperClassLinks");
            DataCache.AddCachedObject(DataCache.DataCacheType.SuperClassLinksByMember, MemberDataClass.ToString(), superClassLinksData);
			return superClassLinksData;
		}

        // Tor 20140320 New method to return one SuperClassLinks record
        /// <summary>
        /// Return owner and member SuperClassLinksRecord
        /// </summary>
        /// <param name="OwnerDataClass"></param>
        /// <param name="MemberDataClass"></param>
        /// <returns></returns>
        public static SuperClassLinksDS GetSuperClassLinksByOwnerAndMember(GADataClass OwnerDataClass,GADataClass MemberDataClass)
        {
            SuperClassLinksDS superClassLinksData = new SuperClassLinksDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            string ownerMemberSql = String.Format(_superclassLinksSelectSql + " WHERE OwnerClass='{0}' and MemberClass='{1}' ",OwnerDataClass, MemberDataClass);
            //Logger.LogInfo("GetSuperClassLinksByOwner: ",membersSql);
            SqlDataAdapter da = new SqlDataAdapter(ownerMemberSql, myConnection);
            da.Fill(superClassLinksData, "GASuperClassLinks");
            return superClassLinksData;
        }

        private void findOneLevel(System.Collections.ArrayList MemberRowIds, string OwnerClass, int RowId, string TargetMemberClass, SqlConnection conn) 
		{
			string sql = "select OwnerClass, OwnerClassRowId, MemberClass, MemberClassRowId from gasuperclass ";
			sql = sql + "where ownerclass = '" + OwnerClass + "'";
			sql = sql + " and ownerclassrowid = " + RowId;
			SqlCommand myCommand = new SqlCommand(sql, conn);
			SqlDataReader myReader = myCommand.ExecuteReader();
			
			System.Collections.ArrayList nextLevel = new System.Collections.ArrayList();

			if (myReader.HasRows) 
			{
				while(myReader.Read()) 
				{
					if (!myReader.IsDBNull(2)) 
					{
						if (myReader.GetString(2) == TargetMemberClass) 
						{
							MemberRowIds.Add(myReader.GetInt32(3));
						} 
						else 
						{
							
							//findOneLevel(MemberRowIds, (string)myReader.GetString(2), (int)myReader.GetInt32(3), TargetMemberClass, conn);
							nextLevel.Add(new StructMemberRecord((string)myReader.GetString(2), (int)myReader.GetInt32(3)));
						}
					}
				} // end while
			}
			myReader.Close();
			//run next level;
			foreach(StructMemberRecord memberRecord in nextLevel) 
			{
				findOneLevel(MemberRowIds, memberRecord.ClassName, memberRecord.RowId, TargetMemberClass, conn);
			}

			return;
		}

		public int[] FindAllMemberRowIds(string OwnerClass, int RowId, string TargetMemberClass) 
		{
			
			
			int [] temp;
			System.Collections.ArrayList memberRowIds = new System.Collections.ArrayList();
			//memberRowIds.Add(4);
			SqlConnection conn = new SqlConnection(DataUtils.getConnectionString());
			conn.Open();
			findOneLevel(memberRowIds, OwnerClass, RowId, TargetMemberClass, conn);
			conn.Close();
			temp = (int[])memberRowIds.ToArray(typeof(System.Int32));
			return temp;
		}

        public static SuperClassDS GetSuperClassByOwnerAndMember(GADataRecord Owner, GADataRecord Member, GADataTransaction transaction) 
		{
			SuperClassDS ds = new SuperClassDS();
			string sql = String.Format(_superclassSelectSql + " WHERE MemberClassRowId={0} AND MemberClass='{1}'  AND OwnerClassRowId={2} AND OwnerClass='{3}' ", 
				Member.RowId.ToString(), Member.DataClass.ToString(), Owner.RowId.ToString(), Owner.DataClass.ToString());
			SqlConnection myConnection = DataUtils.GetConnection(transaction);
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;
			da.Fill(ds, "GASuperClass");	

			return ds;
		}

        public static SuperClassDS GetSuperClassByOwnerAndMember(GADataRecord Owner, GADataRecord Member)
        {
            return GetSuperClassByOwnerAndMember(Owner, Member, null);
        }

        public static SuperClassDS GetSuperClassByOwnerAndMemberClass(GADataRecord Owner, GADataClass MemberClass, GADataTransaction transaction)
        {
            SuperClassDS ds = new SuperClassDS();
            string sql = String.Format(_superclassSelectSql + " WHERE MemberClass='{0}'  AND OwnerClassRowId={1} AND OwnerClass='{2}' ",
                MemberClass.ToString(), Owner.RowId.ToString(), Owner.DataClass.ToString());
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;
            da.Fill(ds, "GASuperClass");

            return ds;
        }
		
	}

	public struct StructMemberRecord
	{
		public int RowId;
		public string ClassName;

		public StructMemberRecord(string NewClassName, int NewRowId) 
		{
			RowId = NewRowId;
			ClassName = NewClassName;
		}
	}
}
