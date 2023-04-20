using System;

namespace GASystem.DotNetApre
{
	/// <summary>
	/// Summary description for IRouter.
	/// </summary>
	public interface IRouter
	{
		IAgent DetermineAgent(string ParticipantName);
	}
}
