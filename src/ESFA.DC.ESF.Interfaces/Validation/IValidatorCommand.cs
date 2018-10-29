using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IValidatorCommand
    {
        IList<ValidationErrorModel> Errors { get; }

        bool RejectFile { get; }

        bool IsValid { get; }

        int Priority { get; }

        Task Execute(SupplementaryDataModel model);
    }
}