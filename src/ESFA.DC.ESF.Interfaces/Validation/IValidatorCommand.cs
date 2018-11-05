using System.Collections.Generic;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IValidatorCommand
    {
        IList<ValidationErrorModel> Errors { get; }

        bool RejectFile { get; }

        int Priority { get; }

        bool Execute(SupplementaryDataModel model);
    }
}