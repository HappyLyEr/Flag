using System;

namespace GASystem.DotNetApre.impl
{
	/// <summary>
	/// Summary description for SimpleAgentBuilder.
	/// </summary>
	public class SimpleAgentBuilder : IAgentBuilder
	{
		public const string SMSAGENT = "gaagent-smsagent";
		public const string ACTIONCOMPLETEDAGENT = "gaagent-actioncompletedagent";
		public const string UPDATEDBFIELDAGENT = "gaagent-updatedbfieldagent";
		public const string GETDBFIELDAGENT = "gaagent-getdbfieldagent";
		public const string SMTPAGENT = "gaagent-smtpagent";	
		public const string GETUSERIDFROMDB	= "gaagent-getuseridfromdbfieldagent";			  
		public const string GETRESPONSIBLEAGENT	= "gaagent-getresponsibleagent";
        public const string GETOWNERAGENT = "gaagent-getowneragent";
        public const string BROADCASTSMTPAGENT = "gaagent-broadcastsmtpagent";
        // Tor 20140227 Added method to get action ownerclass from list of requested classes 
        public const string GETOWNERCLASSAGENT = "gaagent-getownerclassagent";
        // Tor 20150623 Added method to create new record from another record 
        public const string CREATENEWRECORDFROMRECORDAGENT = "gaagent-createnewrecordfromrecordagent";
        // Tor 20150708 Added method to send e-mail notifications to flagdnn community members 
        public const string SMTPTOCOMMUNITYMEMBERSAGENT = "gaagent-smtptocommunitymembersagent";
        // Tor 20160624 Added method to get action owner reference id value
        public const string GETOWNERREFERENCEIDAGENT = "gaagent-getownerreferenceidagent";	
        // Tor 20160624 Added method to get action owner reference id value
        public const string GETOWNEROBJECTNAMEAGENT = "gaagent-getownerobjectnameagent";	
        // Tor 20160624 Added method to get action owner reference id value
        public const string GETOWNERCLASSNAMEAGENT = "gaagent-getownerclassnameagent";
        // Tor 20170407 Added method to update GAOpportunity from GAOpportunityDetails on request from Phil Bigg and Daniella Bordon
        public const string OPPORTUNITYGOPUBLICAGENT = "gaagent-opportunitygopublicagent";
        public const string GETLISTDESCRIPTIONFROMCODEVALUE = "gaagent-getlistdescriptionfromcodevalue";
        // Tor 20190224 Added method to update date field in Action owner record
        public const string UPDATEDATEAGENT = "gaagent-updatedateagent";
        // Tor 20190622 Added method to debug workflows
        public const string DEBUGAGENT = "gaagent-debugagent";
        

        public SimpleAgentBuilder()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region IAgentBuilder Members

		public IAgent Build(string AgentBuilder)
		{
			if (AgentBuilder == SMSAGENT)
				return new SMSAgent();
			if (AgentBuilder == ACTIONCOMPLETEDAGENT)
				return new ActionCompletedAgent();
			if (AgentBuilder == UPDATEDBFIELDAGENT)
				return new UpdateDBFieldAgent();
			if (AgentBuilder == GETDBFIELDAGENT)
				return new GetDBFieldAgent();
			if (AgentBuilder == SMTPAGENT)
				return new SMTPAgent();
            if (AgentBuilder == BROADCASTSMTPAGENT)
                return new BroadcastSMTPAgent();
			if (AgentBuilder == GETUSERIDFROMDB)
				return new GetUserIdFromDBFieldAgent();
            if (AgentBuilder == GETRESPONSIBLEAGENT)
                return new GetResponsibleAgent();
            if (AgentBuilder == GETOWNERAGENT)
                return new GetOwnerAgent();
            // Tor 20140228 added method to return action ownerclass : input paremeter is class list to look for. searches in path from right to left
            if (AgentBuilder == GETOWNERCLASSAGENT)
                return new GetOwnerclassAgent();
            // Tor 20150623 Added method to create new record from another record : INPUT: actionRowId (get action owner) and class to create
            if (AgentBuilder == CREATENEWRECORDFROMRECORDAGENT)
                return new CreateNewRecordFromRecordAgent();
            // Tor 20150708 Added method to send e-mail notifications to flagdnn community members 
            if (AgentBuilder == SMTPTOCOMMUNITYMEMBERSAGENT)
                return new SmtpToCommunityMembersAgent();
            // Tor 20160624 added method Add agents getOwnerReferenceId
            if (AgentBuilder == GETOWNERREFERENCEIDAGENT)
                return new GetOwnerReferenceIdAgent();
            // Tor 20160624 added method Add agents getOwnerName
            if (AgentBuilder == GETOWNEROBJECTNAMEAGENT)
                return new GetOwnerObjectNameAgent();
            if (AgentBuilder == GETOWNERCLASSNAMEAGENT)
                return new GetOwnerClassNameAgent();
            // Tor 20170407 added method to update GAOpportunity from GAOpportunityDetail on request from Phil Bigg and Daniella Bordon
            if (AgentBuilder == OPPORTUNITYGOPUBLICAGENT)
                return new OpportunityGoPublicAgent();
            if (AgentBuilder == GETLISTDESCRIPTIONFROMCODEVALUE)
                return new GetListDescriptionFromCodeValue();
            // Tor 20191127 rest removed
            //// Tor 20190224 method for updating date field in Action owner
            //if (AgentBuilder == UPDATEDBFIELDAGENT)
            //    return new UpdateDateAgent();
            //if (AgentBuilder == DEBUGAGENT)
            //    return new DebugAgent();
            return null;
		}

		#endregion
	}
}
