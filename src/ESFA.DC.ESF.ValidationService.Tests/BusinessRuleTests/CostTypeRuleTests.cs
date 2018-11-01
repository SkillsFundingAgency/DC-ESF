using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.BusinessRuleTests
{
    public class CostTypeRuleTests
    {
        [Fact]
        public void CostTypeRule01CatchesCostTypesNotInValidList()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "I am not valid"
            };
            var rule = new CostTypeRule01();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void CostTypeRule01PassesValidCostTypes()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "Grant"
            };
            var rule = new CostTypeRule01();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void CostTypeRule02CatchesInvalidCostTypeDeliverableCodeCombinations()
        {
            var model = new SupplementaryDataModel
            {
                DeliverableCode = "CG01",
                CostType = "I am not valid"
            };
            var rule = new CostTypeRule02();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void CostTypeRule02PassesValidCostTypeDeliverableCodeCombinations()
        {
            var model = new SupplementaryDataModel
            {
                DeliverableCode = "CG01",
                CostType = "Grant"
            };
            var rule = new CostTypeRule02();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}
