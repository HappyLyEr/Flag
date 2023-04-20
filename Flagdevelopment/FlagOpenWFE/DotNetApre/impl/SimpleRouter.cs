using System;
using GASystem.DotNetApre;

namespace GASystem.DotNetApre.impl
{
	/// <summary>
	/// Summary description for SimpleRouter.
	/// </summary>
	public class SimpleRouter : IRouter
	{
		public SimpleRouter()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region IRouter Members

		public IAgent DetermineAgent(string ParticipantName)
		{
			IAgentBuilder agentBuilder = new SimpleAgentBuilder();
			return agentBuilder.Build(ParticipantName);
		}

		#endregion
	}
}
