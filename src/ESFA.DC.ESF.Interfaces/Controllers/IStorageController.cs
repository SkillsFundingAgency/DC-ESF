using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ESF.Interfaces.Controllers
{
    public interface IStorageController
    {
        Task<bool> StoreData(
            IJobContextMessage jobContextMessage,
            CancellationToken cancellationToken,
            IEnumerable<SupplementaryDataModel> models);
    }
}