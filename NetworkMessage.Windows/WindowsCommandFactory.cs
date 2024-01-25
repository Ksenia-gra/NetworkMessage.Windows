using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;
using NetworkMessage.Windows.WindowsCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Windows
{
    public class WindowsCommandFactory : ICommandFactory
    {
        public BaseNetworkCommand CreateAmountOfOccupiedRAMCommand()
        {
            return new WindowsAmountOfOccupiedRAM();
        }

        public BaseNetworkCommand CreateAmountOfRAMCommand()
        {
            throw new NotImplementedException();
        }

        public BaseNetworkCommand CreateBatteryChargePersentageCommand()
        {
            throw new NotImplementedException();
        }

        public BaseNetworkCommand CreateDirectoryInfoCommand(string path)
        {
            throw new NotImplementedException();
        }

        public BaseNetworkCommand CreateDownloadDirectoryCommand(string path)
        {
            throw new NotImplementedException();
        }

        public BaseNetworkCommand CreateDownloadFileCommand(string path)
        {
            return new WindowsDownloadFileCommand(path);
        }

        public BaseNetworkCommand CreateFileInfoCommand(string path)
        {
            throw new NotImplementedException();
        }

        public BaseNetworkCommand CreateGuidCommand()
        {
            return new WindowsGuidCommand();
        }

        public BaseNetworkCommand CreateMacAddressCommand()
        {
            return new WindowsMacAddressCommand();
        }

        public BaseNetworkCommand CreateNestedDirectoriesInfoCommand(string path)
        {
            return new WindowsNestedDirectoriesCommand(path);
        }

        public BaseNetworkCommand CreateNestedFilesInfoCommand(string path)
        {
            return new WindowsNestedFilesInfoCommand(path);
        }

        public BaseNetworkCommand CreatePercentageOfCPUUsageCommand()
        {
            throw new NotImplementedException();
        }

        public BaseNetworkCommand CreateScreenshotCommand()
        {
            throw new NotImplementedException();
        }
    }
}
