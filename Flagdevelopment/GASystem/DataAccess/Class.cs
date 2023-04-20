using System;
using GASystem.DataModel;
using System.Data;
using System.Data.SqlClient;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for Class.
	/// </summary>
	public class Class
	{
		public Class()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private static string _selectSql = @"SELECT * FROM GAClass ";

     
		public static ClassDS GetClassByGADataClass(GADataClass DataClass)  
		{
			return GetClassByGADataClass(DataClass.ToString());
		}

		public static ClassDS GetClassByGADataClass(string DataClassName)  
		{
            DataCache.ValidateCache(DataCache.DataCacheType.SuperClassLinksByOwner);

            ClassDS cachedObject = (ClassDS)DataCache.GetCachedObject(DataCache.DataCacheType.ClassByGADataClass, DataClassName);
            if (cachedObject != null)
                return cachedObject;
            
            ClassDS Class = new ClassDS();
            string sql = _selectSql + " where class = '" + DataClassName + "'";
            Class = fillDataSet(sql);
            DataCache.AddCachedObject(DataCache.DataCacheType.ClassByGADataClass, DataClassName, Class);
            return Class;
		}


		public static ClassDS GetAllClasses() 
		{
			return fillDataSet(_selectSql);
		}

		private static ClassDS fillDataSet(string sql) 
		{
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			ClassDS ds = new ClassDS();
			da.Fill(ds, GADataClass.GAClass.ToString());
			myConnection.Close();
			return ds;
		}
	}
}
