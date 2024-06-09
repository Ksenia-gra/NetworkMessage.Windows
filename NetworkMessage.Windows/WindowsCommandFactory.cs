using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;
using NetworkMessage.Windows.WindowsCommand;

namespace NetworkMessage.Windows
{
	public class WindowsCommandFactory : ICommandFactory
	{
			public INetworkCommand CreateAmountOfOccupiedRAMCommand()
			{
				return new WindowsAmountOfOccupiedRAM();
			}

			public INetworkCommand CreateAmountOfRAMCommand()
			{
				return new WindowsAmountOfRAMCommand();
			}

			public INetworkCommand CreateBatteryChargePersentageCommand()
			{
				return new WindowsBatteryChargePersentageCommand();
			}

			public INetworkCommand CreateDirectoryInfoCommand(string path)
			{
				return new WindowsDirectoryInfoCommand(path);
			}

			public INetworkCommand CreateDownloadDirectoryCommand(string path)
			{
				return new WindowsDownloadDirectoryCommand(path);
			}

			public INetworkCommand CreateDownloadFileCommand(string path)
			{
				return new WindowsDownloadFileCommand(path);
			}

			public INetworkCommand CreateDrivesInfoCommand()
			{
				throw new NotImplementedException();
			}

			public INetworkCommand CreateFileInfoCommand(string path)
			{
				return new WindowsFileInfoCommand(path);
			}

			public INetworkCommand CreateGuidCommand()
			{
				return new WindowsGuidCommand();
			}

			public INetworkCommand CreateMacAddressCommand()
			{
				return new WindowsMacAddressCommand();
			}

			public INetworkCommand CreateNestedDirectoriesInfoCommand(string path)
			{
				return new WindowsNestedDirectoriesCommand(path);
			}

			public INetworkCommand CreateNestedFilesInfoCommand(string path)
			{
				return new WindowsNestedFilesInfoCommand(path);
			}

			public INetworkCommand CreatePercentageOfCPUUsageCommand()
			{
				return new WindowsPercentageOfCPUUsageCommand();
			}

			public INetworkCommand CreateScreenshotCommand()
			{
				return new WindowsSceenshotCommand();
			}

			public INetworkCommand CreateRunningProgramsCommand()
			{
				return new WindowsRunningProgramsCommand();
			}

			public INetworkCommand CreateStartProgramCommand(string path)
			{
				throw new NotImplementedException();
			}
	}
}
