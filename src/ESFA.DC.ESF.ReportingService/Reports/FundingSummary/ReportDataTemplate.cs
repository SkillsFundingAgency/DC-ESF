using System.Collections.Generic;
using ESFA.DC.ESF.Models.Reports.FundingSummaryReport;

namespace ESFA.DC.ESF.ReportingService.Reports.FundingSummary
{
    public static class ReportDataTemplate
    {
        public static readonly List<FundingReportRow> FundingModelRowDefinitions = new List<FundingReportRow>
        {
            new FundingReportRow { RowType = RowType.Spacer },
            new FundingReportRow { RowType = RowType.MainTitle, Title = "European Social Fund 2014-2020" },
            new FundingReportRow { RowType = RowType.Spacer },

            new FundingReportRow { RowType = RowType.Title, Title = "Learner Assessment and Plan" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "ST01", RowType = RowType.Data, Title = "ILR ST01 Learner Assessment and Plan (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "ST01", RowType = RowType.Data, Title = "SUPPDATA ST01 Learner Assessment and Plan Adjustments (£)" },
            new FundingReportRow { DeliverableCode = "ST01", RowType = RowType.Total, Title = "Total Learner Assessment and Plan (£)" },
            new FundingReportRow { RowType = RowType.Spacer },

            new FundingReportRow { RowType = RowType.Title, Title = "Regulated Learning" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "RQ01", RowType = RowType.Data, Title = "ILR RQ01 Regulated Learning - Start Funding (£)", AttributeNames = new List<string> { "StartEarnings" } },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "RQ01", RowType = RowType.Data, Title = "ILR RQ01 Regulated Learning - Achievement Funding (£)", AttributeNames = new List<string> { "AchievementEarnings" } },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "RQ01", RowType = RowType.Total, Title = "ILR Total RQ01 Regulated Learning (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "RQ01", RowType = RowType.Data, Title = "SUPPDATA RQ01 Regulated Learning Audit Adjustments (£)", ReferenceType = "Audit Adjustment" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "RQ01", RowType = RowType.Data, Title = "SUPPDATA RQ01 Regulated Learning Authorised Claims (£)", ReferenceType = "Authorised Claims" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "RQ01", RowType = RowType.Total, Title = "SUPPDATA Total RQ01 Regulated Learning (£)" },
            new FundingReportRow { DeliverableCode = "RQ01", RowType = RowType.Total, Title = "Total Regulated Learning (£)" },
            new FundingReportRow { RowType = RowType.Spacer },

            new FundingReportRow { RowType = RowType.Title, Title = "Non Regulated Activity" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "NR01", RowType = RowType.Data, Title = "ILR NR01 Non Regulated Activity - Start Funding (£)", AttributeNames = new List<string> { "StartEarnings" } },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "NR01", RowType = RowType.Data, Title = "ILR NR01 Non Regulated Activity - Achievement Funding (£)", AttributeNames = new List<string> { "AchievementEarnings" } },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "NR01", RowType = RowType.Total, Title = "ILR Total NR01 Non Regulated Activity (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "NR01", RowType = RowType.Data, Title = "SUPPDATA NR01 Non Regulated Activity Audit Adjustments (£)", ReferenceType = "Audit Adjustment" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "NR01", RowType = RowType.Data, Title = "SUPPDATA NR01 Non Regulated Activity Authorised Claims (£)", ReferenceType = "Authorised Claims" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "NR01", RowType = RowType.Total, Title = "SUPPDATA Total NR01 Non Regulated Activity (£)" },
            new FundingReportRow { DeliverableCode = "NR01", RowType = RowType.Total, Title = "Total Non Regulated Activity (£)" },
            new FundingReportRow { RowType = RowType.Spacer },

