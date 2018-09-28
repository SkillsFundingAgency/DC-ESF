using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.DataStore;

namespace ESFA.DC.ESF.DataStore
{
    public sealed class StoreClear : IStoreClear
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;

        public StoreClear(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public async Task ClearAsync(int ukPrn, string filename, CancellationToken cancellationToken)
        {
            using (SqlCommand sqlCommand =
                new SqlCommand("[dbo].[DeleteExistingRecords]", _connection, _transaction))
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //sqlCommand.Parameters.Add("@ukprn", SqlDbType.Int).Value = ukPrn;
                //sqlCommand.Parameters.Add("@fileName", SqlDbType.NVarChar).Value = filename;
                sqlCommand.CommandTimeout = 600;
                await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }
}