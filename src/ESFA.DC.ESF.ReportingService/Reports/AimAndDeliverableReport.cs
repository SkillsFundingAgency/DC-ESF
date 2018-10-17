using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Interfaces.Reports;
using ESFA.DC.ESF.Interfaces.Reports.Services;
using ESFA.DC.ESF.Models;
using ESFA.DC.IO.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports
{
    public class AimAndDeliverableReport : AbstractReportBuilder, IModelReport
    {
        private readonly IKeyValuePersistenceService _storage;

        private readonly IReferenceDataService _referenceDataService;

        public AimAndDeliverableReport(IDateTimeProvider dateTimeProvider,
            [KeyFilter(PersistenceStorageKeys.Blob)] IKeyValuePersistenceService storage,
            IReferenceDataService referenceDataService
            )
            : base(dateTimeProvider)
        {
            _storage = storage;
            _referenceDataService = referenceDataService;

            ReportFileName = "ESF Aim and Deliverable Report";
        }

        public async Task GenerateReport(
            IList<SupplementaryDataModel> data, 
            SourceFileModel sourceFile,
            ZipArchive archive, 
            CancellationToken cancellationToken)
        {
            var externalFileName = GetExternalFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);
            var fileName = GetFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            string csv = await GetCsv(cancellationToken);
            await _storage.SaveAsync($"{externalFileName}.csv", csv, cancellationToken);
            await WriteZipEntry(archive, $"{fileName}.csv", csv);
        }

        private async Task<string> GetCsv(CancellationToken cancellationToken)
        {
            //learning delivery funding model = 70

            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            // _referenceDataService.


            return string.Empty;
        }
    }
}
