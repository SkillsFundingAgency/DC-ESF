using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class BaseSupplementaryDataStrategy
    {
        private const int EsfMonthPadding = 7;

        protected string DeliverableCode;

        protected string ReferenceType;

        public bool IsMatch(string deliverableCode)
        {
            return deliverableCode == DeliverableCode;
        }

        public void Execute(IList<SupplementaryDataModel> data, FundingSummaryReportYearlyValueModel esf)
        {
            for (var i = 1; i < 13; i++)
            {
                esf.Values[i - 1] = data.SingleOrDefault(supp => supp.CalendarMonth == i + EsfMonthPadding
                                                                 && supp.DeliverableCode == DeliverableCode
                                                                 && supp.ReferenceType == ReferenceType)?.Value ?? 0.0M;
            }
        }
    }
}
