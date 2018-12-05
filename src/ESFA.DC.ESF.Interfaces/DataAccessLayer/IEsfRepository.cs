using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Database.EF;

namespace ESFA.DC.ESF.Interfaces.DataAccessLayer
{
    public interface IEsfRepository
    {
        Task<SourceFile> PreviousFiles(string ukPrn, string conRefNumber, CancellationToken cancellationToken);

        Task<IList<string>> GetContractsForProvider(
            string ukPrn,
            CancellationToken cancellationToken);

        Task<IList<SourceFile>> AllPreviousFilesForValidation(
            string ukPrn,
            string conRefNum,
            CancellationToken cancellationToken);

        Task<IList<SupplementaryData>> GetSupplementaryDataPerSourceFile(
            int sourceFileId,
            CancellationToken cancellationToken);
    }
}