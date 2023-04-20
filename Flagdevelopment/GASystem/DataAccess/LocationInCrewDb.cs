using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
    public class LocationInCrewDb
    {
        //private static SqlConnection myConnection;
        private static string _selectSql = @"select LocationInCrewRowId from GALocationInCrew where (EndDate is null or EndDate > GETDATE()) and ";
        
        public LocationInCrewDb()
		{
			//
			// TODO: Add constructor logic here
            //		public ListCategory()
		}


        public static int GetAllLocationInCrew(int LocationRowId)
        {
            String appendSql = "LocationRowId=" + LocationRowId;
            int locationincrewrowid = 0;
            LocationInCrewDS ds = new LocationInCrewDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(_selectSql+appendSql, myConnection);
                locationincrewrowid = (int)myCommand.ExecuteScalar();
            }
            catch
            {

            }
            finally
            {
                myConnection.Close();
            }
            return locationincrewrowid;
        }
             
    }
}
