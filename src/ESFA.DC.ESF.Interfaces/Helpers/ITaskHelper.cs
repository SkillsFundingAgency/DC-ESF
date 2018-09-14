using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ESF.Interfaces.Helpers
{
    public interface ITaskHelper
    {
        Task ExecuteTasks(IReadOnlyList<ITaskItem> tasks, IList<ESFModel> records, CancellationToken cancellationToken);
    }
}