using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsDownloadFileCommand : BaseNetworkCommand
    {
        public string Path { get; set; }

        public WindowsDownloadFileCommand(string path) 
        { 
            if(!string.IsNullOrWhiteSpace(path) && path.IndexOf("root") == 0)
            {
                path = path.Substring(4);
            }
            Path = path;
        }

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult loadedFileResult;
            if (!(File.Exists(Path)))
            {
                loadedFileResult = new DownloadFileResult(errorMessage: "File doesn't exist");
                return Task.FromResult(loadedFileResult);

            }

            try
            {
                loadedFileResult = new DownloadFileResult(Path);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                loadedFileResult = new DownloadFileResult("Недопустимый путь", directoryNotFoundException);
            }
            catch (IOException ioException)
            {
                loadedFileResult = new DownloadFileResult("Ошибка при чтении файла", ioException);
            }
            catch (SecurityException securityException)
            {
                loadedFileResult = new DownloadFileResult("Отсутствует необходимое разрешение", securityException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                loadedFileResult = new DownloadFileResult("Эта операция не поддерживается на текущей платформе", unauthorizedAccessException);
            }
            catch (Exception ex)
            {
                loadedFileResult = new DownloadFileResult(ex.Message, ex);
            }
            return Task.FromResult(loadedFileResult);
        }
    }
}
