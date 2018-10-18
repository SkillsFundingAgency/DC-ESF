using System;

namespace ESFA.DC.ESF.Models.Reports
{
    public class AimAndDeliverableModel
    {
        public string LearnRefNumber { get; set; }

        public long? ULN { get; set; }

        public int? AimSeqNumber { get; set; }

        public string ConRefNumber { get; set; }

        public string DeliverableCode { get; set; }

        public string DeliverableName { get; set; }

        public string LearnAimRef { get; set; }

        public decimal? DeliverableUnitCost { get; set; }

        public decimal? ApplicWeightFundRate { get; set; }

        public decimal? AimValue { get; set; }

        public string LearnAimRefTitle { get; set; }

        public int? PMUKPRN { get; set; }

        public string CampId { get; set; }

        public string ProvSpecLearnMonA { get; set; }

        public string ProvSpecLearnMonB { get; set; }

        public string SWSupAimId { get; set; }

        public string NotionalNVQLevelv2 { get; set; }

        public decimal? SectorSubjectAreaTier2 { get; set; }

        public decimal? AdjustedAreaCostFactor { get; set; }

        public decimal? AdjustedPremiumFactor { get; set; }

        public DateTime? LearnStartDate { get; set; }

        public DateTime? LDESFEngagementStartDate { get; set; }

        public DateTime? LearnPlanEndDate { get; set; }

        public int? CompStatus { get; set; }

        public DateTime? LearnActEndDate { get; set; }

        public int? Outcome { get; set; }

        public int? AddHours { get; set; }

        public string LearnDelFAMCode { get; set; }

        public string ProvSpecDelMonA { get; set; }

        public string ProvSpecDelMonB { get; set; }

        public string ProvSpecDelMonC { get; set; }

        public string ProvSpecDelMonD { get; set; }

        public int? PartnerUKPRN { get; set; }

        public string DelLocPostCode { get; set; }

        public DateTime? LatestPossibleStartDate { get; set; }

        public DateTime? EligibleProgressionOutomeStartDate { get; set; }

        public DateTime? EligibleOutcomeEndDate { get; set; }

        public DateTime? EligibleOutcomeCollectionDate { get; set; }

        public DateTime? EligibleOutcomeDateProgressionLength { get; set; }
        
        public string EligibleProgressionOutcomeType { get; set; }

        public long? EligibleProgressionOutcomeCode { get; set; }

        public string Period { get; set; }

        public long? DeliverableVolume { get; set; }

        public decimal? StartEarnings { get; set; }

        public decimal? AchievementEarnings { get; set; }

        public decimal? AdditionalProgCostEarnings { get; set; }

        public decimal? ProgressionEarnings { get; set; }

        public decimal? TotalEarnings { get; set; }

        public string OfficialSensitive { get; }
    }
}