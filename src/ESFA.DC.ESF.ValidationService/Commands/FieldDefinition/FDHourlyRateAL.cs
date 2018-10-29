using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Helpers;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDHourlyRateAL : IFieldDefinitionValidator
    {
        private const int IntegerPartLength = 6;

        private const int PrecisionLength = 2;

        public string ErrorName => "FD_HourlyRate_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The HourlyRate does not conform to the decimal ({IntegerPartLength + PrecisionLength},{PrecisionLength}) field type. Please adjust the value and resubmit the file.";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = string.IsNullOrEmpty(model.HourlyRate.ToString().Trim())
                      || DecimalHelper.CheckDecimalLengthAndPrecision(model.HourlyRate ?? 0.0M, IntegerPartLength, PrecisionLength);

            return Task.CompletedTask;
        }
    }
}
