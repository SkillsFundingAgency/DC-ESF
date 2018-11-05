using System.Collections.Generic;

namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public sealed class FundingSummaryModel
    {
        public FundingSummaryModel(int excelHeaderStyle = 4, int excelRecordStyle = 4)
        {
            ExcelHeaderStyle = excelHeaderStyle;
            ExcelRecordStyle = excelRecordStyle;

            YearlyValues = new List<FundingSummaryReportYearlyValueModel>();
            Totals = new List<decimal>();
        }

        public FundingSummaryModel(string title, HeaderType headerType = HeaderType.None, int excelHeaderStyle = 4)
        {
            ExcelHeaderStyle = excelHeaderStyle;
            ExcelRecordStyle = 4;
            Title = title;
            HeaderType = headerType;

            YearlyValues = new List<FundingSummaryReportYearlyValueModel>();
            Totals = new List<decimal>();
        }

        public string Title { get; set; }

        public string DeliverableCode { get; set; }

        public List<FundingSummaryReportYearlyValueModel> YearlyValues { get; set; }

        public List<decimal> Totals { get; set; }

        public decimal GrandTotal { get; set; }

        public int ExcelHeaderStyle { get; set; }

        public int ExcelRecordStyle { get; set; }

        public HeaderType HeaderType { get; }
    }
}