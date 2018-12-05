using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Controllers
{
    public interface IValidationController
    {
        bool RejectFile { get; }

        void ValidateData(
            SupplementaryDataWrapper wrapper,
            SourceFileModel sourceFile,
            CancellationToken cancellationToken);
    }
}