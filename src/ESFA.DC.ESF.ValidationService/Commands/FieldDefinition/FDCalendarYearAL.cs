using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCalendarYearAL : IFieldDefinitionValidator
    {
        private const int FieldLength = 4;

        public string ErrorName => "FD_CalendarYear_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The CalendarYear must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public bool Execute(SupplementaryDataModel model)
        {
            var year = model.CalendarYear.ToString();

            return !string.IsNullOrEmpty(year)
                      && !string.IsNullOrWhiteSpace(year)
                      && year.Length <= FieldLength;
        }
    }
}
