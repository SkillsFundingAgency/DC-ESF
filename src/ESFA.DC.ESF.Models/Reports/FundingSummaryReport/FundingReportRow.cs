using System.Collections.Generic;

namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public enum RowType
    {
        Header,
        Title,
        Data,
        Total,
        Spacer,
        Cumulative,
        Footer
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
