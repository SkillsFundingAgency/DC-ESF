using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.DataStore;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Service.Config;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.DataStore
{
    public class StorageController : IStorageController
    {
        private readonly IStoreESF _store;
        private readonly IStoreClear _storeClear;
        private readonly IStoreFileDetails _storeFileDetails;
        private readonly PersistDataConfiguration _persistDataConfiguration;
        private readonly ILogger _logger;

        public StorageController(
            PersistDataConfiguration persistDataConfiguration,
            IStoreESF store,
            IStoreClear storeClear,
            IStoreFileDetails storeFileDetails,
            ILogger logger)
        {
            _persistDataConfiguration = persistDataConfiguration;
            _store = store;
            _storeClear = storeClear;
            _storeFileDetails = storeFileDetails;
            _logger = logger;
        }

        public async Task<bool> StoreData(
            IJobContextMessage jobContextMessage,
            CancellationToken cancellationToken,
            IEnumerable<SupplementaryDataModel> models)
        {
            int ukPrn = int.Parse(jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn].ToString());
            var fileName = jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString();
            bool successfullyCommitted = false;

            using (SqlConnection connection =
                new SqlConnection(_persistDataConfiguration.DataStoreConnectionString))
            {
                SqlTransaction transaction = null;
                try
                {

                    await connection.OpenAsync(cancellationToken);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return false;
                    }

                    transaction = connection.BeginTransaction();

                    await _storeClear.ClearAsync(ukPrn, fileName, cancellationToken);

                    int fileId = await _storeFileDetails.StoreAsync(connection, transaction, cancellationToken);

                    await _store.StoreAsync(connection, transaction, fileId, models, cancellationToken);

                    transaction = connection.BeginTransaction();

                    transaction.Commit();
                    successfullyCommitted = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to persist to DEDs", ex);
                }
                finally
                {
                    if (!successfullyCommitted)
                    {
                        try
                        {
                            transaction?.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            _logger.LogError("Failed to rollback DEDs persist transaction", ex2);
                        }
                    }
                }
            }

            return successfullyCommitted;

        }
    }
}
