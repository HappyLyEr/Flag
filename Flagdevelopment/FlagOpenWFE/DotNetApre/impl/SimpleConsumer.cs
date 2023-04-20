using System;
using openwfe.workitem;

namespace GASystem.DotNetApre.impl
{
	/// <summary>
	/// Summary description for SimpleConsumer.
	/// </summary>
	public class SimpleConsumer : GASystem.DotNetApre.IConsumer
	{
		public SimpleConsumer()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Returns router for this consumer. Putting it in a seperate method so that
		/// it should be simple to change if needed.
		/// </summary>
		/// <returns></returns>
		protected virtual IRouter lookupRouter()
		{
			return new SimpleRouter();
		}
		
		protected virtual void setAgentResult(int ResultCode, InFlowWorkitem WorkItem)
		{
			WorkItem.attributes[new StringAttribute(AbstractAgent.AGENT_RESULT)] = new IntegerAttribute(ResultCode);
		}
		
		#region IConsumer Members

		public virtual void UseAgent(InFlowWorkitem wi)
		{
			IRouter router = lookupRouter();
			IAgent agent = router.DetermineAgent(wi.participantName);
			int returnCode = 0;
			try
			{
				agent.Use(wi);
 			} catch (System.Exception e)
 			{
				Console.WriteLine(e.Message);
 				returnCode = 1;
 			}
			setAgentResult(returnCode, wi);
		}



		#endregion

		public static string WorkSessionServerAddress
		{
			get 
			{
				//return "localhost"; 
                // Tor 20160308 OWFEWorkSessionServerAddress has to reside in .config file - can differ between servers
				return System.Configuration.ConfigurationSettings.AppSettings.Get("OWFEWorkSessionServerAddress");
			}	
		}

		public static int WorkSessionServerPort
		{
			get 
			{
				//return 5080; 
				// Tor 20160308 return int.Parse(System.Configuration.ConfigurationSettings.AppSettings.Get("OWFEWorkSessionServerPort"));
				return int.Parse(new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEWorkSessionServerPort"));
			}	
		}

		public static string OWFEUserName
		{
			get 
			{
				//return "ga"; 
                // Tor 20160308 return System.Configuration.ConfigurationSettings.AppSettings.Get("OWFEUserName");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEUserName");
            }	
		}
		public static string OWFEPassword
		{
			get 
			{
				//return "bob"; 
                // Tor 20160308 return System.Configuration.ConfigurationSettings.AppSettings.Get("OWFEPassword");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("OWFEPassword");
			}	
		}
        public static string CoordinatorWorkitemRole
        {
            get
            {
                //return "ga"; 
                // Tor 20160308 return System.Configuration.ConfigurationSettings.AppSettings.Get("CoordinatorWorkitemRole");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("CoordinatorWorkitemRole");
            }
        }
        public static string CoordinatorWorkitemGivenName
        {
            get
            {
                //return "ga"; 
                // Tor 20160308 return System.Configuration.ConfigurationSettings.AppSettings.Get("CoordinatorWorkitemGivenName");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("CoordinatorWorkitemGivenName");
            }
        }
        public static string CoordinatorWorkitemFamilyName
        {
            get
            {
                //return "ga"; 
                // Tor 20160308 return System.Configuration.ConfigurationSettings.AppSettings.Get("CoordinatorWorkitemFamilyName");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("CoordinatorWorkitemFamilyName");
            }
        }
        public static int CoordinatorWorkitemPersonnelRowId
        {
            get
            {
                //return "ga"; 
                // Tor 20160308 return int.Parse(System.Configuration.ConfigurationSettings.AppSettings.Get("CoordinatorWorkitemPersonnelRowId"));
                return int.Parse(new GASystem.AppUtils.FlagSysResource().GetResourceString("CoordinatorWorkitemPersonnelRowId"));
            }
        }
    }
}
