﻿using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CalendarYearRule01 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The CalendarYear is not valid";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = model.CalendarYear != null && model.CalendarYear >= 2016 && model.CalendarYear <= 2019;
            return Task.CompletedTask;
        }
    }
}
