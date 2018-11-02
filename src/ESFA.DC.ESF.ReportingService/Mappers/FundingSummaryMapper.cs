using CsvHelper.Configuration;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Mappers
{
    public sealed class FundingSummaryMapper : ClassMap<FundingSummaryModel>, IClassMapper
    {
        public FundingSummaryMapper()
        {
            Map(m => m.Title).Index(0).Name("NA");
            Map(m => m.YearlyValues).Index(1).Name("January {Y}", "February {Y}", "March {Y}", "April {Y}", "May {Y}", "June {Y}", "July {Y}", "August {Y}", "September {Y}", "October {Y}", "November {Y}", "December {Y}");
            Map(m => m.Totals).Index(2).Name("{SP}/{SY} Subtotal");
            Map(m => m.GrandTotal).Index(3).Name("Grand Total");
        }
    }
}
