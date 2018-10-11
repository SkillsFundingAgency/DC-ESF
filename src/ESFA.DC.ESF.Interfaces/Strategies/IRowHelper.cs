using System.Collections.Generic;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.Interfaces.Strategies
{
    public interface IRowHelper
    {
        bool IsMatch(RowType rowType);

        void Execute(
            IList<FundingSummaryReportRowModel> reportOutput,
            FundingReportRow row,
            IList<SupplementaryDataModel> esfDataModels,
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> irlData);
    }
}