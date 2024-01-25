using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using System;
using System.CodeDom.Compiler;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NickStrupat;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsAmountOfOccupiedRAM : BaseNetworkCommand
    {
        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            ComputerInfo computerInfo = new ComputerInfo();
            float totalMemoryAmount = (float)Math.Round((computerInfo.TotalPhysicalMemory / 1024.0 / 1024.0 / 1024.0), 1);
            float availableMemoryAmount = (float)Math.Round((computerInfo.AvailablePhysicalMemory / 1024.0 / 1024.0 / 1024.0), 1);
            BaseNetworkCommandResult totalOccupiedMemory = new AmountOfOccupiedRAMResult(totalMemoryAmount - availableMemoryAmount);
            return Task.FromResult(totalOccupiedMemory);
        }
    }
}
