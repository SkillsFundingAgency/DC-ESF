using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCostTypeAL : IFieldDefinitionValidator
    {
        private const int FieldLength = 20;

        public string ErrorName => "FD_CostType_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The CostType must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public bool Execute(SupplementaryDataModel model)
        {
            return !string.IsNullOrEmpty(model.CostType?.Trim()) && model.CostType.Length <= FieldLength;
        }
    }
}
