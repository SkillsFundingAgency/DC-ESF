using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Reports.Services;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ReportingService.Services
{
    public class ILRService : IILRService
    {
        private readonly IFM70Repository _repository;
        private readonly ILegacyILRService _legacyIlrService;

        public ILRService(
            IFM70Repository repository,
            ILegacyILRService legacyILRService)
        {
            _repository = repository;
            _legacyIlrService = legacyILRService;
        }

        public async Task<IEnumerable<ILRFileDetailsModel>> GetIlrFileDetails(int ukPrn, CancellationToken cancellationToken)
        {
            ILRFileDetailsModel ilrFileData = await _repository.GetFileDetails(ukPrn, cancellationToken);

            IEnumerable<ILRFileDetailsModel> previousYearsFiles = await _legacyIlrService.GetPreviousYearsILRFileDetails(ukPrn, cancellationToken);

            var ilrYearlyFileData = new List<ILRFileDetailsModel>();
            if (previousYearsFiles != null)
            {
                ilrYearlyFileData.AddRange(previousYearsFiles.OrderBy(fd => fd.Year));
            }

            ilrYearlyFileData.Add(ilrFileData);

            return ilrYearlyFileData;
        }

        public async Task<IEnumerable<FM70PeriodisedValuesYearlyModel>> GetYearlyIlrData(int ukPrn, CancellationToken cancellationToken)
        {
            IList<FM70PeriodisedValuesModel> ilrData = await _repository.GetPeriodisedValues(ukPrn, cancellationToken);
            var previousYearsILRData = await _legacyIlrService.GetPreviousYearsFM70Data(ukPrn, cancellationToken);
            var allILRData = new List<FM70PeriodisedValuesModel>();
            if (previousYearsILRData != null)
            {
                allILRData.AddRange(previousYearsILRData);
            }

            allILRData.AddRange(ilrData);
            var fm70YearlyData = GroupFm70DataIntoYears(allILRData);

            return fm70YearlyData;
        }

        private IEnumerable<FM70PeriodisedValuesYearlyModel> GroupFm70DataIntoYears(IList<FM70PeriodisedValuesModel> fm70Data)
        {
            var yearlyFm70Data = new List<FM70PeriodisedValuesYearlyModel>();
            if (fm70Data == null)
            {
                return yearlyFm70Data;
            }

            var groupings = fm70Data.GroupBy(sd => sd.FundingYear);

            foreach (var yearGroup in groupings)
            {
                yearlyFm70Data.Add(new FM70PeriodisedValuesYearlyModel
                {
                    FundingYear = yearGroup.Key,
                    Fm70PeriodisedValues = yearGroup.ToList()
                });
            }

            return yearlyFm70Data;
        }
    }
}