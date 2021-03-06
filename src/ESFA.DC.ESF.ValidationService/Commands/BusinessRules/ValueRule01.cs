﻿using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ValueRule01 : IBusinessRuleValidator
    {
        readonly List<string> _costTypesRequiringValue = new List<string>
        {
            Constants.CostTypeStaffPT,
            Constants.CostTypeStaffFT,
            "Staff Expenses",
            "Other Costs",
            "Apportioned Cost",
            "Grant",
            "Grant Management",
            "Funding Adjustment"
        };

        public string ErrorMessage => "The Value must be returned for the selected CostType.";

        public string ErrorName => "Value_01";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            return !_costTypesRequiringValue.Contains(model.CostType) || model.Value != null;
        }
    }
}
