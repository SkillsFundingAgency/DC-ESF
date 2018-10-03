using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.Interfaces.DataStore
{
    public interface IStoreFileDetails
    {
        Task<int> StoreAsync(SqlConnection sqlConnection, SqlTransaction sqlTransaction, CancellationToken cancellationToken, SourceFileModel sourceFile);
    }
}