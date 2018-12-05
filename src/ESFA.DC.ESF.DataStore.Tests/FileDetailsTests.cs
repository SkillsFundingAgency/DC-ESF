using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.DataStore.Tests.Builders;
using Xunit;
using Xunit.Abstractions;

namespace ESFA.DC.ESF.DataStore.Tests
{
    public class FileDetailsTests
    {
        private readonly ITestOutputHelper _output;

        public FileDetailsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task TestSourceFileSave()
        {
            var store = new StoreFileDetails();
            CancellationToken cancellationToken = default(CancellationToken);

            var model = SourceFileModelBuilder.BuildSourceFileModel();

            using (SqlConnection connection =
                new SqlConnection(ConfigurationManager.AppSettings["TestConnectionString"]))
            {
                SqlTransaction transaction = null;
                try
                {
                    await connection.OpenAsync(cancellationToken);
                    transaction = connection.BeginTransaction();

                    await store.StoreAsync(connection, transaction, cancellationToken, model);

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
                    new SqlCommand($"SELECT ConRefNumber FROM dbo.SourceFile Where UKPRN = {model.UKPRN}", connection))
                {
                    Assert.Equal(model.ConRefNumber, sqlCommand.ExecuteScalar());
                }
            }
        }
    }
}
