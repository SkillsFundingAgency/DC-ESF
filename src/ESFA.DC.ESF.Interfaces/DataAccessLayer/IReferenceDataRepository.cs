using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.ULN.Model;
using ESFA.DC.ReferenceData.FCS.Model;

namespace ESFA.DC.ESF.Interfaces.DataAccessLayer
{
    public interface IReferenceDataRepository
    {
        string GetPostcodeVersion(CancellationToken cancellationToken);

        string GetLarsVersion(CancellationToken cancellationToken);

        string GetOrganisationVersion(CancellationToken cancellationToken);

        string GetProviderName(int ukPrn, CancellationToken cancellationToken);

        List<UniqueLearnerNumber> GetUlnLookup(CancellationToken cancellationToken);

        Task<IList<LARS_LearningDelivery>> GetLarsLearningDelivery(List<string> learnAimRefs, CancellationToken cancellationToken);

        Task<IList<ContractDeliverableCodeMapping>> GetContractDeliverableCodeMapping(List<string> deliverableCodes, CancellationToken cancellationToken);
    }
}