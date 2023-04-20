using System;
using System.Data;
using GASystem.DataModel;
using GASystem.DataAccess;
using openwfe.workitem;
using System.Web.Caching;
using log4net;
using openwfe.rest.worklist;
using System.Collections;
using GASystem.DataAccess.Security;
using GASystem.BusinessLayer.Utils;
using System.Collections.Generic;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Personnel.
	/// </summary>
	public class Workitem : BusinessClass
	{
		public const string  ROLE_SUFFIX = "garole-";
		public const string  USER_SUFFIX = "gauser-";
        public const string PERSONNEL_SUFFIX = "gapersonnel-";
		public const string ACTORSREPLY = "actorsreply";
		public const string WORKITEM_CACHE = "AllWorkItems";
		public const string GAPARTICIPANT = "gaparticipant";
        public const string WORKITEMTYPE = "FlagWorkitemType"; 

		// 20130508 Added reply options  to public enum ActorsReply {completed, rejected, ok, yes, no}; 
        public enum ActorsReply { completed, rejected, dissatisfied, ok, yes, no, approved, proceed, btnFree1, btnFree2, btnFree3, Close };
        public enum WorkitemStatus { Active, ProceedPending, Completed, Failed };
        public enum WorkitemType { Remedial, RemedialBroadcastAll, RemedialBroadcastContainer, Info, InfoBroadcastContainer, InfoBroadcastAll, Approve, Default, WorkflowStart };
        public enum AcknowledgementStatus {Acknowledged, AwaitingAcknowledgement, AcknowledgementRejected, NoAcknowledgementNeeded };
		
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Workitem));

		public Workitem()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAWorkitem;
		}

