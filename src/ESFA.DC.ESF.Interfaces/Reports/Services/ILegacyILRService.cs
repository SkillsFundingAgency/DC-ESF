using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.Reports.Services
{
    public interface ILegacyILRService
    {
        Task<IList<ILRFileDetailsModel>> GetPreviousYearsILRFileDetails(
            int ukPrn,
            CancellationToken cancellationToken);

        Task<IList<FM70PeriodisedValuesModel>> GetPreviousYearsFM70Data(
            int ukPrn,
            CancellationToken cancellationToken);
    }
}