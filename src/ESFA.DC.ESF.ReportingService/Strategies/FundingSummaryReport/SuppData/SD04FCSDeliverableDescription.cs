﻿using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class SD04FCSDeliverableDescription : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        protected override string DeliverableCode => "SD04";
    }
}