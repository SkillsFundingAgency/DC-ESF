using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Database.EF;

namespace ESFA.DC.ESF.Interfaces.DataAccessLayer
{
    public interface IEsfRepository
    {
        Task<IList<SourceFile>> PreviousFiles(string ukPrn, string conRefNumber, CancellationToken cancellationToken);
    }
}