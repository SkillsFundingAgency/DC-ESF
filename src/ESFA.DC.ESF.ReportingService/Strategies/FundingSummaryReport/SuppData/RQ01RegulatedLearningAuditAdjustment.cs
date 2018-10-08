using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class RQ01RegulatedLearningAuditAdjustment : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        private string DeliverableCode = "RQ01";

        private string ReferenceType = "Audit Adjustment";
    }
}