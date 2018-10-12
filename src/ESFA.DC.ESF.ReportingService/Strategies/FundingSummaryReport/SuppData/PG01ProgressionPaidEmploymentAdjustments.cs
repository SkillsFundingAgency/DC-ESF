using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class PG01ProgressionPaidEmploymentAdjustments : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        private readonly string DeliverableCode = "PG01";
    }
}