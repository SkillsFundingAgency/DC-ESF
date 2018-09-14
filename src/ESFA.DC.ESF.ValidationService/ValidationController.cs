using System.Collections.Generic;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService
{
    public class ValidationController : IValidationController
    {
        private readonly IList<IValidatorCommand> _validatorCommands;

        public ValidationController(IList<IValidatorCommand> validatorCommands)
        {
            _validatorCommands = validatorCommands;
        }

        public async Task ValidateData(ESFModel model)
        {
            foreach (var command in _validatorCommands)
            {
                await command.Execute(model);
            }
        }
    }    
}
