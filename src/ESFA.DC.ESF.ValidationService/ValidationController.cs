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

        public ValidationController(IList<IValidatorCommand> validatorCommands)
        {
            _validatorCommands = validatorCommands.OrderBy(c => c.Priority).ToList();
        }

        public bool RejectFile { get; private set; }

        public IList<ValidationErrorModel> Errors { get; private set; }

        public async Task ValidateData(IList<SupplementaryDataModel> allModels, SupplementaryDataModel model)
        {
            Errors = new List<ValidationErrorModel>();

            foreach (var command in _validatorCommands)
            {
                if (command is ICrossRecordCommand)
                {
                    ((ICrossRecordCommand)command).AllRecords = allModels;
                }

                command.Execute(model);

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
