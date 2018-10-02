using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Controllers
{
    public interface IValidationController
    {
        Task ValidateData(IList<SupplementaryDataModel> allModels, SupplementaryDataModel model);

        bool RejectFile { get; }

        IList<ValidationErrorModel> Errors { get; }
    }
}