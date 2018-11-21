using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.FieldDefinitionRuleTests
{
    public class HourlyRateTests
    {
        [Fact]
        public void FDHourlyRateALCatchesTooLongHourlyRate()
        {
            var model = new SupplementaryDataLooseModel
            {
                HourlyRate = "1234567.123"
            };
            var rule = new FDHourlyRateAL();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDHourlyRateALPassesValidHourlyRate()
        {
            var model = new SupplementaryDataLooseModel
            {
                HourlyRate = "123456.12"
            };
            var rule = new FDHourlyRateAL();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDOrgHoursALCatchesTooLongOrgHours()
        {
            var model = new SupplementaryDataLooseModel
            {
                OrgHours = "1234567.123"
            };
            var rule = new FDOrgHoursAL();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDOrgHoursALPassesValidOrgHours()
        {
            var model = new SupplementaryDataLooseModel
            {
                OrgHours = "123456.12"
            };
            var rule = new FDOrgHoursAL();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDProjectHoursALCatchesTooLongProjectHours()
        {
            var model = new SupplementaryDataLooseModel
            {
                ProjectHours = "1234567.123"
            };
            var rule = new FDProjectHoursAL();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDProjectHoursALPassesValidProjectHours()
        {
            var model = new SupplementaryDataLooseModel
            {
                ProjectHours = "123456.12"
            };
            var rule = new FDProjectHoursAL();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDTotalHoursWorkedALCatchesTooLongTotalHoursWorked()
        {
            var model = new SupplementaryDataLooseModel
            {
                TotalHoursWorked = "1234567.123"
            };
            var rule = new FDTotalHoursWorkedAL();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDTotalHoursWorkedALPassesValidTotalHoursWorked()
        {
            var model = new SupplementaryDataLooseModel
            {
                TotalHoursWorked = "123456.12"
            };
            var rule = new FDTotalHoursWorkedAL();

            Assert.True(rule.Execute(model));
        }
    }
}