using System;
using System.Threading;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CalendarYearCalendarMonthRule03 : IBusinessRuleValidator
    {
        private readonly IReferenceDataCache _referenceDataCache;
        private readonly IFcsCodeMappingHelper _mappingHelper;

        public CalendarYearCalendarMonthRule03(
            IReferenceDataCache referenceDataCache,
            IFcsCodeMappingHelper mappingHelper)
        {
            _referenceDataCache = referenceDataCache;
            _mappingHelper = mappingHelper;
        }

        public string ErrorMessage => "The CalendarMonth and CalendarYear is after the contract allocation end date.";

        public string ErrorName => "CalendarYearCalendarMonth_03";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            var year = model.CalendarYear ?? 0;
            var month = model.CalendarMonth ?? 0;

            if (year == 0 || month == 0)
            {
                return false;
            }

            var startDateMonth = new DateTime(year, month, 1);

            var fcsDeliverableCode = _mappingHelper.GetFcsDeliverableCode(model, CancellationToken.None);
            var contractAllocation = _referenceDataCache.GetContractAllocation(model.ConRefNumber, fcsDeliverableCode, CancellationToken.None);

            return contractAllocation != null && contractAllocation.EndDate > startDateMonth;
        }
    }
}
