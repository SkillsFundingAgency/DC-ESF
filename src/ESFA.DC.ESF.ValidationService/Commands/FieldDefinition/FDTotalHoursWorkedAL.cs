using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Helpers;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDTotalHoursWorkedAL : IFieldDefinitionValidator
    {
        private const int IntegerPartLength = 6;

        private const int PrecisionLength = 2;

        public string ErrorName => "FD_TotalHoursWorked_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The TotalHoursWorked does not conform to the decimal ({IntegerPartLength + PrecisionLength},{PrecisionLength}) field type. Please adjust the value and resubmit the file.";

        public bool Execute(SupplementaryDataLooseModel model)
        {
            return string.IsNullOrEmpty(model.TotalHoursWorked?.Trim())
                      || (decimal.TryParse(model.TotalHoursWorked, out var totalHoursWorked) &&
                       DecimalHelper.CheckDecimalLengthAndPrecision(totalHoursWorked, IntegerPartLength, PrecisionLength));
        }
    }
}
