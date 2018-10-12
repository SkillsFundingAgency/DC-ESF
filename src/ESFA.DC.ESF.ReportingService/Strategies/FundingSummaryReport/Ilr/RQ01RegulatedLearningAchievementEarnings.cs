using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr
{
    public class RQ01RegulatedLearningAchievementEarnings : BaseILRDataStrategy, IILRDataStrategy
    {
        private new readonly string DeliverableCode = "RQ01";

        private new readonly List<string> AttributeNames = new List<string>
        {
            "AchievementEarnings"
        };
    }
}