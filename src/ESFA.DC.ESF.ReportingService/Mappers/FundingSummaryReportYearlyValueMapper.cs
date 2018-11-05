using CsvHelper.Configuration;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Mappers
{
    // Todo: Remove
    public sealed class FundingSummaryReportYearlyValueMapper : ClassMap<FundingSummaryReportYearlyValueModel>, IClassMapper
    {
        public FundingSummaryReportYearlyValueMapper()
        {
            Map(x => x.Values).Name("January {Y}", "February {Y}", "March {Y}", "April {Y}", "May {Y}", "June {Y}", "July {Y}", "August {Y}", "September {Y}", "October {Y}", "November {Y}", "December {Y}");
        }
    }
}
