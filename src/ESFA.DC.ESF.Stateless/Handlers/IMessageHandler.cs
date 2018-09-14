using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.JobContext;

namespace ESFA.DC.ESF.Service.Stateless.Handlers
{
    public interface IMessageHandler
    {
        Task<bool> Handle(JobContextMessage jobContextMessage, CancellationToken cancellationToken);
    }
}
