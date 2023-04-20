using System;
using GASystem.DotNetApre;
using openwfe.workitem;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem;
using System.Net.Mail;
using System.Collections;

namespace GASystem.DotNetApre.impl
{
	/// <summary>
	/// Summary description for SMTPAgent.
	/// </summary>
	public class SMTPAgent : AbstractAgent
	{
		public const string DEFAULTSMTPRECEIVER =  "__default_smtp_receiver__";
		public const string SMTPRECEIVER = "__smtpto__";
		public const string SMTPMESSAGE = "__smtpmessage__";
		public const string SMTPSUBJECT =  "__smtpsubject__";
		public const string WORKITEMSUBJECT =  "__subject__";
        public const string SMTPTEMPLATENAME = "__smtptemplatename__";
        public const string SMTPFILTERONVERTICAL = "__smtpFilterOnVertical__";
        public const string YES = "YES";
        public bool isSmtpFilterOnVertical = false;

		
		public SMTPAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
			
			int actionId = GetActionId(wi);
			System.Console.WriteLine("Starting SMTP agent. Actionid: "  + actionId.ToString());

			ActionDS ds = Action.GetActionByActionRowId(actionId);
			System.Collections.ArrayList recipients = new System.Collections.ArrayList();
			

			//get email recipients
			string smtpTo = utils.AttributeHelper.GetAttribute(wi, SMTPRECEIVER);
			System.Console.WriteLine("SMTP receiver in workflow is:"  + smtpTo);

            // Tor 20151113 get SMS recipients
            string ifSmtpFilterOnVertical = utils.AttributeHelper.GetAttribute(wi, SMTPFILTERONVERTICAL);
            System.Console.WriteLine("SMTP Filter on Vertical is:" + ifSmtpFilterOnVertical);
            if (ifSmtpFilterOnVertical.ToUpper() == YES) isSmtpFilterOnVertical = true;


			if (smtpTo != string.Empty) {
                // Tor 20141217 moved to utils.AttributeHelper getRecipients(recipients, smtpTo,actionId); 
                recipients = utils.AttributeHelper.getRecipients(smtpTo, actionId, isSmtpFilterOnVertical);
			}
			
			if (recipients.Count == 0)
            // Tor code below moved to utils.AttributeHelper getDefaultRecipients
                recipients = utils.AttributeHelper.getDefaultRecipients(wi, DEFAULTSMTPRECEIVER, actionId);
            //if (recipients.Count == 0)
            //{
            //    System.Console.WriteLine("Did not find any recipients for the mail. Checking for DEFAULTSMTPRECEIVER in config");
            //    //did not find any recipients above. use default recipient
            //    //check for default receiver
            //    string defaultSmtpRecipent = utils.AttributeHelper.GetAttribute(wi, DEFAULTSMTPRECEIVER);
            //    if (defaultSmtpRecipent != string.Empty)
            //        getRecipients(recipients, defaultSmtpRecipent, actionId); 
            //}

            //if (recipients.Count == 0)     
            //{
            //    //no recipents found, use gaparticipent and alternatively default workitem recipients implemented by GetPersonnelRowIdForGAParticipantsAndActionId method
            //    string gaparticipants =  utils.AttributeHelper.GetAttribute(wi, GASystem.BusinessLayer.Workitem.GAPARTICIPANT);
            //    System.Collections.ArrayList personnelRowIds = BusinessLayer.Workitem.GetPersonnelRowIdForGAParticipantsAndActionId(gaparticipants, actionId);
            //    foreach (int personnelRow in personnelRowIds)
            //    {
            //        recipients.Add(GASystem.BusinessLayer.User.GetLogonIdByPersonnelRowId(personnelRow));
            //    }
            //}


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
            /* Tor 20130919 added attributes for sending to all receivers with one SMTP login */
            string toEmailAddresses = string.Empty;
            string toAddress = string.Empty;
            bool firstAddress = true;
            bool lastAddress = false;
            int receivers = 0;

