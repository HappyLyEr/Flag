using System;
using openwfe.workitem;

namespace GASystem.DotNetApre.utils
{
	/// <summary>
	/// Summary description for AttributeHelper.
	/// </summary>
	public class AttributeHelper
	{
		public const string  ROLE_SUFFIX = "garole-";
		public const string  USER_SUFFIX = "gauser-";
		

		public AttributeHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// get value for workitem attribute
		/// </summary>
		/// <param name="wi"></param>
		/// <param name="AttributeName"></param>
		/// <returns>String: attribute value. If attribute does not exists, return empty string</returns>
		
		public static string GetAttribute(openwfe.workitem.InFlowWorkitem wi, string AttributeName) 
		{
			string attributeValue = string.Empty;
			try 
			{
				attributeValue = wi.attributes[new StringAttribute(AttributeName)].ToString();
			} 
			catch {}

			return attributeValue;
		}

		public static void SetAttribute(openwfe.workitem.InFlowWorkitem wi, string AttributeName, string AttributeValue) 
		{
			StringAttribute fieldNameKey = new StringAttribute(AttributeName);
			StringAttribute fieldValueKeyValue = new StringAttribute(AttributeValue);

			
			wi.attributes[fieldNameKey] = fieldValueKeyValue; 
		}

		public static bool IsParticipantARole(string Participant) 
		{
			return Participant.IndexOf(ROLE_SUFFIX) == 0 || Participant.IndexOf(ROLE_SUFFIX) == 1; //is a role is role suffix is found at the start of the string
		}

