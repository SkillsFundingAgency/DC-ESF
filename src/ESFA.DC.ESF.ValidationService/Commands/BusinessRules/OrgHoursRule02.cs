﻿using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class OrgHoursRule02 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The OrgHours is not required for the selected CostType.";

        public string ErrorName => "OrgHours_02";

        public bool IsWarning => true;

        public bool Execute(SupplementaryDataModel model)
        {
            return model.CostType == Constants.CostTypeApportionedCost || model.OrgHours == null;
        }
    }
}
