using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ReferenceTypeRule01 : IBusinessRuleValidator
    {
        private List<string> _validReferenceTypes = new List<string>
        {
            "Employee ID",
            "Invoice",
            "Grant Recipient",
            "LearnRefNumber",
            "Company Name",
            "Other",
            "Authorised Claims",
            "Audit Adjustment"
        };

        public string ErrorMessage => "The ReferenceType is not valid.";

        public string ErrorName => "ReferenceType_01";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            return _validReferenceTypes.Contains(model.ReferenceType?.Trim());
        }
    }
}
