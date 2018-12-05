using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.BusinessRuleTests
{
    public class ValueRuleTests
    {
        [Fact]
        public void ValueRule01CatchesEmptyValueForCostTypeRequiringOne()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "Grant",
                Value = null
            };

            var rule = new ValueRule01();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void ValueRule01PassesNonEmptyValueForCostTypeRequiringOne()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "Grant",
                Value = 19.99M
            };

            var rule = new ValueRule01();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void ValueRule02CatchesValueForCostTypeNotRequiringOne()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "Unit Cost",
                Value = 19.99M
            };

            var rule = new ValueRule02();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void ValueRule02PassesEmptyValueForCostTypeNotRequiringOne()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "Unit Cost",
                Value = null
            };

            var rule = new ValueRule02();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void ValueRule03CatchesValueGreaterThanRateTimesHoursForPartTimeCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeStaffPT,
                HourlyRate = 1,
                TotalHoursWorked = 1,
                Value = 20.00M
            };

            var rule = new ValueRule03();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void ValueRule03PassesValueNotGreaterThanRateTimesHoursForPartTimeCostType()
        {
            var model = new SupplementaryDataModel
            {
                CostType = Constants.CostTypeStaffPT,
                HourlyRate = 10,
                TotalHoursWorked = 2,
                Value = 19.99M
            };

            var rule = new ValueRule03();

            Assert.True(rule.Execute(model));
        }
    }
}
