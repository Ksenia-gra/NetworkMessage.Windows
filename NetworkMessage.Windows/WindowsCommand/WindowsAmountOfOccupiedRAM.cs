using Microsoft.VisualBasic.Devices;
using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsAmountOfOccupiedRAM : BaseNetworkCommand
    {
        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
			float freeRam = (float)Math.Round(new ComputerInfo().AvailablePhysicalMemory / 1024.0 / 1024.0 / 1024.0, 1);
			GCMemoryInfo gcMemoryInfo = GC.GetGCMemoryInfo();
			long totalMemory = gcMemoryInfo.TotalAvailableMemoryBytes;
			float totalMemoryAmount = (float)Math.Ceiling(totalMemory / 1024.0 / 1024.0 / 1024.0);
			BaseNetworkCommandResult totalMemoryRes = new AmountOfOccupiedRAMResult(totalMemoryAmount - freeRam);
			return Task.FromResult(totalMemoryRes);
		}
    }
}
