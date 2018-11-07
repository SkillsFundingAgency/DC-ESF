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
        private readonly IReferenceDataCache _dataCache;
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
            IULN ulnContext,
            IReferenceDataCache dataCache)
        {
            _logger = logger;
            _postcodes = postcodes;
            _lars = lars;
            _organisations = organisations;
            _fcsContext = fcsContext;
            _ulnContext = ulnContext;
            _dataCache = dataCache;
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

                var uncached = new List<string>();
                lock (_larsDeliveryLock)
                {
                    foreach (var learnAimRef in learnAimRefs)
                    {
                        if (_dataCache.LarsLearningDeliveries.All(x => x.LearnAimRef != learnAimRef))
                        {
                            uncached.Add(learnAimRef);
                        }
                    }

                    if (uncached.Any())
                    {
                        _dataCache.LarsLearningDeliveries.AddRange(_lars.LARS_LearningDelivery
                            .Where(x => uncached.Contains(x.LearnAimRef)).ToList());
                    }
                }

                learningDelivery = _dataCache.LarsLearningDeliveries.Where(l => learnAimRefs.Contains(l.LearnAimRef)).ToList();
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

                if (!_dataCache.ProviderNameByUkprn.ContainsKey(ukPrn))
                {
                    _dataCache.ProviderNameByUkprn[ukPrn] = _organisations.Org_Details.FirstOrDefault(o => o.UKPRN == ukPrn)?.Name;
                }

                providerName = _dataCache.ProviderNameByUkprn[ukPrn];
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

                var uniqueUlns = searchUlns.Distinct();
                var unknownUlns = new List<long?>();
                lock (_ulnLock)
                {
                    foreach (var uln in uniqueUlns)
                    {
                        if (_dataCache.Ulns.All(u => u.ULN != uln))
                        {
                            unknownUlns.Add(uln);
                        }
                    }

                    if (unknownUlns.Any())
                    {
                        var result = new List<UniqueLearnerNumber>();
                        var ulnShards = SplitList(unknownUlns, 5000);
                        foreach (var shard in ulnShards)
                        {
                            result.AddRange(_ulnContext.UniqueLearnerNumbers
                                .Where(u => shard.Contains(u.ULN)).ToList());
                        }

                        _dataCache.Ulns.AddRange(result);
                    }
                }

                ulns = _dataCache.Ulns.Where(x => searchUlns.Contains(x.ULN)).ToList();
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

                var uncached = new List<string>();
                lock (_codeMappingLock)
                {
                    foreach (var deliverableCode in deliverableCodes)
                    {
                        if (_dataCache.CodeMappings.All(x => x.ExternalDeliverableCode != deliverableCode))
                        {
                            uncached.Add(deliverableCode);
                        }
                    }

                    if (uncached.Any())
                    {
                        _dataCache.CodeMappings.AddRange(_fcsContext.ContractDeliverableCodeMappings
                            .Where(x => uncached.Contains(x.ExternalDeliverableCode)).ToList());
                    }
                }

                codeMapping = _dataCache.CodeMappings
                    .Where(m => deliverableCodes.Contains(m.ExternalDeliverableCode)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get FCS ContractDeliverableCodeMapping", ex);
            }

            return codeMapping;
        }

        public ContractAllocation GetContractAllocation(string conRefNum, int deliverableCode, CancellationToken cancellationToken, long? ukPrn = null)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                if (ukPrn != null && !_dataCache.ContractAllocations
                    .Any(ca => ca.DeliverableCode == deliverableCode &&
                               ca.ContractAllocation.ContractAllocationNumber == conRefNum))
                {
                    var contractAllocation = _fcsContext.Contractors.Where(c => c.Ukprn == ukPrn)
                        .SelectMany(c => c.Contracts)
                        .SelectMany(c => c.ContractAllocations).Where(ca => ca.ContractAllocationNumber == conRefNum)
                        .Join(_fcsContext.ContractDeliverables.Where(cd => cd.DeliverableCode == deliverableCode), c => c.Id, d => d.ContractAllocationId, (c, d) => c)
                        .FirstOrDefault();

                    if (contractAllocation != null)
                    {
                        _dataCache.ContractAllocations.Add(new ContractAllocationCacheModel
                        {
                            DeliverableCode = deliverableCode,
                            ContractAllocation = contractAllocation
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get FCS ContractDeliverableCodeMapping", ex);
            }

            return _dataCache.ContractAllocations.FirstOrDefault(ca => ca.DeliverableCode == deliverableCode &&
                                                              ca.ContractAllocation.ContractAllocationNumber == conRefNum)?.ContractAllocation;
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
