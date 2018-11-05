using System;
using System.Collections.Generic;
using System.Threading;
using ESFA.DC.Data.ULN.Model;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using Moq;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.BusinessRuleTests
{
    public class UlnRuleTests
    {
        [Fact]
        public void ULNRule01CatchesEmptyULNs()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "LearnRefNumber",
                ULN = null
            };

            var rule = new ULNRule01();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void ULNRule01PassesPopulatedULNs()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "LearnRefNumber",
                ULN = 1990909009
            };

            var rule = new ULNRule01();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void ULNRule02CatchesULNNotInLookup()
        {
            //var referenceRepo = new Mock<IReferenceDataRepository>();
            //referenceRepo
            //    .Setup(x => x.GetUlnLookup(It.IsAny<IList<long>>(), It.IsAny<CancellationToken>()))
            //    .Returns(new List<UniqueLearnerNumber>());

            //var model = new SupplementaryDataModel
            //{
            //    ReferenceType = "LearnRefNumber",
            //    ULN = 1990909009
            //};

            //var rule = new ULNRule02(referenceRepo.Object);
            //

            //Assert.False(rule.Execute(model));
        }

        [Fact]
        public void ULNRule02PassesULNsFoundInLookup()
        {
            var referenceRepo = new Mock<IReferenceDataRepository>();
            referenceRepo
                .Setup(x => x.GetUlnLookup(It.IsAny<IList<long?>>(), It.IsAny<CancellationToken>()))
                .Returns(new List<UniqueLearnerNumber> { new UniqueLearnerNumber { ULN = 1990909009 } });

            var model = new SupplementaryDataModel
            {
                ReferenceType = "LearnRefNumber",
                ULN = 1990909009
            };

            var rule = new ULNRule02(referenceRepo.Object);

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void ULNRule03CatchesULNsForDatesOlderThan2MonthsAgo()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "LearnRefNumber",
                ULN = 1990909009,
                CalendarYear = DateTime.Now.Year,
                CalendarMonth = DateTime.Now.AddMonths(-6).Month
            };

            var rule = new ULNRule03();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void ULNRule03PassesULNsForMonthsAfer2MonthsAgo()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "LearnRefNumber",
                ULN = 1990909009,
                CalendarYear = DateTime.Now.Year,
                CalendarMonth = DateTime.Now.Month
            };

            var rule = new ULNRule03();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void ULNRule04CatchesULNsWhenNotRequired()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "Other",
                ULN = 1990909009
            };

            var rule = new ULNRule04();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void ULNRule04PassesEmptyULNsWhenNotRequired()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "Other",
                ULN = null
            };

            var rule = new ULNRule04();

            Assert.True(rule.Execute(model));
        }
    }
}
