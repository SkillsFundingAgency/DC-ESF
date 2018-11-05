using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ULNRule04 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The ULN is not required for the selected ReferenceType.";

        public string ErrorName => "ULN_04";

        public bool IsWarning => true;

        public bool Execute(SupplementaryDataModel model)
        {
            return model.ReferenceType == "LearnRefNumber" || model.ULN == null;
        }
    }
}
