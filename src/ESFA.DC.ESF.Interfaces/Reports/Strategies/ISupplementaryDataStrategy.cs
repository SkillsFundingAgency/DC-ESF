using System.Collections.Generic;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.Interfaces.Reports.Strategies
{
    public interface ISupplementaryDataStrategy
    {
        bool IsMatch(string deliverableCode);

        void Execute(IList<SupplementaryDataModel> data, FundingSummaryReportYearlyValueModel esf);
    }
}