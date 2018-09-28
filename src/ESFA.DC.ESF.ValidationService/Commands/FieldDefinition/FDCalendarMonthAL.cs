using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCalendarMonthAL : IFieldDefinitionValidator
    {
        public string ErrorMessage => $"The CalendarMonth must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public string ErrorName => "FD_CalendarMonth_AL";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        private const int FieldLength = 2;

        public Task Execute(SupplementaryDataModel model)
        {
            var month = model.CalendarMonth.ToString();

            IsValid = !string.IsNullOrEmpty(month)
                      && !string.IsNullOrWhiteSpace(month)
                      && month.Length <= FieldLength;

            return Task.CompletedTask;
        }
    }
}
