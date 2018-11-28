using System.Collections.Generic;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.Interfaces.Strategies
{
    public interface IRowHelper
    {
        bool IsMatch(RowType rowType);

        void Execute(
            IList<FundingSummaryModel> reportOutput,
            FundingReportRow row,
            IList<SupplementaryDataYearlyModel> esfDataModels,
            IList<FM70PeriodisedValuesYearlyModel> ilrData);
    }
}