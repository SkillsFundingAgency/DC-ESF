using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.Interfaces.Repositories
{
    public interface IFM70Repository
    {
        Task<FileDetail> GetFileDetails(int ukPrn, CancellationToken cancellationToken);

        Task<IList<ESF_LearningDelivery>> GetLearningDeliveries(int ukPrn, CancellationToken cancellationToken);

        Task<IList<ESF_LearningDeliveryDeliverable>> GetLearningDeliveryDeliverables(int ukPrn, CancellationToken cancellationToken);

        Task<IList<ESF_LearningDeliveryDeliverable_Period>> GetLearningDeliveryDeliverablePeriods(int ukPrn, CancellationToken cancellationToken);

        Task<IList<ESF_LearningDeliveryDeliverable_PeriodisedValues>> GetPeriodisedValues(int ukPrn, CancellationToken cancellationToken);

        Task<IList<ESF_DPOutcome>> GetOutcomes(int ukPrn, CancellationToken cancellationToken);
    }
}