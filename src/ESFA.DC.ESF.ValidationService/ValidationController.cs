using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService
{
    public class ValidationController : IValidationController
    {
        private readonly IList<IValidatorCommand> _validatorCommands;

        public bool RejectFile { get; private set; }

        public IList<ValidationErrorModel> Errors { get; }

        public ValidationController(IList<IValidatorCommand> validatorCommands)
        {
            _validatorCommands = validatorCommands.OrderBy(c => c.Priority).ToList();
        }

        public async Task ValidateData(SupplementaryDataModel model)
        {
            foreach (var command in _validatorCommands)
            {
                await command.Execute(model);

                if (command.IsValid)
                {
                    continue;
                }

                foreach (var error in command.Errors)
                {
                    Errors.Add(error);
                }

                if (!command.RejectFile)
                {
                    continue;
                }

                RejectFile = true;
                break;
            }
        }
    }    
}
