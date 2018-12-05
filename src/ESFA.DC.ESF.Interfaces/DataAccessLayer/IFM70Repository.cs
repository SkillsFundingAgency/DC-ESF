using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.Interfaces.DataAccessLayer
{
    public interface IFM70Repository
    {
        Task<ILRFileDetailsModel> GetFileDetails(int ukPrn, CancellationToken cancellationToken);

        Task<IList<ESF_LearningDelivery>> GetLearningDeliveries(int ukPrn, CancellationToken cancellationToken);

        Task<IList<ESF_LearningDeliveryDeliverable>> GetLearningDeliveryDeliverables(int ukPrn, CancellationToken cancellationToken);

        Task<IList<ESF_LearningDeliveryDeliverable_Period>> GetLearningDeliveryDeliverablePeriods(int ukPrn, CancellationToken cancellationToken);

        Task<IList<FM70PeriodisedValuesModel>> GetPeriodisedValues(int ukPrn, CancellationToken cancellationToken);

        Task<IList<ESF_DPOutcome>> GetOutcomes(int ukPrn, CancellationToken cancellationToken);
    }
}