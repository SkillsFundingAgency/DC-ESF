using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class SU11SustainedPaidEmployment6MonthsAdjustments : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        private readonly string DeliverableCode = "SU11";
    }
}