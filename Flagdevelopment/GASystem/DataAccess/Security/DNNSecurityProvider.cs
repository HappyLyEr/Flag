using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using GASystem.DataModel;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess.Security
{
	/// <summary>
	/// Summary description for DNNSecurityProvider.
	/// </summary>
	public class DNNSecurityProvider : IGASecurityProvider
	{
		private string _dnnConnectionString = null;

		private string _selectUserRoles = @"SELECT     Roles.* 
											FROM         Users INNER JOIN
											UserRoles ON Users.UserID = UserRoles.UserID INNER JOIN
											Roles ON UserRoles.RoleID = Roles.RoleID
											WHERE     (Users.Username = N'{0}')";
		
		public DNNSecurityProvider(string DnnConnectionString)
		{
			_dnnConnectionString = DnnConnectionString;
		}


		public RolesDS GetUserGroups(string UserId)
		{
            DataCache.ValidateCache(DataCache.DataCacheType.UserGroups);

            RolesDS cachedObject = (RolesDS)DataCache.GetCachedObject(DataCache.DataCacheType.UserGroups, UserId);
            if (cachedObject != null)
                return cachedObject;

            string sql = string.Format(_selectUserRoles, UserId);
			RolesDS RolesData = new RolesDS();
			SqlDataAdapter da = new SqlDataAdapter(sql, _dnnConnectionString);
			da.Fill(RolesData, "Roles");
            DataCache.AddCachedObject(DataCache.DataCacheType.UserGroups, UserId, RolesData);
			return RolesData;
			/*
			IDataReader reader = DataUtils.executeSelect("", _dnnConnectionString);
			ArrayList roleArray = new ArrayList();
			while (reader.Read())
			{
				roleArray.Add(reader["RoleId"].ToString());
			}
			return (string[]) roleArray.ToArray(typeof(string));*/
		}

		public RolesDS GetAllGroups()
		{
			String sql = "SELECT * FROM Roles";
			RolesDS RolesData = new RolesDS();
			SqlDataAdapter da = new SqlDataAdapter(sql, _dnnConnectionString);
			da.Fill(RolesData, "Roles");
			return RolesData;
			
		}



	}
}
