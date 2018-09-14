using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.JobContext;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF
{
    public class EntryPoint
    {
        private readonly ILogger _logger;

        public EntryPoint(
            ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> Callback(JobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            _logger.LogInfo("ESF callback invoked");

            await ExecuteTasks(jobContextMessage, cancellationToken);
                
            return !cancellationToken.IsCancellationRequested;
        }

        private async Task ExecuteTasks(IJobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            foreach (ITaskItem taskItem in jobContextMessage.Topics[jobContextMessage.TopicPointer].Tasks)
            {
                if (taskItem.SupportsParallelExecution)
                {
                    // Parallel.ForEach(
                    //    taskItem.Tasks,
                    //    new ParallelOptions { CancellationToken = cancellationToken },
                    //    async task => { await ... });
                }
                else
                {
                    foreach (string task in taskItem.Tasks)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        // await ...;
                    }
                }
            }
        }
    }
}
