using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
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

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            BaseNetworkCommandResult loadedFileResult;
			try
            {
                const string disk = "disk_";
                if (string.IsNullOrWhiteSpace(Path) || Path == "/")
                {
                    loadedFileResult = new DownloadFileResult(errorMessage: "File doesn't exist");
                    return Task.FromResult(loadedFileResult);                    
                }
                
                Path = Path[disk.Length..];
                Path = Path.Insert(Path.IndexOf('/'), ":");
                if (Path.Last() == '/')
                {
                    Path = Path[..^1];
                }
                
                if (!File.Exists(Path))
                {
                    loadedFileResult = new DownloadFileResult(errorMessage: "File doesn't exist");
                    return Task.FromResult(loadedFileResult);
                }

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
