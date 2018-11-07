using System.Threading;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IFcsCodeMappingHelper
    {
        int GetFcsDeliverableCode(SupplementaryDataModel model, CancellationToken cancellationToken);
    }
}