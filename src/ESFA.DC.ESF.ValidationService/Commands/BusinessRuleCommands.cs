using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESFA.DC.ESF.Interfaces.Validation;
using ESFA.DC.ESF.Models;
using ESFA.DC.ESF.ValidationService.Builders;

namespace ESFA.DC.ESF.ValidationService.Commands
{
    public class BusinessRuleCommands : IValidatorCommand
    {
        private readonly IList<IBusinessRuleValidator> _validators;

        public bool IsValid { get; private set; }

        public IList<ValidationErrorModel> Errors { get; private set; }

        public int Priority => 4;

        public bool RejectFile => false;

        public BusinessRuleCommands(IList<IBusinessRuleValidator> validators)
        {
            _validators = validators;
        }

        public async Task Execute(SupplementaryDataModel model)
        {
            Errors = new List<ValidationErrorModel>();
            IsValid = true;
            foreach (var validator in _validators)
            {
                validator.Execute(model);
            }

            var failed = _validators.Where(v => !v.IsValid).ToList();
            if (failed.Any())
            {
                IsValid = false;
                foreach (var validator in failed)
                {
                    Errors.Add(ValidationErrorBuilder.BuildValidationErrorModel(model, validator));
                }
            }
        }
    }
}