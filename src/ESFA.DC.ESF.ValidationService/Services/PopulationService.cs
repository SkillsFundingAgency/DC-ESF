using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.ValidationService.Services
{
    public class PopulationService : IPopulationService
    {
        private readonly IReferenceDataRepository _repository;
        private readonly IFcsCodeMappingHelper _mappingHelper;
        private readonly ILogger _logger;

        public PopulationService(
            IReferenceDataRepository repository,
            IFcsCodeMappingHelper mappingHelper,
            ILogger logger)
        {
            _repository = repository;
            _logger = logger;
            _mappingHelper = mappingHelper;
        }

        public void PrePopulateUlnCache(IList<long?> ulns, CancellationToken cancellationToken)
        {
            _repository.GetUlnLookup(ulns, cancellationToken);
        }

        public void PrePopulateContractAllocations(long ukPrn, IList<SupplementaryDataModel> models, CancellationToken cancellationToken)
        {
            foreach (var model in models)
            {
                var fcsDeliverableCode = _mappingHelper.GetFcsDeliverableCode(model, cancellationToken);
                _repository.GetContractAllocation(model.ConRefNumber, fcsDeliverableCode, cancellationToken, ukPrn);
            }
        }
    }
}
