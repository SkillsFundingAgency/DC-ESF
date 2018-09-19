using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDReferenceMA : IFieldDefinitionValidator
    {
        public string Level => "Error";

        public string ErrorMessage =>
            "The Reference is mandatory. Please resubmit the file including the appropriate value.";

        public bool IsValid { get; private set; }

        public Task Execute(ESFModel model)
        {
            IsValid = model.Reference != null;

            return Task.CompletedTask;
        }
    }
}
