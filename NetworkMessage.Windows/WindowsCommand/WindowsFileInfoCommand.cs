using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using NetworkMessage.Models;
using System.IO;
using System.Security;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsFileInfoCommand : BaseNetworkCommand
    {
        public string Path {  get; set; }

        public WindowsFileInfoCommand(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && path.IndexOf("root") == 0)
            {
                path = path.Substring(5);
            }

            Path = path;
        }

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult fileInfoResult;
            try
            {
                Path = Path[5..];
                Path = Path.Insert(Path.IndexOf('/'), ":");
                FileInfo fileInfo = new FileInfo(Path);
                if (!fileInfo.Exists)
                {
                    fileInfoResult = new FileInfoResult(errorMessage: "File doesn't exist");
                    return Task.FromResult(fileInfoResult);
                }

                string fileName = fileInfo.Name;
                long fileLength = fileInfo.Length;
                DateTime creationTime = fileInfo.CreationTimeUtc;
                DateTime changingDate = fileInfo.LastWriteTimeUtc;
                string[] splited = fileInfo.FullName.Split(":");
                string fullName = "Disk_" + splited[0] + splited[1..];
                fileInfoResult = new FileInfoResult(new MyFileInfo(fileName, creationTime, changingDate, fileLength, fullName));
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                fileInfoResult = new FileInfoResult("Недопустимый путь", directoryNotFoundException);
            }
            catch (IOException ioException)
            {
                fileInfoResult = new FileInfoResult("Ошибка при чтении файла", ioException);
            }
            catch (SecurityException securityException)
            {
                fileInfoResult = new FileInfoResult("Отсутствует необходимое разрешение", securityException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                fileInfoResult = new FileInfoResult("Отсутствует необходимое разрешение", unauthorizedAccessException);
            }
            catch (Exception exception)
            {
                fileInfoResult = new FileInfoResult(exception.Message, exception);
            }


            return Task.FromResult(fileInfoResult);
        }
    }
}
