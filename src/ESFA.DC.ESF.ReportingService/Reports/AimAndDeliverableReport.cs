using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.ESF.Interfaces.DataAccessLayer;
using ESFA.DC.ESF.Interfaces.Reports;
using ESFA.DC.ESF.Interfaces.Services;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.Models.Reports;
using ESFA.DC.ESF.ReportingService.Comparers;
using ESFA.DC.ESF.ReportingService.Mappers;
using ESFA.DC.IO.Interfaces;

namespace ESFA.DC.ESF.ReportingService.Reports
{
    public class AimAndDeliverableReport : AbstractReportBuilder, IModelReport
    {
        private const string FundingStreamPeriodCode = "ESF1420";

        private readonly IKeyValuePersistenceService _storage;

        private readonly IReferenceDataCache _referenceDataCache;

        private readonly IValidRepository _validRepository;

        private readonly IFM70Repository _fm70Repository;

        private readonly AimAndDeliverableComparer _comparer;

        private readonly string[] _reportMonths =
        {
            "Aug-18",
            "Sep-18",
            "Oct-18",
            "Nov-18",
            "Dec-18",
            "Jan-19",
            "Feb-19",
            "Mar-19",
            "Apr-18",
            "May-19",
            "Jun-19",
            "Jul-19",
        };

        public AimAndDeliverableReport(
            IDateTimeProvider dateTimeProvider,
            IStreamableKeyValuePersistenceService storage,
            IReferenceDataCache referenceDataCache,
            IValidRepository validRepository,
            IFM70Repository fm70Repository,
            IValueProvider valueProvider,
            IAimAndDeliverableComparer comparer)
            : base(dateTimeProvider, valueProvider)
        {
            _storage = storage;
            _referenceDataCache = referenceDataCache;
            _validRepository = validRepository;
            _fm70Repository = fm70Repository;
            _comparer = comparer as AimAndDeliverableComparer;

            ReportFileName = "ESF Aim and Deliverable Report";
        }

        public async Task GenerateReport(
            SupplementaryDataWrapper wrapper,
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

            var ukPrn = Convert.ToInt32(sourceFile.UKPRN);
            string csv = await GetCsv(ukPrn, cancellationToken);
            if (csv != null)
            {
                await _storage.SaveAsync($"{externalFileName}.csv", csv, cancellationToken);
                await WriteZipEntry(archive, $"{fileName}.csv", csv);
            }
        }

