using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class PG06ProgressionJobSearchAdjustments : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        private readonly string DeliverableCode = "PG06";
    }
}