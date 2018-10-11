using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;

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
            IList<FundingSummaryReportRowModel> reportOutput,
            FundingReportRow row,
            IList<SupplementaryDataModel> esfDataModels,
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> ilrData)
        {
            reportOutput.Add( new FundingSummaryReportRowModel
            {
                RowType = RowType
            });
        }
    }
}
