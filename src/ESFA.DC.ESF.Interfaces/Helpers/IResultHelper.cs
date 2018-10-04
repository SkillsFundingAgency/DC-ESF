using System.Collections.Generic;
using ESFA.DC.ESF.Models;
using ESFA.DC.Jobs.Model;

namespace ESFA.DC.ESF.Interfaces.Helpers
{
    public interface IResultHelper
    {
        FileValidationResult GenerateFrontEndValidationReport(
            IList<SupplementaryDataModel> data,
            IList<ValidationErrorModel> validationErrors);
    }
}