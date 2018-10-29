using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ReportingService.Reports;
using ESFA.DC.ILR1819.DataStore.EF;
using ESFA.DC.ILR1819.DataStore.EF.Valid;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.ReferenceData.FCS.Model;
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
            var filename = $"10033670_1_ESF Aim and Deliverable Report {dateTime:yyyyMMdd-HHmmss}";

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
                .Returns(new List<ContractDeliverableCodeMapping>()); // todo
            refRepoMock.Setup(m => m.GetLarsLearningDelivery(It.IsAny<IList<string>>(), It.IsAny<CancellationToken>()))
                .Returns(new List<LARS_LearningDelivery>()); // todo

            var validRepoMock = new Mock<IValidRepository>();
            validRepoMock.Setup(m => m.GetLearners(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Learner>()); // todo
            validRepoMock.Setup(m => m.GetLearningDeliveries(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LearningDelivery>()); // todo
            validRepoMock.Setup(m => m.GetLearningDeliveryFAMs(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<LearningDeliveryFAM>()); // todo
            validRepoMock.Setup(m => m.GetDPOutcomes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DPOutcome>()); // todo
            validRepoMock.Setup(m =>
                    m.GetProviderSpecDeliveryMonitorings(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProviderSpecDeliveryMonitoring>()); // todo
            validRepoMock
                .Setup(m => m.GetProviderSpecLearnerMonitorings(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProviderSpecLearnerMonitoring>()); // todo

            var fm70RepoMock = new Mock<IFM70Repository>();
            fm70RepoMock.Setup(m => m.GetFileDetails(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FileDetail()); // todo
            fm70RepoMock.Setup(m => m.GetLearningDeliveries(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ESF_LearningDelivery>()); // todo
            fm70RepoMock.Setup(m => m.GetLearningDeliveryDeliverables(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ESF_LearningDeliveryDeliverable>()); // todo
            fm70RepoMock.Setup(m =>
                    m.GetLearningDeliveryDeliverablePeriods(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ESF_LearningDeliveryDeliverable_Period>()); // todo
            fm70RepoMock.Setup(m => m.GetOutcomes(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ESF_DPOutcome>()); // todo
            fm70RepoMock.Setup(m => m.GetPeriodisedValues(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ESF_LearningDeliveryDeliverable_PeriodisedValues>()); // todo

            var aimAndDeliverableReport = new AimAndDeliverableReport(
                dateTimeProviderMock.Object,
                storage.Object,
                refRepoMock.Object,
                validRepoMock.Object,
                fm70RepoMock.Object);

            SupplementaryDataWrapper wrapper = new SupplementaryDataWrapper();
            wrapper.SupplementaryDataModels = new List<SupplementaryDataModel>();
            SourceFileModel sourceFile = new SourceFileModel();

            await aimAndDeliverableReport.GenerateReport(wrapper, sourceFile, null, CancellationToken.None);

            Assert.True(!string.IsNullOrEmpty(csv));
        }

        private IList<Learner> GetLearners()
        {
            return new List<Learner>
            {
            };
        }
    }
}
