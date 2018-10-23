using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContextManager.Model.Interface;

namespace ESFA.DC.ESF.Helpers
{
    public class TaskHelper : ITaskHelper
    {
        private readonly IList<ITaskStrategy> _taskHandlers;

        public TaskHelper(IList<ITaskStrategy> taskHandlers)
        {
            _taskHandlers = taskHandlers;
        }

        public async Task ExecuteTasks(IReadOnlyList<ITaskItem> tasks,
            SourceFileModel sourceFileModel,
            SupplementaryDataWrapper supplementaryDataWrapper, 
            CancellationToken cancellationToken)
        {
            foreach (ITaskItem taskItem in tasks)
            {
                if (taskItem.SupportsParallelExecution)
                {
                    Parallel.ForEach(
                       taskItem.Tasks,
                       new ParallelOptions { CancellationToken = cancellationToken },
                       async task => { await HandleTask(supplementaryDataWrapper, task, sourceFileModel, cancellationToken); });
                }
                else
                {
                    foreach (var task in taskItem.Tasks)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        await HandleTask(supplementaryDataWrapper, task, sourceFileModel, cancellationToken);
                    }
                }
            }
        }

        private async Task HandleTask(SupplementaryDataWrapper wrapper,
            string task,
            SourceFileModel sourceFile,
            CancellationToken cancellationToken)
        {
            var orderedHandlers = _taskHandlers.OrderBy(t => t.Order);
            foreach (var handler in orderedHandlers)
            {
                if (!handler.IsMatch(task))
                {
                    continue;
                }

                await handler.Execute(sourceFile, wrapper, cancellationToken);
                break;
            }
        }
    }
}
