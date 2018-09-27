using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ESF.Helpers
{
    public class TaskHelper : ITaskHelper
    {
        private readonly IList<ITaskStrategy> _taskHandlers;

        public TaskHelper(IList<ITaskStrategy> taskHandlers)
        {
            _taskHandlers = taskHandlers;
        }

        public async Task ExecuteTasks(IReadOnlyList<ITaskItem> tasks, IList<SupplementaryDataModel> records, CancellationToken cancellationToken)
        {
            foreach (ITaskItem taskItem in tasks)
            {
                if (taskItem.SupportsParallelExecution)
                {
                    Parallel.ForEach(
                       taskItem.Tasks,
                       new ParallelOptions { CancellationToken = cancellationToken },
                       async task => { await HandleTask(records, task); });
                }
                else
                {
                    foreach (string task in taskItem.Tasks)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        await HandleTask(records, task);
                    }
                }
            }
        }

        private async Task HandleTask(IList<SupplementaryDataModel> records, string task)
        {
            foreach (var handler in _taskHandlers)
            {
                if (!handler.IsMatch(task))
                {
                    continue;
                }

                await handler.Execute(records);
                break;
            }
        }
    }
}