//		public override System.Data.DataSet GetNewRecord()
//		{
//			throw new System.Exception("Not implemented");
//		}

		public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
		{
            // Tor 20161109 added: use UpdateGASuperClassChangedBy to update attributes in GASuperClass record 
            GASystem.AppUtils.SuperClassAttributes.UpdateGASuperClassChangedBy(ds, transaction);
            //throw new System.Exception("Not implemented");
			return WorkitemDb.UpdateWorkitem((WorkitemDS)ds, transaction);
		}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			//throw new System.Exception("Not implemented");
			return WorkitemDb.GetWorkItemByWorkItemId(RowId);
		}

		/// <summary>
		/// Get all workitems
		/// </summary>
		/// <returns></returns>
		public static WorkitemDS GetAllWorkitems()
		{
			return GASystem.DataAccess.WorkitemDb.GetAllWorkitems();
		}

		public static WorkitemDS GetAllPendingWorkitems() 
		{
			return GASystem.DataAccess.WorkitemDb.GetAllPendingWorkitems();
		}

		/// <summary>
		/// Get all workitems. Uses webcache for caching the workitems.
		/// </summary>
		/// <param name="WebCache"></param>
		/// <returns></returns>
		public static WorkitemDS GetAllWorkitems(System.Web.Caching.Cache WebCache) 
		{
//			if (WebCache[WORKITEM_CACHE] != null)
//				return (WorkitemDS)WebCache["AllWorkItems"];
//			
//			//items not in cache, reload cache.
//			LoadWorkItemCache(WebCache);
//			
//			//get items from cache
//			if (WebCache[WORKITEM_CACHE] != null)
//				return (WorkitemDS)WebCache["AllWorkItems"];
//
//			//items still not in cache!! There must be an error, return a emtpy dataset
//			WorkitemDS ds = new WorkitemDS();
//			return ds;

			//cache no longer need just return it
			return GetAllWorkitems();
		}

		private static void LoadWorkItemCache(System.Web.Caching.Cache WebCache) 
		{
			System.Web.Caching.CacheItemRemovedCallback onRemove = new System.Web.Caching.CacheItemRemovedCallback(RemovedCallback);
			WorkitemDS ds = GetAllWorkitems();
			WebCache.Insert(WORKITEM_CACHE, ds, null, System.DateTime.Now.AddSeconds(OWFEWorkitemCacheTimeout), System.TimeSpan.Zero, CacheItemPriority.High, onRemove);
		}

		public static void RemovedCallback(String k, Object v, CacheItemRemovedReason r)
		{
			//lets readd workitems
			try 
			{
				System.Web.Caching.Cache WebCache = System.Web.HttpRuntime.Cache;
				LoadWorkItemCache(WebCache); 
			} 
			catch (Exception ex) 
			{
				throw ex;
			}
		}

		/// <summary>
		/// Get all workitems assigned to a specific users
		/// </summary>
		/// <param name="LogonId"></param>
		/// <returns></returns>
		public static WorkitemDS GetAllWorkitemsByLogonId(string LogonId, System.Web.Caching.Cache WebCache)
		{
			int personnelId = User.GetPersonnelIdByLogonId(LogonId);
			return WorkitemDb.GetAllActiveWorkitemsByPersonnelId(personnelId);

//			string gaparticipant = createUserIdentifier(LogonId);
//			WorkitemDS wds = GetAllWorkitems(WebCache);
//			System.Data.DataView dv = new DataView(wds.GAWorkitem, "gaparticipant Like '%" + gaparticipant + "%'", "", DataViewRowState.CurrentRows  );
//			
//			WorkitemDS ds = new WorkitemDS();
//			foreach (DataRowView r in dv)
//			{
//				ds.GAWorkitem.ImportRow(r.Row);
//			}

//			return ds;
		}

        /// <summary>
        /// Get all workitems assigned to a specific users
        /// </summary>
        /// <param name="LogonId"></param>
        /// <returns></returns>
        public static DataSet GetAllWorkitemsDataSetByLogonId(string LogonId, string myFilter)
        {
            int personnelId = User.GetPersonnelIdByLogonId(LogonId);

            string userFilter = WorkitemDb.WorkitemUserAllFiler;

            // Tor 20160628 if OWFEisVerticalTestOnNotifications = YES, test assignment on vertical, else no vertical test
            string OWFEisVerticalTestOnNotifications = new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEisVerticalTestOnNotifications");
            if (OWFEisVerticalTestOnNotifications.ToUpper() == "YES")
            {
                userFilter = string.Format(userFilter, personnelId.ToString(), WorkitemDb.WorkitemUserVerticalFilter, myFilter);
            }
            else
            {
                userFilter = string.Format(userFilter, personnelId.ToString(), " ");
            }

            string myUserFilter = null;
            if (myFilter.ToString().Trim() == "")
            {
                myUserFilter = userFilter;
            }
            else
            {
                myUserFilter = myFilter + " AND " + userFilter;
            }
            
            // Tor 20130915 added get default top node from web.config top Class must be GACompany instead of GAFlag-1 which finds no records because GAFlag-1 is not included in GASuperClass path details fields
            //                              <add key="DefaultContextClass" value="GACompany" />
            //                              <add key="DefaultContextRowId" value="13" />
            //  string defaultContextClass = System.Configuration.ConfigurationManager.AppSettings.Get("DefaultContextClass");
            // Tor 20160303 int defaultContextRowId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("DefaultContextRowId"));
            int defaultContextRowId = Convert.ToInt32(new GASystem.AppUtils.FlagSysResource().GetResourceString("DefaultContextRowId"));

            return RecordsetFactory.GetAllRecordsForGAListViewWithinOwnerAndLinkedRecordsIncludingDropDownDetails(
                  GADataClass.GAWorkitem, 
                  new GADataRecord(defaultContextRowId, GADataClass.GACompany),
                  // Tor 20130915 current line replaced by the line above: new GADataRecord(1, GADataClass.GAFlag), 
                  System.DateTime.MinValue, System.DateTime.MaxValue, myUserFilter);
            
           // return WorkitemDb.GetAllActiveWorkitemsByPersonnelId(personnelId);
        }
		
		/// <summary>
		/// Returns all workitems where no member of the workitem role can be found
		/// </summary>
		/// <returns></returns>
		public static WorkitemDS GetAllWorkitemsNotAssignedToARoleMember(System.Web.Caching.Cache WebCache)
		{
			WorkitemDS wds = GetAllWorkitems(WebCache);
			
			WorkitemDS wdsNotAssigned = new WorkitemDS();
			foreach (WorkitemDS.GAWorkitemRow r in wds.GAWorkitem)
			{
				if (r.GAParticipant.IndexOf(ROLE_SUFFIX) == 0)   //participant is a role
				{					
					EmploymentDS eds = Employment.GetEmploymentsByActionIdDateAndRoleId(r.ActionRowId, System.DateTime.Now, r.GAParticipant);
					if (eds.GAEmployment.Rows.Count == 0) 
					{
						//no user has the workitem role for this workitems add it to the list
						wdsNotAssigned.GAWorkitem.ImportRow(r);
					}
				}
			}
			return wdsNotAssigned;
		}

        ///// <summary>
        ///// Return unassigned workitems for a user based on a master role. Scan Unassigned workitems,if the user
        ///// hold the specified role in the action hierarchy return the workitem to this user
        ///// </summary>
        ///// <returns></returns>

        //public static WorkitemDS GetUnAssignedWorkitemsByUserRole(string LogonId, string Role, System.Web.Caching.Cache WebCache)
        //{
        //    int PersonnelId = User.GetPersonnelIdByLogonId(LogonId);

        //    WorkitemDS wdsUnAssinged = new WorkitemDS();
        //    foreach (WorkitemDS.GAWorkitemRow row in GetAllWorkitemsNotAssignedToARoleMember(WebCache).GAWorkitem)
        //    {
        //        if (IsPersonnelFirstLevelRoleMemberForAction(PersonnelId, row.ActionRowId, Role))
        //        {
        //            wdsUnAssinged.GAWorkitem.ImportRow(row);
        //        }
        //    }
        //    return wdsUnAssinged;
        //}

        /// <summary>
        /// Update all Active workitems under owner with new VerticalRowId
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="VerticalListsRowId"></param>
        /// <returns></returns>
        public static void updateVerticalInAllActiveWorkitemsUnderOwner(GADataRecord owner, int VerticalListsRowId, GADataTransaction transaction)
        {
            /*	
                1. get all active and proceed pending workitems under owner
                2. for each row in dataset set Vertical to VerticalListsRowId
            */

            WorkitemDS ds = GASystem.DataAccess.WorkitemDb.GetAllActiveWorkitemsUnderOwner(owner, transaction);
            // set Vertical in all rows
            foreach (WorkitemDS.GAWorkitemRow row in ds.GAWorkitem.Rows)
            {
// Tor 20160901                row.MasterRowId = VerticalListsRowId;
                //row.MasterRowId = VerticalListsRowId == -1 ? null : VerticalListsRowId;

                if (VerticalListsRowId == -1)
                { 
                row.SetMasterRowIdNull();
                }
                else
                {
                    row.MasterRowId = VerticalListsRowId;
                }
            }
            // update dataset
            ds = GASystem.DataAccess.WorkitemDb.UpdateWorkitem(ds, transaction);
            return;
        }

		/// <summary>
		/// Get all workitems assigned to a users through the users roles
		/// </summary>
		/// <param name="LogonId"></param>
		/// <returns></returns>
		public static WorkitemDS GetAllWorkitemsForUserRoles(string LogonId, System.Web.Caching.Cache WebCache)
		{
			/*	TODO
				Use case:  
				1. get all roles assigned to the user
				2. find all workitems assigned to these roles.
				3. filter the result based on context.
					1. Remove workitem from list if another user has this role futher down the class path.
					2. check that the user has this role in the workitem context
			*/
			
			//get all roles assigned to user  //TODO move this to a operation in employment
			
			int PersonnelId = User.GetPersonnelIdByLogonId(LogonId);
			EmploymentDS ds = Employment.GetEmploymentsByPersonnelIdAndDate(PersonnelId, System.DateTime.Now);
			
			if (ds.GAEmployment.Rows.Count == 0)
				return new WorkitemDS();			//user is not a member of any roles. return an empty workitem list
			
			//get list ids from employment
			System.Collections.ArrayList userRolesId = new System.Collections.ArrayList(ds.GAEmployment.Rows.Count);
			foreach (EmploymentDS.GAEmploymentRow row in ds.GAEmployment.Rows) 
			{
				if (!row.IsRoleListsRowIdNull()) 
					userRolesId.Add(Convert.ToInt32(row.RoleListsRowId));
			}

			//get rolenames from list table
			int[] roleIds = (int[])userRolesId.ToArray(typeof(System.Int32));
			ListsDS lds = Lists.GetListsByListsRowIds(roleIds);

			if (lds.GALists.Rows.Count == 0)	
				return new WorkitemDS();			//ids did not return any role names. There must be some invalid
													//ids in the employment list. TODO log it to disk using log4net
													//handle it as if the user is not a member of any roles. return an empty workitem list			

			System.Collections.ArrayList userRoles = new System.Collections.ArrayList(lds.GALists.Rows.Count);
			foreach (ListsDS.GAListsRow row in lds.GALists.Rows) 
			{
				userRoles.Add(createRoleIdentifier(row.GAListValue.ToString()));
			}				

			//generete where clause
			string participantFiler = string.Empty;
			foreach (string participantRole in userRoles) 
			{
				if (participantFiler != string.Empty) 
				{
					participantFiler += " or ";
				}
				participantFiler += "gaparticipant like '%" +  participantRole + "%'";
			}

			WorkitemDS wds = GetAllWorkitems(WebCache);
			//System.Data.DataView dv = new DataView(wds.GAWorkitem, "gaparticipant in (" + AppUtils.GAUtils.ConvertArrayToString((string[])userRoles.ToArray(typeof(System.String)), ", ") + ")", "", DataViewRowState.CurrentRows  );
			System.Data.DataView dv = new DataView(wds.GAWorkitem, participantFiler, "", DataViewRowState.CurrentRows  );
			
			WorkitemDS wdsSorted = new WorkitemDS();
			foreach (DataRowView r in dv)
			{
				if (IsPersonnelFirstLevelRoleMemberForWorkItem(PersonnelId, (GASystem.DataModel.WorkitemDS.GAWorkitemRow)r.Row))
					wdsSorted.GAWorkitem.ImportRow(r.Row);
			}

			return wdsSorted;
		}

		/// <summary>
		/// Checks whether personnel has role specified in workitem. Starts at gaaction and check for every level to the top of 
		/// the hierarchy
		/// </summary>
		/// <param name="PersonnelId">Id of personnel to check for</param>
		/// <param name="row">Workitem row</param>
		/// <returns>
		/// False: If personnel does not hold the role in the context of this action. And if another person holds this 
		///        role at a lower level in the hierarchy
		/// True:  User holds this role in the context of the action. 
		///</returns>
		private static bool IsPersonnelFirstLevelRoleMemberForWorkItem(int PersonnelId, GASystem.DataModel.WorkitemDS.GAWorkitemRow row) 
		{
			//TODO get participants, splitt string, foreach on all rows matching users roles
			string[] GAParticipants = row.GAParticipant.Split(',');

			foreach (string gaParticipant in GAParticipants) 
			{
				//gaParticipant
				//TDOO if statement
				if (isParticipantARole(gaParticipant))
				{
					string roleName = getRoleIdentiferNamePart(gaParticipant);
					return IsPersonnelFirstLevelRoleMemberForAction(PersonnelId, row.ActionRowId, roleName);
				}
			}
			return false;
		}

        // Tor 20190228 added method
        /// <summary>
        /// Checks whether personnel has ADDED role specified in workitem. 
        /// </summary>
        /// <param name="PersonnelId">Id of personnel to check for</param>
        /// <param name="row">Workitem row</param>
        /// <returns>
        /// False: If personnel does not hold the ADDED role in this WORKITEM. 
        /// True:  User holds this ADDED role in this WORKITEM. 
        ///</returns>
        public bool IsPersonnelAddedRoleForWorkItem(int PersonnelId, int WorkitemRowId)
        {
            WorkitemDS ds = WorkitemDb.GetWorkItemByWorkItemId(WorkitemRowId);
            if (ds.GAWorkitem[0].IsExtra4Null() || ds.GAWorkitem[0].Extra4 == "") return false;
            
            //System.Collections.ArrayList result = new System.Collections.ArrayList();
            GADataRecord record = new GADataRecord(WorkitemRowId, GADataRecord.ParseGADataClass("GAWorkitem"));
            string result = Employment.GetDistinctJobTitleOrRoleAboveMember(record, PersonnelId, "Title");
            if (result == string.Empty) return false;
            string[] Titles = ds.GAWorkitem[0].Extra4.Substring(1, ds.GAWorkitem[0].Extra4.Length - 2).Split(';');

            foreach (string Title in Titles) // int foundTitle in result)
            {
                if (result.Contains(";"+Title+";"))
                    //if (ds.GAWorkitem[0].Extra4.Contains(";" + foundTitle.ToString() + ";"))
                return true;
            }

            return false;
        }

        private static bool isParticipantARole(string Participant) 
		{
			return Participant.IndexOf(ROLE_SUFFIX) > -1;
			
		}

        private static bool isParticipantAPerson(string Participant)
        {
            return Participant.IndexOf(PERSONNEL_SUFFIX) > -1;

        }

		/// <summary>
		/// Checks whether personnel has role specified in workitem. Starts at gaaction and check for every level to the top of 
		/// the hierarchy
		/// </summary>
		/// <param name="PersonnelId">Id of personnel to check for</param>
		/// <param name="ActionId">Action id</param>
		/// <param name="Role">Role name without the GAROLE suffix</param>
		/// <returns>
		/// False: If personnel does not hold the role in the context of this action. And if another person holds this 
		///        role at a lower level in the hierarchy
		/// True:  User holds this role in the context of the action. 
		///</returns>
		private static bool IsPersonnelFirstLevelRoleMemberForAction(int PersonnelId, int ActionId, string Role) 
		{
			GADataRecord owner = DataClassRelations.GetOwner(new GADataRecord(ActionId, GADataClass.GAAction));
			if (owner == null)
				//throw new Exception("Action is not related to any ga object");
				return false;   //returns false, action in object does not match any owner in gasuperclass
			//throwing an exception stops user from accessing the other workitems

			//get roleid from galists
            // Tor 20170313 Responsible changed from Role ER to Title TITL  int categoryId = BusinessLayer.ListCategory.GetListCategoryRowIdByName("ER");  //TODO replace with value from config
            int categoryId = BusinessLayer.ListCategory.GetListCategoryRowIdByName("TITL");  //TODO replace with value from config
            // Tor 20170313 Responsible changed from Role ER to Title TITL int roleId = BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", Role);
            int roleId = BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", Role);

			bool isRoleMember = false;
			while (owner != null) 
			{
				EmploymentDS ds = Employment.GetEmploymentsByOwnerDateAndRoleId(owner.RowId, owner.DataClass, System.DateTime.Now, roleId);
				if (ds.GAEmployment.Rows.Count != 0) 
				{
					//found records where the action for this workitem has correct rolemembers;
					//check is personnel is member of these roles at this level (context)
					foreach (EmploymentDS.GAEmploymentRow empRow in ds.GAEmployment) 
					{
						if (empRow.Personnel == PersonnelId) 
						{
							//found match
							isRoleMember = true;
						}
					}
					//role has been found in action path. leave loop
					break;
				} 
				else
				{
					//check for next level
					owner = DataClassRelations.GetOwner(owner);
				}

			}
			return isRoleMember;
		}

		private static string getWorkitemUniqueId(openwfe.workitem.Header workitemHeader) 
		{
			return workitemHeader.flowExpressionId.workflowInstanceId + "-" + workitemHeader.flowExpressionId.expressionId;
		}

		public static openwfe.workitem.InFlowWorkitem GetWorkItemById(string WorkitemId)
		{
			openwfe.rest.worklist.WorkSession ws = new openwfe.rest.worklist.WorkSession(WorkSessionServerAddress, WorkSessionServerPort, OWFEUserName, OWFEPassword);
			
			//find workitem
			System.Collections.IList headers = ws.GetHeaders(GAUserStoreName);
			openwfe.workitem.FlowExpressionId itemId = null;
			foreach (openwfe.workitem.Header h in headers)
			{
				if (getWorkitemUniqueId(h) == WorkitemId)
				{
					itemId = h.flowExpressionId;
					break; //found, leave loop
				}					
			}		
			openwfe.workitem.InFlowWorkitem wi = ws.GetWorkitem(GAUserStoreName, itemId);
			ws.Close();
			return wi;
		}

        public static openwfe.workitem.InFlowWorkitem GetWorkItemByHeader(Header header)
        {
            openwfe.rest.worklist.WorkSession ws = new openwfe.rest.worklist.WorkSession(WorkSessionServerAddress, WorkSessionServerPort, OWFEUserName, OWFEPassword);

            openwfe.workitem.FlowExpressionId itemId = header.flowExpressionId;
            openwfe.workitem.InFlowWorkitem wi = ws.GetWorkitem(GAUserStoreName, itemId);
            ws.Close();
            return wi;
        }

        public static openwfe.workitem.InFlowWorkitem GetWorkItemByFlowExpressionId(openwfe.workitem.FlowExpressionId flowExpression)
        {
            openwfe.rest.worklist.WorkSession ws = new openwfe.rest.worklist.WorkSession(WorkSessionServerAddress, WorkSessionServerPort, OWFEUserName, OWFEPassword);
            openwfe.workitem.InFlowWorkitem wi = ws.GetWorkitem(GAUserStoreName, flowExpression);
            ws.Close();
            return wi;
        }

        public static WorkitemDS GetWorkItemDataSetById(Header header)
        {
            return GetWorkItemDataSetById(header.flowExpressionId);
        }


        public static WorkitemDS GetWorkItemDataSetById(openwfe.workitem.FlowExpressionId flowExpression) 
		{          
            openwfe.workitem.InFlowWorkitem wi = GetWorkItemByFlowExpressionId(flowExpression); //GetWorkItemByHeader(header);
            Workitem workitem = new Workitem();
            int actionId = int.Parse(wi.attributes[new StringAttribute("__gaactionid__")].ToString());
            GADataRecord owner = new GADataRecord(actionId, GADataClass.GAAction);

            WorkitemDS ds = (WorkitemDS)workitem.GetNewRecord(owner);  //new WorkitemDS();
            WorkitemDS.GAWorkitemRow row = (WorkitemDS.GAWorkitemRow)ds.GAWorkitem.Rows[0];

			row.GAParticipant = wi.attributes[new StringAttribute("gaparticipant")].ToString();

            row.ActionRowId = actionId;
            // Tor 20160321 Get VerticalListsRowId from Action owner
            int verticalListsRowId = GASystem.DataAccess.DataAccess.getVerticalFromActionOwner("GAAction", actionId);
            if (verticalListsRowId != 0) row.MasterRowId = verticalListsRowId;
			
			row.WorkitemIdentifier = wi.lastExpressionId.workflowInstanceId + "-" +  wi.lastExpressionId.expressionId;
		
			row.WorkflowURL = wi.lastExpressionId.workflowDefinitionUrl;
		
			row.WorkflowName = wi.lastExpressionId.workflowDefinitionName;
			
			row.WfInstanceId = wi.lastExpressionId.workflowInstanceId;
			row.ExpressionId = wi.lastExpressionId.expressionId;
		
			try 
			{
               //subject field in database has restricted size, cut length if larger then max length
                GASystem.AppUtils.FieldDescription fdsubject = GASystem.AppUtils.FieldDefintion.GetFieldDescription("subject", GADataClass.GAWorkitem.ToString());
                string subjectText = wi.attributes[new openwfe.workitem.StringAttribute("__subject__")].ToString().Trim();
                if (fdsubject.DataLength > 0 && fdsubject.DataLength < subjectText.Length)
                    subjectText = subjectText.Substring(0, fdsubject.DataLength);
                row.Subject = subjectText;
			} 
			catch 
			{
				row.Subject = "(no subject)";
			}
			try 
			{
				row.Notes = wi.attributes[new openwfe.workitem.StringAttribute("__notes__")].ToString();
			} 
			catch 
			{
				row.Notes = "(no notes)";
			}
			try 
			{
				row.ActorsReply = wi.attributes[new openwfe.workitem.StringAttribute("actorsreply")].ToString();
			} 
			catch 
			{
				row.ActorsReply = string.Empty;
			}
			try 
			{
				row.ReplyResult = wi.attributes[new openwfe.workitem.StringAttribute("__result__")].ToString();
			} 
			catch 
			{
				row.ReplyResult = "";
			}
			try 
			{
				row.GAActionInitiator = wi.attributes[new openwfe.workitem.StringAttribute("GAActionInitiator")].ToString();
			} 
			catch 
			{
				row.GAActionInitiator = "";
			}
			try 
			{
				row.GAActionResponsible = wi.attributes[new openwfe.workitem.StringAttribute("GAActionResponsible")].ToString();
			} 
			catch 
			{
				row.GAActionResponsible = "";
			}
            
            try 
			{
                row.TextFree1 = Enum.Parse(typeof(Workitem.WorkitemType),  wi.attributes[new openwfe.workitem.StringAttribute(WORKITEMTYPE)].ToString(), true).ToString();
			} 
			catch 
			{
                row.TextFree1 = WorkitemType.Default.ToString();
			}
            if (row.TextFree1 == WorkitemType.Remedial.ToString())
                row.TextFree2 = AcknowledgementStatus.AwaitingAcknowledgement.ToString();
            else
                row.TextFree2 = AcknowledgementStatus.NoAcknowledgementNeeded.ToString();

            if (!row.IsIntFree3Null())
                row.Extra5 = CreateCombinedUserAndRoleAssignment( ";" + row.IntFree3.ToString() + ";", string.Empty);

			row.WorkitemStatus = WorkitemStatus.Active.ToString();

            //get user participant for workitems
            GASystem.AppUtils.FieldDescription fdextra1 = GASystem.AppUtils.FieldDefintion.GetFieldDescription("extra1", GADataClass.GAWorkitem.ToString());
            int extra1Length = fdextra1.DataLength;
            string extra1Text = string.Empty;
       
            ArrayList personnels = GetPersonnelRowIdForGAParticipants(row.GAParticipant.ToString());
            if (personnels.Count > 0)
                extra1Text = ";";
            foreach (int personnelId in personnels)
            {
                if (extra1Text.Length + personnelId.ToString().Length + 1 < extra1Length)
                    extra1Text = extra1Text + personnelId.ToString() + ";";
                else
                    break;  //there is not enough room in extra1 to hold all participants break and assign tasks to currently found participants
            }
            if (extra1Text != string.Empty)
                row.Extra1 = extra1Text;

            //get role participant for workitems
            string extra3Text = string.Empty;
           
            ArrayList roleIds = GetRoleIdForGAParticipants(row.GAParticipant.ToString());
            if (roleIds.Count > 0)
                extra3Text = ";";
            foreach (int roleId in roleIds)
            {
                if (extra3Text.Length + roleId.ToString().Length + 1 < extra1Length)
                    extra3Text = extra3Text + roleId.ToString() + ";";
                else
                    break;  //there is not enough room in extra3 to hold all participants break and assign tasks to currently found participants
            }
            if (extra3Text != string.Empty)
                row.Extra3 = extra3Text;

			//ds.GAWorkitem.Rows.Add(row);
			return ds;
		}

		/// <summary>
		/// Get ActionId from workitem
		/// </summary>
		/// <param name="wi">openwfe.workitem.InFlowWorkitem</param>
		/// <returns>ActionId. Returns -1 if there are no actionid in the workitem.</returns>
		public static int GetActionIdForWorkItem(openwfe.workitem.InFlowWorkitem wi)
		{
			//TODO replace this with a gaexception.workitemactionidnotfoundexeption or similar
			int actionId = -1;
			try
			{
				actionId = Convert.ToInt32(wi.attributes[new StringAttribute("__gaactionid__")].ToString());
			} catch {}
			return actionId;
		}
	
		public static int GetActionIdForWorkItemId(string WorkitemId)
		{
			return GetActionIdForWorkItem(GetWorkItemById(WorkitemId));
		}

		/// <summary>
		/// get gaparticipant from workitem
		/// </summary>
		/// <param name="wi"></param>
		/// <returns></returns>
		public static string GetGAParticipantForWorkItem(openwfe.workitem.InFlowWorkitem wi)
		{
			//TODO replace this with a gaexception.workitemactionidnotfoundexeption or similar
			//TODO expand participants to take into account references to other fields
			string participants = string.Empty;
			try
			{
				participants = wi.attributes[new StringAttribute("gaparticipant")].ToString();
			} 
			catch {}
			return participants;
		}

		/// <summary>
		/// set gaparticipant from workitem
		/// </summary>
		/// <param name="wi"></param>
		/// <returns></returns>
		
		public static void SetGAParticipantForWorkitem(openwfe.workitem.InFlowWorkitem wi, string NewGAParticipant) 
		{
			StringAttribute participantKey = new StringAttribute("gaparticipant");
			StringAttribute participantKeyValue = new StringAttribute(NewGAParticipant);
			
			wi.attributes[participantKey] = participantKeyValue;
		}

		public static void DoProceedWorkFlow(string WorkitemId)
		{
			openwfe.rest.worklist.WorkSession ws = new openwfe.rest.worklist.WorkSession(WorkSessionServerAddress, WorkSessionServerPort, OWFEUserName, OWFEPassword);
			
			//find workitem
			System.Collections.IList headers = ws.GetHeaders(GAUserStoreName);
			openwfe.workitem.FlowExpressionId itemId = null;
			foreach (openwfe.workitem.Header h in headers)
			{
				if (getWorkitemUniqueId(h) == WorkitemId)
				{
					itemId = h.flowExpressionId;
					break; //found, leave loop
				}
			}
		
			openwfe.workitem.InFlowWorkitem wi = ws.GetAndLockWorkitem(GAUserStoreName, itemId);
			ws.ForwardWorkitem(GAUserStoreName, wi);
			ws.Close();
		}

		public static void ProceedWorkFlow(int WorkitemId, string Result, string msg) 
		{
            Workitem bc = (Workitem)Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);
			WorkitemDS wds = (WorkitemDS) bc.GetByRowId(WorkitemId);
			if (wds.GAWorkitem[0].IsWorkitemIdentifierNull()) 
			{
				wds.GAWorkitem[0].WorkitemStatus = WorkitemStatus.Completed.ToString();   //single task, no workflow contected to item
				//complete action aswell
				Action.SetActionCompleted(wds.GAWorkitem[0].ActionRowId);
			}
			else
				wds.GAWorkitem[0].WorkitemStatus = WorkitemStatus.ProceedPending.ToString();
			wds.GAWorkitem[0].ReplyResult = Result;
            wds.GAWorkitem[0].DateTimeFree3 = System.DateTime.Now;
			try 
			{
				//add userid of person completing the workitem
				wds.GAWorkitem[0].IntFree2 = User.GetPersonnelIdByLogonId(AppUtils.GAUsers.GetUserId());
				//wds.GAWorkitem[0].HistoryLog += "<br/>" + System.DateTime.Now.ToLongDateString() + Result + " by " +  AppUtils.GAUsers.GetUserId();
			} 
			catch 
			{
				//silently ingnore if not found
				//TODO: log 
			}

			bc.CommitDataSet(wds);

            //write message to log/notes
            string msgHeader = string.Format(AppUtils.Localization.GetGuiElementText("WorkitemResultBy"), AppUtils.Localization.GetGuiElementText(Result), User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId()));
            bc.addNotes(msg, msgHeader, WorkitemId);
		}

		public static void ProceedPendingWorkitems(List<openwfe.workitem.FlowExpressionId> flowExpressions) 
		{
			//get pending workitems
            WorkitemDS wds = GetAllPendingWorkitems();

            //get all workitems headers
            openwfe.rest.worklist.WorkSession ws = new openwfe.rest.worklist.WorkSession(WorkSessionServerAddress, WorkSessionServerPort, OWFEUserName, OWFEPassword);
           
            //System.Collections.IList headers = ws.GetHeaders(GAUserStoreName);  //test
            List<string> proceededWorkitemsByIdenitifer = new List<string>();

			foreach (WorkitemDS.GAWorkitemRow row in wds.GAWorkitem) 
			{
                //BusinessClass bc = Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);
                try 
				    {
                        //if multiple workitems have the same workitem identfier, try to proceed only one
                        if (!proceededWorkitemsByIdenitifer.Contains(row.WorkitemIdentifier))
                        {

                            DoProceedWorkFlow(row.WorkitemIdentifier, row.ReplyResult, flowExpressions, ws);
                            proceededWorkitemsByIdenitifer.Add(row.WorkitemIdentifier);
                            BusinessClass bc = Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);
                            WorkitemDS ds = (WorkitemDS)bc.GetByRowId(row.WorkitemRowId);
                            ds.GAWorkitem[0].WorkitemStatus = WorkitemStatus.Completed.ToString();
                            bc.CommitDataSet(ds);
                        }
                        else
                        {
                            //workitem was proceeded on first run, set as proceeded
                            BusinessClass bc = Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);
                            WorkitemDS ds = (WorkitemDS)bc.GetByRowId(row.WorkitemRowId);
                            ds.GAWorkitem[0].WorkitemStatus = WorkitemStatus.Completed.ToString();
                            bc.CommitDataSet(ds);
                        }
				    } 
				    catch (Exception ex)
				    {
                        // Tor 20190101 add more values to error message
                        string workitemRowId = string.IsNullOrEmpty(row.WorkitemRowId.ToString()) ? "" : " where WorkitemRowId=" + row.WorkitemRowId.ToString();
                    // _logger.Error("Error proceeding workitem in gaworkitem table", ex);
                        if (workitemRowId != "")
                    {
                        _logger.Error("Error proceeding workitem in gaworkitem table where workitemRowId=" + workitemRowId + ". Workitemstatus updated to Completed.", ex);
                        // Tor 20190922 record in OWFE with row.WorkitemIdentifier probably not found. Set workitem status to Completed.
                        BusinessClass bc = Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);
                        WorkitemDS ds = (WorkitemDS)bc.GetByRowId(row.WorkitemRowId);
                        ds.GAWorkitem[0].WorkitemStatus = WorkitemStatus.Completed.ToString();
                        bc.CommitDataSet(ds);
                    }
                    else
                    {
                        _logger.Error("Error proceeding workitem in gaworkitem table. WorkitemRowId is null or empty ", ex);
                    }
                }

            }

            ws.Close();
		}

        public static void DoProceedWorkFlow(string WorkitemId, string Result, List<openwfe.workitem.FlowExpressionId> flowExpressions,  openwfe.rest.worklist.WorkSession ws)
		{
            //find workitem
            openwfe.workitem.FlowExpressionId itemId = null;

            //System.Collections.IList headers = ws.GetHeaders(GAUserStoreName);  //test
            //foreach (openwfe.workitem.Header h in headers)
            //{
            //    if (getWorkitemUniqueId(h) == WorkitemId)
            //    {
            //        itemId = h.flowExpressionId;
            //        break; //found, leave loop
            //    }

            //}

            //openwfe.workitem.FlowExpressionId itemtestId = null;
            foreach (openwfe.workitem.FlowExpressionId flowExpression in flowExpressions)
            {
                if (flowExpression.workflowInstanceId + "-" + flowExpression.expressionId == WorkitemId)
                {
                    //itemtestId = flowExpression;
                    itemId = flowExpression;
                    break; //found, leave loop
                }

            }

            //if no workitem is found, throw error. TODO logit!!
            if (itemId == null)
            // Tor 20190922 set workitemstatus = completed
            {
                string sql=@"Select w.WorkitemRowId /*update GAWorkitem set WorkitemStatus = 'Completed'*/
from GAWorkitem w inner join GASuperClass s on s.MemberClass = 'GAAction' and s.MemberClassRowId = w.ActionRowId
where w.WorkitemIdentifier like '1567667295156-0.0.31.1.0' + '%' and w.WorkitemStatus = 'ProceedPending'
                    ";
                throw new Exception("workitem " + WorkitemId + " was not found in worklist headers");
            }
            //getting info for workitem, then using this info to get and lock workitem. Are getting intermittant problems
            //with locking when trying to read and lock using the filename generated 
            openwfe.workitem.InFlowWorkitem wiInfo = ws.GetWorkitem(GAUserStoreName, itemId);

            openwfe.workitem.InFlowWorkitem wi = ws.GetAndLockWorkitem(GAUserStoreName, wiInfo.lastExpressionId);
			StringAttribute attResultKey = new StringAttribute("__result__");
			StringAttribute attResultValue = new StringAttribute(Result);
			
			wi.attributes[attResultKey] = attResultValue;
			//ws.ReleaseWorkitem(GAUserStoreName, wi);
			
            //ws.SaveWorkitem(GAUserStoreName, wi);
            try
            {
                ws.SaveWorkitem(GAUserStoreName, wi);
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Message.IndexOf("500") > -1)   //owfe bug workaround, if the rest server has been resting for too long 
                {									  //it might return a 500 error. Try starting again
                    ws.SaveWorkitem(GAUserStoreName, wi);
                }
                else
                {
                    throw ex;
                }
            }

			try 
			{
				ws.ForwardWorkitem(GAUserStoreName, wi);
			} 
			catch (System.Net.WebException ex) 
			{
				if (ex.Message.IndexOf("500") > -1)   //owfe bug workaround, if the rest server has been resting for too long 
				{									  //it might return a 500 error. Try starting again
					ws.ForwardWorkitem(GAUserStoreName, wi);
				} 
				else 
				{
					throw ex;
				}
			}
		}

        public static void DelegateToPerson(int WorkitemRowId, int personnelRowId, string Msg)
        {
            string delegateTo = string.Empty;
            string NewParticipant = string.Empty;
            BusinessLayer.Personnel pbc = new Personnel();
            try 
            {
                
                delegateTo = pbc.getPersonnelFullName(personnelRowId);
            } 
            catch (GAExceptions.GADataAccessException ex) 
            {
                 throw new GAExceptions.GADataAccessException("Could not delegate workitem. Person not found");
            }
            try
            {
                NewParticipant = createUserIdentifier(User.GetLogonIdByPersonnelRowId(personnelRowId));
            }
            catch (GAExceptions.GADataAccessException ex)
            {
                throw new GAExceptions.GADataAccessException("Could not delegate workitem. "+ delegateTo + " does not have a login");
            }

            Workitem wbc = new Workitem();
            WorkitemDS ds = (WorkitemDS)wbc.GetByRowId(WorkitemRowId);

            string historyLog = string.Empty;
            try
            {
                historyLog += string.Format(AppUtils.Localization.GetGuiElementText("delegatedtouserbyuser"), pbc.getPersonnelFullName(personnelRowId), User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId()));
            }
            catch (Exception ex)
            {
                historyLog +=   string.Format(AppUtils.Localization.GetGuiElementText("delegatedByUser"), AppUtils.GAUsers.GetUserId());
            }

            //save old assigments
            ds.GAWorkitem[0].Extra5 = CreateCombinedUserAndRoleAssignment(ds.GAWorkitem[0].IsExtra1Null() ? string.Empty : ds.GAWorkitem[0].Extra1, ds.GAWorkitem[0].IsExtra3Null() ? string.Empty : ds.GAWorkitem[0].Extra3);
         
            //ds.GAWorkitem[0].HistoryLog = historyLog;
            ds.GAWorkitem[0].GAParticipant = NewParticipant;
            ds.GAWorkitem[0].Extra1 = ";" + personnelRowId.ToString() + ";";

            //delegating to user, clear role delegations
            ds.GAWorkitem[0].Extra3 = string.Empty;

            //set acknowledgment status is workitem is remedial
            if (ds.GAWorkitem[0].TextFree1 == WorkitemType.Remedial.ToString())
                ds.GAWorkitem[0].TextFree2 = AcknowledgementStatus.AwaitingAcknowledgement.ToString();
            else
                ds.GAWorkitem[0].TextFree2 = AcknowledgementStatus.NoAcknowledgementNeeded.ToString();

            // Tor 20170422 Send eMail Notification when person is delegated to
            // Tor 20181030 string SMTPNotificationOnOff = new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPNotificationOnOff");
            // Tor 20181030 if (SMTPNotificationOnOff != null && SMTPNotificationOnOff.ToUpper() == "ON")
            // Tor 20181030 if ((new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPNotificationOnOff")).ToString().ToUpper() == "ON")

            if (IsSMTPNotificationOnOff())
            {
                ArrayList toPerson = new ArrayList();
                toPerson.Add((int)personnelRowId);
                SendParticipationEmail(toPerson, "person", "delegate", ds.GAWorkitem[0].WorkitemRowId, ds.GAWorkitem[0].Subject,Msg);
            }

            wbc.CommitDataSet(ds);

            wbc.addNotes(Msg, historyLog, WorkitemRowId);
        }

		public static void DelegateToRole(int workitemRowId, int roleId, string msg)
		{
            ListsDS ds = GASystem.BusinessLayer.Lists.GetListsByListsRowId(roleId);
			if (ds.GALists.Rows.Count > 0)
                DelegateToRole(workitemRowId, ds.GALists[0].GAListDescription.ToString(), roleId, msg);

			//TODO log error no user found
		}


		public static void DelegateToRole(int workitemRowId, string roleName, int roleId, string msg)
		{
            if (roleName == null || roleName == string.Empty || roleName == "-To be decided-")
                return;      //invalid roleid return silently

            string NewParticipant = createRoleIdentifier(roleName);
			
			Workitem wbc = new Workitem();
			WorkitemDS ds = (WorkitemDS)wbc.GetByRowId(workitemRowId);

            //save old assigments
            ds.GAWorkitem[0].Extra5 = CreateCombinedUserAndRoleAssignment(ds.GAWorkitem[0].IsExtra1Null() ? string.Empty : ds.GAWorkitem[0].Extra1, ds.GAWorkitem[0].IsExtra3Null() ? string.Empty : ds.GAWorkitem[0].Extra3);

            string historyLog = string.Format(AppUtils.Localization.GetGuiElementText("DelegatedToRoleByUser"), roleName,  User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId()));
			//ds.GAWorkitem[0].HistoryLog = historyLog;
			ds.GAWorkitem[0].GAParticipant = NewParticipant;
			//ds.GAWorkitem[0].Extra1 = ";" + GASystem.AppUtils.GAUtils.ConvertArrayToString(GetPersonnelRowIdForGAParticipantsAndActionId(ds.GAWorkitem[0].GAParticipant.ToString(), (int)ds.GAWorkitem[0].ActionRowId).ToArray(), ",") + ";";
            ds.GAWorkitem[0].Extra1 = string.Empty;
            ds.GAWorkitem[0].Extra3 = ";" + roleId + ";";

            //set acknowledgment status is workitem is remedial
            if (ds.GAWorkitem[0].TextFree1 == WorkitemType.Remedial.ToString())
                ds.GAWorkitem[0].TextFree2 = AcknowledgementStatus.AwaitingAcknowledgement.ToString();
            else
                ds.GAWorkitem[0].TextFree2 = AcknowledgementStatus.NoAcknowledgementNeeded.ToString();

            // Tor 20170422 Send eMail Notification when job title is delegated to
            if (IsSMTPNotificationOnOff())
            {
                // Tor 20190101 use new method for retrieving recipient user names
                //System.Collections.ArrayList recipients = getRecipients(roleId,workitemRowId, IsFilterOnVertical());
                System.Collections.ArrayList recipients=getRecipientsWithJobTitle(ds.GAWorkitem[0].Extra3,workitemRowId,IsFilterOnVertical());
                SendParticipationEmail(recipients, "JobTitle", "delegate", ds.GAWorkitem[0].WorkitemRowId, ds.GAWorkitem[0].Subject, msg);
            }

			wbc.CommitDataSet(ds);

            wbc.addNotes(msg, historyLog, workitemRowId);
		}

		
		public static void AddRoleParticipant(int workitemRowId, int roleId, string msg) 
		{
			ListsDS ds = GASystem.BusinessLayer.Lists.GetListsByListsRowId(roleId);
			if (ds.GALists.Rows.Count > 0)
                AddRoleParticipant(workitemRowId, ds.GALists[0].GAListDescription.ToString(), roleId, msg);

			//TODO log error
		}


		public static void AddRoleParticipant(int workitemRowId, string roleName, int roleId, string msg) 
		{
            if (roleName == null || roleName == string.Empty || roleName == "-To be decided-")
				return;      //invalid roleid return silently
			
			Workitem wbc = new Workitem();
			WorkitemDS ds = (WorkitemDS)wbc.GetByRowId(workitemRowId);

            string currentRoles = string.Empty;
            if (!ds.GAWorkitem[0].IsExtra4Null())
                currentRoles = ds.GAWorkitem[0].Extra4;

            //check if new role is already in rolelist
            if (currentRoles.Contains(";" + roleId.ToString() + ";"))
                return;

            //add new role
            if (currentRoles == string.Empty)
                currentRoles = ";";

            string historyLog = string.Format(AppUtils.Localization.GetGuiElementText("ParticipantRoleAddedBy"), roleName, User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId()));
            
            ds.GAWorkitem[0].Extra4 = currentRoles + roleId.ToString() + ";";

            if (IsSMTPNotificationOnOff())
            {
                // Tor 20190101 use new method for retrieving recipient user names
                //System.Collections.ArrayList recipients = getRecipients(roleId, workitemRowId, IsFilterOnVertical());
                System.Collections.ArrayList recipients=getRecipientsWithJobTitle(";" + roleId.ToString() + ";",workitemRowId,IsFilterOnVertical());
                SendParticipationEmail(recipients, "JobTitle", "add", ds.GAWorkitem[0].WorkitemRowId, ds.GAWorkitem[0].Subject, msg);
            }

			wbc.CommitDataSet(ds);
            wbc.addNotes(msg, historyLog, workitemRowId);
		}
	
        public static void addParticipantPerson(int WorkitemRowId, int personnelRowId, string Msg)
        {
            string delegateTo = string.Empty;
            string NewParticipant = string.Empty;
            BusinessLayer.Personnel pbc = new Personnel();
            try
            {

                delegateTo = pbc.getPersonnelFullName(personnelRowId);
            }
            catch (GAExceptions.GADataAccessException ex)
            {
                throw new GAExceptions.GADataAccessException("Could not add participant. Person not found");
            }
            try
            {
                NewParticipant = createUserIdentifier(User.GetLogonIdByPersonnelRowId(personnelRowId));
            }
            catch (GAExceptions.GADataAccessException ex)
            {
                throw new GAExceptions.GADataAccessException("Could add participant. " + delegateTo + " does not have a login");
            }

            Workitem wbc = new Workitem();
            WorkitemDS ds = (WorkitemDS)wbc.GetByRowId(WorkitemRowId);

            string historyLog = string.Empty;
           
            try
            {
                historyLog = string.Format(AppUtils.Localization.GetGuiElementText("ParticipantUserAddedByUser"), pbc.getPersonnelFullName(personnelRowId), User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId()));
            }
            catch (Exception ex)
            {
                historyLog = string.Format(AppUtils.Localization.GetGuiElementText("ParticipantAddedBy"), AppUtils.GAUsers.GetUserId());
            }
            
            ds.GAWorkitem[0].GAParticipant = NewParticipant;
            if (ds.GAWorkitem[0].IsExtra2Null() || ds.GAWorkitem[0].Extra2.Equals(string.Empty)) ds.GAWorkitem[0].Extra2 = ";";
            ds.GAWorkitem[0].Extra2 += personnelRowId.ToString() + ";";
            
            // Tor 20170422 Send eMail Notification when person is delegated to
            //string SMTPNotificationOnOff = new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPNotificationOnOff");
            //if (SMTPNotificationOnOff != null && SMTPNotificationOnOff.ToUpper() == "ON")
            //if ((new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPNotificationOnOff")).ToString().ToUpper() == "ON")
            if (IsSMTPNotificationOnOff())
            {
                ArrayList toPerson = new ArrayList();
                toPerson.Add((int)personnelRowId);
                SendParticipationEmail(toPerson, "person", "add", ds.GAWorkitem[0].WorkitemRowId, ds.GAWorkitem[0].Subject, Msg);
            }

            wbc.CommitDataSet(ds);
            wbc.addNotes(Msg, historyLog, WorkitemRowId);
        }

        /// <summary>
        /// get current acknowlegdment status for workitem
        /// </summary>
        /// <param name="workitemRowID"></param>
        /// <returns></returns>
        public static AcknowledgementStatus getWorkitemAcknowledgmentStatus(int workitemRowID)
        {
            BusinessClass wbc = RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS ds = (WorkitemDS)wbc.GetByRowId(workitemRowID);
            return getWorkitemAcknowledgmentStatus(ds.GAWorkitem[0]);
        }

        /// <summary>
        /// get current acknowlegdment status for workitem
        /// </summary>
        /// <param name="workitemRowID"></param>
        /// <returns></returns>
        public static AcknowledgementStatus getWorkitemAcknowledgmentStatus(WorkitemDS.GAWorkitemRow row)
        {
            AcknowledgementStatus ackStatus = AcknowledgementStatus.NoAcknowledgementNeeded; //default value for existing items

            if (row.IsTextFree2Null())
                return ackStatus;

            try
            {
                ackStatus = (AcknowledgementStatus)Enum.Parse(typeof(AcknowledgementStatus), row.TextFree2.ToString());
            } catch (ArgumentException ex) 
                {
                    ackStatus = AcknowledgementStatus.NoAcknowledgementNeeded;
            }
            return ackStatus;
        }

        /// <summary>
        /// get current added participants for workitem
        /// </summary>
        /// <param name="workitemRowID"></param>
        /// <returns></returns>
        public static string getAddedParticipants(int workitemRowID)
        {
            BusinessClass wbc = RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS ds = (WorkitemDS)wbc.GetByRowId(workitemRowID);
            return getAddedParticipants(ds.GAWorkitem[0]);
        }

        /// <summary>
        /// get current added participants for workitem
        /// </summary>
        /// <param name="workitemRowID"></param>
        /// <returns></returns>
        public static string getAddedParticipants(WorkitemDS.GAWorkitemRow row)
        {
            return row.Extra2;
        }

        /// <summary>
        /// get current added participant roles for workitem
        /// </summary>
        /// <param name="workitemRowID"></param>
        /// <returns></returns>
        public static string getAddedParticipantRoles(int workitemRowID)
        {
            BusinessClass wbc = RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS ds = (WorkitemDS)wbc.GetByRowId(workitemRowID);
            return getAddedParticipantRoles(ds.GAWorkitem[0]);
        }

        /// <summary>
        /// get current added participants for workitem
        /// </summary>
        /// <param name="workitemRowID"></param>
        /// <returns></returns>
        public static string getAddedParticipantRoles(WorkitemDS.GAWorkitemRow row)
        {
            return row.Extra4;
        }

        /// <summary>
        /// get if current workitem is active, ie workitemstatus = Active and not Workitem Type (TextFree1) not = WorkflowStart
        /// </summary>
        /// <param name="workitemRowID"></param>
        /// <returns></returns>
        public static bool getIfWorkitemIsActive(int workitemRowID)
        {
            BusinessClass wbc = RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS ds = (WorkitemDS)wbc.GetByRowId(workitemRowID);
            return getIfWorkitemIsActive(ds.GAWorkitem[0]);
        }
        /// <summary>
        /// get if current workitem is active, ie workitemstatus = Active and not Workitem Type (TextFree1) not = WorkflowStart
        /// </summary>
        /// <param name="workitemRowID"></param>
        /// <returns></returns>
        public static bool getIfWorkitemIsActive(WorkitemDS.GAWorkitemRow row)
        {
            return (row.WorkitemStatus.ToString().ToUpper() =="ACTIVE" && row.TextFree1.ToString().ToUpper() != "WORKFLOWSTART");
        }

        //TODO update these with calls to config
		public static string GAUserStoreName
		{
			get 
			{
				//return "Store.gauser"; 
                // Tor 20160127	return System.Configuration.ConfigurationManager.AppSettings.Get("OWFEGAUserStoreName");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEGAUserStoreName");
			}	
		}

		public static string WorkSessionServerAddress
		{
			get 
			{
				//return "localhost"; 
                // Tor 20160301 Cannot be stored in GALists with GAListValue SYS because the value is different on different sites
				return System.Configuration.ConfigurationManager.AppSettings.Get("OWFEWorkSessionServerAddress");
			}	
		}

		public static int WorkSessionServerPort
		{
			get 
			{
				//return 5080; 
                // Tor 20160127	return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("OWFEWorkSessionServerPort"));
                return int.Parse(new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEWorkSessionServerPort"));
            }	
		}

		public static string OWFEUserName
		{
			get 
			{
				//return "ga"; 
                // Tor 20160127	return System.Configuration.ConfigurationManager.AppSettings.Get("OWFEUserName");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEUserName");
            }	
		}

		public static string OWFEPassword 
		{
			get 
			{
				//return "bob"; 
                // Tor 20160127	return System.Configuration.ConfigurationManager.AppSettings.Get("OWFEPassword");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEPassword");
            }	
		}

		public static int OWFEWorkitemCacheTimeout
		{
			get 
			{
				//return "120"; 
                // Tor 20160127	return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("OWFEWorkitemCacheTimeout"));
                return int.Parse(new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEWorkitemCacheTimeout"));
            }	
		}

		public static string CoordinatorWorkitemRole 
		{
			get 
			{
// Tor 20160127				return System.Configuration.ConfigurationManager.AppSettings.Get("CoordinatorWorkitemRole");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("CoordinatorWorkitemRole");

			}
		}

		/// <summary>
		/// Creates an workflow user identifier of format {USERSUFFIX-Userid} from userid
		/// </summary>
		/// <param name="logonId"></param>
		/// <returns></returns>
		public static  string createUserIdentifier(string logonId) 
		{
			return "{" +  USER_SUFFIX + logonId + "}";
		}
	
		public static string createUserIdentifier(int PersonnelRowId) 
		{
			string logonId = User.GetLogonIdByPersonnelRowId(PersonnelRowId);
			return createUserIdentifier(logonId);
		}


		/// <summary>
		/// Creates an workflow role identifier of format {ROLESUFFIX-RoleName} from userid
		/// </summary>
		/// <param name="roleName"></param>
		/// <returns></returns>
		private static string createRoleIdentifier(string roleName) 
		{
			return  "{" +  ROLE_SUFFIX + roleName + "}";
		}

		public static string createRoleIdentifier(int RoleRowId) 
		{
			ListsDS ds = Lists.GetListsByListsRowId(RoleRowId);
			if (ds.GALists.Rows.Count == 0)
				throw new Exception("Role not found");
			
			return createRoleIdentifier(ds.GALists[0].GAListValue);
		}
		/// <summary>
		/// Get rolename from garoleparticipant string.
		/// </summary>
		/// <param name="roleIdenifier"></param>
		/// <returns></returns>
		public static string getRoleIdentiferNamePart(string roleIdenifier) 
		{
			//TODO add check on whether it is a role
			return roleIdenifier.Trim().Replace(ROLE_SUFFIX, string.Empty).Replace("{",string.Empty).Replace("}", string.Empty);
		}

		/// <summary>
		/// Get userid from gaparticipant string.
		/// </summary>
		/// <param name="roleIdenifier"></param>
		/// <returns></returns>
		private static string getUserIdentiferNamePart(string userIdentifier) 
		{
			return userIdentifier.Replace(USER_SUFFIX, string.Empty).Replace("{",string.Empty).Replace("}", string.Empty);
		}

		/// <summary>
		/// Get a list of all personnel assigned to and worikitem (by participants string and action id) 
		/// </summary>
		/// <param name="Participants"></param>
		/// <param name="ActionId"></param>
		/// <returns></returns>
		public static DataSet GetPersonnelForGAParticipantsAndActionId(string Participants, int ActionId) 
		{
			
			System.Collections.ArrayList rowids = GetPersonnelRowIdForGAParticipantsAndActionId(Participants, ActionId);
			//return personnel records

			return GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForRowIds(GASystem.DataModel.GADataClass.GAPersonnel, rowids);
		}

		public static System.Collections.ArrayList GetPersonnelRowIdForGAParticipantsAndActionId(string Participants, int ActionId) 
		{
			//get personnel ids for all users
			string[] GAParticipants = Participants.Split(';');
			System.Collections.ArrayList rowids = new System.Collections.ArrayList();
			foreach (string gaParticipant in GAParticipants) 
			{
                if (gaParticipant.Length > 0)
                {
                    if (isParticipantARole(gaParticipant))
                    {
                        GASystem.DataModel.EmploymentDS ds = Employment.GetEmploymentsByActionIdDateAndRoleId(ActionId, System.DateTime.Now, getRoleIdentiferNamePart(gaParticipant));
                        foreach (GASystem.DataModel.EmploymentDS.GAEmploymentRow row in ds.GAEmployment)
                        {
                            //TODO check that the person has an login.

                            rowids.Add(row.Personnel);
                        }
                    }
                    else  //a user, get rowid by logonid
                    {
                        GASystem.DataModel.UserDS ds = User.GetUserByLogonId(getUserIdentiferNamePart(gaParticipant.Trim()));
                        if (ds.GAUser.Rows.Count > 0)
                        {
                            rowids.Add(ds.GAUser[0].PersonnelRowId);
                        }
                    }
                }
			}

			//if no participants are found, return default participant
			if (rowids.Count == 0) 
			{
				GASystem.DataModel.EmploymentDS ds = Employment.GetEmploymentsByActionIdDateAndRoleId(ActionId, System.DateTime.Now,  getRoleIdentiferNamePart(CoordinatorWorkitemRole));
				foreach (GASystem.DataModel.EmploymentDS.GAEmploymentRow row in ds.GAEmployment) 
				{
					rowids.Add(row.Personnel);
				}
			}
            return rowids;
		}

        /// <summary>
        /// Get Flag personnel rowids for gaparticipant userids in owfe workitem participant attribute
        /// </summary>
        /// <param name="Participants">Workitem/workflow participant string</param>
        /// <returns></returns>
        public static System.Collections.ArrayList GetPersonnelRowIdForGAParticipants(string Participants)
        {
            //get personnel ids for all users
            // Tor 20141208 define split character - when applied from workflow, split = , when used from GAWorkitem, split = ;
            int i=0;
            bool isFromWorkitem = false; // data from GAWorkitem.Extra2 or Extra4 on format: ;rowid;rowid;, from workflowengine ,
            string[] GAParticipants ;
            if (Participants.IndexOf(';') > -1) 
            {
                GAParticipants = Participants.Split(';');
                isFromWorkitem=true;
            }
            else
            {
                GAParticipants = Participants.Split(',');
            }

            System.Collections.ArrayList rowids = new System.Collections.ArrayList();
            foreach (string gaParticipant in GAParticipants)
            {
                if (gaParticipant.Length > 0)
                {
                    if (isParticipantARole(gaParticipant))
                    {
                        //participant is a role, ignore when finding users
                        //GASystem.DataModel.EmploymentDS ds = Employment.GetEmploymentsByActionIdDateAndRoleId(ActionId, System.DateTime.Now, getRoleIdentiferNamePart(gaParticipant));
                        //foreach (GASystem.DataModel.EmploymentDS.GAEmploymentRow row in ds.GAEmployment)
                        //{
                        //    //TODO check that the person has an login.

                        //    rowids.Add(row.Personnel);
                        //}
                    }
                    else  //a user, get rowid by logonid
                    {
                        if (isFromWorkitem) // data from GAWorkitem.Extra2 or Extra4
                        {
                            if (gaParticipant != string.Empty)
                            {
                                try
                                {
                                    i = Convert.ToInt32(gaParticipant);
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine("Error when converting workitem added participating person: " + ex.Message);
                                    //TODO log error
                                }
                                rowids.Add(i);
                            }
                        }
                        else // data from workflow engine - convert Flag user id to PersonnelRowId
                        {
                            GASystem.DataModel.UserDS ds = User.GetUserByLogonId(getUserIdentiferNamePart(gaParticipant.Trim()));
                            if (ds.GAUser.Rows.Count > 0)
                            {
                                rowids.Add(ds.GAUser[0].PersonnelRowId);
                            }
                        }
                        
                    }
                }
            }
           return rowids;
        }

        /// <summary>
        /// Get Flag roles for gaparticipant in owfe workitem participant attribute
        /// </summary>
        /// <param name="Participants">Workitem/workflow participant string</param>
        /// <returns></returns>
        public static System.Collections.ArrayList GetRoleIdForGAParticipants(string Participants)
        {
            //get personnel ids for all users
            // Tor 20141208 define split character - when applied from workflow, split = , when used from GAWorkitem, split = ;
            int i = 0;
            bool isFromWorkitem = false; // data from GAWorkitem.Extra2 or Extra4 on format: ;rowid;rowid;, from workflowengine ,
            string[] GAParticipants;
            if (Participants.IndexOf(';') > -1)
            {
                GAParticipants = Participants.Split(';');
                isFromWorkitem = true;
            }
            else
            {
                GAParticipants = Participants.Split(',');
            }

            System.Collections.ArrayList rowids = new System.Collections.ArrayList();
            foreach (string gaParticipant in GAParticipants)
            {
                if (gaParticipant.Length > 0)
                {
                    if (isParticipantARole(gaParticipant))
                    {
                        //participant is a role, ignore when finding users
                        //get roleid
                        string roleName = getRoleIdentiferNamePart(gaParticipant);
                        // Tor 20170313 Responsible changed from Role ER to Title TITL  int roleId = BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", roleName);
                        int roleId = BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", roleName);

                        rowids.Add(roleId);
                    }
                    else  //a user, get rowid by logonid
                    {
                        if (isFromWorkitem)// data from GAWorkitem.Extra2 or Extra4
                        {
                            if (gaParticipant != string.Empty)
                            {
                                try
                                {
                                    i = Convert.ToInt32(gaParticipant);
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine("Error when converting workitem added participating role: " + ex.Message);
                                    //TODO log error
                                }
                                rowids.Add(i);
                            }
                        }
                        //a user, ignore
                    }
                }
            }

           return rowids;
        }



        //public static void LoadWorkitems() 
        //{
        //    //get headers
        ////	WorkitemDS headerDs = WorkitemDb.GetAllWorkitemHeaders();
        //    WorkSession ws = new openwfe.rest.worklist.WorkSession(Workitem.WorkSessionServerAddress, Workitem.WorkSessionServerPort, Workitem.OWFEUserName, Workitem.OWFEPassword);

        //    System.Collections.IList headers = ws.GetHeaders(Workitem.GAUserStoreName);
        //    foreach (Header h in headers)
        //    {
		
        //        try 
        //        {
        //            string workitemId = h.flowExpressionId.workflowInstanceId + "-" +  h.flowExpressionId.expressionId;
        //            //for each header check if it exists
        //            if (!WorkitemDb.hasWorkitemInTable(workitemId)) 
        //            {
        //                //add item if it does not exists
        //                WorkitemDS wds = GetWorkItemDataSetById(h);
        //                BusinessClass bc = new GenericBusinnesClass(GADataClass.GAWorkitem);
        //                int actionId = wds.GAWorkitem[0].ActionRowId;
        //                bc.CommitDataSet(wds, new GADataRecord(actionId, GADataClass.GAAction));
        //                System.Console.WriteLine("New workitem added: " + wds.GAWorkitem[0].WorkitemRowId );
                        
        //                //mark workflowstart workitem completed
        //                setWorkflowStartWorkitemCompleted(actionId);

        //            }
        //        } 
        //        catch (Exception ex)
        //        {
        //            System.Console.WriteLine("Error getting workitem: " + ex.Message); 
        //            //TODO log error
        //        }

        //    }
        //    ws.Close();
			
        //}


        //TEST JOF 20081103,  passing list of flowexpressions using expressions loaded from disk
        public static void LoadWorkitems(List<openwfe.workitem.FlowExpressionId> flowExpressions)
        {
            //get headers
            //	WorkitemDS headerDs = WorkitemDb.GetAllWorkitemHeaders();
            WorkSession ws = new openwfe.rest.worklist.WorkSession(Workitem.WorkSessionServerAddress, Workitem.WorkSessionServerPort, Workitem.OWFEUserName, Workitem.OWFEPassword);

            //System.Collections.IList headers = ws.GetHeaders(Workitem.GAUserStoreName);
            foreach (openwfe.workitem.FlowExpressionId flowExpression in flowExpressions)
            {
                //Tor 20200216 Added for debugging ir try fails
                string myWorkitemId = flowExpression.workflowInstanceId + "-" + flowExpression.expressionId;
                try
                {
                    string workitemId = flowExpression.workflowInstanceId + "-" + flowExpression.expressionId;
                    //for each header check if it exists
                    if (!WorkitemDb.hasWorkitemInTable(workitemId))
                    {
                        //add item if it does not exists
                        WorkitemDS wds = GetWorkItemDataSetById(flowExpression);
                        BusinessClass bc = new GenericBusinnesClass(GADataClass.GAWorkitem);
                        int actionId = wds.GAWorkitem[0].ActionRowId;
                        string RoleIds = string.Empty;
                        if (  !wds.GAWorkitem[0].IsExtra3Null())
                              RoleIds = wds.GAWorkitem[0].Extra3;
                        
                        WorkitemType workitemType = WorkitemType.Default;

                        try 
                        {
                            workitemType = (WorkitemType) Enum.Parse(typeof(WorkitemType), wds.GAWorkitem[0].TextFree1, true);
                        } 
                        catch (System.ArgumentException) 
                        {
                            workitemType = WorkitemType.Default;
                        }

                        //handle broadcast all
                        if (workitemType == WorkitemType.InfoBroadcastAll || workitemType == WorkitemType.RemedialBroadcastAll)
                        {
                            //convert types to standard
                            if (workitemType == WorkitemType.InfoBroadcastAll)
                                wds.GAWorkitem[0].TextFree1 = WorkitemType.Info.ToString();

                            if (workitemType == WorkitemType.RemedialBroadcastAll)
                                wds.GAWorkitem[0].TextFree1 = WorkitemType.Remedial.ToString();

                            //workitem is broadcast, set broadcast flag
                            wds.GAWorkitem[0].SwitchFree1 = true;

                            int roleId = 0;
                            //find first role only, there should be only one selected
                            if (RoleIds != string.Empty && RoleIds.Length > 2)
                            {
                                RoleIds = RoleIds.Substring(1, RoleIds.Length-2); //remove first and last ';'
                                string[] roles = RoleIds.Split(';');
                                if (roles.Length > 0)
                                {
                                    if (!int.TryParse(roles[0].ToString(),out roleId))
                                        roleId = 0;
                                }
                            }
                            if (roleId != 0)
                            {
                                EmploymentDS eds = Employment.GetEmploymentsByRoleAndDate(System.DateTime.Now, roleId);
                                List<GADataRecord> owners = new List<GADataRecord>();
                                List<GADataRecord> members = new List<GADataRecord>();
                                //only one assignment with given role
                                if (eds.GAEmployment.Rows.Count != 0)
                                {
                                    for (int t = 0; t < eds.GAEmployment.Rows.Count; t++)
                                    {
                                        EmploymentDS.GAEmploymentRow row = (EmploymentDS.GAEmploymentRow)eds.GAEmployment.Rows[t];
                                        GADataRecord owner = DataClassRelations.GetOwner(new GADataRecord(row.EmploymentRowId, GADataClass.GAEmployment));
                                        if (!owners.Contains(owner))
                                        {
                                            owners.Add(owner);
                                            WorkitemDS tempwds = new WorkitemDS();
                                            tempwds.GAWorkitem.Rows.Add(wds.GAWorkitem.Rows[0].ItemArray);
                                            bc.CommitDataSet(tempwds, new GADataRecord(actionId, GADataClass.GAAction));
                                            members.Add(new GADataRecord(((WorkitemDS.GAWorkitemRow)tempwds.GAWorkitem.Rows[0]).WorkitemRowId, GADataClass.GAWorkitem));
                                        }
                                    }
                                    for (int t = 0; t < members.Count; t++) 
	                                {
                                        if (owners.Count > t && owners[t] != null && members[t] != null) 
                                        {
                                            DataClassRelations.CreateDataClassRelation(owners[t].RowId, owners[t].DataClass, members[t].RowId, GADataClass.GAWorkitem);
                                        }
	                                }
                                }
                                else
                                {
                                    //there is no assignments of given role, use default handling
                                    bc.CommitDataSet(wds, new GADataRecord(actionId, GADataClass.GAAction));
                                }
                            }
                        }
                        //handle broadcast container
                        else 
                            if (workitemType == WorkitemType.InfoBroadcastContainer || workitemType == WorkitemType.RemedialBroadcastContainer)
                            {
                            }
                            else 
                            {
                                //handle default
                                bc.CommitDataSet(wds, new GADataRecord(actionId, GADataClass.GAAction));
                                System.Console.WriteLine("New workitem added: " + wds.GAWorkitem[0].WorkitemRowId);
                            }
                        //mark workflowstart workitem completed
                        setWorkflowStartWorkitemCompleted(actionId);
                    }
                }
                catch (Exception ex)
                {
                    // Tor 20200216 extended error message
                    //System.Console.WriteLine("Error getting workitem: " + ex.Message);
                    System.Console.WriteLine("Error getting workitem: WorkitemId: " + myWorkitemId + " Message: " + ex.Message);
                    //TODO log error
                }
            }
            ws.Close();
        }

        private static void setWorkflowStartWorkitemCompleted(int actionId)
        {
            Workitem wbc = (Workitem)RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS wds = (WorkitemDS)wbc.GetByOwner(new GADataRecord(actionId, GADataClass.GAAction), null);
            if (wds.GAWorkitem.Rows.Count == 0)
                return;

            bool workitemsUpdated = false;
            foreach (WorkitemDS.GAWorkitemRow row in wds.GAWorkitem.Rows)
            {
                if (row.TextFree1 == WorkitemType.WorkflowStart.ToString())
                {
                    row.WorkitemStatus = WorkitemStatus.Completed.ToString();
                    workitemsUpdated = true;
                }
            }

            if (workitemsUpdated)
                wbc.CommitDataSet(wds);
        }

		public static void SetWorkitemAssignments() 
		{
			WorkitemDS ds = GetAllWorkitems();
			Workitem wbc = new Workitem();
			foreach (WorkitemDS.GAWorkitemRow row in ds.GAWorkitem) 
			{
				try 
				{
					if (row.IsExtra1Null() || row.Extra1 == string.Empty) 
					{
						WorkitemDS dsSingle = (WorkitemDS)wbc.GetByRowId(row.WorkitemRowId);	
						dsSingle.GAWorkitem[0].Extra1 = ";" +  GASystem.AppUtils.GAUtils.ConvertArrayToString(GetPersonnelRowIdForGAParticipantsAndActionId(row.GAParticipant.ToString(), (int)row.ActionRowId).ToArray(), ";") + ";";
						wbc.CommitDataSet(dsSingle);
					}
				} 
				catch 
				{
				}
			}
		}

		public static bool HasEditPermission(string loginId, int WorkitemRowId) 
		{
			
			int personnelId = User.GetPersonnelIdByLogonId(loginId);
			WorkitemDS ds = WorkitemDb.GetWorkItemByWorkItemId(WorkitemRowId);
			if (ds.GAWorkitem.Rows.Count == 0)
				return false;   //no such workitem, return false
            if (ds.GAWorkitem[0].IsExtra1Null())
                return false;

			return ds.GAWorkitem[0].Extra1.IndexOf(";" + personnelId.ToString() +";") > -1;   //return true if personnelId exists in participant list
		}

        /// <summary>
        /// checks whether person have a direct user assignment to workitem?
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns></returns>
        public bool hasUserAssignmentToWorkitem(int personnelId, int rowId)
        {
            WorkitemDS ds = WorkitemDb.GetWorkItemByWorkItemId(rowId);
            if (ds.GAWorkitem.Rows.Count == 0)
                return false;   //no such workitem, return false

            if (ds.GAWorkitem[0].IsExtra1Null())
                return false;

            return ds.GAWorkitem[0].Extra1.IndexOf(";" + personnelId.ToString() + ";") > -1;   //return true if personnelId exists in participant list
        }

        /// <summary>
        /// checks whether person is a user participant to workitem?
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns></returns>
        public bool hasUserParticipantToWorkitem(int personnelId, int rowId)
        {
            WorkitemDS ds = WorkitemDb.GetWorkItemByWorkItemId(rowId);
            if (ds.GAWorkitem.Rows.Count == 0)
                return false;   //no such workitem, return false

            if (ds.GAWorkitem[0].IsExtra2Null())
                return false;

            return ds.GAWorkitem[0].Extra2.IndexOf(";" + personnelId.ToString() + ";") > -1;   //return true if personnelId exists in participant list
        }

        /// <summary>
        /// checks whether person has a role assigment to workitem
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns></returns>
        public bool hasRoleAssignmentToWorkitem(int personnelId, int rowId)
        {
           return WorkitemDb.hasRoleInWorkitem(personnelId, rowId);
        }

        /// <summary>
        /// checks whether person has a role partipantship to workitem
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns></returns>
        public bool hasRoleParticipantToWorkitem(int personnelId, int rowId)
        {
            return WorkitemDb.hasParticipantRoleInWorkitem(personnelId, rowId);
        }

        /// <summary>
        /// checks whether person has a flag assigment permission to workitem
        /// this uses the gaactionworkitemview class, true if user has a edit permissions in the 
        /// current context on gaactionworkitemview
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns></returns>
        public bool hasEditAssignmentToWorkitem(int personnelId, int rowId)
        {
            if (Security_new.IsGAAdministrator()) return true;

            //find the dataclass owning the workitem's action
            GADataRecord action = DataClassRelations.GetOwner(new GADataRecord(rowId, GADataClass.GAWorkitem));
            GADataRecord actionOwner = DataClassRelations.GetOwner(action);

            // Tor 201611 Security 20161122 
            GASecurityDb_new securityDb = new GASecurityDb_new(DataClass, null);
            // Tor Rollback 201611 Security 20161122 GASecurityDb_new securityDb = new GASecurityDb_new(action, DataClass, null);


            string updateRoles = securityDb.GetUpdateRolesForArcLink(actionOwner.DataClass, GADataClass.GAActionWorkitemView);
            
            //If the ALL role has readaccess to _dataClass, user has always access. Return true;
            if (updateRoles.IndexOf(";-1;") > -1)
                return true;

            string rolesIds = ConvertToSqlNumberList(updateRoles);

            return WorkitemDb.hasEditAssigmentToWorkitem(personnelId, action.RowId, rolesIds);

            //check arclink permissisions for gaactionworkitemsview for this datarecord
            //return Security.HasUpdatePermissionInArcLink(GADataClass.GAActionWorkitemView, actionOwner);
        }

        private static String ConvertToSqlNumberList(String semicolonseparatedList)
        {
            if (null == semicolonseparatedList || semicolonseparatedList.Length == 0)
                return "";

            System.Text.StringBuilder sqlArray = new System.Text.StringBuilder();

            foreach (String listElement in semicolonseparatedList.Split(new char[] { ';' }))
            {
                if (listElement == "" || listElement == null) //prohibit blank roles from being authorized
                    continue;
                sqlArray.Append(listElement).Append(", ");
            }
            sqlArray.Remove(sqlArray.Length - 2, 2);
            return sqlArray.ToString();
        }

        /// <summary>
        /// Checks whether a users has any form of assigment to a workitem
        /// </summary>
        /// <returns></returns>
        public bool hasAssigmentToWorkitem(int personnelId, int rowId)
        {
            try
            {
                if (this.hasUserAssignmentToWorkitem(personnelId, rowId))
                    return true;

                if (this.hasRoleAssignmentToWorkitem(personnelId, rowId))
                    return true;

                if (this.hasEditAssignmentToWorkitem(personnelId, rowId))
                    return true;

            }
            catch (Exception ex)
            {
                throw new GAExceptions.GAException(ex.Message);//TODO log;
            }
            return false;
        }


        public override bool HasPermissionToRecord(int RowId)
        {
            //check towards base permission check
            bool hasBaseAccess = base.HasPermissionToRecord (RowId);
            if (RowId == 0)
                return hasBaseAccess;    //new record. use only permissions from base 
			
            return hasBaseAccess && HasEditPermission(AppUtils.GAUsers.GetUserId(), RowId);
        }

		public static string GetWorkitemLogByAction(int ActionId)
		{
			GADataRecord owner = new GADataRecord(ActionId, GADataClass.GAAction);

			Workitem wbc = new Workitem();
			WorkitemDS ds = (WorkitemDS)wbc.GetByOwner(owner, null);

			string Log = string.Empty;
			foreach (WorkitemDS.GAWorkitemRow row in ds.GAWorkitem.Rows) 
			{
				Log += "<br/>" + AppUtils.Localization.GetGuiElementText("GAWorkitem") + ": " + row.Subject;
				if (!row.IsHistoryLogNull())
					Log += "<br/>" + row.HistoryLog;

				if (!row.IsMessageNull() && row.Message != string.Empty)
					Log += "<br/>" +   row.Message +"</br>";
			}
			return Log;
		}

        /// <summary>
        /// add a simple comment to the workitem
        /// </summary>
        /// <param name="notes"></param>
        /// <param name="headerText"></param>
        /// <param name="workitemId"></param>
        public void addNotes(string notes, int workitemId)
        {
            //get username
            string personnelName = User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId());
            string headerText = string.Format(AppUtils.Localization.GetGuiElementText("NotesByUser"),personnelName);
            addNotes(notes, headerText, workitemId);

        }
        /// <summary>
        /// Add notes/log to an existing workitems and its related action
        /// </summary>
        /// <param name="Notes">Notes to add</param>
        /// <param name="logonId">Current Users logonid</param>
        public void addNotes(string notes, string headerText, int workitemId)
        {
            //replace newlines with br
            notes = notes.Replace("\n", "<br/>");

            //get date
            string currentDateAndTime = System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToShortTimeString();

            //get dataclass
            WorkitemDS wds;
            try
            {
                wds = (WorkitemDS)this.GetByRowId(workitemId);
            }
            catch (Exception ex)
            {
                throw new GAExceptions.GADataAccessException("Could not find workitem", ex);
            }
            //get action id
            int actionId;
            ActionDS ads;
            BusinessClass abc = BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAction);
            try
            {
                actionId = ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).ActionRowId;
                ads = (ActionDS)abc.GetByRowId(actionId);
            }
            catch (Exception ex)
            {
                throw new GAExceptions.GADataAccessException("Could not get action related to workitem", ex);
            }

            //string workitemsNotesTemplate =
            //    "<p><div class=\"workitemsNotesHeader\" >{0}: {1}</div><div class=\"workitemnotesdetails\">{2}</div></p>";
            //string actionNotesTemplate =
            //    "<p><div class=\"workitemsNotesHeader\" >{0}: {1}</div><br/><div class=\"workitemsNotesHeader\" >Workitem subject: {3}</div><br/><div class=\"workitemnotesdetails\">{2}</div><p>";
            string workitemNotes = string.Format(AppUtils.Localization.GetGuiElementText("workitemsNotesTemplate"), currentDateAndTime, headerText, notes);
            string actionNotes = string.Format(AppUtils.Localization.GetGuiElementText("actionNotesTemplate"), currentDateAndTime, headerText, notes, ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Subject);
            //string workitemNotes = string.Format(workitemsNotesTemplate, currentDateAndTime, headerText, notes);
            //string actionNotes = string.Format(actionNotesTemplate, currentDateAndTime, headerText, notes, ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Subject);

            //save workitem
            if (!((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).IsHistoryLogNull())
                ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).HistoryLog += workitemNotes;
            else
                ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).HistoryLog = workitemNotes;

            this.CommitDataSet(wds);
            //save action
            if (!((ActionDS.GAActionRow)ads.GAAction.Rows[0]).IsnTextFree1Null())
                ((ActionDS.GAActionRow)ads.GAAction.Rows[0]).nTextFree1 += actionNotes;
            else
                ((ActionDS.GAActionRow)ads.GAAction.Rows[0]).nTextFree1 = actionNotes;
            abc.CommitDataSet(ads);
        }

        // Tor 20190123  Add Notes notification to responsible and action initiator
        public void addNotesNotification(string notes, int workitemId)
        {

            //EMNTWorkitemAddedNoteMessage 
            //<br>Message added to workitem by the user: <strong>{0}</strong><br>Action owner: <strong>{1}</strong><br>Reference: <strong>{2}</strong><br>Action Subject: <strong>{3}</strong><br>Action Description: <strong>{4}</strong><br>Added Note: <strong>{5}</strong>
            if (IsSMTPNotificationOnOff())
            {
                WorkitemDS wds;
                try
                {
                    wds = (WorkitemDS)this.GetByRowId(workitemId);
                }
                catch (Exception ex)
                {
                    throw new GAExceptions.GADataAccessException("Could not find workitem", ex);
                }
                //get owner info, eg action owner class name and reference id/or class object name, action.subject, action.description, user adding the note
                int actionId;
                ActionDS ads;
                BusinessClass abc = BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAction);
                try
                {
                    actionId = ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).ActionRowId;
                    ads = (ActionDS)abc.GetByRowId(actionId);
                }
                catch (Exception ex)
                {
                    throw new GAExceptions.GADataAccessException("Could not get action related to workitem", ex);
                }
                //create notification message
                string referenceId=((ActionDS.GAActionRow)ads.GAAction.Rows[0]).ActionReferenceId;
                string workflowDescription=((ActionDS.GAActionRow)ads.GAAction.Rows[0]).Description;
                string personnelName = User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId());
                string headerText = string.Format(AppUtils.Localization.GetGuiElementText("NotesByUser"), personnelName);
                //replace newlines with br
                notes = notes.Replace("\n", "<br/>");
                string currentDateAndTime = System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToShortTimeString();
                string subjectText = string.Format(AppUtils.Localization.GetGuiElementText("AddedNotesNotificationSubject")
                    , ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Subject);
                string messageText = string.Format(AppUtils.Localization.GetGuiElementText("AddedNotesNotificationBody")
                    ,referenceId,workflowDescription, currentDateAndTime, headerText, notes);
                //get initiator and responsible
                int responsibleRole = 0;
                int responsiblePerson = 0;
                int actionInitiator = 0;
