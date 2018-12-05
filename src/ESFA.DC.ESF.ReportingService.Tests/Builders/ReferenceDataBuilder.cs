using System.Collections.Generic;
using ESFA.DC.Data.LARS.Model;
using ESFA.DC.ReferenceData.FCS.Model;

namespace ESFA.DC.ESF.ReportingService.Tests.Builders
{
    public class ReferenceDataBuilder
    {
        public static List<ContractDeliverableCodeMapping> BuildContractDeliverableCodeMapping()
        {
            return new List<ContractDeliverableCodeMapping>
            {
                new ContractDeliverableCodeMapping
                {
                    Claimable = false,
                    DeliverableName = "T01 Learner Assessment and Plan",
                    FCSDeliverableCode = "1",
                    FundingStreamPeriodCode = "ESF1420",
                    ExternalDeliverableCode = "ST01"
                },
                new ContractDeliverableCodeMapping
                {
                    Claimable = false,
                    DeliverableName = "Q01 Regulated Learning",
                    FCSDeliverableCode = "2",
                    FundingStreamPeriodCode = "ESF1420",
                    ExternalDeliverableCode = "RQ01"
                }
            };
        }

        public static List<LARS_LearningDelivery> BuildLarsLearningDeliveries()
        {
            return new List<LARS_LearningDelivery>
            {
                new LARS_LearningDelivery
                {
                    LearnAimRef = "ZESF0001",
                    NotionalNVQLevelv2 = "X",
                    SectorSubjectAreaTier2 = -2.00M,
                    LearnAimRefTitle = "ESF learner start and assessment"
                }
            };
        }
    }
}
