using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDULNDT : IFieldDefinitionValidator
    {
        public string Level => "Error";

        public string ErrorMessage => $"ULN must be an integer between {Min} and {Max}. Please adjust the value and resubmit the file.";

        public bool IsValid { get; private set; }

        private const long Min = 1000000000;

        private const long Max = 9999999999;

        public Task Execute(SupplementaryDataModel model)
        {
            var uln = model.ULN ?? 0.0;
            IsValid = uln >= Min && uln <= Max;

            return Task.CompletedTask;
        }
    }
}
