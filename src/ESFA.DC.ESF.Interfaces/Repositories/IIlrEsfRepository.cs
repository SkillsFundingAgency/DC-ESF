using System.Collections.Generic;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.Interfaces.Repositories
{
    public interface IIlrEsfRepository
    {
        FileDetail GetFileDetails(int ukPrn);

        IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> GetPeriodisedValues(int ukPrn);
    }
}