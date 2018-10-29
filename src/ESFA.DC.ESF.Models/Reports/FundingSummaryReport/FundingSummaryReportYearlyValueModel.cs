namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public class FundingSummaryReportYearlyValueModel
    {
        public FundingSummaryReportYearlyValueModel()
        {
            Values = new decimal[12];
        }

        public int FundingYear { get; set; }

        public decimal[] Values { get; set; }
    }
}