using System.Collections.Generic;

namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public enum RowType
    {
        Spacer,
        MainTitle,
        Title,
        Data,
        Total,
        FinalTotal,
        Cumulative,
        FinalCumulative
    }

    public class FundingReportRow
    {
        public RowType RowType { get; set; }

        public string CodeBase { get; set; }

        public string DeliverableCode { get; set; }

        public string ReferenceType { get; set; }

        public List<string> AttributeNames { get; set; }

        public string Title { get; set; }
    }
}
