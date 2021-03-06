﻿using System.Text.RegularExpressions;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ReferenceRule01 : IBusinessRuleValidator
    {
        private const string Pattern = @"^[A-Z,a-z,0-9,\s\.,;:~!”@#\$&’()\/\+-<=>\[\]\{}\^£€]*$";

        public string ErrorMessage => "The Reference contains invalid characters.";

        public string ErrorName => "Reference_01";

        public bool IsWarning => false;

        public bool Execute(SupplementaryDataModel model)
        {
            return string.IsNullOrEmpty(model.Reference) || Regex.IsMatch(model.Reference, Pattern);
        }
    }
}
