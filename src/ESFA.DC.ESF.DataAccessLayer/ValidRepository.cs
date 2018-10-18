using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ILR1819.DataStore.EF.Valid;
using ESFA.DC.ILR1819.DataStore.EF.Valid.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Repositories
{
    public class ValidRepository : IValidRepository
    {
        private readonly IILR1819_DataStoreEntitiesValid _context;
        private readonly ILogger _logger;

        public ValidRepository(
            IILR1819_DataStoreEntitiesValid context,
            ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Learner>> GetLearners(int ukPrn, CancellationToken cancellationToken)
        {
            List<Learner> learners = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                learners = await _context.Learners.Where(l => l.UKPRN == ukPrn).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get valid learners with ukPrn {ukPrn}", ex);
            }

            return learners;
        }

        public async Task<List<LearningDelivery>> GetLearningDeliveries(int ukPrn, CancellationToken cancellationToken)
        {
            List<LearningDelivery> deliveries = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                deliveries = await _context.LearningDeliveries.Where(l => l.UKPRN == ukPrn && l.FundModel == 70).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get valid LearningDeliveries with ukPrn {ukPrn}", ex);
            }

            return deliveries;
        }

        public async Task<List<LearningDeliveryFAM>> GetLearningDeliveryFAMs(int ukPrn, CancellationToken cancellationToken)
        {
            List<LearningDeliveryFAM> deliveryFams = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                deliveryFams =  await _context.LearningDeliveryFAMs.Where(l => l.UKPRN == ukPrn).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get valid LearningDeliveryFAMs with ukPrn {ukPrn}", ex);
            }

            return deliveryFams;
        }

        public async Task<List<ProviderSpecLearnerMonitoring>> GetProviderSpecLearnerMonitorings(int ukPrn, CancellationToken cancellationToken)
        {
            List<ProviderSpecLearnerMonitoring> monitorings = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                monitorings = await _context.ProviderSpecLearnerMonitorings.Where(l => l.UKPRN == ukPrn).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get valid ProviderSpecLearnerMonitorings with ukPrn {ukPrn}", ex);
            }

            return monitorings;
        }

        public async Task<List<ProviderSpecDeliveryMonitoring>> GetProviderSpecDeliveryMonitorings(int ukPrn, CancellationToken cancellationToken)
        {
            List<ProviderSpecDeliveryMonitoring> monitorings = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                monitorings = await _context.ProviderSpecDeliveryMonitorings.Where(l => l.UKPRN == ukPrn).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get valid ProviderSpecDeliveryMonitorings with ukPrn {ukPrn}", ex);
            }

            return monitorings;
        }

        public async Task<List<DPOutcome>> GetDPOutcomes(int ukPrn, CancellationToken cancellationToken)
        {
            List<DPOutcome> outcomes = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                outcomes = await _context.DPOutcomes.Where(l => l.UKPRN == ukPrn).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get valid DPOutcomes with ukPrn {ukPrn}", ex);
            }

            return outcomes;
        }
    }
}