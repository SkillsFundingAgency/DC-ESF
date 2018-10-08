namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public enum RowType
    {
        Header,
        Data,
        Total,
        Spacer
    }

    public class FundingReportRow
    {
        public RowType RowType { get; set; }

        public string CodeBase { get; set; }

        public string DeliverableCode { get; set; }

        public string Title { get; set; }
    }
}
