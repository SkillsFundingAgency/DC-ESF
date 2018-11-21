using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Builders
{
    public class ValidationErrorBuilder
    {
        public static ValidationErrorModel BuildValidationErrorModel(SupplementaryDataModel model,  IBaseValidator validator)
        {
            return new ValidationErrorModel
            {
                RuleName = validator.ErrorName,
                ErrorMessage = validator.ErrorMessage,
                IsWarning = validator.IsWarning,
                ConRefNumber = model.ConRefNumber,
                DeliverableCode = model.DeliverableCode,
                CalendarYear = model.CalendarYear.ToString(),
                CalendarMonth = model.CalendarMonth.ToString(),
                CostType = model.CostType,
                StaffName = model.StaffName,
                ProviderSpecifiedReference = model.ProviderSpecifiedReference,
                ULN = model.ULN.ToString(),
                ReferenceType = model.ReferenceType,
                Reference = model.Reference,
                ProjectHours = model.ProjectHours.ToString(),
                OrgHours = model.OrgHours.ToString(),
                TotalHoursWorked = model.TotalHoursWorked.ToString(),
                HourlyRate = model.HourlyRate.ToString(),
                Value = model.Value.ToString()
            };
        }

        public static ValidationErrorModel BuildValidationErrorModel(SupplementaryDataLooseModel model, IBaseValidator validator)
        {
            return new ValidationErrorModel
            {
                RuleName = validator.ErrorName,
                ErrorMessage = validator.ErrorMessage,
                IsWarning = validator.IsWarning,
                ConRefNumber = model.ConRefNumber,
                DeliverableCode = model.DeliverableCode,
                CalendarYear = model.CalendarYear,
                CalendarMonth = model.CalendarMonth,
                CostType = model.CostType,
                StaffName = model.StaffName,
                ProviderSpecifiedReference = model.ProviderSpecifiedReference,
                ULN = model.ULN,
                ReferenceType = model.ReferenceType,
                Reference = model.Reference,
                ProjectHours = model.ProjectHours,
                OrgHours = model.OrgHours,
                TotalHoursWorked = model.TotalHoursWorked,
                HourlyRate = model.HourlyRate,
                Value = model.Value
            };
        }
    }
}
