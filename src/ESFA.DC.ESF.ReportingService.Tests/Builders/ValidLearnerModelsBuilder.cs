using System;
using System.Collections.Generic;
using ESFA.DC.ILR1819.DataStore.EF.Valid;

namespace ESFA.DC.ESF.ReportingService.Tests.Builders
{
    public class ValidLearnerModelsBuilder
    {
        public static List<Learner> BuildLearners()
        {
            return new List<Learner>
            {
                new Learner
                {
                    UKPRN = 10005752,
                    LearnRefNumber = "9900000004",
                    ULN = 9900000004,
                    Ethnicity = 31,
                    Sex = "F",
                    CampId = null,
                    Email = "myemail@myemail.com",
                    GivenNames = "Mary Jane",
                    FamilyName = "Sméth",
                    PMUKPRN = null,
                    DateOfBirth = new DateTime(2000, 9, 1),
                    Postcode = "ZZ99 9ZZ"
                }
            };
        }

        public static List<LearningDelivery> BuildLearningDeliveries()
        {
            return new List<LearningDelivery>
            {
                new LearningDelivery
                {
                    UKPRN = 10005752,
                    LearnRefNumber = "9900000004",
                    LearnAimRef = "ZESF0001",
                    AimType = 4,
                    AimSeqNumber = 1,
                    LearnStartDate = new DateTime(2016, 10, 19),
                    LearnPlanEndDate = new DateTime(2016, 10, 19),
                    FundModel = 70,
                    PartnerUKPRN = 10048217,
                    DelLocPostCode = "BH12 4AR",
                    ConRefNumber = "ESF-2108",
                    CompStatus = 2,
                    LearnActEndDate = new DateTime(2016, 10, 19),
                    Outcome = 1,
                    SWSupAimId = "00F40788-2618-46AC-A1BB-C9786CD66BDA"
                }
            };
        }

        public static List<LearningDeliveryFAM> BuildLearningDeliveryFams()
        {
            return new List<LearningDeliveryFAM>
            {
                new LearningDeliveryFAM
                {
                    UKPRN = 10005752,
                    LearnRefNumber = "ZESF0001",
                    AimSeqNumber = 1,
                    LearnDelFAMType = "RES",
                    LearnDelFAMCode = "1"
                },
            };
        }
    }
}
