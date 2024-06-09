using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using System.IO;
using System.Security;
using NetworkMessage.DTO;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsDirectoryInfoCommand : BaseNetworkCommand
    {
        public string Path { get; set; }

        public WindowsDirectoryInfoCommand(string path) 
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

        public override async Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult directoryInfoResult;       
            try
            {
                const string disk = "disk_";
                if (string.IsNullOrWhiteSpace(Path) || Path == "/")
                {
                    directoryInfoResult = new DirectoryInfoResult(errorMessage: "Incorrect path");
                    return directoryInfoResult;
                }

                Path = Path[disk.Length..];
                Path = Path.Insert(Path.IndexOf('/'), ":"); 
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                if (!directoryInfo.Exists)
                {
                    directoryInfoResult = new DirectoryInfoResult(errorMessage: "Directory doesn't exist");
                    return directoryInfoResult;
                }
                
                DateTime creationTime = directoryInfo.CreationTimeUtc;
                DateTime changingDate = directoryInfo.LastWriteTimeUtc;
                string[] splited = directoryInfo.FullName.Split(":");
                string fullName = "Disk_" + splited[0] + splited[1..];
                long dirSize = await Task.Run(() => directoryInfo.EnumerateFiles( "*", SearchOption.AllDirectories).Sum(file => file.Length))
                    .ConfigureAwait(false);
                directoryInfoResult = new DirectoryInfoResult(new FileInfoDTO(directoryInfo.Name, creationTime, changingDate, dirSize, fullName, FileType.Directory));
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                directoryInfoResult = new FileInfoResult("Недопустимый путь", directoryNotFoundException);
            }
            catch (IOException ioException)
            {
                directoryInfoResult = new FileInfoResult("Ошибка при чтении файла", ioException);
            }
            catch (SecurityException securityException)
            {
                directoryInfoResult = new FileInfoResult("Отсутствует необходимое разрешение", securityException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                directoryInfoResult = new FileInfoResult("Отсутствует необходимое разрешение", unauthorizedAccessException);
            }
            catch (Exception exception)
            {
                directoryInfoResult = new FileInfoResult(exception.Message, exception);
            }
            
            return directoryInfoResult;
        }
    }
}
