﻿using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ValueRule03 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The Value must not exceed the HourlyRate x TotalHoursWorked";

        public string ErrorName => "Value_03";

        public bool IsWarning => true;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = !(model.CostType == Constants.CostTypeStaffPT
                        &&
                        model.Value > model.HourlyRate * model.TotalHoursWorked);

            return Task.CompletedTask;
        }
    }
}
