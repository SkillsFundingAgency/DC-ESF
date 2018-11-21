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
            var model = new SupplementaryDataLooseModel
            {
                StaffName = new string('1', 101)
            };
            var rule = new FDStaffNameAL();

            Assert.False(rule.Execute(model));
        }

        [Fact]
        public void FDStaffNameALPassesValidStaffNames()
        {
            var model = new SupplementaryDataLooseModel
            {
                StaffName = new string('1', 100)
            };
            var rule = new FDStaffNameAL();

            Assert.True(rule.Execute(model));
        }
    }
}