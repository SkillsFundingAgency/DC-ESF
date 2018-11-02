using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.FieldDefinitionRuleTests
{
    public class ReferenceTests
    {
        [Fact]
        public void FDProviderSpecifiedReferenceALCatchesTooLongProviderSpecifiedReferences()
        {
            var model = new SupplementaryDataModel
            {
                ProviderSpecifiedReference = new string('1', 201)
            };
            var rule = new FDProviderSpecifiedReferenceAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDProviderSpecifiedReferenceALPassesValidProviderSpecifiedReferences()
        {
            var model = new SupplementaryDataModel
            {
                ProviderSpecifiedReference = new string('1', 200)
            };
            var rule = new FDProviderSpecifiedReferenceAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDReferenceALCatchesTooLongReferences()
        {
            var model = new SupplementaryDataModel
            {
                Reference = new string('1', 101)
            };
            var rule = new FDReferenceAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDReferenceALPassesValidReferences()
        {
            var model = new SupplementaryDataModel
            {
                Reference = new string('1', 100)
            };
            var rule = new FDReferenceAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDReferenceMACatchesEmptyReferences()
        {
            var model = new SupplementaryDataModel
            {
                Reference = null
            };
            var rule = new FDReferenceMA();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDReferenceMAPassesValidReferences()
        {
            var model = new SupplementaryDataModel
            {
                Reference = new string('1', 100)
            };
            var rule = new FDReferenceMA();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDReferenceTypeALCatchesTooLongReferenceTypes()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = new string('1', 21)
            };
            var rule = new FDReferenceTypeAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDReferenceTypeALPassesValidReferenceTypes()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = new string('1', 20)
            };
            var rule = new FDReferenceTypeAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }

        [Fact]
        public void FDReferenceTypeMACatchesEmptyReferenceTypes()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = null
            };
            var rule = new FDReferenceTypeMA();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDReferenceTypeMAPassesValidReferenceTypes()
        {
            var model = new SupplementaryDataModel
            {
                ReferenceType = new string('1', 20)
            };
            var rule = new FDReferenceTypeMA();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}