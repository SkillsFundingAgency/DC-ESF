using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IFileLevelValidator
    {
        string ErrorMessage { get; }

        bool IsValid { get; }

        bool RejectFile { get; }

        Task Execute(string fileName, SupplementaryDataModel model);
    }
}