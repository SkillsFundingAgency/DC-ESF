using CsvHelper.Configuration;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ReportingService.Mappers
{
    public sealed class ValidationErrorMapper : ClassMap<ValidationErrorModel>, IClassMapper
    {
        public ValidationErrorMapper()
        {
            Map(m => m.IsWarning ? "W" : "E").Index(1).Name("Error/Warning");
            Map(m => m.RuleName).Index(2).Name("RuleName");
            Map(m => m.ErrorMessage).Index(3).Name("ErrorMessage");
            Map(m => m.ConRefNumber).Index(4).Name("ConRefNumber");
            Map(m => m.DeliverableCode).Index(5).Name("DeliverableCode");
            Map(m => m.CalendarYear).Index(6).Name("CalendarYear");
            Map(m => m.CalendarMonth).Index(7).Name("CalendarMonth");
            Map(m => m.CostType).Index(8).Name("CostType");
            Map(m => m.StaffName).Index(9).Name("StaffName");
            Map(m => m.ReferenceType).Index(10).Name("ReferenceType");
            Map(m => m.Reference).Index(11).Name("Reference");
            Map(m => m.ULN).Index(12).Name("ULN");
            Map(m => m.ProviderSpecifiedReference).Index(13).Name("ProviderSpecifiedReference");
            Map(m => m.Value).Index(14).Name("Value");
            Map(m => m.HourlyRate).Index(15).Name("HourlyRate");
            Map(m => m.TotalHoursWorked).Index(16).Name("TotalHoursWorked");
            Map(m => m.ProjectHours).Index(17).Name("ProjectHours");
            Map(m => m.OrgHours).Index(18).Name("OrgHours");
            Map(m => m.RuleName).Index(19).Name("RuleName");
            Map().Index(20).Name("OFFICIAL – SENSITIVE");
        }
    }
}
