using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using System.Diagnostics;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsPercentageOfCPUUsageCommand : BaseNetworkCommand
    {
        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            float sum = 0;
            for (int i = 0; i < 100; i++)
            {
                float cpuUsage = cpuCounter.NextValue();
                sum += cpuUsage;
                Thread.Sleep(10);
            }

            BaseNetworkCommandResult cpuResult = new PercentageOfCPUUsageResult((byte)(sum/100));
            return Task.FromResult(cpuResult);
        }
    }
}
