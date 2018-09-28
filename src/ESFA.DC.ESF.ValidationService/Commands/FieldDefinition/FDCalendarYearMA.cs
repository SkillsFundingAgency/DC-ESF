using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCalendarYearMA : IFieldDefinitionValidator
    {
        public string ErrorName => "FD_CalendarYear_MA";

        public bool IsWarning => false;

        public string ErrorMessage =>
            "The DeliverableCode is mandatory. Please resubmit the file including the appropriate value.";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = model.CalendarYear != null;

            return Task.CompletedTask;
        }
    }
}
