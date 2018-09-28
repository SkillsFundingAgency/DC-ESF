using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CalendarYearCalendarMonthRule02 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The CalendarMonth and CalendarYear is prior to the contract allocation start date.";

        public string ErrorName => "CalendarYearCalendarMonth_02";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            //todo need FCA

            return Task.CompletedTask;
        }
    }
}
