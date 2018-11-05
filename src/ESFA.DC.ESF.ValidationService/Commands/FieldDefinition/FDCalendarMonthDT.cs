using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCalendarMonthDT : IFieldDefinitionValidator
    {
        public string ErrorName => "FD_CalendarMonth_DT";

        public bool IsWarning => false;

        public string ErrorMessage => "CalendarMonth must be an integer (whole number). Please adjust the value and resubmit the file.";

        public bool Execute(SupplementaryDataModel model)
        {
            return model.CalendarMonth != null;
        }
    }
}
