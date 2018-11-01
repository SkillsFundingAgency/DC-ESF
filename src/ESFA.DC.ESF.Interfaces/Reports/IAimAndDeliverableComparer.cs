using ESFA.DC.ESF.Models.Reports;

namespace ESFA.DC.ESF.Interfaces.Reports
{
    public interface IAimAndDeliverableComparer
    {
        int Compare(AimAndDeliverableModel first, AimAndDeliverableModel second);
    }
}