using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Models;
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
            string[] fileNameParts = fileName.Substring(0, fileName.IndexOf('.') - 1).Split('-');

            if (fileNameParts.Length != 4)
            {
                throw new ArgumentException($"{nameof(JobContextMessageKey.Filename)} is invalid");
            }

            if (!DateTime.TryParse(fileNameParts[3], out var preparationDateTime))
            {
                throw new ArgumentException($"{nameof(JobContextMessageKey.Filename)} is invalid");
            }

            return new SourceFileModel
            {
                ConRefNumber = fileNameParts[2],
                UKPRN = fileNameParts[1],
                FileName = fileName,
                PreparationDate = preparationDateTime,
                SuppliedDate = jobContextMessage.SubmissionDateTimeUtc
            };
        }

        public async Task<IList<SupplementaryDataModel>> GetESFRecords(IJobContextMessage jobContextMessage, CancellationToken cancellationToken)
        {
            return await _providerService.GetESFRecordsFromFile(jobContextMessage, cancellationToken);
        }
    }
}
