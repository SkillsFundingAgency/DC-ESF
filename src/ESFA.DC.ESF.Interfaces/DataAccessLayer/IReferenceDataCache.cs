using System.Collections.Generic;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.Data.ULN.Model;
using ESFA.DC.ESF.Models.Validation;
using ESFA.DC.ReferenceData.FCS.Model;

namespace ESFA.DC.ESF.Interfaces.DataAccessLayer
{
    public interface IReferenceDataCache
    {
        List<UniqueLearnerNumber> Ulns { get; }

        List<ContractDeliverableCodeMapping> CodeMappings { get; }

        List<ContractAllocationCacheModel> ContractAllocations { get; }

        IDictionary<int, string> ProviderNameByUkprn { get; }

        List<LARS_LearningDelivery> LarsLearningDeliveries { get; }
    }
}