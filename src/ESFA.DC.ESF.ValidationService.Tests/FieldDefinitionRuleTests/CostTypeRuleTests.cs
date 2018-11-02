using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.FieldDefinitionRuleTests
{
    public class CostTypeRuleTests
    {
        [Fact]
        public void FDCostTypeALCatchesTooLongCostTypes()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "123456789012345678901"
            };
            var rule = new FDCostTypeAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDCostTypeALPassesValidCostTypes()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "12345678901234567890"
            };
            var rule = new FDCostTypeAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDCostTypeMACatchesEmptyCostTypes()
        {
            var model = new SupplementaryDataModel
            {
                CostType = null
            };
            var rule = new FDCostTypeMA();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDCostTypeMAPassesValidCostTypes()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "12345678901234567890"
            };
            var rule = new FDCostTypeMA();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}