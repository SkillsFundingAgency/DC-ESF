using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers
{
    public class DataRowHelper : IRowHelper
    {
        private const string EsfCodeBase = "ESF";

        private readonly IList<ISupplementaryDataStrategy> _esfStrategies;

        private readonly IList<IILRDataStrategy> _ilrStrategies;

        private readonly RowType RowType = RowType.Data;

        public DataRowHelper(
            IList<ISupplementaryDataStrategy> esfStrategies,
            IList<IILRDataStrategy> ilrStrategies)
        {
            _esfStrategies = esfStrategies;
            _ilrStrategies = ilrStrategies;
        }

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
            var reportRow = new FundingSummaryReportRowModel
            {
                Title = row.Title,
                RowType = RowType,
                DeliverableCode = row.DeliverableCode
            };

            var reportRowYearlyValues = new List<FundingSummaryReportYearlyValueModel>();
            var codeBase = row.CodeBase;
            if (codeBase == EsfCodeBase)
            {
                foreach (var strategy in _esfStrategies)
                {
                    if (row.ReferenceType != null)
                    {
                        if (!strategy.IsMatch(row.DeliverableCode, row.ReferenceType))
                        {
                            continue;
                        }
                    }

                    if (!strategy.IsMatch(row.DeliverableCode))
                    {
                        continue;
                    }

                    strategy.Execute(esfDataModels, reportRowYearlyValues);
                    break;
                }
            }
            else
            {
                if (ilrData != null)
                {
                    foreach (var strategy in _ilrStrategies)
                    {
                        if (row.AttributeNames != null)
                        {
                            if (!strategy.IsMatch(row.DeliverableCode, row.AttributeNames))
                            {
                                continue;
                            }
                        }

                        if (!strategy.IsMatch(row.DeliverableCode))
                        {
                            continue;
                        }

                        strategy.Execute(ilrData, reportRowYearlyValues);
                        break;
                    }
                }
            }

            reportRow.YearlyValues = reportRowYearlyValues;

            reportRowYearlyValues.ForEach(v =>
            {
                reportRow.Totals.Add(v.Values.Sum());
            });

            reportRow.Totals.Add(reportRow.Totals.Sum());

            reportOutput.Add(reportRow);
        }
    }
}
