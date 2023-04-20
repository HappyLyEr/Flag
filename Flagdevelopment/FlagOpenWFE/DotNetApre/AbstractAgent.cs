using System;
using openwfe.workitem;

namespace GASystem.DotNetApre
{
	/// <summary>
	/// Summary description for AbstractAgent.
	/// </summary>
	public abstract class AbstractAgent : IAgent 	
	{
		public const string AGENT_RESULT = "__agent_result__";
		public const string GA_ACTIONID = "__gaactionid__";
		

		
		public AbstractAgent()
		{
			//
			// TODO: Add constructor logic here
			//

		}

		public abstract InFlowWorkitem Use(InFlowWorkitem wi);

		public void SetAgentResult(int ResultCode, InFlowWorkitem WorkItem)
		{
			WorkItem.attributes[new StringAttribute(AGENT_RESULT)] = new IntegerAttribute(ResultCode);
		}

		public int GetActionId(InFlowWorkitem wi)
		{
			//return ((IntegerAttribute)wi.attributes[new StringAttribute(GA_ACTIONID)]).Value;	
			if( GASystem.AppUtils.GAUtils.IsNumeric(wi.attributes[new StringAttribute(GA_ACTIONID)].ToString()))
				return System.Convert.ToInt32(wi.attributes[new StringAttribute(GA_ACTIONID)].ToString());
			return 0;
		}
	}
}
