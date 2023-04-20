using System;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Security.Cryptography;
using System.IO;
using System.Text;


namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for License.
	/// </summary>
	public class License : BusinessClass
	{
		private static byte[] IV = {123,45,23,87,201,43,79,23,45,12,56,12,218,95,34,27};
		private static byte[] KEY = {74,92,34,21,83,229,194,73,81,4,96,163,132,173,77,52,27,35,12,2,54,142,149,43,71,154,103,12,63,82,9,239};
		
		public static bool IsLicenseValid = false;
		
		public License()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static LicenseDS GetAllLicenses() 
		{
			//			GASystem.DataAccess.DataAccess licenseDA = new GASystem.DataAccess.DataAccess(GADataClass.GALicense, null);
			//			return (LicenseDS)licenseDA.GetAll();
			return LicenseDb.GetAllLicenses();
		}

		public static void ValidateLicense() 
		{
			if (IsLicenseValid == true) 
				return;

			//check that there is a license row
			LicenseDS lds = GetAllLicenses();
			if (lds.GALicense.Rows.Count == 0)
				throw new Exception("No license for FLAG found");

			//check signature
			string clearText = lds.GALicense[0].CompanyName + lds.GALicense[0].ExpireDate.Year.ToString() 
				+ lds.GALicense[0].ExpireDate.Month.ToString() + lds.GALicense[0].ExpireDate.Day.ToString();

			MD5 md5 = new MD5CryptoServiceProvider();
			
			ASCIIEncoding textConverter = new ASCIIEncoding();
			byte[] fromText = textConverter.GetBytes(clearText);


			byte[] md5result = md5.ComputeHash(fromText);
			//string sresult = System.Convert.ToBase64String(result);


			


		
			RijndaelManaged myRijndael = new RijndaelManaged();
			

			//
			//			//Get an encryptor.
			ICryptoTransform encryptor = myRijndael.CreateEncryptor(KEY, IV);
			
			//            
			//			//Encrypt the data.
			MemoryStream msEncrypt = new MemoryStream();
			CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
			
			//			//Write all data to the crypto stream and flush it.
			csEncrypt.Write(md5result, 0, md5result.Length);
			csEncrypt.FlushFinalBlock();
			
			//Get encrypted array of bytes.
			byte[] encrypted;
			encrypted = msEncrypt.ToArray();
			string encryptedBase64 = System.Convert.ToBase64String(encrypted);



			if (encryptedBase64 != lds.GALicense[0].Signature.ToString())
				throw new Exception("Invalid license for FLAG. Signature does not match");
		





			//check date
			if (lds.GALicense[0].ExpireDate < System.DateTime.Now)
				throw new Exception("Your license for FLAG has expired");


			
			IsLicenseValid = true;
			

			return;
		}

	}
}
