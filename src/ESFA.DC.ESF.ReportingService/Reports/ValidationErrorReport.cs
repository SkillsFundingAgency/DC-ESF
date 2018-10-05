using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ReportingService.Mappers;
using ESFA.DC.IO.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports
{
    public class ValidationErrorReport : AbstractReportBuilder
    {
        private readonly IKeyValuePersistenceService _storage;

        public ValidationErrorReport(IDateTimeProvider dateTimeProvider,
                [KeyFilter(PersistenceStorageKeys.Blob)] IKeyValuePersistenceService storage)
            : base(dateTimeProvider)
        {

            ReportFileName = "ESF Supplementary Data Rule Violation Report";

            _storage = storage;
        }

        public async Task GenerateReport(
            IList<SupplementaryDataModel> data,
            SourceFileModel sourceFile,
            IList<ValidationErrorModel> validationErrors,
            ZipArchive archive,
            CancellationToken cancellationToken)
        {
            var csv = GetCsv(validationErrors);

            var externalFileName = GetExternalFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);
            var fileName = GetFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);

            await _storage.SaveAsync($"{externalFileName}.csv", csv, cancellationToken);
            await WriteZipEntry(archive, $"{fileName}.csv", csv);
        }

        private string GetCsv(IList<ValidationErrorModel> validationErrorModels)
        {
            using (var ms = new MemoryStream())
            {
                BuildCsvReport<ValidationErrorMapper, ValidationErrorModel>(ms, validationErrorModels);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
