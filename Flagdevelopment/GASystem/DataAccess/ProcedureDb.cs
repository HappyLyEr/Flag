using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for ProcedureDb.
	/// </summary>
	public class ProcedureDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAProcedure";
		
		public ProcedureDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ProcedureDS GetAllProcedures()
		{

			ProcedureDS ProcedureData = new ProcedureDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(ProcedureData, "GAProcedure");
		
			return ProcedureData;
		}

		public static ProcedureDS GetProcedureByProcedureRowId(int ProcedureRowId)
		{
			String appendSql = " WHERE ProcedureRowId="+ProcedureRowId;
			ProcedureDS ProcedureData = new ProcedureDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(ProcedureData, "GAProcedure");
			return ProcedureData;
		}

        // Tor 20140524 added method to get all procedures where ActionOwnerClass like searched for 
        public static ProcedureDS GetProcedureForGADataClass(string dataClass)
        {
			String appendSql = " WHERE ActionOwnerClass like '"+dataClass+"%'";
            ProcedureDS ProcedureData = new ProcedureDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(ProcedureData, "GAProcedure");
			return ProcedureData;
		}

        //  Gao 20230403 added method to get another procedure where there are 2 procedures for a single ActionOnwerClass which is GARisk
        public static ProcedureDS GetSecondProcedureForGADataClass(string dataClass)
        {
            String appendSql = " WHERE ActionOwnerClass like '" + dataClass + "verify%'";
            ProcedureDS ProcedureData = new ProcedureDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            SqlDataAdapter da = new SqlDataAdapter(_selectSql + appendSql, myConnection);
            da.Fill(ProcedureData, "GAProcedure");
            return ProcedureData;
        }
        

        public static ProcedureDS GetProceduresByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			ProcedureDS ProcedureData = new ProcedureDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAProcedure, OwnerRowId, OwnerDataClass);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(ProcedureData, GADataClass.GAProcedure.ToString());
			return ProcedureData;
		}

		public static ProcedureDS UpdateProcedure(ProcedureDS ProcedureSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			

			da.Update(ProcedureSet, GADataClass.GAProcedure.ToString());
			return ProcedureSet;
		}
	}
}
