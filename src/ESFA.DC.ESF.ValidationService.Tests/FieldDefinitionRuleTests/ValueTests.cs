using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.FieldDefinitionRuleTests
{
    public class ValueTests
    {
        [Fact]
        public void FDValueALCatchesTooLongValues()
        {
            var model = new SupplementaryDataModel
            {
                Value = 1234567.123M
            };
            var rule = new FDValueAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDValueALPassesValidValues()
        {
            var model = new SupplementaryDataModel
            {
                Value = 123456.12M
            };
            var rule = new FDValueAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}