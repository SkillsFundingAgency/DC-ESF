﻿using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class HourlyRateRule01 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The HourlyRate must be returned for the selected CostType";

        public string ErrorName => "HourlyRate_01";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            return !(model.CostType == Constants.CostTypeStaffPT
                        &&
                        model.HourlyRate == null);
        }
    }
}
