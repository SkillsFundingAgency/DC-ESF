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

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDCostTypeALPassesValidCostTypes()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "12345678901234567890"
            };
            var rule = new FDCostTypeAL();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDCostTypeMACatchesEmptyCostTypes()
        {
            var model = new SupplementaryDataModel
            {
                CostType = null
            };
            var rule = new FDCostTypeMA();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDCostTypeMAPassesValidCostTypes()
        {
            var model = new SupplementaryDataModel
            {
                CostType = "12345678901234567890"
            };
            var rule = new FDCostTypeMA();

            Assert.True(rule.Execute(model));
        }
    }
}