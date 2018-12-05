using System.Collections.Generic;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.BusinessRuleTests
{
    public class DeliverableCodeTests
    {
        [Fact]
        public void DeliverableCodeRule01CatchesInvalidDeliverableCodes()
        {
            var model = new SupplementaryDataModel
            {
                DeliverableCode = "Invalid Code"
            };

            var rule = new DeliverableCodeRule01();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void DeliverableCodeRule01PassesValidDeliverableCodes()
        {
            var validCodes = new List<string>
            {
                "ST01", "AC01", "CG01", "CG02", "FS01", "SD01", "SD02", "SD03", "SD04", "SD05", "SD06", "SD07",
                "SD08", "SD09", "SD10", "NR01", "RQ01", "PG01", "PG02", "PG03", "PG04", "PG05", "PG06", "SU01",
                "SU02", "SU03", "SU04", "SU05", "SU11", "SU12", "SU13", "SU14", "SU15", "SU21", "SU22", "SU23", "SU24"
            };

            foreach (var code in validCodes)
            {
                var model = new SupplementaryDataModel
                {
                    DeliverableCode = code
                };

                var rule = new DeliverableCodeRule01();

                Assert.True(rule.Execute(model));
            }
        }
    }
}
