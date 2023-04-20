using System;
using System.Data;
using System.Data.SqlClient;
using GASystem.DataModel;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for StoreAttributeDb.
	/// </summary>
	public class StoreAttributeDb
	{
		private static string _selectSql = @"SELECT * FROM GAStoreAttribute ";
		
		public StoreAttributeDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static StoreAttributeDS GetStoreAttributeByRowId(int RowId) 
		{
			string sql = _selectSql + " where StoreAttributeRowId = " + RowId;
			return (StoreAttributeDS) fillDataSet(sql);
		}

		
		public static StoreAttributeDS GetStoreAttributesByOwner(GADataRecord Owner) 
		{
			string selectSqlOwner = "";
			//TODO, should we throw an error if the owner class is not a valid parent?
			if (Owner.DataClass == GADataClass.GAStoreObject)   //hardwired owner, we know this relation from the datamodel
				selectSqlOwner = _selectSql + " where StoreObjectRowId = " + Owner.RowId;
			else
				selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAStoreAttribute, Owner.RowId, Owner.DataClass);
			return (StoreAttributeDS) fillDataSet(selectSqlOwner);
		}

		public static StoreAttributeDS UpdateStoreAttribute(StoreAttributeDS StoreAttributeDataSet)
		{
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);

			da.Update(StoreAttributeDataSet, GADataClass.GAStoreAttribute.ToString());
			return StoreAttributeDataSet;
		}

		private static DataSet fillDataSet(string sql) 
		{
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			StoreAttributeDS ds = new StoreAttributeDS();
			da.Fill(ds, GADataClass.GAStoreAttribute.ToString());
			myConnection.Close();
			return ds;
		}
	}
}
