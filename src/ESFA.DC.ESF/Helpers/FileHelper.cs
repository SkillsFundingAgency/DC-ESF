using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Utils;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.JobContextManager.Model.Interface;

namespace ESFA.DC.ESF.Helpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IESFProviderService _providerService;

        public FileHelper(IESFProviderService providerService)
        {
            _providerService = providerService;
        }

        public SourceFileModel GetSourceFileData(IJobContextMessage jobContextMessage)
        {
            if (!jobContextMessage.KeyValuePairs.ContainsKey(JobContextMessageKey.Filename))
            {
                throw new ArgumentException($"{nameof(JobContextMessageKey.Filename)} is required");
            }

            var fileName = jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename].ToString();

            string[] fileNameParts = FileNameHelper.SplitFileName(fileName);

            if (fileNameParts.Length != 5)
            {
                throw new ArgumentException($"{nameof(JobContextMessageKey.Filename)} is invalid");
            }

            var fileNameDatePart = FileNameHelper.GetPreparedDateFromFileName(fileName);
            if (!DateTime.TryParse(fileNameDatePart, out var preparationDateTime))
            {
                throw new ArgumentException($"{nameof(JobContextMessageKey.Filename)} is invalid");
            }

            var jobId = jobContextMessage.JobId;

            return new SourceFileModel
            {
                ConRefNumber = fileNameParts[2],
                UKPRN = fileNameParts[1],
                FileName = fileName,
                PreparationDate = preparationDateTime,
                SuppliedDate = jobContextMessage.SubmissionDateTimeUtc,
                JobId = jobId
            };
        }

        public async Task<IList<SupplementaryDataLooseModel>> GetESFRecords(SourceFileModel sourceFileModel, CancellationToken cancellationToken)
        {
            return await _providerService.GetESFRecordsFromFile(sourceFileModel, cancellationToken);
        }
    }
}
