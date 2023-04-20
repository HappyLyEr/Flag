using System;
using GASystem.DotNetApre;
using openwfe.workitem;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem;

namespace GASystem.DotNetApre.impl
{
	/// <summary>
	/// Summary description for SMSAgent.
	/// </summary>
	public class SMSAgent : AbstractAgent
	{
		public const string DEFAULTSMSRECEIVER =  "__default_sms_receiver__";
        public const string DEFAULTSMTPRECEIVER = "__default_smtp_receiver__";
		public const string SMSRECEIVER = "__smsto__";
		public const string SMSMESSAGE = "__smsmessage__";
        public const string SMSFILTERONVERTICAL = "__smsFilterOnVertical__";
        public const string YES="Yes";
        public bool isSmsFilterOnVertical = false;
		
		public SMSAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
			
			int actionId = GetActionId(wi);
            System.Console.WriteLine("Starting SMS agent. Actionid: " + actionId.ToString());

            ActionDS ds = Action.GetActionByActionRowId(actionId);
            System.Collections.ArrayList recipients = new System.Collections.ArrayList();

            //get SMS recipients
            string smsTo = utils.AttributeHelper.GetAttribute(wi, SMSRECEIVER);
            System.Console.WriteLine("SMS receiver in workflow is:" + smsTo);
            
            // Tor 20151113 get SMS recipients
            string ifSmsFilterOnVertical = utils.AttributeHelper.GetAttribute(wi, SMSFILTERONVERTICAL);
            System.Console.WriteLine("SMS Filter on Vertical is:" + ifSmsFilterOnVertical);
            if (ifSmsFilterOnVertical == YES) isSmsFilterOnVertical = true;

            // Tor 20141217 Cloned get repipients code from SMTPAgent.cs
            if (smsTo != string.Empty)
            {
                recipients = utils.AttributeHelper.getRecipients(smsTo, actionId, isSmsFilterOnVertical);
            }
// Tor 20141217 Replaced by code above
            //if (smsTo != string.Empty) 
            //{
            //    if (utils.AttributeHelper.IsParticipantARole(smsTo))
            //    {
            //        string roleName = smsTo.Replace(utils.AttributeHelper.ROLE_SUFFIX, "");
            //        roleName = roleName.Replace("{", string.Empty);
            //        roleName = roleName.Replace("}", string.Empty);
					

            //        GASystem.DataModel.EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByActionIdDateAndRoleId(GetActionId(wi), System.DateTime.Now, roleName);
            //        foreach (GASystem.DataModel.EmploymentDS.GAEmploymentRow row in eds.GAEmployment) 
            //        {
            //            GASystem.DataModel.UserDS uds = GASystem.BusinessLayer.User.GetUserByUserRowId(row.Personnel);
            //            foreach (GASystem.DataModel.UserDS.GAUserRow urow in uds.GAUser) 
            //            {
            //                recipients.Add(urow.DNNUserId);
            //            }
            //        }
            //    } 
            //    else 
            //    {
            //        string smsUser = smsTo.Replace(utils.AttributeHelper.USER_SUFFIX, string.Empty);
            //        smsUser = smsUser.Replace("{", string.Empty);
            //        smsUser = smsUser.Replace("}", string.Empty);

            //        recipients.Add(smsUser);
            //    }

            //}

            // Tor 20141217 Cloned from SMTPAgent.cs
            if (recipients.Count == 0)
            {
                System.Console.WriteLine("Did not find any recipients for the SMS. Checking for DEFAULTSMSRECEIVER in config");
                recipients = utils.AttributeHelper.getDefaultRecipients(wi, DEFAULTSMSRECEIVER, actionId);
                // Tor 20141217 added test below
                if (recipients.Count == 0)
                {
                    System.Console.WriteLine("Did not find DEFAULTSMSRECEIVER. Checking for DEFAULTSMTPRECEIVER in config");
                    recipients = utils.AttributeHelper.getDefaultRecipients(wi, DEFAULTSMTPRECEIVER, actionId);
                }                
            }
            // Tor code below moved to utils.AttributeHelper getDefaultRecipients
            //if (recipients.Count == 0)
            //{
            //    System.Console.WriteLine("Did not find any recipients for the SMS. Checking for DEFAULTSMSRECEIVER in config");
            //    //did not find any recipients above. use default recipient
            //    //check for default receiver
            //    string defaultSMSRecipent = utils.AttributeHelper.GetAttribute(wi, DEFAULTSMSRECEIVER);
            //    if (defaultSMSRecipent != string.Empty)
            //        getRecipients(recipients, defaultSmtpRecipent, actionId);
            //}


            //if (recipients.Count == 0)
            //{
            //    //no recipents found, use gaparticipent and alternatively default workitem recipients implemented by GetPersonnelRowIdForGAParticipantsAndActionId method
            //    string gaparticipants = utils.AttributeHelper.GetAttribute(wi, GASystem.BusinessLayer.Workitem.GAPARTICIPANT);
            //    System.Collections.ArrayList personnelRowIds = BusinessLayer.Workitem.GetPersonnelRowIdForGAParticipantsAndActionId(gaparticipants, actionId);
            //    foreach (int personnelRow in personnelRowIds)
            //    {
            //        recipients.Add(GASystem.BusinessLayer.User.GetLogonIdByPersonnelRowId(personnelRow));
            //    }
            //}

            //if (recipients.Count == 0)     
            //{
            //    //did not find any recipients above. use default recipient
            //    //check for default receiver

            //    string responsibleId = utils.AttributeHelper.GetAttribute(wi, DEFAULTSMSRECEIVER);
			
            //    if (responsibleId == string.Empty) 
            //    {
            //        //no specific recipent set user gaparticipant
            //        string gaparticipants =  utils.AttributeHelper.GetAttribute(wi, GASystem.BusinessLayer.Workitem.GAPARTICIPANT);
            //        System.Data.DataSet pds = BusinessLayer.Workitem.GetPersonnelForGAParticipantsAndActionId(gaparticipants, actionId);
            //        foreach (System.Data.DataRow row in pds.Tables[0].Rows)
            //        {
            //            recipients.Add(GASystem.BusinessLayer.User.GetLogonIdByPersonnelRowId(int.Parse(row["personnelrowid"].ToString())));
            //        }

            //    }
					//responsibleId = "host";  //set default to host   //TODO change to get default role
//				if (ds.GAAction.Rows.Count > 0) 
//				{
//					if (!ds.GAAction[0].IsResponsibleNull()) 
//					{
//						int personellId = ds.GAAction[0].Responsible;
//						GASystem.DataModel.UserDS uds = GASystem.BusinessLayer.User.GetUserByPersonnelRowId(personellId);
//				
//						//todo add test for role and not person
//				
//						if (uds.GAUser.Rows.Count > 0)
//							responsibleId = uds.GAUser[0].DNNUserId;
//					}
//				}
//
//				recipients.Add(responsibleId);
            //}

			string message = utils.AttributeHelper.GetAttribute(wi, SMSMESSAGE);
			if (message == string.Empty)
				message = ds.GAAction[0].Subject;

			//send message

			foreach (object receiver in recipients) 
			{
				try 
				{
					GASystem.BusinessLayer.sms.SendMessage(receiver.ToString(), message);
				} 
				catch (Exception ex) 
				{
					Console.WriteLine(ex.Message);
				}
			}

			return wi;
		}

	}
}
