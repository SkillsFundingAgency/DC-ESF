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
using ESFA.DC.IO.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports.FundingSummary
{
    public class FundingSummaryReport : AbstractReportBuilder, IModelReport
    {
        private readonly IStreamableKeyValuePersistenceService _storage;
        private readonly IList<IRowHelper> _rowHelpers;
        private readonly IILRService _ilrService;
        private readonly IReferenceDataCache _referenceDataCache;
        private readonly IExcelStyleProvider _excelStyleProvider;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IVersionInfo _versionInfo;
        private readonly ISupplementaryDataService _supplementaryDataService;

        private readonly ModelProperty[] _cachedModelProperties;
        private readonly FundingSummaryMapper _fundingSummaryMapper;

        private string[] _cachedHeaders;
        private CellStyle[] _cellStyles;
        private int _reportWidth = 1;

        public FundingSummaryReport(
            IDateTimeProvider dateTimeProvider,
            IValueProvider valueProvider,
            IStreamableKeyValuePersistenceService storage,
            IILRService ilrService,
            ISupplementaryDataService supplementaryDataService,
            IList<IRowHelper> rowHelpers,
            IReferenceDataCache referenceDataCache,
            IExcelStyleProvider excelStyleProvider,
            IVersionInfo versionInfo)
            : base(dateTimeProvider, valueProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _storage = storage;
            _rowHelpers = rowHelpers;
            _referenceDataCache = referenceDataCache;
            _excelStyleProvider = excelStyleProvider;
            _versionInfo = versionInfo;
            _supplementaryDataService = supplementaryDataService;
            _ilrService = ilrService;

            ReportFileName = "ESF Funding Summary Report";
            _fundingSummaryMapper = new FundingSummaryMapper();
            _cachedModelProperties = _fundingSummaryMapper
                .MemberMaps
                .OrderBy(x => x.Data.Index)
                .Select(x => new ModelProperty(x.Data.Names.Names.ToArray(), (PropertyInfo)x.Data.Member))
                .ToArray();
        }

        public async Task GenerateReport(
            SupplementaryDataWrapper supplementaryDataWrapper,
            SourceFileModel sourceFile,
            ZipArchive archive,
            CancellationToken cancellationToken)
        {
            var ukPrn = Convert.ToInt32(sourceFile.UKPRN);

            var sourceFiles = await _supplementaryDataService.GetImportFiles(sourceFile.UKPRN, cancellationToken);
            var supplementaryData =
                await _supplementaryDataService.GetSupplementaryData(sourceFiles, cancellationToken);

            var ilrYearlyFileData = await _ilrService.GetIlrFileDetails(ukPrn, cancellationToken);
            var fm70YearlyData = (await _ilrService.GetYearlyIlrData(ukPrn, cancellationToken)).ToList();

            FundingSummaryHeaderModel fundingSummaryHeaderModel =
                PopulateReportHeader(sourceFile, ilrYearlyFileData, ukPrn, cancellationToken);

            var workbook = new Workbook();
            workbook.Worksheets.Clear();

            foreach (var file in sourceFiles)
            {
                var fundingYear = FileNameHelper.GetFundingYearFromFileName(file.FileName);
                var thisYearsFm70Data = fm70YearlyData.Where(d => d.FundingYear == fundingYear);

                var fundingSummaryModels = PopulateReportData(thisYearsFm70Data, supplementaryData[file.SourceFileId]).ToList();
                ApplyFundingYearToEmptyFundingYears(fundingSummaryModels, fundingYear);

                FundingSummaryFooterModel fundingSummaryFooterModel = PopulateReportFooter(cancellationToken);

                FundingSummaryModel rowOfData = fundingSummaryModels.FirstOrDefault(x => x.DeliverableCode == "ST01" && x.YearlyValues.Any());
                var yearAndDataLengthModels = new List<YearAndDataLengthModel>();
                if (rowOfData != null)
                {
                    int valCount = rowOfData.YearlyValues.Sum(x => x.Values.Length);
                    _reportWidth = valCount + rowOfData.Totals.Count + 2;
                    foreach (FundingSummaryReportYearlyValueModel fundingSummaryReportYearlyValueModel in
                        rowOfData.YearlyValues)
                    {
                        yearAndDataLengthModels.Add(new YearAndDataLengthModel(
                            fundingSummaryReportYearlyValueModel.FundingYear,
                            fundingSummaryReportYearlyValueModel.Values.Length));
                    }
                }

                _cachedHeaders = GetHeaderEntries(yearAndDataLengthModels);
                _cellStyles = _excelStyleProvider.GetFundingSummaryStyles(workbook);

                Worksheet sheet = workbook.Worksheets.Add(file.ConRefNumber);
                workbook = GetWorkbookReport(workbook, sheet, fundingSummaryHeaderModel, fundingSummaryModels, fundingSummaryFooterModel);
                ApplyAdditionalFormatting(workbook, rowOfData);
            }

            string externalFileName = GetExternalFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);
            string fileName = GetFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);

            using (var ms = new MemoryStream())
            {
                workbook.Save(ms, SaveFormat.Xlsx);
                await _storage.SaveAsync($"{externalFileName}.xlsx", ms, cancellationToken);
                await WriteZipEntry(archive, $"{fileName}.xlsx", ms, cancellationToken);
            }
        }

        private static void ApplyFundingYearToEmptyFundingYears(IEnumerable<FundingSummaryModel> fundingSummaryModels, int fundingYear)
        {
            foreach (var model in fundingSummaryModels)
            {
                foreach (var yearlyValue in model.YearlyValues)
                {
                    if (yearlyValue.FundingYear == 0)
                    {
                        yearlyValue.FundingYear = fundingYear;
                    }
                }
            }
        }

        private void ApplyAdditionalFormatting(Workbook workbook, FundingSummaryModel rowOfData)
        {
            if (rowOfData == null)
            {
                return;
            }

            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Cells.CreateRange(1, 5, 4, 1).ApplyStyle(_cellStyles[6].Style, _cellStyles[6].StyleFlag); // Header

            int valCount = rowOfData.YearlyValues.Sum(x => x.Values.Length);
            int nonYearCount = rowOfData.YearlyValues.Sum(x => x.FundingYear != 2018 ? x.Values.Length : 0);
            int yearCount = rowOfData.YearlyValues.Sum(x => x.FundingYear == 2018 ? x.Values.Length : 0);
            worksheet.Cells.CreateRange(9, nonYearCount + 1, 110, yearCount).ApplyStyle(_cellStyles[6].Style, _cellStyles[6].StyleFlag); // Current Year
            worksheet.Cells.CreateRange(9, valCount + rowOfData.Totals.Count, 110, 1).ApplyStyle(_cellStyles[6].Style, _cellStyles[6].StyleFlag); // Current Year Subtotal
        }

        private FundingSummaryHeaderModel PopulateReportHeader(
            SourceFileModel sourceFile,
            IEnumerable<ILRFileDetailsModel> fileData,
            int ukPrn,
            CancellationToken cancellationToken)
        {
            var ukPrnRow =
                new List<string> { ukPrn.ToString(), string.Empty, string.Empty };
            var contractReferenceNumberRow =
                new List<string> { sourceFile.ConRefNumber, string.Empty, string.Empty, "ILR File :" };
            var supplementaryDataFileRow =
                new List<string> { sourceFile.FileName.Contains("/") ? sourceFile.FileName.Substring(sourceFile.FileName.IndexOf("/", StringComparison.Ordinal) + 1) : sourceFile.FileName, string.Empty, string.Empty, "Last ILR File Update :" };
            var lastSupplementaryDataFileUpdateRow =
                new List<string> { sourceFile.SuppliedDate?.ToString("dd/MM/yyyy hh:mm:ss"), string.Empty, string.Empty, "File Preparation Date :" };

            foreach (var model in fileData)
            {
                var preparationDate = FileNameHelper.GetPreparedDateFromILRFileName(model.FileName);
                var secondYear = FileNameHelper.GetSecondYearFromReportYear(model.Year);

                ukPrnRow.Add(string.Empty);
                ukPrnRow.Add($"{model.Year}/{secondYear}");
                contractReferenceNumberRow.Add(model.FileName.Substring(model.FileName.Contains("/") ? model.FileName.IndexOf("/", StringComparison.Ordinal) + 1 : 0));
                contractReferenceNumberRow.Add(string.Empty);
                supplementaryDataFileRow.Add(model.LastSubmission?.ToString("dd/MM/yyyy hh:mm:ss"));
                supplementaryDataFileRow.Add(string.Empty);
                lastSupplementaryDataFileUpdateRow.Add(preparationDate);
                lastSupplementaryDataFileUpdateRow.Add(string.Empty);
            }

            var header = new FundingSummaryHeaderModel
            {
                ProviderName = _referenceDataCache.GetProviderName(ukPrn, cancellationToken),
                Ukprn = ukPrnRow.ToArray(),
                ContractReferenceNumber = contractReferenceNumberRow.ToArray(),
                SupplementaryDataFile = supplementaryDataFileRow.ToArray(),
                LastSupplementaryDataFileUpdate = lastSupplementaryDataFileUpdateRow.ToArray()
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

        private IEnumerable<FundingSummaryModel> PopulateReportData(
            IEnumerable<FM70PeriodisedValuesYearlyModel> fm70YearlyData,
            IEnumerable<SupplementaryDataYearlyModel> data)
        {
            var fundingSummaryModels = new List<FundingSummaryModel>();
            foreach (var fundingReportRow in ReportDataTemplate.FundingModelRowDefinitions)
            {
                foreach (var rowHelper in _rowHelpers)
                {
                    if (!rowHelper.IsMatch(fundingReportRow.RowType))
                    {
                        continue;
                    }

                    rowHelper.Execute(fundingSummaryModels, fundingReportRow, data, fm70YearlyData);
                    break;
                }
            }

            return fundingSummaryModels;
        }

        private Workbook GetWorkbookReport(
            Workbook workbook,
            Worksheet sheet,
            FundingSummaryHeaderModel fundingSummaryHeaderModel,
            IEnumerable<FundingSummaryModel> fundingSummaryModels,
            FundingSummaryFooterModel fundingSummaryFooterModel)
        {
            WriteExcelRecords(sheet, new FundingSummaryHeaderMapper(), new List<FundingSummaryHeaderModel> { fundingSummaryHeaderModel }, _cellStyles[5], _cellStyles[5], true);
            foreach (var fundingSummaryModel in fundingSummaryModels)
            {
                if (string.IsNullOrEmpty(fundingSummaryModel.Title))
                {
                    WriteBlankRow(sheet);
                    continue;
                }

                CellStyle excelHeaderStyle = _excelStyleProvider.GetCellStyle(_cellStyles, fundingSummaryModel.ExcelHeaderStyle);

                if (fundingSummaryModel.HeaderType == HeaderType.TitleOnly)
                {
                    WriteTitleRecord(sheet, fundingSummaryModel.Title, excelHeaderStyle, _reportWidth);
                    continue;
                }

                if (fundingSummaryModel.HeaderType == HeaderType.All)
                {
                    _fundingSummaryMapper.MemberMaps.Single(x => x.Data.Index == 0).Name(fundingSummaryModel.Title);
                    _cachedHeaders[0] = fundingSummaryModel.Title;
                    WriteRecordsFromArray(sheet, _fundingSummaryMapper, _cachedHeaders, excelHeaderStyle);
                    continue;
                }

                CellStyle excelRecordStyle = _excelStyleProvider.GetCellStyle(_cellStyles, fundingSummaryModel.ExcelRecordStyle);

                WriteExcelRecordsFromModelProperty(sheet, _fundingSummaryMapper, _cachedModelProperties, fundingSummaryModel, excelRecordStyle);
            }

            WriteExcelRecords(sheet, new FundingSummaryFooterMapper(), new List<FundingSummaryFooterModel> { fundingSummaryFooterModel }, _cellStyles[5], _cellStyles[5], true);

            return workbook;
        }

        private string[] GetHeaderEntries(List<YearAndDataLengthModel> yearAndDataLengthModels)
        {
            var values = new List<string>
            {
                string.Empty // placeholder for title
            };
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
            List<string> values,
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
            List<string> values,
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