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
using ESFA.DC.Data.ULN.Model;
using ESFA.DC.Data.ULN.Model.Interfaces;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Models.Validation;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.ReferenceData.FCS.Model;
using ESFA.DC.ReferenceData.FCS.Model.Interface;

namespace ESFA.DC.ESF.DataAccessLayer
{
    public class ReferenceDataRepository : IReferenceDataRepository
    {
        private readonly IPostcodes _postcodes;
        private readonly ILARS _lars;
        private readonly IOrganisations _organisations;
        private readonly IFcsContext _fcsContext;
        private readonly IULN _ulnContext;
        private readonly ILogger _logger;

        private readonly object _ulnLock = new object();
        private readonly object _larsDeliveryLock = new object();
        private readonly object _codeMappingLock = new object();

        public ReferenceDataRepository(
            ILogger logger,
            IPostcodes postcodes,
            ILARS lars,
            IOrganisations organisations,
            IFcsContext fcsContext,
            IULN ulnContext)
        {
            _logger = logger;
            _postcodes = postcodes;
            _lars = lars;
            _organisations = organisations;
            _fcsContext = fcsContext;
            _ulnContext = ulnContext;
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

        public IList<LARS_LearningDelivery> GetLarsLearningDelivery(IList<string> learnAimRefs, CancellationToken cancellationToken)
        {
            List<LARS_LearningDelivery> learningDelivery = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                lock (_larsDeliveryLock)
                {
                    learningDelivery = _lars.LARS_LearningDelivery
                        .Where(x => learnAimRefs.Contains(x.LearnAimRef)).ToList();
                }
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

        public IList<UniqueLearnerNumber> GetUlnLookup(IList<long?> searchUlns, CancellationToken cancellationToken)
        {
            List<UniqueLearnerNumber> ulns = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                lock (_ulnLock)
                {
                    var result = new List<UniqueLearnerNumber>();
                    var ulnShards = SplitList(searchUlns, 5000);
                    foreach (var shard in ulnShards)
                    {
                        result.AddRange(_ulnContext.UniqueLearnerNumbers
                            .Where(u => shard.Contains(u.ULN)).ToList());
                    }

                    ulns.AddRange(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get uln data", ex);
            }

            return ulns;
        }

        public IList<ContractDeliverableCodeMapping> GetContractDeliverableCodeMapping(IList<string> deliverableCodes, CancellationToken cancellationToken)
        {
            List<ContractDeliverableCodeMapping> codeMapping = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                lock (_codeMappingLock)
                {
                    codeMapping = _fcsContext.ContractDeliverableCodeMappings
                            .Where(x => deliverableCodes.Contains(x.ExternalDeliverableCode)).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get FCS ContractDeliverableCodeMapping", ex);
            }

            return codeMapping;
        }

        public ContractAllocationCacheModel GetContractAllocation(string conRefNum, int deliverableCode, CancellationToken cancellationToken, long? ukPrn = null)
        {
            ContractAllocationCacheModel contractAllocationModel = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                contractAllocationModel = _fcsContext.ContractAllocations
                    .Where(ca => ca.Contract.Contractor.Ukprn == ukPrn && ca.ContractDeliverables.Any(cd => cd.DeliverableCode == deliverableCode))
                    .Select(ca => new ContractAllocationCacheModel
                    {
                        DeliverableCode = deliverableCode,
                        ContractAllocationNumber = ca.ContractAllocationNumber,
                        StartDate = ca.StartDate,
                        EndDate = ca.EndDate
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get FCS ContractDeliverableCodeMapping", ex);
            }

            return contractAllocationModel;
        }

        private IEnumerable<List<long?>> SplitList(IEnumerable<long?> ulns, int nSize = 30)
        {
            var ulnList = ulns.ToList();

            for (var i = 0; i < ulnList.Count; i += nSize)
            {
                yield return ulnList.GetRange(i, Math.Min(nSize, ulnList.Count - i));
            }
        }
    }
}
