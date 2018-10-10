using System.Collections.Generic;
using CsvHelper;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers
{
    public class TitleRowHelper : IRowHelper
    {
        private readonly RowType RowType = RowType.Header;

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
            writer.NextRecord();
        }
    }
}
