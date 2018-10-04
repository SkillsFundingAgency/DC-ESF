using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContextManager.Model.Interface;

namespace ESFA.DC.ESF.Interfaces.Helpers
{
    public interface IFileHelper
    {
        Task<IList<SupplementaryDataModel>> GetESFRecords(SourceFileModel sourceFileModel, CancellationToken cancellationToken);

        SourceFileModel GetSourceFileData(IJobContextMessage jobContextMessage);
    }
}