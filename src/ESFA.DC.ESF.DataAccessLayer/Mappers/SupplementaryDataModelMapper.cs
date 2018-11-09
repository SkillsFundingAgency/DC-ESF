using ESFA.DC.ESF.Database.EF;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.DataAccessLayer.Mappers
{
    public class SupplementaryDataModelMapper : ISupplementaryDataModelMapper
    {
        public SupplementaryDataModel GetModelFromEntity(SupplementaryData entity)
        {
            return new SupplementaryDataModel
            {
                DeliverableCode = entity.DeliverableCode,
                ConRefNumber = entity.ConRefNumber,
                ULN = entity.ULN,
                CostType = entity.CostType,
                CalendarYear = entity.CalendarYear,
                CalendarMonth = entity.CalendarMonth,
                ReferenceType = entity.ReferenceType,
                Reference = entity.Reference,
                ProviderSpecifiedReference = entity.ProviderSpecifiedReference,
                StaffName = entity.StaffName,
                OrgHours = entity.OrgHours,
                ProjectHours = entity.ProjectHours,
                HourlyRate = entity.HourlyRate,
                TotalHoursWorked = entity.TotalHoursWorked,
                Value = entity.Value
            };
        }
    }
}