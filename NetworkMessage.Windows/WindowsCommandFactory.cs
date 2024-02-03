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
            return new WindowsAmountOfRAMCommand();
        }

        public BaseNetworkCommand CreateBatteryChargePersentageCommand()
        {
            return new WindowsBatteryChargePersentageCommand();
        }

        public BaseNetworkCommand CreateDirectoryInfoCommand(string path)
        {
            return new WindowsDirectoryInfoCommand(path);
        }

        public BaseNetworkCommand CreateDownloadDirectoryCommand(string path)
        {
            return new WindowsDownloadDirectoryCommand(path);
        }

        public BaseNetworkCommand CreateDownloadFileCommand(string path)
        {
            return new WindowsDownloadFileCommand(path);
        }

        public BaseNetworkCommand CreateFileInfoCommand(string path)
        {
            return new WindowsFileInfoCommand(path);
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
            return new WindowsPercentageOfCPUUsageCommand();
        }

        public BaseNetworkCommand CreateScreenshotCommand()
        {
            return new WindowsSceenshotCommand();
        }
    }
}
