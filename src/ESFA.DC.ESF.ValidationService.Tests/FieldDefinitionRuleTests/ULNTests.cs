using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.FieldDefinitionRuleTests
{
    public class ULNTests
    {
        [Fact]
        public void FDULNALCatchesTooLongULNs()
        {
            var model = new SupplementaryDataModel
            {
                ULN = 12345678901
            };
            var rule = new FDULNAL();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDULNALPassesValidULNs()
        {
            var model = new SupplementaryDataModel
            {
                ULN = 1234567890
            };
            var rule = new FDULNAL();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDULNDTCatchesInvalidULNs()
        {
            var model = new SupplementaryDataModel
            {
                ULN = 0
            };
            var rule = new FDULNDT();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDULNDTPassesValidULNs()
        {
            var model = new SupplementaryDataModel
            {
                ULN = 1234567890
            };
            var rule = new FDULNDT();

            Assert.True(rule.Execute(model));
        }
    }
}