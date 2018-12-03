using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Reports.Services
{
    public interface ISupplementaryDataService
    {
        Task<IList<SourceFileModel>> GetImportFiles(
            string ukPrn,
            CancellationToken cancellationToken);

        Task<IDictionary<int, IEnumerable<SupplementaryDataYearlyModel>>> GetSupplementaryData(
            IEnumerable<SourceFileModel> sourceFiles,
            CancellationToken cancellationToken);
    }
}