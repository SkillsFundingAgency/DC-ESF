using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDReferenceTypeAL : IFieldDefinitionValidator
    {
        private const int FieldLength = 20;

        public string ErrorName => "FD_ReferenceType_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The ReferenceType must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public bool Execute(SupplementaryDataLooseModel model)
        {
            return string.IsNullOrEmpty(model.ReferenceType?.Trim()) || model.ReferenceType.Length <= FieldLength;
        }
    }
}
