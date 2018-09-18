using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CalendarMonthRule01 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The CalendarMonth is not valid.";

        public bool IsValid { get; private set; }

        public Task Execute(ESFModel model)
        {
            IsValid = model.CalendarMonth != null && model.CalendarMonth >= 1 && model.CalendarMonth <= 12;

            return Task.CompletedTask;
        }
    }
}
