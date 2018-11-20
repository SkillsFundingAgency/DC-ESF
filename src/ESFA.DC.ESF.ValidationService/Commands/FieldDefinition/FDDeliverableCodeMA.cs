using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDDeliverableCodeMA : IFieldDefinitionValidator
    {
        public string ErrorName => "FD_DeliverableCode_MA";

        public bool IsWarning => false;

        public string ErrorMessage =>
            "The DeliverableCode is mandatory. Please resubmit the file including the appropriate value.";

        public bool Execute(SupplementaryDataModel model)
        {
            return !string.IsNullOrEmpty(model.DeliverableCode);
        }
    }
}