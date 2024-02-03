using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NickStrupat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
