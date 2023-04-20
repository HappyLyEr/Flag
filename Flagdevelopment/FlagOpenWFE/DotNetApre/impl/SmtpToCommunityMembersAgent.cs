using System;
using System.Data;
using GASystem.DotNetApre;
using openwfe.workitem;
using GASystem.BusinessLayer;
using GASystem.DataAccess;
using GASystem.DataModel;
using GASystem;
using System.Net.Mail;
using System.Data.SqlClient;



namespace GASystem.DotNetApre.impl
{
	/// <summary>
    /// Summary description for SmtpToCommunityMembersAgent.
    /// Get Action owner, get attribute UserCommunityRowId from all Action Owner GAInfoToCommunity member records
    /// build smtp message
    /// send smtp message
	/// </summary>
    public class SmtpToCommunityMembersAgent : AbstractAgent
	{
		public const string DEFAULTSMTPRECEIVER =  "__default_smtp_receiver__";
		public const string SMTPRECEIVER = "__smtpto__";
		public const string SMTPMESSAGE = "__smtpmessage__";
		public const string SMTPSUBJECT =  "__smtpsubject__";
		public const string WORKITEMSUBJECT =  "__subject__";
        public const string SMTPTEMPLATENAME = "__smtptemplatename__";
		
		
		public SmtpToCommunityMembersAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
			int actionId = GetActionId(wi);
            System.Console.WriteLine("Starting SmtpToCommunityMembersAgent agent. Actionid: " + actionId.ToString());
            GADataRecord action= new GADataRecord(actionId,GADataRecord.ParseGADataClass("GAAction"));
            GADataRecord actionOwner = GASystem.BusinessLayer.DataClassRelations.GetOwner(action); 
            if (actionOwner == null) return wi;
//            GASystem.DataModel.InfoToCommunityDS ids = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(GADataRecord.ParseGADataClass("GAInfoToCommunity"), actionOwner);
//            InfoToCommunityDS ids = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassByOwner(GASystem.DataModel.GADataRecord.ParseGADataClass("GAInfoToCommunity"), actionOwner);
            DataSet ids = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassByOwner(GASystem.DataModel.GADataRecord.ParseGADataClass("GAInfoToCommunity"), actionOwner);

            if (ids == null) return wi;
            int roleId=0;
            string communities=string.Empty;
//            foreach (InfoToCommunityDS.GAInfoToCommunityRow row in ids.GAInfoToCommunity)
            foreach (InfoToCommunityDS.GAInfoToCommunityRow row in ids.Tables[0].Rows)
            {
                if (!row.IsUserCommunityRowIdNull())
                {
                    roleId = getRoleId(row.UserCommunityRowId);
                    if(roleId!=null && roleId!=0) communities = communities + roleId.ToString()+",";
                }
            }

            if (communities == string.Empty) return wi;
            // remove the last comma
            communities = communities.Substring(0, communities.Length - 1);

            //Create Email subject and text
            ActionDS ds = Action.GetActionByActionRowId(actionId);
            //set email subject.  will try to add email subject for either SMTPSUBJECT, WORKITEMSUBJECT, or ACTIONSUBJECT. In that order
            string subject = utils.AttributeHelper.GetAttribute(wi, SMTPSUBJECT);
            if (subject == string.Empty) subject = utils.AttributeHelper.GetAttribute(wi, WORKITEMSUBJECT);
            if (subject == string.Empty) subject = ds.GAAction[0].Subject;

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
                if (message == string.Empty) message = ds.GAAction[0].Subject;
                // replace \n in message with line changes
                message = message.Replace("\\n", "\n");
                // add signature
                message += "\n" + SMTPSignature;
            }

            // get all distinct e-mail addresses
            // Tor 20160515 changed to return addresses in ArrayList SqlDataReader eMailAddresses=GetEmailAddresses(communities);
            System.Collections.ArrayList eMailAddresses = new System.Collections.ArrayList();
            eMailAddresses = GetEmailAddresses(communities);
            if (eMailAddresses.Count == 0) return wi;
            // Tor 20160515 if (!eMailAddresses.HasRows) return wi;

            //send message
            string toEmailAddresses = string.Empty;
            string toAddress = string.Empty;
            bool firstAddress = true;
            bool lastAddress = false;
            int receivers = 0;

            // Tor 20130926 create message
            MailMessage messageObject = new MailMessage();

            // Tor 20160515 Replaced with for loop below 
            //while (eMailAddresses.Read())
            //{
            //    toAddress=eMailAddresses.GetString(0);
            //    toEmailAddresses += toAddress + ',';
            //    receivers += 1;
            //    GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(messageObject,firstAddress, lastAddress, toAddress, subject, message, sendInHTLMFormat);
            //    firstAddress = false;
            //}

            for (int iIndex = 0; iIndex < eMailAddresses.Count; iIndex++)
            {
                toAddress=eMailAddresses[iIndex].ToString();
                toEmailAddresses += toAddress + ',';
                receivers += 1;
                GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(messageObject, firstAddress, lastAddress, toAddress, subject, message, sendInHTLMFormat);
                firstAddress = false;
            }

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

