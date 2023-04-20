using System;
//using System.Web.Mail;
using System.Net.Mail;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for FlagSMTP.
    /// JOF 20130402: new version using system.net.mail 
	/// </summary>
	public class FlagSMTP
	{
		public static string EMAILLISTSVALUE = "e-mail 1";

		public FlagSMTP()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public static void SendEmailMsg(string To,  string Subject, string TextBody, bool UseHtmlFormat) 
		{
            SendEmailMsg(To, string.Empty, Subject, TextBody, UseHtmlFormat);
		}

        public static void SendEmailMsg(string To, string CarbonCopy, string Subject, string TextBody, bool UseHtmlFormat)
        {
            char SplitCharacter = ',';
            System.Collections.ArrayList aTo = new System.Collections.ArrayList();
            System.Collections.ArrayList aCC = new System.Collections.ArrayList();
            CopyEmailAddressesToArray(ref aTo,To,SplitCharacter);
            CopyEmailAddressesToArray(ref aCC,CarbonCopy,SplitCharacter);

            SendEmailMsg(aTo, aCC, null, Subject, TextBody, UseHtmlFormat);
// Tor 20160331 The rest of method moved to same method with array lists
//            MailAddress from = new MailAddress(SMTPFromAddress);
//            MailAddress to = new MailAddress(To);
////            MailMessage message= new MailMessage(
//            MailMessage message = new MailMessage(from, to);
//            if (CarbonCopy != string.Empty)
//            {            // Add a carbon copy recipient.
//                MailAddress copy = new MailAddress(CarbonCopy);
//                message.CC.Add(copy);
//            }

//            message.Subject = Subject;
//            message.Body = TextBody;
//            message.IsBodyHtml = UseHtmlFormat;

//            SmtpClient client = new SmtpClient(SMTPServer);
//            client.Port = int.Parse(SMTPPort);
//            client.Credentials = new System.Net.NetworkCredential(SMTPUserName, SMTPPassword);

//            try
//            {
//                client.Send(message);
//            }
//            catch (Exception ex)
//            {
//                //   Console.WriteLine("Exception caught in CreateTestMessage1(): {0}", ex.ToString());

//                //TODO:   add logging

//            }
        }
        private static void CopyEmailAddressesToArray(ref System.Collections.ArrayList list, string element,char SplitCharacter)
        {
            string[] EmailTo = element.Split(SplitCharacter);
            foreach (string Element in EmailTo)
            {
                AddElementToArrayIfNotAlreadyThere(ref list,Element);
            }                            
        }
        // Tor 20170426 change from privat to public
        public static void AddElementToArrayIfNotAlreadyThere(ref System.Collections.ArrayList list, string element)
        {
            if (element != string.Empty)
            {
                // check if email address already in list before adding
                bool found = false;
                foreach (string a in list)
                {
                    if (a == element) found = true;
                }
                if (!found) list.Add(element);
            }
            return;
        }


        public static void SendEmailMsg(System.Collections.ArrayList To, System.Collections.ArrayList CarbonCopy, System.Collections.ArrayList BlindCopy, string Subject, string TextBody, bool UseHtmlFormat)
        {
            MailMessage message = new MailMessage();
//            MailAddress from = new MailAddress(SMTPFromAddress);

            message.From = new MailAddress(cleanupEmailAddress(SMTPFromAddress));

            if (To != null)
            {
                foreach (string address in To)
                {
                    if (address != string.Empty)
                    {
                        // Tor 20170327 cleanup address MailAddress a = new MailAddress(address);
                        MailAddress a = new MailAddress(cleanupEmailAddress(address));
                        message.To.Add(a);
                    }
                }
            }
            if (CarbonCopy != null)
            {
                foreach (string address in CarbonCopy)
                {
                    if (address != string.Empty)
                    {
                        // Tor 20170327 cleanup address MailAddress a = new MailAddress(address);
                        MailAddress a = new MailAddress(cleanupEmailAddress(address));
                        message.CC.Add(a);
                    }
                }
            }

            if (BlindCopy != null)
            {
                foreach (string address in BlindCopy)
                {
                    if (address != string.Empty)
                    {
                        // Tor 20170327 cleanup address MailAddress a = new MailAddress(address);
                        MailAddress a = new MailAddress(cleanupEmailAddress(address));
                        message.Bcc.Add(a);
                    }
                }
            }

            message.Subject = Subject;
            message.Body = TextBody;
            message.IsBodyHtml = UseHtmlFormat;

            SmtpClient client = new SmtpClient(SMTPServer);
            client.Port = int.Parse(SMTPPort);
            client.Credentials = new System.Net.NetworkCredential(SMTPUserName, SMTPPassword);

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                //   Console.WriteLine("Exception caught in CreateTestMessage1(): {0}", ex.ToString());

                //TODO:   add logging

            }
        }

        
        // Tor 20130925 added SendEmailMsg overload method to create and send message to several recipients
        public static void SendEmailMsg(MailMessage message, bool firstAddress, bool lastAddress, string To, string Subject, string TextBody, bool UseHtmlFormat)
        {
            if (firstAddress) // create message object on first to e-mail address, including first To address
            {
                string cleanToAddress = cleanupEmailAddress(To);
                // Tor 20170328 MailAddress from = new MailAddress(SMTPFromAddress);
                MailAddress from = new MailAddress(cleanupEmailAddress(SMTPFromAddress));
                MailAddress to = new MailAddress(cleanToAddress);
                //MailAddress to = new MailAddress(To);
                //message.From.Address = from;
                message.From = from;
                message.To.Add(to);
//                message = new MailMessage(from, to);

                message.Subject = Subject;
                message.Body = TextBody;
                message.IsBodyHtml = UseHtmlFormat;
            }
            else
            {
                if (lastAddress) // last address has been transferred, send e-mail to all recipients
                {
                    SmtpClient client = new SmtpClient(SMTPServer);
                    client.Port = int.Parse(SMTPPort);
                    client.Credentials = new System.Net.NetworkCredential(SMTPUserName, SMTPPassword);

                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception caught in CreateTestMessage1(): {0}", ex.ToString());
                        //TODO:   add logging
                    }
                }
                else // add address to recipients when not firts address and not last address
                {
                    // Tor 20170328 string a = cleanupEmailAddress(To); message.To.Add(a);
                    message.To.Add(cleanupEmailAddress(To));
                }
            }
        }

        public static string cleanupEmailAddress(string emailAddress)
        {
            // Tor 20170326 when address has url format '<a href="mailto:ola.Nordmann@aaa.com">Ola Nordmann</a>'
            // change <a href="mailto:t-hesk@online.no">Tor Heskje</a> to t-hesk@online.no
            string cleanAddress = emailAddress;
            if (emailAddress.IndexOf("<a href=") > -1)
            {
                cleanAddress = emailAddress.Substring(emailAddress.IndexOf(":") + 1, emailAddress.IndexOf(">") - 2 - emailAddress.IndexOf(":"));
            }
            return cleanAddress;
        }

		/// <summary>
		/// send email message to login.
		/// Sends message to  default sms receiver if login can not be match to an mobile number.
		/// </summary>
		/// <param name="LoginId"></param>
		/// <param name="TextMessage"></param>
		
        // Tor 20130925 this method is no longer in use
        //public static void SendMessage(string LoginId, string Subject, string TextBody, bool UseHtmlFormat) 
        //{
        //    int PersonnelId = User.GetPersonnelIdByLogonId(LoginId);
        //    SendMessage(PersonnelId, Subject, TextBody, UseHtmlFormat);
        //}

        /// <summary>
        /// send email message to login.
        /// Sends message to  default sms receiver if login can not be match to an mobile number.
        /// </summary>
        /// <param name="LoginId"></param>
        /// <param name="TextMessage"></param>
        public static void SendMessage(int PersonnelRowId, string Subject, string TextBody, bool UseHtmlFormat)
        {
            string toAddress;
            toAddress = GetEmailForPersonnelRowId(PersonnelRowId);
            SendEmailMsg(toAddress, Subject, TextBody, UseHtmlFormat);
        }

        /// <summary>
        /// Get email address for personnel rowid
        /// throws an exeption if login can not be match by email address
        /// </summary>
        /// <returns>String containg a email address</returns>
        //private static string GetEmailForPersonnelRowId(int PersonnelRowId)
        public static string GetEmailForPersonnelRowId(int PersonnelRowId)
        {
            string emailaddress = Personnel.GetPersonnelEmailAddress(PersonnelRowId);
            // Tor 20170325 email address moved to GAPersonnel string emailaddress = MeansOfContact.GetContactAddressByOwnerAndDeviceTypeId(PersonnelRowId, DataModel.GADataClass.GAPersonnel, EMAILLISTSVALUE, null);
            if (emailaddress == string.Empty)
// Tor 20160225                throw new GAExceptions.GAException("No means of contact found for login");
                throw new GAExceptions.GAException("No means of contact found for login with PersonnelRowId: "+PersonnelRowId.ToString());
            return emailaddress;
        }


		private static string SMTPServer 
		{
// Tor 20160225            get { return System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServer"); }
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPServer"); }

		}

		private static string SMTPFromAddress 
		{
            // Tor 20160225            get { return System.Configuration.ConfigurationManager.AppSettings.Get("SMTPFromAddress"); }
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPFromAddress"); }
		}

        private static string  SMTPUserName
        {
// Tor 20160225            get { return System.Configuration.ConfigurationManager.AppSettings.Get("SMTPUserName"); }
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPUserName"); }
        }

        private static string SMTPPassword
        {
// Tor 20160225            get { return System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPassword"); }
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPPassword"); }
        }


		private static string SMTPPort 
		{
			get 
			{
// Tor 20160225                if (System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPort") == null)
                if (new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPPort") == null)
                        return "25";
// Tor 20160525				return System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPort");	
                    return new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPPort"); 
			}
		}
	}
}
