using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService.Services
{
    public class PopulationService : IPopulationService
    {
        private readonly IReferenceDataRepository _repository;

        public PopulationService(IReferenceDataRepository repository)
        {
            _repository = repository;
        }

        public void PrePopulateUlnCache(IList<long?> ulns, CancellationToken cancellationToken)
        {
            _repository.GetUlnLookup(ulns, cancellationToken);
        }
    }
}
