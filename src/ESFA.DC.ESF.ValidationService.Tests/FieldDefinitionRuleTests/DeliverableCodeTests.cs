using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.FieldDefinitionRuleTests
{
    public class DeliverableCodeTests
    {
        [Fact]
        public void FDDeliverableCodeALCatchesTooLongDeliverableCodes()
        {
            var model = new SupplementaryDataLooseModel
            {
                DeliverableCode = "12345678901"
            };
            var rule = new FDDeliverableCodeAL();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDDeliverableCodeALPassesValidDeliverableCodes()
        {
            var model = new SupplementaryDataLooseModel
            {
                DeliverableCode = "1234567890"
            };
            var rule = new FDDeliverableCodeAL();

            Assert.True(rule.Execute(model));
        }

        [Fact]
        public void FDDeliverableCodeMACatchesEmptyDeliverableCodes()
        {
            var model = new SupplementaryDataLooseModel
            {
                DeliverableCode = null
            };
            var rule = new FDDeliverableCodeMA();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDDeliverableCodeMAPassesValidDeliverableCodes()
        {
            var model = new SupplementaryDataLooseModel
            {
                DeliverableCode = "1234567890"
            };
            var rule = new FDDeliverableCodeMA();

            Assert.True(rule.Execute(model));
        }
    }
}