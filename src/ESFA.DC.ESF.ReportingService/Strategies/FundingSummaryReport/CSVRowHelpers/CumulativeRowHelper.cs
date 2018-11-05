using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers
{
    public class CumulativeRowHelper : IRowHelper
    {
        public bool IsMatch(RowType rowType)
        {
            return rowType == RowType.Cumulative || rowType == RowType.FinalCumulative;
        }

        public void Execute(
            IList<FundingSummaryModel> reportOutput,
            FundingReportRow row,
            IList<SupplementaryDataModel> esfDataModels,
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> ilrData)
        {
            FundingSummaryModel rowModel = new FundingSummaryModel(row.Title, HeaderType.None, 3);
            FundingSummaryModel grandTotalRow = reportOutput.FirstOrDefault(r => r.Title == "<ESF-1> Total (£)");

            if (row.RowType == RowType.FinalCumulative)
            {
                rowModel.ExcelHeaderStyle = 0;
                rowModel.ExcelRecordStyle = 0;
            }

            if (grandTotalRow == null)
            {
                reportOutput.Add(rowModel);
                return;
            }

            var yearlyValues = new List<FundingSummaryReportYearlyValueModel>();
            var cumulativeTotal = 0M;
            foreach (var year in grandTotalRow.YearlyValues)
            {
                var yearValues = new FundingSummaryReportYearlyValueModel();
                for (var i = 0; i < 12; i++)
                {
                    cumulativeTotal += year.Values[i];
                    yearValues.Values[i] = cumulativeTotal;
                }

                yearlyValues.Add(yearValues);
            }

            rowModel.YearlyValues = yearlyValues;

            var yearEndCumulative = 0M;
            for (var index = 0; index < grandTotalRow.Totals.Count - 1; index++)
            {
                var total = grandTotalRow.Totals[index];
                yearEndCumulative += total;
                rowModel.Totals.Add(yearEndCumulative);
            }

            reportOutput.Add(rowModel);
        }
    }
}
