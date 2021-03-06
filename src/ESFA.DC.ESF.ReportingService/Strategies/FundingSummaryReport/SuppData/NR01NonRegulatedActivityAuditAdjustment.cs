﻿using ESFA.DC.ESF.Interfaces.Reports.Strategies;

namespace ESFA.DC.ESF.ReportingService.Strategies.FundingSummaryReport.SuppData
{
    public class NR01NonRegulatedActivityAuditAdjustment : BaseSupplementaryDataStrategy, ISupplementaryDataStrategy
    {
        protected override string DeliverableCode => "NR01";

        protected override string ReferenceType => "Audit Adjustment";
    }
}