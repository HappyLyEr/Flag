using System;
using GASystem.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for LicenseDb.
	/// </summary>
	public class LicenseDb
	{
		public LicenseDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static LicenseDS GetAllLicenses() 
		{
			
		    string _selectSql = @"SELECT * FROM GALicense";
			LicenseDS LicenseData = new LicenseDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			da.Fill(LicenseData, GADataClass.GALicense.ToString());
		
			return LicenseData;
		}
	}
}
