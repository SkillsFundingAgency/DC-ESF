using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CalendarYearCalendarMonthRule02 : IBusinessRuleValidator
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public CalendarYearCalendarMonthRule02(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public string ErrorMessage => "The CalendarMonth and CalendarYear is prior to the contract allocation start date.";

        public string ErrorName => "CalendarYearCalendarMonth_02";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            //var mappings = _referenceDataRepository.GetContractDeliverableCodeMapping(
            //    new List<string> { model.DeliverableCode },
            //    CancellationToken.None);

            //var year = model.CalendarYear ?? 0;
            //var month = model.CalendarMonth ?? 0;

            //if (year == 0 || month == 0)
            //{
            //    return false;
            //    return Task.CompletedTask;
            //}

            //var startDateMonthEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            //var contractMatches = mappings.Where(m =>
            //    m.ContractDeliverable.ContractAllocation.ContractAllocationNumber == model.ConRefNumber
            //    && m.ContractDeliverable.ContractAllocation.StartDate < startDateMonthEnd).ToList();

            //return contractMatches.Any();

            return true;
        }
    }
}
