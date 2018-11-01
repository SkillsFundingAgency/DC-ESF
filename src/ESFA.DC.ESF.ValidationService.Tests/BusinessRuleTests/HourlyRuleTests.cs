using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.BusinessRuleTests
{
    public class HourlyRuleTests
    {
        [Fact]
        public void HourlyRateRule01CatchesNullRatesForPartTimeStaffCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeStaffPT,
                HourlyRate = null
            };

            var rule = new HourlyRateRule01();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void HourlyRateRule01PassesNotNullRatesForPartTimeStaffCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeStaffPT,
                HourlyRate = 12.0M
            };

            var rule = new HourlyRateRule01();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void OrgHoursRule01CatchesNullOrgHoursForApportionedCostCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeApportionedCost,
                OrgHours = null
            };

            var rule = new OrgHoursRule01();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void OrgHoursRule01PassesNotNullOrgHoursForApportionedCostCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeApportionedCost,
                OrgHours = 12.0M
            };

            var rule = new OrgHoursRule01();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void OrgHoursRule02CatchesOrgHoursForNonApportionedCostCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "I don't need org hours",
                OrgHours = 12
            };

            var rule = new OrgHoursRule02();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void OrgHoursRule02PassesNullOrgHoursForNonApportionedCostCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "I don't need org hours",
                OrgHours = null
            };

            var rule = new OrgHoursRule02();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void ProjectHoursOrgHoursRule01CatchesProjectHoursMoreThanOrgHoursForApportionedCostCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeApportionedCost,
                OrgHours = 10,
                ProjectHours = 20
            };

            var rule = new ProjectHoursOrgHoursRule01();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void ProjectHoursOrgHoursRule01PassesProjectHoursNotMoreThanOrgHoursForApportionedCostCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeApportionedCost,
                OrgHours = 10,
                ProjectHours = 5
            };

            var rule = new ProjectHoursOrgHoursRule01();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void ProjectHoursRule01CatchesNullProjectHoursForApportionedCostCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeApportionedCost,
                ProjectHours = null
            };

            var rule = new ProjectHoursRule01();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void ProjectHoursRule01PassesNotNullOHoursForApportionedCostCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeApportionedCost,
                ProjectHours = 12.0M
            };

            var rule = new ProjectHoursRule01();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void ProjectHoursRule02CatchesProjectHoursForNonApportionedCostCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "I don't need project hours",
                ProjectHours = 12
            };

            var rule = new ProjectHoursRule02();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void ProjectHoursRule02PassesNullProjectHoursForNonApportionedCostCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "I don't need project hours",
                ProjectHours = null
            };

            var rule = new ProjectHoursRule02();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void TotalHoursWorkedRule01CatchesNullTotalHoursForStaffBasedCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeStaffFT,
                TotalHoursWorked = null
            };

            var rule = new TotalHoursWorkedRule01();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void TotalHoursWorkedRule01PassesTotalHoursForStaffBasedCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeStaffFT,
                TotalHoursWorked = 30
            };

            var rule = new TotalHoursWorkedRule01();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void TotalHoursWorkedRule02CatchesTotalHoursForNonStaffBasedCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "Don't need total",
                TotalHoursWorked = 30
            };

            var rule = new TotalHoursWorkedRule02();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void TotalHoursWorkedRule02PassesNullTotalHoursForNonStaffBasedCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "Don't need total",
                TotalHoursWorked = null
            };

            var rule = new TotalHoursWorkedRule02();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}
