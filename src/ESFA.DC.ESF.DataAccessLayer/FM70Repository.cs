using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ILR1819.DataStore.EF;
using ESFA.DC.ILR1819.DataStore.EF.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.DataAccessLayer
{
    public class FM70Repository : IFM70Repository
    {
        private readonly IILR1819_DataStoreEntities _context;
        private readonly ILogger _logger;

        public FM70Repository(
            IILR1819_DataStoreEntities context,
            ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<FileDetail> GetFileDetails(int ukPrn, CancellationToken cancellationToken)
        {
            FileDetail fileDetail = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                fileDetail = await _context.FileDetails
                    .Where(fd => fd.UKPRN == ukPrn)
                    .OrderBy(fd => fd.SubmittedTime)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get file details with ukPrn {ukPrn}", ex);
            }

            return fileDetail;
        }

        public async Task<IList<ESF_LearningDelivery>> GetLearningDeliveries(int ukPrn, CancellationToken cancellationToken)
        {
            IList<ESF_LearningDelivery> values = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                values = await _context.ESF_LearningDelivery
                    .Where(v => v.UKPRN == ukPrn)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get FM70 learningDeliveries with ukPrn {ukPrn}", ex);
            }

            return values;
        }

        public async Task<IList<ESF_LearningDeliveryDeliverable>> GetLearningDeliveryDeliverables(int ukPrn, CancellationToken cancellationToken)
        {
            IList<ESF_LearningDeliveryDeliverable> values = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                values = await _context.ESF_LearningDeliveryDeliverable
                    .Where(v => v.UKPRN == ukPrn)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get FM70 LearningDeliveryDeliverables with ukPrn {ukPrn}", ex);
            }

            return values;
        }

        public async Task<IList<ESF_LearningDeliveryDeliverable_Period>> GetLearningDeliveryDeliverablePeriods(int ukPrn, CancellationToken cancellationToken)
        {
            IList<ESF_LearningDeliveryDeliverable_Period> values = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                values = await _context.ESF_LearningDeliveryDeliverable_Period
                    .Where(v => v.UKPRN == ukPrn)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get FM70 LearningDeliveryDeliverablePeriods with ukPrn {ukPrn}", ex);
            }

            return values;
        }

        public async Task<IList<ESF_LearningDeliveryDeliverable_PeriodisedValues>> GetPeriodisedValues(int ukPrn, CancellationToken cancellationToken)
        {
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> values = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                values = await _context.ESF_LearningDeliveryDeliverable_PeriodisedValues
                    .Where(v => v.UKPRN == ukPrn)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get FM70 periodised values with ukPrn {ukPrn}", ex);
            }

            return values;
        }

        public async Task<IList<ESF_DPOutcome>> GetOutcomes(int ukPrn, CancellationToken cancellationToken)
        {
            IList<ESF_DPOutcome> values = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                values = await _context.ESF_DPOutcome
                    .Where(v => v.UKPRN == ukPrn)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get FM70 DPOutcomes with ukPrn {ukPrn}", ex);
            }

            return values;
        }
    }
}
