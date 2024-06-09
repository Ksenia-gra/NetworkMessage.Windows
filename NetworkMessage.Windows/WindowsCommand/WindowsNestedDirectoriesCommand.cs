using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using System.IO;
using System.Security;
using NetworkMessage.DTO;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsNestedDirectoriesCommand : BaseNetworkCommand
    {
        public string Path { get; set; }

        public WindowsNestedDirectoriesCommand(string path)
        {
            const string root = "root";
            if (!string.IsNullOrWhiteSpace(path) && path.IndexOf("root") == 0)
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
            BaseNetworkCommandResult nestedDirectoriesInfo;
            try
            {
                const string disk = "disk_";
                if (string.IsNullOrWhiteSpace(Path) || Path == "/") 
                {
                    IEnumerable<FileInfoDTO> drivesInfo = DriveInfo.GetDrives()
                        .Select(d => "Disk_" + d.Name[..d.Name.IndexOf(':')])
                        .Select(d => new FileInfoDTO(d, null, null, null, d, FileType.Drive));
                    nestedDirectoriesInfo = new NestedDirectoriesInfoResult(drivesInfo);
                    return Task.FromResult(nestedDirectoriesInfo);
                }

                Path = Path[disk.Length..];
                Path = Path.Insert(Path.IndexOf('/'), ":");
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                if (!directoryInfo.Exists)
                {
                    nestedDirectoriesInfo = new NestedDirectoriesInfoResult(errorMessage: "Directory doesn't exist");
                    return Task.FromResult(nestedDirectoriesInfo);
                }
                
                IEnumerable<FileInfoDTO> directoriesInfo
                    = directoryInfo.GetDirectories().Select(d =>
                    {
                        string[] splited = d.FullName.Replace('\\', '/').Split(":");
                        string fullName = "Disk_" + splited[0] + splited[1];
                        return new FileInfoDTO(d.Name, d.CreationTimeUtc, d.LastWriteTimeUtc, null, fullName, FileType.Directory);
                    });
                nestedDirectoriesInfo = new NestedDirectoriesInfoResult(directoriesInfo);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                nestedDirectoriesInfo = new NestedFilesInfoResult("Недопустимый путь", directoryNotFoundException);
            }
            catch (IOException ioException)
            {
                nestedDirectoriesInfo = new NestedFilesInfoResult("Ошибка при чтении файла", ioException);
            }
            catch (SecurityException securityException)
            {
                nestedDirectoriesInfo = new NestedFilesInfoResult("Отсутствует необходимое разрешение", securityException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                nestedDirectoriesInfo = new NestedFilesInfoResult("Отсутствует необходимое разрешение", unauthorizedAccessException);
            }
            catch (Exception exception)
            {
                nestedDirectoriesInfo = new NestedFilesInfoResult(exception.Message, exception);
            }

            return Task.FromResult(nestedDirectoriesInfo);
        }
    }
}
