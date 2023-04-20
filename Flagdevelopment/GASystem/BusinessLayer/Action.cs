using System;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Collections;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Action.
	/// </summary>
	public class Action : BusinessClass
	{
		public const string ACTIONSTATUSCATEGORYNAME = "ST";

		public const string START_WORKFLOW_AUTOMATICALLY = "start_workflow_automatically";
				
		public Action()
		{
			this.DataClass = GADataClass.GAAction;
			//
			// TODO: Add constructor logic here
			//
		}

		public static void SetActionCompleted(int ActionId) 
		{
			GASystem.DataModel.ActionDS ads = GetActionByActionRowId(ActionId);
			ads.GAAction[0].DateEndActual = System.DateTime.Now;
			ads.GAAction[0].ActionStatusListsRowId = Lists.GetListsRowIdByCategoryAndKey(ACTIONSTATUSCATEGORYNAME, "Closed");
			UpdateAction(ads);
			//ads.GAAction[0].ActionStatusListsRowIdColumn = 10;

	    }




		private void SendMailMessageToSingleTaskParticipants(ArrayList Recipients, string Subject, string Description, GADataTransaction transaction)
		{
			//Message
			string mailsubject = "Message from FLAG: "  + Subject;
			string mailBody = "Action description: \n" + Description + " \n\nA single task workitem has been added to your FLAG Workitem list. \n \nLog on to FLAG and the workitem will be presented to you in your Workitem list"; 
			
			try 
			{
				foreach (string recipient in Recipients) 
				{
					//string emailAddress = this.GetEmailForLoginId(recipient, transaction).
					FlagSMTP.SendEmailMsg(GetEmailForLoginId(recipient, transaction), mailsubject, mailBody, false);
				}
			} 
			catch (Exception ex) 
			{
				string msgex = ex.Message;
				//TODO log
				//mail sending fails is not critical in this setting
			}
		}

		private string GetEmailForLoginId(string LoginId, GADataTransaction transaction)
		{
			int PersonnelId = User.GetPersonnelIdByLogonId(LoginId);
			//DataModel.MeansOfContactDS ds =  MeansOfContact.GetMeansOfContactsByOwnerAndDeviceTypeId(PersonnelId, DataModel.GADataClass.GAPersonnel, EMAILLISTSVALUE);
			string emailaddress =  MeansOfContact.GetContactAddressByOwnerAndDeviceTypeId(PersonnelId, DataModel.GADataClass.GAPersonnel, FlagSMTP.EMAILLISTSVALUE, transaction);
			if (emailaddress == string.Empty) 
				throw new GAExceptions.GAException("No means of contact found for login");

			return emailaddress;
			
		}

		public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
		{
            // Tor 201611 Security 20161109 added: use UpdateGASuperClassChangedBy to update attributes in GASuperClass record 
            GASystem.AppUtils.SuperClassAttributes.UpdateGASuperClassChangedBy(ds, transaction);
			return UpdateAction((ActionDS)ds, transaction);
		}

		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetActionByActionRowId(RowId);
		}
		
		public static ActionDS GetActionForWorkFlowStartPending() 
		{
			return ActionDb.GetActionForWorkFlowStartPending();
		}

		public static ActionDS GetAllActions()
		{
			return ActionDb.GetAllActions();
		}
	

		public static ActionDS GetActionsByOwner(GADataRecord ActionOwner)
		{
			return  ActionDb.GetActionsByOwner(ActionOwner.RowId, ActionOwner.DataClass);
		}

		public static ActionDS GetOpenActionsByOwner(GADataRecord ActionOwner) 
		{
			return ActionDb.GetOpenActionsByOwner(ActionOwner.RowId, ActionOwner.DataClass);
		}

//        // Tor 20151105 Get Vertical ListsRowId from GAAction record or above
//        public static object GetVerticalListsRowIdByGADataRecord(GADataRecord record)
//        {
//            // Tor TODO
////            exec [dbo].[GAGetVerticalFromOwnerAllLevels] @myClass,@myRowId, @myVerticalRowId = @x OUTPUT;

