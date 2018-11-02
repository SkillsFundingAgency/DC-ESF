using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.FieldDefinitionRuleTests
{
    public class ConRefNumberTests
    {
        [Fact]
        public void FDConRefNumberALCatchesTooLongConRefNumbers()
        {
            var model = new SupplementaryDataModel
            {
                ConRefNumber = "123456789012345678901"
            };
            var rule = new FDConRefNumberAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDConRefNumberALPassesValidConRefNumbers()
        {
            var model = new SupplementaryDataModel
            {
                ConRefNumber = "12345678901234567890"
            };
            var rule = new FDConRefNumberAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDConRefNumberMACatchesEmptyConRefNumbers()
        {
            var model = new SupplementaryDataModel
            {
                ConRefNumber = null
            };
            var rule = new FDConRefNumberMA();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDConRefNumberMAPassesValidConRefNumbers()
        {
            var model = new SupplementaryDataModel
            {
                ConRefNumber = "12345678901234567890"
            };
            var rule = new FDConRefNumberMA();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}