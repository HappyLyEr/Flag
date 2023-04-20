using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using openwfe.workitem;
using Workitem = GASystem.BusinessLayer.Workitem;
using GASystem.DataAccess.Utils;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for WorkitemDb.
	/// </summary>
	public class WorkitemDb
	{
		private static string _selectSql = "select * from gaworkitem ";


//        public const string WorkitemUserFiler = @" (WorkItemRowId in (
//                                                SELECT WorkItemRowId from GAWorkItem 
//	                                              WHERE extra1 LIKE '%;' + CAST({0} AS varchar) + ';%'
//	                                              OR extra2 LIKE '%;' + CAST({0} AS varchar) + ';%'
//                                                UNION
//                                                SELECT w.WorkitemViewRowId
//                                                FROM dbo.GAProjectedEmploymentsView AS e 
//                                                LEFT OUTER JOIN dbo.GAWorkitemView AS w ON w.Path LIKE e.Path + '%' 
//                                                AND 
//                                                (
//                                                    w.Extra3 LIKE '%;' + CAST(e.RoleListsRowId AS varchar) + ';%'
//                                                    OR w.Extra4 LIKE '%;' + CAST(e.RoleListsRowId AS varchar) + ';%'
//                                                )
//                                                WHERE     (w.WorkitemStatus = 'Active') and (w.switchfree1 = 0 or w.switchfree1 is null)
//                                                AND Personnel = {0}
//                                                UNION 
//                                                SELECT w.WorkitemViewRowId
//                                                FROM dbo.GAEmploymentPathView AS e 
//                                                LEFT OUTER JOIN dbo.GAWorkitemBroadcastView AS w ON w.Path = e.Path 
//                                                AND 
//                                                (
//                                                    w.Extra3 LIKE '%;' + CAST(e.RoleListsRowId AS varchar) + ';%'
//                                                    or w.Extra4 LIKE '%;' + CAST(e.RoleListsRowId AS varchar) + ';%'
//                                                )
//                                                WHERE (w.WorkitemStatus = 'Active') and (w.switchfree1 = 1)
//                                                AND Personnel = {0}) and WorkitemStatus = 'Active') ";


//        public const string WorkitemUserFiler = @"(WorkItemRowId in (
//                                                SELECT distinct WorkItemRowId from GAWorkItem w 
//                                                inner join GAWorkitemParticipant p on w.WorkitemRowId=p.OwnerRowId and p.ParticipantType in ('RP','AP') and p.WorkitemStatus = 'Active' and p.ParticipantId={0}
//                                                UNION
//                                                SELECT distinct w.WorkitemViewRowId FROM dbo.GAProjectedEmploymentsView AS e 
//                                                LEFT OUTER JOIN dbo.GAWorkitemView AS w ON w.Path LIKE e.Path + '%' and w.ParticipantWorkitemStatus='Active' and w.ParticipantType in ('RR','AR') and w.ParticipantId=e.RoleListsRowId 
//                                                WHERE (w.switchfree1 = 0 or w.switchfree1 is null) AND Personnel = {0}
//                                                UNION 
//                                                SELECT distinct w.WorkitemViewRowId FROM dbo.GAEmploymentPathView AS e 
//                                                LEFT OUTER JOIN dbo.GAWorkitemBroadcastView AS w ON w.Path = e.Path and w.ParticipantWorkitemStatus='Active' and w.ParticipantType in ('RR','AR') and w.ParticipantId=e.RoleListsRowId 
//                                                WHERE (w.switchfree1 = 1) AND Personnel = {0})
//                                                ) ";



// Tor 20120508 changed not to include workitem type "WorkflowStart"        public const string WorkitemUserFiler = @"(WorkItemRowId in (
//                                                SELECT OwnerRowId from GAWorkitemParticipant 
//                                                WHERE ParticipantId={0} and WorkitemStatus='Active' 
//                                                AND ParticipantType in ('RP','AP')  --/* get all active workitems assigned to person */
//                                                UNION                               --/* get all active workitems assigned to the person's role(s) */
//                                                SELECT wp.OwnerRowId from GAProjectedEmploymentsView e
//                                                INNER JOIN GAWorkitemParticipant wp ON wp.ParticipantId=e.RoleListsRowId AND wp.WorkitemStatus='Active' AND wp.ParticipantType in ('RR','AR') AND (wp.isBroadCast is null or wp.isBroadCast=0)
//                                                INNER JOIN GASuperClass s ON s.MemberClass='GAWorkitem' AND s.MemberClassRowId=wp.OwnerRowId WHERE Personnel={0} AND s.Path like e.path+'%'
//                                                UNION                               --/* get all active broadcast workitems assigned to the person's role(s) */
//                                                SELECT wp.OwnerRowId from GAEmploymentPathView e
//                                                INNER JOIN GAWorkitemParticipant wp ON wp.ParticipantId=e.RoleListsRowId AND wp.WorkitemStatus='Active' AND wp.ParticipantType in ('RR','AR') AND (wp.isBroadCast = 1)
//                                                INNER JOIN GASuperClass s ON s.MemberClass='GAWorkitem' AND s.MemberClassRowId=wp.OwnerRowId AND (s.OwnerClass NOT LIKE 'GAAction') /* is broadcast workitem */
//                                                WHERE Personnel={0} AND s.Path = e.path))";

//        public const string WorkitemUserFiler = @"(WorkItemRowId in (
//                                                SELECT OwnerRowId from GAWorkitemParticipant 
//                                                WHERE ParticipantId={0} and WorkitemStatus='Active' 
//                                                AND ParticipantType in ('RP','AP')  --/* get all active workitems assigned to person */
//                                                UNION                               --/* get all active workitems assigned to the person's role(s) */
//                                                SELECT wp.OwnerRowId from GAProjectedEmploymentsView e
//                                                INNER JOIN GAWorkitemParticipant wp ON wp.ParticipantId=e.RoleListsRowId AND wp.WorkitemStatus='Active' AND wp.ParticipantType in ('RR','AR') AND (wp.isBroadCast is null or wp.isBroadCast=0)
//                                                INNER JOIN GASuperClass s ON s.MemberClass='GAWorkitem' AND s.MemberClassRowId=wp.OwnerRowId
//                                                WHERE Personnel={0} AND s.Path like e.path+'%'
//                                                UNION                               --/* get all active broadcast workitems assigned to the person's role(s) */
//                                                SELECT wp.OwnerRowId from GAEmploymentPathView e
//                                                INNER JOIN GAWorkitemParticipant wp ON wp.ParticipantId=e.RoleListsRowId AND wp.WorkitemStatus='Active' AND wp.ParticipantType in ('RR','AR') AND (wp.isBroadCast = 1)
//                                                INNER JOIN GASuperClass s ON s.MemberClass='GAWorkitem' AND s.MemberClassRowId=wp.OwnerRowId AND (s.OwnerClass NOT LIKE 'GAAction') /* is broadcast workitem */
//                                                WHERE Personnel={0} AND s.Path = e.path))";

        // Tor 20160628 split workitemFilter in parts to enable swithcing test on Verical on and off

        // Tor 20160411 added vertical to select criteria when assigned to Role
//        public const string WorkitemUserFiler = @"(WorkItemRowId in (
//                                                SELECT OwnerRowId from GAWorkitemParticipant 
//                                                WHERE ParticipantId={0} and WorkitemStatus='Active' 
//                                                AND ParticipantType in ('RP','AP')  --/* get all active workitems assigned to person */
//                                                UNION                               --/* get all active workitems assigned to the person's role(s) and vertical(s) */
//                                                SELECT wp.OwnerRowId from GAProjectedEmploymentsView e
//                                                INNER JOIN GAWorkitemParticipant wp ON wp.ParticipantId=e.RoleListsRowId AND wp.WorkitemStatus='Active' AND wp.ParticipantType in ('RR','AR') AND (wp.isBroadCast is null or wp.isBroadCast=0)
//                                                INNER JOIN GASuperClass s ON s.MemberClass='GAWorkitem' AND s.MemberClassRowId=wp.OwnerRowId
//                                                /*added 20160411 ((assignment vertical is null or assignment vertical = workitem vertical) or workitem vertical is null) */ inner join GAWorkitem w on w.WorkitemRowId=wp.OwnerRowId inner join GAEmployment em on e.EmploymentRowId=em.EmploymentRowId and ((em.JobTitleListsRowId is null or em.JobTitleListsRowId=w.MasterRowId) or w.MasterRowId is null) /*end add*/ 
//                                                WHERE e.Personnel={0} AND s.Path like e.path+'%'
//                                                UNION                               --/* get all active broadcast workitems assigned to the person's role(s) and vertical(s)*/
//                                                SELECT wp.OwnerRowId from GAEmploymentPathView e
//                                                INNER JOIN GAWorkitemParticipant wp ON wp.ParticipantId=e.RoleListsRowId AND wp.WorkitemStatus='Active' AND wp.ParticipantType in ('RR','AR') AND (wp.isBroadCast = 1)
//                                                INNER JOIN GASuperClass s ON s.MemberClass='GAWorkitem' AND s.MemberClassRowId=wp.OwnerRowId AND (s.OwnerClass NOT LIKE 'GAAction') /* is broadcast workitem */
//                                                /*added 20160411 ((assignment vertical is null or assignment vertical = workitem vertical) or workitem vertical is null) */ inner join GAWorkitem w on w.WorkitemRowId=wp.OwnerRowId inner join GAEmployment em on e.EmploymentRowId=em.EmploymentRowId and ((em.JobTitleListsRowId is null or em.JobTitleListsRowId=w.MasterRowId) or w.MasterRowId is null) /*end add*/ 
//                                                WHERE e.Personnel={0} AND s.Path = e.path))";

        // Tor 20160628 added vertical to select criteria when SYS paremeter OWFEisVerticalTestOnNotifications = "YES"
        public const string WorkitemUserFiler = @"(WorkItemRowId in (
SELECT OwnerRowId from GAWorkitemParticipant 
WHERE ParticipantId={0} and WorkitemStatus='Active' 
AND ParticipantType in ('RP','AP')  --/* get all active workitems assigned to person */
UNION                               --/* get all active workitems assigned to the person's role(s) and vertical(s) */
SELECT wp.OwnerRowId from GAProjectedEmploymentsView e
INNER JOIN GAWorkitemParticipant wp ON wp.ParticipantId=e.JobDescription /*201703 Job Title, was accessrole: e.RoleListsRowId*/ AND wp.WorkitemStatus='Active' AND wp.ParticipantType in ('RR','AR') AND (wp.isBroadCast is null or wp.isBroadCast=0)
INNER JOIN GASuperClass s ON s.MemberClass='GAWorkitem' AND s.MemberClassRowId=wp.OwnerRowId 
{1}
WHERE e.Personnel={0} AND s.Path like e.path+'%'
UNION                               --/* get all active broadcast workitems assigned to the person's role(s) and vertical(s)*/
SELECT wp.OwnerRowId from GAEmploymentPathView e
INNER JOIN GAWorkitemParticipant wp ON wp.ParticipantId=e.JobDescription /*201703 Job Title, was accessrole: e.RoleListsRowId*/ AND wp.WorkitemStatus='Active' AND wp.ParticipantType in ('RR','AR') AND (wp.isBroadCast = 1)
INNER JOIN GASuperClass s ON s.MemberClass='GAWorkitem' AND s.MemberClassRowId=wp.OwnerRowId AND (s.OwnerClass NOT LIKE 'GAAction') /* is broadcast workitem */
 {1}
WHERE e.Personnel={0} AND s.Path = e.path))";


        public const string WorkitemUserAllFiler = @"(WorkItemRowId in (
