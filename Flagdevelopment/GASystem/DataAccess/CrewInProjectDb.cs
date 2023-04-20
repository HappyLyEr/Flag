using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
    public class CrewInProjectDb
    {
        //private static SqlConnection myConnection;
        private static string _selectSql = @"select CrewInProjectRowId from GACrewInProject where (EndDate is null or EndDate > GETDATE()) and ";
        
        public CrewInProjectDb()
		{
			//
			// TODO: Add constructor logic here
            //		
		}


        public static int GetAllCrewInProject(int CrewRowId)
        {
            String appendSql = "CrewRowId=" + CrewRowId;
            int crewinprojectrowid = 0;
            CrewInProjectDS ds = new CrewInProjectDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(_selectSql+appendSql, myConnection);
                crewinprojectrowid = (int)myCommand.ExecuteScalar();
            }
            catch
            { }
            finally
            {
                myConnection.Close();
            }
            return crewinprojectrowid;
        }
             
    }
}
