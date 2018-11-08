using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Database.EF;

namespace ESFA.DC.ESF.Interfaces.DataAccessLayer
{
    public interface IEsfRepository
    {
        Task<IList<SourceFile>> AllPreviousFilesForValidation(string ukPrn, CancellationToken cancellationToken);

        Task<SourceFile> PreviousFiles(string ukPrn, string conRefNumber, CancellationToken cancellationToken);

        Task<IList<string>> GetAdditionalContractsForProvider(
            string ukPrn,
            CancellationToken cancellationToken,
            string conRefNum = null);

        Task<IList<SupplementaryData>> PreviousSupplementaryData(
            int sourceFileId,
            CancellationToken cancellationToken);
    }
}