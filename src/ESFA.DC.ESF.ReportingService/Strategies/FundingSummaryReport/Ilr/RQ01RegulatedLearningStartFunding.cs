using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr
{
    public class RQ01RegulatedLearningStartFunding : BaseILRDataStrategy, IILRDataStrategy
    {
        protected override string DeliverableCode => "RQ01";

        protected override List<string> AttributeNames => new List<string>
        {
            "StartEarnings"
        };
    }
}