using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Reports
{
    public interface IValidationReport
    {
        Task GenerateReport(
            SourceFileModel sourceFile,
            SupplementaryDataWrapper wrapper,
            ZipArchive archive,
            CancellationToken cancellationToken);
    }
}