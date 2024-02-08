using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using NickStrupat;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsAmountOfRAMCommand : BaseNetworkCommand
    {
        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            ComputerInfo computerInfo = new ComputerInfo();
            float totalMemoryAmount = (float)Math.Round((computerInfo.TotalPhysicalMemory / 1024.0 / 1024.0 / 1024.0), 1);
            BaseNetworkCommandResult totalMemoryRes = new AmountOfRAMResult(totalMemoryAmount);
            return Task.FromResult(totalMemoryRes);
        }
    }
}
