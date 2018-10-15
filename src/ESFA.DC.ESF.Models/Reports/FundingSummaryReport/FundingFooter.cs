using System;

namespace ESFA.DC.ESF.Models.Reports.FundingSummaryReport
{
    public class FundingFooter
    {
        public string ApplicationVersion { get; set; }

        public string LARSData { get; set; }

        public string PostcodeData { get; set; }

        public string OrganisationData { get; set; }

        public DateTime ReportGeneratedAt { get; set; }
    }
}