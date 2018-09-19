using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF
{
    public class EntryPoint
    {
        private readonly ILogger _logger;
        private readonly IFileHelper _fileHelper;
        private readonly ITaskHelper _taskHelper;

        public EntryPoint(
            IFileHelper fileHelper,
            ITaskHelper taskHelper,
            ILogger logger)
        {
            _logger = logger;
            _fileHelper = fileHelper;
            _taskHelper = taskHelper;
        }

        public async Task<bool> Callback(IJobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            _logger.LogInfo("ESF callback invoked");

            var tasks = jobContextMessage.Topics[jobContextMessage.TopicPointer].Tasks;
            if(!tasks.Any())
            {
                return true;
            }

            var esfRecords = await _fileHelper.GetESFRecords(jobContextMessage, cancellationToken);
            if (esfRecords == null || !esfRecords.Any())
            {
                _logger.LogInfo("No ESF records to process");
                return true;
            }
            
            await _taskHelper.ExecuteTasks(tasks, esfRecords, cancellationToken);
                
            return !cancellationToken.IsCancellationRequested;
        }
    }
}
