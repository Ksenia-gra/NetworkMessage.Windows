using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using NetworkMessage.Intents;
using NetworkMessage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsDownloadDirectoryCommand : BaseNetworkCommand
    {
        public string Path {  get; set; }

        public WindowsDownloadDirectoryCommand(string path) 
        {
            if (!string.IsNullOrWhiteSpace(path) && path.IndexOf("root") == 0)
            {
                path = path.Substring(5);
            }
            Path = path;
        }

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult downloadDirectoryResult;
            try
            {
                if (string.IsNullOrWhiteSpace(Path) || Path == "/")
                {
                    downloadDirectoryResult = new DownloadDirectoryResult(errorMessage: "Directory doesn't exist");
                    return Task.FromResult(downloadDirectoryResult);
                }

                Path = Path[5..];
                Path = Path.Insert(Path.IndexOf('/'), ":");
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                if (!directoryInfo.Exists)
                {
                    downloadDirectoryResult = new DownloadDirectoryResult(errorMessage: "Directory doesn't exist");
                    return Task.FromResult(downloadDirectoryResult);
                }
                downloadDirectoryResult = new DownloadDirectoryResult(Path);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                downloadDirectoryResult = new DirectoryInfoResult("Недопустимый путь", directoryNotFoundException);
            }
            catch (IOException ioException)
            {
                downloadDirectoryResult = new NestedFilesInfoResult("Ошибка при чтении файла", ioException);
            }
            catch (SecurityException securityException)
            {
                downloadDirectoryResult = new NestedFilesInfoResult("Отсутствует необходимое разрешение", securityException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                downloadDirectoryResult = new NestedFilesInfoResult("Отсутствует необходимое разрешение", unauthorizedAccessException);
            }
            catch (Exception exception)
            {
                downloadDirectoryResult = new NestedFilesInfoResult(exception.Message, exception);
            }

            return Task.FromResult(downloadDirectoryResult);
        }
    }
}
