using System;
using System.Threading;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using ESFA.DC.ReferenceData.FCS.Model;
using Moq;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.BusinessRuleTests
{
    public class CalendarRuleTests
    {
        [Fact]
        public void TestThatCalendarMonthRule01CatchesInvalidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 16
            };
            var rule = new CalendarMonthRule01();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void TestThatCalendarMonthRule01PassesValidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 10
            };
            var rule = new CalendarMonthRule01();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void CalendarYearCalendarMonthRule01CatchesFutureDates()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 10,
                CalendarYear = 2050
            };
            var rule = new CalendarYearCalendarMonthRule01();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void CalendarYearCalendarMonthRule01PassesDatesNotInTheFuture()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 10,
                CalendarYear = 2017
            };
            var rule = new CalendarYearCalendarMonthRule01();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void CalendarYearCalendarMonthRule02CatchesDatesPriorToContractDate()
        {
            var allocation = new ContractAllocation
            {
                StartDate = new DateTime(2018, 01, 01)
            };

            var mapper = new Mock<IFcsCodeMappingHelper>();
            mapper.Setup(
                    x => x.GetFcsDeliverableCode(It.IsAny<SupplementaryDataModel>(), It.IsAny<CancellationToken>()))
                .Returns(3);

            var referenceRepo = new Mock<IReferenceDataRepository>();
            referenceRepo
                .Setup(x => x.GetContractAllocation(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<long?>()))
                .Returns(allocation);

            var model = new SupplementaryDataModel
            {
                ConRefNumber = "ESF-2111",
                CalendarMonth = 10,
                CalendarYear = 2017
            };
            var rule = new CalendarYearCalendarMonthRule02(referenceRepo.Object, mapper.Object);

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void CalendarYearCalendarMonthRule02PassesDatesInTheContractPeriod()
        {
            var allocation = new ContractAllocation
            {
                StartDate = new DateTime(2017, 11, 01)
            };

            var mapper = new Mock<IFcsCodeMappingHelper>();
            mapper.Setup(
                    x => x.GetFcsDeliverableCode(It.IsAny<SupplementaryDataModel>(), It.IsAny<CancellationToken>()))
                .Returns(3);

            var referenceRepo = new Mock<IReferenceDataRepository>();
            referenceRepo
                .Setup(x => x.GetContractAllocation(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<long?>()))
                .Returns(allocation);

            var model = new SupplementaryDataModel
            {
                ConRefNumber = "ESF-2111",
                CalendarMonth = 11,
                CalendarYear = 2017
            };
            var rule = new CalendarYearCalendarMonthRule02(referenceRepo.Object, mapper.Object);

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void CalendarYearCalendarMonthRule03CatchesDatesAfterTheContractDate()
        {
            var allocation = new ContractAllocation
            {
                EndDate = new DateTime(2017, 11, 01)
            };

            var mapper = new Mock<IFcsCodeMappingHelper>();
            mapper.Setup(
                    x => x.GetFcsDeliverableCode(It.IsAny<SupplementaryDataModel>(), It.IsAny<CancellationToken>()))
                .Returns(3);

            var referenceRepo = new Mock<IReferenceDataRepository>();
            referenceRepo
                .Setup(x => x.GetContractAllocation(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<long?>()))
                .Returns(allocation);

            var model = new SupplementaryDataModel
            {
                ConRefNumber = "ESF-2111",
                CalendarMonth = 12,
                CalendarYear = 2017
            };
            var rule = new CalendarYearCalendarMonthRule03(referenceRepo.Object, mapper.Object);

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void CalendarYearCalendarMonthRule03PassesDatesInTheContractPeriod()
        {
            var allocation = new ContractAllocation
            {
                EndDate = new DateTime(2017, 11, 01)
            };

            var mapper = new Mock<IFcsCodeMappingHelper>();
            mapper.Setup(
                    x => x.GetFcsDeliverableCode(It.IsAny<SupplementaryDataModel>(), It.IsAny<CancellationToken>()))
                .Returns(3);

            var referenceRepo = new Mock<IReferenceDataRepository>();
            referenceRepo
                .Setup(x => x.GetContractAllocation(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>(), It.IsAny<long?>()))
                .Returns(allocation);

            var model = new SupplementaryDataModel
            {
                ConRefNumber = "ESF-2111",
                CalendarMonth = 10,
                CalendarYear = 2017
            };
            var rule = new CalendarYearCalendarMonthRule03(referenceRepo.Object, mapper.Object);

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void CalendarYearRule01CatchesYearsOutsideOfTheAllowedRange()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = 1998
            };
            var rule = new CalendarYearRule01();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void CalendarYearRule01PassesYearsInsideOfTheAllowedRange()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = 2018
            };
            var rule = new CalendarYearRule01();

            Assert.True(rule.Execute(model));
        }
    }
}
