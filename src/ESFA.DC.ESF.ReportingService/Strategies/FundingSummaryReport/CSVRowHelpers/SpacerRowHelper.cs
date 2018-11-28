using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers
{
    public class SpacerRowHelper : IRowHelper
    {
        private readonly RowType RowType = RowType.Spacer;

        public bool IsMatch(RowType rowType)
        {
            return rowType == RowType;
        }

        public void Execute(
            IList<FundingSummaryModel> reportOutput,
            FundingReportRow row,
            IList<SupplementaryDataYearlyModel> esfDataModels,
            IList<FM70PeriodisedValuesYearlyModel> ilrData)
        {
            reportOutput.Add(new FundingSummaryModel());
        }
    }
}
