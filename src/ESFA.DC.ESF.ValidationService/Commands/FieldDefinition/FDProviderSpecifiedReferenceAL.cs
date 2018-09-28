using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDProviderSpecifiedReferenceAL : IFieldDefinitionValidator
    {
        public string ErrorName => "FD_ProviderSpecifiedReference_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The ProviderSpecifiedReference must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public bool IsValid { get; private set; }

        private const int FieldLength = 200;

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = string.IsNullOrEmpty(model.ProviderSpecifiedReference.Trim()) || model.ProviderSpecifiedReference.Length <= FieldLength;

            return Task.CompletedTask;
        }
    }
}
