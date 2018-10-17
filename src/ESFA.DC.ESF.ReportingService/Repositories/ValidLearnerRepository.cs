using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.ILR1819.DataStore.EF.Valid;
using ESFA.DC.ILR1819.DataStore.EF.Valid.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Repositories
{
    public class ValidLearnerRepository
    {
        private readonly IILR1819_DataStoreEntitiesValid _context;
        private readonly ILogger _logger;

        public ValidLearnerRepository(
            IILR1819_DataStoreEntitiesValid context,
            ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Learner> GetValidLearners(int ukPrn, CancellationToken cancellationToken)
        {
            List<Learner> learners = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                learners = _context.Learners.Where(l => l.UKPRN == ukPrn).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get valid learners with ukPrn {ukPrn}", ex);
            }

            return learners;
        }
    }
}