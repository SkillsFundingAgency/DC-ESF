using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;

namespace ESFA.DC.ESF.ValidationService.Commands
{
    public class FieldDefinitionCommand : IValidatorCommand
    {
        private readonly IList<IFieldDefinitionValidator> _validators;

        public bool IsValid { get; private set; }

        public Dictionary<string, List<string>> Errors { get; }

        public FieldDefinitionCommand(IList<IFieldDefinitionValidator> validators)
        {
            _validators = validators;

            Errors = new Dictionary<string, List<string>>();
        }

        public async Task Execute(SupplementaryDataModel model)
        {
            IsValid = true;
            foreach (var validator in _validators)
            {
                await validator.Execute(model);
            }

            var failed = _validators.Where(v => !v.IsValid).ToList();
            if (failed.Any())
            {
                IsValid = false;
                var errors = new List<string>();
                foreach (var validator in failed)
                {
                    errors.Add(validator.ErrorMessage);
                }
                Errors.Add(model.ConRefNumber, errors);
            }
        }
    }
}
