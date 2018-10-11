﻿using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
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
            IList<FundingSummaryReportRowModel> reportOutput,
            FundingReportRow row,
            IList<SupplementaryDataModel> esfDataModels,
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> ilrData)
        {
            var deliverableCodes = row.DeliverableCode?.Split(',').ToList();
            
            var reportRowsToTotal = deliverableCodes == null ? 
                reportOutput.Where(r => r.RowType == RowType.Data).ToList() : 
                reportOutput.Where(r => deliverableCodes.Contains(row.DeliverableCode) && r.RowType == RowType.Data).ToList();

            if (!reportRowsToTotal.Any())
            {
                return;
            }

            var rowModel =new FundingSummaryReportRowModel
            {
                RowType = RowType.Total,
                Title = row.Title,
                DeliverableCode = row.DeliverableCode 
            };

            var yearlyValueTotals = new List<FundingSummaryReportYearlyValueModel>();
            foreach (var yearlyValue in reportRowsToTotal.First().YearlyValues)
            {
                var yearlyModel = new FundingSummaryReportYearlyValueModel
                {
                    FundingYear = yearlyValue.FundingYear
                };

                var periodValues = reportRowsToTotal.SelectMany(r => r.YearlyValues).ToList();

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
            var total = 0M;
            foreach (var model in yearlyModel)
            {
                total += model.Values[period];
            }

            return total;
        }
    }
}