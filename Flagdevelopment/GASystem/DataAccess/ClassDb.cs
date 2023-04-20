using System;
using GASystem.DataModel;
using Class = GASystem.BusinessLayer.Class;
using System.Data;
using System.Data.SqlClient;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
    class ClassDb
    {
        public ClassDb()
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

        public static ClassDS GetClassByClassRowId(int ClassRowId)
        {
            string sql = _selectSql + " where ClassRowId = @myClassRowId";
            ClassDS ds = new ClassDS();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            da.SelectCommand.Parameters.AddWithValue("@myClassRowId", ClassRowId);
            da.Fill(ds, GADataClass.GAClass.ToString());
            return ds;
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
