using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr
{
    public class BaseILRDataStrategy
    {
        private const string PeriodPrefix = "Period_";

        protected virtual string DeliverableCode { get; set; }

        protected virtual List<string> AttributeNames { get; set; }

        public bool IsMatch(string deliverableCode, List<string> attributeNames = null)
        {
            if (attributeNames == null)
            {
                return deliverableCode == DeliverableCode;
            }

            var firstNotSecond = attributeNames.Except(AttributeNames).ToList();
            var secondNotFirst = AttributeNames.Except(attributeNames).ToList();
            return deliverableCode == DeliverableCode && !firstNotSecond.Any() && !secondNotFirst.Any();
        }

        public void Execute(
            IList<FM70PeriodisedValuesYearlyModel> ilrData,
            IList<FundingSummaryReportYearlyValueModel> yearlyData)
        {
            foreach (var year in ilrData)
            {
                var data = year.Fm70PeriodisedValues.Where(d =>
                    d.DeliverableCode == DeliverableCode && AttributeNames.Contains(d.AttributeName)).ToList();

                var yearData = new FundingSummaryReportYearlyValueModel();
                for (var i = 1; i < 13; i++)
                {
                    yearData.Values[i - 1] = GetPeriodValueSum(data, i);
                }

                yearData.FundingYear = year.FundingYear;
                yearlyData.Add(yearData);
            }
        }

        private static decimal GetPeriodValueSum(IEnumerable<FM70PeriodisedValuesModel> data, int period)
        {
            return data.Sum(v => (decimal)(v.GetType().GetProperty($"{PeriodPrefix}{period.ToString()}")?.GetValue(v) ?? 0M));
        }
    }
}
