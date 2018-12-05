using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastMember;

namespace ESFA.DC.ESF.DataStore
{
    public sealed class BulkInsert : IDisposable
    {
        private readonly CancellationToken _cancellationToken;

        private readonly SqlBulkCopy _sqlBulkCopy;

        public BulkInsert(SqlConnection connection, SqlTransaction sqlTransaction, CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, sqlTransaction)
            {
                BatchSize = 5_000, // https://stackoverflow.com/questions/779690/what-is-the-recommended-batch-size-for-sqlbulkcopy
                BulkCopyTimeout = 600
            };
        }

        public async Task Insert<T>(string table, IList<T> source)
        {
            try
            {
                if (!source.Any())
                {
                    return;
                }

                if (_cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                _sqlBulkCopy.ColumnMappings.Clear();
                using (ObjectReader reader = ObjectReader.Create(source))
                {
                    _sqlBulkCopy.DestinationTableName = table;
                    DataTable schema = reader.GetSchemaTable();
                    if (schema == null)
                    {
                        return;
                    }

                    for (int i = 0; i < schema.Rows.Count; i++)
                    {
                        _sqlBulkCopy.ColumnMappings.Add(schema.Rows[i].ItemArray[1].ToString(), schema.Rows[i].ItemArray[1].ToString());
                    }

                    await _sqlBulkCopy.WriteToServerAsync(reader, _cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(table);
                Console.Write(ex);
                throw;
            }
        }

        public void Dispose()
        {
            ((IDisposable)_sqlBulkCopy)?.Dispose();
        }
    }
}
