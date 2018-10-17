using System.Collections.Generic;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.Interfaces.Reports.Strategies
{
    public interface IILRDataStrategy
    {
        bool IsMatch(string deliverableCode, List<string> attributeNames = null);
        void Execute(
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> irlData, 
            IList<FundingSummaryReportYearlyValueModel> reportRowYearlyValues);
    }
}