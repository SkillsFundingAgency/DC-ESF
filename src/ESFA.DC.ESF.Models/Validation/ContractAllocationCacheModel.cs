using ESFA.DC.ReferenceData.FCS.Model;

namespace ESFA.DC.ESF.Models.Validation
{
    public class ContractAllocationCacheModel
    {
        public int DeliverableCode { get; set; }

        public ContractAllocation ContractAllocation { get; set; }
    }
}