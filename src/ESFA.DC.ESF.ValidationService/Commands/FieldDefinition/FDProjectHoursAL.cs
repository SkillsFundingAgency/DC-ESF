using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Helpers;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDProjectHoursAL : IFieldDefinitionValidator
    {
        private const int IntegerPartLength = 6;

        private const int PrecisionLength = 2;

        public string ErrorName => "FD_ProjectHours_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The ProjectHours does not conform to the decimal ({IntegerPartLength + PrecisionLength},{PrecisionLength}) field type. Please adjust the value and resubmit the file.";

        public bool Execute(SupplementaryDataLooseModel model)
        {
            return string.IsNullOrEmpty(model.ProjectHours?.Trim())
                      || (decimal.TryParse(model.ProjectHours, out var projectHours) &&
                       DecimalHelper.CheckDecimalLengthAndPrecision(projectHours, IntegerPartLength, PrecisionLength));
        }
    }
}
