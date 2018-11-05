using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Controllers
{
    public interface IValidationController
    {
        bool RejectFile { get; }

        IList<ValidationErrorModel> Errors { get; }

        Task ValidateData(
            IList<SupplementaryDataModel> allModels,
            SupplementaryDataModel model,
            CancellationToken cancellationToken);
    }
}