using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class SU01SustainedPaidEmployment3MonthsAdjustments : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        private readonly string DeliverableCode = "SU01";
    }
}