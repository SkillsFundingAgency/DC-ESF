using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.Strategies
{
    public class PersistenceStrategy : ITaskStrategy
    {
        private readonly IStorageController _storageController;
        private readonly ILogger _logger;

        public int Order => 3;

        public PersistenceStrategy(
            IStorageController storageController,
            ILogger logger)
        {
            _storageController = storageController;
            _logger = logger;
        }

        public bool IsMatch(string taskName)
        {
            return taskName == Constants.StorageTask;
        }

        public async Task Execute(
            SourceFileModel sourceFile,
            IList<SupplementaryDataModel> esfRecords,
            IList<ValidationErrorModel> errors,
            CancellationToken cancellationToken)
        {
            var success = await _storageController.StoreData(sourceFile, esfRecords, cancellationToken);

            if (!success)
            {
                _logger.LogError("Failed to save data to the data store.");
            }
        }
    }
}
