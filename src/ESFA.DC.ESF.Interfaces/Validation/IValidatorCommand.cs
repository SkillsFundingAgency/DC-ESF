using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IValidatorCommand
    {
        Task Execute(SupplementaryDataModel model);

        Dictionary<string, List<string>> Errors { get; }

        bool RejectFile { get; }

        bool IsValid { get; }

        int Priority { get; }
    }
}