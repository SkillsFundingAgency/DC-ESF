using System.Collections.Generic;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ReportingService.Tests.Builders
{
    public class SupplementaryDataModelBuilder
    {
        public static List<SupplementaryDataModel> GetModels()
        {
            return new List<SupplementaryDataModel>
            {
                new SupplementaryDataModel
                {
                    ConRefNumber = "12345678901234567890",
                    ULN = 12345678,
                    DeliverableCode = "ST01",
                    CostType = "test",
                    Reference = "test",
                    ReferenceType = "test",
                    CalendarYear = 2018,
                    CalendarMonth = 10,
                    ProviderSpecifiedReference = "test",
                    StaffName = "test",
                    HourlyRate = 5.00M,
                    ProjectHours = 10,
                    OrgHours = 100,
                    TotalHoursWorked = 7,
                    Value = 35.00M
                }
            };
        }
    }
}