        private async Task<string> GetCsv(int ukPrn, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var learners = await _validRepository.GetLearners(ukPrn, cancellationToken);

            var learningDeliveries = await _validRepository.GetLearningDeliveries(ukPrn, cancellationToken);

            if (learners == null || learningDeliveries == null)
            {
                return null;
            }

            var learnerDeliveryFams = await _validRepository.GetLearningDeliveryFAMs(ukPrn, cancellationToken);

            var outcomes = await _validRepository.GetDPOutcomes(ukPrn, cancellationToken);

            var learnMonitorings =
                await _validRepository.GetProviderSpecLearnerMonitorings(ukPrn, cancellationToken);

            var deliveryMonitorings =
                await _validRepository.GetProviderSpecDeliveryMonitorings(ukPrn, cancellationToken);

            var fm70LearningDeliveries = await _fm70Repository.GetLearningDeliveries(ukPrn, cancellationToken);

            var fm70Deliverables =
                await _fm70Repository.GetLearningDeliveryDeliverables(ukPrn, cancellationToken);

            var fm70DeliverablePeriods =
                await _fm70Repository.GetLearningDeliveryDeliverablePeriods(ukPrn, cancellationToken);

            var fm70Outcomes = await _fm70Repository.GetOutcomes(ukPrn, cancellationToken);

            var learnAimRefs = learningDeliveries.Select(ld => ld.LearnAimRef).ToList();
            var deliverableCodes = fm70Deliverables?.Select(d => d.DeliverableCode).ToList();

            var fcsCodeMappings =
                _referenceDataCache.GetContractDeliverableCodeMapping(deliverableCodes, cancellationToken);
            var larsDeliveries = _referenceDataCache.GetLarsLearningDelivery(learnAimRefs, cancellationToken);

            var reportData = new List<AimAndDeliverableModel>();

            foreach (var learner in learners)
            {
                var deliveries = learningDeliveries.Where(ld => ld.LearnRefNumber == learner.LearnRefNumber).ToList();
                foreach (var delivery in deliveries)
                {
                    var fm70Delivery = fm70LearningDeliveries?.SingleOrDefault(d =>
                        d.LearnRefNumber == learner.LearnRefNumber
                        && d.AimSeqNumber == delivery.AimSeqNumber);
                    var fm70DeliveryDeliverables = fm70Deliverables?.Where(d =>
                        d.LearnRefNumber == learner.LearnRefNumber
                        && d.AimSeqNumber == delivery.AimSeqNumber);

                    var deliveryFam = learnerDeliveryFams?.SingleOrDefault(l =>
                        l.LearnRefNumber == learner.LearnRefNumber
                        && l.AimSeqNumber == delivery.AimSeqNumber
                        && l.LearnDelFAMType == "RES");

                    var outcomeType = fm70Delivery?.EligibleProgressionOutcomeType;
                    var outcomeCode = fm70Delivery?.EligibleProgressionOutcomeCode;
                    var outcomeStartDate = fm70Delivery?.EligibleProgressionOutomeStartDate;
                    var outcome = outcomes?.SingleOrDefault(o => o.OutType == outcomeType
                                                                && o.OutCode == outcomeCode
                                                                && o.OutStartDate == outcomeStartDate);

                    var fm70Outcome = fm70Outcomes?.SingleOrDefault(o => o.OutType == outcomeType
                                                                        && o.OutCode == outcomeCode
                                                                        && o.OutStartDate == outcomeStartDate);

                    var learnerMonitorings =
                        learnMonitorings?.Where(m => m.LearnRefNumber == learner.LearnRefNumber).ToList();
                    var learnerDeliveryMonitorings = deliveryMonitorings
                        ?.Where(m => m.LearnRefNumber == learner.LearnRefNumber).ToList();
                    var larsDelivery = larsDeliveries?.SingleOrDefault(l => l.LearnAimRef == delivery.LearnAimRef);

                    foreach (var fm70Deliverable in fm70DeliveryDeliverables)
                    {
                        var deliverableCode = fm70Deliverable?.DeliverableCode;
                        var fcsMapping = fcsCodeMappings?.SingleOrDefault(f =>
                            f.ExternalDeliverableCode == deliverableCode
                            && f.FundingStreamPeriodCode == FundingStreamPeriodCode);

                        var fm70Periods = fm70DeliverablePeriods?.Where(p =>
                            p.LearnRefNumber == learner.LearnRefNumber
                            && p.AimSeqNumber == delivery.AimSeqNumber
                            && p.DeliverableCode == fm70Deliverable?.DeliverableCode);

                        foreach (var period in fm70Periods)
                        {
                            var total = period?.StartEarnings ?? 0 + period?.AchievementEarnings ?? 0
                                        + period?.AdditionalProgCostEarnings ?? 0 + period?.ProgressionEarnings ?? 0;
                            if (period.ReportingVolume == 0 && period.DeliverableVolume == 0 && total == 0)
                            {
                                continue;
                            }

                            var reportModel = new AimAndDeliverableModel
                            {
                                LearnRefNumber = learner.LearnRefNumber,
                                ULN = learner.ULN,
                                AimSeqNumber = delivery.AimSeqNumber,
                                ConRefNumber = delivery.ConRefNumber,
                                DeliverableCode = deliverableCode,
                                DeliverableName = fcsMapping?.DeliverableName,
                                LearnAimRef = delivery.LearnAimRef,
                                DeliverableUnitCost = fm70Deliverable?.DeliverableUnitCost,
                                ApplicWeightFundRate = fm70Delivery?.ApplicWeightFundRate,
                                AimValue = fm70Delivery?.AimValue,
                                LearnAimRefTitle = larsDelivery?.LearnAimRefTitle,
                                PMUKPRN = learner.PMUKPRN,
                                CampId = learner.CampId,
                                ProvSpecLearnMonA = learnerMonitorings
                                    ?.SingleOrDefault(m => m.ProvSpecLearnMonOccur == "A")?.ProvSpecLearnMon,
                                ProvSpecLearnMonB = learnerMonitorings
                                    ?.SingleOrDefault(m => m.ProvSpecLearnMonOccur == "B")?.ProvSpecLearnMon,
                                SWSupAimId = delivery.SWSupAimId,
                                NotionalNVQLevelv2 = larsDelivery?.NotionalNVQLevelv2,
                                SectorSubjectAreaTier2 = larsDelivery?.SectorSubjectAreaTier2,
                                AdjustedAreaCostFactor = fm70Delivery?.AdjustedAreaCostFactor,
                                AdjustedPremiumFactor = fm70Delivery?.AdjustedPremiumFactor,
                                LearnStartDate = delivery.LearnStartDate,
                                LDESFEngagementStartDate = fm70Delivery?.LDESFEngagementStartDate,
                                LearnPlanEndDate = delivery.LearnPlanEndDate,
                                CompStatus = delivery.CompStatus,
                                LearnActEndDate = delivery.LearnActEndDate,
                                Outcome = delivery.Outcome,
                                AddHours = delivery.AddHours,
                                LearnDelFAMCode = deliveryFam?.LearnDelFAMCode,
                                ProvSpecDelMonA = learnerDeliveryMonitorings
                                    ?.SingleOrDefault(m => m.ProvSpecDelMonOccur == "A")?.ProvSpecDelMon,
                                ProvSpecDelMonB = learnerDeliveryMonitorings
                                    ?.SingleOrDefault(m => m.ProvSpecDelMonOccur == "B")?.ProvSpecDelMon,
                                ProvSpecDelMonC = learnerDeliveryMonitorings
                                    ?.SingleOrDefault(m => m.ProvSpecDelMonOccur == "C")?.ProvSpecDelMon,
                                ProvSpecDelMonD = learnerDeliveryMonitorings
                                    ?.SingleOrDefault(m => m.ProvSpecDelMonOccur == "D")?.ProvSpecDelMon,
                                PartnerUKPRN = delivery.PartnerUKPRN,
                                DelLocPostCode = delivery.DelLocPostCode,
                                LatestPossibleStartDate = fm70Delivery?.LatestPossibleStartDate,
                                EligibleProgressionOutomeStartDate = fm70Delivery?.EligibleProgressionOutomeStartDate,
                                EligibleOutcomeEndDate = outcome?.OutEndDate,
                                EligibleOutcomeCollectionDate = outcome?.OutCollDate,
                                EligibleOutcomeDateProgressionLength = fm70Outcome?.OutcomeDateForProgression,
                                EligibleProgressionOutcomeType = fm70Delivery?.EligibleProgressionOutcomeType,
                                EligibleProgressionOutcomeCode = fm70Delivery?.EligibleProgressionOutcomeCode,
                                Period = _reportMonths[period?.Period - 1 ?? 0],
                                DeliverableVolume = period?.DeliverableVolume,
                                StartEarnings = period?.StartEarnings,
                                AchievementEarnings = period?.AchievementEarnings,
                                AdditionalProgCostEarnings = period?.AdditionalProgCostEarnings,
                                ProgressionEarnings = period?.ProgressionEarnings,
                                TotalEarnings = total
                            };

                            reportData.Add(reportModel);
                        }
                    }
                }
            }

            reportData.Sort(_comparer);

            using (var ms = new MemoryStream())
            {
                var utF8Encoding = new UTF8Encoding(false, true);
                using (var textWriter = new StreamWriter(ms, utF8Encoding))
                {
                    using (var csvWriter = new CsvWriter(textWriter))
                    {
                        WriteCsvRecords<AimAndDeliverableMapper, AimAndDeliverableModel>(csvWriter, reportData);

                        csvWriter.Flush();
                        textWriter.Flush();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
        }
    }
}
