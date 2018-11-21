using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Data.ULN.Model;
using ESFA.DC.ESF.DataAccessLayer.Mappers;
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
        public void TestController()
        {
            Mock<IReferenceDataCache> cacheMock = new Mock<IReferenceDataCache>();
            cacheMock.Setup(m => m.GetUlnLookup(It.IsAny<IList<long?>>(), It.IsAny<CancellationToken>())).Returns(new List<UniqueLearnerNumber>());

            Mock<IFcsCodeMappingHelper> mapperMock = new Mock<IFcsCodeMappingHelper>();
            mapperMock.Setup(m =>
                m.GetFcsDeliverableCode(It.IsAny<SupplementaryDataModel>(), It.IsAny<CancellationToken>())).Returns(1);

            Mock<IPopulationService> popMock = new Mock<IPopulationService>();
            popMock.Setup(m => m.PrePopulateUlnCache(It.IsAny<IList<long?>>(), It.IsAny<CancellationToken>()));

            SupplementaryDataModelMapper mapper = new SupplementaryDataModelMapper();

            var looseValidation = GetLooseValidators();
            var validators = GetValidators(cacheMock, mapperMock);
            var controller = new ValidationController(looseValidation, validators, popMock.Object, mapper);

            var wrapper = new SupplementaryDataWrapper
            {
                SupplementaryDataLooseModels = GetSupplementaryDataList()
            };

            controller.ValidateData(wrapper, GetEsfSourceFileModel(), CancellationToken.None);

            //Assert.True(controller.Errors.Any());
        }

        private IList<SupplementaryDataLooseModel> GetSupplementaryDataList()
        {
            return new List<SupplementaryDataLooseModel>
            {
                GetSupplementaryData()
            };
        }

        private SourceFileModel GetEsfSourceFileModel()
        {
            return new SourceFileModel
            {
                UKPRN = "10005752",
                JobId = 1,
                ConRefNumber = "ESF-2108",
                FileName = "SUPPDATA-10005752-ESF-2108-20180909-090911.CSV",
                SuppliedDate = DateTime.Now,
                PreparationDate = DateTime.Now.AddDays(-1)
            };
        }

        private SupplementaryDataLooseModel GetSupplementaryData()
        {
            return new SupplementaryDataLooseModel
            {
                ConRefNumber = "ESF - 2270",
                DeliverableCode = "ST01",
                CalendarYear = "2016",
                CalendarMonth = "5",
                CostType = "Unit Cost",
                Reference = "|",
                ReferenceType = "LearnRefNumber",
                ULN = "1000000019",
                ProviderSpecifiedReference = "DelCode 01A"
            };
        }

        private ILooseValidatorCommand GetLooseValidators()
        {
            return new FieldDefinitionCommand(
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
                });
        }

        private IList<IValidatorCommand> GetValidators(Mock<IReferenceDataCache> cacheMock, Mock<IFcsCodeMappingHelper> mapperMock)
        {
            return new List<IValidatorCommand>
            {
                new BusinessRuleCommands(
                    new List<IBusinessRuleValidator>
                    {
                        new CalendarMonthRule01(),
                        new CalendarYearCalendarMonthRule01(),
                        new CalendarYearCalendarMonthRule02(cacheMock.Object, mapperMock.Object),
                        new CalendarYearCalendarMonthRule03(cacheMock.Object, mapperMock.Object),
                        new CalendarYearRule01(),
                        new CostTypeRule01(),
                        new CostTypeRule02(),
                        new DeliverableCodeRule01(),
                        new DeliverableCodeRule02(cacheMock.Object, mapperMock.Object),
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
                        new ULNRule02(cacheMock.Object),
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
                    })
            };
        }
    }
}
