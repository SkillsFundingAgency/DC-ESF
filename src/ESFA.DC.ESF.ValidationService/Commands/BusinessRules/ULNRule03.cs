﻿using System;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Helpers;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ULNRule03 : IBusinessRuleValidator
    {
        public string ErrorMessage => "This ULN should not be used for months that are more than two months older than the current month.";

        public string ErrorName => "ULN_03";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid =
                (model.ULN ?? 0) == 9999999999 ||
                MonthYearHelper.GetCalendarDateTime(model.CalendarYear, model.CalendarMonth) > DateTime.Now.AddMonths(-2);

            return Task.CompletedTask;
        }
    }
}
