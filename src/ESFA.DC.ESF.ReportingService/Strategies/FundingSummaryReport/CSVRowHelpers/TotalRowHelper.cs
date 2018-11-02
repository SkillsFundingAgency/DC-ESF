using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers
{
    public class TotalRowHelper : IRowHelper
    {
        private readonly RowType RowType = RowType.Total;

        public bool IsMatch(RowType rowType)
        {
            return rowType == RowType;
        }

        public void Execute(
            IList<FundingSummaryModel> reportOutput,
            FundingReportRow row,
            IList<SupplementaryDataModel> esfDataModels,
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> ilrData)
        {
            List<string> deliverableCodes = row.DeliverableCode?.Split(',').Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            List<FundingSummaryModel> reportRowsToTotal = deliverableCodes == null ?
                reportOutput.Where(r => r.ExcelRecordStyle == 4).ToList() :
                reportOutput.Where(r => deliverableCodes.Contains(r.DeliverableCode) && r.ExcelRecordStyle == 4).ToList();

            if (!reportRowsToTotal.Any())
            {
                return;
            }

            FundingSummaryModel rowModel = new FundingSummaryModel(row.Title, HeaderType.None, 2)
            {
                DeliverableCode = row.DeliverableCode
            };

            List<FundingSummaryReportYearlyValueModel> yearlyValueTotals = new List<FundingSummaryReportYearlyValueModel>();
            foreach (var yearlyValue in reportRowsToTotal.First().YearlyValues)
            {
                var yearlyModel = new FundingSummaryReportYearlyValueModel
                {
                    FundingYear = yearlyValue.FundingYear
                };

                List<FundingSummaryReportYearlyValueModel> periodValues = reportRowsToTotal.SelectMany(r => r.YearlyValues).ToList();

                for (var i = 0; i < (periodValues.FirstOrDefault()?.Values.Length ?? 0); i++)
                {
                    yearlyModel.Values[i] = GetPeriodTotals(periodValues, i);
                }

                yearlyValueTotals.Add(yearlyModel);
            }

            rowModel.YearlyValues = yearlyValueTotals;

            yearlyValueTotals.ForEach(v =>
            {
                rowModel.Totals.Add(v.Values.Sum());
            });

            rowModel.Totals.Add(rowModel.Totals.Sum());

            reportOutput.Add(rowModel);
        }

        private decimal GetPeriodTotals(
            List<FundingSummaryReportYearlyValueModel> yearlyModel,
            int period)
        {
            decimal total = 0M;
            foreach (var model in yearlyModel)
            {
                total += model.Values[period];
            }

            return total;
        }
    }
}