            new FundingReportRow { RowType = RowType.Title, Title = "Additional Programme Cost" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "FS01", RowType = RowType.Data, Title = "ILR FS01 Additional Programme Cost (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "FS01", RowType = RowType.Data, Title = "SUPPDATA FS01 Additional Programme Cost Adjustments (£)" },
            new FundingReportRow { DeliverableCode = "FS01", RowType = RowType.Total, Title = "Total Additional Programme Cost (£)" },
            new FundingReportRow { RowType = RowType.Spacer },

            new FundingReportRow { RowType = RowType.Title, Title = "Progression and Sustained Progression" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG01", RowType = RowType.Data, Title = "ILR PG01 Progression Paid Employment (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU01", RowType = RowType.Data, Title = "ILR SU01 Sustained Paid Employment 3 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU11", RowType = RowType.Data, Title = "ILR SU11 Sustained Paid Employment 6 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU21", RowType = RowType.Data, Title = "ILR SU21 Sustained Paid Employment 12 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG01, SU01, SU11, SU21", RowType = RowType.Total, Title = "ILR Total Paid Employment Progression (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG01", RowType = RowType.Data, Title = "SUPPDATA PG01 Progression Paid Employment Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU01", RowType = RowType.Data, Title = "SUPPDATA SU01 Sustained Paid Employment 3 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU11", RowType = RowType.Data, Title = "SUPPDATA SU11 Sustained Paid Employment 6 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU21", RowType = RowType.Data, Title = "SUPPDATA SU21 Sustained Paid Employment 12 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG01, SU01, SU11, SU21", RowType = RowType.Total, Title = "SUPPDATA Total Paid Employment Progression Adjustments (£)" },
            new FundingReportRow { DeliverableCode = "PG01, SU01, SU11, SU21", RowType = RowType.Total, Title = "Total Paid Employment Progression (£)" },

            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG02", RowType = RowType.Data, Title = "ILR PG02 Progression Unpaid Employment (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU02", RowType = RowType.Data, Title = "ILR SU02 Sustained Unpaid Employment 3 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU12", RowType = RowType.Data, Title = "ILR SU12 Sustained Unpaid Employment 6 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU22", RowType = RowType.Data, Title = "ILR SU22 Sustained Unpaid Employment 12 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG02, SU02, SU12, SU22", RowType = RowType.Total, Title = "ILR Total Unpaid Employment Progression (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG02", RowType = RowType.Data, Title = "SUPPDATA PG02 Progression Unpaid Employment Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU02", RowType = RowType.Data, Title = "SUPPDATA SU02 Sustained Unpaid Employment 3 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU12", RowType = RowType.Data, Title = "SUPPDATA SU12 Sustained Unpaid Employment 6 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU22", RowType = RowType.Data, Title = "SUPPDATA SU22 Sustained Unpaid Employment 12 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG02, SU02, SU12, SU22", RowType = RowType.Total, Title = "SUPPDATA Total Unpaid Employment Progression Adjustments (£)" },
            new FundingReportRow { DeliverableCode = "PG02, SU02, SU12, SU22", RowType = RowType.Total, Title = "Total Unpaid Employment Progression (£)" },

            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG03", RowType = RowType.Data, Title = "ILR PG03 Progression Education (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU03", RowType = RowType.Data, Title = "ILR SU03 Sustained Education 3 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU13", RowType = RowType.Data, Title = "ILR SU13 Sustained Education 6 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU23", RowType = RowType.Data, Title = "ILR SU23 Sustained Education 12 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG03, SU03, SU13, SU23", RowType = RowType.Total, Title = "ILR Total Education Progression (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG03", RowType = RowType.Data, Title = "SUPPDATA PG03 Progression Education Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU03", RowType = RowType.Data, Title = "SUPPDATA SU03 Sustained Education 3 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU13", RowType = RowType.Data, Title = "SUPPDATA SU13 Sustained Education 6 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU23", RowType = RowType.Data, Title = "SUPPDATA SU23 Sustained Education 12 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG03, SU03, SU13, SU23", RowType = RowType.Total, Title = "SUPPDATA Total Education Progression Adjustments (£)" },
            new FundingReportRow { DeliverableCode = "PG03, SU03, SU13, SU23", RowType = RowType.Total, Title = "Total Education Progression (£)" },

            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG04", RowType = RowType.Data, Title = "ILR PG04 Progression Apprenticeship (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU04", RowType = RowType.Data, Title = "ILR SU04 Sustained Apprenticeship 3 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU14", RowType = RowType.Data, Title = "ILR SU14 Sustained Apprenticeship 6 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU24", RowType = RowType.Data, Title = "ILR SU24 Sustained Apprenticeship 12 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG04, SU04, SU14, SU24", RowType = RowType.Total, Title = "ILR Total Apprenticeship Progression (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG04", RowType = RowType.Data, Title = "SUPPDATA PG04 Progression Apprenticeship Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU04", RowType = RowType.Data, Title = "SUPPDATA SU04 Sustained Apprenticeship 3 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU14", RowType = RowType.Data, Title = "SUPPDATA SU14 Sustained Apprenticeship 6 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU24", RowType = RowType.Data, Title = "SUPPDATA SU24 Sustained Apprenticeship 12 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG04, SU04, SU14, SU24", RowType = RowType.Total, Title = "SUPPDATA Total Apprenticeship Progression Adjustments (£)" },
            new FundingReportRow { DeliverableCode = "PG04, SU04, SU14, SU24", RowType = RowType.Total, Title = "Total Apprenticeship Progression (£)" },

            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG05", RowType = RowType.Data, Title = "ILR PG05 Progression Traineeship (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU05", RowType = RowType.Data, Title = "ILR SU05 Sustained Traineeship 3 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "SU15", RowType = RowType.Data, Title = "ILR SU15 Sustained Traineeship 6 months (£)" },
            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG05, SU05, SU15, SU25", RowType = RowType.Total, Title = "ILR Total Education Progression (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG05", RowType = RowType.Data, Title = "SUPPDATA PG05 Progression Traineeship Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU05", RowType = RowType.Data, Title = "SUPPDATA SU05 Sustained Traineeship 3 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SU15", RowType = RowType.Data, Title = "SUPPDATA SU15 Sustained Traineeship 6 months Adjustments (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG05, SU05, SU15, SU25", RowType = RowType.Total, Title = "SUPPDATA Total Traineeship Progression Adjustments (£)" },
            new FundingReportRow { DeliverableCode = "PG05, SU05, SU15, SU25", RowType = RowType.Total, Title = "Total Traineeship Progression (£)" },

            new FundingReportRow { CodeBase = "ILR", DeliverableCode = "PG06", RowType = RowType.Data, Title = "ILR PG06 Progression Job Search (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "PG06", RowType = RowType.Data, Title = "SUPPDATA PG06 Progression Job Search Adjustments (£)" },
            new FundingReportRow { DeliverableCode = "PG06", RowType = RowType.Total, Title = "Total Job Search Progression (£)" },
            new FundingReportRow
            {
                DeliverableCode = "PG01, PG02, PG03, PG04, PG05, SU01, SU02, SU03, SU04, SU05, SU11, SU12, SU13, SU14, SU15, SU21, SU22, SU23, SU24, SU25",
                RowType = RowType.Total,
                Title = "Total Progression and Sustained Progression (£)"
            },
            new FundingReportRow { RowType = RowType.Spacer },

            new FundingReportRow { RowType = RowType.Title, Title = "Actual Costs" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "AC01", RowType = RowType.Data, Title = "SUPPDATA AC01 Actual Costs (£)" },
            new FundingReportRow { DeliverableCode = "AC01", RowType = RowType.Total, Title = "Total Actual Costs (£)" },
            new FundingReportRow { RowType = RowType.Spacer },

            new FundingReportRow { RowType = RowType.Title, Title = "Community Grant" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "CG01", RowType = RowType.Data, Title = "SUPPDATA CG01 Community Grant Payment (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "CG02", RowType = RowType.Data, Title = "SUPPDATA CG02 Community Grant Management Cost (£)" },
            new FundingReportRow { DeliverableCode = "CG01, CG02", RowType = RowType.Total, Title = "Total Community Grant (£)" },
            new FundingReportRow { RowType = RowType.Spacer },

            new FundingReportRow { RowType = RowType.Title, Title = "Specification Defined" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SD01", RowType = RowType.Data, Title = "SUPPDATA SD01 <a description of the deliverable from FCS> (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SD02", RowType = RowType.Data, Title = "SUPPDATA SD02 <a description of the deliverable from FCS> (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SD03", RowType = RowType.Data, Title = "SUPPDATA SD03 <a description of the deliverable from FCS> (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SD04", RowType = RowType.Data, Title = "SUPPDATA SD04 <a description of the deliverable from FCS> (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SD05", RowType = RowType.Data, Title = "SUPPDATA SD05 <a description of the deliverable from FCS> (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SD06", RowType = RowType.Data, Title = "SUPPDATA SD06 <a description of the deliverable from FCS> (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SD07", RowType = RowType.Data, Title = "SUPPDATA SD07 <a description of the deliverable from FCS> (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SD08", RowType = RowType.Data, Title = "SUPPDATA SD08 <a description of the deliverable from FCS> (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SD09", RowType = RowType.Data, Title = "SUPPDATA SD09 <a description of the deliverable from FCS> (£)" },
            new FundingReportRow { CodeBase = "ESF", DeliverableCode = "SD10", RowType = RowType.Data, Title = "SUPPDATA SD10 <a description of the deliverable from FCS> (£)" },
            new FundingReportRow { DeliverableCode = "SD01, SD02, SD03, SD04, SD05, SD06, SD07, SD08, SD09, SD10", RowType = RowType.Total, Title = "Total Specification Defined (£)" },
            new FundingReportRow { RowType = RowType.Spacer },

            new FundingReportRow { RowType = RowType.Total, Title = "<ESF-1> Total (£)" },
            new FundingReportRow { RowType = RowType.Cumulative, Title = "<ESF-1> Cumulative (£)" },
            new FundingReportRow { RowType = RowType.Spacer }
        };
    }
}
