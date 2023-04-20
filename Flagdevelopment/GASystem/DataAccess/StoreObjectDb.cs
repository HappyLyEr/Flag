using System;
using System.Data;
using System.Data.SqlClient;
using GASystem.DataModel;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for StoreObjectDb.
	/// </summary>
	public class StoreObjectDb
	{
		private static string _selectSql = @"SELECT * FROM GAStoreObject ";
		
		public StoreObjectDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static StoreObjectDS GetStoreObjectByRowId(int RowId) 
		{
			return (StoreObjectDS) GetStoreObjectByRowId(RowId, null);
		}
		public static StoreObjectDS GetStoreObjectByRowId(int RowId, GADataTransaction transaction) 
		{
			string sql = _selectSql + " where StoreObjectRowId = " + RowId;
			return (StoreObjectDS) fillDataSet(sql, transaction);
		}

		public static StoreObjectDS GetStoreObjectsByOwner(GADataRecord Owner) 
		{
			return (StoreObjectDS) GetStoreObjectsByOwner(Owner, null);
		}
		public static StoreObjectDS GetStoreObjectsByOwner(GADataRecord Owner, GADataTransaction transaction) 
		{
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAStoreObject, Owner.RowId, Owner.DataClass);
			return (StoreObjectDS) fillDataSet(selectSqlOwner, transaction);
		}

		public static StoreObjectDS GetStoreObjectsByOwnerClass(string OwnerClassName) 
		{
			return (StoreObjectDS) GetStoreObjectsByOwnerClass(OwnerClassName, null);
		}
		public static StoreObjectDS GetStoreObjectsByOwnerClass(string OwnerClassName, GADataTransaction transaction) 
		{
			string sql = _selectSql + " where OwnerClass = '" + OwnerClassName + "'";
			return (StoreObjectDS) fillDataSet(sql, transaction);
		}

		public static StoreObjectDS UpdateStoreObject(StoreObjectDS StoreObjectDataSet)
		{
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);

			da.Update(StoreObjectDataSet, GADataClass.GAStoreObject.ToString());
			return StoreObjectDataSet;
		}

		
		private static DataSet fillDataSet(string sql, GADataTransaction transaction) 
		{
			SqlConnection myConnection = DataUtils.GetConnection(transaction);
			
			
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			if (transaction != null)
				da.SelectCommand.Transaction = (SqlTransaction) transaction.Transaction;
			
			StoreObjectDS ds = new StoreObjectDS();
			da.Fill(ds, GADataClass.GAStoreObject.ToString());
			//myConnection.Close();
			return ds;
		}
	}
}
