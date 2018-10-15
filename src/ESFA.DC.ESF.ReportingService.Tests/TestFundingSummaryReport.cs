using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces.Config;
using ESFA.DC.ESF.Interfaces.Reports.Services;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;
using ESFA.DC.ESF.Interfaces.Repositories;
using ESFA.DC.ESF.Interfaces.Strategies;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ReportingService.Reports.FundingSummary;
using ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.CSVRowHelpers;
using ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr;
using ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData;
using ESFA.DC.ILR1819.DataStore.EF;
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
            var filename = $"10033670_1_ESF Funding Summary Report {dateTime:yyyyMMdd-HHmmss}";
            var csv = string.Empty;

            Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.GetNowUtc()).Returns(dateTime);
            dateTimeProviderMock.Setup(x => x.ConvertUtcToUk(It.IsAny<DateTime>())).Returns(dateTime);

            Mock<IStreamableKeyValuePersistenceService> storage = new Mock<IStreamableKeyValuePersistenceService>();
            storage.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Callback<string, Stream, CancellationToken>((st, sr, ct) => File.OpenRead("ILR.xml").CopyTo(sr))
                .Returns(Task.CompletedTask);
            storage.Setup(x => x.SaveAsync($"{filename}.csv", It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback<string, string, CancellationToken>((key, value, ct) => csv = value)
                .Returns(Task.CompletedTask);

            Mock<IIlrEsfRepository> ilrRepo = new Mock<IIlrEsfRepository>();
            ilrRepo.Setup(m => m.GetFileDetails(It.IsAny<int>())).Returns(GetTestFileDetail());
            // ilrRepo.Setup(m => m.GetPeriodisedValues(It.IsAny<int>())).Returns();

            IList<IRowHelper> rowHelpers = GenerateRowHelpersWithStrategies();

            Mock<IReferenceDataService> referenceDataService = new Mock<IReferenceDataService>();
            referenceDataService.Setup(m => m.GetLarsVersion()).Returns("123456");
            referenceDataService.Setup(m => m.GetOrganisationVersion()).Returns("234567");
            referenceDataService.Setup(m => m.GetPostcodeVersion()).Returns("345678");
            referenceDataService.Setup(m => m.GetProviderName(It.IsAny<int>())).Returns("Foo College");

            Mock<IVersionInfo> versionInfo = new Mock<IVersionInfo>();
            versionInfo.Setup(m => m.ServiceReleaseVersion).Returns("1.2.3.4");

            var fundingSummaryReport = new FundingSummaryReport(
                dateTimeProviderMock.Object,
                storage.Object,
                ilrRepo.Object,
                rowHelpers,
                referenceDataService.Object,
                versionInfo.Object
            );

            IList<SupplementaryDataModel> suppData = new List<SupplementaryDataModel>();
            SourceFileModel sourceFile = new SourceFileModel();

            await fundingSummaryReport.GenerateReport(suppData, sourceFile, null, CancellationToken.None);
        }

        private FileDetail GetTestFileDetail()
        {
            return new FileDetail
            {
                UKPRN = 10033670,
                Filename = "",
                SubmittedTime = DateTime.Now
            };
        }

        private IList<IRowHelper> GenerateRowHelpersWithStrategies()
        {
            return new List<IRowHelper>
            {
                new SpacerRowHelper(),
                new TitleRowHelper(),
                new TotalRowHelper(),
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
