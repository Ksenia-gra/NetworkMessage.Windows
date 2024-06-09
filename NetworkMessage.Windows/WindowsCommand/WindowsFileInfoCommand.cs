using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using System.IO;
using System.Security;
using NetworkMessage.DTO;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsFileInfoCommand : BaseNetworkCommand
    {
        public string Path {  get; set; }

        public WindowsFileInfoCommand(string path)
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
            BaseNetworkCommandResult fileInfoResult;
            try
            {
                const string disk = "disk_";
                if (string.IsNullOrWhiteSpace(Path) || Path == "/")
                {
                    fileInfoResult = new FileInfoResult(errorMessage: "Incorrect path");
                    return Task.FromResult(fileInfoResult);
                }

                Path = Path[disk.Length..];
                Path = Path.Insert(Path.IndexOf('/'), ":");
                if (Path.Last() == '/')
                {
                    Path = Path[..^1];
                }

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
                fileInfoResult = new FileInfoResult(new FileInfoDTO(fileName, creationTime, changingDate, fileLength, fullName, FileType.File));
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