SELECT OwnerRowId from GAWorkitemParticipant 
WHERE ParticipantId={0}
AND ParticipantType in ('RP','AP')  --/* get all active workitems assigned to person */
UNION                               --/* get all active workitems assigned to the person's role(s) and vertical(s) */
SELECT wp.OwnerRowId from GAProjectedEmploymentsView e
INNER JOIN GAWorkitemParticipant wp ON wp.ParticipantId=e.JobDescription /*201703 Job Title, was accessrole: e.RoleListsRowId*/ AND wp.WorkitemStatus='Active' AND wp.ParticipantType in ('RR','AR') AND (wp.isBroadCast is null or wp.isBroadCast=0)
INNER JOIN GASuperClass s ON s.MemberClass='GAWorkitem' AND s.MemberClassRowId=wp.OwnerRowId 
{1}
WHERE e.Personnel={0} AND s.Path like e.path+'%'
UNION                               --/* get all active broadcast workitems assigned to the person's role(s) and vertical(s)*/
SELECT wp.OwnerRowId from GAEmploymentPathView e
INNER JOIN GAWorkitemParticipant wp ON wp.ParticipantId=e.JobDescription /*201703 Job Title, was accessrole: e.RoleListsRowId*/ AND wp.WorkitemStatus='Active' AND wp.ParticipantType in ('RR','AR') AND (wp.isBroadCast = 1)
INNER JOIN GASuperClass s ON s.MemberClass='GAWorkitem' AND s.MemberClassRowId=wp.OwnerRowId AND (s.OwnerClass NOT LIKE 'GAAction') /* is broadcast workitem */
 {1}
