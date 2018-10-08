﻿using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Reports;
using ESFA.DC.ESF.Models;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Jobs.Model;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports
{
    public class ValidationResultReport : AbstractReportBuilder, IValidationResultReport
    {
        private readonly IKeyValuePersistenceService _storage;
        private readonly IJsonSerializationService _jsonSerializationService;

        public ValidationResultReport(IDateTimeProvider dateTimeProvider,
            IJsonSerializationService jsonSerializationService,
            [KeyFilter(PersistenceStorageKeys.Blob)] IKeyValuePersistenceService storage) 
            : base(dateTimeProvider)
        {
            ReportFileName = "ESF Supplementary Data Rule Violation Report";

            _jsonSerializationService = jsonSerializationService;
            _storage = storage;
        }

        public async Task GenerateReport(
            IList<SupplementaryDataModel> data,
            SourceFileModel sourceFile,
            IList<ValidationErrorModel> validationErrors,
            ZipArchive archive,
            CancellationToken cancellationToken)
        {
            var report = GetValidationReport(data, validationErrors);

            var fileName = GetExternalFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);

            await SaveJson(fileName, report, cancellationToken);
        }

        private FileValidationResult GetValidationReport(
            IList<SupplementaryDataModel> data,
            IList<ValidationErrorModel> validationErrors)
        {
            var errors = validationErrors.Where(x => !x.IsWarning).ToList();
            var warnings = validationErrors.Where(x => x.IsWarning).ToList();

            return new FileValidationResult
            {
                TotalLearners = data.GroupBy(w => w.ULN).Count(),
                TotalErrors = errors.Count,
                TotalWarnings = warnings.Count,
                TotalWarningLearners = warnings.GroupBy(w => w.ULN).Count(),
                TotalErrorLearners = errors.GroupBy(e => e.ULN).Count(),
                ErrorMessage = validationErrors.FirstOrDefault(x => string.IsNullOrEmpty(x.ConRefNumber))?.ErrorMessage
            };
        }

        private async Task SaveJson(string fileName, FileValidationResult result, CancellationToken cancellationToken)
        {
            await _storage.SaveAsync($"{fileName}.json", _jsonSerializationService.Serialize(result), cancellationToken);
        }
    }
}