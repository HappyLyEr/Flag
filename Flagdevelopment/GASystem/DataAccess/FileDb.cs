using System;
using GASystem.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for FileDb.
	/// </summary>
	public class FileDb
	{
		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAFile ";
		private static string _selectSqlNoContent = @"Select FileRowId, FileId, FileName, Description, Owner, mimetype from GAFile";

		public static string TableName = "GAFile";

		public FileDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static FileDS GetFileByFileRowId(int FileRowId) 
		{
			string ssql = _selectSql + " where FileRowId = " + FileRowId;
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(ssql, myConnection);
			FileDS ds = new FileDS();
			da.Fill(ds, TableName);
			return ds;
		}

		public static FileDS GetFileNoContentByFileRowId(int FileRowId) 
		{
			string ssql = _selectSqlNoContent + " where FileRowId = " + FileRowId;
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(ssql, myConnection);
			FileDS ds = new FileDS();
			da.Fill(ds, TableName);
			return ds;
		}
		
		public static FileDS GetFileByFileRowId(int[] FileRowIds) 
		{
			string ssql;
			if (FileRowIds.Length == 0) 
			{
				ssql = _selectSqlNoContent + " where FileRowId = 0";
			} 
			else
			{
				System.Text.StringBuilder sqlin = new System.Text.StringBuilder(_selectSqlNoContent);
				sqlin.Append(" where FileRowId in (");
				sqlin.Append(FileRowIds[0]);
				foreach(int fileRowId in FileRowIds)
					sqlin.Append(", " + fileRowId);
				sqlin.Append(")");
				ssql = sqlin.ToString();
			}


            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(ssql, myConnection);
			FileDS ds = new FileDS();
			da.Fill(ds, TableName);
			return ds;
		}
		
		public static FileDS GetFilesByOwner(int OwnerRowId, GADataClass OwnerDataClass) 
		{
			FileDS fileData = new FileDS();
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAFile, OwnerRowId, OwnerDataClass);
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(fileData, GADataClass.GAFile.ToString());
			return fileData;
		}

		public static FileDS GetAllFiles() 
		{
			FileDS fileData = new FileDS();
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(_selectSql , myConnection);
			da.Fill(fileData, GADataClass.GAFile.ToString());
			return fileData;
		}

		public static FileDS UpdateFile(FileDS FileSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			
			da.Update(FileSet, GADataClass.GAFile.ToString());
			return FileSet;
		}
	}
}
