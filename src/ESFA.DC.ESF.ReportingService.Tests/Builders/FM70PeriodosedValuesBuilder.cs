using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ReportingService.Tests.Builders
{
    public class FM70PeriodosedValuesBuilder
    {
        public static async Task<IList<FM70PeriodisedValuesModel>> BuildModel()
        {
            return new List<FM70PeriodisedValuesModel>
            {
                new FM70PeriodisedValuesModel
                {
                    UKPRN = 10001639,
                    LearnRefNumber = "0DOB43",
                    AimSeqNumber = 1,
                    AttributeName = "AchievementEarnings",
                    DeliverableCode = "ST01",
                    Period1 = 1,
                    Period2 = 1,
                    Period3 = 1,
                    Period4 = 1,
                    Period5 = 1,
                    Period6 = 1,
                    Period7 = 1,
                    Period8 = 1,
                    Period9 = 1,
                    Period10 = 1,
                    Period11 = 1,
                    Period12 = 1
                }
            };
        }
    }
}