using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Reports.Services;
using ESFA.DC.ESF.Models;
using ESFA.DC.ILRLegacy.DataStore.Interfaces.Services;

namespace ESFA.DC.ESF.ReportingService.Services
{
    public class LegacyILRService : ILegacyILRService
    {
        private readonly IFM70DataService _fm70Service;
        private readonly IFileDetailsDataService _fileDetailsService;

        public LegacyILRService(
            IFM70DataService fm70Service,
            IFileDetailsDataService fileDetailsService)
        {
            _fileDetailsService = fileDetailsService;
            _fm70Service = fm70Service;
        }

        public async Task<IEnumerable<ILRFileDetailsModel>> GetPreviousYearsILRFileDetails(
            int ukPrn,
            CancellationToken cancellationToken)
        {
            var fileDetails = await _fileDetailsService.GetFileDetailsForUkPrn(ukPrn, cancellationToken);

            return fileDetails?.Select(fd => new ILRFileDetailsModel
            {
                FileName = fd.FileName,
                LastSubmission = fd.LastSubmission
            }).ToList();
        }

        public async Task<IEnumerable<FM70PeriodisedValuesModel>> GetPreviousYearsFM70Data(
            int ukPrn,
            CancellationToken cancellationToken)
        {
            var fm70Data = await _fm70Service.GetPeriodisedValues(ukPrn, cancellationToken);

            return fm70Data?.Select(fd => new FM70PeriodisedValuesModel
            {
                FundingYear = fd.FundingYear,
                UKPRN = fd.UKPRN,
                LearnRefNumber = fd.LearnRefNumber,
                DeliverableCode = fd.DeliverableCode,
                AimSeqNumber = fd.AimSeqNumber,
                AttributeName = fd.AttributeName,
                Period1 = fd.Period1,
                Period2 = fd.Period2,
                Period3 = fd.Period3,
                Period4 = fd.Period4,
                Period5 = fd.Period5,
                Period6 = fd.Period6,
                Period7 = fd.Period7,
                Period8 = fd.Period8,
                Period9 = fd.Period9,
                Period10 = fd.Period10,
                Period11 = fd.Period11,
                Period12 = fd.Period12
            }).ToList();
        }
    }
}