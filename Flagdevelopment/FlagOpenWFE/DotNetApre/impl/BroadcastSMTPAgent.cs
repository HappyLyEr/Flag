using System;
using GASystem.DotNetApre;
using openwfe.workitem;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem;

namespace GASystem.DotNetApre.impl
{
	/// <summary>
	/// Summary description for SMTPAgent.
	/// </summary>
	public class BroadcastSMTPAgent : AbstractAgent
	{
		public const string DEFAULTSMTPRECEIVER =  "__default_smtp_receiver__";
		public const string SMTPRECEIVER = "__smtpto__";
		public const string SMTPMESSAGE = "__smtpmessage__";
		public const string SMTPSUBJECT =  "__smtpsubject__";
		public const string WORKITEMSUBJECT =  "__subject__";
        public const string SMTPTEMPLATENAME = "__smtptemplatename__";


        public BroadcastSMTPAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
			
			int actionId = GetActionId(wi);
			System.Console.WriteLine("Starting Broadcast SMTP agent. Actionid: "  + actionId.ToString());

			ActionDS ds = Action.GetActionByActionRowId(actionId);
			System.Collections.Generic.List<int> recipients = new System.Collections.Generic.List<int>();

			//1. get email recipients using SMTPRECEIVER attribute
			string smtpTo = utils.AttributeHelper.GetAttribute(wi, SMTPRECEIVER);
            System.Console.WriteLine("Broadcast SMTP receiver in workflow is:" + smtpTo);

			if (smtpTo != string.Empty) {
                    getRecipients(recipients, smtpTo); 
			}

			// 2. if no recipients where found using SMTPRECEIVER attribute check for DEFAULTSMTPRECEIVER in config
			if (recipients.Count == 0)     
			{
				System.Console.WriteLine("Did not find any recipients for the mail. Checking for DEFAULTSMTPRECEIVER in config");
				//did not find any recipients above. use default recipient
				//check for default receiver
				string defaultSmtpRecipent = utils.AttributeHelper.GetAttribute(wi, DEFAULTSMTPRECEIVER);
				if (defaultSmtpRecipent != string.Empty)
					getRecipients(recipients, defaultSmtpRecipent); 
			}

            // 3. final test, if no recipients are found, use Workitem.GAPARTICIPANT
			if (recipients.Count == 0)     
			{
				//no recipents found, use gaparticipent and alternatively default workitem recipients implemented by GetPersonnelRowIdForGAParticipantsAndActionId method
				string gaparticipants =  utils.AttributeHelper.GetAttribute(wi, GASystem.BusinessLayer.Workitem.GAPARTICIPANT);
				System.Collections.ArrayList personnelRowIds = BusinessLayer.Workitem.GetPersonnelRowIdForGAParticipantsAndActionId(gaparticipants, actionId);
				foreach (int personnelRow in personnelRowIds)
				{
					recipients.Add(personnelRow);
				}
	
			}
				


        //Create Email subject and text
            



			
			//set email subject.  will try to add email subject for either SMTPSUBJECT, WORKITEMSUBJECT, or ACTIONSUBJECT. In that order
			string subject = utils.AttributeHelper.GetAttribute(wi, SMTPSUBJECT); 
			if (subject == string.Empty)
				subject = utils.AttributeHelper.GetAttribute(wi, WORKITEMSUBJECT);
			if (subject == string.Empty)
				subject = ds.GAAction[0].Subject;


            //get message text, check for template name and use if appropriate
            string message = string.Empty;
            bool sendInHTLMFormat = false;
            string smtpTemplateName = utils.AttributeHelper.GetAttribute(wi, SMTPTEMPLATENAME);
            if (smtpTemplateName != string.Empty)
            {
                //use template
                ProcedureSMTPTemplate smtpTemplate = new ProcedureSMTPTemplate(actionId, smtpTemplateName);
                message = "<html><body>" + smtpTemplate.getExpandedText() + "</body></html>";
                sendInHTLMFormat = true;
            }
            else
            {

                message = utils.AttributeHelper.GetAttribute(wi, SMTPMESSAGE);

                if (message == string.Empty)
                    message = ds.GAAction[0].Subject;

                // replace \n in message with line changes
                message = message.Replace("\\n", "\n");
                // add signature
                message += "\n" + SMTPSignature;
            }

			





		//send message
			foreach (int receiver in recipients) 
			{
				try 
				{
                    GASystem.BusinessLayer.FlagSMTP.SendMessage(receiver, subject, message, sendInHTLMFormat);
					System.Console.WriteLine("Sent mail: " + subject + " to recipient " + receiver.ToString());

				} 
				catch (Exception ex) 
				{
					System.Console.WriteLine("Failed to send mail: " + subject + " to recipient " + receiver.ToString());
					Console.WriteLine(ex.Message);
				}
			}

			return wi;
		}


        /// <summary>
        /// Get all recipients for selected roles and users by personnelrowid
        /// </summary>
        /// <param name="recipentsList">Return value, contains a list of user by loginname for sending the message to</param>
        /// <param name="garecipent"></param>
        /// <param name="ActionId"></param>
		private void getRecipients(System.Collections.Generic.IList<int> recipentsList, string garecipent) 
		{
            //split into individual recipients
            string[] smtpToRecipients = garecipent.Split(',');
            foreach (string smtpToRecipient in smtpToRecipients)
                if (utils.AttributeHelper.IsParticipantARole(smtpToRecipient))
			    {
                    string roleName = getRoleIdentiferNamePart(smtpToRecipient);
    				
                    //TODO change this method to return all users currently holding this role
                    GASystem.DataModel.EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByRoleAndDate(System.DateTime.Now, roleName);
    				
                    
                    foreach (GASystem.DataModel.EmploymentDS.GAEmploymentRow row in eds.GAEmployment) 
				    {
                        if (!row.IsPersonnelNull())    
                          recipentsList.Add(row.Personnel);
				    }
			    } 
			    else 
			    {
                    recipentsList.Add(User.GetPersonnelIdByLogonId(getUserIdentiferNamePart(smtpToRecipient)));
                        //note getUserIdentiferNamePart returns -1 if user is not found
			    }
		}


		/// <summary>
		/// Get rolename from garoleparticipant string.
		/// </summary>
		/// <param name="roleIdenifier"></param>
		/// <returns></returns>
		private static string getRoleIdentiferNamePart(string roleIdenifier) 
		{
			//TODO add check on whether it is a role
			return roleIdenifier.Trim().Replace(utils.AttributeHelper.ROLE_SUFFIX, string.Empty).Replace("{",string.Empty).Replace("}", string.Empty);
		}


		/// <summary>
		/// Get userid from gaparticipant string.
		/// </summary>
		/// <param name="roleIdenifier"></param>
		/// <returns></returns>
		private static string getUserIdentiferNamePart(string userIdentifier) 
		{
			return userIdentifier.Replace(utils.AttributeHelper.USER_SUFFIX, string.Empty).Replace("{",string.Empty).Replace("}", string.Empty);
		}

		private static string SMTPSignature 
		{
			get 
			{
				// Tor 20160308 if (System.Configuration.ConfigurationSettings.AppSettings.Get("SMTPSignature") != null)
                if (new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPSignature") != null)
                    {
					// Tor 20160308 string signatur = System.Configuration.ConfigurationSettings.AppSettings.Get("SMTPSignature");
                    string signatur = new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPSignature");
                    return signatur.Replace("\\n", "\n");
				}
				return string.Empty;
                // return new GASystem.AppUtils.FlagSysResource().GetResourceString("CoordinatorWorkitemFamilyName");
			}
		}

	}
}
