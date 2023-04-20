using System;
using GASystem.DataModel;
using System.Web;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Summary description for GUIQueryString.
	/// </summary>
	public class GUIQueryString
	{
		private static string OWNERROWID = "ownerrowid";
		private static string OWNERCLASS = "ownerclass";
		
		
		
		public GUIQueryString()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static GADataRecord GetOwner(HttpRequest Request) 
		{
			if (null==Request[OWNERCLASS] || null == Request[OWNERROWID] || !GAUtils.IsNumeric(Request[OWNERROWID]))
				return null;
		
			GADataClass dataClass;
			try 
			{
				dataClass = GADataRecord.ParseGADataClass(Request[OWNERCLASS].ToString());
			} 
			catch
			{
				//ownerclass does not contain a valid gadataclass, return null
				return null;
			}
			
			return new GADataRecord(int.Parse(Request[OWNERROWID]), dataClass);
		}
	}
}
