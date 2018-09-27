using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDDeliverableCodeMA : IFieldDefinitionValidator
    {
        public string Level => "Error";

        public string ErrorMessage =>
            "The DeliverableCode is mandatory. Please resubmit the file including the appropriate value.";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = model.DeliverableCode != null;

            return Task.CompletedTask;
        }
    }
}