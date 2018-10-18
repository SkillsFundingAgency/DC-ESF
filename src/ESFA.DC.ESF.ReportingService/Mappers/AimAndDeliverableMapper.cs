using CsvHelper.Configuration;
using ESFA.DC.ESF.Interfaces;
using ESFA.DC.ESF.Models.Reports;

namespace ESFA.DC.ESF.ReportingService.Mappers
{
    public sealed class AimAndDeliverableMapper : ClassMap<AimAndDeliverableModel>, IClassMapper
    {
        public AimAndDeliverableMapper()
        {
            int i = 0;
            Map(m => m.LearnRefNumber).Index(i++).Name("Learner reference number");
            Map(m => m.ULN).Index(i++).Name("Unique learner number");
            Map(m => m.AimSeqNumber).Index(i++).Name("Aim sequence number");
            Map(m => m.ConRefNumber).Index(i++).Name("Contract reference number");
            Map(m => m.DeliverableCode).Index(i++).Name("Deliverable code");
            Map(m => m.DeliverableName).Index(i++).Name("Deliverable name");
            Map(m => m.LearnAimRef).Index(i++).Name("Learning aim reference");
            Map(m => m.DeliverableUnitCost).Index(i++).Name("Unit cost(£)");
            Map(m => m.ApplicWeightFundRate).Index(i++).Name("Funding rate - LARS plus ESOL hours(£)");
            Map(m => m.AimValue).Index(i++).Name("Aim value(£)");
            Map(m => m.LearnAimRefTitle).Index(i++).Name("Learning aim title");
            Map(m => m.PMUKPRN).Index(i++).Name("Pre - merger UKPRN");
            Map(m => m.CampId).Index(i++).Name("Campus identifier");
            Map(m => m.ProvSpecLearnMonA).Index(i++).Name("Provider specified learner monitoring(A)");
            Map(m => m.ProvSpecLearnMonB).Index(i++).Name("Provider specified learner monitoring(B)");
            Map(m => m.SWSupAimId).Index(i++).Name("Software supplier aim identifier");
            Map(m => m.NotionalNVQLevelv2).Index(i++).Name("Notional NVQ level");
            Map(m => m.SectorSubjectAreaTier2).Index(i++).Name("Tier 2 sector subject area");
            Map(m => m.AdjustedAreaCostFactor).Index(i++).Name("Area uplift");
            Map(m => m.AdjustedPremiumFactor).Index(i++).Name("Learning rate premium");
            Map(m => m.LearnStartDate).Index(i++).Name("Learning start date");
            Map(m => m.LDESFEngagementStartDate).Index(i++).Name("Learning start date of first assessment");
            Map(m => m.LearnPlanEndDate).Index(i++).Name("Learning planned end date");
            Map(m => m.CompStatus).Index(i++).Name("Completion status");
            Map(m => m.LearnActEndDate).Index(i++).Name("Learning actual end date");
            Map(m => m.Outcome).Index(i++).Name("Outcome");
            Map(m => m.AddHours).Index(i++).Name("Additional delivery hours");
            Map(m => m.LearnDelFAMCode).Index(i++).Name("Learning delivery funding and monitoring type - restart indicator");
            Map(m => m.ProvSpecDelMonA).Index(i++).Name("Provider specified delivery monitoring(A)");
            Map(m => m.ProvSpecDelMonB).Index(i++).Name("Provider specified delivery monitoring(B)");
            Map(m => m.ProvSpecDelMonC).Index(i++).Name("Provider specified delivery monitoring(C)");
            Map(m => m.ProvSpecDelMonD).Index(i++).Name("Provider specified delivery monitoring(D)");
            Map(m => m.PartnerUKPRN).Index(i++).Name("Sub contracted or partnership UKPRN");
            Map(m => m.DelLocPostCode).Index(i++).Name("Delivery location postcode");
            Map(m => m.LatestPossibleStartDate).Index(i++).Name("Latest possible progression start date");
            Map(m => m.EligibleProgressionOutomeStartDate).Index(i++).Name("Eligible outcome start date");
            Map(m => m.EligibleOutcomeEndDate).Index(i++).Name("Eligible outcome end date");
            Map(m => m.EligibleOutcomeCollectionDate).Index(i++).Name("Eligible outcome collection date");
            Map(m => m.EligibleOutcomeDateProgressionLength).Index(i++).Name("Eligible outcome date used for progression length");
            Map(m => m.EligibleProgressionOutcomeType).Index(i++).Name("Eligible outcome type");
            Map(m => m.EligibleProgressionOutcomeCode).Index(i++).Name("Eligible outcome code");
            Map(m => m.Period).Index(i++).Name("Month");
            Map(m => m.DeliverableVolume).Index(i++).Name("Deliverable volume");
            Map(m => m.StartEarnings).Index(i++).Name("Start earnings(£)");
            Map(m => m.AchievementEarnings).Index(i++).Name("Achievement earnings(£)");
            Map(m => m.AdditionalProgCostEarnings).Index(i++).Name("Additional programme cost earnings(£)");
            Map(m => m.ProgressionEarnings).Index(i++).Name("Progression earnings(£)");
            Map(m => m.TotalEarnings).Index(i++).Name("Total earnings(£)");
            Map(m => m.OfficialSensitive).Index(i).Name("OFFICIAL - SENSITIVE");
        }
    }
}