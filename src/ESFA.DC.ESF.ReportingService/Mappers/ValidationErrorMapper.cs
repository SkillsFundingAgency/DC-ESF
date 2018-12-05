using CsvHelper.Configuration;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ReportingService.Mappers
{
    public sealed class ValidationErrorMapper : ClassMap<ValidationErrorModel>, IClassMapper
    {
        public ValidationErrorMapper()
        {
            int i = 0;
            Map(m => m.IsWarning).ConvertUsing(c => c.IsWarning ? "W" : "E").Index(i++).Name("Error/Warning");
            Map(m => m.RuleName).Index(i++).Name("RuleName");
            Map(m => m.ErrorMessage).Index(i++).Name("ErrorMessage");
            Map(m => m.ConRefNumber).Index(i++).Name("ConRefNumber");
            Map(m => m.DeliverableCode).Index(i++).Name("DeliverableCode");
            Map(m => m.CalendarYear).Index(i++).Name("CalendarYear");
            Map(m => m.CalendarMonth).Index(i++).Name("CalendarMonth");
            Map(m => m.CostType).Index(i++).Name("CostType");
            Map(m => m.StaffName).Index(i++).Name("StaffName");
            Map(m => m.ReferenceType).Index(i++).Name("ReferenceType");
            Map(m => m.Reference).Index(i++).Name("Reference");
            Map(m => m.ULN).Index(i++).Name("ULN");
            Map(m => m.ProviderSpecifiedReference).Index(i++).Name("ProviderSpecifiedReference");
            Map(m => m.Value).Index(i++).Name("Value");
            Map(m => m.HourlyRate).Index(i++).Name("HourlyRate");
            Map(m => m.TotalHoursWorked).Index(i++).Name("TotalHoursWorked");
            Map(m => m.ProjectHours).Index(i++).Name("ProjectHours");
            Map(m => m.OrgHours).Index(i++).Name("OrgHours");
            Map(m => m.RuleName).Index(i++).Name("RuleName");
            Map(m => m.OfficialSensitive).Index(i).Name("OFFICIAL – SENSITIVE");
        }
    }
}
