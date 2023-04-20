using System;
using GASystem.DataModel;
using GASystem.DataAccess.Utils;
using System.Data.SqlClient;
using System.Data;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for PersonnelDb.
	/// </summary>
	public class PersonnelDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAPersonnel";
        // Tor 20150828 check if PersonnelRowId exists - return PersonnelRowId
        private static string _selectSqlIfPersonExists = @"SELECT PersonnelRowId FROM GAPersonnel ";
		
		public PersonnelDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static PersonnelDS GetAllPersonnels()
		{
            PersonnelDS PersonnelData = new PersonnelDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(PersonnelData, "GAPersonnel");
		
			return PersonnelData;
		}

		public static PersonnelDS GetPersonnelByPersonnelRowId(int PersonnelRowId)
		{
            DataCache.ValidateCache(DataCache.DataCacheType.PersonnelByPersonnelRowId);

            PersonnelDS cachedObject = (PersonnelDS)DataCache.GetCachedObject(DataCache.DataCacheType.PersonnelByPersonnelRowId, PersonnelRowId.ToString());
            if (cachedObject != null)
                return cachedObject;

            String appendSql = " WHERE PersonnelRowId=" + PersonnelRowId;
			PersonnelDS PersonnelData = new PersonnelDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(PersonnelData, "GAPersonnel");
            DataCache.AddCachedObject(DataCache.DataCacheType.PersonnelByPersonnelRowId, PersonnelRowId.ToString(), PersonnelData);
            return PersonnelData;
		}

        // Tor 20140524 added for use by WorkflowStarter.WorkflowStarter.cs to build GAAction records
        public static PersonnelDS GetPersonnelByPersonnelGivenNameAndFamilyName(string GivenName, string Familyname)
        {
            String appendSql = " WHERE GivenName='" + GivenName + "' and FamilyName='"+Familyname+"'";
            PersonnelDS PersonnelData = new PersonnelDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            SqlDataAdapter da = new SqlDataAdapter(_selectSql + appendSql, myConnection);
            da.Fill(PersonnelData, "GAPersonnel");
            return PersonnelData;
        }

        // Tor 20150828 Check if person record exists - return first PersonnelRowId occurrence
        public static int ifPersonExists(int PersonnelRowId)
        {
            string sql = _selectSqlIfPersonExists + " where PersonnelRowId=" + PersonnelRowId;
            return GetPersonnelRowId(sql);
        }

        public static int ifPersonExists(string GivenName, string FamilyName)
        {
            string sql = _selectSqlIfPersonExists + " where GivenName='" + GivenName.ToString() + "' and FamilyName='" + FamilyName.ToString() + "'";
            return GetPersonnelRowId(sql);
        }

        private static int GetPersonnelRowId(string sql)
        // Tor 20150828 Check if person record exists - return first PersonnelRowId occurrence
        {
            // Tor 20160515 return data in array list            SqlDataReader result = GASystem.DataAccess.DataUtils.executeSelectSpecial(sql, DataUtils.getConnectionString());
            System.Collections.ArrayList result = GASystem.DataAccess.DataUtils.executeSelectSpecial(sql, DataUtils.getConnectionString(), "int");

            if (result.Count != 0) return (int)result[0];
            // returns first occurence

            return 0;
        }

        public static string GetPersonnelEmailAddresByLogonId(string LogonId)
        // Tor 20181031 Returns email address
        {
            string sql = "select p.TextFree1 from GAPersonnel p inner join GAUser u on u.PersonnelRowId=p.PersonnelRowId where u.DNNUserId='{0}'";
            sql = string.Format(sql, LogonId);
            System.Collections.ArrayList result = GASystem.DataAccess.DataUtils.executeSelectSpecial(sql, DataUtils.getConnectionString(), "STRING");
            if (result.Count != 0) return result[0].ToString();
            // returns first occurence
            return "";
        }


		public static PersonnelDS GetPersonnelsByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
//			PersonnelDS PersonnelData = new PersonnelDS();	
//			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAPersonnel, OwnerRowId, OwnerDataClass);
//			
//			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
//			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
//			da.Fill(PersonnelData, GADataClass.GAPersonnel.ToString());
//			return PersonnelData;
			
			//personnel is a top level dataclass. return all personnel
			return GetAllPersonnels();
		}

			
		public static PersonnelDS UpdatePersonnel(PersonnelDS PersonnelSet, GADataTransaction Transaction)
		{
			//
			// TODO: Check if foreach is allowed within transaction, or if it has to be built outside the transaction, in business layer ?
			//
			foreach(PersonnelDS.GAPersonnelRow row in PersonnelSet.GAPersonnel)
			{
				if (row.RowState !=  DataRowState.Deleted)    //can not make changes to a row marked for deletion
                    row.Name = row.FamilyName + " " + row.GivenName;
			}

			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			
			da.Update(PersonnelSet, GADataClass.GAPersonnel.ToString());
			return PersonnelSet;
		}
	}
}