            // Tor 20130926 create message
            MailMessage messageObject = new MailMessage();

			foreach (object receiver in recipients) 
			{
				try
                {   // Tor 20130919 changed to read all e-mail addresses and send to all receivers in one e-mail 
					//GASystem.BusinessLayer.FlagSMTP.SendMessage(receiver.ToString(), subject, message, sendInHTLMFormat);
                    int PersonnelId = User.GetPersonnelIdByLogonId(receiver.ToString());
                    toAddress = GASystem.BusinessLayer.FlagSMTP.GetEmailForPersonnelRowId(PersonnelId);
//                  GetEmailForPersonnelRowId(PersonnelId);
                    /*add address if not already there */
                    if (toEmailAddresses.IndexOf(toAddress) < 0)
                    { 
                        toEmailAddresses += toAddress + ',';
                        receivers += 1;
//                      GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(firstAddress, lastAddress, toAddress, subject, message, sendInHTLMFormat);
                        GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(messageObject,firstAddress, lastAddress, toAddress, subject, message, sendInHTLMFormat);
                        firstAddress = false;
                    }

				} 
				catch (Exception ex) 
				{
					System.Console.WriteLine("Failed to send mail: " + subject + " to recipient " + receiver.ToString());
					Console.WriteLine(ex.Message);
				}
			}

            // Tor 20130919 Added (replaces //GASystem.BusinessLayer.FlagSMTP.SendMessage(receiver.ToString(), subject, message, sendInHTLMFormat); in the foreach above
            if (toEmailAddresses != string.Empty)
            {
                lastAddress = true;
                GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(messageObject,firstAddress,lastAddress,toAddress,subject,message,sendInHTLMFormat);
                System.Console.WriteLine("Sent mail: " + subject + " to recipient(s) " + toEmailAddresses);
					
            }
            else
            {
                System.Console.WriteLine("No e-mail receiver for: " + subject );
            }

