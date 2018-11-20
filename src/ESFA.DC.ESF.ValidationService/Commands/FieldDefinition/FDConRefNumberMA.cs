using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDConRefNumberMA : IFieldDefinitionValidator
    {
        public string ErrorName => "FD_ConRefNumber_MA";

        public bool IsWarning => false;

        public string ErrorMessage => "The ConRefNumber is mandatory. Please resubmit the file including the appropriate value.";

        public bool Execute(SupplementaryDataModel model)
        {
            return !string.IsNullOrEmpty(model.ConRefNumber);
        }
    }
}
