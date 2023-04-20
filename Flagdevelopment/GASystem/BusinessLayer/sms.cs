using System;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for sms.
	/// </summary>
	public class sms
	{
		public sms()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void SendTextMessage(string MobileNumber, string TextMessage) 
		{
			PSWinCom.Gateway.Client.SMSClient gw = new PSWinCom.Gateway.Client.SMSClient();
			gw.Username = SMSGatewayUsername; //"norsol"; // your username
			gw.Password = SMSGatewayPassword; //"Bcvvhdms"; // your password
			gw.PrimaryGateway = SMSGatewayURL; //"http://sms3.pswin.com/sms";

            // Tor 20190518 Do not send if any of the sms logon parameters are null
            if (!(string.IsNullOrEmpty(gw.PrimaryGateway) || string.IsNullOrEmpty(gw.Password) || string.IsNullOrEmpty(gw.Username)))
//			try 
			{
				// Create a message object
				PSWinCom.Gateway.Client.Message m = new PSWinCom.Gateway.Client.Message();
				m.ReceiverNumber = MobileNumber;
				m.SenderNumber = SMSSenderString;
				m.Text = TextMessage;
				m.RequestReceipt = false; // Set to true for receipt
				// Add message object to Messages collection of client object
				gw.Messages.Add(1, m);

				// Send all messages in Messages collection. This method is blocking. After completed,
				// each message will have Status updated to reflect the result of send-operation.
				gw.SendMessages();
				// Reference will only be available if activated on account.
				//txtTraceLog.Text += "Message done. Status=" + m.Status.ToString() + " Reference=" + m.Reference + "\n";
            }  
//			catch  {}
		}

		/// <summary>
		/// send textmessage to login.
		/// Sends message to  default sms receiver if login can not be match to an mobile number.
		/// </summary>
		/// <param name="LoginId"></param>
		/// <param name="TextMessage"></param>
		public static void SendMessage(string LoginId, string TextMessage) 
		{
//			int UserId = User.GetUserIdByLogonId(LoginId);
//			BusinessLayer.MeansOfContact.GetMeansOfContactsByOwner(new DataModel.GADataRecord(UserId, ga
//			
//			GASystem.DataModel.UserPersonnelViewDS ds =  UserPersonnelView.GetUserPersonnelViewByLogonId(LoginId);
//			if (ds.GAUserPersonnelView.Rows.Count > 0)
//				SendTextMessage(ds.GAUserPersonnelView[0].ContactDeviceAddress, TextMessage);
			string mobileNumber;
			try 
			{
				mobileNumber = GetMobileNumberForLoginId(LoginId);
			} 
			catch (GAExceptions.GAException gae) 
			{
				mobileNumber = SMSDefaultReceiver;
				TextMessage +=  "-- You have received this message because user " + LoginId + " could not be found";
			}
			SendTextMessage(mobileNumber, TextMessage);
		}

		/// <summary>
		/// Get mobile number for Login
		/// throws an exeption if login can not be match by mobile number
		/// </summary>
		/// <returns>String containg a mobile number</returns>
		private static string GetMobileNumberForLoginId(string LoginId) 
		{
			int PersonnelId = User.GetPersonnelIdByLogonId(LoginId);
            string mobilePhoneNumber = Personnel.GetPersonnelMobilePhoneNumber(PersonnelId);
            // Tor 20170325 mobile phone number moved to GAPersonnel from GAMeansOfContact
            //DataModel.MeansOfContactDS ds =  MeansOfContact.GetMeansOfContactsByOwnerAndDeviceTypeId(PersonnelId, DataModel.GADataClass.GAPersonnel, "Mobile phone");
            //if (ds.GAMeansOfContact.Rows.Count == 0) 
            //    throw new GAExceptions.GAException("No means of contact found for login");
            //return ds.GAMeansOfContact[0].ContactDeviceAddress;

            if (mobilePhoneNumber == string.Empty)
                throw new GAExceptions.GAException("No mobile phone number found for user with login id: " +LoginId);
            return mobilePhoneNumber;
			
		}

		private static string SMSGatewayURL 
		{
			// Tor 20160303 get {return System.Configuration.ConfigurationManager.AppSettings.Get("SMSGatewayURL");	}
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("SMSGatewayURL"); }

		}
		private static string SMSGatewayUsername 
		{
			// Tor 20160303 get {return System.Configuration.ConfigurationManager.AppSettings.Get("SMSGatewayUsername");	}
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("SMSGatewayUsername"); }
		}
		private static string SMSGatewayPassword
		{
			// Tor 20160303 get {return System.Configuration.ConfigurationManager.AppSettings.Get("SMSGatewayPassword");	}
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("SMSGatewayPassword"); }
		}
		
		private static string SMSDefaultReceiver 
		{
			// Tor 20160303 get {return System.Configuration.ConfigurationManager.AppSettings.Get("SMSDefaultReceiver");	}
            get { return new GASystem.AppUtils.FlagSysResource().GetResourceString("SMSDefaultReceiver"); }
		}

		private static string SMSSenderString 
		{
			get 
			{
				// Tor 20160303 if (System.Configuration.ConfigurationManager.AppSettings.Get("SMSSenderString") != null)
                if (new GASystem.AppUtils.FlagSysResource().GetResourceString("SMSSenderString") != null)
					// Tor 20160303 return System.Configuration.ConfigurationManager.AppSettings.Get("SMSSenderString");
                    return new GASystem.AppUtils.FlagSysResource().GetResourceString("SMSSenderString");
				return "Flag"; //Default value used if config value is missing
			}
		}



	}
}

