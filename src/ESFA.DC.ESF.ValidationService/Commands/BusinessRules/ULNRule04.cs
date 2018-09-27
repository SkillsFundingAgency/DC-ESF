using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ULNRule04 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The ULN is not required for the selected ReferenceType.";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = !(model.ReferenceType != "LearnRefNumber"
                        &&
                        model.ULN != null);

            return Task.CompletedTask;
        }
    }
}
