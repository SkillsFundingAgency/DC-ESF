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
            var model = new SupplementaryDataLooseModel
            {
                Value = "1234567.123"
            };
            var rule = new FDValueAL();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDValueALPassesValidValues()
        {
            var model = new SupplementaryDataLooseModel
            {
                Value = "123456.12"
            };
            var rule = new FDValueAL();

            Assert.True(rule.Execute(model));
        }
    }
}