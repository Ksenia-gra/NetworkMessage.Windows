using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsBatteryChargePersentageCommand : BaseNetworkCommand
    {
        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult butteryChargePercent;
            ManagementClass powerClass = new ManagementClass("Win32_Battery");
            ManagementObjectCollection powerDevices = powerClass.GetInstances();
            byte batteryLevel = 100;
            try
            {
                foreach (ManagementObject powerDevice in powerDevices)
                {
                    if (powerDevice == null && powerDevice["EstimatedChargeRemaining"] == null) break;
                    batteryLevel = Convert.ToByte(powerDevice["EstimatedChargeRemaining"]);
                }
                butteryChargePercent = new BatteryChargeResult(batteryLevel);
            }
            catch(Exception ex)
            {
                butteryChargePercent = new BatteryChargeResult(ex.Message,ex);
            }
            
            return Task.FromResult(butteryChargePercent);
        }
    }
}
