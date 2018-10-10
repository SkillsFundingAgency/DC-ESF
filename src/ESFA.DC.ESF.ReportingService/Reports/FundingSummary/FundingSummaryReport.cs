using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using CsvHelper;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.IO.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports.FundingSummary
{
    public class FundingSummaryReport : AbstractReportBuilder
    {
        private readonly IKeyValuePersistenceService _storage;

        private readonly IList<IRowHelper> _rowHelpers;

        public FundingSummaryReport(IDateTimeProvider dateTimeProvider,
            [KeyFilter(PersistenceStorageKeys.Blob)] IKeyValuePersistenceService storage,
            IList<IRowHelper> rowHelpers)
            : base(dateTimeProvider)
        {
            _storage = storage;
            _rowHelpers = rowHelpers;
        }

        public async Task GenerateReport(
            IList<SupplementaryDataModel> data,
            SourceFileModel sourceFile,
            ZipArchive archive,
            CancellationToken cancellationToken)
        {
            string csv;
            var utF8Encoding = new UTF8Encoding(false, true);
            using (var ms = new MemoryStream())
            {
                using (TextWriter textWriter = new StreamWriter(ms, utF8Encoding, 1024, true))
                {
                    using (CsvWriter csvWriter = new CsvWriter(textWriter))
                    {
                        GenerateCsv(csvWriter, data);
                    }
                }

                csv = Encoding.UTF8.GetString(ms.ToArray());
            }

            var externalFileName = GetExternalFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);
            var fileName = GetFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);

            await _storage.SaveAsync($"{externalFileName}.csv", csv, cancellationToken);
            await WriteZipEntry(archive, $"{fileName}.csv", csv);
        }

        private void GenerateCsv(
            CsvWriter writer,
            IList<SupplementaryDataModel> data)
        {
            foreach (var fundingReportRow in ReportDataRows.FundingModelDeliverables)
            {
                foreach (var rowHelper in _rowHelpers)
                {
                    if (!rowHelper.IsMatch(fundingReportRow.RowType))
                    {
                        continue;
                    }
                    rowHelper.Execute(writer, fundingReportRow, data);
                    break;
                }
            }
        }
    }
}