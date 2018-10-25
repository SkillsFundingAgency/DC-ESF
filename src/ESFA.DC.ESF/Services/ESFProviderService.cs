using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using CsvHelper;
using ESFA.DC.ESF.Interfaces;
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
            [KeyFilter(PersistenceStorageKeys.Blob)] IKeyValuePersistenceService storage)
        {
            _logger = logger;
            _storage = storage;
            _getESFLock = new SemaphoreSlim(1, 1);
        }

        public async Task<IList<SupplementaryDataModel>> GetESFRecordsFromFile(SourceFileModel sourceFile, CancellationToken cancellationToken)
        {
            List<SupplementaryDataModel> model = null;

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

                    model = csvReader.GetRecords<SupplementaryDataModel>().ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get and deserialise ESF from storage, key: {sourceFile.FileName}", ex);
            }
            finally
            {
                _getESFLock.Release();
            }

            return model;
        }
    }
}
