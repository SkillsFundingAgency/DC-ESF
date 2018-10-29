using System.Collections.Generic;

namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public class FundingSummaryReportRowModel
    {
        public FundingSummaryReportRowModel()
        {
            YearlyValues = new List<FundingSummaryReportYearlyValueModel>();
            Totals = new List<decimal>();
        }

        public RowType RowType { get; set; }

        public string Title { get; set; }

        public string DeliverableCode { get; set; }

        public List<FundingSummaryReportYearlyValueModel> YearlyValues { get; set; }

        public List<decimal> Totals { get; set; }
    }
}