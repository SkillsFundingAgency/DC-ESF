using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContext;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.JobContextManager.Model.Interface;
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

            IList<SupplementaryDataModel> esfRecords = new List<SupplementaryDataModel>();
            try
            {
                esfRecords = await _fileHelper.GetESFRecords(jobContextMessage, cancellationToken);
                if (esfRecords == null || !esfRecords.Any())
                {
                    _logger.LogInfo("No ESF records to process");
                    return true;
                }
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"The file format is incorrect, key: {JobContextMessageKey.Filename}", ex);
                IReadOnlyList<ITaskItem> errorTasks = new List<ITaskItem>{ new TaskItem
                {
                    SupportsParallelExecution = false,
                    Tasks = new List<string> { "ProduceJsonOnly" }
                }};

                await _taskHelper.ExecuteTasks(errorTasks, null, cancellationToken);

                return true;
            }

            await _taskHelper.ExecuteTasks(tasks, esfRecords, cancellationToken);
                
            return !cancellationToken.IsCancellationRequested;
        }
    }
}
