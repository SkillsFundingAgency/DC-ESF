using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr
{
    public class SU03SustainedEducation3Months : BaseILRDataStrategy, IILRDataStrategy
    {
        protected override string DeliverableCode => "SU03";

        protected override List<string> AttributeNames => new List<string>
        {
            "StartEarnings",
            "AchievementEarnings",
            "AdditionalProgCostEarnings",
            "ProgressionEarnings"
        };
    }
}