using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IPopulationService
    {
        void PrePopulateUlnCache(IList<long?> ulns, CancellationToken cancellationToken);

        void PrePopulateContractAllocations(long ukPrn, IList<SupplementaryDataModel> models, CancellationToken cancellationToken);
    }
}