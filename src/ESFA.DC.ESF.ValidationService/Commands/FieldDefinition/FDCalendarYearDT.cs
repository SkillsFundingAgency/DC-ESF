using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCalendarYearDT : IFieldDefinitionValidator
    {
        public string Level => "Error";

        public string ErrorMessage => "CalendarYear must be an integer (whole number). Please adjust the value and resubmit the file.";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = model.CalendarYear != null;

            return Task.CompletedTask;
        }
    }
}
