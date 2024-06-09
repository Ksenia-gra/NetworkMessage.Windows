using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using Microsoft.Win32;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsGuidCommand : BaseNetworkCommand
    {
        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
			string key = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Cryptography";
			string guid = (string)Registry.GetValue(key, "MachineGuid", (object)"default");
			BaseNetworkCommandResult guidResult = new GuidResult(guid);
            return Task.FromResult(guidResult);
        }
    }
}
