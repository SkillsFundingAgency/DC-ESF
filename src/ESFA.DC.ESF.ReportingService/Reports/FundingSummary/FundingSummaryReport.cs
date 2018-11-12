using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Cells;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces.Config;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Reports;
using ESFA.DC.ESF.Interfaces.Reports.Services;
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
        private readonly IReferenceDataCache _referenceDataCache;
        private readonly IExcelStyleProvider _excelStyleProvider;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IVersionInfo _versionInfo;
        private readonly ISupplementaryDataService _supplementaryDataService;

        private readonly List<FundingSummaryModel> _fundingSummaryModels;
        private readonly ModelProperty[] _cachedModelProperties;
        private readonly FundingSummaryMapper _fundingSummaryMapper;

        private IList<SourceFileModel> _sourceFiles;
        private Dictionary<int, IList<SupplementaryDataModel>> _contractSupplementaryDataModels;
        private object[] _cachedHeaders;
        private CellStyle[] _cellStyles;
        private int _reportWidth = 1;

        public FundingSummaryReport(
            IDateTimeProvider dateTimeProvider,
            IValueProvider valueProvider,
            IStreamableKeyValuePersistenceService storage,
            IFM70Repository repository,
            ISupplementaryDataService supplementaryDataService,
            IList<IRowHelper> rowHelpers,
            IReferenceDataCache referenceDataCache,
            IExcelStyleProvider excelStyleProvider,
            IVersionInfo versionInfo)
            : base(dateTimeProvider, valueProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _storage = storage;
            _repository = repository;
            _rowHelpers = rowHelpers;
            _referenceDataCache = referenceDataCache;
            _excelStyleProvider = excelStyleProvider;
            _versionInfo = versionInfo;
            _supplementaryDataService = supplementaryDataService;

            ReportFileName = "ESF Funding Summary Report";
            _fundingSummaryModels = new List<FundingSummaryModel>();
            _fundingSummaryMapper = new FundingSummaryMapper();
            _cachedModelProperties = _fundingSummaryMapper.MemberMaps.OrderBy(x => x.Data.Index).Select(x => new ModelProperty(x.Data.Names.Names.ToArray(), (PropertyInfo)x.Data.Member)).ToArray();
        }

        public async Task GenerateReport(
            SupplementaryDataWrapper supplementaryDataWrapper,
            SourceFileModel sourceFile,
            ZipArchive archive,
            CancellationToken cancellationToken)
        {
            var ukPrn = Convert.ToInt32(sourceFile.UKPRN);
            FileDetail ilrFileData = await _repository.GetFileDetails(ukPrn, cancellationToken);

            await GetDataForPreviousContractImports(sourceFile.UKPRN, cancellationToken);
            _contractSupplementaryDataModels.Add(sourceFile.SourceFileId, supplementaryDataWrapper.SupplementaryDataModels);

            FundingSummaryHeaderModel fundingSummaryHeaderModel =
                PopulateReportHeader(sourceFile, ilrFileData, ukPrn, cancellationToken);

            var workbook = new Workbook();
            workbook.Worksheets.Clear();
            foreach (var file in _sourceFiles)
            {
                await PopulateReportData(ukPrn, ilrFileData, _contractSupplementaryDataModels[file.SourceFileId], cancellationToken);
                FundingSummaryFooterModel fundingSummaryFooterModel = PopulateReportFooter(cancellationToken);

                FundingSummaryModel rowOfData = _fundingSummaryModels.FirstOrDefault(x => x.DeliverableCode == "ST01");
                List<YearAndDataLengthModel> yearAndDataLengthModels = new List<YearAndDataLengthModel>();
                if (rowOfData != null)
                {
                    int valCount = rowOfData.YearlyValues.Sum(x => x.Values.Length);
                    _reportWidth = valCount + rowOfData.Totals.Count + 2;
                    foreach (FundingSummaryReportYearlyValueModel fundingSummaryReportYearlyValueModel in rowOfData
                        .YearlyValues)
                    {
                        yearAndDataLengthModels.Add(new YearAndDataLengthModel(
                            fundingSummaryReportYearlyValueModel.FundingYear,
                            fundingSummaryReportYearlyValueModel.Values.Length));
                    }
                }

                _cachedHeaders = GetHeaderEntries(yearAndDataLengthModels);
                _cellStyles = _excelStyleProvider.GetFundingSummaryStyles(workbook);

                Worksheet sheet = workbook.Worksheets.Add(file.ConRefNumber);
                workbook = GetWorkbookReport(workbook, sheet, fundingSummaryHeaderModel, fundingSummaryFooterModel);
                ApplyAdditionalFormatting(workbook, rowOfData);
            }

            string externalFileName = GetExternalFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);
            string fileName = GetFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Save(ms, SaveFormat.Xlsx);
                await _storage.SaveAsync($"{externalFileName}.xlsx", ms, cancellationToken);
                await WriteZipEntry(archive, $"{fileName}.xlsx", ms, cancellationToken);
            }
        }

        private async Task GetDataForPreviousContractImports(string ukPrn, CancellationToken cancellationToken)
        {
            _sourceFiles = await _supplementaryDataService.GetPreviousContractImportFilesForProvider(ukPrn, cancellationToken) ??
                                           new List<SourceFileModel>();

            _contractSupplementaryDataModels = new Dictionary<int, IList<SupplementaryDataModel>>();
            foreach (var sourceFile in _sourceFiles)
            {
                var supplementaryData =
                    await _supplementaryDataService.GetPreviousContractDataForProvider(
                        sourceFile.SourceFileId,
                        cancellationToken);

                if (supplementaryData == null)
                {
                    continue;
                }

                _contractSupplementaryDataModels.Add(sourceFile.SourceFileId, supplementaryData);
            }
        }

        private void ApplyAdditionalFormatting(Workbook workbook, FundingSummaryModel rowOfData)
        {
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Cells.CreateRange(1, 5, 4, 1).ApplyStyle(_cellStyles[6].Style, _cellStyles[6].StyleFlag); // Header
            if (rowOfData != null)
            {
                int valCount = rowOfData.YearlyValues.Sum(x => x.Values.Length);
                int nonYearCount = rowOfData.YearlyValues.Sum(x => x.FundingYear != 2018 ? x.Values.Length : 0);
                int yearCount = rowOfData.YearlyValues.Sum(x => x.FundingYear == 2018 ? x.Values.Length : 0);
                worksheet.Cells.CreateRange(9, nonYearCount + 1, 110, yearCount).ApplyStyle(_cellStyles[6].Style, _cellStyles[6].StyleFlag); // Current Year
                worksheet.Cells.CreateRange(9, valCount + rowOfData.Totals.Count, 110, 1).ApplyStyle(_cellStyles[6].Style, _cellStyles[6].StyleFlag); // Current Year Subtotal
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
                ProviderName = _referenceDataCache.GetProviderName(ukPrn, cancellationToken),
                Ukprn = new[] { ukPrn.ToString(), string.Empty, string.Empty, string.Empty, "2015/16", string.Empty, "2016/17", string.Empty, "2017/18", string.Empty, "2018/19" },
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
                LarsData = _referenceDataCache.GetLarsVersion(cancellationToken),
                OrganisationData = _referenceDataCache.GetOrganisationVersion(cancellationToken),
                PostcodeData = _referenceDataCache.GetPostcodeVersion(cancellationToken),
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

                    rowHelper.Execute(_fundingSummaryModels, fundingReportRow, data, ilrData);
                    break;
                }
            }

            var fundingYear = FileNameHelper.GetFundingYearFromILRFileName(ilrFileDetail?.Filename);
            if (fundingYear == default(int))
            {
                return;
            }

            var yearData = _fundingSummaryModels.SelectMany(rd => rd.YearlyValues);
            foreach (var model in yearData)
            {
                model.FundingYear = fundingYear;
            }
        }

        private Workbook GetWorkbookReport(
            Workbook workbook,
            Worksheet sheet,
            FundingSummaryHeaderModel fundingSummaryHeaderModel,
            FundingSummaryFooterModel fundingSummaryFooterModel)
        {
            WriteExcelRecords(sheet, new FundingSummaryHeaderMapper(), new List<FundingSummaryHeaderModel> { fundingSummaryHeaderModel }, _cellStyles[5], _cellStyles[5], true);
            foreach (var fundingSummaryModel in _fundingSummaryModels)
            {
                if (string.IsNullOrEmpty(fundingSummaryModel.Title))
                {
                    WriteExcelRecords(sheet);
                    continue;
                }

                CellStyle excelHeaderStyle = _excelStyleProvider.GetCellStyle(_cellStyles, fundingSummaryModel.ExcelHeaderStyle);

                if (fundingSummaryModel.HeaderType == HeaderType.TitleOnly)
                {
                    WriteExcelRecords(sheet, fundingSummaryModel.Title, excelHeaderStyle, _reportWidth);
                    continue;
                }

                if (fundingSummaryModel.HeaderType == HeaderType.All)
                {
                    _fundingSummaryMapper.MemberMaps.Single(x => x.Data.Index == 0).Name(fundingSummaryModel.Title);
                    _cachedHeaders[0] = fundingSummaryModel.Title;
                    WriteExcelRecords(sheet, _fundingSummaryMapper, _cachedHeaders, excelHeaderStyle);
                    continue;
                }

                CellStyle excelRecordStyle = _excelStyleProvider.GetCellStyle(_cellStyles, fundingSummaryModel.ExcelRecordStyle);

                WriteExcelRecords(sheet, _fundingSummaryMapper, _cachedModelProperties, fundingSummaryModel, excelRecordStyle);
            }

            WriteExcelRecords(sheet, new FundingSummaryFooterMapper(), new List<FundingSummaryFooterModel> { fundingSummaryFooterModel }, _cellStyles[5], _cellStyles[5], true);

            return workbook;
        }

        private object[] GetHeaderEntries(List<YearAndDataLengthModel> yearAndDataLengthModels)
        {
            List<object> values = new List<object>();
            foreach (var cachedModelProperty in _cachedModelProperties)
            {
                if (typeof(IEnumerable).IsAssignableFrom(cachedModelProperty.MethodInfo.PropertyType))
                {
                    BuildYears(values, cachedModelProperty.Names, yearAndDataLengthModels);
                    BuildTotals(values, cachedModelProperty.Names, yearAndDataLengthModels);
                }
                else
                {
                    values.Add(cachedModelProperty.Names[0]);
                }
            }

            return values.ToArray();
        }

        private void BuildTotals(
            List<object> values,
            string[] names,
            List<YearAndDataLengthModel> yearAndDataLengthModels)
        {
            for (int i = 2016; i < 2020; i++)
            {
                YearAndDataLengthModel yearAndDataLengthModel = yearAndDataLengthModels.SingleOrDefault(x => x.Year == i);
                if (yearAndDataLengthModel == null)
                {
                    continue;
                }

                int counter = 0;
                foreach (string name in names)
                {
                    if (counter > yearAndDataLengthModel.DataLength)
                    {
                        break;
                    }

                    if (!name.Contains("{Y}"))
                    {
                        return;
                    }

                    values.Add(name.Replace("{Y}", i.ToString()));
                    counter++;
                }
            }
        }

        private void BuildYears(
            List<object> values,
            string[] names,
            List<YearAndDataLengthModel> yearAndDataLengthModels)
        {
            for (int i = 2016; i < 2020; i++)
            {
                YearAndDataLengthModel yearAndDataLengthModel = yearAndDataLengthModels.SingleOrDefault(x => x.Year == i);
                if (yearAndDataLengthModel == null)
                {
                    continue;
                }

                int counter = 0;
                foreach (string name in names)
                {
                    if (counter > yearAndDataLengthModel.DataLength)
                    {
                        break;
                    }

                    if (!name.Contains("{SP}"))
                    {
                        return;
                    }

                    values.Add(name.Replace("{SP}", (i - 1).ToString()).Replace("{SY}", i.ToString().Substring(2)));
                    counter++;
                }
            }
        }
    }
}