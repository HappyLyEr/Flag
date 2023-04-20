using System;
using GASystem.DataModel;
using System.Data.SqlClient;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for DNNUsersDb.
	/// </summary>
	public class DNNUsersDb
	{
		
		//private static SqlConnection myConnection;

		
		private static string _selectSql = @"SELECT * FROM Users ";
		
		
		public DNNUsersDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DNNUsersDS GetDNNUserByUserId(int UserId)
		{
			String appendSql = " WHERE UserId="+UserId;
			DNNUsersDS UserData= new DNNUsersDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(UserData, "Users");
			return UserData;
		}

	}
}
