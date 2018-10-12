using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr
{
    public class PG02ProgressionUnpaidEmployment : BaseILRDataStrategy, IILRDataStrategy
    {
        private new readonly string DeliverableCode = "PG02";

        private new readonly List<string> AttributeNames = new List<string>
        {
            "StartEarnings",
            "AchievementEarnings",
            "AdditionalProgCostEarnings",
            "ProgressionEarnings"
        };
    }
}