namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public class FundingSummaryReportYearlyValueModel
    {
        public int FundingYear { get; set; }

        public decimal[] Values { get; set; }
    }
}