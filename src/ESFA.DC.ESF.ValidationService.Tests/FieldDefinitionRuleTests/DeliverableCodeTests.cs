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
            var model = new SupplementaryDataModel
            {
                DeliverableCode = "12345678901"
            };
            var rule = new FDDeliverableCodeAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDDeliverableCodeALPassesValidDeliverableCodes()
        {
            var model = new SupplementaryDataModel
            {
                DeliverableCode = "1234567890"
            };
            var rule = new FDDeliverableCodeAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDDeliverableCodeMACatchesEmptyDeliverableCodes()
        {
            var model = new SupplementaryDataModel
            {
                DeliverableCode = null
            };
            var rule = new FDDeliverableCodeMA();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDDeliverableCodeMAPassesValidDeliverableCodes()
        {
            var model = new SupplementaryDataModel
            {
                DeliverableCode = "1234567890"
            };
            var rule = new FDDeliverableCodeMA();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}