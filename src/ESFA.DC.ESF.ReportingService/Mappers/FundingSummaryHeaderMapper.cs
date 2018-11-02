using CsvHelper.Configuration;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Mappers
{
    public sealed class FundingSummaryHeaderMapper : ClassMap<FundingSummaryHeaderModel>, IClassMapper
    {
        public FundingSummaryHeaderMapper()
        {
            Map(m => m.ProviderName).Index(0).Name("Provider Name");
            Map(m => m.Ukprn).Index(1).Name("UKPRN");
            Map(m => m.ContractReferenceNumber).Index(2).Name("Contract Reference Number");
            Map(m => m.SupplementaryDataFile).Index(3).Name("Supplementary Data File");
            Map(m => m.LastSupplementaryDataFileUpdate).Index(4).Name("Last Supplementary Data File Update");
            Map(m => m.SecurityClassification).Index(5).Name("Security Classification");
        }
    }
}
