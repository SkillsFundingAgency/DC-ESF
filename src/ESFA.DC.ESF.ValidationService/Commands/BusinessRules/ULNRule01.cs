using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ULNRule01 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The ULN must be returned.";

        public bool IsValid { get; private set; }

        public Task Execute(ESFModel model)
        {
            IsValid = !(model.ReferenceType == "LearnRefNumber" && model.ULN == null);

            return Task.CompletedTask;
        }
    }
}
