using System;
using openwfe.workitem;

namespace GASystem.DotNetApre
{
	
	
	
	
	/// <summary>
	/// Summary description for IAgent.
	/// </summary>
	public interface IAgent
	{
		InFlowWorkitem Use(InFlowWorkitem wi);
		void SetAgentResult(int ResultCode, InFlowWorkitem wi);
	}
}
