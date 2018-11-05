using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Services
{
    public interface IFileValidationService
    {
        Task<SupplementaryDataWrapper> GetFile(
            SourceFileModel sourceFileModel,
            CancellationToken cancellationToken);

        SupplementaryDataWrapper RunFileValidators(
            SourceFileModel sourceFileModel,
            SupplementaryDataWrapper wrapper);
    }
}