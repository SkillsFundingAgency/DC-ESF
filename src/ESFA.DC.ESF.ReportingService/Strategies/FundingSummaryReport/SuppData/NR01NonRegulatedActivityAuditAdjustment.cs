using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class NR01NonRegulatedActivityAuditAdjustment : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        private readonly string DeliverableCode = "NR01";

        private readonly string ReferenceType = "Audit Adjustment";
    }
}