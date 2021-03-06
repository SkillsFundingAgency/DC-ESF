﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces.Config;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Reports.Services;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ReportingService.Reports.FundingSummary;
using ESFA.DC.ESF.ReportingService.Services;
using ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers;
using ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr;
using ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData;
using ESFA.DC.ESF.ReportingService.Tests.Builders;
using ESFA.DC.IO.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ESF.ReportingService.Tests
{
    public class TestFundingSummaryReport
    {
        [Fact]
        public async Task TestFundingSummaryReportGeneration()
        {
            var dateTime = DateTime.UtcNow;
            var filename = $"10005752_1_ESF Funding Summary Report {dateTime:yyyyMMdd-HHmmss}";
            byte[] xlsx = null;

            Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.GetNowUtc()).Returns(dateTime);
            dateTimeProviderMock.Setup(x => x.ConvertUtcToUk(It.IsAny<DateTime>())).Returns(dateTime);

            Mock<IStreamableKeyValuePersistenceService> storage = new Mock<IStreamableKeyValuePersistenceService>();
            storage.Setup(x => x.SaveAsync($"{filename}.xlsx", It.IsAny<Stream>(), It.IsAny<CancellationToken>())).Callback<string, Stream, CancellationToken>(
                    (key, value, ct) =>
                    {
                        value.Seek(0, SeekOrigin.Begin);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            value.CopyTo(ms);
                            xlsx = ms.ToArray();
                        }
                    })
                .Returns(Task.CompletedTask);

            IList<IRowHelper> rowHelpers = GenerateRowHelpersWithStrategies();

            var supplementaryDataService = new Mock<ISupplementaryDataService>();
            supplementaryDataService
                .Setup(s => s.GetSupplementaryData(It.IsAny<IEnumerable<SourceFileModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dictionary<int, IEnumerable<SupplementaryDataYearlyModel>>());
            supplementaryDataService
                .Setup(s => s.GetImportFiles(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SourceFileModel>());

            Mock<IReferenceDataCache> referenceDataCache = new Mock<IReferenceDataCache>();
            referenceDataCache.Setup(m => m.GetLarsVersion(It.IsAny<CancellationToken>())).Returns("123456");
            referenceDataCache.Setup(m => m.GetOrganisationVersion(It.IsAny<CancellationToken>())).Returns("234567");
            referenceDataCache.Setup(m => m.GetPostcodeVersion(It.IsAny<CancellationToken>())).Returns("345678");
            referenceDataCache.Setup(m => m.GetProviderName(It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns("Foo College");

            Mock<IVersionInfo> versionInfo = new Mock<IVersionInfo>();
            versionInfo.Setup(m => m.ServiceReleaseVersion).Returns("1.2.3.4");
            var valueProvider = new ValueProvider();
            var excelStyleProvider = new ExcelStyleProvider();

            IList<FM70PeriodisedValuesYearlyModel> periodisedValues = new List<FM70PeriodisedValuesYearlyModel>();
            var ilrMock = new Mock<IILRService>();
            ilrMock.Setup(m => m.GetYearlyIlrData(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodisedValues);
            ilrMock.Setup(m => m.GetIlrFileDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(GetTestFileDetail());

            var fundingSummaryReport = new FundingSummaryReport(
                dateTimeProviderMock.Object,
                valueProvider,
                storage.Object,
                ilrMock.Object,
                supplementaryDataService.Object,
                rowHelpers,
                referenceDataCache.Object,
                excelStyleProvider,
                versionInfo.Object);

            SourceFileModel sourceFile = GetEsfSourceFileModel();

            SupplementaryDataWrapper wrapper = new SupplementaryDataWrapper();
            wrapper.SupplementaryDataModels = SupplementaryDataModelBuilder.GetModels();

            await fundingSummaryReport.GenerateReport(wrapper, sourceFile, null, CancellationToken.None);

            storage.Verify(s => s.SaveAsync($"{filename}.xlsx", It.IsAny<Stream>(), It.IsAny<CancellationToken>()));

           Assert.NotEmpty(xlsx);

#if DEBUG
            File.WriteAllBytes($"{filename}.xlsx", xlsx);
#endif
        }

        private IEnumerable<ILRFileDetailsModel> GetTestFileDetail()
        {
            return new List<ILRFileDetailsModel>
            {
                new ILRFileDetailsModel
                {
                    FileName = "ILR-10005752-1819-20181004-152148-02.xml",
                    LastSubmission = DateTime.Now
                }
            };
        }

        private SourceFileModel GetEsfSourceFileModel()
        {
            return new SourceFileModel
            {
                UKPRN = "10005752",
                JobId = 1,
                ConRefNumber = "ESF-2108",
                FileName = "SUPPDATA-10005752-ESF-2108-20180909-090911.CSV",
                SuppliedDate = DateTime.Now,
                PreparationDate = DateTime.Now.AddDays(-1)
            };
        }

        private IList<IRowHelper> GenerateRowHelpersWithStrategies()
        {
            return new List<IRowHelper>
            {
                new SpacerRowHelper(),
                new TitleRowHelper(),
                new TotalRowHelper(),
                new CumulativeRowHelper(),
                new MainTitleRowHelper(),
                new DataRowHelper(
                    new List<ISupplementaryDataStrategy>
                    {
                        new AC01ActualCosts(),
                        new CG01CommunityGrantPayment(),
                        new CG02CommunityGrantManagementCost(),
                        new FS01AdditionalProgrammeCostAdjustments(),
                        new NR01NonRegulatedActivityAuditAdjustment(),
                        new NR01NonRegulatedActivityAuthorisedClaims(),
                        new PG01ProgressionPaidEmploymentAdjustments(),
                        new PG02ProgressionUnpaidEmploymentAdjustments(),
                        new PG03ProgressionEducationAdjustments(),
                        new PG04ProgressionApprenticeshipAdjustments(),
                        new PG05ProgressionTraineeshipAdjustments(),
                        new PG06ProgressionJobSearchAdjustments(),
                        new RQ01RegulatedLearningAuditAdjustment(),
                        new RQ01RegulatedLearningAuthorisedClaims(),
                        new SD01FCSDeliverableDescription(),
                        new SD02FCSDeliverableDescription(),
                        new SD03FCSDeliverableDescription(),
                        new SD04FCSDeliverableDescription(),
                        new SD05FCSDeliverableDescription(),
                        new SD06FCSDeliverableDescription(),
                        new SD07FCSDeliverableDescription(),
                        new SD08FCSDeliverableDescription(),
                        new SD09FCSDeliverableDescription(),
                        new SD10FCSDeliverableDescription(),
                        new ST01LearnerAssessmentAndPlanAdjustments(),
                        new SU01SustainedPaidEmployment3MonthsAdjustments(),
                        new SU02SustainedUnpaidEmployment3MonthsAdjustments(),
                        new SU03SustainedEducation3MonthsAdjustments(),
                        new SU04SustainedApprenticeship3MonthsAdjustments(),
                        new SU05SustainedTraineeship3MonthsAdjustments(),
                        new SU11SustainedPaidEmployment6MonthsAdjustments(),
                        new SU12SustainedUnpaidEmployment6MonthsAdjustments(),
                        new SU13SustainedEducation6MonthsAdjustments(),
                        new SU14SustainedApprenticeship6MonthsAdjustments(),
                        new SU15SustainedTraineeship6MonthsAdjustments(),
                        new SU21SustainedPaidEmployment12MonthsAdjustments(),
                        new SU22SustainedUnpaidEmployment12MonthsAdjustments(),
                        new SU23SustainedEducation12MonthsAdjustments(),
                        new SU24SustainedApprenticeship12MonthsAdjustments()
                    },
                    new List<IILRDataStrategy>
                    {
                        new FS01AdditionalProgrammeCost(),
                        new NR01NonRegulatedActivityAchievementEarnings(),
                        new NR01NonRegulatedActivityStartFunding(),
                        new PG01ProgressionPaidEmployment(),
                        new PG02ProgressionUnpaidEmployment(),
                        new PG03ProgressionEducation(),
                        new PG04ProgressionApprenticeship(),
                        new PG05ProgressionTraineeship(),
                        new PG06ProgressionJobSearch(),
                        new RQ01RegulatedLearningAchievementEarnings(),
                        new RQ01RegulatedLearningStartFunding(),
                        new ST01LearnerAssessmentAndPlan(),
                        new SU01SustainedPaidEmployment3Months(),
                        new SU02SustainedUnpaidEmployment3Months(),
                        new SU03SustainedEducation3Months(),
                        new SU04SustainedApprenticeship3Months(),
                        new SU05SustainedTraineeship3Months(),
                        new SU11SustainedPaidEmployment6Months(),
                        new SU12SustainedUnpaidEmployment6Months(),
                        new SU13SustainedEducation6Months(),
                        new SU14SustainedApprenticeship6Months(),
                        new SU15SustainedTraineeship6Months(),
                        new SU21SustainedPaidEmployment12Months(),
                        new SU22SustainedUnpaidEmployment12Months(),
                        new SU23SustainedEducation12Months(),
                        new SU24SustainedApprenticeship12Months()
                    })
            };
        }
    }
}
