using System;

namespace ESFA.DC.ESF.Models.Validation
{
    public class ContractAllocationCacheModel
    {
        public int DeliverableCode { get; set; }

        public string ContractAllocationNumber { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}