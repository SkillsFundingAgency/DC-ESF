using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCostTypeMA : IFieldDefinitionValidator
    {
        public string Level => "Error";

        public string ErrorMessage =>
            "The CostType is mandatory. Please resubmit the file including the appropriate value.";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = model.CostType != null;

            return Task.CompletedTask;
        }
    }
}