			return wi;
		}
        // Tor 20141217 getRecipients moved to utils.AttributeHelper
		private void getRecipients(System.Collections.ArrayList recipentsList, string garecipent, int ActionId) 
		{
            // Tor 20150804 Start Get owners all levels and all many to many levels above ActionId
            GADataRecord ActionRecord = new GADataRecord(ActionId, GADataRecord.ParseGADataClass("GAAction"));
            System.Collections.Generic.List<GADataRecord> foundActionOwnerRecords = GASystem.BusinessLayer.DataClassRelations.GetCurrentParentLevelDataRecords(ActionRecord);
            String whereStatement = string.Empty;
            foreach (GADataRecord foundOwner in foundActionOwnerRecords)
            {
                whereStatement = whereStatement + " (s.OwnerClass='" + foundOwner.DataClass.ToString()
                    + "' and s.OwnerClassRowId=" + foundOwner.RowId.ToString() + ") or ";
            }
            if (whereStatement != string.Empty) whereStatement = "( " + whereStatement.Substring(0, whereStatement.Length - 3) + " )"; // remove laste OR
            // Tor 20150804 End

            //split into individual recipients
            string roleIds=string.Empty;
            string[] smtpToRecipients = garecipent.Split(',');
            foreach (string smtpToRecipient in smtpToRecipients)
            {
                if (utils.AttributeHelper.IsParticipantARole(smtpToRecipient))
			    {
                    // Tor 20141217 moved to utils.AttributeHelper.  string roleName = getRoleIdentiferNamePart(smtpToRecipient);
                    string roleName = utils.AttributeHelper.getRoleIdentiferNamePart(smtpToRecipient);
                    // Tor 20170325 Job Title (Role) category moved from ER to TITL
                    //int myroleId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", roleName);
                    int myroleId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", roleName);
                    if (myroleId != null) roleIds = roleIds + myroleId.ToString() + ",";

                    // Tor 20150804 start replaced by code above and below
                    //GASystem.DataModel.EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByActionIdDateAndRoleId(ActionId, System.DateTime.Now, roleName);
                    //foreach (GASystem.DataModel.EmploymentDS.GAEmploymentRow row in eds.GAEmployment) 
                    //{
                    //    GASystem.DataModel.UserDS uds = GASystem.BusinessLayer.User.GetUserByPersonnelRowId(row.Personnel);
                    //    foreach (GASystem.DataModel.UserDS.GAUserRow urow in uds.GAUser) 
                    //    {
                    //        // Tor: 20130830 add to recipientlist only if DNNUserId is not already there
                    //        if (recipentsList.IndexOf(urow.DNNUserId)<0)
                    //        {
                    //            recipentsList.Add(urow.DNNUserId);
                    //        }
                    //    }
                    //}
                    // Tor 20150804 end replaced by code above and below

			    } 
			    else 
			    {
                    recipentsList.Add(utils.AttributeHelper.getUserIdentiferNamePart(smtpToRecipient));
			    }
            }
            // Tor 20150804 start Get all DNN User Id with role 
            if (roleIds != string.Empty) roleIds = roleIds.Substring(0, roleIds.Length - 1); // remove last comma
            // Tor 20151113 Start Get VerticalListsRowId from first owner above action if Vertical filter is true
            int verticalListsRowId = 0;
            if (isSmtpFilterOnVertical)
            {
                verticalListsRowId = GASystem.DataAccess.DataUtils.getVerticalListsRowIdFromOwner("GAAction", ActionId, null);
                // Tor 20160417 added test below
                if (verticalListsRowId == null) verticalListsRowId = 0;
            }
            // Tor 20151113 End
            if (whereStatement != string.Empty && roleIds != string.Empty)
            {
                // Tor 20151113 added parameter to call below
                System.Collections.ArrayList myDNNUsers=GASystem.BusinessLayer.User.getDNNuserIdWhereOwnerInListAndRolesInList(whereStatement,roleIds,verticalListsRowId);
                foreach (string myDNNUser in myDNNUsers)
                {
                    recipentsList.Add(myDNNUser);
                }
            }

//                select distinct u.DNNUserId from GAEmployment e
//inner join GASuperClass s on s.MemberClass='GAEmployment' and s.MemberClassRowId=e.EmploymentRowId and 
//(
//(s.OwnerClass='GAFlag' and s.OwnerClassRowId=1) or
//(s.OwnerClass='GALocation' and s.OwnerClassRowId=1) or
//(s.OwnerClass='GACompany' and s.OwnerClassRowId=1) or
//(s.OwnerClass='GACrew' and s.OwnerClassRowId=1) or
//(s.OwnerClass='GAProject' and s.OwnerClassRowId=1) --or
//)
//inner join GAUser u on u.PersonnelRowId=e.Personnel
//where (e.RoleListsRowId in (56) and (e.FromDate<=GETDATE() and (e.ToDate>=GETDATE() or e.ToDate is null))) 
//--and datecondition

            
            }            // Tor 20150804 end Get all DNN User Id with role 
		

        // Tor 20141217 getRoleIdentiferNamePart and getUserIdentiferNamePart moved to utils.AttributeHelper.
        ///// <summary>
        ///// Get rolename from garoleparticipant string.
        ///// </summary>
        ///// <param name="roleIdenifier"></param>
        ///// <returns></returns>
        //private static string getRoleIdentiferNamePart(string roleIdenifier) 
        //{
        //    //TODO add check on whether it is a role
        //    return roleIdenifier.Trim().Replace(utils.AttributeHelper.ROLE_SUFFIX, string.Empty).Replace("{",string.Empty).Replace("}", string.Empty);
        //}


        ///// <summary>
        ///// Get userid from gaparticipant string.
        ///// </summary>
        ///// <param name="roleIdenifier"></param>
        ///// <returns></returns>
        //private static string getUserIdentiferNamePart(string userIdentifier) 
        //{
        //    return userIdentifier.Replace(utils.AttributeHelper.USER_SUFFIX, string.Empty).Replace("{",string.Empty).Replace("}", string.Empty);
        //}

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
			}
		}

	}
}
