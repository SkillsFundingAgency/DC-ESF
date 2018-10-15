using System.Collections.Generic;

namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public class FundingHeader
    {
        public string ProviderName { get; set; }

        public string UKPRN { get; set; }

        public string ContractReferenceNumber { get; set; }

        public string SupplementaryDataFile { get; set; }

        public string LastSupplementaryDataFileUpdate { get; set; }

        public string SecurityClassification => "OFFICIAL-SENSITIVE";

        public List<FundingHeaderYear> FundingYears { get; set; }

        public class FundingHeaderYear
        {
            public string Header { get; set; }

            public string ILRFile { get; set; }

            public string LastILRFileUpdate { get; set; }

            public string FilePreparationDate { get; set; }
        }
    }
}