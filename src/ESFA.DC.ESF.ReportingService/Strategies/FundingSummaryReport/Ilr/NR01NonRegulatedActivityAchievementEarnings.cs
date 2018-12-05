using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr
{
    public class NR01NonRegulatedActivityAchievementEarnings : BaseILRDataStrategy, IILRDataStrategy
    {
        protected override string DeliverableCode => "NR01";

        protected override List<string> AttributeNames => new List<string>
        {
            "AchievementEarnings"
        };
    }
}