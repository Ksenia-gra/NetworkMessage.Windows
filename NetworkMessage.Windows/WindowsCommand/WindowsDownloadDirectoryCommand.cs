using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using System.IO;
using System.Security;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsDownloadDirectoryCommand : BaseNetworkCommand
    {
        public string Path {  get; set; }

        public WindowsDownloadDirectoryCommand(string path) 
        {
            const string root = "root";
            if (!string.IsNullOrWhiteSpace(path) && path.IndexOf(root) == 0)
            {
                path = path[root.Length..].Replace('\\', '/').ToLower();
                if (!string.IsNullOrWhiteSpace(path))
                {
                    if (path.First() == '/')
                    {
                        path = path[1..];
                    }
                    
                    if (path.LastOrDefault() != '/')
                    {
                        path += '/';
                    }
                }
            }

            Path = path;
        }

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult downloadDirectoryResult;
            try
            {
                const string disk = "disk_";
                if (string.IsNullOrWhiteSpace(Path) || Path == "/")
                {
                    downloadDirectoryResult = new DownloadDirectoryResult(errorMessage: "Directory doesn't exist");
                    return Task.FromResult(downloadDirectoryResult);
                }

                Path = Path[disk.Length..];
                Path = Path.Insert(Path.IndexOf('/'), ":");
                if (Path.Last() == '/')
                {
                    Path = Path[..^1];
                }
                
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
