using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsAmountOfRAMCommand : BaseNetworkCommand
    {
        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
			GCMemoryInfo gcMemoryInfo = GC.GetGCMemoryInfo();
			long totalMemory = gcMemoryInfo.TotalAvailableMemoryBytes;
			float totalMemoryAmount = (float)Math.Ceiling(totalMemory / 1024.0 / 1024.0 / 1024.0);
            BaseNetworkCommandResult totalMemoryRes = new AmountOfRAMResult(totalMemoryAmount);
            return Task.FromResult(totalMemoryRes);
        }
    }
}
