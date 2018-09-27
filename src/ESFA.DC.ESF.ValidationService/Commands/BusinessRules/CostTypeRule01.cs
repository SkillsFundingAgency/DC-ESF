﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CostTypeRule01 : IBusinessRuleValidator
    {
        private readonly IList<string> _validCostTypes = new List<string>
        {
            Constants.CostTypeStaffPT,
            Constants.CostTypeStaffFT,
            "Apportioned Cost",
            "Staff Expenses",
            "Grant",
            "Grant Management",
            "Unit Cost",
            "Unit Cost Deduction",
            "Other Costs",
            "Funding Adjustment"
        };

        public string ErrorMessage => "The CostType is not valid";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = _validCostTypes.Contains(model.CostType);
            return Task.CompletedTask;
        }
    }
}
