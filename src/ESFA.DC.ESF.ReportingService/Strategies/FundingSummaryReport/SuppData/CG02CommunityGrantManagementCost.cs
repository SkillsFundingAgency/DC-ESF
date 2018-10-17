﻿using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class CG02CommunityGrantManagementCost : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        protected override string DeliverableCode => "CG02";
    }
}