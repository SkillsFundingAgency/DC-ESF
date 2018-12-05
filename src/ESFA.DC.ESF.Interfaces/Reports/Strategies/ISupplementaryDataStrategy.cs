using System.Collections.Generic;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.Interfaces.Reports.Strategies
{
    public interface ISupplementaryDataStrategy
    {
        bool IsMatch(string deliverableCode, string referenceType = null);

        void Execute(IEnumerable<SupplementaryDataYearlyModel> data, IList<FundingSummaryReportYearlyValueModel> reportRowYearlyValues);
    }
}