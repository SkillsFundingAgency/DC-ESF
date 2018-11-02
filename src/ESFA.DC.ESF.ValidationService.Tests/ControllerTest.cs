using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Data.ULN.Model;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Commands;
using ESFA.DC.ESF.ValidationService.Commands.BusinessRules;
using ESFA.DC.ESF.ValidationService.Commands.CrossRecord;
using ESFA.DC.ESF.ValidationService.Commands.FieldDefinition;
using Moq;
using Xunit;

namespace ESFA.DC.ESF.ValidationService.Tests
{
    public class ControllerTest
    {
        [Fact]
        public async Task TestController()
        {
            Mock<IReferenceDataRepository> repoMock = new Mock<IReferenceDataRepository>();
            repoMock.Setup(m => m.GetUlnLookup(It.IsAny<IList<long>>(), It.IsAny<CancellationToken>())).Returns(new List<UniqueLearnerNumber>());

            var validators = GetValidators(repoMock);
            var controller = new ValidationController(validators);

            await controller.ValidateData(GetSupplementaryDataList(), GetSupplementaryData());

            Assert.True(controller.Errors.Any());
            ULNRule03 ulnRule03 = new ULNRule03();
            Assert.True(controller.Errors[1].ErrorMessage == ulnRule03.ErrorMessage);
        }

        private IList<SupplementaryDataModel> GetSupplementaryDataList()
        {
            return new List<SupplementaryDataModel>
            {
                GetSupplementaryData()
            };
        }

        private SupplementaryDataModel GetSupplementaryData()
        {
            return new SupplementaryDataModel
            {
                ConRefNumber = "ESF - 2270",
                DeliverableCode = "ST01",
                CalendarYear = 2016,
                CalendarMonth = 5,
                CostType = "Unit Cost",
                Reference = "|",
                ReferenceType = "LearnRefNumber",
                ULN = 1000000019,
                ProviderSpecifiedReference = "DelCode 01A"
            };
        }

        private IList<IValidatorCommand> GetValidators(Mock<IReferenceDataRepository> repoMock)
        {
            return new List<IValidatorCommand>
            {
                new BusinessRuleCommands(
                    new List<IBusinessRuleValidator>
                    {
                        new CalendarMonthRule01(),
                        new CalendarYearCalendarMonthRule01(),
                        new CalendarYearCalendarMonthRule02(repoMock.Object),
                        new CalendarYearCalendarMonthRule03(repoMock.Object),
                        new CalendarYearRule01(),
                        new CostTypeRule01(),
                        new CostTypeRule02(),
                        new DeliverableCodeRule01(),
                        new DeliverableCodeRule02(repoMock.Object),
                        new HourlyRateRule01(),
                        new HourlyRateRule02(),
                        new OrgHoursRule01(),
                        new OrgHoursRule02(),
                        new ProjectHoursOrgHoursRule01(),
                        new ProjectHoursRule01(),
                        new ProjectHoursRule02(),
                        new ProviderSpecifiedReferenceRule01(),
                        new ReferenceRule01(),
                        new ReferenceTypeRule02(),
                        new ReferenceTypeRule01(),
                        new StaffNameRule01(),
                        new StaffNameRule02(),
                        new StaffNameRule03(),
                        new TotalHoursWorkedRule01(),
                        new TotalHoursWorkedRule02(),
                        new ULNRule01(),
                        new ULNRule02(repoMock.Object),
                        new ULNRule03(),
                        new ULNRule04(),
                        new ValueRule01(),
                        new ValueRule02(),
                        new ValueRule03()
                    }),
                new CrossRecordCommands(
                    new List<ICrossRecordValidator>
                    {
                        new Duplicate01()
                    }),
                new FieldDefinitionCommand(
                    new List<IFieldDefinitionValidator>
                    {
                        new FDCalendarMonthAL(),
                        new FDCalendarMonthDT(),
                        new FDCalendarMonthMA(),
                        new FDCalendarYearAL(),
                        new FDCalendarYearDT(),
                        new FDCalendarYearMA(),
                        new FDConRefNumberAL(),
                        new FDConRefNumberMA(),
                        new FDCostTypeAL(),
                        new FDCostTypeMA(),
                        new FDDeliverableCodeAL(),
                        new FDDeliverableCodeMA(),
                        new FDHourlyRateAL(),
                        new FDOrgHoursAL(),
                        new FDProjectHoursAL(),
                        new FDProviderSpecifiedReferenceAL(),
                        new FDReferenceAL(),
                        new FDReferenceMA(),
                        new FDReferenceTypeAL(),
                        new FDReferenceTypeMA(),
                        new FDStaffNameAL(),
                        new FDTotalHoursWorkedAL(),
                        new FDULNAL(),
                        new FDULNDT(),
                        new FDValueAL()
                    })
            };
        }
    }
}
