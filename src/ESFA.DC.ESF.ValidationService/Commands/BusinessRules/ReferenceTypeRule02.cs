using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ReferenceTypeRule02 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The ReferenceType is not valid for the selected CostType. Please refer to the ESF Supplementary Data supporting documentation for further information.";

        public string ErrorName => "ReferenceType_02";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            var employeeIdCostTypes = new List<string>
                {"Staff Part Time", "Staff Full Time", "Staff Expenses", "Apportioned Cost"};

            var invoiceCostTypes = new List<string> { "Other Costs", "Apportioned Cost" };

            var grantRecipientCostTypes = new List<string> { "Grant", "Grant Management" };

            var unitReferenceTypes = new List<string> {"LearnRefNumber", "Company Name", "Other"};

            var unitCostTypes = new List<string> {"Unit Cost", "Unit Cost Deduction"};

            var adjustmentReferenceTypes = new List<string> { "Authorised Claims", "Audit Adjustment" };

            var errorCondition =
                (model.ReferenceType == "Employee ID" && !employeeIdCostTypes.Contains(model.CostType))
                ||
                (model.ReferenceType == "Invoice" && !invoiceCostTypes.Contains(model.CostType))
                ||
                (model.ReferenceType == "Grant Recipient" && !grantRecipientCostTypes.Contains(model.CostType))
                ||
                (unitReferenceTypes.Contains(model.ReferenceType) && !unitCostTypes.Contains(model.CostType))
                ||
                (adjustmentReferenceTypes.Contains(model.ReferenceType) && model.CostType != "Funding Adjustment");

            IsValid = !errorCondition;

            return Task.CompletedTask;
        }
    }
}
