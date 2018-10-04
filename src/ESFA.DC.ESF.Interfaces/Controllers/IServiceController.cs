using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.JobContextManager.Model.Interface;

namespace ESFA.DC.ESF.Interfaces.Controllers
{
    public interface IServiceController
    {
        Task RunTasks(IJobContextMessage jobContextMessage,
            IReadOnlyList<ITaskItem> tasks,
            CancellationToken cancellationToken);
    }
}