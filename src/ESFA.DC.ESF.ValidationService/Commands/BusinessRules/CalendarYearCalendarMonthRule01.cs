﻿using System;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Helpers;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CalendarYearCalendarMonthRule01 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The CalendarMonth you have submitted data for cannot be in the future.";

        public string ErrorName => "CalendarYearCalendarMonth_01";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            return MonthYearHelper.GetCalendarDateTime(model.CalendarYear, model.CalendarMonth) < DateTime.Today;
        }
    }
}
