using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr
{
    public class BaseILRDataStrategy
    {
        private const string PeriodPrefix = "Period_";

        protected  virtual string DeliverableCode { get; set; }

        protected virtual List<string> AttributeNames { get; set; }

        public bool IsMatch(string deliverableCode, List<string> attributeNames = null)
        {
            if (attributeNames != null)
            {
                var firstNotSecond = attributeNames.Except(AttributeNames).ToList();
                var secondNotFirst = AttributeNames.Except(attributeNames).ToList();
                return deliverableCode == DeliverableCode && !firstNotSecond.Any() && !secondNotFirst.Any();
            }

            return deliverableCode == DeliverableCode;
        }

        public void Execute(
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> ilrData,
            IList<FundingSummaryReportYearlyValueModel> yearlyData)
        {
            var data = ilrData.Where(d => d.DeliverableCode == DeliverableCode && AttributeNames.Contains(d.AttributeName)).ToList();

            var yearData = new FundingSummaryReportYearlyValueModel();
            for (var i = 1; i < 13; i++)
            {
                yearData.Values[i - 1] = GetPeriodValueSum(data, i);
            }

            yearlyData.Add(yearData);
        }

        private decimal GetPeriodValueSum(IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> data, int period)
        {
            return data.Sum(v => (decimal)(v.GetType().GetProperty($"{PeriodPrefix}{period.ToString()}")?.GetValue(v) ?? 0M));
        }
    }
}
