using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Services
{
    public interface IFileValidationService
    {
        Task<bool> GetFile(
            SourceFileModel sourceFileModel,
            IList<SupplementaryDataModel> esfRecords,
            IList<ValidationErrorModel> errors,
            CancellationToken cancellationToken);

        Task<bool> RunFileValidators(SourceFileModel sourceFileModel,
            IList<SupplementaryDataModel> models,
            IList<ValidationErrorModel> errors);
    }
}