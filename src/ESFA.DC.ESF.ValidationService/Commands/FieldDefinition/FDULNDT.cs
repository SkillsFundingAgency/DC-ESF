﻿using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDULNDT : IFieldDefinitionValidator
    {
        private const long Min = 1000000000;

        private const long Max = 9999999999;

        public string ErrorName => "FD_ULN_DT";

        public bool IsWarning => false;

        public string ErrorMessage => $"ULN must be an integer between {Min} and {Max}. Please adjust the value and resubmit the file.";

        public bool Execute(SupplementaryDataLooseModel model)
        {
            return string.IsNullOrEmpty(model.ULN) ||
                   (long.TryParse(model.ULN, out var uln) && uln >= Min && uln <= Max);
        }
    }
}
