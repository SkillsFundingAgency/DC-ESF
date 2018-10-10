using System.Collections.Generic;
using CsvHelper;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers
{
    public class DataRowHelper : IRowHelper
    {
        private const string EsfCodeBase = "ESF";

        private readonly IList<ISupplementaryDataStrategy> _esfStrategies;

        public DataRowHelper(
            IList<ISupplementaryDataStrategy> esfStrategies)
        {
            _esfStrategies = esfStrategies;
        }

        private readonly RowType RowType = RowType.Data;

        public bool IsMatch(RowType rowType)
        {
            return rowType == RowType;
        }

        public void Execute(
            CsvWriter writer,
            FundingReportRow row,
            IList<SupplementaryDataModel> esfDataModels)
        {
            writer.WriteField(row.Title);

            var esf = new FundingSummaryReportYearlyValueModel();
            var codeBase = row.CodeBase;
            if (codeBase == EsfCodeBase)
            {
                foreach (var strategy in _esfStrategies)
                {
                    if (!strategy.IsMatch(row.DeliverableCode))
                    {
                        continue;
                    }
                    strategy.Execute(esfDataModels, esf);
                    break;
                }
            }
            else
            {
                { }
            }


            foreach (var value in esf.Values)
            {
                writer.WriteField(value);
            }
            writer.NextRecord();
        }
    }
}
