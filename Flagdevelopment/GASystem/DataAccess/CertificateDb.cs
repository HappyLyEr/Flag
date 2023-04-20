using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using System.Data;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for CertificateDb.
	/// </summary>
	public class CertificateDb
	{

		//private static SqlConnection myConnection;
		private static string _selectSql = @"SELECT * FROM GACertificate";
		
		public CertificateDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static CertificateDS GetAllCertificates()
		{

			CertificateDS CertificateData = new CertificateDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(CertificateData, "GACertificate");
		
			return CertificateData;
		}

		public static CertificateDS GetCertificateByCertificateRowId(int CertificateRowId)
		{
			String appendSql = " WHERE CertificateRowId="+CertificateRowId;
			CertificateDS CertificateData = new CertificateDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(CertificateData, "GACertificate");
			return CertificateData;
		}

		public static CertificateDS GetCertificatesByOwner(int OwnerRowId, GADataClass OwnerDataClass )
		{
			CertificateDS CertificateData = new CertificateDS();	
			string selectSqlOwner = SQLGenerateUtils.GetSelectSqlMembers(GADataClass.GACertificate, OwnerRowId, OwnerDataClass);
			
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(selectSqlOwner , myConnection);
			da.Fill(CertificateData, GADataClass.GACertificate.ToString());
			return CertificateData;
		}

			
		public static CertificateDS UpdateCertificate(CertificateDS CertificateSet, GADataTransaction Transaction)
		{
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);
			
			da.Update(CertificateSet, GADataClass.GACertificate.ToString());
			return CertificateSet;
		}
	}
}
