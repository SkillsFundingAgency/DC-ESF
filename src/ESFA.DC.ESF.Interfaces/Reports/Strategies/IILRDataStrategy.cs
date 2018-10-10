using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.Interfaces.Reports.Strategies
{
    public interface IILRDataStrategy
    {
        bool IsMatch(string deliverableCode);
        void Execute(int ukPrn, FundingSummaryReportYearlyValueModel esf);
    }
}