using System;
using GASystem.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for FileContentDb.
	/// </summary>
	public class FileContentDb
	{
		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAFileContent ";
		private static string _selectSqlNoContent = @"Select FileContentRowId, FileId, FileName, Description, Owner, mimetype from GAFileContent";

		public static string TableName = "GAFileContent";

		public FileContentDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static FileContentDS GetFileContentByFileContentRowId(int FileContentRowId) 
		{
			string ssql = _selectSql + " where FileContentRowId = " + FileContentRowId;
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(ssql, myConnection);
			FileContentDS ds = new FileContentDS();
			da.Fill(ds, TableName);
			return ds;
		}

		public static FileContentDS GetFileContentNoContentByFileContentRowId(int FileContentRowId) 
		{
			string ssql = _selectSqlNoContent + " where FileContentRowId = " + FileContentRowId;
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(ssql, myConnection);
			FileContentDS ds = new FileContentDS();
			da.Fill(ds, TableName);
			return ds;
		}
		
		public static FileContentDS GetFileContentByFileContentRowId(int[] FileContentRowIds) 
		{
			string ssql;
			if (FileContentRowIds.Length == 0) 
			{
				ssql = _selectSqlNoContent + " where FileContentRowId = 0";
			} 
			else
			{
				System.Text.StringBuilder sqlin = new System.Text.StringBuilder(_selectSqlNoContent);
				sqlin.Append(" where FileContentRowId in (");
				sqlin.Append(FileContentRowIds[0]);
				foreach(int FileContentRowId in FileContentRowIds)
					sqlin.Append(", " + FileContentRowId);
				sqlin.Append(")");
				ssql = sqlin.ToString();
			}


            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(ssql, myConnection);
			FileContentDS ds = new FileContentDS();
			da.Fill(ds, TableName);
			return ds;
		}
		
		public static FileContentDS GetFileContentsByOwner(int OwnerRowId, GADataClass OwnerDataClass) 
		{
			FileContentDS FileContentData = new FileContentDS();
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAFileContent, OwnerRowId, OwnerDataClass);
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(FileContentData, GADataClass.GAFileContent.ToString());
			return FileContentData;
		}

		public static FileContentDS UpdateFileContent(FileContentDS FileContentSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			
			da.Update(FileContentSet, GADataClass.GAFileContent.ToString());
			return FileContentSet;
		}
	}
}
