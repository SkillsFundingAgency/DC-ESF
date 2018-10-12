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
using ESFA.DC.ESF.Interfaces.Repositories;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.IO.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports.FundingSummary
{
    public class FundingSummaryReport : AbstractReportBuilder
    {
        private readonly IKeyValuePersistenceService _storage;

        private readonly IList<IRowHelper> _rowHelpers;

        private readonly IIlrEsfRepository _repository;

        public FundingSummaryReport(IDateTimeProvider dateTimeProvider,
            [KeyFilter(PersistenceStorageKeys.Blob)] IKeyValuePersistenceService storage,
            IIlrEsfRepository repository,
            IList<IRowHelper> rowHelpers)
            : base(dateTimeProvider)
        {
            _storage = storage;
            _repository = repository;
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
            var ukPrn = Convert.ToInt32(sourceFile.UKPRN);

            var reportData = PopulateReportData(ukPrn, data);

            using (var ms = new MemoryStream())
            {
                using (TextWriter textWriter = new StreamWriter(ms, utF8Encoding, 1024, true))
                {
                    using (CsvWriter csvWriter = new CsvWriter(textWriter))
                    {
                        GenerateCsv(csvWriter, reportData);
                    }
                }

                csv = Encoding.UTF8.GetString(ms.ToArray());
            }

            var externalFileName = GetExternalFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);
            var fileName = GetFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);

            await _storage.SaveAsync($"{externalFileName}.csv", csv, cancellationToken);
            await WriteZipEntry(archive, $"{fileName}.csv", csv);
        }

        private List<FundingSummaryReportRowModel> PopulateReportData(int ukPrn,
            IList<SupplementaryDataModel> data)
        {
            var reportData = new List<FundingSummaryReportRowModel>();

            var ilrData = _repository.GetPeriodisedValues(ukPrn);

            foreach (var fundingReportRow in ReportDataRows.FundingModelRowDefinitions)
            {
                foreach (var rowHelper in _rowHelpers)
                {
                    if (!rowHelper.IsMatch(fundingReportRow.RowType))
                    {
                        continue;
                    }
                    rowHelper.Execute(reportData, fundingReportRow, data, ilrData);
                    break;
                }
            }

            return reportData;
        }

        private void GenerateCsv(
            CsvWriter writer,
            List<FundingSummaryReportRowModel> reportData)
        {
            // todo write header

            writer.WriteField("European Social Fund 2014-2020");
            writer.NextRecord();
            foreach (var rowModel in reportData)
            {
                switch (rowModel.RowType)
                {
                    case RowType.Spacer:
                        writer.NextRecord();
                        break;
                    case RowType.Title:
                        writer.WriteField(rowModel.Title);
                        writer.NextRecord();
                        break;
                    case RowType.Data:
                    case RowType.Total:
                        writer.WriteField(rowModel.Title);
                        foreach (var yearlyValues in rowModel.YearlyValues)
                        {
                            foreach (var value in yearlyValues.Values)
                            {
                                writer.WriteField(value);
                            }
                        }
                        writer.NextRecord();
                        break;
                }
            }

            // todo write footer
        }
    }
}