        private int getRoleId(int userCommunityRowId)
        { 
//            if (userCommunityRowId==null) return null;
            int roleId=0;
            string sql="select u.RoleId from GAUserCommunity u where u.UserCommunityRowId="+userCommunityRowId.ToString();
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(sql, myConnection);
                roleId=(int)myCommand.ExecuteScalar();
            }
            catch
            { }
            finally
            {
                myConnection.Close();
            }
            return roleId;
        }

//        private SqlDataReader GetEmailAddresses (string communities)
//        {
//            string sql = @"
///* select distinct u.Email from flagdnn.dbo.Roles r */
//select distinct m.ContactDeviceAddress as Email from flagdnn.dbo.Roles r 
//inner join flagdnn.dbo.UserRoles ur on ur.RoleID=r.RoleID
//inner join flagdnn.dbo.Users u on u.UserID=ur.UserID
//inner join flagdnn.dbo.aspnet_Users au on au.UserName=u.Username
//inner join flagdnn.dbo.aspnet_Membership am on am.UserId=au.UserId and am.IsApproved=1 --and am.IsLockedOut=0
//inner join GAUser fu on fu.DNNUserId=u.Username
//inner join GAEmployment fe on fe.Personnel=fu.PersonnelRowId and fe.FromDate<=GETDATE() and (fe.ToDate is null or fe.ToDate>=GETDATE())
///* for test start */
//inner join GAPersonnel p on p.PersonnelRowId=fe.Personnel
//inner join GASuperClass s on s.OwnerClass='GAPersonnel' and s.OwnerClassRowId=p.PersonnelRowId and s.MemberClass='GAMeansOfContact'
//inner join GAMeansOfContact m on m.MeansOfContactRowId=s.MemberClassRowId and /*e-mail address*/ m.ContactDeviceTypeListsRowId=(select l.ListsRowId from GALists l where l.GAListCategory='DT' and l.GAListValue='e-mail 1')
///* for test end */
//where r.PortalID=0 and r.RoleID in ( {0} ) 
//";
//            sql = string.Format(sql, communities);
////            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
//// 20160515            SqlDataReader result = GASystem.DataAccess.DataUtils.executeSelectSpecial(sql, DataUtils.getConnectionString());
//            System.Collections.ArrayList result = GASystem.DataAccess.DataUtils.executeSelectSpecial(sql, DataUtils.getConnectionString(), "string");
//            return result;
//        }

        // Tor 20150515 Added method to return email addresses in array
        private System.Collections.ArrayList GetEmailAddresses(string communities)
        {
            // Tor 20170325 get email address from flagdnn.Users instead of from GAMeansOfContact after moving email address to GAPersonnel (TextFree1)
//            string sql = @"
///* select distinct u.Email from flagdnn.dbo.Roles r */
//select distinct /*m.ContactDeviceAddress*/u.Email as Email from flagdnn.dbo.Roles r 
//inner join flagdnn.dbo.UserRoles ur on ur.RoleID=r.RoleID
//inner join flagdnn.dbo.Users u on u.UserID=ur.UserID
//inner join flagdnn.dbo.aspnet_Users au on au.UserName=u.Username
//inner join flagdnn.dbo.aspnet_Membership am on am.UserId=au.UserId and am.IsApproved=1 --and am.IsLockedOut=0
//inner join GAUser fu on fu.DNNUserId=u.Username
//inner join GAEmployment fe on fe.Personnel=fu.PersonnelRowId and fe.FromDate<=GETUTCDATE() and (fe.ToDate is null or fe.ToDate>=GETUTCDATE())
///* for test start */
///*inner join GAPersonnel p on p.PersonnelRowId=fe.Personnel
//inner join GASuperClass s on s.OwnerClass='GAPersonnel' and s.OwnerClassRowId=p.PersonnelRowId and s.MemberClass='GAMeansOfContact'
//inner join GAMeansOfContact m on m.MeansOfContactRowId=s.MemberClassRowId and /*e-mail address*/ m.ContactDeviceTypeListsRowId=(select l.ListsRowId from GALists l where l.GAListCategory='DT' and l.GAListValue='e-mail 1')
//*/
///* for test end */
//where r.PortalID=0 and r.RoleID in ( {0} ) 
//";
            // Tor 20170610 sql query moved to GALists GAListCategory=SYS
            string sql = EMNSQLGAInfoToCommunity;
            if (sql == string.Empty) return null;

            sql = string.Format(sql, communities);
            //            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            System.Collections.ArrayList result = GASystem.DataAccess.DataUtils.executeSelectSpecial(sql, DataUtils.getConnectionString(), "string");
                
//                GASystem.DataAccess.DataUtils.executeSelectSpecial(sql, DataUtils.getConnectionString(), "string");
            return result;
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
            }
        }
        private static string EMNSQLGAInfoToCommunity
        {
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNSQLGAInfoToCommunity"); }
        }


	}
}
