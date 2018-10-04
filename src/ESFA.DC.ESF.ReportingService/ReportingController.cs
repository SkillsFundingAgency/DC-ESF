using System.Collections.Generic;
using System.Threading;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.Helpers;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ReportingService
{
    public class ReportingController : IReportingController
    {
        private readonly IResultHelper _resultHelper;

        public ReportingController(IResultHelper resultHelper)
        {
            _resultHelper = resultHelper;
        }

        public void FileLevelError(
            IList<SupplementaryDataModel> models,
            IList<ValidationErrorModel> errors,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var report = _resultHelper.GenerateFrontEndValidationReport(models, errors);

        }
    }
}
