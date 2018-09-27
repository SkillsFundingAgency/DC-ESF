using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ESF.Interfaces.Services
{
    public interface IESFProviderService
    {
        Task<IList<SupplementaryDataModel>> GetESFRecordsFromFile(IJobContextMessage jobContextMessage, CancellationToken cancellationToken);
    }
}