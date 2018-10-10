namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public class FundingSummaryReportYearlyValueModel
    {
        public int FundingYear { get; set; }

        public decimal[] Values { get; private set; }

        public FundingSummaryReportYearlyValueModel()
        {
            Values = new decimal[12];
        }
    }
}