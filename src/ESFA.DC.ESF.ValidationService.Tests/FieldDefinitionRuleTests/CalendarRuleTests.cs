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

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDCalendarMonthALPassesValidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 10
            };
            var rule = new FDCalendarMonthAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDCalendarMonthDTCatchesInvalidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = null
            };
            var rule = new FDCalendarMonthDT();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDCalendarMonthDTPassesValidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 10
            };
            var rule = new FDCalendarMonthDT();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDCalendarMonthMACatchesNullMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = null
            };
            var rule = new FDCalendarMonthMA();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDCalendarMonthMAPassesValidMonths()
        {
            var model = new SupplementaryDataModel
            {
                CalendarMonth = 10
            };
            var rule = new FDCalendarMonthMA();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDCalendarYearALCatchesInvalidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = 12345
            };
            var rule = new FDCalendarYearAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDCalendarYearALPassesValidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = 2010
            };
            var rule = new FDCalendarYearAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDCalendarYearDTCatchesInvalidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = null
            };
            var rule = new FDCalendarYearDT();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDCalendarYearDTPassesValidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = 2010
            };
            var rule = new FDCalendarYearDT();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDCalendarYearMACatchesInvalidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = null
            };
            var rule = new FDCalendarYearMA();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDCalendarYearMAPassesValidYears()
        {
            var model = new SupplementaryDataModel
            {
                CalendarYear = 2010
            };
            var rule = new FDCalendarYearMA();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}
