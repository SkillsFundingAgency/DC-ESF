namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public class FundingSummaryReportYearlyValueModel
    {
        public int FundingYear { get; set; }

        public decimal[] Values { get; set; }

        public FundingSummaryReportYearlyValueModel()
        {
            Values = new decimal[12];
        }
    }
}