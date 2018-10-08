using System.Collections.Generic;

namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public class FundingSummaryReportRowModel
    {
        public string Title { get; set; }

        public List<FundingSummaryReportYearlyValueModel> Values { get; set; }

        public List<decimal> Totals { get; set; }
    }
}