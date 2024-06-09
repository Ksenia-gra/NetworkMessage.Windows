using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using System.IO;
using System.Security;
using NetworkMessage.DTO;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsNestedFilesInfoCommand : BaseNetworkCommand
    {
        public string Path { get; set; }

        public WindowsNestedFilesInfoCommand(string path)
        {
            string root = "root";
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

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default,
            params object[] objects)
        {
            BaseNetworkCommandResult nestedFilesInfo;
            try
            {
                const string disk = "disk_";
                if (string.IsNullOrWhiteSpace(Path) || Path == "/")
                {
                    nestedFilesInfo = new NestedFilesInfoResult(errorMessage: "File doesn't exist");
                    return Task.FromResult(nestedFilesInfo);
                }

                Path = Path[disk.Length..];
                Path = Path.Insert(Path.IndexOf('/'), ":");
                DirectoryInfo directoryInfo = new DirectoryInfo(Path);
                if (!directoryInfo.Exists)
                {
                    nestedFilesInfo = new NestedFilesInfoResult(errorMessage: "File doesn't exist");
                    return Task.FromResult(nestedFilesInfo);
                }

                IEnumerable<FileInfoDTO> filesInfo = directoryInfo.GetFiles().Select(f =>
                {
                    string[] splited = f.FullName.Replace('\\', '/').Split(":");
                    string fullName = "Disk_" + splited[0] + splited[1];
                    return new FileInfoDTO(f.Name, f.CreationTimeUtc, f.LastWriteTimeUtc, f.Length, fullName, FileType.File);
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
                nestedFilesInfo =
                    new NestedFilesInfoResult("Отсутствует необходимое разрешение", unauthorizedAccessException);
            }
            catch (Exception exception)
            {
                nestedFilesInfo = new NestedFilesInfoResult(exception.Message, exception);
            }

            return Task.FromResult(nestedFilesInfo);
        }
    }
}