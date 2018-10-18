using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.LARS.Model.Interfaces;
using ESFA.DC.Data.Organisatons.Model.Interface;
using ESFA.DC.Data.Postcodes.Model.Interfaces;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;

namespace ESFA.DC.ESF.ReportingService.Services
{
    public class ReferenceDataRepository : IReferenceDataRepository
    {
        private readonly IPostcodes _postcodes;
        private readonly ILARS _lars;
        private readonly IOrganisations _organisations;
        private readonly IFcsContext _fcsContext;

        private readonly ILogger _logger;

        private const string FundingStreamPeriodCode = "ESF1420";

        public ReferenceDataRepository(
            ILogger logger,
            IPostcodes postcodes,
            ILARS lars,
            IOrganisations organisations,
            IFcsContext fcsContext)
        {
            _logger = logger;
            _postcodes = postcodes;
            _lars = lars;
            _organisations = organisations;
            _fcsContext = fcsContext;
        }

        public string GetPostcodeVersion(CancellationToken cancellationToken)
        {
            var version = string.Empty;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                version = _postcodes.VersionInfos.OrderByDescending(v => v.VersionNumber).Select(v => v.VersionNumber)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get postcode version", ex);
            }

            return version;
        }

        public string GetLarsVersion(CancellationToken cancellationToken)
        {
            var version = string.Empty;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                version = _lars.LARS_Version.OrderByDescending(v => v.MainDataSchemaName).Select(lv => lv.MainDataSchemaName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get lars version", ex);
            }

            return version;
        }

        public async Task<IList<LARS_LearningDelivery>> GetLarsLearningDelivery(List<string> learnAimRefs, CancellationToken cancellationToken)
        {
            List<LARS_LearningDelivery> learningDelivery = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }
                learningDelivery = await _lars.LARS_LearningDelivery.Where(ld => learnAimRefs.Contains(ld.LearnAimRef)).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get lars learning delivery", ex);
            }

            return learningDelivery;
        }

        public string GetOrganisationVersion(CancellationToken cancellationToken)
        {
            var version = string.Empty;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                version = _organisations.Org_Version.OrderByDescending(v => v.MainDataSchemaName).Select(lv => lv.MainDataSchemaName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get org version", ex);
            }

            return version;
        }

        public string GetProviderName(int ukPrn, CancellationToken cancellationToken)
        {
            var providerName = string.Empty;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                providerName = _organisations.Org_Details.FirstOrDefault(o => o.UKPRN == ukPrn)?.Name;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get org provider name", ex);
            }

            return providerName;
        }

        public async Task<IList<ContractDeliverableCodeMapping>> GetContractDeliverableCodeMapping(List<string> deliverableCodes, CancellationToken cancellationToken)
        {
            List<ContractDeliverableCodeMapping> codeMapping = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }
                codeMapping = await _fcsContext.ContractDeliverableCodeMappings
                    .Where(m => deliverableCodes.Contains(m.ExternalDeliverableCode) 
                                          && m.FundingStreamPeriodCode == FundingStreamPeriodCode).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get FCS ContractDeliverableCodeMapping", ex);
            }

            return codeMapping;
        }
    }
}
