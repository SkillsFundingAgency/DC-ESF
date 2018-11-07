using System;
using System.Collections.Generic;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.ULN.Model;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Models.Validation;
using ESFA.DC.ReferenceData.FCS.Model;

namespace ESFA.DC.ESF.DataAccessLayer
{
    public class ReferenceDataCache : IReferenceDataCache
    {
        public ReferenceDataCache()
        {
            Ulns = new List<UniqueLearnerNumber>();
            CodeMappings = new List<ContractDeliverableCodeMapping>();
            ProviderNameByUkprn = new Dictionary<int, string>();
            LarsLearningDeliveries = new List<LARS_LearningDelivery>();
            ContractAllocations = new List<ContractAllocationCacheModel>();
        }

        public List<UniqueLearnerNumber> Ulns { get; }

        public List<ContractDeliverableCodeMapping> CodeMappings { get; }

        public List<ContractAllocationCacheModel> ContractAllocations { get; }

        public IDictionary<int, string> ProviderNameByUkprn { get; }

        public List<LARS_LearningDelivery> LarsLearningDeliveries { get; }
    }
}