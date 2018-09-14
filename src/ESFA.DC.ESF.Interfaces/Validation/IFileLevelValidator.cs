using System.Threading.Tasks;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IFileLevelValidator
    {
        string ErrorMessage { get; }

        bool IsValid { get; }

        bool RejectFile { get; }

        Task Execute(string fileName, string contents);
    }
}