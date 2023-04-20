using System;
using GASystem.DataModel;
using GASystem.DataAccess.Utils;
using System.Data.SqlClient;
using System.Data;


namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for EmploymentDb.
    /// 
    /// Note this class is rewritten to use projectedemploymentsview instead of employmentspathview
	/// </summary>
	public class EmploymentPathViewDb
	{

		//private static SqlConnection myConnection;
        private static string _selectSql = @"SELECT * FROM GAProjectedEmploymentsView";

        public EmploymentPathViewDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static  EmploymentPathViewDS GetAllEmployments()
		{

            EmploymentPathViewDS EmploymentData = new EmploymentPathViewDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
            da.Fill(EmploymentData, "GAEmploymentPathView");
		
			return EmploymentData;
		}



        public static EmploymentPathViewDS GetEmploymentByPersonnelId(int personnelId, GADataTransaction transaction)   //, DateTime endDate)
		{


            String appendSql = " WHERE Personnel = @personnelid";
            string sql = _selectSql + appendSql;
            
            EmploymentPathViewDS EmploymentData = new EmploymentPathViewDS();
            EmploymentPathViewDS cachedObject = new EmploymentPathViewDS();

            DataCache.ValidateCache(DataCache.DataCacheType.EmploymentByPersonnelId);

            cachedObject = (EmploymentPathViewDS)DataCache.GetCachedObject(DataCache.DataCacheType.EmploymentByPersonnelId, personnelId.ToString());
            if (cachedObject != null)
                return cachedObject;

            SqlCommand command = new SqlCommand(sql);
            command.Parameters.AddWithValue("@personnelid", personnelId);
            
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            command.Connection = myConnection;

            SqlDataAdapter da = new SqlDataAdapter(command);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;

            da.Fill(EmploymentData, GADataClass.GAEmploymentPathView.ToString());
            DataCache.AddCachedObject(DataCache.DataCacheType.EmploymentByPersonnelId, personnelId.ToString(), EmploymentData);
            return EmploymentData;
            
		}
		

	}
}
