using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for ListCategoryDb.
	/// </summary>
	public class ListCategoryDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAListCategory";
		
		public ListCategoryDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ListCategoryDS GetAllListCategories()
		{

			ListCategoryDS ListCategoryData = new ListCategoryDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(ListCategoryData, "GAListCategory");
		
			return ListCategoryData;
		}

		/// <summary>
		/// Get listcategory row for a given rowid
		/// </summary>
		/// <param name="ListCategoryRowId"></param>
		/// <returns></returns>
		public static ListCategoryDS GetListCategoryByListCategoryRowId(int ListCategoryRowId)
		{
			String appendSql = " WHERE ListCategoryRowId="+ListCategoryRowId;
			ListCategoryDS ListCategoryData = new ListCategoryDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(ListCategoryData, "GAListCategory");
			return ListCategoryData;
		}

		/// <summary>
		/// get listcategories rows for by an array rowids
		/// </summary>
		/// <param name="ListCategoryRowIds"></param>
		/// <returns></returns>
		public static ListCategoryDS GetListCategoryByListCategoryRowIds(int[] ListCategoryRowIds)
		{
			if (ListCategoryRowIds.Length == 0)
				return new ListCategoryDS();  //ids passed return empty dataset
			
			//build a comma seperated string of listrow ids.  TODO move this to a general utility function.
			System.Text.StringBuilder sb = new System.Text.StringBuilder(ListCategoryRowIds[0].ToString());
			for (int t=1; t<ListCategoryRowIds.Length; t++) 
			{
				sb.Append(", ");
				sb.Append(ListCategoryRowIds[t]);
			}


			String appendSql = " WHERE ListCategoryRowId in (" + sb.ToString() +  ")";
			ListCategoryDS ListCategoryData = new ListCategoryDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(ListCategoryData, "GAListCategory");
			return ListCategoryData;
		}

		/// <summary>
		/// Returns all categories. Same as list all. Included for compatibility with coding standard.
		/// </summary>
		/// <param name="OwnerRowId"></param>
		/// <param name="OwnerDataClass"></param>
		/// <returns></returns>
		public static ListCategoryDS GetListCategorysByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			return GetAllListCategories();
		}

		
		public static ListCategoryDS UpdateListCategory(ListCategoryDS ListCategorySet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			

			da.Update(ListCategorySet, GADataClass.GAListCategory.ToString());
			return ListCategorySet;
		}

		public static int GetListCategoryRowIdByName(string ListCategoryName) 
		{
//			String appendSql = " WHERE GAListCategory='"+ListCategoryName + "'";
//			ListCategoryDS ListCategoryData = new ListCategoryDS();		
//			myConnection = new SqlConnection(DataUtils.getConnectionString());
//		
//			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
//			da.Fill(ListCategoryData, "GAListCategory");
//			if (ListCategoryData.GAListCategory.Rows.Count == 0 )
//				return -1;  //TODO throw exception
//			
//			return ListCategoryData.GAListCategory[0].ListCategoryRowId;

            DataCache.ValidateCache(DataCache.DataCacheType.ListCategoryRowIdByName);

            if (DataCache.GetCachedObject(DataCache.DataCacheType.ListCategoryRowIdByName, ListCategoryName) != null)
            {
                int cachedObject = (int)DataCache.GetCachedObject(DataCache.DataCacheType.ListCategoryRowIdByName, ListCategoryName);
                return cachedObject;
            }
            
            string sql = "select ListCategoryRowId FROM GAListCategory WHERE GAListCategory='"+ListCategoryName + "'";

			//ListsDS ListsData = new ListsDS();		
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			int rowid = -1;
			try 
			{
				myConnection.Open();
				SqlCommand myCommand = new SqlCommand(sql, myConnection);
				//myCommand.Transaction.IsolationLevel
				rowid = (int)myCommand.ExecuteScalar();
			} 
			catch (Exception ex)
			{
				string exmsg = ex.Message;
			}
			finally
			{
				myConnection.Close();
			}

            DataCache.AddCachedObject(DataCache.DataCacheType.ListCategoryRowIdByName, ListCategoryName, rowid);
            return rowid;			
		}

	}
}
