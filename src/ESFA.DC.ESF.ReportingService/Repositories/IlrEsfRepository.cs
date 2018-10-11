using System.Collections.Generic;
using System.Linq;
using ESFA.DC.ESF.Interfaces.Repositories;
using ESFA.DC.ILR1819.DataStore.EF;
using ESFA.DC.ILR1819.DataStore.EF.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Repositories
{
    public class IlrEsfRepository : IIlrEsfRepository
    {
        private readonly IILR1819_DataStoreEntities _context;

        public IlrEsfRepository(IILR1819_DataStoreEntities context)
        {
            _context = context;
        }

        public FileDetail GetFileDetails(int ukPrn)
        {
            return _context.FileDetails
                .Where(fd => fd.UKPRN == ukPrn)
                .OrderBy(fd => fd.SubmittedTime)
                .FirstOrDefault();
        }

        public IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> GetPeriodisedValues(int ukPrn)
        {
            return _context.ESF_LearningDeliveryDeliverable_PeriodisedValues
                .Where(v => v.UKPRN == ukPrn)
                .ToList();
        }
    }
}
