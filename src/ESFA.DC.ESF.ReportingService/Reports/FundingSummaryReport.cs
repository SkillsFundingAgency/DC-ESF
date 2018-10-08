using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;
using ESFA.DC.ILR.FundingService.FM70.FundingOutput.Model.Output;
using ESFA.DC.IO.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports
{
    public class FundingSummaryReport : AbstractReportBuilder
    {
        private readonly IKeyValuePersistenceService _storage;

        public FundingSummaryReport(IDateTimeProvider dateTimeProvider,
            [KeyFilter(PersistenceStorageKeys.Blob)] IKeyValuePersistenceService storage)
            : base(dateTimeProvider)
        {
            _storage = storage;
        }

        public async Task GenerateReport(
            IList<SupplementaryDataModel> data,
            SourceFileModel sourceFile,
            ZipArchive archive,
            CancellationToken cancellationToken)
        {
            var csv = GetCsv(data);

            var externalFileName = GetExternalFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);
            var fileName = GetFilename(sourceFile.UKPRN, sourceFile.JobId ?? 0, sourceFile.SuppliedDate ?? DateTime.MinValue);

            await _storage.SaveAsync($"{externalFileName}.csv", csv, cancellationToken);
            await WriteZipEntry(archive, $"{fileName}.csv", csv);
        }

        public string GetCsv(
            IList<SupplementaryDataModel> data)
        {
            List<FundingSummaryReportRowModel> reportRows = new List<FundingSummaryReportRowModel>();

            FM70Global global = new FM70Global();
            //global.Learners[0].LearningDeliveries[0].LearningDeliveryDeliverableValues[0].DeliverableCode
              //  .LearningDeliveryDeliverablePeriodisedValues

            foreach (var fundingReportRow in ReportBuilder.FundingModelDeliverables)
            {
                
            }


            return null;
        }
    }
}