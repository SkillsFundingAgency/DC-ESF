using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Reports.Strategies;
using ESFA.DC.ESF.Interfaces.Repositories;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.Ilr
{
    public class ST01LearnerAssessmentAndPlanILR : BaseILRDataStrategy, IILRDataStrategy
    {
        public ST01LearnerAssessmentAndPlanILR(IIlrEsfRepository repository)
            :base(repository)
        {
            
        }

        private new readonly string DeliverableCode = "ST01";

        private new readonly List<string> AttributeNames = new List<string>
        {
            "StartEarnings",
            "AchievementEarnings",
            "AdditionalProgCostEarnings",
            "ProgressionEarnings"
        };
    }
}