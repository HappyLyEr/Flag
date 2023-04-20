using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;
using GASystem.DataAccess.Utils;
using System.Runtime.InteropServices;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for ListsDb.
	/// </summary>
	public class ListsDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GALists";
		
		public ListsDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ListsDS GetAllListss()
		{

			ListsDS ListsData = new ListsDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(ListsData, "GALists");
		
			return ListsData;
		}

		/// <summary>
		/// Get list row for a given rowid
		/// </summary>
		/// <param name="ListsRowId"></param>
		/// <returns></returns>
		public static ListsDS GetListsByListsRowId(int ListsRowId)
		{
            string langListKey = GASystem.AppUtils.Localization.GetCurrentLocalizedListKey();

            DataCache.ValidateCache(DataCache.DataCacheType.ListsByListsRowId);

            ListsDS cachedObject = (ListsDS)DataCache.GetCachedObject(DataCache.DataCacheType.ListsByListsRowId, ListsRowId.ToString(), langListKey);
            if (cachedObject != null)
                return cachedObject;
            
            String appendSql = " WHERE ListsRowId="+ListsRowId;
			ListsDS ListsData = new ListsDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(ListsData, "GALists");

            GASystem.AppUtils.Localization.SetLocalizedListDescription(ListsData.GALists);

            DataCache.AddCachedObject(DataCache.DataCacheType.ListsByListsRowId, ListsRowId.ToString(), ListsData);
            return ListsData;
		}

		/// <summary>
		/// get list rows for by an array rowids
		/// </summary>
		/// <param name="ListsRowIds"></param>
		/// <returns></returns>
		public static ListsDS GetListsByListsRowIds(int[] ListsRowIds)
		{
			if (ListsRowIds.Length == 0)
				return new ListsDS();  //ids passed return empty dataset
			
			//build a comma seperated string of listrow ids.  TODO move this to a general utility function.
			System.Text.StringBuilder sb = new System.Text.StringBuilder(ListsRowIds[0].ToString());
			for (int t=1; t<ListsRowIds.Length; t++) 
			{
				sb.Append(", ");
				sb.Append(ListsRowIds[t]);
			}


			String appendSql = " WHERE ListsRowId in (" + sb.ToString() +  ")";
			ListsDS ListsData = new ListsDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(ListsData, "GALists");
			return ListsData;
		}

		public static int GetListsRowIdByCategoryAndKey(int Category, string Key) 
		{
//			String appendSql = " WHERE GAListValue = '" + Key + "' and ListCategoryRowId = " + Category;
//
//			ListsDS ListsData = new ListsDS();		
//			myConnection = new SqlConnection(DataUtils.getConnectionString());
//		
//			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
//			da.Fill(ListsData, "GALists");
//			if (ListsData.GALists.Rows.Count == 0)
//				return -1;  //TODO throw exception
//			return ListsData.GALists[0].ListsRowId;

			//String appendSql = " WHERE GAListValue = '" + Key + "' and ListCategoryRowId = " + Category;
			string sql = "select ListsRowId from galists where  GAListValue = '" + Key + "' and ListCategoryRowId = " + Category;

			//ListsDS ListsData = new ListsDS();		
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			int rowid = -1;
			try 
			{
				myConnection.Open();
				SqlCommand myCommand = new SqlCommand(sql, myConnection);
				rowid = (int)myCommand.ExecuteScalar();
			} 
			catch 
			{}
			finally
			{
				myConnection.Close();
			}
				return rowid;
			
		}

        public static string GetStringColumnValueByListsRowId(int RowId, string ColumnName)
        {
            string sql = "select " + ColumnName + " from GALists where ListsRowId=" + RowId;
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            string columnValue = string.Empty;
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(sql, myConnection);
                columnValue = (string)myCommand.ExecuteScalar();
            }
            catch
            { }
            finally
            {
                myConnection.Close();
            }
            return columnValue;
        }

        public static int GetIntColumnValueByListsRowId(int RowId, string ColumnName)
        {
            string sql = "select " + ColumnName + " from GALists where ListsRowId=" + RowId;
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            int columnValue = -1;
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(sql, myConnection);
                columnValue = (int)myCommand.ExecuteScalar();
            }
            catch
            { }
            finally
            {
                myConnection.Close();
            }
            return columnValue;
        }

        public ListsDS GetListsRowIdByCategory(int Category)
        {
            return GetListsRowIdByCategoryLocalizable(Category, true);
        }

        //july23-2020 localize the GAList with the current language
        public ListsDS GetListsRowIdByCategory(int Category, [Optional, DefaultParameterValue(true)]bool localizeIt)
        {
            return GetListsRowIdByCategoryLocalizable(Category, localizeIt);
        }

        private ListsDS GetListsRowIdByCategoryLocalizable(int Category, bool localizeIt)
        {
            string langListKey = GASystem.AppUtils.Localization.GetCurrentLocalizedListKey();

            DataCache.ValidateCache(DataCache.DataCacheType.ListsRowIdByCategory);

            ListsDS cachedObject = (ListsDS)DataCache.GetCachedObject(DataCache.DataCacheType.ListsRowIdByCategory,
                Category.ToString(), langListKey);
            if (cachedObject != null)
                return cachedObject;

            String appendSql = " WHERE ListCategoryRowId = " + Category;
            string sortSql = " order by sort1, galistvalue";
            ListsDS ListsData = new ListsDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

            SqlDataAdapter da = new SqlDataAdapter(_selectSql + appendSql + sortSql, myConnection);

            da.Fill(ListsData, "GALists");
            if (myConnection.State != ConnectionState.Closed)
                myConnection.Close();

            if (localizeIt)
            {
                GASystem.AppUtils.Localization.SetLocalizedListDescription(ListsData.GALists);
            }

            //			if (ListsData.GALists.Rows.Count == 0)
            //				return -1;  //TODO throw exception
            //			return ListsData.GALists[0].ListsRowId;
            DataCache.AddCachedObject(DataCache.DataCacheType.ListsRowIdByCategory, Category.ToString(), ListsData, langListKey);
            return ListsData;
        }

        //ToDo Tor new get by category and filter
        public ListsDS GetListsRowIdByCategoryAndFilter(int Category, string filter)
        {
            // ??? Cannot cache when filtering ????
            //DataCache.ValidateCache(DataCache.DataCacheType.ListsRowIdByCategory);

            //ListsDS cachedObject = (ListsDS)DataCache.GetCachedObject(DataCache.DataCacheType.ListsRowIdByCategory, Category.ToString());
            //if (cachedObject != null)
            //    return cachedObject;

            //SELECT * FROM GALists
            //inner join (
            //select distinct e.RoleListsRowId from GAEmployment e
            //inner join GASuperClass s on s.MemberClass='GAEmployment' and s.MemberClassRowId=e.EmploymentRowId
            //where (OwnerClass='GALocation' and OwnerClassRowId=9000002
            //or OwnerClass='GACrew' and OwnerClassRowId=4005
            //or OwnerClass='GAProject' and OwnerClassRowId=12000002
            //or OwnerClass='GACompany' and OwnerClassRowId=4054)
            //and e.FromDate<=GETDATE() and (e.ToDate is null or e.ToDate>=GETDATE())

            //) a on ListsRowId=a.RoleListsRowId
            //WHERE ListCategoryRowId = (select ListCategoryRowId from GAListCategory where GAListCategory= 'er')
            //order by sort1, galistvalue


            String appendSql = " WHERE ListCategoryRowId = " + Category;
            string sortSql = " order by sort1, galistvalue";
            ListsDS ListsData = new ListsDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
// 20140123 Tor add filter to sql statement
            //SqlDataAdapter da = new SqlDataAdapter(_selectSql + appendSql + sortSql, myConnection);
            SqlDataAdapter da = new SqlDataAdapter(_selectSql + filter + appendSql + sortSql, myConnection);

            da.Fill(ListsData, "GALists");
            if (myConnection.State != ConnectionState.Closed)
                myConnection.Close();

            //			if (ListsData.GALists.Rows.Count == 0)
            //				return -1;  //TODO throw exception
            //			return ListsData.GALists[0].ListsRowId;
            //DataCache.AddCachedObject(DataCache.DataCacheType.ListsRowIdByCategory, Category.ToString(), ListsData);
            return ListsData;
        }
        public static ListsDS GetListsByClass(GADataClass DataClass) 
		{
            DataCache.ValidateCache(DataCache.DataCacheType.ListsByClass);

            ListsDS cachedObject = (ListsDS)DataCache.GetCachedObject(DataCache.DataCacheType.ListsByClass, DataClass.ToString());
            if (cachedObject != null)
                return cachedObject;
            
            String appendSql = " WHERE Class='"+DataClass.ToString() + "'";
			ListsDS ListsData = new ListsDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(ListsData, "GALists");
            DataCache.AddCachedObject(DataCache.DataCacheType.ListsByClass, DataClass.ToString(), ListsData);
            return ListsData;
		}

		public static ListsDS GetListssByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
