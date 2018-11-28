using System.Collections.Generic;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.Interfaces.Reports.Strategies
{
    public interface IILRDataStrategy
    {
        bool IsMatch(string deliverableCode, List<string> attributeNames = null);

        void Execute(
            IList<FM70PeriodisedValuesYearlyModel> irlData,
            IList<FundingSummaryReportYearlyValueModel> reportRowYearlyValues);
    }
}