using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCalendarMonthAL : IFieldDefinitionValidator
    {
        private const int FieldLength = 2;

        public string ErrorMessage => $"The CalendarMonth must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public string ErrorName => "FD_CalendarMonth_AL";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataLooseModel model)
        {
            var month = model.CalendarMonth;

            return !string.IsNullOrEmpty(month)
                      && !string.IsNullOrWhiteSpace(month)
                      && month.Length <= FieldLength;
        }
    }
}
