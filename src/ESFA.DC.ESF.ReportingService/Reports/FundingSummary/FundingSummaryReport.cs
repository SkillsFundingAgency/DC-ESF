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
using ESFA.DC.ESF.Interfaces.Reports;
using ESFA.DC.ESF.Interfaces.Reports.Services;
using ESFA.DC.ESF.Interfaces.Repositories;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR1819.DataStore.EF;
using ESFA.DC.IO.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports.FundingSummary
{
    public class FundingSummaryReport : AbstractReportBuilder, IModelReport
    {
        private readonly IKeyValuePersistenceService _storage;

        private readonly IList<IRowHelper> _rowHelpers;

        private readonly IIlrEsfRepository _repository;

        private readonly IReferenceDataService _referenceDataService;

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
            IIlrEsfRepository repository,
            IList<IRowHelper> rowHelpers,
            IReferenceDataService referenceDataService)
            : base(dateTimeProvider)
        {
            _storage = storage;
            _repository = repository;
            _rowHelpers = rowHelpers;
            _referenceDataService = referenceDataService;
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

            var ilrFileData = _repository.GetFileDetails(ukPrn);

            var reportHeader = PopulateReportHeader(sourceFile, ilrFileData, ukPrn);
            var reportData = PopulateReportData(ukPrn, ilrFileData, data);
            var reportFooter = PopulateReportFooter();

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
            int ukPrn)
        {
            // todo get other years data

            string preparationDate = GetPreparedDateFromFileName(sourceFile.FileName);
            var year = GetFundingYearFromFileName(sourceFile.FileName);
            var secondYear = GetSecondYearFromReportYear(year);

            var header = new FundingHeader
            {
                UKPRN = ukPrn.ToString(),
                SupplementaryDataFile = sourceFile.FileName,
                ContractReferenceNumber = sourceFile.ConRefNumber,
                ProviderName =  _referenceDataService.GetProviderName(ukPrn),
                LastSupplementaryDataFileUpdate = sourceFile.SuppliedDate.ToString(),
                FundingYears = new List<FundingHeader.FundingHeaderYear> // todo get other years data
                {
                    new FundingHeader.FundingHeaderYear
                    {
                        Header = $"{year.ToString()}/{secondYear}",
                        ILRFile = fileData.Filename,
                        LastILRFileUpdate = fileData.SubmittedTime.ToString(),
                        FilePreparationDate = preparationDate != null && DateTime.TryParse(preparationDate, out var prepDate) 
                            ? prepDate.ToShortDateString() : string.Empty
                    }
                }
            };

            return header;
        }

        private FundingFooter PopulateReportFooter()
        {
            return new FundingFooter
            {
                ReportGeneratedAt = DateTime.Now,
                LARSData = _referenceDataService.GetLarsVersion(),
                OrganisationData = _referenceDataService.GetOrganisationVersion(),
                PostcodeData = _referenceDataService.GetPostcodeVersion(),
                ApplicationVersion = string.Empty // todo
            };
        }

        private List<FundingSummaryReportRowModel> PopulateReportData(
            int ukPrn,
            FileDetail ilrFileDetail,
            IList<SupplementaryDataModel> data)
        {
            var reportData = new List<FundingSummaryReportRowModel>();

            var ilrData = _repository.GetPeriodisedValues(ukPrn);
            // todo get other years data

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

            var yearData = reportData.SelectMany(rd => rd.YearlyValues);
            foreach (var model in yearData)
            {
                model.FundingYear = GetFundingYearFromFileName(ilrFileDetail.Filename);
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
            foreach (var rowModel in reportData)
            {
                switch (rowModel.RowType)
                {
                    case RowType.Spacer:
                        writer.NextRecord();
                        break;
                    case RowType.Title:
                        writer.WriteField(rowModel.Title);
                        foreach (var yearlyValues in rowModel.YearlyValues)
                        {
                            for (var i = 0; i < yearlyValues.Values.Length; i++)
                            {
                                writer.WriteField($"{_calendarMonths[i]} {yearlyValues.FundingYear}");
                            }
                        }

                        var years = rowModel.YearlyValues.Count;
                        for (var i = 0; i < rowModel.Totals.Count; i++)
                        {
                            if (i < years)
                            {
                                var reportYear = rowModel.YearlyValues[i].FundingYear;
                                var reportYear2 = GetSecondYearFromReportYear(reportYear);
                                writer.WriteField($"{reportYear.ToString()}/{reportYear2} Subtotal");
                                continue;
                            }

                            writer.WriteField("Grand Total");
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
            writer.WriteField("Report generated at: ");
            writer.WriteField($"{footerData.ReportGeneratedAt.ToShortTimeString()} on {footerData.ReportGeneratedAt.ToShortDateString()}");
        }

        private int GetFundingYearFromFileName(
            string fileName)
        {
            var fileNameParts = fileName.Split('-');
            if (fileNameParts.Length <= 3)
            {
                return 0;
            }

            var constructedYear = $"20{fileName.Split('-')[2].Substring(0, 2)}";

            int.TryParse(constructedYear, out var year);
            return year;
        }

        private string  GetSecondYearFromReportYear(int year)
        {
            return year.ToString().Length > 3 ?  
                (Convert.ToInt32(year.ToString().Substring(year.ToString().Length - 2)) + 1).ToString() : 
                string.Empty;
        }

        private string GetPreparedDateFromFileName(
            string fileName)
        {
            var fileNameParts = fileName.Split('-');
            return fileNameParts.Length <= 4 ? string.Empty : fileNameParts[3];
        }
    }
}