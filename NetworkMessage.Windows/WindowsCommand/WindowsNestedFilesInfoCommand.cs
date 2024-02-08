using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using NetworkMessage.Models;
using System.IO;
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
                path = path.Substring(4);
            }

            Path = path;
        }

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult nestedFilesInfo;
            if (string.IsNullOrWhiteSpace(Path) || Path == "/")
            {
                nestedFilesInfo = new NestedFilesInfoResult(new List<MyFileInfo>());
                return Task.FromResult(nestedFilesInfo);
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(Path);
            if (!directoryInfo.Exists)
            {
                nestedFilesInfo = new NestedFilesInfoResult(errorMessage:"File doesn't exist");
                return Task.FromResult(nestedFilesInfo);
            }

            try
            {
                IEnumerable<MyFileInfo> filesInfo
                    = directoryInfo.GetFiles().Select(f => new MyFileInfo(f.Name, f.CreationTimeUtc, f.LastWriteTimeUtc, f.Length, f.FullName));
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
