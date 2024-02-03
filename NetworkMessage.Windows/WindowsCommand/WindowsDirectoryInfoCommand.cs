using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
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
                path = path.Substring(4);
            }
            Path = path;
        }

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Path);
            BaseNetworkCommandResult directoryInfoResult;
            if (!directoryInfo.Exists)
            {
                directoryInfoResult = new DirectoryInfoResult(errorMessage: "Directory doesn't exist");
                return Task.FromResult(directoryInfoResult);
            }

            try
            {
                string directoryName = directoryInfo.Name;
                DateTime creationTime = directoryInfo.CreationTimeUtc;
                DateTime changingDate = directoryInfo.LastWriteTimeUtc;
                string fullName = directoryInfo.FullName;
                directoryInfoResult = new DirectoryInfoResult(new MyDirectoryInfo(directoryName, creationTime, changingDate, fullName));
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
