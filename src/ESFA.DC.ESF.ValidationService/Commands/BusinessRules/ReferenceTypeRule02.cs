using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ReferenceTypeRule02 : IBusinessRuleValidator
    {
        public string ErrorMessage => "The ReferenceType is not valid for the selected CostType. Please refer to the ESF Supplementary Data supporting documentation for further information.";

        public string ErrorName => "ReferenceType_02";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            var referenceType = model.ReferenceType.Trim();
            var costType = model.CostType.Trim();

            var employeeIdCostTypes = new List<string>
                { "Staff Part Time", "Staff Full Time", "Staff Expenses", "Apportioned Cost" };

            var invoiceCostTypes = new List<string> { "Other Costs", "Apportioned Cost" };

            var grantRecipientCostTypes = new List<string> { "Grant", "Grant Management" };

            var unitReferenceTypes = new List<string> { "LearnRefNumber", "Company Name", "Other" };

            var unitCostTypes = new List<string> { "Unit Cost", "Unit Cost Deduction" };

            var adjustmentReferenceTypes = new List<string> { "Authorised Claims", "Audit Adjustment" };

            var errorCondition =
                (referenceType == "Employee ID" && !employeeIdCostTypes.Contains(costType))
                ||
                (referenceType == "Invoice" && !invoiceCostTypes.Contains(costType))
                ||
                (referenceType == "Grant Recipient" && !grantRecipientCostTypes.Contains(costType))
                ||
                (unitReferenceTypes.Contains(referenceType) && !unitCostTypes.Contains(costType))
                ||
                (adjustmentReferenceTypes.Contains(referenceType) && costType != "Funding Adjustment");

            return !errorCondition;
        }
    }
}
