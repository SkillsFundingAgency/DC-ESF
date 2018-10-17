using System.Collections.Generic;
using ESFA.DC.ILR1819.DataStore.EF;

namespace ESFA.DC.ESF.ReportingService.Tests.Builders
{
    public class FM70PeriodosedValuesBuilder
    {
        public static List<ESF_LearningDeliveryDeliverable_PeriodisedValues> BuildModel()
        {
            return new List<ESF_LearningDeliveryDeliverable_PeriodisedValues>
            {
                new ESF_LearningDeliveryDeliverable_PeriodisedValues
                {
                    UKPRN = 10001639,
                    LearnRefNumber = "0DOB43",
                    AimSeqNumber = 1,
                    AttributeName = "AchievementEarnings",
                    DeliverableCode = "ST01",
                    Period_1 = 1,
                    Period_2 = 1,
                    Period_3 = 1,
                    Period_4 = 1,
                    Period_5 = 1,
                    Period_6 = 1,
                    Period_7 = 1,
                    Period_8 = 1,
                    Period_9 = 1,
                    Period_10 = 1,
                    Period_11 = 1,
                    Period_12 = 1,
                }
            };
        }
    }
}
