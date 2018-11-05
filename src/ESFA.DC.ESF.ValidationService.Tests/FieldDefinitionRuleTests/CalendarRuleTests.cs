using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.FieldDefinitionRuleTests
{
    public class CalendarRuleTests
    {
        [Fact]
        public void FDCalendarMonthALCatchesInvalidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 1612
            };
            var rule = new FDCalendarMonthAL();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarMonthALPassesValidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 10
            };
            var rule = new FDCalendarMonthAL();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarMonthDTCatchesInvalidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = null
            };
            var rule = new FDCalendarMonthDT();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarMonthDTPassesValidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 10
            };
            var rule = new FDCalendarMonthDT();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarMonthMACatchesNullMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = null
            };
            var rule = new FDCalendarMonthMA();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarMonthMAPassesValidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 10
            };
            var rule = new FDCalendarMonthMA();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarYearALCatchesInvalidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = 12345
            };
            var rule = new FDCalendarYearAL();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarYearALPassesValidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = 2010
            };
            var rule = new FDCalendarYearAL();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarYearDTCatchesInvalidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = null
            };
            var rule = new FDCalendarYearDT();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarYearDTPassesValidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = 2010
            };
            var rule = new FDCalendarYearDT();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarYearMACatchesInvalidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = null
            };
            var rule = new FDCalendarYearMA();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDCalendarYearMAPassesValidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = 2010
            };
            var rule = new FDCalendarYearMA();

            Assert.True(rule.Execute(model));
        }
    }
}