        // Tor 20141217 getRecipients moved from SMTPAgent.cs to be used by SMTPAgent.cs and SMSAgent.cs
        // Tor 20151113 added bool parameter isSmsFilterOnVertical : filter role recipients on Vertical
        public static System.Collections.ArrayList getRecipients(string garecipent, int ActionId, bool isFilterOnVertical)
        {
            // Tor 20150804 Start Get owners all levels and all many to many levels above ActionId
            System.Collections.ArrayList recipentsList = new System.Collections.ArrayList();

            GASystem.DataModel.GADataRecord ActionRecord = new GASystem.DataModel.GADataRecord(ActionId, GASystem.DataModel.GADataRecord.ParseGADataClass("GAAction"));
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
            int verticalListsRowId=0;
            if (isFilterOnVertical) 
            {
                verticalListsRowId = GASystem.DataAccess.DataUtils.getVerticalListsRowIdFromOwner("GAAction", ActionId, null);
                // Tor 20160417 added test below
                if (verticalListsRowId == null) verticalListsRowId = 0;
            }
            // Tor 20151113 End
 
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
			    } 
			    else 
			    {
                    recipentsList.Add(utils.AttributeHelper.getUserIdentiferNamePart(smtpToRecipient));
			    }
            }
            // Tor 20150804 start Get all DNN User Id with role 
            if (roleIds != string.Empty) roleIds = roleIds.Substring(0, roleIds.Length - 1); // remove last comma
            if (whereStatement!=string.Empty && roleIds!=string.Empty)
            {
                // Tor 20151113 added vertical to parameter list
                System.Collections.ArrayList myDNNUsers = GASystem.BusinessLayer.User.getDNNuserIdWhereOwnerInListAndRolesInList(whereStatement, roleIds, verticalListsRowId);
                foreach (string myDNNUser in myDNNUsers)
                {
                    recipentsList.Add(myDNNUser);
                }
            }
            return recipentsList;
        }
        // Tor 20150804 end Get all DNN User Id with role 

            // Tor 20150804 below replaced by above
            //System.Collections.ArrayList recipientsList = new System.Collections.ArrayList();

            ////split into individual recipients
            //string[] Recipients = garecipent.Split(',');
            //foreach (string Recipient in Recipients)
            //    if (utils.AttributeHelper.IsParticipantARole(Recipient))
            //    {
            //        // Tor 20141217 moved to utils.AttributeHelper.  string roleName = getRoleIdentiferNamePart(smtpToRecipient);
            //        string roleName = utils.AttributeHelper.getRoleIdentiferNamePart(Recipient);
            //        GASystem.DataModel.EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByActionIdDateAndRoleId(ActionId, System.DateTime.Now, roleName);
            //        foreach (GASystem.DataModel.EmploymentDS.GAEmploymentRow row in eds.GAEmployment)
            //        {
            //            GASystem.DataModel.UserDS uds = GASystem.BusinessLayer.User.GetUserByPersonnelRowId(row.Personnel);
            //            foreach (GASystem.DataModel.UserDS.GAUserRow urow in uds.GAUser)
            //            {
            //                // Tor: 20130830 add to recipientlist only if DNNUserId is not already there
            //                if (recipientsList.IndexOf(urow.DNNUserId) < 0)
            //                {
            //                    recipientsList.Add(urow.DNNUserId);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        recipientsList.Add(utils.AttributeHelper.getUserIdentiferNamePart(Recipient));
            //    }
            //return recipientsList;

        // Tor 20141217 getRecipients moved from SMTPAgent.cs to be used by SMTPAgent.cs and SMSAgent.cs
        public static System.Collections.ArrayList getDefaultRecipients(openwfe.workitem.InFlowWorkitem wi,string gaDefaultrecipent, int ActionId)
        {
            System.Collections.ArrayList recipientsList = new System.Collections.ArrayList();
            System.Console.WriteLine("Did not find any recipients. Checking for "+gaDefaultrecipent+" in workflow");
            string defaultRecipent = utils.AttributeHelper.GetAttribute(wi, gaDefaultrecipent);
            if (defaultRecipent != string.Empty)
                // Tor 20151113 added parameter false to call below
                recipientsList = getRecipients(defaultRecipent, ActionId, false);

            if (recipientsList.Count == 0)
            {
                //no recipents found, use gaparticipant and alternatively default workitem recipients 
                //implemented by GetPersonnelRowIdForGAParticipantsAndActionId method
                string gaparticipants = utils.AttributeHelper.GetAttribute(wi, GASystem.BusinessLayer.Workitem.GAPARTICIPANT);
                System.Collections.ArrayList personnelRowIds = BusinessLayer.Workitem.GetPersonnelRowIdForGAParticipantsAndActionId(gaparticipants, ActionId);
                foreach (int personnelRow in personnelRowIds)
                {
                    recipientsList.Add(GASystem.BusinessLayer.User.GetLogonIdByPersonnelRowId(personnelRow));
                }
            }

            if (recipientsList.Count == 0)
            // Tor 20150828 did not find default, Get PersonnelRowId from ConsolConsumer.exe.config, if not found, try with givenname and familyname
            {
                int myPersonnelRowId = GASystem.DotNetApre.impl.SimpleConsumer.CoordinatorWorkitemPersonnelRowId;
                System.Console.WriteLine("Did not find any Default recipient " + gaDefaultrecipent + ". Checking for Person with rowid " + myPersonnelRowId.ToString() + " from config file");
                if (myPersonnelRowId > 0
                    && GASystem.BusinessLayer.Personnel.ifPersonExists(myPersonnelRowId) > 0
                    && (GASystem.BusinessLayer.User.GetLogonIdByPersonnelRowId(myPersonnelRowId) != string.Empty))
                {
                    recipientsList.Add(GASystem.BusinessLayer.User.GetLogonIdByPersonnelRowId(myPersonnelRowId));
                }
                else
                {
                    System.Console.WriteLine("Did not find person with PersonnelRowId " + myPersonnelRowId.ToString() + ". Checking for Person with GivenName " 
                        + GASystem.DotNetApre.impl.SimpleConsumer.CoordinatorWorkitemGivenName + " and FamilyName " 
                        + GASystem.DotNetApre.impl.SimpleConsumer.CoordinatorWorkitemFamilyName + " from config file");

                    myPersonnelRowId = getPersonnelRowIdCoordinatorByName();
                    if (myPersonnelRowId > 0
                        && (GASystem.BusinessLayer.User.GetLogonIdByPersonnelRowId(myPersonnelRowId) != string.Empty))
                    {
                        recipientsList.Add(GASystem.BusinessLayer.User.GetLogonIdByPersonnelRowId(myPersonnelRowId));
                    }
                }
            }
            return recipientsList;
        }


        /// <summary>
        /// Get rolename from garoleparticipant string.
        /// </summary>
        /// <param name="roleIdenifier"></param>
        /// <returns></returns>
        public static string getRoleIdentiferNamePart(string roleIdenifier)
        {
            //TODO add check on whether it is a role
            return roleIdenifier.Trim().Replace(utils.AttributeHelper.ROLE_SUFFIX, string.Empty).Replace("{", string.Empty).Replace("}", string.Empty);
        }

        /// <summary>
        /// Get userid from gaparticipant string.
        /// </summary>
        /// <param name="roleIdenifier"></param>
        /// <returns></returns>
        public static string getUserIdentiferNamePart(string userIdentifier)
        {
            return userIdentifier.Replace(utils.AttributeHelper.USER_SUFFIX, string.Empty).Replace("{", string.Empty).Replace("}", string.Empty);
        }
        public static int getPersonnelRowIdCoordinatorByName()
        {
            return GASystem.BusinessLayer.Personnel.ifPersonExists
                        (GASystem.DotNetApre.impl.SimpleConsumer.CoordinatorWorkitemGivenName,
                        GASystem.DotNetApre.impl.SimpleConsumer.CoordinatorWorkitemFamilyName);
        }

	}

}
