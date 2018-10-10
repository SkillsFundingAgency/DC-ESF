using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Interfaces.Repositories;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class BaseILRDataStrategy
    {
        private const string PeriodPrefix = "Period_";

        protected string DeliverableCode;

        protected List<string> AttributeNames;

        private readonly IIlrEsfRepository _repository;

        public BaseILRDataStrategy(IIlrEsfRepository repository)
        {
            _repository = repository;
        }

        public bool IsMatch(string deliverableCode)
        {
            return deliverableCode == DeliverableCode;
        }

        public void Execute(int ukPrn, FundingSummaryReportYearlyValueModel esf)
        {
            for (var i = 1; i < 13; i++)
            {
                var data = _repository.GetPeriodisedValues(ukPrn, AttributeNames, DeliverableCode);
                esf.Values[i - 1] = GetPeriodValueSum(data, i);
            }
        }

        private decimal GetPeriodValueSum(IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> data, int period)
        {
            return data.Sum(v => (decimal)(v.GetType().GetProperty($"{PeriodPrefix}{period.ToString()}")?.GetValue(v) ?? 0M));
        }
    }
}
