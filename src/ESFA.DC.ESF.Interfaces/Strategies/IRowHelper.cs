using System.Collections.Generic;
using CsvHelper;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.Interfaces.Strategies
{
    public interface IRowHelper
    {
        bool IsMatch(RowType rowType);

        void Execute(
            CsvWriter writer,
            FundingReportRow row,
            IList<SupplementaryDataModel> esfDataModels);
    }
}