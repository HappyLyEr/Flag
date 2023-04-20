using System;
using GASystem.DataModel;
using GASystem.AppUtils;

namespace GASystem.BusinessLayer.SecurityRoles
{
    /// <summary>
    /// Summary description for FlagLinkedRole.
    /// </summary>
    public class FlagLinkedRole : FlagRole
    {
        System.DateTime _dateFrom;
        System.DateTime _dateTo;


        public FlagLinkedRole(int RoleId, GADataRecord Context, GASystem.DataAccess.GADataTransaction transaction, System.DateTime DateFrom, System.DateTime DateTo)
            : base(RoleId, Context, transaction)
        {

            _dateFrom = DateFrom;
            _dateTo = DateTo;
        }

        public System.DateTime DateFrom
        {
            get { return _dateFrom; }
        }

        public System.DateTime DateTo
        {
            get { return _dateTo; }
        }

        /// <summary>
        /// Generate where statement to be used in filter query for getting rows accesible for this role
        /// </summary>
        /// <param name="QueryDataClass">Dataclass of table queried in the base query</param>
        /// <returns></returns>
        public override string GetWhereStatement(string QueryDataClass,
            GASystem.DataAccess.Security.GAAccessType AccessType, string pathAlias)
        {
            //where statement is of format: ( path like '%/galocation-7/%' or (memberclass = 'galocation' and memberclassrowid = 7)
            //sample for location 7. last part is included in order to include context class

            //A valid LinkedRole must have a context. If it is missing genereate a select as if it was a ordinary Role. Wrong type used?
            if (Context == null)
                return base.GetWhereStatement(QueryDataClass, AccessType, pathAlias);

            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(QueryDataClass);

            string sql = string.Empty;

            //if the class does not have a date field, treat it as a normal role. No date restriction on listing
            if (cd.DateField == string.Empty && cd.DateFromField == string.Empty)
                return base.GetWhereStatement(QueryDataClass, AccessType, pathAlias);

            if (cd.DateField != string.Empty)
            {
                sql = string.Format("( ({7}.path like '%/{0}-{1}/%' or (memberclass = '{0}' and memberclassrowid = {1})) and {2} like '%;{3};%' and DataClassQuery.{4} between {5} and {6})",
                    new object[]
                    {
                        Context.DataClass.ToString(), Context.RowId.ToString(), AccessType.ToString() + "Roles",
                        this.RoleId.ToString(), cd.DateField, GetSQLFriendlyDate(this.DateFrom),
                        GetSQLFriendlyDate(this.DateTo), pathAlias
                    });

            }
            else if (cd.DateFromField != string.Empty)
            {
                sql = string.Format(
                    "( ({6}.path like '%/{0}-{1}/%' or (memberclass = '{0}' and memberclassrowid = {1})) and {2} like '%;{3};%' and DataClassQuery.{4} >= {5} )",

                    Context.DataClass.ToString(), Context.RowId.ToString(), AccessType.ToString() + "Roles",
                    this.RoleId.ToString(), cd.DateFromField, GetSQLFriendlyDate(this.DateFrom), pathAlias
                );
            }

            return sql;
        }

        protected string GetSQLFriendlyDate(DateTime date)
        {
            return "'" + date.Year + "-" + date.Month + "-" + date.Day + "'";
        }

        protected string GetSqlMaxDate()
        {
            //select convert(varchar, getdate(), 23) YYYY-MM-DD
            return "'" + DateTime.MaxValue.ToString("yyyy-MM-dd") + "'";
        }

    }
}