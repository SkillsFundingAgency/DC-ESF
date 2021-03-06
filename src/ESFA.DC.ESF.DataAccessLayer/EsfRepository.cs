﻿using System;
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

        public async Task<IList<string>> GetContractsForProvider(
            string ukPrn,
            CancellationToken cancellationToken)
        {
            List<string> contractRefNums = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                contractRefNums = await _context.SourceFiles
                    .Join(_context.SupplementaryDatas, sf => sf.SourceFileId, sd => sd.SourceFileId, (sf, sd) => sf) // not all files will have data
                    .Where(sf => sf.UKPRN == ukPrn)
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

                sourceFile = await _context.SourceFiles
                    .Join(_context.SupplementaryDatas, sf => sf.SourceFileId, sd => sd.SourceFileId, (sf, sd) => sf) // not all files will have data
                    .Where(s => s.UKPRN == ukPrn && s.ConRefNumber == conRefNumber)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get ESF SuppData source file data", ex);
            }

            return sourceFile;
        }

        public async Task<IList<SourceFile>> AllPreviousFilesForValidation(
            string ukPrn,
            string conRefNum,
            CancellationToken cancellationToken)
        {
            List<SourceFile> sourceFiles = null;
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                sourceFiles = await _context.SourceFiles
                    .Where(sf => sf.UKPRN == ukPrn && sf.ConRefNumber == conRefNum)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get previous ESF SuppData source file data", ex);
            }

            return sourceFiles;
        }

        public async Task<IList<SupplementaryData>> GetSupplementaryDataPerSourceFile(
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