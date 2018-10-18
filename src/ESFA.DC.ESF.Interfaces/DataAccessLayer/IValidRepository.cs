using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR1819.DataStore.EF.Valid;

namespace ESFA.DC.ESF.Interfaces.DataAccessLayer
{
    public interface IValidRepository
    {
        Task<List<Learner>> GetLearners(int ukPrn, CancellationToken cancellationToken);

        Task<List<LearningDelivery>> GetLearningDeliveries(int ukPrn, CancellationToken cancellationToken);

        Task<List<LearningDeliveryFAM>> GetLearningDeliveryFAMs(int ukPrn, CancellationToken cancellationToken);

        Task<List<ProviderSpecLearnerMonitoring>> GetProviderSpecLearnerMonitorings(int ukPrn, CancellationToken cancellationToken);

        Task<List<ProviderSpecDeliveryMonitoring>> GetProviderSpecDeliveryMonitorings(int ukPrn, CancellationToken cancellationToken);

        Task<List<DPOutcome>> GetDPOutcomes(int ukPrn, CancellationToken cancellationToken);
    }
}