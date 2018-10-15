using System.Linq;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ESF.Interfaces.Reports.Services;

namespace ESFA.DC.ESF.ReportingService.Services
{
    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IPostcodes _postcodes;
        private readonly ILARS _lars;
        private readonly IOrganisations _organisations;

        public ReferenceDataService(
            IPostcodes postcodes,
            ILARS lars,
            IOrganisations organisations)
        {
            _postcodes = postcodes;
            _lars = lars;
            _organisations = organisations;
        }

        public string GetPostcodeVersion()
        {
            return _postcodes.VersionInfos.OrderByDescending(v => v.VersionNumber).Select(v => v.VersionNumber)
                .FirstOrDefault();
        }

        public string GetLarsVersion()
        {
            return _lars.LARS_Version.OrderByDescending(v => v.MainDataSchemaName).Select(lv => lv.MainDataSchemaName).FirstOrDefault();
        }

        public string GetOrganisationVersion()
        {
            return _organisations.Org_Version.OrderByDescending(v => v.MainDataSchemaName).Select(lv => lv.MainDataSchemaName).FirstOrDefault();
        }

        public string GetProviderName(int ukPrn)
        {
            return _organisations.Org_Details.FirstOrDefault(o => o.UKPRN == ukPrn)?.Name;
        }
    }
}
