using System.Collections.Generic;
using System.Threading;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IPopulationService
    {
        void PrePopulateUlnCache(IList<long?> ulns, CancellationToken cancellationToken);
    }
}