using System;
using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Reports;
using ESFA.DC.ESF.Models.Reports;

namespace ESFA.DC.ESF.ReportingService.Comparers
{
    public class AimAndDeliverableComparer : IComparer<AimAndDeliverableModel>, IAimAndDeliverableComparer
    {
        public int Compare(AimAndDeliverableModel first, AimAndDeliverableModel second)
        {
            if (first == null && second == null)
            {
                return 0;
            }

            if (first == null)
            {
                return -1;
            }

            if (second == null)
            {
                return 1;
            }

            if (first == second)
            {
                return 0;
            }

            var cmp = string.Compare(first.LearnRefNumber, second.LearnRefNumber, StringComparison.OrdinalIgnoreCase);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = string.Compare(first.ConRefNumber, second.ConRefNumber, StringComparison.OrdinalIgnoreCase);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = DateTime.Compare(first.LearnStartDate ?? DateTime.MinValue, second.LearnStartDate ?? DateTime.MinValue);
            if (cmp != 0)
            {
                return cmp;
            }

            if (first.AimSeqNumber != second.AimSeqNumber)
            {
                return first.AimSeqNumber > second.AimSeqNumber ? 1 : -1;
            }

            cmp = string.Compare(first.Period, second.Period, StringComparison.OrdinalIgnoreCase);
            if (cmp != 0)
            {
                return cmp;
            }

            cmp = string.Compare(first.DeliverableCode, second.DeliverableCode, StringComparison.OrdinalIgnoreCase);
            if (cmp != 0)
            {
                return cmp;
            }

            return 0;
        }
    }
}
