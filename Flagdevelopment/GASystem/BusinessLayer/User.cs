using System;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Data;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for User.
	/// </summary>
	public class User : BusinessClass
	{
		public User()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAUser;
		}

        // Tor 20161028 Use standard method in : BusinessClass
        //public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
        //{
        //    return UpdateUser((UserDS)ds, transaction);
        //}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetUserByUserRowId(RowId);
		}

		public static UserDS GetAllUsers()
		{
			return UserDb.GetAllUsers();
		}
	

		public static UserDS GetUsersByOwner(GADataRecord UserOwner)
		{
			return  UserDb.GetUsersByOwner(UserOwner.RowId, UserOwner.DataClass);
		}

		public static UserDS GetUserByUserRowId(int UserRowId)
		{
			return UserDb.GetUserByUserRowId(UserRowId);
		}

		public static string GetLogonIdByPersonnelRowId(int PersonnelRowId) 
		{
			UserDS ds = GetUserByPersonnelRowId(PersonnelRowId);
            if (ds.GAUser.Rows.Count == 0 || ds.GAUser[0].IsDNNUserIdNull())
                //				throw new GAExceptions.GADataAccessException("User not found");
                // Tor 20131023 Added personnelrowid to error message string
                throw new GAExceptions.GADataAccessException("User with PersonnelRowId "+PersonnelRowId.ToString()+" not found");
			return ds.GAUser[0].DNNUserId;

		}

		public static UserDS GetUserByLogonId(string LogonId)
		{
			return GetUserByLogonId(LogonId, null);
		}

        public static UserDS GetUserByLogonId(string LogonId, GADataTransaction transaction)
        {
            return (new GASystem.DataAccess.UserDb()).GetUserByLogonId(LogonId, transaction  );
        }

		public static int GetUserIdByLogonId(string LogonId)
		{
			UserDS ds = GetUserByLogonId(LogonId);
			if (ds.GAUser.Rows.Count == 0)
				return -1;
			return ds.GAUser[0].UserRowId;
			
		}

        //public static string GetEmailByLogonId(string LogonId)
        //{
        //    UserDS ds = GetUserByLogonId(LogonId);
        //    if (ds.GAUser.Rows.Count == 0)
        //        return "";
        //    return ds.GAUser[0].UserRowId;

        //}
        public static int GetPersonnelIdByLogonId(string LogonId)
		{
			UserDS ds = GetUserByLogonId(LogonId);
			if (ds.GAUser.Rows.Count == 0)
				return -1;
			return ds.GAUser[0].PersonnelRowId;
			
		}

		public static int GetUserIdByPersonnelRowId(int PersonnelRowId) 
		{
			UserDS ds = GetUserByPersonnelRowId(PersonnelRowId);
			if (ds.GAUser.Rows.Count == 0)
				return -1;

			return ds.GAUser[0].UserRowId;
		}

		

		public static UserDS GetUserByPersonnelRowId(int PersonnelRowId) 
		{
			return UserDb.GetUserByPersonnelRowId(PersonnelRowId);
		}

		public static UserDS GetNewUser()
		{
			UserDS iDS = new UserDS();
			GASystem.DataModel.UserDS.GAUserRow row = iDS.GAUser.NewGAUserRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GAUser.Rows.Add(row);
			return iDS;
			
		}

		public static UserDS SaveNewUser(UserDS UserSet, GADataRecord UserOwner)
		{
			UserSet = UpdateUser(UserSet);
			//DataClassRelations.CreateDataClassRelation(UserOwner.RowId, UserOwner.DataClass, UserSet.GAUser[0].UserRowId, GADataClass.GAUser);
			return UserSet;
		}

		public static UserDS UpdateUser(UserDS UserSet, GADataTransaction transaction )
		{
			return UserDb.UpdateUser(UserSet, transaction);
		}
		public static UserDS UpdateUser(UserDS UserSet)
		{
			return UpdateUser(UserSet, null);
		}


        public static string getUserNameByLogonId(string LogonId)
        {
            DataSet ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(GADataClass.GAUser, new GADataRecord(1, GADataClass.GAFlag), "DNNUserId='" + LogonId + "'");
            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows.Count > 1)
                return string.Empty;

            return ds.Tables[0].Rows[0]["personnelrowid"].ToString();
       }

        // Tor 20150804 Added to get all distinct DNN user ids by current employment where employment owner in where statement and roles in role list
        // Tor 20151113 Added parameter VerticalListsRowId if filter on vertical
        public static System.Collections.ArrayList getDNNuserIdWhereOwnerInListAndRolesInList(string ownerList, string roleList, int verticalListsRowId)
        { 
            return GASystem.DataAccess.UserDb.getDNNuserIdWhereOwnerInListAndRolesInList(ownerList,roleList, verticalListsRowId);
        }
	}
}
