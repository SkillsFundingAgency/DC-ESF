using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr
{
    public class SU03SustainedEducation3Months : BaseILRDataStrategy, IILRDataStrategy
    {
        private new readonly string DeliverableCode = "SU03";

        private new readonly List<string> AttributeNames = new List<string>
        {
            "StartEarnings",
            "AchievementEarnings",
            "AdditionalProgCostEarnings",
            "ProgressionEarnings"
        };
    }
}