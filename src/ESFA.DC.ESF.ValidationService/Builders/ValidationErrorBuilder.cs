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
