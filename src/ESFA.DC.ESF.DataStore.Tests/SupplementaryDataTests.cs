using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.DataStore.Tests.Builders;
using ESFA.DC.ESF.Models;
using Xunit;
using Xunit.Abstractions;

namespace ESFA.DC.ESF.DataStore.Tests
{
    public class SupplementaryDataTests
    {
        private readonly ITestOutputHelper _output;

        public SupplementaryDataTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task TestSupplementaryDataSave()
        {
            var store = new StoreESF();
            var cancellationToken = default(CancellationToken);
            const int fileId = 1;
            var model = new List<SupplementaryDataModel> {SupplementaryDataModelBuilder.BuildSupplementaryData()};

            using (SqlConnection connection =
                new SqlConnection(ConfigurationManager.AppSettings["TestConnectionString"]))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync(cancellationToken);
                    transaction = connection.BeginTransaction();

                    await store.StoreAsync(connection, transaction, fileId, model, cancellationToken);

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    _output.WriteLine(ex.ToString());

                    try
                    {
                        transaction?.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        _output.WriteLine(ex2.ToString());
                    }

                    Assert.True(false);
                }

                using (SqlCommand sqlCommand =
                    new SqlCommand($"SELECT Count(1) FROM dbo.SupplementaryData Where ConRefNumber = '{model[0].ConRefNumber}'", connection))
                {
                    Assert.Equal(model.Count, sqlCommand.ExecuteScalar());
                }
            }
        }
    }
}
