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

        IList<UniqueLearnerNumber> GetUlnLookup(IList<long?> searchUlns, CancellationToken cancellationToken);

        IList<LARS_LearningDelivery> GetLarsLearningDelivery(IList<string> learnAimRefs, CancellationToken cancellationToken);

        IList<ContractDeliverableCodeMapping> GetContractDeliverableCodeMapping(IList<string> deliverableCodes, CancellationToken cancellationToken);
    }
}