//            return ActionDb.GetActionByActionRowId(ActionRowId);
//        }

        public static ActionDS GetActionByActionRowId(int ActionRowId)
		{
			return ActionDb.GetActionByActionRowId(ActionRowId);
		}

        public static ActionDS GetActionByActionRowId(int ActionRowId, GADataTransaction transaction)
        {
            return ActionDb.GetActionByActionRowId(ActionRowId, transaction);
        }

		public static int GetActionIdByWorkflowId(int WorkflowId) 
		{
			ActionDS ds = ActionDb.GetActionByWorkFlowId(WorkflowId);
			return ds.GAAction[0].ActionRowId;
		}

		public static ActionDS GetNewAction()
		{
			ActionDS iDS = new ActionDS();
			//System.Data.DataSet iDS = new ActionDS();
			
			GASystem.DataModel.ActionDS.GAActionRow row = iDS.GAAction.NewGAActionRow();
		//	set default values for non-null attributes
		//	row.EmploymentRowId = 0;
		//	row.DateTimeFrom = DateTime.Today;
			iDS.GAAction.Rows.Add(row);
//			iDS.Tables["GAAction"].Rows.Add(iDS.Tables["GAAction"].NewRow());
			return iDS;
		}

		public static System.Data.DataSet GetNewRow()
		{
			//ActionDS iDS = new ActionDS();
			System.Data.DataSet iDS = new ActionDS();
			
			//	GASystem.DataModel.ActionDS.GAActionRow row = iDS.GAAction.NewGAActionRow();
			//	set default values for non-null attributes
			//	row.EmploymentRowId = 0;
			//	row.DateTimeFrom = DateTime.Today;
			//	iDS.GAAction.Rows.Add(row);
			iDS.Tables["GAAction"].Rows.Add(iDS.Tables["GAAction"].NewRow());
			return iDS;
		}

		public static ActionDS SaveNewAction(ActionDS ActionSet, GADataRecord ActionOwner)
		{
			if (ActionSet.GAAction[0].IsReportDateNull())
				ActionSet.GAAction[0].ReportDate=System.DateTime.Now;
			
	// Transaction start
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				ActionSet = UpdateAction(ActionSet, transaction);
				DataClassRelations.CreateDataClassRelation(ActionOwner.RowId, ActionOwner.DataClass, ActionSet.GAAction[0].ActionRowId, GADataClass.GAAction, transaction);
				//add member classes
				// Utils.StoreObject.AddMemberClasses(new GADataRecord(ActionSet.GAAction[0].ActionRowId, GADataClass.GAAction), transaction);
				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				throw ex;
			}
			finally 
			{
				transaction.Connection.Close();
			}
			return ActionSet;
		}

		public static ActionDS UpdateAction(ActionDS ActionSet)
		{
			return UpdateAction(ActionSet, null);
		}
		public static ActionDS UpdateAction(ActionDS ActionSet, GADataTransaction transaction)
		{
			return ActionDb.UpdateAction(ActionSet, transaction);
		}
// transaction end

		public bool HasWorkflow(int ActionId) 
		{
			bool hasWF = false;
			//get action details
			ActionDS ads = (ActionDS)GetByRowId(ActionId);
			//get linked procedure
			if (ads.GAAction.Rows.Count > 0) 
			{
				if (!ads.GAAction[0].IsProcedureRowIdNull()) 
				{
					int procedureRowId = ads.GAAction[0].ProcedureRowId; 
					BusinessClass bc = Utils.RecordsetFactory.Make(GADataClass.GAProcedure);
					ProcedureDS pds = (ProcedureDS)bc.GetByRowId(procedureRowId);
					if (pds.GAProcedure.Rows.Count > 0) 
						hasWF = !pds.GAProcedure[0].IsWorkflownameNull() && pds.GAProcedure[0].Workflowname != string.Empty ;
				}
			}

			return hasWF;
			//
		}
	}
}
