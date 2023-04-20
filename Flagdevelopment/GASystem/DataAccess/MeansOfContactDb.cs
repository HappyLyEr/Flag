using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for MeansOfContactDb.
	/// </summary>
	public class MeansOfContactDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GAMeansOfContact";
		
		public MeansOfContactDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static MeansOfContactDS GetAllMeansOfContacts()
		{

			MeansOfContactDS MeansOfContactData = new MeansOfContactDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(MeansOfContactData, "GAMeansOfContact");
		
			return MeansOfContactData;
		}

		public static MeansOfContactDS GetMeansOfContactByMeansOfContactRowId(int MeansOfContactRowId)
		{
			String appendSql = " WHERE MeansOfContactRowId="+MeansOfContactRowId;
			MeansOfContactDS MeansOfContactData = new MeansOfContactDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(MeansOfContactData, "GAMeansOfContact");
			return MeansOfContactData;
		}

		public static MeansOfContactDS GetMeansOfContactsByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			MeansOfContactDS MeansOfContactData = new MeansOfContactDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAMeansOfContact, OwnerRowId, OwnerDataClass);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(MeansOfContactData, GADataClass.GAMeansOfContact.ToString());
			return MeansOfContactData;
		}

		public static MeansOfContactDS GetMeansOfContactsByOwnerAndDeviceTypeId(int OwnerRowId, GADataClass OwnerDataClass, int DeviceTypeId )
		{
			MeansOfContactDS MeansOfContactData = new MeansOfContactDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAMeansOfContact, OwnerRowId, OwnerDataClass);
			selectSqlOwner += " and ContactDeviceTypeListsRowId = " + DeviceTypeId.ToString();
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(MeansOfContactData, GADataClass.GAMeansOfContact.ToString());
			return MeansOfContactData;
		}

		/// <summary>
		/// Return first address by specified type and owner
		/// </summary>
		/// <param name="OwnerRowId"></param>
		/// <param name="OwnerDataClass"></param>
		/// <param name="DeviceTypeId"></param>
		/// <returns></returns>
		public static string GetContactAddressByOwnerAndDeviceTypeId(int OwnerRowId, GADataClass OwnerDataClass, int DeviceTypeId, GADataTransaction transaction) 
		{
			//string sql = _selectSql = "SELECT ContactDeviceAddress FROM GAMeansOfContact";

			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GAMeansOfContact, OwnerRowId, OwnerDataClass);
			selectSqlOwner += " and ContactDeviceTypeListsRowId = " + DeviceTypeId.ToString();
			selectSqlOwner = selectSqlOwner.Replace("*", "ContactDeviceAddress");

			//ListsDS ListsData = new ListsDS();		
			SqlConnection myConnection = DataUtils.GetConnection(transaction);
			string address = string.Empty;
			try 
			{
				//myConnection.Open();
				SqlCommand myCommand = new SqlCommand(selectSqlOwner, myConnection);
				
				
				if (null != transaction)
					myCommand.Transaction = (SqlTransaction) transaction.Transaction;
				else
					myConnection.Open();
				address = (string)myCommand.ExecuteScalar();
			} 
			catch (Exception ex)
			{
				string exmsg = ex.Message;
				throw new GAExceptions.GAException("Error Getting contact details for personnel id " + OwnerRowId.ToString() + " :\n" + ex.Message,  ex);
				//TODO log;
			}
			finally
			{
				if (transaction == null)
					myConnection.Close();
			}
			return address;
		}

		public static MeansOfContactDS UpdateMeansOfContact(MeansOfContactDS MeansOfContactSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			

			da.Update(MeansOfContactSet, GADataClass.GAMeansOfContact.ToString());
			return MeansOfContactSet;
		}
	}
}
