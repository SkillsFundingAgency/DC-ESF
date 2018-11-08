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
            ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IList<string>> GetAdditionalContractsForProvider(
            string ukPrn,
            CancellationToken cancellationToken,
            string conRefNum = null)
        {
            List<string> contractRefNums = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                contractRefNums = await _context.SourceFiles
                    .Where(sf => sf.UKPRN == ukPrn && sf.ConRefNumber != conRefNum)
                    .Select(sf => sf.ConRefNumber)
                    .ToListAsync(cancellationToken);

                contractRefNums = contractRefNums.Distinct().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get Additional ESF SuppData source file data", ex);
            }

            return contractRefNums;
        }

        public async Task<SourceFile> PreviousFiles(string ukPrn, string conRefNumber, CancellationToken cancellationToken)
        {
            SourceFile sourceFile = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                sourceFile = await _context.SourceFiles.Where(s => s.UKPRN == ukPrn && s.ConRefNumber == conRefNumber)
                    .OrderByDescending(c => c.DateTime).FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get previous ESF SuppData source file data", ex);
            }

            return sourceFile;
        }

        public async Task<IList<SourceFile>> AllPreviousFilesForValidation(string ukPrn, CancellationToken cancellationToken)
        {
            List<SourceFile> sourceFiles = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                sourceFiles = await _context.SourceFiles.Where(s => s.UKPRN == ukPrn)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get previous ESF SuppData source file data", ex);
            }

            return sourceFiles;
        }

        public async Task<IList<SupplementaryData>> PreviousSupplementaryData(
            int sourceFileId,
            CancellationToken cancellationToken)
        {
            List<SupplementaryData> data = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                data = await _context.SupplementaryDatas.Where(s => s.SourceFileId == sourceFileId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get ESF SuppData source file data", ex);
            }

            return data;
        }
    }
}