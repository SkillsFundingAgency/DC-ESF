using System.Text.RegularExpressions;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class StaffNameRule02 : IBusinessRuleValidator
    {
        private const string Pattern = @"^[A-Z,a-z,0-9,\s\.,;:~!”@#\$&’()\/\+-<=>\[\]\{}\^£€]*$";

        public string ErrorMessage => "The StaffName contains invalid characters.";

        public string ErrorName => "StaffName_02";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            return string.IsNullOrEmpty(model.StaffName) || Regex.IsMatch(model.StaffName, Pattern);
        }
    }
}
