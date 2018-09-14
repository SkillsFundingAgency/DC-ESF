using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Validation
{
    public interface IBusinessRuleValidator
    {
        string ErrorMessage { get; }

        bool IsValid { get; }

        Task Execute(ESFModel model);
    }
}