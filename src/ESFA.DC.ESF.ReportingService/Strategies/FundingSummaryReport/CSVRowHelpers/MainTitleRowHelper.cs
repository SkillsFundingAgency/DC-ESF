using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers
{
    public sealed class MainTitleRowHelper : IRowHelper
    {
        private readonly RowType RowType = RowType.MainTitle;

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
            reportOutput.Add(new FundingSummaryModel(row.Title, HeaderType.TitleOnly, 0));
        }
    }
}
