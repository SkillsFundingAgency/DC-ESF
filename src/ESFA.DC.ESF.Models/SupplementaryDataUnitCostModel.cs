namespace ESFA.DC.ESF.Models
{
    public class SupplementaryDataUnitCostModel
    {
        public string ConRefNumber { get; set; }

        public string DeliverableCode { get; set; }

        public int CalendarYear { get; set; }

        public int CalendarMonth { get; set; }

        public string CostType { get; set; }

        public string StaffName { get; set; }

        public string ReferenceType { get; set; }

        public string Reference { get; set; }

        public decimal? Value { get; set; }
    }
}
