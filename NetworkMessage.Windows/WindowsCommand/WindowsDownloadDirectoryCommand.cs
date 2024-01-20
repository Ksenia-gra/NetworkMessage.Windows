using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Windows.WindowsCommand
{
    public class WindowsDownloadDirectoryCommand : BaseNetworkCommand
    {
        public string Path {  get; set; }

        public WindowsDownloadDirectoryCommand(string path) 
        {
            Path = path;
        }

        public override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
        {
            throw new NotImplementedException();
        }
    }
}
