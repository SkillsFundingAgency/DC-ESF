using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.BusinessRuleTests
{
    public class StaffNameRuleTests
    {
        [Fact]
        public void StaffNameRule01CatchesEmptyStaffNameForEmployeeIDReferenceType()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "Employee ID",
                StaffName = null
            };

            var rule = new StaffNameRule01();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void StaffNameRule01PassesStaffNameForEmployeeIDReferenceType()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "Employee ID",
                StaffName = "Mr Bob"
            };

            var rule = new StaffNameRule01();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void StaffNameRule02CatchesRegexViolations()
        {
            var model = new SupplementaryDataModel
            {
                StaffName = "|~"
            };

            var rule = new StaffNameRule02();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void StaffNameRule02PassesValidStaffNames()
        {
            var model = new SupplementaryDataModel
            {
                StaffName = @"Aa0.,;:~!”@#$&’()/+-<=>[]{}^£€"
            };

            var rule = new StaffNameRule02();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void StaffNameRule03CatchesNotRequiredStaffName()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "Other",
                StaffName = "Mr Bob"
            };

            var rule = new StaffNameRule03();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void StaffNameRule03PassesEmptyStaffNamesWhenNotRequired()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "Other",
                StaffName = null
            };

            var rule = new StaffNameRule03();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}
