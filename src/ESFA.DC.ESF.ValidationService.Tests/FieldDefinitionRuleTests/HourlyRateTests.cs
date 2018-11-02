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
            var model = new SupplementaryDataModel
            {
                HourlyRate = 1234567.123M
            };
            var rule = new FDHourlyRateAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDHourlyRateALPassesValidHourlyRate()
        {
            var model = new SupplementaryDataModel
            {
                HourlyRate = 123456.12M
            };
            var rule = new FDHourlyRateAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDOrgHoursALCatchesTooLongOrgHours()
        {
            var model = new SupplementaryDataModel
            {
                OrgHours = 1234567.123M
            };
            var rule = new FDOrgHoursAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDOrgHoursALPassesValidOrgHours()
        {
            var model = new SupplementaryDataModel
            {
                OrgHours = 123456.12M
            };
            var rule = new FDOrgHoursAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDProjectHoursALCatchesTooLongProjectHours()
        {
            var model = new SupplementaryDataModel
            {
                ProjectHours = 1234567.123M
            };
            var rule = new FDProjectHoursAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDProjectHoursALPassesValidProjectHours()
        {
            var model = new SupplementaryDataModel
            {
                ProjectHours = 123456.12M
            };
            var rule = new FDProjectHoursAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDTotalHoursWorkedALCatchesTooLongTotalHoursWorked()
        {
            var model = new SupplementaryDataModel
            {
                TotalHoursWorked = 1234567.123M
            };
            var rule = new FDTotalHoursWorkedAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDTotalHoursWorkedALPassesValidTotalHoursWorked()
        {
            var model = new SupplementaryDataModel
            {
                TotalHoursWorked = 123456.12M
            };
            var rule = new FDTotalHoursWorkedAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}