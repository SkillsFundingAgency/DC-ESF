using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ESF.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IESFProviderService _providerService;

        public FileHelper(IESFProviderService providerService)
        {
            _providerService = providerService;
        }

        public async Task<IList<ESFModel>> GetESFRecords(IJobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            return await _providerService.GetESFRecordsFromFile(jobContextMessage, cancellationToken);
        }
    }
}
