using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for UserDb.
	/// </summary>
	public class UserDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAUser";
		
		public UserDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static UserDS GetAllUsers()
		{

			UserDS UserData = new UserDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(UserData, "GAUser");
		
			return UserData;
		}

		public static UserDS GetUserByUserRowId(int UserRowId)
		{
			String appendSql = " WHERE UserRowId="+UserRowId;
			UserDS UserData = new UserDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(UserData, "GAUser");
			return UserData;
		}

		public UserDS GetUserByLogonId(string LogonId)
		{
			return GetUserByLogonId(LogonId, null);
		}


        public UserDS GetUserByLogonId(string LogonId, GADataTransaction Transaction)
		{
            DataCache.ValidateCache(DataCache.DataCacheType.UserByLogonId);
                            
            UserDS cachedObject = (UserDS) DataCache.GetCachedObject(DataCache.DataCacheType.UserByLogonId, LogonId);
            if (cachedObject!=null)
                return cachedObject;
            
            String appendSql = " WHERE DNNUserId='"+LogonId+"'";
			UserDS UserData = new UserDS();
            SqlConnection myConnection = DataUtils.GetConnection(Transaction); // new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			if (Transaction != null)
				da.SelectCommand.Transaction = (SqlTransaction)Transaction.Transaction;
			da.Fill(UserData, "GAUser");
            if (Transaction == null && myConnection.State != ConnectionState.Closed)
                myConnection.Close();
            DataCache.AddCachedObject(DataCache.DataCacheType.UserByLogonId, LogonId, UserData);
			return UserData;
		}

		public static UserDS GetUserByPersonnelRowId(int PersonnelRowId)
		{
			String appendSql = " WHERE PersonnelRowId="+PersonnelRowId;
			UserDS UserData = new UserDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(UserData, "GAUser");
          //  myConnection.Close();
			return UserData;
		}

		public static UserDS GetUsersByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			UserDS UserData = new UserDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAUser, OwnerRowId, OwnerDataClass);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(UserData, GADataClass.GAUser.ToString());
			return UserData;
		}

		public static PersonnelDS GetPersonnelByUserId(String UserId)
		{
			PersonnelDS personnelData = new PersonnelDS();
			string sql = "SELECT * FROM GAPersonnel INNER JOIN GAUser ON GAPersonnel.PersonnelRowId=GAUser.PersonnelRowId WHERE GAUser.DNNUserId='"+UserId+"'";
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(sql , myConnection);
			da.Fill(personnelData, GADataClass.GAPersonnel.ToString());
			return personnelData;
		}

        // Tor 20150804 Added to get all distinct DNN user ids by current employment where employment owner in where statement and roles in role list
        // Tor 20151113 Added parameter VerticalListsRowId - if value != null and value != 0 filter on GAEmployment.JobTitleListsRowId=parameter value
        public static System.Collections.ArrayList getDNNuserIdWhereOwnerInListAndRolesInList(string ownerList, string roleList, int verticalListsRowId)
        {
            // Tor 20170325 Look for Job Titles (TITL in GAEmployment.JobDescription) instead of Access Roles (ER in GAEmployment.RoleListsRowId)
            string selectDNNUsers = @"
select distinct u.DNNUserId 
from GAEmployment e 
inner join GASuperClass s on s.MemberClass='GAEmployment' and s.MemberClassRowId=e.EmploymentRowId and ( {0} ) 
inner join GAUser u on u.PersonnelRowId=e.Personnel 
where (/*e.RoleListsRowId*/e.JobDescription in ({1}) and (e.FromDate<=GETUTCDATE() and (e.ToDate>=GETUTCDATE() or e.ToDate is null))) ";

            if (verticalListsRowId > 0)
                //if (verticalListsRowId != null && verticalListsRowId != 0)
            {
                // Tor 20160626 if OWFEisVerticalTestOnNotifications = YES, test assignment on vertical, else no vertical test
                string OWFEisVerticalTestOnNotifications = new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEisVerticalTestOnNotifications");
                if (OWFEisVerticalTestOnNotifications.ToUpper() == "YES")
                {
                    selectDNNUsers = selectDNNUsers + " and (e.JobTitleListsRowId=" + verticalListsRowId.ToString() + " or " + verticalListsRowId.ToString() + "=0 or e.JobTitleListsRowId is null)";
                }
            }
            string mySql = string.Format(selectDNNUsers, ownerList, roleList);

            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            System.Collections.ArrayList DNNUsersList=new System.Collections.ArrayList();
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(mySql, myConnection);
                SqlDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0].ToString()!="" && reader[0].ToString()!=string.Empty) DNNUsersList.Add(reader[0].ToString());
                }
            }
            catch
            { }
            finally
            {
                myConnection.Close();
            }
            return DNNUsersList;
        }
        
        public static UserDS UpdateUser(UserDS UserSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			if (Transaction != null)
				da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			

			da.Update(UserSet, GADataClass.GAUser.ToString());
			return UserSet;
		}


	}
}
