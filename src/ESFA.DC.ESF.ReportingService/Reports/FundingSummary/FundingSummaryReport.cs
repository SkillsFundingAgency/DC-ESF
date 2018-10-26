using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using CsvHelper;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Config;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Reports;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ESF.Utils;
using ESFA.DC.ILR1819.DataStore.EF;
using ESFA.DC.IO.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports.FundingSummary
{
    public class FundingSummaryReport : AbstractReportBuilder, IModelReport
    {
        private readonly IKeyValuePersistenceService _storage;

        private readonly IList<IRowHelper> _rowHelpers;

        private readonly IFM70Repository _repository;

        private readonly IReferenceDataRepository _referenceRepository;

        private readonly IVersionInfo _versionInfo;

        private readonly string[] _calendarMonths = {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        };

        public FundingSummaryReport(IDateTimeProvider dateTimeProvider,
            [KeyFilter(PersistenceStorageKeys.Blob)] IKeyValuePersistenceService storage,
            IFM70Repository repository,
            IList<IRowHelper> rowHelpers,
            IReferenceDataRepository referenceDataRepository,
            IVersionInfo versionInfo)
            : base(dateTimeProvider)
        {
            _storage = storage;
            _repository = repository;
            _rowHelpers = rowHelpers;
            _referenceRepository = referenceDataRepository;
            _versionInfo = versionInfo;

            ReportFileName = "ESF Funding Summary Report";
        }

        public async Task GenerateReport(
            SupplementaryDataWrapper wrapper,
            SourceFileModel sourceFile,
            ZipArchive archive,
            CancellationToken cancellationToken)
        {
            string csv;
            var utF8Encoding = new UTF8Encoding(false, true);
            var ukPrn = Convert.ToInt32(sourceFile.UKPRN);

            var ilrFileData = await _repository.GetFileDetails(ukPrn, cancellationToken);

            var reportHeader = PopulateReportHeader(sourceFile, ilrFileData, ukPrn, cancellationToken);
            var reportData = await PopulateReportData(ukPrn, ilrFileData, wrapper.SupplementaryDataModels, cancellationToken);
            var reportFooter = PopulateReportFooter(cancellationToken);

            using (var ms = new MemoryStream())
            {
                using (TextWriter textWriter = new StreamWriter(ms, utF8Encoding, 1024, true))
                {
                    using (var csvWriter = new CsvWriter(textWriter))
                    {
                        GenerateCsv(csvWriter, reportHeader, reportData, reportFooter);
                    }
                }

                csv = Encoding.UTF8.GetString(ms.ToArray());
            }

            var externalFileName = GetExternalFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);
            var fileName = GetFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);

            await _storage.SaveAsync($"{externalFileName}.csv", csv, cancellationToken);
            await WriteZipEntry(archive, $"{fileName}.csv", csv);
        }

        private FundingHeader PopulateReportHeader(
            SourceFileModel sourceFile,
            FileDetail fileData,
            int ukPrn,
            CancellationToken cancellationToken)
        {
            // todo get other years data

            string preparationDate = FileNameHelper.GetPreparedDateFromFileName(sourceFile.FileName);
            var year = FileNameHelper.GetFundingYearFromFileName(sourceFile.FileName);
            var secondYear = FileNameHelper.GetSecondYearFromReportYear(year);

            var header = new FundingHeader
            {
                UKPRN = ukPrn.ToString(),
                SupplementaryDataFile = sourceFile.FileName,
                ContractReferenceNumber = sourceFile.ConRefNumber,
                ProviderName =  _referenceRepository.GetProviderName(ukPrn, cancellationToken),
                LastSupplementaryDataFileUpdate = sourceFile.SuppliedDate.ToString(),
                FundingYears = new List<FundingHeader.FundingHeaderYear>
                {
                    new FundingHeader.FundingHeaderYear
                    {
                        Header = $"{year.ToString()}/{secondYear}",
                        ILRFile = fileData?.Filename,
                        LastILRFileUpdate = fileData?.SubmittedTime.ToString(),
                        FilePreparationDate = preparationDate != null && DateTime.TryParse(preparationDate, out var prepDate) 
                            ? prepDate.ToShortDateString() : string.Empty
                    }
                }
            };

            return header;
        }

        private FundingFooter PopulateReportFooter(CancellationToken cancellationToken)
        {
            return new FundingFooter
            {
                ReportGeneratedAt = DateTime.Now,
                LARSData = _referenceRepository.GetLarsVersion(cancellationToken),
                OrganisationData = _referenceRepository.GetOrganisationVersion(cancellationToken),
                PostcodeData = _referenceRepository.GetPostcodeVersion(cancellationToken),
                ApplicationVersion = _versionInfo.ServiceReleaseVersion
            };
        }

        private async Task<List<FundingSummaryReportRowModel>> PopulateReportData(
            int ukPrn,
            FileDetail ilrFileDetail,
            IList<SupplementaryDataModel> data,
            CancellationToken cancellationToken)
        {
            var reportData = new List<FundingSummaryReportRowModel>();

            var ilrData = await _repository.GetPeriodisedValues(ukPrn, cancellationToken);
            // todo get other years data

            foreach (var fundingReportRow in ReportDataTemplate.FundingModelRowDefinitions)
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

            var fundingYear = FileNameHelper.GetFundingYearFromILRFileName(ilrFileDetail?.Filename);
            var yearData = reportData.SelectMany(rd => rd.YearlyValues);
            foreach (var model in yearData)
            {
                model.FundingYear = fundingYear;
            }

            return reportData;
        }

        private void GenerateCsv(
            CsvWriter writer,
            FundingHeader headerData,
            List<FundingSummaryReportRowModel> reportData,
            FundingFooter footerData)
        {
            // report header
            writer.WriteField("Provider Name : ");
            writer.WriteField(headerData.ProviderName);
            writer.NextRecord();

            writer.WriteField("UKPRN : ");
            writer.WriteField(headerData.UKPRN);
            writer.WriteField(string.Empty);
            writer.WriteField(string.Empty);
            writer.WriteField(string.Empty);
            foreach (var year in headerData.FundingYears)
            {
                writer.WriteField(year.Header);
                writer.WriteField(string.Empty);
            }
            writer.NextRecord();

            writer.WriteField("Contract Reference Number : ");
            writer.WriteField(headerData.ContractReferenceNumber);
            writer.WriteField(string.Empty);
            writer.WriteField(string.Empty);
            writer.WriteField("ILR File : ");
            foreach (var year in headerData.FundingYears)
            {
                writer.WriteField(year.ILRFile);
                writer.WriteField(string.Empty);
            }
            writer.NextRecord();

            writer.WriteField("Supplementary Data File : ");
            writer.WriteField(headerData.SupplementaryDataFile);
            writer.WriteField(string.Empty);
            writer.WriteField(string.Empty);
            writer.WriteField("Last ILR File Update : ");
            foreach (var year in headerData.FundingYears)
            {
                writer.WriteField(year.LastILRFileUpdate);
                writer.WriteField(string.Empty);
            }
            writer.NextRecord();

            writer.WriteField("Last Supplementary Data File Update : ");
            writer.WriteField(headerData.LastSupplementaryDataFileUpdate);
            writer.WriteField(string.Empty);
            writer.WriteField(string.Empty);
            writer.WriteField("File Preparation Date : ");
            foreach (var year in headerData.FundingYears)
            {
                writer.WriteField(year.FilePreparationDate);
                writer.WriteField(string.Empty);
            }
            writer.NextRecord();

            writer.WriteField("Security Classification : ");
            writer.WriteField(headerData.SecurityClassification);
            writer.NextRecord();
            writer.NextRecord();

            // report body
            writer.WriteField("European Social Fund 2014-2020");
            writer.NextRecord();

            var rowWithYearlyData = reportData.FirstOrDefault(r => r.YearlyValues.Count > 0);
            foreach (var rowModel in reportData)
            {
                switch (rowModel.RowType)
                {
                    case RowType.Spacer:
                        writer.NextRecord();
                        break;
                    case RowType.Title:
                        writer.WriteField(rowModel.Title);

                        if (rowWithYearlyData != null)
                        {
                            foreach (var yearlyValues in rowWithYearlyData.YearlyValues)
                            {
                                for (var i = 0; i < yearlyValues.Values.Length; i++)
                                {
                                    writer.WriteField($"{_calendarMonths[i]} {yearlyValues.FundingYear}");
                                }
                            }

                            var years = rowWithYearlyData.YearlyValues.Count;
                            for (var i = 0; i < rowWithYearlyData.Totals.Count; i++)
                            {
                                if (i < years)
                                {
                                    var reportYear = rowWithYearlyData.YearlyValues[i].FundingYear;
                                    var reportYear2 = FileNameHelper.GetSecondYearFromReportYear(reportYear);
                                    writer.WriteField($"{reportYear.ToString()}/{reportYear2} Subtotal");
                                    continue;
                                }

                                writer.WriteField("Grand Total");
                            }
                        }

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
                        foreach (var rowTotal in rowModel.Totals)
                        {
                            writer.WriteField(rowTotal);
                        }
                        writer.NextRecord();
                        break;
                    case RowType.Cumulative:
                        writer.WriteField(rowModel.Title);
                        foreach (var yearlyValues in rowModel.YearlyValues)
                        {
                            foreach (var value in yearlyValues.Values)
                            {
                                writer.WriteField(value);
                            }
                        }
                        foreach (var rowTotal in rowModel.Totals)
                        {
                            writer.WriteField(rowTotal);
                        }
                        writer.WriteField("n/a");
                        writer.NextRecord();
                        break;
                }
            }

            //report footer
            writer.NextRecord();
            writer.WriteField("Application Version : ");
            writer.WriteField(footerData.ApplicationVersion);
            writer.NextRecord();
            writer.WriteField("LARS Data : ");
            writer.WriteField(footerData.LARSData);
            writer.NextRecord();
            writer.WriteField("Postcode Data : ");
            writer.WriteField(footerData.PostcodeData);
            writer.NextRecord();
            writer.WriteField("Organisation Data : ");
            writer.WriteField(footerData.OrganisationData);
            writer.NextRecord();
            writer.WriteField("Report generated at : ");
            writer.WriteField($"{footerData.ReportGeneratedAt.ToShortTimeString()} on {footerData.ReportGeneratedAt.ToShortDateString()}");
        }
    }
}