using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Controllers
{
    public interface IValidationController
    {
        bool RejectFile { get; }

        IList<ValidationErrorModel> Errors { get; }

        void ValidateData(
            IList<SupplementaryDataModel> allModels,
            SupplementaryDataModel model,
            SourceFileModel sourceFile,
            CancellationToken cancellationToken);
    }
}