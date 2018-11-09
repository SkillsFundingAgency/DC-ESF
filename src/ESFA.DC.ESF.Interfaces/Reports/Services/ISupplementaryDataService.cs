using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Reports.Services
{
    public interface ISupplementaryDataService
    {
        Task<IList<SourceFileModel>> GetPreviousContractImportFilesForProvider(
            string ukPrn,
            CancellationToken cancellationToken);

        Task<IList<SupplementaryDataModel>> GetPreviousContractDataForProvider(
            int sourceFileId,
            CancellationToken cancellationToken);
    }
}