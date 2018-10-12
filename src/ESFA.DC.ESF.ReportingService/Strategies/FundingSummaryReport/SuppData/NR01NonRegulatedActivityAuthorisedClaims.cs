using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class NR01NonRegulatedActivityAuthorisedClaims : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        private string DeliverableCode = "NR01";

        private string ReferenceType = "Authorised Claims";      
    }
}