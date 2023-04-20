using System;

namespace GASystem.DotNetApre
{
	/// <summary>
	/// Summary description for IAgentBuilder.
	/// </summary>
	public interface IAgentBuilder
	{
		IAgent Build(string AgentBuilder);
	}
}
