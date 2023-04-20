using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for CompanyDb.
	/// </summary>
	public class CompanyDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GACompany";
		
		/// // Tor 20130908 replaced this with next statement private static string _selectSqlAll = @"SELECT CompanyRowId, CompanyId,Name, Address,Comment,BankAccount,Telephone, MimeType FROM GACompany";
		private static string _selectSqlAll = @"SELECT CompanyRowId, CompanyId, Name, Address,Comment,BankAccount,Telephone, EMailAddress, WebAddress, FileRowId FROM GACompany";
		
		public CompanyDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static CompanyDS GetAllCompanys()
		{

			CompanyDS CompanyData = new CompanyDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

            // Tor 20130908 replaced this with next statement SqlDataAdapter da = new SqlDataAdapter(_selectSqlAll, myConnection);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(CompanyData, "GACompany");
		
			return CompanyData;
		}

		public static CompanyDS GetCompanyByCompanyRowId(int CompanyRowId)
		{
            DataCache.ValidateCache(DataCache.DataCacheType.CompanyByCompanyRowId);

            CompanyDS cachedObject = (CompanyDS)DataCache.GetCachedObject(DataCache.DataCacheType.CompanyByCompanyRowId, CompanyRowId.ToString());
            if (cachedObject != null)
                return cachedObject;
            
            String appendSql = " WHERE CompanyRowId="+CompanyRowId;
			CompanyDS CompanyData = new CompanyDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(CompanyData, "GACompany");
            DataCache.AddCachedObject(DataCache.DataCacheType.CompanyByCompanyRowId, CompanyRowId.ToString(), CompanyData);
            return CompanyData;
		}

		public static CompanyDS GetCompanyNoContentByCompanyRowId(int CompanyRowId)
		{
            DataCache.ValidateCache(DataCache.DataCacheType.CompanyNoContentByCompanyRowId);
            
            CompanyDS cachedObject = (CompanyDS)DataCache.GetCachedObject(DataCache.DataCacheType.CompanyNoContentByCompanyRowId, CompanyRowId.ToString());
            if (cachedObject != null)
                return cachedObject;
            
            String appendSql = " WHERE CompanyRowId="+CompanyRowId;
			CompanyDS CompanyData = new CompanyDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSqlAll+appendSql, myConnection);
			da.Fill(CompanyData, "GACompany");
            DataCache.AddCachedObject(DataCache.DataCacheType.CompanyNoContentByCompanyRowId, CompanyRowId.ToString(), CompanyData);
            return CompanyData;
		}

		public static CompanyDS GetCompanysByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			CompanyDS CompanyData = new CompanyDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GACompany, OwnerRowId, OwnerDataClass);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(CompanyData, GADataClass.GACompany.ToString());
			return CompanyData;
		}

		
		public static CompanyDS UpdateCompany(CompanyDS CompanySet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			
			da.Update(CompanySet, GADataClass.GACompany.ToString());
			return CompanySet;
		}

	}
}
