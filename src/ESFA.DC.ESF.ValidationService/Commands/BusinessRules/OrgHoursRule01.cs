using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class OrgHoursRule01 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The OrgHours must be returned for the selected CostType.";

        public string ErrorName => "OrgHours_01";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            return model.CostType != Constants.CostTypeApportionedCost || model.OrgHours != null;
        }
    }
}
