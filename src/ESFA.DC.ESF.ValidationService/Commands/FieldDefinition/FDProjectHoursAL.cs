using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Helpers;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDProjectHoursAL : IFieldDefinitionValidator
    {
        public string Level => "Error";

        public string ErrorMessage => $"The ProjectHours does not conform to the decimal ({IntegerPartLength + PrecisionLength},{PrecisionLength}) field type. Please adjust the value and resubmit the file.";

        public bool IsValid { get; private set; }

        private const int IntegerPartLength = 6;

        private const int PrecisionLength = 2;

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = string.IsNullOrEmpty(model.ProjectHours.ToString().Trim()) 
                      || DecimalHelper.CheckDecimalLengthAndPrecision(model.ProjectHours ?? 0.0M, IntegerPartLength, PrecisionLength);

            return Task.CompletedTask;
        }
    }
}
