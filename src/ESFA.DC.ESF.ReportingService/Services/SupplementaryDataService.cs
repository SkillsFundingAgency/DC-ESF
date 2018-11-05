using ESFA.DC.ESF.Interfaces.DataAccessLayer;

namespace ESFA.DC.ESF.ReportingService.Services
{
    public class SupplementaryDataService
    {
        private readonly IEsfRepository _repository;

        public SupplementaryDataService(IEsfRepository repository)
        {
            _repository = repository;
        }
    }
}