using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class BaseSupplementaryDataStrategy
    {
        private const string PeriodPrefix = "Period_";

        private const int EsfMonthPadding = 7;

        protected virtual string DeliverableCode { get; set; }

        protected virtual string ReferenceType { get; set; }

        public bool IsMatch(string deliverableCode, string referenceType = null)
        {
            if (referenceType != null)
            {
                return deliverableCode == DeliverableCode && referenceType == ReferenceType;
            }

            return deliverableCode == DeliverableCode;
        }

        public void Execute(
            IList<SupplementaryDataModel> data,
            IList<FundingSummaryReportYearlyValueModel> yearlyData)
        {
            // todo for each year of data

            var yearData = new FundingSummaryReportYearlyValueModel();
            for (var i = 1; i < 13; i++)
            {
                var deliverableData = data.Where(supp => supp.CalendarMonth == i + EsfMonthPadding
                                                         && supp.DeliverableCode == DeliverableCode);
                if (ReferenceType != null)
                {
                    deliverableData =
                        deliverableData.Where(supp => supp.ReferenceType == ReferenceType);
                }

                yearData.Values[i - 1] = GetPeriodValueSum(deliverableData, i);
            }

            yearlyData.Add(yearData);
        }

        private decimal GetPeriodValueSum(IEnumerable<SupplementaryDataModel> data, int period)
        {
            return data.Sum(v => (decimal)(v.GetType().GetProperty($"{PeriodPrefix}{period.ToString()}")?.GetValue(v) ?? 0M));
        }
    }
}
