using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class HourlyRateRule02 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The HourlyRate is not required for the selected CostType";

        public bool IsValid { get; private set; }

        public Task Execute(ESFModel model)
        {
            IsValid = !(model.CostType != Constants.CostTypeStaffPT
                        &&
                        model.HourlyRate != null);

            return Task.CompletedTask;
        }
    }
}
