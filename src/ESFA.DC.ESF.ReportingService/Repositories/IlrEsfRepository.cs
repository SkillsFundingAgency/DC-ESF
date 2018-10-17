using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.ESF.Interfaces.Repositories;
using ESFA.DC.ILR1819.DataStore.EF;
using ESFA.DC.ILR1819.DataStore.EF.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Repositories
{
    public class IlrEsfRepository : IIlrEsfRepository
    {
        private readonly IILR1819_DataStoreEntities _context;
        private readonly ILogger _logger;

        public IlrEsfRepository(
            IILR1819_DataStoreEntities context,
            ILogger logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public FileDetail GetFileDetails(int ukPrn, CancellationToken cancellationToken)
        {
            FileDetail fileDetail = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                fileDetail = _context.FileDetails
                    .Where(fd => fd.UKPRN == ukPrn)
                    .OrderBy(fd => fd.SubmittedTime)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get file details with ukPrn {ukPrn}", ex);
            }

            return fileDetail;
        }

        public IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> GetPeriodisedValues(int ukPrn, CancellationToken cancellationToken)
        {
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> values = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                values = _context.ESF_LearningDeliveryDeliverable_PeriodisedValues
                    .Where(v => v.UKPRN == ukPrn)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get FM70 periodised values with ukPrn {ukPrn}", ex);
            }

            return values;
        }
    }
}
