using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCalendarYearDT : IFieldDefinitionValidator
    {
        public string ErrorName => "FD_CalendarYear_DT";

        public bool IsWarning => false;

        public string ErrorMessage => "CalendarYear must be an integer (whole number). Please adjust the value and resubmit the file.";

        public bool Execute(SupplementaryDataLooseModel model)
        {
            return !string.IsNullOrEmpty(model.CalendarYear) && int.TryParse(model.CalendarYear, out var year);
        }
    }
}
