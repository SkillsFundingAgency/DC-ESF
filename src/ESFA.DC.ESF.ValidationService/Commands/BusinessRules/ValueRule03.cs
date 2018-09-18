using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ValueRule03 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The Value must not exceed the HourlyRate x TotalHoursWorked";

        public bool IsValid { get; private set; }

        public Task Execute(ESFModel model)
        {
            IsValid = !(model.CostType == Constants.CostTypeStaffPT
                        &&
                        model.Value > model.HourlyRate * model.TotalHoursWorked);

            return Task.CompletedTask;
        }
    }
}
