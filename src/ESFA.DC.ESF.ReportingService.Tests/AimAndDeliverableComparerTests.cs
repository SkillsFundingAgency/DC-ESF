using System;
using System.Collections.Generic;
using ESFA.DC.ESF.Models.Reports;
using ESFA.DC.ESF.ReportingService.Comparers;
using Xunit;

namespace ESFA.DC.ESF.ReportingService.Tests
{
    public class AimAndDeliverableComparerTests
    {
        [Fact]
        public void CheckComparerCorrectlySortsByLearnerReference()
        {
            var comparer = new AimAndDeliverableComparer();

            var model1 = new AimAndDeliverableModel
            {
                LearnRefNumber = string.Empty
            };

            var model2 = new AimAndDeliverableModel
            {
                LearnRefNumber = "1"
            };

            var model3 = new AimAndDeliverableModel
            {
                LearnRefNumber = "2"
            };

            var model4 = new AimAndDeliverableModel
            {
                LearnRefNumber = null
            };

            var models = new List<AimAndDeliverableModel>
            {
                model4,
                model3,
                model2,
                model1
            };

            models.Sort(comparer);

            Assert.True(models[0].LearnRefNumber == null);
            Assert.True(models[3].LearnRefNumber == "2");
        }

        [Fact]
        public void CheckComparerCorrectlySortsByContractReference()
        {
            var comparer = new AimAndDeliverableComparer();

            var model1 = new AimAndDeliverableModel
            {
                LearnRefNumber = "2",
                ConRefNumber = "4"
            };

            var model2 = new AimAndDeliverableModel
            {
                LearnRefNumber = "2",
                ConRefNumber = "3"
            };

            var model3 = new AimAndDeliverableModel
            {
                LearnRefNumber = "2",
                ConRefNumber = "2"
            };

            var model4 = new AimAndDeliverableModel
            {
                LearnRefNumber = "2",
                ConRefNumber = "1"
            };

            var models = new List<AimAndDeliverableModel>
            {
                model1,
                model2,
                model2,
                model3,
                model4
            };

            models.Sort(comparer);

            Assert.True(models[0].ConRefNumber == "1");
            Assert.True(models[4].ConRefNumber == "4");
        }

        [Fact]
        public void CheckComparerCorrectlySortsByAimSequenceNumber()
        {
            var comparer = new AimAndDeliverableComparer();

            var model1 = new AimAndDeliverableModel
            {
                LearnRefNumber = "2",
                ConRefNumber = "2",
                LearnStartDate = new DateTime(2018, 11, 10),
                AimSeqNumber = 4
            };

            var model2 = new AimAndDeliverableModel
            {
                LearnRefNumber = "2",
                ConRefNumber = "2",
                LearnStartDate = new DateTime(2018, 11, 10),
                AimSeqNumber = 3
            };

            var model3 = new AimAndDeliverableModel
            {
                LearnRefNumber = "2",
                ConRefNumber = "2",
                LearnStartDate = new DateTime(2018, 11, 10),
                AimSeqNumber = 2
            };

            var model4 = new AimAndDeliverableModel
            {
                LearnRefNumber = "2",
                ConRefNumber = "2",
                LearnStartDate = new DateTime(2018, 11, 10),
                AimSeqNumber = 1
            };

            var models = new List<AimAndDeliverableModel>
            {
                model1,
                model2,
                model2,
                model3,
                model4
            };

            models.Sort(comparer);

            Assert.True(models[0].AimSeqNumber == 1);
            Assert.True(models[4].AimSeqNumber == 4);
        }
    }
}