using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Database.EF;
using ESFA.DC.ESF.Database.EF.Interfaces;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.DataAccessLayer
{
    public class EsfRepository : IEsfRepository
    {
        private readonly IESF_DataStoreEntities _context;
        private readonly ILogger _logger;

        public EsfRepository(
            IESF_DataStoreEntities context,
            ILogger logger
        )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IList<SourceFile>> PreviousFiles(string ukPrn, string conRefNumber, CancellationToken cancellationToken)
        {
            IList<SourceFile> sourceFiles = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                sourceFiles = await _context.SourceFiles.Where(s => s.UKPRN == ukPrn && s.ConRefNumber == conRefNumber)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get ESF SuppData source file data", ex);
            }

            return sourceFiles;
        }
    }
}