WHERE e.Personnel={0} AND s.Path = e.path))";

        // Tor 20160628 vertical part of select criteria to be used when SYS paremeter OWFEisVerticalTestOnNotifications = "YES"
        public const string WorkitemUserVerticalFilter = 
@"/*added 20160411 ((assignment vertical is null or assignment vertical = workitem vertical) or workitem vertical is null) */ 
inner join GAWorkitem w on w.WorkitemRowId=wp.OwnerRowId 
inner join GAEmployment em on e.EmploymentRowId=em.EmploymentRowId 
and ((em.JobTitleListsRowId is null or em.JobTitleListsRowId=w.MasterRowId) 
or w.MasterRowId is null) 
/*end add*/ ";
        public WorkitemDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static WorkitemDS GetAllWorkitems() 
		{
			string sql = "select * from gaworkitem where WorkitemStatus = '" + GASystem.BusinessLayer.Workitem.WorkitemStatus.Active.ToString() +   "'";
			WorkitemDS wds = new WorkitemDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			da.Fill(wds, GADataClass.GAWorkitem.ToString());
		
			return wds;
		}

		public static WorkitemDS GetAllActiveWorkitemsByPersonnelId(int PersonnelId) 
		{
			//string sql = "select workitemrowid, subject from gaworkitem where WorkitemStatus = '" + GASystem.BusinessLayer.Workitem.WorkitemStatus.Active + "' and Extra1 like '%;" + PersonnelId +   ";%'" ;

            //            string sqlview = @"select * from gaworkitem where workitemrowid in (
            //            select workitemrowid from gaworkitem 
            //	where extra1 LIKE '%;' + CAST({0} AS varchar) + ';%'
            //	or extra2 LIKE '%;' + CAST({0} AS varchar) + ';%'
            //union
            //SELECT     w.WorkitemViewRowId
            //FROM         dbo.GAProjectedEmploymentsView AS e LEFT OUTER JOIN
            // dbo.GAWorkitemView AS w ON w.Path LIKE e.path + '%' 
            // AND (
            //w.Extra3 LIKE '%;' + CAST(e.RoleListsRowId AS varchar) + ';%'
            //or w.Extra4 LIKE '%;' + CAST(e.RoleListsRowId AS varchar) + ';%'
            //)
            //WHERE     (w.WorkitemStatus = 'Active') and (not(w.switchfree1 = 1))
            //and personnel = {0} 
            //UNION 
            //SELECT     w.WorkitemViewRowId
            //FROM         dbo.GAEmploymentPathView AS e LEFT OUTER JOIN
            // dbo.GAWorkitemBroadcastView AS w ON w.Path = e.path 
            // AND (
            //w.Extra3 LIKE '%;' + CAST(e.RoleListsRowId AS varchar) + ';%'
            //or w.Extra4 LIKE '%;' + CAST(e.RoleListsRowId AS varchar) + ';%'
            //)
            //WHERE     (w.WorkitemStatus = 'Active') and (w.switchfree1 = 1)
            //and personnel = {0}) and WorkitemStatus = 'Active'  
            //
            //";

            DataCache.ValidateCache(DataCache.DataCacheType.AllActiveWorkItemsByPersonnelId);

            WorkitemDS cachedObject = (WorkitemDS)DataCache.GetCachedObject(DataCache.DataCacheType.AllActiveWorkItemsByPersonnelId, PersonnelId.ToString());
            if (cachedObject != null)
                return cachedObject;

            string sqlview = _selectSql + " where " + WorkitemUserFiler;
            string sql = string.Empty;


			//string sql = "select * from gaworkitem where WorkitemStatus = '" + GASystem.BusinessLayer.Workitem.WorkitemStatus.Active + "' and Extra1 like '%;" + PersonnelId +   ";%'" ;
            //set ordering

            // Tor 20160628 if OWFEisVerticalTestOnNotifications = YES, test assignment on vertical, else no vertical test
            string OWFEisVerticalTestOnNotifications = new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEisVerticalTestOnNotifications");
            if (OWFEisVerticalTestOnNotifications.ToUpper() == "YES")
            {
                sql = string.Format(sqlview, PersonnelId.ToString(),WorkitemUserVerticalFilter);
            }
            else
            {
                sql = string.Format(sqlview, PersonnelId.ToString()," ");
            }

            //string sql = string.Format(sqlview, PersonnelId.ToString());

            // Tor 20120508 : changed to exclude workitems of type "WorkflowStart" (field TextFree1 sql += " order by workitemrowid desc";
            sql += " and not (TextFree1='WorkflowStart') order by workitemrowid desc";
            WorkitemDS wds = new WorkitemDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			da.Fill(wds, GADataClass.GAWorkitem.ToString());
            DataCache.AddCachedObject(DataCache.DataCacheType.AllActiveWorkItemsByPersonnelId, PersonnelId.ToString(), wds);
		
			return wds;
		}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="personnleId"></param>
        /// <param name="workitemId"></param>
        /// <returns></returns>
        public static bool hasRoleInWorkitem(int personnelId, int workitemId)
        {
            // Tor 20170319 test on all roles: convert roles to string and generate w.Extra3 like
//            string sqlTest = @"SELECT COUNT(w.WorkitemViewRowId)
//                            FROM dbo.GAProjectedEmploymentsView AS e 
//                            LEFT OUTER JOIN dbo.GAWorkitemAllActiveView AS w ON w.Path LIKE e.path + '%' 
//                            AND w.Extra3 LIKE '%;' + CAST(e.RoleListsRowId AS varchar) + ';%'
//                            WHERE w.WorkitemViewRowId = {0}
//                            AND Personnel = {1}";
            // Tor 20170626 replace accessRole (RoleListsRowId) with JobTitle (JobDescription)
            //string sqlPersonnelRoles = "select e.RoleListsRowId from GAProjectedEmploymentsView e where Personnel={0}";
            string sqlPersonnelRoles = "select e.JobDescription from GAProjectedEmploymentsView e where Personnel={0}";
            string sql = string.Format(sqlPersonnelRoles, personnelId.ToString());
            SqlDataReader Roles = GASystem.DataAccess.DataUtils.executeSelect(sql);
            if (Roles.HasRows)
            {
                string sqlTest = @"SELECT COUNT(w.WorkitemViewRowId) FROM dbo.GAProjectedEmploymentsView AS e 
                            LEFT OUTER JOIN dbo.GAWorkitemAllActiveView AS w ON w.Path LIKE e.path + '%' 
                            AND ({0})
                            WHERE w.WorkitemViewRowId = {1}
                            AND Personnel = {2}";

                string rolesSelect=" or w.Extra3 LIKE '%;{0};%'";
                string roles = string.Empty;
                while (Roles.Read())
                {
                    roles = roles + string.Format(rolesSelect, Roles.GetInt32(0).ToString());
                }
                if (roles.Length>5 && roles.Substring(0,4)==" or ")
                {
                    sql = string.Format(sqlTest, roles.Substring(3), workitemId.ToString(), personnelId.ToString());
                    SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
                    myConnection.Open();
                    SqlCommand cmd = new SqlCommand(sql, myConnection);
                    bool result = (int)cmd.ExecuteScalar() > 0;
                    myConnection.Close();
                    return result;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personnleId"></param>
        /// <param name="workitemId"></param>
        /// <returns></returns>
        public static bool hasParticipantRoleInWorkitem(int personnelId, int workitemId)
        {
            // Tor 20170319 test on all roles: convert roles to string and generate w.Extra3 like
            // Tor 20170626 replace accessRole (RoleListsRowId) with JobTitle (JobDescription)
            // string sqlPersonnelRoles = "select e.RoleListsRowId from GAProjectedEmploymentsView e where Personnel={0}";
            string sqlPersonnelRoles = "select e.JobDescription from GAProjectedEmploymentsView e where Personnel={0}";
            string sql = string.Format(sqlPersonnelRoles, personnelId.ToString());
            SqlDataReader Roles = GASystem.DataAccess.DataUtils.executeSelect(sql);
            if (Roles.HasRows)
            {
                string sqlTest = @"SELECT COUNT(w.ActionWorkitemViewRowId) FROM dbo.GAProjectedEmploymentsView AS e 
                            LEFT OUTER JOIN dbo.GAActionWorkitemView AS w ON w.Path LIKE e.Path + '%' 
                            AND ({0})
                            WHERE  w.ActionWorkitemViewRowid = {1}
                            AND Personnel = {2}";

                string rolesSelect=" or w.Extra3 LIKE '%;{0};%'";
                string roles = string.Empty;
                while (Roles.Read())
                {
                    roles = roles + string.Format(rolesSelect, Roles.GetInt32(0).ToString());
                }
                if (roles.Length>5 && roles.Substring(0,4)==" or ")
                {
                    sql = string.Format(sqlTest, roles.Substring(3), workitemId.ToString(), personnelId.ToString());
                    SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
                    myConnection.Open();
                    SqlCommand cmd = new SqlCommand(sql, myConnection);
                    bool result = (int)cmd.ExecuteScalar() > 0;
                    myConnection.Close();
                    return result;
                }
            }
//                string sqlTest = @"SELECT COUNT(w.ActionWorkitemViewRowId)
//                            FROM dbo.GAProjectedEmploymentsView AS e 
//                            LEFT OUTER JOIN dbo.GAActionWorkitemView AS w ON w.Path LIKE e.Path + '%' 
//                            AND w.Extra4 LIKE '%;' + CAST(e.RoleListsRowId AS varchar) + ';%'
//                            WHERE  w.ActionWorkitemViewRowid = {0}
//                            AND Personnel = {1}";

//            string sql = string.Format(sqlTest, workitemId.ToString(), personnelId.ToString());

//            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
//            myConnection.Open();
//            SqlCommand cmd = new SqlCommand(sql, myConnection);
//            bool result = (int)cmd.ExecuteScalar() > 0;
//            myConnection.Close();

//            return result;
            return false;
        }

        /// <summary>
        /// Checking whether user has an edit assigment to worktiem
        /// </summary>
        /// <param name="personnelid">users personnelid</param>
        /// <param name="actionid">id of action owning the workitem</param>
        /// <param name="roles">list of roles defined with updatedate access on gaactionworkitemview for action owner</param>
        /// <returns></returns>
        public static bool hasEditAssigmentToWorkitem(int personnelid, int actionid, string roles)
        {
            if (roles == null || roles == string.Empty)
                return false;

            string sqlTest = @"SELECT COUNT(*) from GASuperClass s INNER JOIN dbo.GAProjectedEmploymentsView e on s.Path like e.Path + '%'
                            where s.MemberClass = 'GAAction' and s.MemberClassRowId = {1} AND e.RoleListsRowId in ({2}) and e.Personnel = {0}";

            string sql = string.Format(sqlTest, personnelid.ToString(), actionid.ToString(), roles);

            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            myConnection.Open();
            SqlCommand cmd = new SqlCommand(sql, myConnection);
            bool result = (int)cmd.ExecuteScalar() > 0;
            myConnection.Close();

            return result;

        }

        /// <summary>
        /// find all workitems where all workitems havning the same workitemidentifier is set to proceedpending
        /// </summary>
        /// <returns></returns>
		public static WorkitemDS GetAllPendingWorkitems() 
		{
			
            string sql = "select * from gaworkitem where WorkitemStatus = '" + GASystem.BusinessLayer.Workitem.WorkitemStatus.ProceedPending.ToString() +
                "' and WorkitemIdentifier not in (select workitem2.WorkitemIdentifier from gaworkitem workitem2 where WorkitemStatus = '" + GASystem.BusinessLayer.Workitem.WorkitemStatus.Active.ToString() + "'  and WorkitemIdentifier is not null)";
			WorkitemDS wds = new WorkitemDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
			da.Fill(wds, GADataClass.GAWorkitem.ToString());
		
			return wds;
		}

		public static bool hasWorkitemInTable(string WorkitemIdentifier) 
		{
			string _selectSql = "select count(*) from gaworkitem where WorkitemIdentifier = '" + WorkitemIdentifier +   "'";
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			SqlCommand cmd = new SqlCommand(_selectSql, myConnection);
			myConnection.Open();
			int count = (int) cmd.ExecuteScalar();
			myConnection.Close();

			return count > 0;
		}

//        public static WorkitemDS GetAllWorkitemHeaders()
//        {
//            //TODO use cache
			
//            WorkitemDS ds = new WorkitemDS();
//            openwfe.rest.worklist.WorkSession ws = new openwfe.rest.worklist.WorkSession(Workitem.WorkSessionServerAddress, Workitem.WorkSessionServerPort, Workitem.OWFEUserName, Workitem.OWFEPassword);

//            System.Collections.IList headers = ws.GetHeaders(Workitem.GAUserStoreName);
//            foreach (openwfe.workitem.Header h in headers)
//            {
//                //openwfe.workitem.StringMapAttribute smap = (openwfe.workitem.StringMapAttribute)h.attributes;
				
//                //get participants
//                //TODO expand to take into account participant references
//                string gaParticipant = h.attributes[new StringAttribute("gaparticipant")].ToString();
				
//                if (gaParticipant != string.Empty) 
//                {
//                    //string[] gaPraticipants = gaParticipant.Split(',');
					
				


				
////					//add rows
////					if (gaPraticipants != null) 
////					{
////
////						foreach (string participant in gaPraticipants) 
////						{

//                            WorkitemDS.GAWorkitemRow row = ds.GAWorkitem.NewGAWorkitemRow();
//                            row.GAParticipant = gaParticipant;
//                            row.Subject = h.attributes[new StringAttribute("__subject__")].ToString();
//                            row.Notes = h.attributes[new StringAttribute("__notes__")].ToString();
//                            row.ActionRowId = int.Parse(h.attributes[new StringAttribute("__gaactionid__")].ToString());
//                            row.WorkitemIdentifier = h.flowExpressionId.workflowInstanceId + "-" +  h.flowExpressionId.expressionId;
//                            row.WorkflowURL = h.flowExpressionId.workflowDefinitionUrl;
//                            row.WorkflowName = h.flowExpressionId.workflowDefinitionName;
//                            row.WfInstanceId = h.flowExpressionId.workflowInstanceId;
//                            row.ExpressionId = h.flowExpressionId.expressionId;
//                            ds.GAWorkitem.AddGAWorkitemRow(row);
////						}
////
////					}

//                }
//            }
//            ws.Close();
//            ds.AcceptChanges();
//            return ds;
//        }


		public static WorkitemDS GetWorkItemByWorkItemId(int WorkitemRowId)
		{
			String appendSql = " WHERE WorkItemRowId="+WorkitemRowId;
			WorkitemDS wds = new WorkitemDS();		
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
		
			SqlDataAdapter da = new SqlDataAdapter(_selectSql+appendSql, myConnection);
			da.Fill(wds, GADataClass.GAWorkitem.ToString());
			return wds;
		}
	
		public static WorkitemDS UpdateWorkitem(WorkitemDS WorkitemSet, GADataTransaction Transaction)
		{
			// Transaction start
			SqlConnection myConnection = DataUtils.GetConnection(Transaction);
			SqlDataAdapter da = new SqlDataAdapter(_selectSql, myConnection);
			if (Transaction != null)
				da.SelectCommand.Transaction = (SqlTransaction) Transaction.Transaction;
			da.SelectCommand.Connection = myConnection;
			// 
			SqlCommandBuilder  cb = new SqlCommandBuilder(da);
			da.RowUpdated += new SqlRowUpdatedEventHandler(DataUtils.DataAdapter_RowUpdated);

			da.Update(WorkitemSet, GADataClass.GAWorkitem.ToString());
			return WorkitemSet;
		}

        // Tor 20160206 Get all Active or Proceed pending workitems under owner
        public static WorkitemDS GetAllActiveWorkitemsUnderOwner(GADataRecord owner, GADataTransaction transaction)
        {
            string sql = @"select w.* from GAWorkitem w   
inner join GASuperClass s on s.MemberClass='GAAction' and s.OwnerClass='{0}' and s.OwnerClassRowId={1} and w.ActionRowId=s.MemberClassRowId   
where (w.WorkitemStatus='Active' or w.WorkitemStatus='ProceedPending') and w.TextFree1!='WorkflowStart'";

            sql = string.Format(sql, owner.DataClass.ToString(),owner.RowId.ToString());


            WorkitemDS wds = new WorkitemDS();
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            //            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            

            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;

            da.Fill(wds, GADataClass.GAWorkitem.ToString());

            return wds;
        }
	
	}
}
