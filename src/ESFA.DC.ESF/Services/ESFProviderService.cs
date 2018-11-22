using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Mappers;
using ESFA.DC.ESF.Models;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.Services
{
    public class ESFProviderService : IESFProviderService
    {
        private readonly ILogger _logger;

        private readonly IKeyValuePersistenceService _storage;

        private readonly SemaphoreSlim _getESFLock;

        public ESFProviderService(
            ILogger logger,
            IStreamableKeyValuePersistenceService storage)
        {
            _logger = logger;
            _storage = storage;
            _getESFLock = new SemaphoreSlim(1, 1);
        }

        public async Task<IList<SupplementaryDataLooseModel>> GetESFRecordsFromFile(SourceFileModel sourceFile, CancellationToken cancellationToken)
        {
            List<SupplementaryDataLooseModel> model = null;

            await _getESFLock.WaitAsync(cancellationToken);

            _logger.LogInfo("Try and get csv from Azure blob.");
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                string esf =
                    await _storage.GetAsync(sourceFile.FileName, cancellationToken);

                using (TextReader sr = new StringReader(esf))
                {
                    var csvReader = new CsvReader(sr);
                    csvReader.Configuration.RegisterClassMap(new ESFMapper());
                    csvReader.Configuration.TrimOptions = TrimOptions.Trim;
                    model = csvReader.GetRecords<SupplementaryDataLooseModel>().ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get and deserialise ESF from storage, key: {sourceFile.FileName}", ex);
                if (ex is ValidationException)
                {
                    throw;
                }
            }
            finally
            {
                _getESFLock.Release();
            }

            return model;
        }
    }
}
