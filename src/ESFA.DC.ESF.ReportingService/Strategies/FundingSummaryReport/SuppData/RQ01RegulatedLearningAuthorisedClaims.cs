using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class RQ01RegulatedLearningAuthorisedClaims : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        protected override string DeliverableCode => "RQ01";

        protected override string ReferenceType => "Authorised Claims";      
    }
}