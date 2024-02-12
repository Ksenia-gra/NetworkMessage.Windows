using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using NetworkMessage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsDirectoryInfoCommand : BaseNetworkCommand
    {
        public string Path { get; set; }

        public WindowsDirectoryInfoCommand(string path) 
        {
            if (!string.IsNullOrWhiteSpace(path) && path.IndexOf("root") == 0)
            {
                path = path.Substring(5);
            }
            Path = path;
        }

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult directoryInfoResult;       
            try
            {
                Path = Path[5..];
                Path = Path.Insert(Path.IndexOf('/'), ":");
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                if (!directoryInfo.Exists)
                {
                    directoryInfoResult = new DirectoryInfoResult(errorMessage: "Directory doesn't exist");
                    return Task.FromResult(directoryInfoResult);
                }

                DateTime creationTime = directoryInfo.CreationTimeUtc;
                DateTime changingDate = directoryInfo.LastWriteTimeUtc;
                string[] splited = directoryInfo.FullName.Split(":");
                string fullName = "Disk_" + splited[0] + splited[1..];
                directoryInfoResult = new DirectoryInfoResult(new MyDirectoryInfo(directoryInfo.Name, creationTime, changingDate, fullName));
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
            return Task.FromResult(directoryInfoResult);
        }
    }
}
