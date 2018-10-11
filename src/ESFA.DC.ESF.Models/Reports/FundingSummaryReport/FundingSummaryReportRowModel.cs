using System.Collections.Generic;

namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public class FundingSummaryReportRowModel
    {
        public RowType RowType { get; set; }

        public string Title { get; set; }

        public string DeliverableCode { get; set; }

        public List<FundingSummaryReportYearlyValueModel> YearlyValues { get; set; }

        public List<decimal> Totals { get; set; }
    }
}