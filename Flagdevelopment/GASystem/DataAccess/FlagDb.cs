using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
    // Tor 20140418 required after start using GASuperClassLinks instead of GAClass to decide permission
    class FlagDb
    {
		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAFlag";
		
		public FlagDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Get Flag row for a given rowid
		/// </summary>
		/// <param name="FlagRowId"></param>
		/// <returns></returns>
		public static FlagDS GetFlagByFlagRowId(int FlagRowId)
		{
			String appendSql = " WHERE FlagRowId="+FlagRowId;
			FlagDS FlagData = new FlagDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(FlagData, "GAFlag");
			return FlagData;
		}
    }
}
