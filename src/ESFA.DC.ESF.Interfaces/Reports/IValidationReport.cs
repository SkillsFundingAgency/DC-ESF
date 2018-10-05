using System.Collections.Generic;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Reports
{
    public interface IValidationReport
    {
        Task GenerateReport(
            IList<SupplementaryDataModel> data,
            SourceFileModel sourceFile,
            IList<ValidationErrorModel> validationErrors,
            ZipArchive archive,
            CancellationToken cancellationToken);
    }
}