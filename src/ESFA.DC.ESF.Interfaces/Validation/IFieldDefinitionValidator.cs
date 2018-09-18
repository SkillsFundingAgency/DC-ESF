using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IFieldDefinitionValidator
    {
        string Level { get; }

        string ErrorMessage { get; }

        bool IsValid { get; }

        Task Execute(ESFModel model);
    }
}