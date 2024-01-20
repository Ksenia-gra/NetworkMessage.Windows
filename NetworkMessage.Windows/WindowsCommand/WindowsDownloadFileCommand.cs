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
            Path = path;
        }

        public async override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult loadedFileResult;
            if (!(File.Exists(Path)))
            {
                loadedFileResult = new DownloadFileResult("File doesn't exist");
                return loadedFileResult;

            }

            try
            {
                byte[] file = await File.ReadAllBytesAsync(Path);
                loadedFileResult = new DownloadFileResult(file);
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
            return loadedFileResult;
        }
    }
}
