using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.DataStore.Tests.Builders
{
    public class SupplementaryDataModelBuilder
    {
        public static SupplementaryDataModel BuildSupplementaryData()
        {
            return new SupplementaryDataModel
            {
                ConRefNumber = "123456789abcdefghij",
                DeliverableCode = "1234567890",
                CalendarYear = 2018,
                CalendarMonth = 9,
                CostType = "foo",
                Reference = "asasdadad asdadadada asdadsasdad",
                ReferenceType = "thingie",
                ProviderSpecifiedReference = "123131312ae qq12123",
                StaffName = "Wibble",
                ULN = 3456789012,
                HourlyRate = 7.5M,
                OrgHours = 123,
                ProjectHours = 99,
                TotalHoursWorked = 42,
                Value = 75432.87M
            };
        }
    }
}
