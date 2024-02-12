using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using NetworkMessage.Models;
using System.IO;
using System.Reflection;
using System.Security;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsNestedFilesInfoCommand:BaseNetworkCommand
    {
        public string Path { get; set; }

        public WindowsNestedFilesInfoCommand(string path) 
        {
            if (!string.IsNullOrWhiteSpace(path) && path.IndexOf("root") == 0)
            {
                path = path.Substring(5);
            }

            Path = path;
        }

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult nestedFilesInfo;
            try
            {
                if (string.IsNullOrWhiteSpace(Path) || Path == "/")
                {
                    nestedFilesInfo = new NestedFilesInfoResult(new List<MyFileInfo>());
                    return Task.FromResult(nestedFilesInfo);
                }

                Path = Path[5..];
                Path = Path.Insert(Path.IndexOf('/'), ":");
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                if (!directoryInfo.Exists)
                {
                    nestedFilesInfo = new NestedFilesInfoResult(errorMessage: "File doesn't exist");
                    return Task.FromResult(nestedFilesInfo);
                }

                IEnumerable<MyFileInfo> filesInfo
                    = directoryInfo.GetFiles().Select(f => {
                        string[] splited = f.FullName.Split(":");
                        string fullName = "Disk_" + splited[0] + splited[1..];
                        return new MyFileInfo(f.Name, f.CreationTimeUtc, f.LastWriteTimeUtc, f.Length, fullName);
                    });
                nestedFilesInfo = new NestedFilesInfoResult(filesInfo);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                nestedFilesInfo = new NestedFilesInfoResult("Недопустимый путь", directoryNotFoundException);
            }
            catch (IOException ioException)
            {
                nestedFilesInfo = new NestedFilesInfoResult("Ошибка при чтении файла", ioException);
            }
            catch (SecurityException securityException)
            {
                nestedFilesInfo = new NestedFilesInfoResult("Отсутствует необходимое разрешение", securityException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                nestedFilesInfo = new NestedFilesInfoResult("Отсутствует необходимое разрешение", unauthorizedAccessException);
            }
            catch (Exception exception)
            {
                nestedFilesInfo = new NestedFilesInfoResult(exception.Message, exception);
            }

            return Task.FromResult(nestedFilesInfo);
        }
    }
}
