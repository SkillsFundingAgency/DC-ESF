using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.Reports;
using ESFA.DC.ESF.Models;
using ESFA.DC.IO.Interfaces;
using ESFA.DC.Logging.Interfaces;

namespace ESFA.DC.ESF.ReportingService
{
    public class ReportingController : IReportingController
    {
        private readonly ILogger _logger;
        private readonly IStreamableKeyValuePersistenceService _streamableKeyValuePersistenceService;

        private readonly IValidationResultReport _resultReport;

        private readonly IList<IValidationReport> _validationReports;
        private readonly IList<IModelReport> _esfReports;

        public ReportingController(
            IStreamableKeyValuePersistenceService streamableKeyValuePersistenceService,
            ILogger logger,
            IValidationResultReport resultReport,
            IList<IValidationReport> validationReports,
            IList<IModelReport> esfReports)
        {
            _streamableKeyValuePersistenceService = streamableKeyValuePersistenceService;
            _logger = logger;
            _resultReport = resultReport;
            _validationReports = validationReports;
            _esfReports = esfReports;
        }

        public async Task FileLevelErrorReport(
            SupplementaryDataWrapper wrapper,
            SourceFileModel sourceFile,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await _resultReport.GenerateReport(sourceFile, wrapper, null, cancellationToken);
        }

        public async Task ProduceReports(
            SupplementaryDataWrapper wrapper,
            SourceFileModel sourceFile,
            CancellationToken cancellationToken)
        {
            _logger.LogInfo("ESF Reporting service called");

            //if wrapper supplementary data is empty then "Report Only", go get data for ukPrn

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    foreach (var validationReport in _validationReports)
                    {
                        await validationReport.GenerateReport(sourceFile, wrapper, archive, cancellationToken);
                    }

                    foreach (var report in _esfReports)
                    {
                        await report.GenerateReport(wrapper, sourceFile, archive, cancellationToken);
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    await _streamableKeyValuePersistenceService.SaveAsync($"{sourceFile.UKPRN}_{sourceFile.JobId}_Reports.zip", memoryStream, cancellationToken);
                }
            }
        }
    }
}
