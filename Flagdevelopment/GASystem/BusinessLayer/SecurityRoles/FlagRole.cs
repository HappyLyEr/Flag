using System;
using GASystem.DataModel;
using GASystem.BusinessLayer;

namespace GASystem.BusinessLayer.SecurityRoles
{
	/// <summary>
	/// Summary description for FlagRole.
	/// </summary>
	public class FlagRole
	{
		private int _roleId;
		//private string _roleName;
		private GADataRecord _context;
		//private string _path;

        public FlagRole(int RoleId, GADataRecord Context, GASystem.DataAccess.GADataTransaction transaction)
		{
			_roleId = RoleId;
			_context = Context;
            //if (RoleId < 0)
            //    //special handling for roleids less than 0. These are used for special purpopses in Flag. E.g. -1 is everyone, -2 is company user
            //    _roleName = "InternalFlagRole " + RoleId.ToString();
            //else 
            //{
            //    _roleName = Lists.GetListValueByRowId(RoleId, transaction);
            //    if (_roleName == null)
            //        _roleName = string.Empty;
                
            //    //ListsDS lds = Lists.GetListsByListsRowId(RoleId);
            //    //if (lds.GALists.Rows.Count == 0)
            //    //    throw new GAExceptions.GASecurityException("Role not found in Lists");
            //    //_roleName = lds.GALists[0].GAListValue;
            //}
			
		}

		/// <summary>
		/// RoleId for the role. Equals to rowid in GALists. Roleids are used when settings rolepermissions on classes
		/// </summary>
		public int RoleId 
		{
			get {return _roleId;}
		}

        ///// <summary>   //Removed by jof 20090729, not referenced by any code
        ///// RoleName
        ///// </summary>
        //public string RoleName
        //{
        //    get {return _roleName;}
        //}

		/// <summary>
		/// Context where the user has this role. For Roles based on engagement this is the ownerclass and owner rowid for the 
		/// engagement in gasuperclass
		/// </summary>
		public GADataRecord Context
		{
			get {return _context;}
		}

        public const string whereStringAllUsers = "( ({2}.path like '/%' ) and {0} = ';{1};' )";
        public const string whereStringContext =
            "( ({4}.PathLevel1 = '{0}-{1}' or {4}.PathLevel2 = '{0}-{1}' or {4}.PathLevel3 = '{0}-{1}' or {4}.PathLevel4 = '{0}-{1}' or {4}.PathLevel5 = '{0}-{1}' or {4}.PathLevel6 = '{0}-{1}' or ({4}.memberclass = '{0}' and memberclassrowid = {1})) and {2} like '%;{3};%' )";

        /// <summary>
        /// Generate where statement to be used in filter query for getting rows accesible for this role
        /// </summary>
        /// <param name="QueryDataClass">Dataclass of table queried in the base query</param>
        /// <returns></returns>
        public virtual string GetWhereStatement(string QueryDataClass, GASystem.DataAccess.Security.GAAccessType AccessType, string pathAlias)
		{
			//Lars, July 2010: where statement was formerly of format: ( path like '%/galocation-7/%' or (memberclass = 'galocation' and memberclassrowid = 7)
            //now updated with new PathLevel columns, to increase performance

            //Lars, 05.10.2010: new string separates between role for all users (-1) and company users (-2), to increase performance of the query.
            //
            const string whereStringCompanyUser = "( ({2}.path like '/%' ) and {0} like ';{1};%' )";
            const string whereStringNoContext = "( ({2}.path like '/%' ) and {0} like '%;{1};%' )";
            //Lars, 05.10.2010: added new if test to separate between whereStringAllUsers and whereStringCompanyUser, former whereStringTopLevel removed.
            if (Context == null && this.RoleId.ToString() == "-1")	//top level
                return string.Format(whereStringAllUsers, AccessType.ToString() + "Roles", this.RoleId.ToString(), pathAlias);

            else if (Context == null && this.RoleId.ToString() == "-2")
                return string.Format(whereStringCompanyUser, AccessType.ToString() + "Roles", this.RoleId.ToString(), pathAlias);

            //added in case of no context cases where roleId is not -1 or -2
            else if (Context == null)
                return string.Format(whereStringNoContext, AccessType.ToString() + "Roles", this.RoleId.ToString(), pathAlias);

			return string.Format(whereStringContext, Context.DataClass.ToString(), Context.RowId.ToString(), AccessType.ToString() + "Roles", this.RoleId.ToString(), pathAlias);
		}


//		/// <summary>
//		/// Generate path to be used in where statement
//		/// </summary>
//		/// <returns></returns>
//		protected string generateQueryPath() 
//		{
//			//if context is null we are at the top level
//			if (Context == null)
//				return "/";
//			
//			SuperClassDS sds = GASystem.BusinessLayer.DataClassRelations.GetByMember(Context);
//
//		}



	}
}
