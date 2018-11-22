using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CostTypeRule02 : IBusinessRuleValidator
    {
        private readonly List<string> _AC01InvalidCostTypes = new List<string>
        {
            Constants.CostTypeStaffPT, Constants.CostTypeStaffFT, "Other Costs", "Staff Expenses", string.Empty
        };

        private readonly List<string> _SDCodes = new List<string>
        {
            "SD01", "SD02", "SD03", "SD04", "SD05", "SD06", "SD07", "SD08", "SD09", "SD10"
        };

        private readonly IList<string> _deliveryCodes = new List<string>
        {
            "ST01", "FS01", "PG01", "PG02", "PG03", "PG04", "PG05", "PG06", "SU01", "SU02", "SU03", "SU04", "SU05", "SU11", "SU12", "SU13", "SU14", "SU15", "SU21", "SU22", "SU23", "SU24"
        };

        public string ErrorMessage => "The CostType is not valid for the DeliverableCode. Please refer to the ESF Supplementary Data supporting documentation for further information.";

        public string ErrorName => "CostType_02";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            var deliverableCode = model.DeliverableCode.Trim();
            var costType = model.CostType.Trim();

            var errorCondition =
                (deliverableCode == "AC01" && _AC01InvalidCostTypes.Contains(costType))
                ||
                (deliverableCode == "CG01" && costType != "Grant")
                ||
                (deliverableCode == "CG02" && costType != "Grant Management")
                ||
                (_SDCodes.Contains(deliverableCode) && costType != "Unit Cost")
                ||
                (_deliveryCodes.Contains(deliverableCode) &&
                (costType != "Unit Cost" && costType != "Unit Cost Deduction"))
                ||
                ((deliverableCode == "NR01" || deliverableCode == "RQ01") &&
                costType != "Funding Adjustment");

            return !errorCondition;
        }
    }
}
