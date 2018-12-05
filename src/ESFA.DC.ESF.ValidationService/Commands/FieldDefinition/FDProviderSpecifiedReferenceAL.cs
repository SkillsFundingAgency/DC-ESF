using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDProviderSpecifiedReferenceAL : IFieldDefinitionValidator
    {
        private const int FieldLength = 200;

        public string ErrorName => "FD_ProviderSpecifiedReference_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The ProviderSpecifiedReference must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public bool Execute(SupplementaryDataLooseModel model)
        {
            return string.IsNullOrEmpty(model.ProviderSpecifiedReference?.Trim()) || model.ProviderSpecifiedReference.Length <= FieldLength;
        }
    }
}
