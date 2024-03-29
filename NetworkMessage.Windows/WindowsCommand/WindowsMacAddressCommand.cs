﻿using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using System.Net.NetworkInformation;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsMacAddressCommand : BaseNetworkCommand
    {
        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult result;
            try
            {
                string macAddress = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(x => x.OperationalStatus == OperationalStatus.Up)
                    .OrderByDescending(x => x.GetIPStatistics().BytesSent + x.GetIPStatistics().BytesReceived)
                    .FirstOrDefault()?.GetPhysicalAddress().ToString();
                if (macAddress == null) { }
                result = new MacAddressResult(macAddress);

            }
            catch (NetworkInformationException netEx)
            {
                result = new MacAddressResult("Ошибка получения информации о мак-адресе", netEx);
            }
            catch (Exception ex)
            { 
                result = new MacAddressResult(ex.Message, ex);
            }
            return Task.FromResult(result);
        }
    }
}
