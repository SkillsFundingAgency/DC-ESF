using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDCostTypeMA : IFieldDefinitionValidator
    {
        public string ErrorName => "FD_CostType_MA";

        public bool IsWarning => false;

        public string ErrorMessage =>
            "The CostType is mandatory. Please resubmit the file including the appropriate value.";

        public bool Execute(SupplementaryDataModel model)
        {
            return model.CostType != null;
        }
    }
}
