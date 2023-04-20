using openwfe.workitem;

namespace GASystem.DotNetApre
{
	public interface IConsumer
	{
		void UseAgent(InFlowWorkitem wi);
	}
}