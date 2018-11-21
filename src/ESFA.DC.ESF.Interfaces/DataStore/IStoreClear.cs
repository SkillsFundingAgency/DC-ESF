using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.ESF.Interfaces.DataStore
{
    public interface IStoreClear
    {
        Task ClearAsync(int ukPrn, string conRefNum, CancellationToken cancellationToken);
    }
}