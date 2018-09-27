using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ESF.Interfaces.Helpers
{
    public interface IFileHelper
    {
        Task<IList<SupplementaryDataModel>> GetESFRecords(IJobContextMessage jobContextMessage, CancellationToken cancellationToken);
    }
}