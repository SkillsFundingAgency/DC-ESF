using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.DataStore;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.DataStore
{
    public sealed class StoreFileDetails : IStoreFileDetails
    {
        private SqlConnection _sqlConnection;

        private SqlTransaction _sqlTransaction;

        public async Task<int> StoreAsync(SqlConnection sqlConnection, SqlTransaction sqlTransaction, CancellationToken cancellationToken, SourceFileModel sourceFile)
        {
            _sqlConnection = sqlConnection;
            _sqlTransaction = sqlTransaction;

            return await StoreAsync(sourceFile, cancellationToken);
        }

        private async Task<int> StoreAsync(
            SourceFileModel sourceFile,
            CancellationToken cancellationToken)
        {
            string insertFileDetails =
                    $"INSERT INTO [dbo].[SourceFile] ([ConRefNumber], [UKPRN], [Filename], [DateTime], [FilePreparationDate]) output INSERTED.SourceFileId VALUES ('{sourceFile.ConRefNumber}', '{sourceFile.UKPRN}', '{sourceFile.FileName}', '{sourceFile.SuppliedDate}', '{sourceFile.PreparationDate}')";

            if (cancellationToken.IsCancellationRequested)
            {
                return 0;
            }

            using (var sqlCommand =
                new SqlCommand(insertFileDetails, _sqlConnection, _sqlTransaction))
            {
                return (int)await sqlCommand.ExecuteScalarAsync(cancellationToken);
            }
        }
    }
}
