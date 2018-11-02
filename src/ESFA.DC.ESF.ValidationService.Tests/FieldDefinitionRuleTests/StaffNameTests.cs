using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests.FieldDefinitionRuleTests
{
    public class StaffNameTests
    {
        [Fact]
        public void FDStaffNameALCatchesTooLongStaffNames()
        {
            var model = new SupplementaryDataModel
            {
                StaffName = new string('1', 101)
            };
            var rule = new FDStaffNameAL();

            rule.Execute(model);

            Assert.False(rule.IsValid);
        }

        [Fact]
        public void FDStaffNameALPassesValidStaffNames()
        {
            var model = new SupplementaryDataModel
            {
                StaffName = new string('1', 100)
            };
            var rule = new FDStaffNameAL();

            rule.Execute(model);

            Assert.True(rule.IsValid);
        }
    }
}