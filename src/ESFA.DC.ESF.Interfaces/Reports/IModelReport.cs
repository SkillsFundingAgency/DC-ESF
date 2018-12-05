using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Reports
{
    public interface IModelReport
    {
        Task GenerateReport(
            SupplementaryDataWrapper supplementaryDataWrapper,
            SourceFileModel sourceFile,
            ZipArchive archive,
            CancellationToken cancellationToken);
    }
}