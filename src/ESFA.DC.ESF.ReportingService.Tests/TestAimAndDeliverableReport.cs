using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ReportingService.Comparers;
using ESFA.DC.ESF.ReportingService.Reports;
using ESFA.DC.ESF.ReportingService.Services;
using ESFA.DC.ESF.ReportingService.Tests.Builders;
using ESFA.DC.ILR1819.DataStore.EF;
using ESFA.DC.ILR1819.DataStore.EF.Valid;
using ESFA.DC.IO.Interfaces;
using Moq;
using Xunit;

namespace ESFA.DC.ESF.ReportingService.Tests
{
    public sealed class TestAimAndDeliverableReport
    {
        [Fact]
        public async Task TestAimAndDeliverableReportGeneration()
        {
            var csv = string.Empty;
            var dateTime = DateTime.UtcNow;
            var filename = $"10005752_2_ESF Aim and Deliverable Report {dateTime:yyyyMMdd-HHmmss}";

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(x => x.GetNowUtc()).Returns(dateTime);
            dateTimeProviderMock.Setup(x => x.ConvertUtcToUk(It.IsAny<DateTime>())).Returns(dateTime);

            var storage = new Mock<IStreamableKeyValuePersistenceService>();
            storage.Setup(x => x.SaveAsync($"{filename}.csv", It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback<string, string, CancellationToken>((key, value, ct) => csv = value)
                .Returns(Task.CompletedTask);

            var refRepoMock = new Mock<IReferenceDataRepository>();
            refRepoMock.Setup(m =>
                    m.GetContractDeliverableCodeMapping(It.IsAny<IList<string>>(), It.IsAny<CancellationToken>()))
                .Returns(ReferenceDataBuilder.BuildContractDeliverableCodeMapping());
            refRepoMock.Setup(m => m.GetLarsLearningDelivery(It.IsAny<IList<string>>(), It.IsAny<CancellationToken>()))
                .Returns(ReferenceDataBuilder.BuildLarsLearningDeliveries());

            var validRepoMock = new Mock<IValidRepository>();
            validRepoMock.Setup(m => m.GetLearners(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ValidLearnerModelsBuilder.BuildLearners());
            validRepoMock.Setup(m => m.GetLearningDeliveries(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ValidLearnerModelsBuilder.BuildLearningDeliveries());
            validRepoMock.Setup(m => m.GetLearningDeliveryFAMs(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ValidLearnerModelsBuilder.BuildLearningDeliveryFams());
            validRepoMock.Setup(m => m.GetDPOutcomes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DPOutcome>());
            validRepoMock.Setup(m =>
                    m.GetProviderSpecDeliveryMonitorings(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProviderSpecDeliveryMonitoring>());
            validRepoMock
                .Setup(m => m.GetProviderSpecLearnerMonitorings(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProviderSpecLearnerMonitoring>());

            var fm70RepoMock = new Mock<IFM70Repository>();
            fm70RepoMock.Setup(m => m.GetLearningDeliveries(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FM70ModelsBuilder.BuildLearningDeliveries());
            fm70RepoMock.Setup(m => m.GetLearningDeliveryDeliverables(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FM70ModelsBuilder.BuildLearningDeliveryDeliverables());
            fm70RepoMock.Setup(m =>
                    m.GetLearningDeliveryDeliverablePeriods(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(FM70ModelsBuilder.BuildDeliveryDeliverablePeriods());
            fm70RepoMock.Setup(m => m.GetOutcomes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ESF_DPOutcome>());

            var comparer = new AimAndDeliverableComparer();
            var valueProvider = new ValueProvider();

            var aimAndDeliverableReport = new AimAndDeliverableReport(
                dateTimeProviderMock.Object,
                storage.Object,
                refRepoMock.Object,
                validRepoMock.Object,
                fm70RepoMock.Object,
                valueProvider,
                comparer);

            var wrapper = new SupplementaryDataWrapper
            {
                SupplementaryDataModels = new List<SupplementaryDataModel>()
            };
            SourceFileModel sourceFile = GetEsfSourceFileModel();

            await aimAndDeliverableReport.GenerateReport(wrapper, sourceFile, null, CancellationToken.None);

            Assert.True(!string.IsNullOrEmpty(csv));
        }

        private SourceFileModel GetEsfSourceFileModel()
        {
            return new SourceFileModel
            {
                UKPRN = "10005752",
                JobId = 2,
                ConRefNumber = "ESF-2108",
                FileName = "SUPPDATA-10005752-ESF-2108-20180909-090911.CSV",
                SuppliedDate = DateTime.Now,
                PreparationDate = DateTime.Now.AddDays(-1)
            };
        }
    }
}
