using System;
using System.Collections.Generic;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.ReportingService.Tests.Builders
{
    public class FM70ModelsBuilder
    {
        public static List<ESF_LearningDelivery> BuildLearningDeliveries()
        {
            return new List<ESF_LearningDelivery>
            {
                new ESF_LearningDelivery
                {
                    UKPRN = 10005752,
                    LearnRefNumber = "9900000004",
                    AimSeqNumber = 1,
                    Achieved = true,
                    AddProgCostElig = false,
                    AdjustedAreaCostFactor = 1.0M,
                    AdjustedPremiumFactor = 1.0M,
                    AdjustedStartDate = new DateTime(2017, 10, 14, 0, 0, 0),
                    AimClassification = "Assessment Aim",
                    AimValue = 400.0M,
                    ApplicWeightFundRate = 0.0M,
                    EligibleProgressionOutcomeCode = null,
                    EligibleProgressionOutcomeType = null,
                    EligibleProgressionOutomeStartDate = null,
                    FundStart = true,
                    LARSWeightedRate = 0.0M,
                    LatestPossibleStartDate = null,
                    LDESFEngagementStartDate = new DateTime(2017, 10, 14, 0, 0, 0),
                    PotentiallyEligibleForProgression = true,
                    ProgressionEndDate = null,
                    Restart = false,
                    WeightedRateFromESOL = 0.0M
                }
            };
        }

        public static List<ESF_LearningDeliveryDeliverable> BuildLearningDeliveryDeliverables()
        {
            return new List<ESF_LearningDeliveryDeliverable>
            {
                new ESF_LearningDeliveryDeliverable
                {
                    UKPRN = 10005752,
                    LearnRefNumber = "9900000004",
                    AimSeqNumber = 1,
                    DeliverableCode = "ST01",
                    DeliverableUnitCost = 400.00M
                },
                new ESF_LearningDeliveryDeliverable
                {
                    UKPRN = 10005752,
                    LearnRefNumber = "9900000004",
                    AimSeqNumber = 1,
                    DeliverableCode = "RQ01",
                    DeliverableUnitCost = 50.00M
                }
            };
        }

        public static List<ESF_LearningDeliveryDeliverable_Period> BuildDeliveryDeliverablePeriods()
        {
            return new List<ESF_LearningDeliveryDeliverable_Period>
            {
                new ESF_LearningDeliveryDeliverable_Period
                {
                    UKPRN = 10005752,
                    LearnRefNumber = "9900000004",
                    AimSeqNumber = 1,
                    DeliverableCode = "ST01",
                    Period = 1,
                    DeliverableVolume = 1,
                    ReportingVolume = 1,
                    ProgressionEarnings = 10M,
                    StartEarnings = 0M,
                    AchievementEarnings = 0M,
                    AdditionalProgCostEarnings = 0M
                }
            };
        }
    }
}
