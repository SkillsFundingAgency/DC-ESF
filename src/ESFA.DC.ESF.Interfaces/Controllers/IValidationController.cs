using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Controllers
{
    public interface IValidationController
    {
        Task ValidateData(SupplementaryDataModel model);

        bool RejectFile { get; }

        Dictionary<string, List<string>> Errors { get; }
    }
}