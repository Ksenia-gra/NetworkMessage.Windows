using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using NetworkMessage.Models;
using System.IO;
using System.Security;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsNestedDirectoriesCommand : BaseNetworkCommand
    {
        public string Path { get; set; }

        public WindowsNestedDirectoriesCommand(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && path.IndexOf("root") == 0)
            {
                path = path.Substring(5); 
            }

            Path = path;
        }

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult nestedDirectoriesInfo;
            try
            {
                if (string.IsNullOrWhiteSpace(Path) || Path == "/")
                {
                    IEnumerable<MyDirectoryInfo> drivesInfo = DriveInfo.GetDrives()
                        .Select(d => "Disk_" + d.Name[..d.Name.IndexOf(':')])
                        .Select(d => new MyDirectoryInfo(d));
                    nestedDirectoriesInfo = new NestedDirectoriesInfoResult(drivesInfo);
                    return Task.FromResult(nestedDirectoriesInfo);
                }

                Path = Path[5..];
                Path = Path.Insert(Path.IndexOf('/'), ":");
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                if (!directoryInfo.Exists)
                {

                    nestedDirectoriesInfo = new NestedDirectoriesInfoResult(errorMessage: "Directory doesn't exist");
                    return Task.FromResult(nestedDirectoriesInfo);
                }


                IEnumerable<MyDirectoryInfo> directoriesInfo
                    = directoryInfo.GetDirectories().Select(d =>
                    {
                        string[] splited = d.FullName.Split(":");
                        string fullName = "Disk_" + splited[0] + splited[1..];
                        return new MyDirectoryInfo(d.Name, d.CreationTimeUtc, d.LastWriteTimeUtc, fullName);
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
