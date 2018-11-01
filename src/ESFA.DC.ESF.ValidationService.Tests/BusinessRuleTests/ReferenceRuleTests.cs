using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.BusinessRuleTests
{
    public class ReferenceRuleTests
    {
        [Fact]
        public void ProviderSpecifiedReferenceRule01CatchesRegexViolations()
        {
            var invalidCharacters = "|~?";

            var model = new SupplementaryDataModel
            {
                ProviderSpecifiedReference = invalidCharacters
            };

            var rule = new ProviderSpecifiedReferenceRule01();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void ProviderSpecifiedReferenceRule01PassesValidReferences()
        {
            var model = new SupplementaryDataModel
            {
                ProviderSpecifiedReference = @"Aa0 .,;:~!”@#$&’()/+-<=>[]{}^£€"
            };

            var rule = new ProviderSpecifiedReferenceRule01();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void ReferenceRule01CatchesRegexViolations()
        {
            var invalidCharacters = "|~?";

            var model = new SupplementaryDataModel
            {
                Reference = invalidCharacters
            };

            var rule = new ReferenceRule01();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void ReferenceRule01PassesValidReferences()
        {
            var model = new SupplementaryDataModel
            {
                Reference = @"Aa0 .,;:~!”@#$&’()/+-<=>[]{}^£€"
            };

            var rule = new ReferenceRule01();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void ReferenceTypeRule01CatchesInvalidTypes()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "Not A Valid Type"
            };

            var rule = new ReferenceTypeRule01();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void ReferenceTypeRule01PassesValidTypes()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "LearnRefNumber"
            };

            var rule = new ReferenceTypeRule01();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void ReferenceTypeRule02CatchesInvalidReferenceTypeCostTypeCombinations()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "Other",
                CostType = "Employee ID"
            };

            var rule = new ReferenceTypeRule02();
            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void ReferenceTypeRule02PassesValidReferenceTypeCostTypeCombinations()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = "Apportioned Cost",
                CostType = "Employee ID"
            };

            var rule = new ReferenceTypeRule02();
            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}
