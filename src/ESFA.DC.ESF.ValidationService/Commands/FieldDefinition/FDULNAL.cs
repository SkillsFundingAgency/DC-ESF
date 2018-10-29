﻿using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands.FieldDefinition
{
    public class FDULNAL : IFieldDefinitionValidator
    {
        private const int FieldLength = 10;

        public string ErrorName => "FD_ULN_AL";

        public bool IsWarning => false;

        public string ErrorMessage => $"The ULN must not exceed {FieldLength} characters in length. Please adjust the value and resubmit the file.";

        public bool IsValid { get; private set; }

        public Task Execute(SupplementaryDataModel model)
        {
            IsValid = string.IsNullOrEmpty(model.ULN.ToString()) || model.ULN.ToString().Length <= FieldLength;

            return Task.CompletedTask;
        }
    }
}
