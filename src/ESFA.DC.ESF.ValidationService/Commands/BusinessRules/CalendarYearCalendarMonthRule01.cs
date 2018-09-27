using System;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CalendarYearCalendarMonthRule01 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The UKPRN in the filename does not match the UKPRN in the Hub";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            // var date = new DateTime(model.CalendarYear)

            return Task.CompletedTask;
        }
    }
}
