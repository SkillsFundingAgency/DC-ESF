using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers
{
    public class CumulativeRowHelper : IRowHelper
    {
        private readonly RowType RowType = RowType.Cumulative;

        public bool IsMatch(RowType rowType)
        {
            return rowType == RowType;
        }

        public void Execute(
            IList<FundingSummaryReportRowModel> reportOutput,
            FundingReportRow row,
            IList<SupplementaryDataModel> esfDataModels,
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> ilrData)
        {
            var rowModel = new FundingSummaryReportRowModel
            {
                RowType = RowType.Total,
                Title = row.Title,
            };

            var grandTotalRow = reportOutput.FirstOrDefault(r => r.RowType == RowType.Total && string.IsNullOrEmpty(r.DeliverableCode));
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
            }
            rowModel.YearlyValues = yearlyValues;

            var yearEndCumulative = 0M;
            foreach (var total in grandTotalRow.Totals)
            {
                yearEndCumulative += total;
                rowModel.Totals.Add(yearEndCumulative);
            }

            reportOutput.Add(rowModel);
        }
    }
}
