using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDReferenceAL : IFieldDefinitionValidator
    {
        private const int FieldLength = 100;

        public string ErrorName => "FD_Reference_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The Reference must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = string.IsNullOrEmpty(model.Reference.Trim()) || model.Reference.Length <= FieldLength;

            return Task.CompletedTask;
        }
    }
}
