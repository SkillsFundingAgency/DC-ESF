using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Validation;

namespace ESFA.DC.ESF.ValidationService
{
    public class ValidationController
    {
        private readonly IList<IValidatorCommand> _validatorCommands;

        public ValidationController(IList<IValidatorCommand> validatorCommands)
        {
            _validatorCommands = validatorCommands;
        }

        public void ValidateData()
        {
            foreach (var command in _validatorCommands)
            {
                command.Execute();
            }
        }
    }    
}
