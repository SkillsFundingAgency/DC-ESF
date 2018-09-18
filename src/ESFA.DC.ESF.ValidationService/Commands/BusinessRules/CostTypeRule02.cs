using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class CostTypeRule02 : IBusinessRuleValidator
    {
        private readonly List<string> _AC01InvalidCostTypes = new List<string>
        {
            Constants.CostTypeStaffPT, Constants.CostTypeStaffFT, "Other Costs", "Staff Expenses", String.Empty
        };

        private readonly List<string> _SDCodes = new List<string>
        {
            "SD01", "SD02", "SD03", "SD04", "SD05", "SD06", "SD07", "SD08", "SD09", "SD10"
        };

        readonly IList<string> _deliveryCodes = new List<string>
        {
            "ST01", "FS01", "PG01", "PG02", "PG03", "PG04", "PG05", "PG06", "SU01", "SU02", "SU03", "SU04", "SU05", "SU11", "SU12", "SU13", "SU14", "SU15", "SU21", "SU22", "SU23", "SU24"
        };

        public string ErrorMessage => "The CostType is not valid for the DeliverableCode. Please refer to the ESF Supplementary Data supporting documentation for further information.";

        public bool IsValid { get; private set; }

        public Task Execute(ESFModel model)
        {
            var errorCondition =
                model.DeliverableCode == "AC01" && _AC01InvalidCostTypes.Contains(model.CostType)
                ||
                model.DeliverableCode == "CG01" && model.CostType != "Grant"
                ||
                model.DeliverableCode == "CG02" && model.CostType != "Grant Management"
                ||
                _SDCodes.Contains(model.DeliverableCode) && model.CostType != "Unit Cost"
                ||
                _deliveryCodes.Contains(model.DeliverableCode) &&
                (model.CostType != "Unit Cost" || model.CostType != "Unit Cost Deduction")
                ||
                (model.DeliverableCode == "NR01" || model.DeliverableCode == "RQ01") &&
                model.CostType != "Funding Adjustment";

            IsValid = !errorCondition;
            return Task.CompletedTask;
        }
    }
}
