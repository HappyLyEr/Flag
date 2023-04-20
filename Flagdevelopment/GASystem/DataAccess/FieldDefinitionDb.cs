using System;
using GASystem.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for FieldDefinitionDb.
	/// </summary>
	public class FieldDefinitionDb
	{
		private static string _selectSql = @"SELECT * FROM GAFieldDefinitions ";

		public FieldDefinitionDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static FieldDefinitionDS GetActiveColumnsForTable(string TableName) 
		{
			string sql = _selectSql + " where TableId = '" + TableName + "' and IsActive = 1 ";
			return (FieldDefinitionDS)FillNewDataSet(sql, GADataClass.GAFieldDefinitions);
		}
		
		public static FieldDefinitionDS GetLookupTableFieldsForTable(string TableName, string LookupTable) 
		{
			string sql = _selectSql + " where TableId = '" + TableName + "' and IsActive = 1 and LookupTable = '" + LookupTable + "'" + "order by  columnorder ";
			return (FieldDefinitionDS)FillNewDataSet(sql, GADataClass.GAFieldDefinitions);
		}

		public static FieldDefinitionDS GetSortColumnsForGADataClass(GADataClass DataClass) 
		{
			string sql = _selectSql + " where TableId = '" + DataClass.ToString() + "' and SortOrder is not null order by SortOrder ";
			return (FieldDefinitionDS)FillNewDataSet(sql, GADataClass.GAFieldDefinitions); 
		}

		public static DataSet FillNewDataSet(string sql, GADataClass DataClass) 
		{
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			DataSet ds = new FieldDefinitionDS();
			da.Fill(ds, DataClass.ToString());
			myConnection.Close();
			return ds;
		}
	
	}
}
