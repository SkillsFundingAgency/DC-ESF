using CsvHelper.Configuration;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Mappers
{
    public sealed class ESFMapper : ClassMap<ESFModel>, IClassMapper
    {
        public ESFMapper()
        {
            int i = 0;
            Map(m => m.ConRefNumber).Index(i++);
            Map(m => m.DeliverableCode).Index(i++);
            Map(m => m.CalendarYear).Index(i++);
            Map(m => m.CalendarMonth).Index(i++);
            Map(m => m.CostType).Index(i++);
            Map(m => m.StaffName).Index(i++);
            Map(m => m.ReferenceType).Index(i++);
            Map(m => m.Reference).Index(i++);
            Map(m => m.ULN).Index(i++);
            Map(m => m.ProviderSpecifiedReference).Index(i++);
            Map(m => m.Value).Index(i++);
            Map(m => m.HourlyRate).Index(i++);
            Map(m => m.TotalHoursWorked).Index(i++);
            Map(m => m.ProjectHours).Index(i++);
            Map(m => m.OrgHours).Index(i);
        }
    }
}
