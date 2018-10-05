using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Jobs.Model;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Helpers
{
    public class StorageHelper : IStorageHelper
    {
        private readonly IKeyValuePersistenceService _storage;
        private readonly IJsonSerializationService _jsonSerializationService;

        public StorageHelper(
            IJsonSerializationService jsonSerializationService,
            [KeyFilter(PersistenceStorageKeys.Blob)] IKeyValuePersistenceService storage)
        {
            _storage = storage;
            _jsonSerializationService = jsonSerializationService;
        }

        public async Task SaveJson(string fileName, FileValidationResult result, CancellationToken cancellationToken)
        {
            await _storage.SaveAsync($"{fileName}.json", _jsonSerializationService.Serialize(result), cancellationToken);
        }
    }
}
