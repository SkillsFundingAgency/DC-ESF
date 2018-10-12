using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr
{
    public class SU21SustainedPaidEmployment12Months : BaseILRDataStrategy, IILRDataStrategy
    {
        private new readonly string DeliverableCode = "SU21";

        private new readonly List<string> AttributeNames = new List<string>
        {
            "StartEarnings",
            "AchievementEarnings",
            "AdditionalProgCostEarnings",
            "ProgressionEarnings"
        };
    }
}