using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDDeliverableCodeAL : IFieldDefinitionValidator
    {
        private const int FieldLength = 10;

        public string ErrorName => "FD_DeliverableCode_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The DeliverableCode must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public bool Execute(SupplementaryDataModel model)
        {
            return !string.IsNullOrEmpty(model.DeliverableCode?.Trim()) && model.DeliverableCode.Length <= FieldLength;
        }
    }
}
