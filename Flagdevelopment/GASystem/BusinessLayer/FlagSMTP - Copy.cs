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

            MailAddress from = new MailAddress(SMTPFromAddress);
            MailAddress to = new MailAddress(To);
            MailMessage message = new MailMessage(from, to);
            
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
            
            //MailMessage emailMsg = new MailMessage();
            //emailMsg.To       = To;
            //emailMsg.From     = SMTPFromAddress;
            //emailMsg.Subject  = Subject;
            //emailMsg.Body     = TextBody;
            //if (UseHtmlFormat)
            //    emailMsg.BodyFormat = MailFormat.Html;
            //emailMsg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", SMTPPort);
		
            //SmtpMail.SmtpServer = SMTPServer;
			
			
            ////TODO add try catch
            //SmtpMail.Send(emailMsg);

           
		}

        // Tor 20130925 added SendEmailMsg overload method to create and send message to several recipients
        public static void SendEmailMsg(MailMessage message, bool firstAddress, bool lastAddress, string To, string Subject, string TextBody, bool UseHtmlFormat)
        {
            if (firstAddress) // create message object on first to e-mail address, including first To address
            {
                MailAddress from = new MailAddress(SMTPFromAddress);
                MailAddress to = new MailAddress(To);
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
                    message.To.Add(To);
                }
                
            }

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
            string emailaddress = MeansOfContact.GetContactAddressByOwnerAndDeviceTypeId(PersonnelRowId, DataModel.GADataClass.GAPersonnel, EMAILLISTSVALUE, null);
            if (emailaddress == string.Empty)
                throw new GAExceptions.GAException("No means of contact found for login");
            return emailaddress;
        }


		private static string SMTPServer 
		{
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServer"); }
		}

		private static string SMTPFromAddress 
		{
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("SMTPFromAddress"); }
		}

        private static string  SMTPUserName
        {
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("SMTPUserName"); }
        }

        private static string SMTPPassword
        {
            get { return System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPassword"); }
        }


		private static string SMTPPort 
		{
			get 
			{
                if (System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPort") == null) 
					return "25";
				
				return System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPort");	
			}
		}



	}
}
