using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Cells;
using Autofac.Features.AttributeFilters;
using CsvHelper;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Config;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Reports;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Generation;
using ESFA.DC.ESF.Models.Reports;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ESF.Models.Styling;
using ESFA.DC.ESF.ReportingService.Mappers;
using ESFA.DC.ESF.Utils;
using ESFA.DC.ILR1819.DataStore.EF;
using ESFA.DC.IO.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports.FundingSummary
{
    public class FundingSummaryReport : AbstractReportBuilder, IModelReport
    {
        private readonly IStreamableKeyValuePersistenceService _storage;
        private readonly IList<IRowHelper> _rowHelpers;
        private readonly IFM70Repository _repository;
        private readonly IReferenceDataRepository _referenceRepository;
        private readonly IExcelStyleProvider _excelStyleProvider;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IVersionInfo _versionInfo;

        private readonly List<FundingSummaryModel> fundingSummaryModels;
        private readonly ModelProperty[] _cachedModelProperties;
        private readonly FundingSummaryMapper _fundingSummaryMapper;
        private readonly object[] _cachedHeaders;

        public FundingSummaryReport(
            IDateTimeProvider dateTimeProvider,
            IValueProvider valueProvider,
            [KeyFilter(PersistenceStorageKeys.Blob)] IStreamableKeyValuePersistenceService storage,
            IFM70Repository repository,
            IList<IRowHelper> rowHelpers,
            IReferenceDataRepository referenceDataRepository,
            IExcelStyleProvider excelStyleProvider,
            IVersionInfo versionInfo)
            : base(dateTimeProvider, valueProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _storage = storage;
            _repository = repository;
            _rowHelpers = rowHelpers;
            _referenceRepository = referenceDataRepository;
            _excelStyleProvider = excelStyleProvider;
            _versionInfo = versionInfo;

            ReportFileName = "ESF Funding Summary Report";
            fundingSummaryModels = new List<FundingSummaryModel>();
            _fundingSummaryMapper = new FundingSummaryMapper();
            _cachedModelProperties = _fundingSummaryMapper.MemberMaps.OrderBy(x => x.Data.Index).Select(x => new ModelProperty(x.Data.Names.Names.ToArray(), (PropertyInfo)x.Data.Member)).ToArray();
            _cachedHeaders = GetHeaderEntries();
        }

        public async Task GenerateReport(
            SupplementaryDataWrapper supplementaryDataWrapper,
            SourceFileModel sourceFile,
            ZipArchive archive,
            CancellationToken cancellationToken)
        {
            int ukPrn = Convert.ToInt32(sourceFile.UKPRN);
            FileDetail ilrFileData = await _repository.GetFileDetails(ukPrn, cancellationToken);

            FundingSummaryHeaderModel fundingSummaryHeaderModel = PopulateReportHeader(sourceFile, ilrFileData, ukPrn, cancellationToken);
            await PopulateReportData(ukPrn, ilrFileData, supplementaryDataWrapper.SupplementaryDataModels, cancellationToken);
            FundingSummaryFooterModel fundingSummaryFooterModel = PopulateReportFooter(cancellationToken);

            string csv = GetReportCsv(fundingSummaryHeaderModel, fundingSummaryFooterModel);

            string externalFileName = GetExternalFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);
            string fileName = GetFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);

            await _storage.SaveAsync($"{externalFileName}.csv", csv, cancellationToken);
            await WriteZipEntry(archive, $"{fileName}.csv", csv);

            Workbook workbook = GetWorkbookReport(fundingSummaryHeaderModel, fundingSummaryFooterModel);

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Save(ms, SaveFormat.Xlsx);
                await _storage.SaveAsync($"{externalFileName}.xlsx", ms, cancellationToken);
                await WriteZipEntry(archive, $"{fileName}.xlsx", ms, cancellationToken);
            }
        }

        private FundingSummaryHeaderModel PopulateReportHeader(
            SourceFileModel sourceFile,
            FileDetail fileData,
            int ukPrn,
            CancellationToken cancellationToken)
        {
            // Todo: get other years data
            string preparationDate = FileNameHelper.GetPreparedDateFromFileName(sourceFile.FileName);
            int year = FileNameHelper.GetFundingYearFromFileName(sourceFile.FileName);
            string secondYear = FileNameHelper.GetSecondYearFromReportYear(year);

            var header = new FundingSummaryHeaderModel
            {
                ProviderName = _referenceRepository.GetProviderName(ukPrn, cancellationToken),
                Ukprn = new[] { ukPrn.ToString(), string.Empty, string.Empty, string.Empty, "2015/16", "2016/17", "2017/18", "2018/19" },
                ContractReferenceNumber = new[] { sourceFile.ConRefNumber, string.Empty, string.Empty, "ILR File :" },
                SupplementaryDataFile = new[] { sourceFile.FileName, string.Empty, string.Empty, "Last ILR File Update :" },
                LastSupplementaryDataFileUpdate = new[] { sourceFile.SuppliedDate?.ToString("dd/MM/yyyy"), string.Empty, string.Empty, "File Preparation Date :" }
                //FundingYears = new List<FundingSummaryHeaderModel.FundingHeaderYear>
                //{
                //    new FundingSummaryHeaderModel.FundingHeaderYear
                //    {
                //        Header = $"{year.ToString()}/{secondYear}",
                //        ILRFile = fileData?.Filename,
                //        LastILRFileUpdate = fileData?.SubmittedTime?.ToString("dd/MM/yyyy"),
                //        FilePreparationDate = preparationDate != null && DateTime.TryParse(preparationDate, out var prepDate)
                //            ? prepDate.ToShortDateString() : string.Empty
                //    }
                //}
            };

            return header;
        }

        private FundingSummaryFooterModel PopulateReportFooter(CancellationToken cancellationToken)
        {
            var dateTimeNowUtc = _dateTimeProvider.GetNowUtc();
            var dateTimeNowUk = _dateTimeProvider.ConvertUtcToUk(dateTimeNowUtc);

            return new FundingSummaryFooterModel
            {
                ReportGeneratedAt = "Report generated at " + dateTimeNowUk.ToString("HH:mm:ss") + " on " + dateTimeNowUk.ToString("dd/MM/yyyy"),
                LarsData = _referenceRepository.GetLarsVersion(cancellationToken),
                OrganisationData = _referenceRepository.GetOrganisationVersion(cancellationToken),
                PostcodeData = _referenceRepository.GetPostcodeVersion(cancellationToken),
                ApplicationVersion = _versionInfo.ServiceReleaseVersion
            };
        }

        private async Task PopulateReportData(
            int ukPrn,
            FileDetail ilrFileDetail,
            IList<SupplementaryDataModel> data,
            CancellationToken cancellationToken)
        {
            IList<ESF_LearningDeliveryDeliverable_PeriodisedValues> ilrData = await _repository.GetPeriodisedValues(ukPrn, cancellationToken);
            // Todo: Get other years data

            foreach (var fundingReportRow in ReportDataTemplate.FundingModelRowDefinitions)
            {
                foreach (var rowHelper in _rowHelpers)
                {
                    if (!rowHelper.IsMatch(fundingReportRow.RowType))
                    {
                        continue;
                    }

                    rowHelper.Execute(fundingSummaryModels, fundingReportRow, data, ilrData);
                    break;
                }
            }

            var fundingYear = FileNameHelper.GetFundingYearFromILRFileName(ilrFileDetail?.Filename);
            if (fundingYear == default(int))
            {
                return;
            }

            var yearData = fundingSummaryModels.SelectMany(rd => rd.YearlyValues);
            foreach (var model in yearData)
            {
                model.FundingYear = fundingYear;
            }
        }

        private string GetReportCsv(FundingSummaryHeaderModel fundingSummaryHeaderModel, FundingSummaryFooterModel fundingSummaryFooterModel)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                UTF8Encoding utF8Encoding = new UTF8Encoding(false, true);
                using (TextWriter textWriter = new StreamWriter(ms, utF8Encoding))
                {
                    using (CsvWriter csvWriter = new CsvWriter(textWriter))
                    {
                        WriteCsvRecords<FundingSummaryHeaderMapper, FundingSummaryHeaderModel>(csvWriter, fundingSummaryHeaderModel);
                        foreach (FundingSummaryModel fundingSummaryModel in fundingSummaryModels)
                        {
                            if (string.IsNullOrEmpty(fundingSummaryModel.Title))
                            {
                                WriteCsvRecords(csvWriter);
                                continue;
                            }

                            if (fundingSummaryModel.HeaderType == HeaderType.TitleOnly)
                            {
                                WriteCsvRecords(csvWriter, (object)fundingSummaryModel.Title);
                                continue;
                            }

                            if (fundingSummaryModel.HeaderType == HeaderType.All)
                            {
                                _fundingSummaryMapper.MemberMaps.Single(x => x.Data.Index == 0).Name(fundingSummaryModel.Title);
                                _cachedHeaders[0] = fundingSummaryModel.Title;
                                WriteCsvRecords(csvWriter, _cachedHeaders);
                                continue;
                            }

                            WriteCsvRecords(csvWriter, _fundingSummaryMapper, _cachedModelProperties, fundingSummaryModel);
                        }

                        WriteCsvRecords<FundingSummaryFooterMapper, FundingSummaryFooterModel>(csvWriter, fundingSummaryFooterModel);

                        csvWriter.Flush();
                        textWriter.Flush();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }

        private Workbook GetWorkbookReport(
            FundingSummaryHeaderModel fundingSummaryHeaderModel,
            FundingSummaryFooterModel fundingSummaryFooterModel)
        {
            Workbook workbook = new Workbook();
            CellStyle[] cellStyles = _excelStyleProvider.GetFundingSummaryStyles(workbook);
            Worksheet sheet = workbook.Worksheets[0];

            WriteExcelRecords(sheet, new FundingSummaryHeaderMapper(), new List<FundingSummaryHeaderModel> { fundingSummaryHeaderModel }, cellStyles[5], cellStyles[5], true);
            foreach (FundingSummaryModel fundingSummaryModel in fundingSummaryModels)
            {
                if (string.IsNullOrEmpty(fundingSummaryModel.Title))
                {
                    WriteExcelRecords(sheet);
                    continue;
                }

                CellStyle excelHeaderStyle = _excelStyleProvider.GetCellStyle(cellStyles, fundingSummaryModel.ExcelHeaderStyle);

                if (fundingSummaryModel.HeaderType == HeaderType.TitleOnly)
                {
                    WriteExcelRecords(sheet, fundingSummaryModel.Title, excelHeaderStyle, 17);
                    continue;
                }

                if (fundingSummaryModel.HeaderType == HeaderType.All)
                {
                    _fundingSummaryMapper.MemberMaps.Single(x => x.Data.Index == 0).Name(fundingSummaryModel.Title);
                    _cachedHeaders[0] = fundingSummaryModel.Title;
                    WriteExcelRecords(sheet, _fundingSummaryMapper, _cachedHeaders, excelHeaderStyle);
                    continue;
                }

                CellStyle excelRecordStyle = _excelStyleProvider.GetCellStyle(cellStyles, fundingSummaryModel.ExcelRecordStyle);

                WriteExcelRecords(sheet, _fundingSummaryMapper, _cachedModelProperties, fundingSummaryModel, excelRecordStyle);
            }

            WriteExcelRecords(sheet, new FundingSummaryFooterMapper(), new List<FundingSummaryFooterModel> { fundingSummaryFooterModel }, cellStyles[5], cellStyles[5], true);

            return workbook;
        }

        private object[] GetHeaderEntries()
        {
            List<object> values = new List<object>();
            foreach (ModelProperty cachedModelProperty in _cachedModelProperties)
            {
                if (typeof(IEnumerable).IsAssignableFrom(cachedModelProperty.MethodInfo.PropertyType))
                {
                    BuildYears(values, cachedModelProperty.Names);
                    BuildTotals(values, cachedModelProperty.Names);
                }
                else
                {
                    values.Add(cachedModelProperty.Names[0]);
                }
            }

            return values.ToArray();
        }

        private void BuildTotals(List<object> values, string[] names)
        {
            for (int i = 2016; i < 2020; i++)
            {
                foreach (string name in names)
                {
                    if (!name.Contains("{Y}"))
                    {
                        return;
                    }

                    values.Add(name.Replace("{Y}", i.ToString()));
                }
            }
        }

        private void BuildYears(List<object> values, string[] names)
        {
            for (int i = 2016; i < 2020; i++)
            {
                foreach (string name in names)
                {
                    if (!name.Contains("{SP}"))
                    {
                        return;
                    }

                    values.Add(name.Replace("{SP}", (i - 1).ToString()).Replace("{SY}", i.ToString().Substring(2)));
                }
            }
        }
    }
}