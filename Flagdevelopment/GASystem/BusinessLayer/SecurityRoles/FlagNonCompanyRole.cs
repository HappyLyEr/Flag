using System;
using System.Collections.Generic;
using GASystem.DataModel;
using GASystem.AppUtils;
using GASystem.AppUtils.DateRangeGenerator;
using GASystem.DataAccess.Security;

namespace GASystem.BusinessLayer.SecurityRoles
{
    /// <summary>
    /// Summary description for FlagLinkedRole.
    /// </summary>
    public class FlagNonCompanyRole : FlagLinkedRole
    {
        private readonly string _assignmentPath;
        //private AssignmentRoleContext assignmentRoleContexts;

        public FlagNonCompanyRole(int RoleId, GADataRecord Context, GASystem.DataAccess.GADataTransaction transaction,
            System.DateTime DateFrom, System.DateTime DateTo, string assignmentPath)
            : base(RoleId, Context, transaction, DateFrom, DateTo)
        {
            this._assignmentPath = assignmentPath;
            //this.assignmentRoleContexts = new List<AssignmentRoleContext>();
        }

        /// <summary>
        /// Direct access means that a user should have access to that record because it was assigned DIRECTLY in GAEmployment NO matter the date
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private string SetDirectAccess(string sql, GASystem.DataAccess.Security.GAAccessType AccessType, string pathAlias)
        {
            if (this._assignmentPath != null && AccessType == GAAccessType.Read)
            {
                string directAccess = string.Format(
                    " OR (({0}.Path = '{1}' AND MemberClass = '{2}' AND MemberClassRowId = {3})  OR {0}.Path = '{4}' AND {0}.OwnerClass = '{2}' AND {0}.MemberClass in ('GACrewInProject','GALocationInCrew') )",
                    pathAlias,
                    GetParentAssignmentPath(),
                    this.Context.DataClass.ToString(),
                    this.Context.RowId.ToString(),
                    this._assignmentPath
                );

                return sql.Replace("/*DirectAccess*/", directAccess);
            }
            else
            {
                return sql.Replace("/*DirectAccess*/", "");
            }
        }


        /// <summary>
        /// Gets the parent path where the assignment of personnel was made
        /// </summary>
        private string GetParentAssignmentPath()
        {
            if (this._assignmentPath == null) return null;

            string[] paths = this._assignmentPath.Trim('/').Split('/');

            if (paths.Length <= 1) return null;

            string parentPath = string.Empty;
            for (int c = 0; c < paths.Length - 1; c++)
            {
                parentPath += paths[c] + "/";
            }

            return string.Format("/{0}", parentPath);
        }

        /// <summary>
        /// Generate where statement to be used in filter query for getting rows accesible for this role
        /// </summary>
        /// <param name="QueryDataClass">Dataclass of table queried in the base query</param>
        /// <returns></returns>
        public override string GetWhereStatement(string QueryDataClass,
            GASystem.DataAccess.Security.GAAccessType AccessType, string pathAlias)
        {
            //A valid LinkedRole must have a context. If it is missing genereate a select as if it was a ordinary Role. Wrong type used?
            if (Context == null)
                return base.GetWhereStatement(QueryDataClass, AccessType, pathAlias);

            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(QueryDataClass);

            string dateAlias, dateMember;
            if (cd.DateField == string.Empty && cd.DateFromField == string.Empty)
            {
                dateAlias = pathAlias;
                dateMember = "DateCreated";
            }
            else
            {
                dateAlias = "DataClassQuery";
                dateMember = cd.DateField != string.Empty ? cd.DateField : cd.DateFromField;
            }

            if (this.RoleId.ToString() == "-1")//add date range when role is -1 and the user has "PERSONNEL ASSIGNMENT"
            {
                return string.Format(
                    whereStringAllUsers.TrimEnd(')') + (cd.RemoveFromDateRestrict ? ")" : " and isnull({3}.{4}, convert(date,{5},23)) >= {6} )"),
                    AccessType.ToString() + "Roles",
                    this.RoleId.ToString(),
                    pathAlias,
                    dateAlias,
                    dateMember,
                    this.GetSqlMaxDate(),
                    this.GetSQLFriendlyDate(this.DateFrom)
                );
            }

            string sql1 = string.Format(
                "( " + whereStringContext.TrimEnd(')') + (cd.RemoveFromDateRestrict ? ") /*DirectAccess*/ )" : " and isnull({5}.{6}, convert(date,{7},23)) >= {8} ) /*DirectAccess*/ )"),
                Context.DataClass.ToString(), //TransformClassIfCrewInProjectContext(Context.DataClass, QueryDataClass),
                Context.RowId.ToString(),
                AccessType.ToString() + "Roles",
                this.RoleId.ToString(),
                pathAlias, /*4*/
                dateAlias,
                dateMember,
                this.GetSqlMaxDate(),
                GetSQLFriendlyDate(this.DateFrom)
            );

            return SetDirectAccess(sql1, AccessType, pathAlias);
        }

        /*

        
        private string TransformClassIfCrewInProjectContext(GADataClass contextDataClass, string queryDataClass)
        {
            if (string.Equals(GADataClass.GACrewInProject.ToString(), queryDataClass,
                    StringComparison.InvariantCultureIgnoreCase)
                &&
                string.Equals(contextDataClass.ToString(), GADataClass.GACrew.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)
            )
            {
                return GADataClass.GACrewInProject.ToString();
            }

            return contextDataClass.ToString();
        }


        public string GetAccessByPersonnelAssignmentContext(string dataClass, string rowId,
            DataAccess.Security.GAAccessType accessType, string roleId, string pathAlias)
        {
            return string.Format(" OR " + whereStringContext, dataClass, rowId, accessType + "Roles", roleId, pathAlias);
        }

        public FlagNonCompanyRole AddAssignmentContext(AssignmentRoleContext assignment)
        {
            this.assignmentRoleContexts = assignment;
            return this;
        }*/
    }
}
