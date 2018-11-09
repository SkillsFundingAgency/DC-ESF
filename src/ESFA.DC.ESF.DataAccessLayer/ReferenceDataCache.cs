using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.ULN.Model;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Models.Validation;
using ESFA.DC.ReferenceData.FCS.Model;

namespace ESFA.DC.ESF.DataAccessLayer
{
    public class ReferenceDataCache : IReferenceDataCache
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public ReferenceDataCache(
            IReferenceDataRepository referenceDataRepository)
        {
            Ulns = new List<UniqueLearnerNumber>();
            CodeMappings = new List<ContractDeliverableCodeMapping>();
            ProviderNameByUkprn = new Dictionary<int, string>();
            LarsLearningDeliveries = new List<LARS_LearningDelivery>();
            ContractAllocations = new List<ContractAllocationCacheModel>();

            _referenceDataRepository = referenceDataRepository;
        }

        public List<UniqueLearnerNumber> Ulns { get; }

        public List<ContractDeliverableCodeMapping> CodeMappings { get; }

        public List<ContractAllocationCacheModel> ContractAllocations { get; }

        public IDictionary<int, string> ProviderNameByUkprn { get; }

        public List<LARS_LearningDelivery> LarsLearningDeliveries { get; }

        public string GetProviderName(int ukPrn, CancellationToken cancellationToken)
        {
            if (!ProviderNameByUkprn.ContainsKey(ukPrn))
            {
                ProviderNameByUkprn[ukPrn] = _referenceDataRepository.GetProviderName(ukPrn, cancellationToken);
            }

            return ProviderNameByUkprn[ukPrn];
        }

        public IList<ContractDeliverableCodeMapping> GetContractDeliverableCodeMapping(
            IList<string> deliverableCodes,
            CancellationToken cancellationToken)
        {
            var uncached = deliverableCodes.Where(deliverableCode => CodeMappings.All(x => x.ExternalDeliverableCode != deliverableCode)).ToList();

            if (uncached.Any())
            {
                CodeMappings.AddRange(_referenceDataRepository.GetContractDeliverableCodeMapping(uncached, cancellationToken));
            }

            return CodeMappings;
        }

        public IList<UniqueLearnerNumber> GetUlnLookup(IList<long?> searchUlns, CancellationToken cancellationToken)
        {
            var uniqueUlns = searchUlns.Distinct();
            var unknownUlns = uniqueUlns.Where(uln => Ulns.All(u => u.ULN != uln)).ToList();

            if (unknownUlns.Any())
            {
                Ulns.AddRange(_referenceDataRepository.GetUlnLookup(unknownUlns, cancellationToken));
            }

            return Ulns.Where(x => searchUlns.Contains(x.ULN)).ToList();
        }

        public ContractAllocationCacheModel GetContractAllocation(
            string conRefNum,
            int deliverableCode,
            CancellationToken cancellationToken,
            long? ukPrn = null)
        {
            if (ukPrn != null && !ContractAllocations
                    .Any(ca => ca.DeliverableCode == deliverableCode &&
                               ca.ContractAllocationNumber == conRefNum))
            {
                ContractAllocations.Add(_referenceDataRepository.GetContractAllocation(conRefNum, deliverableCode, cancellationToken, ukPrn));
            }

            return ContractAllocations.FirstOrDefault(ca => ca.DeliverableCode == deliverableCode &&
                                                            ca.ContractAllocationNumber == conRefNum);
        }

        public IList<LARS_LearningDelivery> GetLarsLearningDelivery(
            IList<string> learnAimRefs,
            CancellationToken cancellationToken)
        {
            var uncached = learnAimRefs.Where(learnAimRef => LarsLearningDeliveries.All(x => x.LearnAimRef != learnAimRef)).ToList();

            if (uncached.Any())
            {
                LarsLearningDeliveries.AddRange(_referenceDataRepository.GetLarsLearningDelivery(uncached, cancellationToken));
            }

            return LarsLearningDeliveries.Where(l => learnAimRefs.Contains(l.LearnAimRef)).ToList();
        }
    }
}