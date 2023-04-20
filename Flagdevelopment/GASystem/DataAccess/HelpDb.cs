using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for HelpDb.
	/// </summary>
	public class HelpDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAHelp";
		
		public HelpDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static HelpDS GetAllHelps()
		{

			HelpDS HelpData = new HelpDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(HelpData, "GAHelp");
		
			return HelpData;
		}

		public static HelpDS GetHelpByHelpRowId(int HelpRowId)
		{
			String appendSql = " WHERE HelpRowId="+HelpRowId;
			HelpDS HelpData = new HelpDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(HelpData, "GAHelp");
			return HelpData;
		}

		public static HelpDS GetHelpByClass(GADataClass DataClass) 
		{
			String appendSql = " WHERE Class='"+DataClass.ToString() + "'";
			HelpDS HelpData = new HelpDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(HelpData, "GAHelp");
			return HelpData;
		}

		public static HelpDS GetHelpsByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			HelpDS HelpData = new HelpDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAHelp, OwnerRowId, OwnerDataClass);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(HelpData, GADataClass.GAHelp.ToString());
			return HelpData;
		}

		
//		public static HelpDS UpdateHelp(HelpDS HelpSet)
//		{
//			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
//			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
//			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
//			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);

//			da.Update(HelpSet, GADataClass.GAHelp.ToString());
//			return HelpSet;
//		}
			
		public static HelpDS UpdateHelp(HelpDS HelpSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			

			da.Update(HelpSet, GADataClass.GAHelp.ToString());
			return HelpSet;
		}
	}
}