//              GAWorkitem.extra1	Assigned to Person
//              GAWorkitem.extra3	Assigned to Role
//              GAWorkitem.ChangedBy	Assigned to Job Title
//              GAWorkitem.extra2	Added Participant
//              GAWorkitem.extra4	Added Participant Role
                // Tor 20190720 Add email notifications to added role(s) (extra4) and added participant(s)/person(s) (extra2)
                // Tor 20190720 Send email to persons
                System.Collections.ArrayList recipientPersons = new ArrayList(); // = getRecipientsWithJobTitle(ds.GAWorkitem[0].Extra3, workitemRowId, IsFilterOnVertical());
                // responsible person
                if (!((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).IsExtra1Null() 
                  && ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Extra1 != "")
                {
                    //responsiblePerson = Convert.ToInt32
                    //    (((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Extra1.Replace(";",""));
                    addElement2ExtraXArray(((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Extra1, ref recipientPersons);
                }
                // added persons
                if (!((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).IsExtra2Null()
                  && ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Extra2 != "")
                {
                    addElement2ExtraXArray(((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Extra2, ref recipientPersons);
                }
                // initiator
                if (!((ActionDS.GAActionRow)ads.GAAction.Rows[0]).IsRegisteredByPersonnelRowIdNull())
                {
                    actionInitiator = ((ActionDS.GAActionRow)ads.GAAction.Rows[0]).RegisteredByPersonnelRowId;
                    addElement2ExtraXArray(actionInitiator.ToString(), ref recipientPersons);
                    string x = actionInitiator.ToString();
                }
                if (recipientPersons.Count > 0)
                {
                    SendAddedNotesEmail(recipientPersons, "PERSON", subjectText, messageText);
                }

                System.Collections.ArrayList recipientRoles = new ArrayList(); // = getRecipientsWithJobTitle(ds.GAWorkitem[0].Extra3, workitemRowId, IsFilterOnVertical());

                // Roles
                if (!((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).IsExtra3Null()
                    && ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Extra3 != "")
                {
                    responsibleRole = Convert.ToInt32
                        (((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Extra3.Replace(";", ""));
                    addElement2ExtraXArray(((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Extra3, ref recipientRoles);
                }
                // added roles
                if (!((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).IsExtra4Null()
                    && ((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Extra4 != "")
                {
                    addElement2ExtraXArray(((WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0]).Extra4, ref recipientRoles);
                }

                //email address
                //send notification
                //if (responsibleRole != 0)
                //{
                //    System.Collections.ArrayList recipients = getRecipientsWithJobTitle
                //        (";"+responsibleRole.ToString()+";", workitemId, IsFilterOnVertical());
                //    if (recipients.Count > 0) 
                //        SendAddedNotesEmail(recipients, "JobTitle", subjectText, messageText);
                //}
                string semicolonRoles = string.Empty;
                if (recipientRoles.Count > 0)
                { 
                    semicolonRoles=";";
                    foreach (string x in recipientRoles)
                    {
                        semicolonRoles = semicolonRoles + x + ";";
                    }
                    System.Collections.ArrayList recipients = getRecipientsWithJobTitle(semicolonRoles, workitemId, IsFilterOnVertical());
                    if (recipients.Count > 0)
                        SendAddedNotesEmail(recipients, "JobTitle", subjectText, messageText);
                }
                // Tor 20190721 Replaced by code above
                //if (responsiblePerson != 0 || actionInitiator != 0)
                //{
                //    System.Collections.ArrayList recipient=new ArrayList(); // = getRecipientsWithJobTitle(ds.GAWorkitem[0].Extra3, workitemRowId, IsFilterOnVertical());
                //    recipient.Insert(0, responsiblePerson);
                //    SendAddedNotesEmail(recipient, "PERSON", subjectText, messageText);
                //}
                //if (actionInitiator != 0)
                //{
                //    System.Collections.ArrayList recipient=new ArrayList(); // = getRecipientsWithJobTitle(ds.GAWorkitem[0].Extra3, workitemRowId, IsFilterOnVertical());
                //    recipient.Insert(0, actionInitiator);
                //    SendAddedNotesEmail(recipient, "PERSON", subjectText, messageText);
                //}


            }
        }

        /// <summary>
        /// Set workitem to status acknowledge. The current logged on user is set as the acknowledger
        /// </summary>
        /// <param name="workitemId"></param>
        public static void AcknowledgeWorkitem(int workitemId)
        {
            int userid = 0;
            try {
                string logonId = AppUtils.GAUsers.GetUserId();
                userid =  User.GetPersonnelIdByLogonId(logonId);
            } 
            catch (Exception ex) {
                userid = -1;
            }

            Workitem wbc =  (Workitem) RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS wds =  (WorkitemDS) wbc.GetByRowId(workitemId);
            WorkitemDS.GAWorkitemRow row = (WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0];
            row.IntFree3 = userid;
            row.TextFree2 = AcknowledgementStatus.Acknowledged.ToString();

            wbc.CommitDataSet(wds);

            string historyLog = string.Format(AppUtils.Localization.GetGuiElementText("AcknowledgeWorkitem"), User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId()));
            wbc.addNotes(historyLog, workitemId);
        }

        ///// <summary>
        ///// Set workitme to status acknowledge. The current logged on user is set as the acknowledger
        ///// </summary>
        ///// <param name="workitemId"></param>
        //public static void AcknowledgeWorkitem(int workitemId)
        //{
        //    int userid = 0;
        //    try
        //    {
        //        string logonId = AppUtils.GAUsers.GetUserId();
        //        userid = User.GetPersonnelIdByLogonId(logonId);
        //    }
        //    catch (Exception ex)
        //    {
        //        userid = -1;
        //    }

        //    Workitem wbc = (Workitem)RecordsetFactory.Make(GADataClass.GAWorkitem);
        //    WorkitemDS wds = (WorkitemDS)wbc.GetByRowId(workitemId);
        //    WorkitemDS.GAWorkitemRow row = (WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0];
        //    row.IntFree3 = userid;
        //    row.TextFree2 = AcknowledgementStatus.Acknowledged.ToString();

        //    wbc.CommitDataSet(wds);

        //    string historyLog = string.Format(AppUtils.Localization.GetGuiElementText("AcknowledgeWorkitem"), User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId()));
        //    wbc.addNotes(historyLog, workitemId);


        //}

        /// <summary>
        /// Remove added participants in workitem. The current logged on user is set as the user who removed the added participants. Might add a list of the participants that were removed
        /// </summary>
        /// <param name="workitemId"></param>
        // Tor 20150106 add method for removing added participants
        public static void RemoveAddedParticipants(int workitemId)
        {
            int userid = 0;
            try
            {
                string logonId = AppUtils.GAUsers.GetUserId();
                userid = User.GetPersonnelIdByLogonId(logonId);
            }
            catch (Exception ex)
            {
                userid = -1;
            }

            Workitem wbc = (Workitem)RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS wds = (WorkitemDS)wbc.GetByRowId(workitemId);
            WorkitemDS.GAWorkitemRow row = (WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0];
            // create list with added participants and roles
            string addedParticipantsRemovedText = string.Empty;
            
            // Tor 20170422 Send eMail Notification when added person/role is removed
            //bool sendEmailNotification = false;
            ////string SMTPNotificationOnOff = new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPNotificationOnOff");
            ////if (SMTPNotificationOnOff != null && SMTPNotificationOnOff.ToUpper() == "ON") sendEmailNotification = true;
            //if (IsSMTPNotificationOnOff()) sendEmailNotification = true;

            if (!row.IsExtra2Null())
            // added person(s)
            {
                ArrayList personnels = GetPersonnelRowIdForGAParticipants(row.Extra2.ToString());
                if (personnels.Count > 0)
                {
                    if (personnels.Count > 1)
                    { 
                        addedParticipantsRemovedText += "\nPersons: "; 
                    }
                    else 
                    { 
                        addedParticipantsRemovedText += "\nPerson: "; 
                    }
                    BusinessLayer.Personnel pbc = new Personnel();
                    foreach (int personnelId in personnels)
                    {
                        addedParticipantsRemovedText += pbc.getPersonnelFullName(personnelId) + ", ";
                    }
                    // Tor 20170422 
                    if (IsSMTPNotificationOnOff()) SendParticipationEmail(personnels, "person", "remove", row.WorkitemRowId, row.Subject, "");
                }
            }
            
            if (!row.IsExtra4Null())
            {
                ArrayList roleIds = GetRoleIdForGAParticipants(row.Extra4.ToString());
                if (roleIds.Count > 0)
                {
                    if (roleIds.Count > 1)
                    {
                        addedParticipantsRemovedText += "\nRoles: ";
                    }
                    else
                    {
                        addedParticipantsRemovedText += "\nRole: ";
                    }
                    foreach (int roleId in roleIds)
                    {
                        addedParticipantsRemovedText += GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(roleId) + ", ";
                    }
                    // Tor 20170422
                    //if (IsSMTPNotificationOnOff()) SendParticipationEmail(roleIds, "JobTitle", "remove", row.WorkitemRowId, row.Subject, "");
                    if (IsSMTPNotificationOnOff())
                    {
                        // Tor 20190101 use new method for retrieving recipient user names
                        string myRoles=";";
                        foreach (int roleId in roleIds)
                        {
                            myRoles=myRoles+roleId.ToString()+";";
                        }
                        System.Collections.ArrayList recipients=getRecipientsWithJobTitle(myRoles,row.WorkitemRowId,IsFilterOnVertical());
                        SendParticipationEmail(recipients, "JobTitle", "remove", row.WorkitemRowId, row.Subject, "");

                        //foreach (int roleId in roleIds)
                        //{
                        //    System.Collections.ArrayList recipients = getRecipients(roleId, row.WorkitemRowId, IsFilterOnVertical());
                        //    SendParticipationEmail(recipients, "JobTitle", "remove", row.WorkitemRowId, row.Subject, "");
                        //}
                    }

                }
            }
            row.IntFree3 = userid;
            //row.Extra2 = string.Empty; //remove added Participant
            //row.Extra4 = string.Empty; //remove added Participant role
            row.Extra2 = null;
            row.Extra4 = null;

            wbc.CommitDataSet(wds);
            // get removed roles and persons;
            addedParticipantsRemovedText += "\n";
            string historyLog = string.Format(AppUtils.Localization.GetGuiElementText("WorkitemRemovedAddedParticipants"), addedParticipantsRemovedText, User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId()));
            wbc.addNotes(historyLog, workitemId);
//            wbc.CommitDataSet(wds);
        }

        // Tor 20190131 Send eMail notification when Notes are added to workitem.
        /// <summary>
        /// send email notification, parameters to, toType (person or jobTitle), action (remove, add or delegate), workitemRowId, workitem subject. 
        /// </summary>
        /// <param name="workitemId"></param>
        private static void SendAddedNotesEmail(System.Collections.ArrayList to, string toType, string subject, string body)
        {
            System.Collections.ArrayList toEmailAddresses=new ArrayList();
            string eMailAddress = string.Empty;
            if (toType.ToUpper() == "PERSON")
            {
                // Tor 20190721 added loop for handling all items in arraylist "to" 
                foreach (string x in to)
                {
//                    eMailAddress = Personnel.GetPersonnelEmailAddress(Convert.ToInt32(to[0]));
                    eMailAddress = Personnel.GetPersonnelEmailAddress(Convert.ToInt32(x));
                    if (eMailAddress != string.Empty)
                    {
                        eMailAddress = GASystem.BusinessLayer.FlagSMTP.cleanupEmailAddress(eMailAddress);
                        GASystem.BusinessLayer.FlagSMTP.AddElementToArrayIfNotAlreadyThere(ref toEmailAddresses, eMailAddress);
                    }
                }
            }
            else
                if (toType.ToUpper() == "JOBTITLE")
                {
                    foreach (string DNNUserName in to)
                    {
                        eMailAddress = PersonnelDb.GetPersonnelEmailAddresByLogonId(DNNUserName);
                        if (eMailAddress != string.Empty)
                        {
                            eMailAddress = GASystem.BusinessLayer.FlagSMTP.cleanupEmailAddress(eMailAddress);
                            GASystem.BusinessLayer.FlagSMTP.AddElementToArrayIfNotAlreadyThere(ref toEmailAddresses, eMailAddress);
                        }
                    }
                }
            if (toEmailAddresses.Count > 0)
            {
                GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(toEmailAddresses, null, null, subject, body, true);
            }
        }

            // Tor 20170422 Send eMail notification when particiant is delegated to, added, or particiant is removed.
        /// <summary>
        /// send email notification, parameters to, toType (person or jobTitle), action (remove, add or delegate), workitemRowId, workitem subject. 
        /// </summary>
        /// <param name="workitemId"></param>
        private static void SendParticipationEmail(ArrayList to, string toType, string action, int workitemRowId, string subject, string messageFromUser)
        {
            string eMailAddress = string.Empty;
            ArrayList toEmailAddresses = new ArrayList();
            if (toType.ToUpper() == "PERSON")
            {
                foreach (int personnelRowId in to)
                {
                    eMailAddress = Personnel.GetPersonnelEmailAddress(personnelRowId);
                    if (eMailAddress != string.Empty)
                    {
                        eMailAddress = GASystem.BusinessLayer.FlagSMTP.cleanupEmailAddress(eMailAddress);
                        GASystem.BusinessLayer.FlagSMTP.AddElementToArrayIfNotAlreadyThere(ref toEmailAddresses, eMailAddress);
                    }
                }
            }
            else
                if (toType.ToUpper() == "JOBTITLE")
                {
                    foreach (string DNNUserName in to)
                    // Tor 20181031 Get receivers with job title from workitem and upwards in hierarchy
                    //{
                    //    System.Collections.ArrayList recipients = getRecipients(DNNUserName, workitemRowId, IsFilterOnVertical());
                    //    foreach (string recipient in recipients)
                    //    {
                    //        if (recipient != string.Empty)
                            {

                    eMailAddress = PersonnelDb.GetPersonnelEmailAddresByLogonId(DNNUserName);
                                if (eMailAddress != string.Empty)
                                {
                                    eMailAddress = GASystem.BusinessLayer.FlagSMTP.cleanupEmailAddress(eMailAddress);
                                    GASystem.BusinessLayer.FlagSMTP.AddElementToArrayIfNotAlreadyThere(ref toEmailAddresses, eMailAddress);
                                }
                            }
                        //}

                    //}

                }

            string MailSubjectTail = string.Empty;
            if (action.ToUpper() == "DELEGATE") MailSubjectTail = "delegated to you";
            else
                if (action.ToUpper() == "ADD") MailSubjectTail = "added to your workitem list";
                else
                    if (action.ToUpper() == "REMOVE") MailSubjectTail = "removed from your workitem list";

            if (toEmailAddresses.Count > 0)
            {
                //string a = GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPPassword");
                //string b = new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNTWISubject");
                //string mailSubject = "Workitem with Subject: "+subject+" has been "+MailSubjectTail;
                string mailSubject = string.Format(EMNTWorkitemSubject, subject, MailSubjectTail);
                //string body = @"<br>The update should be active on your Flag site one hour after this e-mail has been sent.<br><br>";
                string body = EMNTWorkitemBody;
                //if (messageFromUser != "") body = body + "Message added to action by the user: " + messageFromUser;
                if (messageFromUser != "") body = body + string.Format(EMNTWorkitemUserMessage, messageFromUser);
                body = body + string.Format(EMNTSignatureHTML, "Workitem", workitemRowId.ToString(), "GAWorkitem");
                GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(toEmailAddresses, null, null, mailSubject, body, true);
            }

        }
        private static string EMNTWorkitemSubject
        {
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNTWorkitemSubject"); }
        }
        private static string EMNTWorkitemBody
        {
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNTWorkitemBody"); }
        }
        private static string EMNTWorkitemUserMessage
        {
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNTWorkitemUserMessage"); }
        }
        private static string EMNTSignatureHTML
        {
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNTSignatureHTML"); }
        }
        private static string EMNTWorkitemAddedNoteMessage
        {
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNTWorkitemAddedNoteMessage"); }
        }

        /// <summary>
        /// set the acknowledge status for the workitem to acknowledge rejected. the current logged on user is logged 
        /// as the rejecter
        /// </summary>
        /// <param name="workitemId"></param>
        public static  void RejectAcknowledgeWorkitem(int workitemId)
        {
            int userid = 0;
            try
            {
                string logonId = AppUtils.GAUsers.GetUserId();
                userid = User.GetPersonnelIdByLogonId(logonId);
            }
            catch (Exception ex)
            {
                userid = -1;
            }

            Workitem wbc = (Workitem)RecordsetFactory.Make(GADataClass.GAWorkitem);
            WorkitemDS wds = (WorkitemDS)wbc.GetByRowId(workitemId);
            WorkitemDS.GAWorkitemRow row = (WorkitemDS.GAWorkitemRow)wds.GAWorkitem.Rows[0];
            
            row.TextFree2 = AcknowledgementStatus.AcknowledgementRejected.ToString();
            row.Extra1 = getUserAssigmentFromCombinedField(row.Extra5);
            row.Extra3 = getRoleAssigmentFromCombinedField(row.Extra5);
            
            wbc.CommitDataSet(wds);

            string historyLog = string.Format(AppUtils.Localization.GetGuiElementText("RejectAcknowledgeWorkitem"), User.getUserNameByLogonId(AppUtils.GAUsers.GetUserId()));
            wbc.addNotes(historyLog, workitemId);
        }

        private static string CreateCombinedUserAndRoleAssignment(string userAssignment, string roleAssignment)
        {
            if (userAssignment == string.Empty)
                userAssignment = "NULL";
            if (roleAssignment == string.Empty)
                roleAssignment = "NULL";
            return userAssignment + "-" + roleAssignment;
        }

        private static string getUserAssigmentFromCombinedField(string combinedField)
        {
            string[] splittedField = combinedField.Split('-');
            if (splittedField.Length != 2)
                return null;
            if (splittedField[0] == "NULL")
                return null;

            return splittedField[0];
        }

        private static string getRoleAssigmentFromCombinedField(string combinedField)
        {
            string[] splittedField = combinedField.Split('-');
            if (splittedField.Length != 2)
                return null;
            if (splittedField[1] == "NULL")
                return null;

            return splittedField[1];
        }
        
        // Tor 20181030
        private static bool IsSMTPNotificationOnOff()
        { 
            string _SMTPNotificationOnOff = new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPNotificationOnOff");
            return (_SMTPNotificationOnOff != null && _SMTPNotificationOnOff.ToUpper() == "ON");
        }

        // Tor 20181030
        private static bool IsFilterOnVertical()
        {
            string _IsFilterOnVertical = new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEisVerticalTestOnNotifications");
            return (_IsFilterOnVertical != null && _IsFilterOnVertical.ToUpper() == "YES");
        }

        // Tor 20181030 Returns email addresses
        public static System.Collections.ArrayList getRecipients(int roleId, int WorkitemRowId, bool isFilterOnVertical)
        {

            GASystem.DataModel.GADataRecord WorkitemRecord = new GASystem.DataModel.GADataRecord(WorkitemRowId, GASystem.DataModel.GADataRecord.ParseGADataClass("GAWorkitem"));
            System.Collections.ArrayList recipentsList = new System.Collections.ArrayList();
            // Tor 20181031 Get owner GADataRecord
            GASystem.DataModel.GADataRecord ActionRecord = GASystem.BusinessLayer.DataClassRelations.GetOwner(WorkitemRecord);
            if (ActionRecord.DataClass.ToString() != "GAAction") // workitem owner is not GAAction - return
                return recipentsList;

            // Tor 20150804 Start Get owners all levels and all many to many levels above ActionId
            System.Collections.Generic.List<GASystem.DataModel.GADataRecord> foundActionOwnerRecords = GASystem.BusinessLayer.DataClassRelations.GetCurrentParentLevelDataRecords(ActionRecord);

            String whereStatement = string.Empty;
            foreach (GASystem.DataModel.GADataRecord foundOwner in foundActionOwnerRecords)
            {
                whereStatement = whereStatement + " (s.OwnerClass='" + foundOwner.DataClass.ToString()
                    + "' and s.OwnerClassRowId=" + foundOwner.RowId.ToString() + ") or ";
            }
            if (whereStatement != string.Empty) whereStatement = "( " + whereStatement.Substring(0, whereStatement.Length - 3) + " )"; // remove laste OR
            // Tor 20150804 End

            // Tor 20151113 Start Get VerticalListsRowId from first owner above action if Vertical filter is true
            int verticalListsRowId = 0;
            if (isFilterOnVertical)
            {
                verticalListsRowId = GASystem.DataAccess.DataUtils.getVerticalListsRowIdFromOwner(ActionRecord.DataClass.ToString(), ActionRecord.RowId, null);
                // Tor 20160417 added test below
                if (verticalListsRowId == 0 || verticalListsRowId == null) verticalListsRowId = 0;
            }
            // Tor 20151113 End

            //split into individual recipients
            string roleIds = string.Empty;
            // Tor 20181031
            roleIds = roleId.ToString();
            // Tor 20181031
            //string[] smtpToRecipients = garecipent.Split(',');
            //foreach (string smtpToRecipient in smtpToRecipients)
            //{
            //    if (utils.AttributeHelper.IsParticipantARole(smtpToRecipient))
            //    {
            //        // Tor 20141217 moved to utils.AttributeHelper.  string roleName = getRoleIdentiferNamePart(smtpToRecipient);
            //        string roleName = utils.AttributeHelper.getRoleIdentiferNamePart(smtpToRecipient);
            //        // Tor 20170325 Job Title (Role) category moved from ER to TITL
            //        //int myroleId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", roleName);
            //        int myroleId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", roleName);
            //        if (myroleId != null) roleIds = roleIds + myroleId.ToString() + ",";
            //    }
            //    else
            //    {
            //        recipentsList.Add(utils.AttributeHelper.getUserIdentiferNamePart(smtpToRecipient));
            //    }
            //}
            //// Tor 20150804 start Get all DNN User Id with role 
            //if (roleIds != string.Empty) roleIds = roleIds.Substring(0, roleIds.Length - 1); // remove last comma
            if (whereStatement != string.Empty && roleIds != string.Empty)
            {
                // Tor 20151113 added vertical to parameter list
                recipentsList = GASystem.BusinessLayer.User.getDNNuserIdWhereOwnerInListAndRolesInList(whereStatement, roleIds, verticalListsRowId);
                //System.Collections.ArrayList myDNNUsers = GASystem.BusinessLayer.User.getDNNuserIdWhereOwnerInListAndRolesInList(whereStatement, roleIds, verticalListsRowId);
                //foreach (string myDNNUser in myDNNUsers)
                //{
                //    recipentsList.Add(myDNNUser);
                //}
            }
            return recipentsList;
        }

        // Tor 20190101 Returns email addresses
        public static System.Collections.ArrayList getRecipientsWithJobTitle(string roles, int WorkitemRowId, bool isFilterOnVertical)
        {
            GASystem.DataModel.GADataRecord WorkitemRecord = new GASystem.DataModel.GADataRecord(WorkitemRowId, GASystem.DataModel.GADataRecord.ParseGADataClass("GAWorkitem"));
            System.Collections.ArrayList recipentsList = new System.Collections.ArrayList();
            GASystem.DataModel.GADataRecord ActionRecord = GASystem.BusinessLayer.DataClassRelations.GetOwner(WorkitemRecord);
            if (ActionRecord.DataClass.ToString() != "GAAction") // workitem owner is not GAAction - return
                return recipentsList;

            System.Collections.Generic.List<GASystem.DataModel.GADataRecord> foundActionOwnerRecords = GASystem.BusinessLayer.DataClassRelations.GetCurrentParentLevelDataRecords(ActionRecord);

            String whereStatement = string.Empty;
            foreach (GASystem.DataModel.GADataRecord foundOwner in foundActionOwnerRecords)
            {
                whereStatement = whereStatement + " (s.OwnerClass='" + foundOwner.DataClass.ToString()
                    + "' and s.OwnerClassRowId=" + foundOwner.RowId.ToString() + ") or ";
            }
            if (whereStatement != string.Empty) whereStatement = "( " + whereStatement.Substring(0, whereStatement.Length - 3) + " )"; // remove laste OR

            int verticalListsRowId = 0;
            if (isFilterOnVertical)
            {
                verticalListsRowId = GASystem.DataAccess.DataUtils.getVerticalListsRowIdFromOwner(ActionRecord.DataClass.ToString(), ActionRecord.RowId, null);
                //if (verticalListsRowId == null) verticalListsRowId = 0;
                //verticalListsRowId=string.IsNullOrEmpty(verticalListsRowId.ToString() ? 0 : verticalListsRowId;
                if (string.IsNullOrEmpty(verticalListsRowId.ToString())) verticalListsRowId=0;
            }

            string smtpToRecipients = roles.Replace(';', ',');
            smtpToRecipients = smtpToRecipients.Substring(1, smtpToRecipients.Length - 2);

            if (whereStatement != string.Empty && smtpToRecipients != string.Empty)
            {
                recipentsList = GASystem.BusinessLayer.User.getDNNuserIdWhereOwnerInListAndRolesInList(whereStatement, smtpToRecipients, verticalListsRowId);
            }
            return recipentsList;
        }

        // Tor 20190721 Returns extraX (; separated elements )in arraylist
        public static void addElement2ExtraXArray(string elements, ref System.Collections.ArrayList recipentsList)
        {
//            System.Collections.ArrayList recipentsList = new System.Collections.ArrayList();
            if (!String.IsNullOrEmpty(elements) && elements.Length > 2)
            {
                string[] c = elements.Substring(1, elements.Length - 2).Split(';');
                int i = 0;
                foreach (string y in c)
                {
                    try
                    {
                        i = System.Convert.ToInt32(y);
                    }
                    catch (FormatException)
                    {
                        i = -1;
                    }
                    if (i > 0) recipentsList.Insert(recipentsList.Count,y);
                }
            }
            return;
        }
    }
}
