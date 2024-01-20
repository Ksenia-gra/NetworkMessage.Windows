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
    internal class WindowsCommandFactory : ICommandFactory
    {
        public BaseNetworkCommand CreateAmountOfOccupiedRAMCommand()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public BaseNetworkCommand CreateMacAddressCommand()
        {
            throw new NotImplementedException();
        }

        public BaseNetworkCommand CreateNestedDirectoriesInfoCommand(string path)
        {
            throw new NotImplementedException();
        }

        public BaseNetworkCommand CreateNestedFilesInfoCommand(string path)
        {
            throw new NotImplementedException();
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
