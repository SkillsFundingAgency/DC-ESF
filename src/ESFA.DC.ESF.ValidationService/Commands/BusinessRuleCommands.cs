using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands
{
    public class BusinessRuleCommands : IValidatorCommand
    {
        private readonly IList<IBusinessRuleValidator> _validators;

        public bool IsValid { get; private set; }

        public bool RejectFile { get; private set; }    

        public BusinessRuleCommands(IList<IBusinessRuleValidator> validators)
        {
            _validators = validators;
        }

        public void Execute(ESFModel model)
        {
            foreach (var validator in _validators)
            {
                validator.Execute(model);
            }
        }
    }
}