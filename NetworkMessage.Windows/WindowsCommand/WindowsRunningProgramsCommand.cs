using System.Diagnostics;
using NetworkMessage.Commands;
using NetworkMessage.CommandsResults;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using NetworkMessage.DTO;

namespace NetworkMessage.Windows.WindowsCommand;

public class WindowsRunningProgramsCommand : BaseNetworkCommand
{
    public async override Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects)
    {
        Process[] processList = Process.GetProcesses();
        IEnumerable<IGrouping<string, Process>> grouped = processList.GroupBy(x => x.ProcessName).OrderBy(x => x.Key);
        List<ProgramInfoDTO> runningPrograms = new List<ProgramInfoDTO>();
        await Task.Run(() =>
        {
            foreach (IGrouping<string, Process> group in grouped)
            {
                try
                {
                    runningPrograms.Add(new ProgramInfoDTO
                    (
                        group.Key,
                        group.Sum(x => x.PrivateMemorySize64),
                        group.FirstOrDefault()?.MainModule?.FileName
                    ));
                }
                catch
                {
                    // ignored
                }
            }
        }).ConfigureAwait(false);

        return new RunningProgramsResult(runningPrograms.OrderBy(p => p.ProgramName).Select(x => x));
    }
}