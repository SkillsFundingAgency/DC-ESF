using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Database.EF;
using ESFA.DC.ESF.Interfaces.DataStore;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.ESF.DataStore
{
    public sealed class StoreFileDetails : IStoreFileDetails
    {
        private SqlConnection _sqlConnection;

        private SqlTransaction _sqlTransaction;

        private readonly IJobContextMessage _jobContextMessage;

        public StoreFileDetails(IJobContextMessage jobContextMessage)
        {
            _jobContextMessage = jobContextMessage;
        }

        public async Task<int> StoreAsync(SqlConnection sqlConnection, SqlTransaction sqlTransaction, CancellationToken cancellationToken)
        {
            _sqlConnection = sqlConnection;
            _sqlTransaction = sqlTransaction;

            GetAndCheckValues(out var ukPrn, out var conRefNumber, out var submissionDateTime, out var preparationDateTime);
            return await StoreAsync(ukPrn, conRefNumber, submissionDateTime, preparationDateTime, cancellationToken);
        }

        private void GetAndCheckValues(out int ukPrn, out string conRefNumber, out DateTime submissionDateTime, out DateTime preparationDateTime)
        {
            if (!_jobContextMessage.KeyValuePairs.ContainsKey(JobContextMessageKey.UkPrn) 
                || !int.TryParse(_jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn].ToString(), out ukPrn))
            {
                throw new ArgumentException($"{nameof(JobContextMessageKey.UkPrn)} is expected to be a number");
            }

            if (!_jobContextMessage.KeyValuePairs.ContainsKey(JobContextMessageKey.Filename))
            {
                throw new ArgumentException($"{nameof(JobContextMessageKey.Filename)} is required");
            }

            var fileName = _jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString();
            string[] fileNameParts = fileName.Substring(0, fileName.IndexOf('.') -1).Split('-');

            if (fileNameParts.Length != 4)
            {
                throw new ArgumentException($"{nameof(JobContextMessageKey.Filename)} is invalid");
            }

            conRefNumber = fileNameParts[2];

            if (!DateTime.TryParse(fileNameParts[3], out preparationDateTime))
            {
                throw new ArgumentException($"{nameof(JobContextMessageKey.Filename)} is invalid");
            }

            submissionDateTime = _jobContextMessage.SubmissionDateTimeUtc;
        }

        private async Task<int> StoreAsync(
            int ukPrn,
            string conRefNumber,
            DateTime submissionDateTime,
            DateTime preparationDateTime,
            CancellationToken cancellationToken)
        {
            SourceFile fileDetails = new SourceFile
            {
                ConRefNumber = conRefNumber,
                UKPRN = ukPrn.ToString(),
                FileName = Path.GetFileName(_jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString()),
                DateTime = submissionDateTime,
                FilePreparationDate = preparationDateTime
            };

            string insertFileDetails =
                    $"INSERT INTO [dbo].[SourceFile] ([ConRefNumber], [UKPRN], [Filename], [DateTime], [FilePreparationDate]) output INSERTED.ID VALUES ({fileDetails.ConRefNumber}, '{fileDetails.UKPRN}', {fileDetails.FileName}, {fileDetails.DateTime}, {fileDetails.FilePreparationDate})";

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
