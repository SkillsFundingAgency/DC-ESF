using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CalendarYearCalendarMonthRule03 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The CalendarMonth and CalendarYear is after the contract allocation end date.";

        public string ErrorName => "CalendarYearCalendarMonth_03";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            // todo need FCA

            return Task.CompletedTask;
        }
    }
}
