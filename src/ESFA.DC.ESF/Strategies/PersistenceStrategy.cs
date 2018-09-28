using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.Strategies
{
    public class PersistenceStrategy // : ITaskStrategy
    {
        private readonly IStorageController _storageController;
        private readonly ILogger _logger;

        public PersistenceStrategy(
            IStorageController storageController,
            ILogger logger)
        {
            _storageController = storageController;
            _logger = logger;
        }

        public bool IsMatch(string taskName)
        {
            return taskName == string.Empty;
        }

        public async Task Execute(
            IJobContextMessage jobContextMessage,
            IList<SupplementaryDataModel> esfRecords, 
            IDictionary<string, ValidationErrorModel> errors,
            CancellationToken cancellationToken)
        {
            bool success = await _storageController.StoreData(jobContextMessage, cancellationToken, esfRecords);
        }
    }
}
