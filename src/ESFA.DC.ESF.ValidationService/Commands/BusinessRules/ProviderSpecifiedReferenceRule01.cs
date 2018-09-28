﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.BusinessRules
{
    public class ProviderSpecifiedReferenceRule01 : IBusinessRuleValidator
    {
        private const string Pattern = @"^[A-Z,a-z,0-9,\s\.,;:~!”@#\$&’()\/\+-<=>\[\]\{}\^£€]*$";

        public string ErrorMessage => "The ProviderSpecifiedReference contains invalid characters.";

        public string ErrorName => "ProviderSpecifiedReference_01";

        public bool IsWarning => false;

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = !(!string.IsNullOrEmpty(model.ProviderSpecifiedReference) &&
                        !Regex.IsMatch(model.ProviderSpecifiedReference, Pattern));
            return Task.CompletedTask;
        }
    }
}
