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
        private readonly IReferenceDataCache _referenceDataCache;
        private readonly IFcsCodeMappingHelper _mappingHelper;

        public CalendarYearCalendarMonthRule02(
            IReferenceDataCache referenceDataCache,
            IFcsCodeMappingHelper mappingHelper)
        {
            _referenceDataCache = referenceDataCache;
            _mappingHelper = mappingHelper;
        }

        public string ErrorMessage => "The CalendarMonth and CalendarYear is prior to the contract allocation start date.";

        public string ErrorName => "CalendarYearCalendarMonth_02";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            var year = model.CalendarYear ?? 0;
            var month = model.CalendarMonth ?? 0;

            if (year == 0 || month == 0 || month < 1 || month > 12)
            {
                return false;
            }

            var startDateMonthEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var fcsDeliverableCode = _mappingHelper.GetFcsDeliverableCode(model, CancellationToken.None);
            var contractAllocation = _referenceDataCache.GetContractAllocation(model.ConRefNumber, fcsDeliverableCode, CancellationToken.None);

            return contractAllocation != null && contractAllocation.StartDate < startDateMonthEnd;
        }
    }
}
