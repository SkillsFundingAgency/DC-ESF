using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Services
{
    public interface IESFProviderService
    {
        Task<IList<SupplementaryDataLooseModel>> GetESFRecordsFromFile(SourceFileModel sourceFile, CancellationToken cancellationToken);
    }
}