//			ListsDS ListsData = new ListsDS();	
//			//string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GALists, OwnerRowId, OwnerDataClass);
//			
//			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
//			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
//			da.Fill(ListsData, GADataClass.GALists.ToString());
//			return ListsData;
			//lists is a top level element return all record
			return GetAllListss();
		}


        public static string GetListValueByRowId(int rowId, GASystem.DataAccess.GADataTransaction transaction)
        {
            String sql = "SELECT GAListValue FROM GALists WHERE ListsRowId= @rowid";
            ListsDS ListsData = new ListsDS();

            SqlConnection myConnection;
            if (transaction == null)
                myConnection = new SqlConnection(DataUtils.getConnectionString());
            else
                myConnection = (SqlConnection)transaction.Connection;

            SqlCommand cmd = new SqlCommand(sql, myConnection);

            cmd.Parameters.Add("@rowid", SqlDbType.Int);
            cmd.Parameters["@rowid"].Value = rowId;



            string scalarValue = string.Empty;
            try
            {
                if (transaction == null && myConnection.State != ConnectionState.Open)
                    myConnection.Open();
                scalarValue = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //TODO log;
                scalarValue = null;
            }
            finally
            {
                if (transaction == null &&  myConnection.State == ConnectionState.Open)
                  myConnection.Close();
            }
  
            return scalarValue;
        }

		
		public static ListsDS UpdateLists(ListsDS ListsSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			

			da.Update(ListsSet, GADataClass.GALists.ToString());
			return ListsSet;
		}


	}
}
