using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class RQ01RegulatedLearningAuthorisedClaims : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        private string DeliverableCode = "RQ01";

        private string ReferenceType = "Authorised Claims";      
    }
}