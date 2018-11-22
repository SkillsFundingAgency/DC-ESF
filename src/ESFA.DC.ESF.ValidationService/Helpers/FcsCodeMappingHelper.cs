using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.ValidationService.Helpers
{
    public class FcsCodeMappingHelper : IFcsCodeMappingHelper
    {
        private readonly IReferenceDataCache _cache;
        private readonly ILogger _logger;

        public FcsCodeMappingHelper(
            IReferenceDataCache repository,
            ILogger logger)
        {
            _cache = repository;
            _logger = logger;
        }

        public int GetFcsDeliverableCode(SupplementaryDataModel model, CancellationToken cancellationToken)
        {
            var result = 0;

            var deliverableCode = model.DeliverableCode.Trim();

            var codeMappings = _cache.GetContractDeliverableCodeMapping(new List<string> { deliverableCode }, cancellationToken);

            var fcsDeliverableCodeString = codeMappings
                .Where(cm => cm.ExternalDeliverableCode == deliverableCode)
                .Select(cm => cm.FCSDeliverableCode).FirstOrDefault();
            if (string.IsNullOrEmpty(fcsDeliverableCodeString))
            {
                return result;
            }

            if (int.TryParse(fcsDeliverableCodeString, out var fcsDeliverableCode))
            {
                result = fcsDeliverableCode;
            }
            else
            {
                _logger.LogError($"DeliverableCode not an integer:- {fcsDeliverableCodeString}");
            }

            return result;
        }
    }
}