using System.Collections.Generic;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService
{
    public class ValidationController
    {
        private readonly IList<IValidatorCommand> _validatorCommands;

        public ValidationController(IList<IValidatorCommand> validatorCommands)
        {
            _validatorCommands = validatorCommands;
        }

        public void ValidateData(ESFModel model)
        {
            foreach (var command in _validatorCommands)
            {
                command.Execute(model);
            }
        }
    }    
}
