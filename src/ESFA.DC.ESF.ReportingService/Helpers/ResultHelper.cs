using System.Collections.Generic;
using System.Linq;
using Autofac.Features.AttributeFilters;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Models;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Jobs.Model;

namespace ESFA.DC.ESF.Helpers
{
    public class ResultHelper : IResultHelper
    {
        private readonly IKeyValuePersistenceService _storage;

        public ResultHelper([KeyFilter(PersistenceStorageKeys.Blob)] IKeyValuePersistenceService storage)
        {
            _storage = storage;
        }

        public FileValidationResult GenerateFrontEndValidationReport(
            IList<SupplementaryDataModel> data, 
            IList<ValidationErrorModel> validationErrors)
        {
            var errors = validationErrors.Where(x => !x.IsWarning).ToList();
            var warnings = validationErrors.Where(x => x.IsWarning).ToList();

            return new FileValidationResult
            {
                TotalLearners = data.GroupBy(w => w.ULN).Count(),
                TotalErrors = errors.Count,
                TotalWarnings = warnings.Count,
                TotalWarningLearners = warnings.GroupBy(w => w.ULN).Count(),
                TotalErrorLearners = errors.GroupBy(e => e.ULN).Count(),
                ErrorMessage = validationErrors.FirstOrDefault(x => string.IsNullOrEmpty(x.ConRefNumber))?.ErrorMessage
            };
        }
    }
}
