using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContextManager.Model.Interface;

namespace ESFA.DC.ESF.Interfaces.Helpers
{
    public interface ITaskHelper
    {
        Task ExecuteTasks(IReadOnlyList<ITaskItem> tasks,
            SourceFileModel sourceFileModel,
            IList<SupplementaryDataModel> records,
            CancellationToken cancellationToken);
    }
}