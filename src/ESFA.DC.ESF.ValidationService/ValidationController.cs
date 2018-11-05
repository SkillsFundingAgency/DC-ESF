using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Controllers;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService
{
    public class ValidationController : IValidationController
    {
        private readonly IList<IValidatorCommand> _validatorCommands;
        private readonly IPopulationService _populationService;

        public ValidationController(
            IList<IValidatorCommand> validatorCommands,
            IPopulationService populationService)
        {
            _validatorCommands = validatorCommands.OrderBy(c => c.Priority).ToList();
            _populationService = populationService;
        }

        public bool RejectFile { get; private set; }

        public IList<ValidationErrorModel> Errors { get; private set; }

        public async Task ValidateData(
            IList<SupplementaryDataModel> allModels,
            SupplementaryDataModel model,
            CancellationToken cancellationToken)
        {
            Errors = new List<ValidationErrorModel>();

            var allUlns = allModels.Select(m => m.ULN).ToList();
            _populationService.PrePopulateUlnCache(allUlns, cancellationToken);

            foreach (var command in _validatorCommands)
            {
                if (command is ICrossRecordCommand)
                {
                    ((ICrossRecordCommand)command).AllRecords = allModels;
                }

                if (command.Execute(model))
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
