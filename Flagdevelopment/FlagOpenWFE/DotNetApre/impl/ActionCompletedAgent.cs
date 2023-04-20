using System;

namespace GASystem.DotNetApre.impl
{
	/// <summary>
	/// Summary description for ActionCompletedAgent.
	/// </summary>
	public class ActionCompletedAgent : AbstractAgent	
	{
		public ActionCompletedAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
			int actionId = GetActionId(wi);
            // Tor 20141217 Added writeline
            System.Console.WriteLine("Closing workflow with ActionRowId:" + actionId.ToString());

			GASystem.BusinessLayer.Action.SetActionCompleted(actionId);
			return wi;
		}

	}
}
