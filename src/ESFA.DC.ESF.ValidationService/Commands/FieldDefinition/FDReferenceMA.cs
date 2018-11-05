using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDReferenceMA : IFieldDefinitionValidator
    {
        public string ErrorName => "FD_Reference_MA";

        public bool IsWarning => false;

        public string ErrorMessage =>
            "The Reference is mandatory. Please resubmit the file including the appropriate value.";

        public bool Execute(SupplementaryDataModel model)
        {
            return model.Reference != null;
        }
    }
}
