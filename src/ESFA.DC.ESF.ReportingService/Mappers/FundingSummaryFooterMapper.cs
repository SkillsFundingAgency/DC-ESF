using CsvHelper.Configuration;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Mappers
{
    public sealed class FundingSummaryFooterMapper : ClassMap<FundingSummaryFooterModel>, IClassMapper
    {
        public FundingSummaryFooterMapper()
        {
            Map(m => m.ApplicationVersion).Index(0).Name("Application Version");
            Map(m => m.LarsData).Index(1).Name("LARS Data");
            Map(m => m.PostcodeData).Index(2).Name("Postcode Data");
            Map(m => m.OrganisationData).Index(3).Name("Organisation Data");
            Map(m => m.ReportGeneratedAt).Index(4).Name("Report generated at");
        }
    }
}
